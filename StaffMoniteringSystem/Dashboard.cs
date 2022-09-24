using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Timers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StaffMoniteringSystem
{
    public partial class Dashboard : Form
    {
        public static String checklogin;
       

        class Global
        {
            public static String TokenY;

        }

        public static class ControlID
        {
            public static string TextData { get; set; }
        }

        public Dashboard(string token)
        {
            InitializeComponent();

            chart1.Series["Series1"].Points.AddXY("1", "60");
            chart1.Series["Series1"].Points.AddXY("2", "60");
            chart1.Series["Series1"].Points.AddXY("3", "80");
            chart1.Series["Series1"].Points.AddXY("4", "75");
            chart1.Series["Series1"].Points.AddXY("5", "95");
            chart1.Series["Series1"].Points.AddXY("6", "90");
            chart1.Series["Series1"].Points.AddXY("7", "75");

            chart2.Series["s2"].Points.AddXY("1", "33");
            chart2.Series["s2"].Points.AddXY("2", "45");
            chart2.Series["s2"].Points.AddXY("3", "56");
            chart2.Series["s2"].Points.AddXY("4", "78");




            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            pictureBox3.Image = bm;

            var aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            aTimer.Interval = 600000;
            aTimer.Enabled = true;
            Global.TokenY = token;

            label2.Text = "Hello " + Login.quantity;
 
    }

 
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string mainfolderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.Directory.CreateDirectory(mainfolderpath + "/sm");

            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            // pictureBox3.Image = bm;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var timestamp = DateTime.Now.ToFileTime();
            bm.Save(path + "/sm/" + timestamp + ".jpg", ImageFormat.Jpeg);
            string myFile = @path + "/sm/" + timestamp + ".jpg";

            //  check client
            try
            {
                WebClient clientck = new WebClient();
                clientck.Credentials = CredentialCache.DefaultCredentials;
                clientck.Headers.Add("Token", Global.TokenY.ToString());
                var responseck = clientck.UploadString(@"https://friendsmatrimony.com/api/admin/check-status", "status");
                var jObjectck = JObject.Parse(responseck);
                var status = jObjectck.GetValue("status").ToString();

                //MessageBox.Show(status);

                if (status.ToString() == "True")
                {
                        try
                        {
                       // MessageBox.Show("Ready to Send data");
                        WebClient client = new WebClient();
                             client.Credentials = CredentialCache.DefaultCredentials;
                             client.Headers.Add("Token", Global.TokenY.ToString());
                            client.UploadFile(@"https://friendsmatrimony.com/api/admin/screen-log", "POST", myFile);
                            client.Dispose();
                        string folderPath = @path + "/sm/";
                        Directory.Delete(folderPath, true);
                    }
                       catch (Exception err)
                       {
                            MessageBox.Show(err.Message);
                       }
                }
                else
                {
                  //  MessageBox.Show("You have not logged in, you may logout or in break");
                }

                }catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

          

 



        }



 


        internal static void Show(JObject jObject)
        {
            throw new NotImplementedException();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }



        private bool islogoutsuccess = false;
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            pictureBox3.Image = bm;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var timestamp = DateTime.Now.ToFileTime();
            bm.Save(path + "/sm/" + timestamp + ".jpg", ImageFormat.Jpeg);
            
        }

 

        private void PictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {

        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if (this.WindowState == FormWindowState.Minimized && cursorNotInBar)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                this.Hide();
            }

        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            //  WindowState = FormWindowState.Normal;

            Dashboard dashboard = new Dashboard(Global.TokenY);
            dashboard.Show();
        }

 

        private void Guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(400);
            islogoutsuccess = true;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (islogoutsuccess)
            {
                pcloader.Visible = false;
                this.Close();
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            pcloader.Visible = true;
            pcloader.Dock = DockStyle.Fill;
            Size size = new Size(149, 96);
            pcloader.Size = size;

            backgroundWorker1.RunWorkerAsync();
            WebClient client = new WebClient();

            client.Credentials = CredentialCache.DefaultCredentials;
            client.Headers.Add("Token", Global.TokenY.ToString());

            var response = client.UploadString(@"https://friendsmatrimony.com/api/admin/logout", "logout");
            var result = JsonConvert.DeserializeObject<object>(response);


            var jObject = JObject.Parse(response);

            var message = jObject.GetValue("message").ToString();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = @path + "/sm/";
            if (File.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            //     MessageBox.Show(message.ToString());

            Properties.Settings.Default.tokenUser = "";
            Properties.Settings.Default.displayName = "";
            Properties.Settings.Default.userName = "";
            Properties.Settings.Default.passUser = "";
            Properties.Settings.Default.Save();

            Application.Exit();
        }
    }
}
