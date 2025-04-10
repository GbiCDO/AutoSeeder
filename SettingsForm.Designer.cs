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
            cmbServer = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            btnEditServers = new Button();
            btnSwitchMode = new Button();
            btnResetDefaults = new Button();
            lblWeekdayTz = new Label();
            lblWeekendTz = new Label();
            SuspendLayout();
            // 
            // lblWeekday
            // 
            lblWeekday.AutoSize = true;
            lblWeekday.Location = new Point(30, 20);
            lblWeekday.Name = "lblWeekday";
            lblWeekday.Size = new Size(133, 19);
            lblWeekday.TabIndex = 0;
            lblWeekday.Text = "Weekday Seed Time:";
            // 
            // lblWeekend
            // 
            lblWeekend.AutoSize = true;
            lblWeekend.Location = new Point(30, 60);
            lblWeekend.Name = "lblWeekend";
            lblWeekend.Size = new Size(134, 19);
            lblWeekend.TabIndex = 2;
            lblWeekend.Text = "Weekend Seed Time:";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(30, 100);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(89, 19);
            lblServer.TabIndex = 4;
            lblServer.Text = "Select Server:";
            // 
            // timeWeekday
            // 
            timeWeekday.CustomFormat = "hh:mm tt";
            timeWeekday.Format = DateTimePickerFormat.Custom;
            timeWeekday.Location = new Point(200, 15);
            timeWeekday.Name = "timeWeekday";
            timeWeekday.ShowUpDown = true;
            timeWeekday.Size = new Size(120, 25);
            timeWeekday.TabIndex = 1;
            // 
            // timeWeekend
            // 
            timeWeekend.CustomFormat = "hh:mm tt";
            timeWeekend.Format = DateTimePickerFormat.Custom;
            timeWeekend.Location = new Point(200, 55);
            timeWeekend.Name = "timeWeekend";
            timeWeekend.ShowUpDown = true;
            timeWeekend.Size = new Size(120, 25);
            timeWeekend.TabIndex = 3;
            // 
            // cmbServer
            // 
            cmbServer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbServer.Location = new Point(200, 95);
            cmbServer.Name = "cmbServer";
            cmbServer.Size = new Size(200, 25);
            cmbServer.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(30, 151);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(210, 37);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(270, 151);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(210, 37);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnEditServers
            // 
            btnEditServers.Location = new Point(406, 95);
            btnEditServers.Name = "btnEditServers";
            btnEditServers.Size = new Size(51, 25);
            btnEditServers.TabIndex = 8;
            btnEditServers.Text = "Edit";
            btnEditServers.UseVisualStyleBackColor = true;
            btnEditServers.Click += btnEditServers_Click;
            // 
            // btnSwitchMode
            // 
            btnSwitchMode.Location = new Point(370, 12);
            btnSwitchMode.Name = "btnSwitchMode";
            btnSwitchMode.Size = new Size(126, 30);
            btnSwitchMode.TabIndex = 0;
            btnSwitchMode.Text = "Aussie Mode";
            btnSwitchMode.Click += btnSwitchMode_Click;
            // 
            // btnResetDefaults
            // 
            btnResetDefaults.Location = new Point(370, 54);
            btnResetDefaults.Name = "btnResetDefaults";
            btnResetDefaults.Size = new Size(126, 30);
            btnResetDefaults.TabIndex = 0;
            btnResetDefaults.Text = "Default Settings";
            btnResetDefaults.Click += btnResetDefaults_Click;
            // 
            // lblWeekdayTz
            // 
            lblWeekdayTz.AutoSize = true;
            lblWeekdayTz.Location = new Point(323, 15);
            lblWeekdayTz.Name = "lblWeekdayTz";
            lblWeekdayTz.Size = new Size(31, 19);
            lblWeekdayTz.TabIndex = 0;
            lblWeekdayTz.Text = "(ET)";
            // 
            // lblWeekendTz
            // 
            lblWeekendTz.AutoSize = true;
            lblWeekendTz.Location = new Point(323, 55);
            lblWeekendTz.Name = "lblWeekendTz";
            lblWeekendTz.Size = new Size(31, 19);
            lblWeekendTz.TabIndex = 1;
            lblWeekendTz.Text = "(ET)";
            // 
            // SettingsForm
            // 
            ClientSize = new Size(537, 201);
            Controls.Add(lblWeekdayTz);
            Controls.Add(lblWeekendTz);
            Controls.Add(btnResetDefaults);
            Controls.Add(btnSwitchMode);
            Controls.Add(btnEditServers);
            Controls.Add(lblWeekday);
            Controls.Add(timeWeekday);
            Controls.Add(lblWeekend);
            Controls.Add(timeWeekend);
            Controls.Add(lblServer);
            Controls.Add(cmbServer);
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
        private DateTimePicker timeWeekday;
        private DateTimePicker timeWeekend;
        private ComboBox cmbServer;
        private Button btnSave;
        private Button btnCancel;
        private Button btnEditServers;
        private Button btnSwitchMode;
        private Button btnResetDefaults;
    }
}