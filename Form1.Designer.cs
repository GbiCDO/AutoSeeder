namespace AutoSeed
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblTitle = new Label();
            btnStart = new Button();
            chkShutdown = new CheckBox();
            chkOverride = new CheckBox();
            lblStatus = new Label();
            btnCancel = new Button();
            picSettings = new PictureBox();
            lblCurrentServer = new Label();
            picLogo = new PictureBox();
            picLogo2 = new PictureBox();
            picInfo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLogo2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picInfo).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 10F);
            lblTitle.ForeColor = SystemColors.Control;
            lblTitle.Location = new Point(64, 474);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(526, 19);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Auto-seeding starts at {weekday} (ET) on weekdays and {weekend} (ET) on weekends.";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.Location = new Point(195, 279);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(225, 50);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start Seeding";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // chkShutdown
            // 
            chkShutdown.AutoSize = true;
            chkShutdown.ForeColor = SystemColors.Control;
            chkShutdown.Location = new Point(195, 439);
            chkShutdown.Name = "chkShutdown";
            chkShutdown.Size = new Size(273, 23);
            chkShutdown.TabIndex = 2;
            chkShutdown.Text = "Shutdown computer after server seeded";
            chkShutdown.UseVisualStyleBackColor = true;
            // 
            // chkOverride
            // 
            chkOverride.AutoSize = true;
            chkOverride.ForeColor = SystemColors.Control;
            chkOverride.Location = new Point(195, 410);
            chkOverride.Name = "chkOverride";
            chkOverride.Size = new Size(208, 23);
            chkOverride.TabIndex = 3;
            chkOverride.Text = "Override schedule (start now)";
            chkOverride.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.DarkGray;
            lblStatus.Location = new Point(12, 9);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(46, 19);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Ready";
            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Location = new Point(195, 335);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(225, 50);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // picSettings
            // 
            picSettings.Cursor = Cursors.Hand;
            picSettings.Image = Properties.Resources.CogWheel;
            picSettings.Location = new Point(561, 9);
            picSettings.Name = "picSettings";
            picSettings.Size = new Size(40, 40);
            picSettings.SizeMode = PictureBoxSizeMode.Zoom;
            picSettings.TabIndex = 0;
            picSettings.TabStop = false;
            picSettings.Click += picSettings_Click;
            // 
            // lblCurrentServer
            // 
            lblCurrentServer.AutoSize = true;
            lblCurrentServer.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblCurrentServer.ForeColor = Color.DimGray;
            lblCurrentServer.Location = new Point(195, 388);
            lblCurrentServer.Name = "lblCurrentServer";
            lblCurrentServer.Size = new Size(169, 19);
            lblCurrentServer.TabIndex = 0;
            lblCurrentServer.Text = "Current server: (loading...)";
            //
            // picLogo (Helios)
            //
            picLogo = new PictureBox();
            picLogo.Image = Image.FromFile("Helios Logo TRANSPARENT.png");
            picLogo.Location = new Point(195, 52);
            picLogo.Size = new Size(225, 218);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.Visible = true;
            this.Controls.Add(picLogo);
            //
            // picLogo2 (Aussie)
            //
            picLogo2 = new PictureBox();
            picLogo2.Image = Image.FromFile("Aussie_logo.jpg");
            picLogo2.Location = new Point(195, 52);
            picLogo2.Size = new Size(225, 218);
            picLogo2.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo2.Visible = false;
            this.Controls.Add(picLogo2);
            // 
            // picInfo
            // 
            picInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            picInfo.Cursor = Cursors.Hand;
            picInfo.Image = Properties.Resources.info_icon;
            picInfo.Location = new Point(12, 466);
            picInfo.Name = "picInfo";
            picInfo.Size = new Size(24, 24);
            picInfo.SizeMode = PictureBoxSizeMode.Zoom;
            picInfo.TabIndex = 2;
            picInfo.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 25, 38);
            ClientSize = new Size(619, 502);
            Controls.Add(picInfo);
            Controls.Add(picLogo2);
            Controls.Add(lblCurrentServer);
            Controls.Add(picSettings);
            Controls.Add(lblTitle);
            Controls.Add(btnStart);
            Controls.Add(chkShutdown);
            Controls.Add(chkOverride);
            Controls.Add(lblStatus);
            Controls.Add(picLogo);
            Controls.Add(btnCancel);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Helios AutoSeeder";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)picSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLogo2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblCurrentServer;
        private Label lblTitle;
        private Button btnStart;
        private CheckBox chkShutdown;
        private CheckBox chkOverride;
        private Label lblStatus;
        private PictureBox picSettings;
        private Button btnCancel;
        private PictureBox picLogo;
        private PictureBox picLogo2;
        private PictureBox picInfo;

    }
}
