using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading;
using System.Timers;
using TimeZoneConverter;


namespace AutoSeed
{
    public partial class Form1 : Form
    {
        private Process hllProcess;

        public Form1()
        {
            InitializeComponent();

            // Safe: add the click handler in user code
            this.picInfo.Click += (s, e) => new InfoForm().ShowDialog();
        }

        private static readonly HttpClient httpClient = new HttpClient();
        private const string apiUrl = "http://207.244.232.58:7010/api/get_public_info";
        private const int playerThreshold = 80;
        private CancellationTokenSource cancellationTokenSource;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var appSettings = Settings.Load();
            TimeSpan seedTime = appSettings.WeekdaySeedTime;
            string seedType = appSettings.ServerName;

            btnStart.Enabled = false;
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
                LaunchHellLetLoose();
                await Task.Delay(30000);

                UpdateStatus("Running join macro...", Color.MediumPurple);
                FocusHellLetLooseWindow();
                await Task.Delay(1500); // Give time for the game to pop in front
                RunJoinMacro();

                await Task.Delay(300000); // 5 minutes
                UpdateStatus("Monitoring player count...", Color.DodgerBlue);
                await MonitorServerPlayerCount();

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
                btnStart.Text = "Start Seeding";
                btnCancel.Enabled = false;
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
            }
        }

        private void LaunchHellLetLoose()
        {
            try
            {
                string appId = "686810"; // Hell Let Loose App ID

                // Step 1: Launch Hell Let Loose via Steam App ID
                var launchGame = new ProcessStartInfo
                {
                    FileName = $"steam://rungameid/{appId}",
                    UseShellExecute = true
                };
                Process.Start(launchGame);

                // Step 2: Wait for the game to fully launch
                //await Task.Delay(15000); // Wait 15 seconds (adjust if needed)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game or connect to server: " + ex.Message);
            }
        }

        private void RunJoinMacro()
        {
            var exePath = Path.Combine(Application.StartupPath, "join_macro.exe");

            if (!File.Exists(exePath))
            {
                MessageBox.Show("join_macro.exe not found!");
                return;
            }

            var settings = Settings.Load();
            string selectedServer = settings.ServerName;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{selectedServer}\"", // pass server name in quotes
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Application.StartupPath
            };

            Process.Start(psi);
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

        private async Task MonitorServerPlayerCount()
        {
            int currentPlayers = 0;

            while (true)
            {
                try
                {
                    string response = await httpClient.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);

                    currentPlayers = json["result"]?["player_count"]?.Value<int>() ?? 0;

                    Console.WriteLine($"Current player count: {currentPlayers}");

                    if (currentPlayers >= playerThreshold)
                    {
                        KillGame();

                        if (chkShutdown.Checked)
                        {
                            UpdateStatus("Threshold met. Shutting down...", Color.Red);
                            ShutdownComputer();
                        }
                        else
                        {
                            UpdateStatus("Seeding complete. Staying online.", Color.DarkGreen);
                        }

                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking server: " + ex.Message);
                }

                await Task.Delay(10000); // wait 10 seconds before next check
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
            lblCurrentServer.Text = $"Current server: {settings.ServerName}";
        }

        private void UpdateLogoForMode()
        {
            var settings = Settings.Load();

            bool isAussie = settings.ServerName.Contains("GARRY")
                || settings.MainFormLogo == "Aussie_logo.jpg";

            picLogo.Visible = !isAussie;
            picLogo2.Visible = isAussie;
        }

        private void ResetAppState()
        {
            btnStart.Enabled = true;
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

            // Apply background color
            this.BackColor = ColorTranslator.FromHtml(settings.MainFormColor);

            // Path to logo file
            string logoPath = Path.Combine(Application.StartupPath, settings.MainFormLogo);

            try
            {
                // Force image reload by disposing the old one and reloading from disk
                if (File.Exists(logoPath))
                {
                    // Dispose old image (important to release resources)
                    if (picLogo.Image != null)
                    {
                        picLogo.Image.Dispose();
                        picLogo.Image = null;
                    }

                    // Force garbage collection to fully release file lock (if needed)
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // Load new image from file (not from embedded resource)
                    picLogo.Image = Image.FromFile(logoPath);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshFromSettings(); // This should call ApplyThemeFromSettings()
            UpdateLogoForMode();
        }
    }
}
