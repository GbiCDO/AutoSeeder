namespace AutoSeed
{
    partial class EditServersForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServerList = new Label();
            this.lstServers = new ListBox();
            this.txtServer = new TextBox();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // lblServerList
            this.lblServerList.AutoSize = true;
            this.lblServerList.Location = new Point(20, 20);
            this.lblServerList.Text = "Saved Servers:";

            // lstServers
            this.lstServers.FormattingEnabled = true;
            this.lstServers.ItemHeight = 16;
            this.lstServers.Location = new Point(20, 50);
            this.lstServers.Size = new Size(340, 120);

            // txtServer
            this.txtServer.Location = new Point(20, 180);
            this.txtServer.Size = new Size(240, 24);

            // btnAdd
            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new Point(270, 178);
            this.btnAdd.Size = new Size(90, 28);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // btnRemove
            this.btnRemove.Text = "Remove Selected";
            this.btnRemove.Location = new Point(20, 220);
            this.btnRemove.Size = new Size(160, 30);
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);

            // btnSave
            this.btnSave.Text = "Save";
            this.btnSave.Location = new Point(200, 270);
            this.btnSave.Size = new Size(75, 30);
            //this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(285, 270);
            this.btnCancel.Size = new Size(75, 30);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // EditServersForm
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(390, 320);
            this.Controls.Add(this.lblServerList);
            this.Controls.Add(this.lstServers);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Servers";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblServerList;
        private ListBox lstServers;
        private TextBox txtServer;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnSave;
        private Button btnCancel;

        #endregion
    }
}