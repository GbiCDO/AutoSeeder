using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoSeed
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "How AutoSeed Works";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            TextBox infoTextBox = new TextBox();
            infoTextBox.Multiline = true;
            infoTextBox.ReadOnly = true;
            infoTextBox.ScrollBars = ScrollBars.Vertical;
            infoTextBox.Dock = DockStyle.Fill;
            infoTextBox.Font = new Font("Segoe UI", 10);
            infoTextBox.BackColor = Color.White;
            infoTextBox.ForeColor = Color.Black;

            infoTextBox.Text =
"==========================\r\n" +
"         AutoSeed App         \r\n" +
"==========================\r\n\r\n" +
"This application automates the process of seeding Hell Let Loose servers.\r\n\r\n" +
"\u2022 Scheduled Launching:\r\n" +
"   - Weekdays: Starts seeding at your configured time (e.g., 10:00 AM ET).\r\n" +
"   - Weekends: Starts earlier (e.g., 9:00 AM ET).\r\n" +
"   - You can override the schedule with a checkbox.\r\n\r\n" +
"\u2022 Australian Mode:\r\n" +
"   - Uses Australian server, different times, logos, and time zone (AEST).\r\n" +
"   - Toggles back and forth with Helios Mode.\r\n\r\n" +
"\u2022 Actions:\r\n" +
"   1. Waits ~80 seconds for the game to fully load.\r\n" +
"   2. Hits [ESC] to dismiss popups.\r\n" +
"   3. Loads into the server.\r\n" +
"\u2022 Player Monitoring:\r\n" +
"   - Continuously polls server player count every 10 seconds.\r\n" +
"   - Once the threshold is hit (e.g., 60 players), the game slowly rolls you out to the next server.\r\n" +
"   - Optionally shuts down your PC after seeding completes.\r\n\r\n" +
"\u2022 Settings:\r\n" +
"   - You can customize times, select your server, choose your region, and switch modes.\r\n" +
"   - Settings are saved separately for Aliance and Aussie modes.\r\n" +
"   - Defaults can be reset per mode independently.\r\n\r\n" +
"Thank you for using AutoSeed!";

            this.Controls.Add(infoTextBox);
        }
    }
}
