using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoSeed
{
    public partial class SettingsForm : Form
    {
        private Settings currentSettings;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
            DisplayServerList();
            UpdateTimezoneLabels();
        }

        private void LoadSettings()
        {
            currentSettings = Settings.Load();
            timeWeekday.Value = DateTime.Today.Add(currentSettings.WeekdaySeedTime);
            timeWeekend.Value = DateTime.Today.Add(currentSettings.WeekendSeedTime);
        }

        private void DisplayServerList()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var server in currentSettings.ServerList)
            {
                sb.AppendLine($"• {server.Name}");
            }

            lblServerDisplay.Text = sb.ToString();
        }

        private void UpdateTimezoneLabels()
        {
            string abbr = GetTimeZoneAbbreviation(currentSettings.TimeZoneId);
            lblWeekdayTz.Text = $"({abbr})";
            lblWeekendTz.Text = $"({abbr})";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                currentSettings.WeekdaySeedTime = timeWeekday.Value.TimeOfDay;
                currentSettings.WeekendSeedTime = timeWeekend.Value.TimeOfDay;

                // Always ensure Aussie mode settings
                currentSettings.MainFormLogo = "Aussie_logo.jpg";
                currentSettings.MainFormColor = "#00008B";

                currentSettings.Save();

                // Signal successful save
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset to default settings?", "Confirm Reset",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Settings.ResetToDefaults();
                LoadSettings();
                DisplayServerList();
                UpdateTimezoneLabels();
            }
        }
    }
}