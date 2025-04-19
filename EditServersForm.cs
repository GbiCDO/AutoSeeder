using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSeed
{
    public partial class EditServersForm : Form
    {
        private Settings appSettings;

        public EditServersForm()
        {
            InitializeComponent();
            appSettings = Settings.Load();
            lstServers.Items.AddRange(appSettings.ServerList.ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string newServer = txtServer.Text.Trim();
            if (!string.IsNullOrEmpty(newServer) && !lstServers.Items.Contains(newServer))
            {
                lstServers.Items.Add(newServer);
                txtServer.Clear();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstServers.SelectedItem != null)
            {
                lstServers.Items.Remove(lstServers.SelectedItem);
            }
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    appSettings.ServerList = lstServers.Items.Cast<string>().ToList();
        //    appSettings.Save();
        //    this.Close();
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
