using System;
using System.Windows.Forms;

namespace AutoSeed
{
    public partial class SettingsForm : Form
    {
        private Settings appSettings;
        private bool isAussieMode = false;

        public SettingsForm()
        {
            InitializeComponent();
            appSettings = Settings.Load();
            isAussieMode = appSettings.ServerName.Contains("GARRYBUSTERS") || appSettings.MainFormLogo == "Aussie_logo.jpg";
            UpdateModeButton();

            // Set the time pickers
            timeWeekday.Value = DateTime.Today + appSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + appSettings.WeekendSeedTime;

            // Populate the server dropdown
            cmbServer.Items.Clear();
            cmbServer.Items.AddRange(appSettings.ServerList.ToArray());

            // Select the saved/default server if it's in the list
            if (!string.IsNullOrEmpty(appSettings.ServerName) &&
                cmbServer.Items.Contains(appSettings.ServerName))
            {
                cmbServer.SelectedItem = appSettings.ServerName;
            }
            else if (cmbServer.Items.Count > 0)
            {
                cmbServer.SelectedIndex = 0; // fallback to first server if nothing is saved
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            appSettings.WeekdaySeedTime = timeWeekday.Value.TimeOfDay;
            appSettings.WeekendSeedTime = timeWeekend.Value.TimeOfDay;
            appSettings.ServerName = cmbServer.SelectedItem?.ToString() ?? "";
            appSettings.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnEditServers_Click(object sender, EventArgs e)
        {
            using (var editForm = new EditServersForm())
            {
                editForm.ShowDialog();

                // Reload updated server list after editing
                cmbServer.Items.Clear();
                cmbServer.Items.AddRange(Settings.Load().ServerList.ToArray());

                if (!string.IsNullOrEmpty(appSettings.ServerName) &&
                    cmbServer.Items.Contains(appSettings.ServerName))
                {
                    cmbServer.SelectedItem = appSettings.ServerName;
                }
            }
        }

        private void btnSwitchMode_Click(object sender, EventArgs e)
        {
            // Save current settings to the appropriate slot
            if (isAussieMode)
            {
                appSettings.AussieSettings = CloneCurrentSettings();
            }
            else
            {
                appSettings.HeliosSettings = CloneCurrentSettings();
            }

            // Switch mode
            isAussieMode = !isAussieMode;
            appSettings.CurrentMode = isAussieMode ? "Aussie" : "Helios";

            // Load mode-specific settings
            var modeSettings = isAussieMode
                ? appSettings.AussieSettings ?? Settings.GetDefault("Aussie")
                : appSettings.HeliosSettings ?? Settings.GetDefault("Helios");

            ApplySettings(modeSettings);

            if (!appSettings.ServerList.Contains(modeSettings.ServerName))
                appSettings.ServerList.Add(modeSettings.ServerName);

            appSettings.Save();

            // Refresh UI
            timeWeekday.Value = DateTime.Today + modeSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + modeSettings.WeekendSeedTime;

            cmbServer.Items.Clear();
            cmbServer.Items.AddRange(appSettings.ServerList.ToArray());
            cmbServer.SelectedItem = modeSettings.ServerName;

            UpdateModeButton();

            MessageBox.Show(
                isAussieMode ? "Australian Mode applied!" : "Helios Mode restored!",
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
            string mode = isAussieMode ? "Aussie" : "Helios";
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
                appSettings.HeliosSettings = defaultSettings;
                ApplySettings(appSettings.HeliosSettings);
            }

            appSettings.Save();

            // Update UI with the newly reset values
            timeWeekday.Value = DateTime.Today + defaultSettings.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + defaultSettings.WeekendSeedTime;

            cmbServer.Items.Clear();
            cmbServer.Items.AddRange(defaultSettings.ServerList.ToArray());
            cmbServer.SelectedItem = defaultSettings.ServerName;

            MessageBox.Show($"{mode} defaults restored.", "Defaults Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateModeButton()
        {
            if (btnSwitchMode != null)
            {
                btnSwitchMode.Text = isAussieMode ? "Helios Mode" : "Aussie Mode";
            }
        }

        private Settings CloneCurrentSettings()
        {
            return new Settings
            {
                WeekdaySeedTime = timeWeekday.Value.TimeOfDay,
                WeekendSeedTime = timeWeekend.Value.TimeOfDay,
                ServerName = cmbServer.SelectedItem?.ToString() ?? "",
                ServerList = new List<string>(cmbServer.Items.Cast<string>()),
                MainFormColor = appSettings.MainFormColor,
                MainFormLogo = appSettings.MainFormLogo,
                TimeZoneId = appSettings.TimeZoneId
            };
        }

        private void ApplySettings(Settings s)
        {
            timeWeekday.Value = DateTime.Today + s.WeekdaySeedTime;
            timeWeekend.Value = DateTime.Today + s.WeekendSeedTime;

            cmbServer.Items.Clear();
            cmbServer.Items.AddRange(s.ServerList.ToArray());
            cmbServer.SelectedItem = s.ServerName;

            // Apply time zone and UI settings to appSettings
            appSettings.WeekdaySeedTime = s.WeekdaySeedTime;
            appSettings.WeekendSeedTime = s.WeekendSeedTime;
            appSettings.ServerName = s.ServerName;
            appSettings.ServerList = new List<string>(s.ServerList);
            appSettings.MainFormColor = s.MainFormColor;
            appSettings.MainFormLogo = s.MainFormLogo;
            appSettings.TimeZoneId = s.TimeZoneId;

            // Use the one being applied (s.TimeZoneId), not appSettings
            string abbreviation = GetTimeZoneAbbreviation(s.TimeZoneId);
            lblWeekdayTz.Text = $"({abbreviation})";
            lblWeekendTz.Text = $"({abbreviation})";
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
    }
}
