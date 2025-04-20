using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using WindowsInput;
using WindowsInput.Native;
using TimeZoneConverter;
using Microsoft.Win32;
using System.Text.Json;
using System.Threading.Tasks;


namespace AutoSeed
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            // Safe: add the click handler in user code
            this.picInfo.Click += (s, e) => new InfoForm().ShowDialog();
            const string CURRENT_VERSION = "1.0.1";
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
            var settings = Settings.Load();

            foreach (var server in settings.ServerList)
            {
                UpdateStatus($"Seeding server: {server.Name}", Color.LightBlue);
                settings.ServerName = server.Name;
                UpdateCurrentServerLabel();

                await StartGameAndJoin(server);
                UpdateStatus($"{server.Name}: Waiting for player threshold...", Color.Orange);

                bool thresholdReached = await WaitForPlayerThresholdAsync(server.ApiUrl, 65); // example threshold

                if (thresholdReached)
                {
                    UpdateStatus("Player threshold reached. Beginning staggered disconnects...", Color.Yellow);
                    await StaggerDisconnectAsync();

                    KillGame();

                    UpdateStatus("Server complete. Moving to next server...", Color.LightGreen);
                    await Task.Delay(10000); // optional pause before next loop
                }
                else
                {
                    UpdateStatus("Failed to reach threshold. Moving to next server.", Color.Red);
                    KillGame();
                }
            }

            UpdateStatus("All servers seeded. Exiting...", Color.DarkGreen);
            if (chkShutdown.Checked)
            {
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
            int timeoutMinutes = 60;
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

            string logoPath = Path.Combine(Application.StartupPath, settings.MainFormLogo);

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

                    // Determine which logo to assign image to
                    bool isAussie = settings.MainFormLogo == "Aussie_logo.jpg";

                    if (isAussie)
                    {
                        picLogo2.Image = Image.FromFile(logoPath);
                    }
                    else
                    {
                        picLogo.Image = Image.FromFile(logoPath);
                    }
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            RefreshFromSettings();
            UpdateLogoForMode();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("--auto"))
            {
                await Task.Delay(1000); // wait for UI to fully load
                await btnStart_Click(null, null); // begin seeding
            }
        }

        private async void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            string versionUrl = "https://hazah7419.github.io/AutoSeeder/version.json";
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

        public class VersionInfo
        {
            public string version { get; set; }
            public string downloadUrl { get; set; }
            public string releaseNotes { get; set; }
        }
    }
}
