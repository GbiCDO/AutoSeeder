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
            lblWeekday.Location = new Point(1, 16);
            lblWeekday.Name = "lblWeekday";
            lblWeekday.Size = new Size(133, 19);
            lblWeekday.TabIndex = 0;
            lblWeekday.Text = "Weekday Seed Time:";
            // 
            // lblWeekend
            // 
            lblWeekend.AutoSize = true;
            lblWeekend.Location = new Point(1, 65);
            lblWeekend.Name = "lblWeekend";
            lblWeekend.Size = new Size(134, 19);
            lblWeekend.TabIndex = 2;
            lblWeekend.Text = "Weekend Seed Time:";
            // 
            // lblServer
            // 
            lblServer.Anchor = AnchorStyles.Left;
            lblServer.AutoSize = true;
            lblServer.Location = new Point(78, 169);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(56, 19);
            lblServer.TabIndex = 4;
            lblServer.Text = "Servers:";
            // 
            // timeWeekday
            // 
            timeWeekday.Anchor = AnchorStyles.Top;
            timeWeekday.CustomFormat = "hh:mm tt";
            timeWeekday.Format = DateTimePickerFormat.Custom;
            timeWeekday.Location = new Point(223, 12);
            timeWeekday.Name = "timeWeekday";
            timeWeekday.ShowUpDown = true;
            timeWeekday.Size = new Size(151, 25);
            timeWeekday.TabIndex = 1;
            // 
            // timeWeekend
            // 
            timeWeekend.Anchor = AnchorStyles.Top;
            timeWeekend.CustomFormat = "hh:mm tt";
            timeWeekend.Format = DateTimePickerFormat.Custom;
            timeWeekend.Location = new Point(224, 65);
            timeWeekend.Name = "timeWeekend";
            timeWeekend.ShowUpDown = true;
            timeWeekend.Size = new Size(150, 25);
            timeWeekend.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.AutoSize = true;
            btnSave.Location = new Point(78, 311);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(210, 37);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.AutoSize = true;
            btnCancel.Location = new Point(389, 311);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(210, 37);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSwitchMode
            // 
            btnSwitchMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSwitchMode.AutoSize = true;
            btnSwitchMode.Location = new Point(502, 9);
            btnSwitchMode.Name = "btnSwitchMode";
            btnSwitchMode.Size = new Size(190, 30);
            btnSwitchMode.TabIndex = 0;
            btnSwitchMode.Text = "Aussie Mode";
            btnSwitchMode.Click += btnSwitchMode_Click;
            // 
            // btnResetDefaults
            // 
            btnResetDefaults.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetDefaults.AutoSize = true;
            btnResetDefaults.Location = new Point(502, 64);
            btnResetDefaults.Name = "btnResetDefaults";
            btnResetDefaults.Size = new Size(190, 30);
            btnResetDefaults.TabIndex = 0;
            btnResetDefaults.Text = "Default Settings";
            btnResetDefaults.Click += btnResetDefaults_Click;
            // 
            // lblWeekdayTz
            // 
            lblWeekdayTz.Anchor = AnchorStyles.Top;
            lblWeekdayTz.AutoSize = true;
            lblWeekdayTz.Location = new Point(380, 12);
            lblWeekdayTz.Name = "lblWeekdayTz";
            lblWeekdayTz.Size = new Size(31, 19);
            lblWeekdayTz.TabIndex = 0;
            lblWeekdayTz.Text = "(ET)";
            // 
            // lblWeekendTz
            // 
            lblWeekendTz.Anchor = AnchorStyles.Top;
            lblWeekendTz.AutoSize = true;
            lblWeekendTz.Location = new Point(380, 65);
            lblWeekendTz.Name = "lblWeekendTz";
            lblWeekendTz.Size = new Size(31, 19);
            lblWeekendTz.TabIndex = 1;
            lblWeekendTz.Text = "(ET)";
            // 
            // lblServerDisplay
            // 
            lblServerDisplay.AutoSize = true;
            lblServerDisplay.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblServerDisplay.Location = new Point(198, 140);
            lblServerDisplay.Name = "lblServerDisplay";
            lblServerDisplay.Size = new Size(0, 15);
            lblServerDisplay.TabIndex = 0;
            // 
            // btnToggleAutoSeed
            // 
            btnToggleAutoSeed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnToggleAutoSeed.AutoSize = true;
            btnToggleAutoSeed.Location = new Point(502, 124);
            btnToggleAutoSeed.Name = "btnToggleAutoSeed";
            btnToggleAutoSeed.Size = new Size(190, 31);
            btnToggleAutoSeed.TabIndex = 8;
            btnToggleAutoSeed.Text = "Auto-Seed";
            btnToggleAutoSeed.UseVisualStyleBackColor = true;
            btnToggleAutoSeed.Click += btnToggleAutoSeed_Click;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(704, 375);
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