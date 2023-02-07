using MetroFramework.Forms;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaffMoniteringSystem
{
    public partial class Remotedesktop : MetroForm
    {
        public Remotedesktop()
        {
            InitializeComponent();
        }

        private void Remotedesktop_Load(object sender, EventArgs e)
        {
            try {
                GetMyIp();
            }                
            catch (NullReferenceException err)
            {
                MessageBox.Show("Error : " + err.Message, "Some Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
            
            
            }

        private void GetMyIp()
        {
            var Host = Dns.GetHostEntry(Dns.GetHostName());

            foreach(var ip in Host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtYourIp.Text = ip.ToString();
                }
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                rdp.Server = txtClientIp.Text;
                rdp.UserName = txtUserName.Text;

                IMsTscNonScriptable Secured = (IMsTscNonScriptable)rdp.GetOcx();

                Secured.ClearTextPassword = txtPassword.Text;
                Secured.AdvancedSettings2.ClearTextPassword = txtPassword.Text;
                rdp.Connect();
                MessageBox.Show(rdp.ToString());

            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Error : " + err.Message, "Some Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            if (rdp.Connected.ToString() == "1")
                rdp.Disconnect();
        }

        private void TxtClientIp_Leave(object sender, EventArgs e)
        {
            string MachineName = string.Empty;
            try
            {
                string ip = txtClientIp.Text;
                MessageBox.Show(ip);
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(ip);

                MachineName = hostEntry.HostName;
                MessageBox.Show(MachineName);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
