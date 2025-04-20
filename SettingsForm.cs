using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoSeed
{
    public partial class SettingsForm : Form
    {
        private Settings appSettings;
        private bool isAussieMode = false;
        private const string TaskName = "AutoSeedWakeup";

        public SettingsForm()
        {
            InitializeComponent();
            appSettings = Settings.Load();
            //btnToggleAutoSeed.Text = IsScheduledTaskCreated() ? "Disable Auto" : "Enable Auto";
            isAussieMode = appSettings.ServerName.Contains("GARRYBUSTERS") || appSettings.MainFormLogo == "Aussie_logo.jpg";
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            UpdateModeButton();

            timeWeekday.Value = DateTime.Today + appSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + appSettings.WeekendSeedTime;

            if (appSettings.ServerList != null && appSettings.ServerList.Count > 0)
            {
                string allServerNames = string.Join("\n", appSettings.ServerList.Select(server => $"• {server.Name}"));
                lblServerDisplay.Text = $"\n{allServerNames}";
            }
            else
            {
                lblServerDisplay.Text = "No servers configured.";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            appSettings.WeekdaySeedTime = timeWeekday.Value.TimeOfDay;
            appSettings.WeekendSeedTime = timeWeekend.Value.TimeOfDay;
            appSettings.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSwitchMode_Click(object sender, EventArgs e)
        {
            if (isAussieMode)
            {
                appSettings.AussieSettings = CloneCurrentSettings();
            }
            else
            {
                appSettings.AllianceSettings = CloneCurrentSettings();
            }

            isAussieMode = !isAussieMode;
            appSettings.CurrentMode = isAussieMode ? "Aussie" : "Aliance";

            var modeSettings = isAussieMode
                ? appSettings.AussieSettings ?? Settings.GetDefault("Aussie")
                : appSettings.AllianceSettings ?? Settings.GetDefault("Alliance");

            ApplySettings(modeSettings);
            appSettings.Save();

            timeWeekday.Value = DateTime.Today + modeSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + modeSettings.WeekendSeedTime;
            if (appSettings.ServerList != null && appSettings.ServerList.Count > 0)
            {
                string allServerNames = string.Join("\n", appSettings.ServerList.Select(server => $"• {server.Name}"));
                lblServerDisplay.Text = $"\n{allServerNames}";
            }
            else
            {
                lblServerDisplay.Text = "No servers configured.";
            }

            UpdateModeButton();

            MessageBox.Show(
                isAussieMode ? "Australian Mode applied!" : "Alliance Mode restored!",
                "Mode Switched",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            ResetToDefaultsBasedOnMode();
        }

        private void ResetToDefaultsBasedOnMode()
        {
            string mode = isAussieMode ? "Aussie" : "Alliance";
            string abbreviation = GetTimeZoneAbbreviation(appSettings.TimeZoneId);
            var defaultSettings = Settings.GetDefault(mode);

            if (isAussieMode)
            {
                lblWeekdayTz.Text = $"({abbreviation})";
                lblWeekendTz.Text = $"({abbreviation})";
                appSettings.AussieSettings = defaultSettings;
                ApplySettings(appSettings.AussieSettings);
            }
            else
            {
                lblWeekdayTz.Text = $"({abbreviation})";
                lblWeekendTz.Text = $"({abbreviation})";
                appSettings.AllianceSettings = defaultSettings;
                ApplySettings(appSettings.AllianceSettings);
            }

            appSettings.Save();

            timeWeekday.Value = DateTime.Today + defaultSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + defaultSettings.WeekendSeedTime;
            if (appSettings.ServerList != null && defaultSettings.ServerList.Count > 0)
            {
                string allServerNames = string.Join("\n", defaultSettings.ServerList.Select(server => $"• {server.Name}"));
                lblServerDisplay.Text = $"\n{allServerNames}";
            }
            else
            {
                lblServerDisplay.Text = "No servers configured.";
            }

            MessageBox.Show($"{mode} defaults restored.", "Defaults Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateModeButton()
        {
            if (btnSwitchMode != null)
            {
                btnSwitchMode.Text = isAussieMode ? "Alliance Mode" : "Aussie Mode";
            }
        }

        private Settings CloneCurrentSettings()
        {
            return new Settings
            {
                WeekdaySeedTime = timeWeekday.Value.TimeOfDay,
                WeekendSeedTime = timeWeekend.Value.TimeOfDay,
                ServerName = appSettings.ServerName,
                ServerList = appSettings.ServerList,
                MainFormColor = appSettings.MainFormColor,
                MainFormLogo = appSettings.MainFormLogo,
                TimeZoneId = appSettings.TimeZoneId
            };
        }

        private void ApplySettings(Settings s)
        {
            timeWeekday.Value = DateTime.Today + s.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + s.WeekendSeedTime;

            appSettings.WeekdaySeedTime = s.WeekdaySeedTime;
            appSettings.WeekendSeedTime = s.WeekendSeedTime;
            appSettings.ServerName = s.ServerName;
            appSettings.ServerList = s.ServerList;
            appSettings.MainFormColor = s.MainFormColor;
            appSettings.MainFormLogo = s.MainFormLogo;
            appSettings.TimeZoneId = s.TimeZoneId;

            string abbreviation = GetTimeZoneAbbreviation(s.TimeZoneId);
            lblWeekdayTz.Text = $"({abbreviation})";
            lblWeekendTz.Text = $"({abbreviation})";
            // NEW: Display all server names from the list
            if (s.ServerList != null && s.ServerList.Count > 0)
            {
                string allServerNames = string.Join("\n", s.ServerList.Select(server => $"• {server.Name}"));
                lblServerDisplay.Text = $"\n{allServerNames}";
            }
            else
            {
                lblServerDisplay.Text = "No servers configured.";
            }
        }

        private string GetTimeZoneAbbreviation(string timeZoneId)
        {
            return timeZoneId switch
            {
                "Eastern Standard Time" => "ET",
                "AUS Eastern Standard Time" => "AEST",
                _ => timeZoneId
            };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnToggleAutoSeed_Click(object sender, EventArgs e)
        {
            if (IsScheduledTaskCreated())
            {
                DeleteScheduledTask();
                MessageBox.Show("Auto-start task removed.", "Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //btnToggleAutoSeed.Text = "Enable Auto";
            }
            else
            {
                CreateScheduledTask();

                //btnToggleAutoSeed.Text = "Disable Auto";
            }
        }

        private void CreateScheduledTask()
        {
            string triggerTime = "06:15"; // You can make this dynamic later

            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSeed.exe");

            // Ensure the path is wrapped in quotes in case it contains spaces (like Program Files)
            string safeExePath = $"\"{exePath}\"";

            string args = $"/Create /SC DAILY /TN \"{TaskName}\" /TR \"{safeExePath}\" /ST {triggerTime} /F";

            ProcessStartInfo psi = new ProcessStartInfo("schtasks", args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    MessageBox.Show($"Failed to create task:\n{error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Auto-seeding task created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Show task details
                    string taskDetails = GetScheduledTaskDetails();
                    MessageBox.Show(
                        $"Auto-start enabled!\n\n{taskDetails}\n\nDaily auto seed should now be setup with scheduled task.\n\nPlease make sure your computer is not asleep and you are logged in during the running time or auto seed will not work.",
                        $"Scheduled Task Created",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        private void DeleteScheduledTask()
        {
            string args = $"/Delete /F /TN \"{TaskName}\"";

            ProcessStartInfo psi = new ProcessStartInfo("schtasks", args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                // no need to handle failure (task may not exist)
            }
        }

        private bool IsScheduledTaskCreated()
        {
            string args = $"/Query /TN \"{TaskName}\"";

            ProcessStartInfo psi = new ProcessStartInfo("schtasks", args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0; // 0 = success (task exists), non-zero = error
            }
        }

        private string GetScheduledTaskDetails()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Query /TN \"{TaskName}\" /V /FO LIST",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                    return "Unable to retrieve task details.";

                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                var keysToInclude = new[]
                {
            "Folder:",
            "HostName:",
            "TaskName:",
            "Next Run Time:",
            "Status:",
            "Logon Mode:",
        };

                var filtered = lines
                    .Where(line => keysToInclude.Any(key => line.StartsWith(key)))
                    .ToList();

                return string.Join(Environment.NewLine, filtered);
            }
        }
    }
}
