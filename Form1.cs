using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using WindowsInput;
using WindowsInput.Native;
using TimeZoneConverter;
using Microsoft.Win32;
using System.Text.Json;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace AutoSeed
{
    public partial class Form1 : Form
    {
        const string CURRENT_VERSION = "1.0.5";
        private Panel[] serverInfoPanels;
        private System.Windows.Forms.Timer serverInfoUpdateTimer;
        private List<ServerEntry> serversToSeed = new List<ServerEntry>();

        public Form1()
        {
            InitializeComponent();

            // Safe: add the click handler in user code
            this.picInfo.Click += (s, e) => new InfoForm().ShowDialog();
            this.Text = $"AutoSeeder - Version: {CURRENT_VERSION}";
        }

        private static readonly HttpClient httpClient = new HttpClient();
        private const int playerThreshold = 80;
        private CancellationTokenSource cancellationTokenSource;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        private async void btnStart_Click_Wrapper(object sender, EventArgs e)
        {
            await btnStart_Click(sender, e);
        }

        private async Task btnStart_Click(object sender, EventArgs e)
        {
            // Validate that at least one server is selected first
            CollectServersToSeed();

            // Check if any servers were selected
            if (serversToSeed.Count == 0)
            {
                // Show message box asking user to select at least one server
                MessageBox.Show(
                    "Please select at least one server to seed by checking the 'Select to seed' checkbox.",
                    "No Servers Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                UpdateStatus("No servers selected for seeding.", Color.Red);
                return; // Exit early without starting the seeding process
            }

            var appSettings = Settings.Load();
            TimeSpan seedTime = appSettings.WeekdaySeedTime;
            string seedType = appSettings.ServerName;

            btnStart.Enabled = false;
            picSettings.Enabled = false;
            btnStart.Text = "Seeding...";
            btnCancel.Enabled = true;
            cancellationTokenSource = new CancellationTokenSource();

            var token = cancellationTokenSource.Token;

            try
            {
                if (!chkOverride.Checked)
                {
                    TimeZoneInfo targetTimeZone = TZConvert.GetTimeZoneInfo(Settings.Load().TimeZoneId);
                    DateTime now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, targetTimeZone);
                    TimeSpan weekdaySeed = appSettings.WeekdaySeedTime;
                    TimeSpan weekendSeed = appSettings.WeekendSeedTime;

                    bool isWeekend = now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday;
                    TimeSpan selectedTime = isWeekend ? weekendSeed : weekdaySeed;

                    DateTime targetTime = now.Date + selectedTime;

                    if (now > targetTime)
                    {
                        // Schedule for next appropriate day
                        DateTime nextDay = now.AddDays(1);
                        bool nextIsWeekend = nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday;
                        selectedTime = nextIsWeekend ? weekendSeed : weekdaySeed;
                        targetTime = nextDay.Date + selectedTime;
                    }

                    TimeSpan waitTime = targetTime - now;

                    UpdateStatus($"Waiting until {targetTime:t} ({targetTimeZone.Id})...", Color.DarkOrange);
                    await Task.Delay(waitTime, token); // <-- token-aware delay
                }
                else
                {
                    UpdateStatus("Override selected. Starting immediately.", Color.ForestGreen);
                }

                UpdateStatus("Launching Hell Let Loose...", Color.Green);
                await SeedAllServersAsync();


                if (chkShutdown.Checked)
                {
                    UpdateStatus("Threshold met. Shutting down...", Color.Red);
                    ShutdownComputer();
                }
                else
                {
                    UpdateStatus("Seeding complete. Staying online.", Color.DarkGreen);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateStatus("Seeding cancelled by user.", Color.Gray);
            }
            finally
            {
                btnStart.Enabled = true;
                picSettings.Enabled = true;
                btnStart.Text = "Start Seeding";
                btnCancel.Enabled = false;
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
            }
        }

        public static string? GetSteamExecutablePath()
        {
            // 32-bit apps on 64-bit systems will look here
            string regPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam";
            string? steamInstallPath = Registry.GetValue(regPath, "InstallPath", null) as string;

            if (string.IsNullOrEmpty(steamInstallPath))
                return null;

            string steamExePath = Path.Combine(steamInstallPath, "steam.exe");
            return File.Exists(steamExePath) ? steamExePath : null;
        }

        private async Task SeedAllServersAsync()
        {
            // Collect the servers that were selected for seeding
            CollectServersToSeed();

            // Check if any servers were selected
            if (serversToSeed.Count == 0)
            {
                // Show message box asking user to select at least one server
                MessageBox.Show(
                    "Please select at least one server to seed by checking the 'Select to seed' checkbox.",
                    "No Servers Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                UpdateStatus("No servers selected for seeding.", Color.Red);
                return;
            }

            var settings = Settings.Load();

            // Loop through each selected server
            foreach (var server in serversToSeed)
            {
                UpdateStatus($"Seeding server: {server.Name}", Color.LightBlue);

                // Update settings to reflect current server
                settings.ServerName = server.Name;
                settings.Save();
                UpdateCurrentServerLabel();

                await StartGameAndJoin(server);
                UpdateStatus($"{server.Name}: Waiting for player threshold...", Color.Orange);

                // Wait for player threshold (reduced to 60 as per request)
                bool thresholdReached = await WaitForPlayerThresholdAsync(server.ApiUrl, 60);

                if (thresholdReached)
                {
                    UpdateStatus("Player threshold reached. Beginning staggered disconnects...", Color.Yellow);
                    await StaggerDisconnectAsync();

                    KillGame();

                    // Check if this is the last server
                    if (server != serversToSeed.Last())
                    {
                        UpdateStatus("Server complete. Moving to next server...", Color.LightGreen);
                        await Task.Delay(10000); // pause before next server
                    }
                    else
                    {
                        UpdateStatus("All servers seeded. Exiting...", Color.DarkGreen);
                    }
                }
                else
                {
                    UpdateStatus("Failed to reach threshold. Moving to next server.", Color.Red);
                    KillGame();
                }
            }

            // Final status update and shutdown if selected
            UpdateStatus("Seeding process complete.", Color.DarkGreen);
            if (chkShutdown.Checked)
            {
                UpdateStatus("Shutting down computer...", Color.Red);
                ShutdownComputer();
            }
        }

        private async Task StartGameAndJoin(ServerEntry server)
        {
            //var sim = new InputSimulator();
            string steamPath = GetSteamExecutablePath(); // Registry based

            // Step 1: Launch normally
            var psi = new ProcessStartInfo
            {
                FileName = steamPath,
                Arguments = $"-applaunch 686810 -dev +connect {server.Ip}",
                UseShellExecute = false
            };
            Process.Start(psi);

            // Step 2: Wait 80s for loading and intro videos
            await Task.Delay(80000);

            // Step 3: Send ENTER to skip welcome screen
            var sim = new InputSimulator();
            FocusHellLetLooseWindow();
            await Task.Delay(1000);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

        }

        private async Task<bool> WaitForPlayerThresholdAsync(string apiUrl, int threshold)
        {
            int timeoutMinutes = 600;
            DateTime endTime = DateTime.Now.AddMinutes(timeoutMinutes);

            while (DateTime.Now < endTime)
            {
                try
                {
                    string response = await httpClient.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);
                    int currentPlayers = json["result"]?["player_count"]?.Value<int>() ?? 0;

                    if (currentPlayers >= threshold)
                        return true;
                }
                catch
                {
                    // Optional: log or retry logic
                }

                await Task.Delay(10000); // check every 10s
            }

            return false;
        }

        private async Task StaggerDisconnectAsync()
        {
            Random rand = new Random();
            int delay = rand.Next(30000, 60000); // between 30–60s
            await Task.Delay(delay);
        }

        private void FocusHellLetLooseWindow()
        {
            var processes = Process.GetProcessesByName("HLL-Win64-Shipping");
            if (processes.Length > 0)
            {
                IntPtr hWnd = processes[0].MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    ShowWindow(hWnd, SW_RESTORE);      // restore if minimized
                    SetForegroundWindow(hWnd);         // bring to front
                }
                else
                {
                    UpdateStatus("Could not focus Hell Let Loose (no window handle)", Color.Red);
                }
            }
            else
            {
                UpdateStatus("Hell Let Loose process not found", Color.Red);
            }
        }

        private void KillGame()
        {
            try
            {
                foreach (var proc in Process.GetProcessesByName("HLL-Win64-Shipping"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error killing HLL: " + ex.Message);
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblStatus.Refresh(); // ensures it updates immediately
        }

        private void ShutdownComputer()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                UpdateStatus("Cancelling...", Color.Gray);
                ResetAppState();
            }
            else
            {
                UpdateStatus("Nothing to cancel.", Color.Gray);
            }
        }

        private void UpdateScheduleLabel()
        {
            var settings = Settings.Load();
            string weekday = DateTime.Today.Add(settings.WeekdaySeedTime).ToString("h:mm tt");
            string weekend = DateTime.Today.Add(settings.WeekendSeedTime).ToString("h:mm tt");

            string abbreviation = GetTimeZoneAbbreviation(settings.TimeZoneId);

            lblTitle.Text = $"Auto-seeding starts at {weekday} ({abbreviation}) on weekdays and {weekend} ({abbreviation}) on weekends.";
        }

        private void UpdateCurrentServerLabel()
        {
            var settings = Settings.Load();

            // If no server name is set or it's not a GARRY server, default to the first GARRY server
            if (string.IsNullOrEmpty(settings.ServerName) || !settings.ServerName.Contains("GARRY"))
            {
                var garryServer = settings.ServerList.FirstOrDefault(s => s.Name.Contains("GARRY"));
                if (garryServer != null)
                {
                    settings.ServerName = garryServer.Name;
                    settings.Save();
                }
            }

            lblCurrentServer.Text = $"Current server: {settings.ServerName}";
        }

        private void UpdateLogoForMode()
        {
            // Always use Aussie mode
            picLogo.Visible = false;
            picLogo2.Visible = true;
        }

        private void ResetAppState()
        {
            btnStart.Enabled = true;
            btnStart.Text = "Start Seeding";
            btnCancel.Enabled = false;

            UpdateStatus("Ready", Color.DarkGray);

            // Optional: reset checkboxes
            // chkShutdown.Checked = false;
            // chkOverride.Checked = false;

            // Clean up cancellation token
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        private void picSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            var result = settingsForm.ShowDialog();

            if (result == DialogResult.OK) // only refresh if user saved
            {
                RefreshFromSettings();
            }
        }

        public void ApplyThemeFromSettings()
        {
            var settings = Settings.Load();


            this.BackColor = ColorTranslator.FromHtml(settings.MainFormColor);

            string logoPath = Path.Combine(Application.StartupPath, "Aussie_logo.jpg");

            try
            {
                if (File.Exists(logoPath))
                {
                    // Clear both images to ensure reset
                    picLogo.Image?.Dispose();
                    picLogo.Image = null;
                    picLogo2.Image?.Dispose();
                    picLogo2.Image = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // Always use Aussie logo
                    picLogo2.Image = Image.FromFile(logoPath);
                }
                else
                {
                    Console.WriteLine("⚠️ Logo file not found: " + logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update logo: " + ex.Message);
            }
        }

        private void ApplyGradientBackground()
        {
            // Create a LinearGradientBrush
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(255, 74, 93, 80),      // Darker grayish-green
                Color.FromArgb(255, 96, 116, 102),    // Lighter grayish-green
                System.Drawing.Drawing2D.LinearGradientMode.Vertical);

            // Create a bitmap the size of the form
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            // Create a graphics object from the bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fill the bitmap with the gradient
                g.FillRectangle(gradientBrush, this.ClientRectangle);
            }

            // Set the form's background image to the bitmap
            this.BackgroundImage = bmp;

            // Clean up the brush
            gradientBrush.Dispose();
        }


        private string GetTimeZoneAbbreviation(string timeZoneId)
        {
            return timeZoneId switch
            {
                "Eastern Standard Time" => "ET",
                "AUS Eastern Standard Time" => "AEST",
                _ => timeZoneId // fallback to full name if unknown
            };
        }

        public void RefreshFromSettings()
        {
            UpdateScheduleLabel();
            UpdateCurrentServerLabel();
            ApplyThemeFromSettings();
            UpdateLogoForMode();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Settings.ResetToDefaults();

            RefreshFromSettings();
            UpdateLogoForMode();

            // Set up the TableLayoutPanel
            SetupTableLayoutPanel();

            // Initialize the server info panels
            InitializeServerPanels();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("--auto"))
            {
                await Task.Delay(1000); // wait for UI to fully load
                await btnStart_Click(null, null); // begin seeding
            }
        }

        private async void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            string versionUrl = "https://gbicdo.github.io/AutoSeeder/version.json";
            try
            {
                using HttpClient client = new HttpClient();
                string json = await client.GetStringAsync(versionUrl);

                var versionInfo = JsonSerializer.Deserialize<VersionInfo>(json);
                Version currentVersion = Version.Parse(
                    new string(Application.ProductVersion
                        .TakeWhile(c => char.IsDigit(c) || c == '.')
                        .ToArray())
                );
                Version latestVersion = new Version(versionInfo.version);

                if (latestVersion > currentVersion)
                {
                    if (MessageBox.Show(
                        $"A new version ({versionInfo.version}) is available!\n\nChanges:\n{versionInfo.releaseNotes}",
                        "Update Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = versionInfo.downloadUrl,
                            UseShellExecute = true
                        });
                    }
                }
                else
                {
                    MessageBox.Show("You are up to date!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to check for updates: " + ex.Message);
            }
        }

        // Method to initialize the server info panels in the TableLayoutPanel
        private void InitializeServerPanels()
        {
            var settings = Settings.Load();

            // Clear existing panels
            tableLayoutPanel1.Controls.Clear();

            // Set up columns properly
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Get servers that contain "GARRY" in their name
            var garryServers = settings.ServerList.Where(s => s.Name.Contains("GARRY")).ToList();

            // Ensure we have at least two servers (for testing, add a duplicate if needed)
            if (garryServers.Count == 1)
            {
                garryServers.Add(garryServers[0]); // Duplicate the first server
            }

            // Create panels for the servers
            for (int i = 0; i < Math.Min(garryServers.Count, 2); i++)
            {
                Panel serverPanel = CreateServerPanel(garryServers[i]);
                tableLayoutPanel1.Controls.Add(serverPanel, i, 0);
            }

            // Set up the timer to update server info
            SetupServerInfoTimer();
        }


        private void SetupTableLayoutPanel()
        {
            // Configure the TableLayoutPanel
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnStyles.Clear();

            // Add two columns with 50% width each
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Instead of Dock = Fill, set a fixed size and position
            tableLayoutPanel1.Dock = DockStyle.None; // Disable docking
            tableLayoutPanel1.Size = new Size(864, 300); // Set a fixed size that leaves room for buttons
            tableLayoutPanel1.Location = new Point(72, 147); 

            tableLayoutPanel1.Margin = new Padding(5);
            tableLayoutPanel1.Padding = new Padding(5);
        }

        private Panel CreateServerPanel(ServerEntry server)
        {
            // Create a panel with better spacing and layout
            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.BackColor = Color.FromArgb(40, 42, 58); // Dark background
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(10); // Standard padding
            panel.Tag = server; // Store the server entry in the panel's tag

            // Server name with better styling
            Label lblServerName = new Label();
            lblServerName.Text = server.Name;
            lblServerName.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            lblServerName.ForeColor = Color.White;
            lblServerName.AutoSize = false;
            lblServerName.TextAlign = ContentAlignment.MiddleCenter;
            lblServerName.Dock = DockStyle.Top;
            lblServerName.Height = 30; // Reduced height to save space
            panel.Controls.Add(lblServerName);

            // Player count label - positioned with absolute coordinates
            Label lblPlayerCount = new Label();
            lblPlayerCount.Text = "Players: Loading...";
            lblPlayerCount.Font = new Font("Segoe UI", 9.5f);
            lblPlayerCount.ForeColor = Color.LightGray;
            lblPlayerCount.AutoSize = true;
            lblPlayerCount.Location = new Point(15, 40); // Positioned directly below title
            lblPlayerCount.Name = "lblPlayerCount";
            panel.Controls.Add(lblPlayerCount);

            // Map name label
            Label lblMapName = new Label();
            lblMapName.Text = "Map: Loading...";
            lblMapName.Font = new Font("Segoe UI", 9.5f);
            lblMapName.ForeColor = Color.LightGray;
            lblMapName.AutoSize = true;
            lblMapName.Location = new Point(15, 65); // Clear spacing between labels
            lblMapName.Name = "lblMapName";
            panel.Controls.Add(lblMapName);

            // Time remaining label
            Label lblTimeRemaining = new Label();
            lblTimeRemaining.Text = "Time: Loading...";
            lblTimeRemaining.Font = new Font("Segoe UI", 9.5f);
            lblTimeRemaining.ForeColor = Color.LightGray;
            lblTimeRemaining.AutoSize = true;
            lblTimeRemaining.Location = new Point(15, 90); // Clear spacing between labels
            lblTimeRemaining.Name = "lblTimeRemaining";
            panel.Controls.Add(lblTimeRemaining);

            // Progress bar for player count
            ProgressBar progressPlayers = new ProgressBar();
            progressPlayers.Minimum = 0;
            progressPlayers.Maximum = 100;
            progressPlayers.Value = 0;
            progressPlayers.Size = new Size(panel.Width - 30, 15);
            progressPlayers.Location = new Point(15, 120); // Positioned below all labels
            progressPlayers.Name = "progressPlayers";
            panel.Controls.Add(progressPlayers);

            // Status label below progress bar
            Label lblStatus = new Label();
            lblStatus.Text = "Status: Loading...";
            lblStatus.Font = new Font("Segoe UI", 8.5f);
            lblStatus.ForeColor = Color.Orange;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(15, 140); // Directly below progress bar
            lblStatus.Name = "lblPanelStatus";
            panel.Controls.Add(lblStatus);

            // Create a better looking connect button
            Button btnConnect = new Button();
            btnConnect.Text = "Connect";
            btnConnect.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnConnect.Size = new Size(120, 32); // Larger button
            btnConnect.Location = new Point((panel.Width - 120) / 2, 170); // At the bottom
            btnConnect.BackColor = Color.FromArgb(75, 95, 140); // Slightly blue
            btnConnect.ForeColor = Color.White;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.FlatAppearance.BorderSize = 0; // Remove border
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.Tag = server;
            btnConnect.Click += BtnConnect_Click;

            // Add a hover effect for the button
            btnConnect.MouseEnter += (s, e) => {
                btnConnect.BackColor = Color.FromArgb(90, 110, 165); // Lighter on hover
            };
            btnConnect.MouseLeave += (s, e) => {
                btnConnect.BackColor = Color.FromArgb(75, 95, 140); // Back to normal
            };

            panel.Controls.Add(btnConnect);

            // Add a larger checkbox with bold text for selecting this server for seeding
            CheckBox chkSelectForSeed = new CheckBox();
            chkSelectForSeed.Text = "Select to seed";
            chkSelectForSeed.ForeColor = Color.White; // Brighter color for visibility
            chkSelectForSeed.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold); // Larger and bold
            chkSelectForSeed.AutoSize = true;

            // Make the checkbox appear larger
            chkSelectForSeed.Scale(new SizeF(1.2f, 1.2f)); // Scale up the checkbox by 20%

            chkSelectForSeed.Location = new Point((panel.Width - chkSelectForSeed.Width) / 2, 210); // Below the connect button
            chkSelectForSeed.Checked = true; // Default to checked
            chkSelectForSeed.Name = "chkSelectForSeed";
            chkSelectForSeed.Tag = server; // Store the server in the tag

            // Add padding around the checkbox text
            chkSelectForSeed.Padding = new Padding(3);

            panel.Controls.Add(chkSelectForSeed);

            // Handle resizing for progress bar, button, and checkbox
            panel.Resize += (s, e) => {
                progressPlayers.Width = panel.Width - 30;
                btnConnect.Location = new Point((panel.Width - 120) / 2, 170);
                chkSelectForSeed.Location = new Point((panel.Width - chkSelectForSeed.Width) / 2, 210);
            };

            return panel;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag is ServerEntry server)
            {
                ConnectToServer(server);
            }
        }

        private async void ConnectToServer(ServerEntry server)
        {
            // Update the current server
            var settings = Settings.Load();
            settings.ServerName = server.Name;
            settings.Save();
            UpdateCurrentServerLabel();

            // Update UI
            UpdateStatus($"Connecting to {server.Name}...", Color.Green);

            // Launch the game and connect
            await StartGameAndJoin(server);
        }

        private void SetupServerInfoTimer()
        {
            // Dispose of existing timer if any
            if (serverInfoUpdateTimer != null)
            {
                serverInfoUpdateTimer.Stop();
                serverInfoUpdateTimer.Dispose();
            }

            // Create a new timer
            serverInfoUpdateTimer = new WinFormsTimer();
            serverInfoUpdateTimer.Interval = 30000; // 30 seconds
            serverInfoUpdateTimer.Tick += ServerInfoUpdateTimer_Tick;
            serverInfoUpdateTimer.Start();

            // Initial update
            UpdateAllServerInfo();
        }

        private void ServerInfoUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Update all server panels
            UpdateAllServerInfo();
        }

        private void UpdateAllServerInfo()
        {
            // Update each server panel
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Panel panel && panel.Tag is ServerEntry server)
                {
                    UpdateServerInfo(panel, server);
                }
            }
        }

        private async void UpdateServerInfo(Panel panel, ServerEntry server)
        {
            try
            {
                // Get the controls from the panel
                Label lblPlayerCount = FindControlByName(panel, "lblPlayerCount") as Label;
                Label lblMapName = FindControlByName(panel, "lblMapName") as Label;
                Label lblTimeRemaining = FindControlByName(panel, "lblTimeRemaining") as Label;
                Label lblStatus = FindControlByName(panel, "lblPanelStatus") as Label;
                ProgressBar progressPlayers = FindControlByName(panel, "progressPlayers") as ProgressBar;

                if (lblPlayerCount == null || lblMapName == null || lblTimeRemaining == null ||
                    lblStatus == null || progressPlayers == null)
                    return;

                // Update status to show we're connecting
                lblStatus.Text = "Status: Connecting...";
                lblStatus.ForeColor = Color.Orange;

                // Fetch server info
                string response = await httpClient.GetStringAsync(server.ApiUrl);
                var json = JObject.Parse(response);

                // Extract data from the JSON
                if (json["result"] != null)
                {
                    // Get player count
                    int playerCount = 0;
                    int maxPlayerCount = 100;
                    if (json["result"]["player_count"] != null)
                    {
                        playerCount = json["result"]["player_count"].Value<int>();
                    }
                    if (json["result"]["max_player_count"] != null)
                    {
                        maxPlayerCount = json["result"]["max_player_count"].Value<int>();
                    }

                    // Get current map
                    string mapName = "Unknown";
                    if (json["result"]["current_map"] != null &&
                        json["result"]["current_map"]["map"] != null &&
                        json["result"]["current_map"]["map"]["pretty_name"] != null)
                    {
                        mapName = json["result"]["current_map"]["map"]["pretty_name"].Value<string>();
                    }

                    // Get time remaining and format it
                    string timeRemaining = "Unknown";
                    if (json["result"]["time_remaining"] != null)
                    {
                        double seconds = json["result"]["time_remaining"].Value<double>();
                        TimeSpan time = TimeSpan.FromSeconds(seconds);
                        timeRemaining = $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
                    }

                    // Update the UI controls
                    lblPlayerCount.Text = $"Players: {playerCount}/{maxPlayerCount}";
                    lblMapName.Text = $"Map: {mapName}";
                    lblTimeRemaining.Text = $"Time: {timeRemaining}";

                    // Update progress bar
                    progressPlayers.Maximum = maxPlayerCount;
                    progressPlayers.Value = Math.Min(playerCount, maxPlayerCount);

                    // Update status based on player count
                    if (playerCount == 0)
                    {
                        lblStatus.Text = "Status: Empty";
                        lblStatus.ForeColor = Color.Gray;
                    }
                    else if (playerCount < 20)
                    {
                        lblStatus.Text = "Status: Low Population";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (playerCount < 60)
                    {
                        lblStatus.Text = "Status: Medium Population";
                        lblStatus.ForeColor = Color.Orange;
                    }
                    else
                    {
                        lblStatus.Text = "Status: High Population";
                        lblStatus.ForeColor = Color.Lime;
                    }
                }
            }
            catch (Exception ex)
            {
                // Update status to show the error
                Label lblStatus = FindControlByName(panel, "lblPanelStatus") as Label;
                if (lblStatus != null)
                {
                    lblStatus.Text = "Status: Connection Error";
                    lblStatus.ForeColor = Color.Red;
                }
                Console.WriteLine($"Error updating server info: {ex.Message}");
            }
        }

        private void CollectServersToSeed()
        {
            // Clear the previous selection
            serversToSeed.Clear();

            // Look through all panels in the TableLayoutPanel
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Panel panel)
                {
                    // Find the checkbox in this panel
                    CheckBox chkSeed = FindControlByName(panel, "chkSelectForSeed") as CheckBox;

                    // If checkbox exists and is checked, add this server to the list
                    if (chkSeed != null && chkSeed.Checked && chkSeed.Tag is ServerEntry server)
                    {
                        serversToSeed.Add(server);
                    }
                }
            }

            // Log how many servers were selected
            Console.WriteLine($"Selected {serversToSeed.Count} servers for seeding");
        }

        private Control FindControlByName(Control container, string name)
        {
            foreach (Control control in container.Controls)
            {
                if (control.Name == name)
                    return control;

                // Check child controls if any
                Control found = FindControlByName(control, name);
                if (found != null)
                    return found;
            }
            return null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the server info update timer
            if (serverInfoUpdateTimer != null)
            {
                serverInfoUpdateTimer.Stop();
                serverInfoUpdateTimer.Dispose();
            }
        }

        public class VersionInfo
        {
            public string version { get; set; }
            public string downloadUrl { get; set; }
            public string releaseNotes { get; set; }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // This is an empty event handler - can be used for custom painting if needed
        }
    }
}