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
            btnCheckForUpdates = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
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
            lblTitle.Location = new Point(42, 714);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(526, 19);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Auto-seeding starts at {weekday} (ET) on weekdays and {weekend} (ET) on weekends.";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.Location = new Point(418, 501);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(225, 50);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start Seeding";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_Wrapper;
            // 
            // chkShutdown
            // 
            chkShutdown.AutoSize = true;
            chkShutdown.ForeColor = SystemColors.Control;
            chkShutdown.Location = new Point(384, 642);
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
            chkOverride.Location = new Point(418, 613);
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
            btnCancel.Location = new Point(418, 557);
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
            picSettings.Location = new Point(975, 12);
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
            lblCurrentServer.Location = new Point(444, 479);
            lblCurrentServer.Name = "lblCurrentServer";
            lblCurrentServer.Size = new Size(169, 19);
            lblCurrentServer.TabIndex = 0;
            lblCurrentServer.Text = "Current server: (loading...)";
            // 
            // picLogo
            // 
            picLogo.Location = new Point(838, 18);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(119, 86);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // picLogo2
            // 
            picLogo2.Location = new Point(838, 12);
            picLogo2.Name = "picLogo2";
            picLogo2.Size = new Size(112, 92);
            picLogo2.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo2.TabIndex = 1;
            picLogo2.TabStop = false;
            picLogo2.Visible = false;
            // 
            // picInfo
            // 
            picInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            picInfo.Cursor = Cursors.Hand;
            picInfo.Image = Properties.Resources.info_icon;
            picInfo.Location = new Point(12, 709);
            picInfo.Name = "picInfo";
            picInfo.Size = new Size(24, 24);
            picInfo.SizeMode = PictureBoxSizeMode.Zoom;
            picInfo.TabIndex = 2;
            picInfo.TabStop = false;
            // 
            // btnCheckForUpdates
            // 
            btnCheckForUpdates.Location = new Point(799, 691);
            btnCheckForUpdates.Name = "btnCheckForUpdates";
            btnCheckForUpdates.Size = new Size(225, 34);
            btnCheckForUpdates.TabIndex = 7;
            btnCheckForUpdates.Text = "Check For Updates";
            btnCheckForUpdates.UseVisualStyleBackColor = true;
            btnCheckForUpdates.Click += btnCheckForUpdates_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(72, 147);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(864, 319);
            tableLayoutPanel1.TabIndex = 8;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 25, 38);
            ClientSize = new Size(1036, 737);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnCheckForUpdates);
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
            Text = "AutoSeeder";
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
        private Button btnCheckForUpdates;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
