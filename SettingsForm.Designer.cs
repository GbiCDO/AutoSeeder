namespace AutoSeed
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblWeekday = new Label();
            lblWeekend = new Label();
            lblServer = new Label();
            timeWeekday = new DateTimePicker();
            timeWeekend = new DateTimePicker();
            btnSave = new Button();
            btnCancel = new Button();
            btnSwitchMode = new Button();
            btnResetDefaults = new Button();
            lblWeekdayTz = new Label();
            lblWeekendTz = new Label();
            lblServerDisplay = new Label();
            btnToggleAutoSeed = new Button();
            SuspendLayout();
            // 
            // lblWeekday
            // 
            lblWeekday.AutoSize = true;
            lblWeekday.Location = new Point(28, 17);
            lblWeekday.Name = "lblWeekday";
            lblWeekday.Size = new Size(133, 19);
            lblWeekday.TabIndex = 0;
            lblWeekday.Text = "Weekday Seed Time:";
            // 
            // lblWeekend
            // 
            lblWeekend.AutoSize = true;
            lblWeekend.Location = new Point(28, 57);
            lblWeekend.Name = "lblWeekend";
            lblWeekend.Size = new Size(134, 19);
            lblWeekend.TabIndex = 2;
            lblWeekend.Text = "Weekend Seed Time:";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(111, 96);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(56, 19);
            lblServer.TabIndex = 4;
            lblServer.Text = "Servers:";
            // 
            // timeWeekday
            // 
            timeWeekday.CustomFormat = "hh:mm tt";
            timeWeekday.Format = DateTimePickerFormat.Custom;
            timeWeekday.Location = new Point(198, 12);
            timeWeekday.Name = "timeWeekday";
            timeWeekday.ShowUpDown = true;
            timeWeekday.Size = new Size(120, 25);
            timeWeekday.TabIndex = 1;
            // 
            // timeWeekend
            // 
            timeWeekend.CustomFormat = "hh:mm tt";
            timeWeekend.Format = DateTimePickerFormat.Custom;
            timeWeekend.Location = new Point(198, 52);
            timeWeekend.Name = "timeWeekend";
            timeWeekend.ShowUpDown = true;
            timeWeekend.Size = new Size(120, 25);
            timeWeekend.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(28, 174);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(210, 37);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(284, 174);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(210, 37);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSwitchMode
            // 
            btnSwitchMode.Location = new Point(368, 9);
            btnSwitchMode.Name = "btnSwitchMode";
            btnSwitchMode.Size = new Size(126, 30);
            btnSwitchMode.TabIndex = 0;
            btnSwitchMode.Text = "Aussie Mode";
            btnSwitchMode.Click += btnSwitchMode_Click;
            // 
            // btnResetDefaults
            // 
            btnResetDefaults.Location = new Point(368, 45);
            btnResetDefaults.Name = "btnResetDefaults";
            btnResetDefaults.Size = new Size(126, 30);
            btnResetDefaults.TabIndex = 0;
            btnResetDefaults.Text = "Default Settings";
            btnResetDefaults.Click += btnResetDefaults_Click;
            // 
            // lblWeekdayTz
            // 
            lblWeekdayTz.AutoSize = true;
            lblWeekdayTz.Location = new Point(321, 12);
            lblWeekdayTz.Name = "lblWeekdayTz";
            lblWeekdayTz.Size = new Size(31, 19);
            lblWeekdayTz.TabIndex = 0;
            lblWeekdayTz.Text = "(ET)";
            // 
            // lblWeekendTz
            // 
            lblWeekendTz.AutoSize = true;
            lblWeekendTz.Location = new Point(321, 52);
            lblWeekendTz.Name = "lblWeekendTz";
            lblWeekendTz.Size = new Size(31, 19);
            lblWeekendTz.TabIndex = 1;
            lblWeekendTz.Text = "(ET)";
            // 
            // lblServerDisplay
            // 
            lblServerDisplay.AutoSize = true;
            lblServerDisplay.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblServerDisplay.Location = new Point(198, 96);
            lblServerDisplay.Name = "lblServerDisplay";
            lblServerDisplay.Size = new Size(0, 15);
            lblServerDisplay.TabIndex = 0;
            // 
            // btnToggleAutoSeed
            // 
            btnToggleAutoSeed.Location = new Point(368, 80);
            btnToggleAutoSeed.Name = "btnToggleAutoSeed";
            btnToggleAutoSeed.Size = new Size(126, 31);
            btnToggleAutoSeed.TabIndex = 8;
            btnToggleAutoSeed.Text = "Auto-Seed";
            btnToggleAutoSeed.UseVisualStyleBackColor = true;
            btnToggleAutoSeed.Click += btnToggleAutoSeed_Click;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(551, 240);
            Controls.Add(btnToggleAutoSeed);
            Controls.Add(lblServerDisplay);
            Controls.Add(lblWeekdayTz);
            Controls.Add(lblWeekendTz);
            Controls.Add(btnResetDefaults);
            Controls.Add(btnSwitchMode);
            Controls.Add(lblWeekday);
            Controls.Add(timeWeekday);
            Controls.Add(lblWeekend);
            Controls.Add(timeWeekend);
            Controls.Add(lblServer);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seeding Schedule Settings";
            ResumeLayout(false);
            PerformLayout();
        }


        private Label lblWeekday;
        private Label lblWeekend;
        private Label lblServer;
        private Label lblWeekdayTz;
        private Label lblWeekendTz;
        private Label lblServerDisplay;
        private DateTimePicker timeWeekday;
        private DateTimePicker timeWeekend;
        private Button btnSave;
        private Button btnCancel;
        private Button btnSwitchMode;
        private Button btnResetDefaults;
        private Button btnToggleAutoSeed;
    }
}