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
    public partial class RemotePc : MetroForm
    {
        public RemotePc()
        {
            InitializeComponent();
        }

        private void RemotePc_Load(object sender, EventArgs e)
        {
            try
            {
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

            foreach (var ip in Host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
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

                // axMsRdpClient9NotSafeForScripting1.Server = txtClientIp.Text;
                //  axMsRdpClient9NotSafeForScripting1.UserName = txtUserName.Text;

                //  axMsRdpClient9NotSafeForScripting1.Server = txtClientIp.Text;
                //  axMsRdpClient9NotSafeForScripting1.UserName = txtUserName.Text;
                //  IMsTscNonScriptable secured = (IMsTscNonScriptable)axMsRdpClient9NotSafeForScripting1.GetOcx();
                //  secured.ClearTextPassword = txtPassword.Text;
                //   axMsRdpClient9NotSafeForScripting1.AdvancedSettings8.EnableCredSspSupport = true;
                //   axMsRdpClient9NotSafeForScripting1.Connect();

                Connect();

            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Error : " + err.Message, "Some Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        public void Connect()
        {
  
            try
            {
                axMsRdpClient9NotSafeForScripting1.UserName = @"desktop-htdlras\kogulan";
                axMsRdpClient9NotSafeForScripting1.Domain = "GTCS";
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings7.ClearTextPassword = txtPassword.Text;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.AuthenticationLevel = 2;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.EnableCredSspSupport = true;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.NegotiateSecurityLayer = false;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RDPPort = 3389;
                axMsRdpClient9NotSafeForScripting1.Server = txtClientIp.Text;
                axMsRdpClient9NotSafeForScripting1.RemoteProgram2.RemoteProgramMode = true;

                axMsRdpClient9NotSafeForScripting1.OnConnected += (o, e) =>
                {
                    var m_connectionState = axMsRdpClient9NotSafeForScripting1.Connected;
                    
                    ((ITSRemoteProgram)((IMsRdpClient9)axMsRdpClient9NotSafeForScripting1.GetOcx()).RemoteProgram).ServerStartProgram(@"||startchrome", "", "", true, "", false);
                    //axMsRdpClient9NotSafeForScripting1.RemoteProgram.ServerStartProgram(@"||startchrome", "", "", true, "", false);
                };

                axMsRdpClient9NotSafeForScripting1.AdvancedSettings7.PublicMode = false;
                axMsRdpClient9NotSafeForScripting1.DesktopWidth = SystemInformation.VirtualScreen.Width;
                axMsRdpClient9NotSafeForScripting1.DesktopHeight = SystemInformation.VirtualScreen.Height;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.SmartSizing = true;
                IMsTscNonScriptable secured = (IMsTscNonScriptable)axMsRdpClient9NotSafeForScripting1.GetOcx();
                secured.ClearTextPassword = txtPassword.Text;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings7.ClearTextPassword = txtPassword.Text;

                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RedirectClipboard = true;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RedirectPrinters = true;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RedirectPorts = false;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RedirectSmartCards = true;
                axMsRdpClient9NotSafeForScripting1.AdvancedSettings9.RedirectDrives = true;
                axMsRdpClient9NotSafeForScripting1.Visible = true;
                axMsRdpClient9NotSafeForScripting1.Enabled = true;                  
                axMsRdpClient9NotSafeForScripting1.StartConnected = 1;
                axMsRdpClient9NotSafeForScripting1.Connect();
                var m_connectionState = axMsRdpClient9NotSafeForScripting1.Connected;
              //  MessageBox.Show(m_connectionState.ToString());
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                MessageBox.Show(ex.Message);
            }



        }








            private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
 
            if (axMsRdpClient9NotSafeForScripting1.Connected.ToString() == "1")
                axMsRdpClient9NotSafeForScripting1.Disconnect();
        }
    }
}
