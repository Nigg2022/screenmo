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

            SetStartup(); //This function will set your app in the registry to run on startup.
            MinimizeApp("-minimized");

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


            //MessageBox.Show(CheckStatus());
            if (CheckStatus() == "True")
            {
                pictureBox7.Image = StaffMoniteringSystem.Properties.Resources.circle_24;
            }
            else
            {
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = CheckStatus() + "! You are not loggedin friendsmatrimony.com Website, System cannot send Screenshots to server. ";
                notifyIcon1.ShowBalloonTip(500);
                Hide();
                pictureBox7.Image = StaffMoniteringSystem.Properties.Resources.circle_24__1_;
            }
            






        }



 

        // ******************** system startup

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length != 0)
            {
                Application.Run(new Dashboard(args[0]));
            }
            else
            {
                Application.Run(new Dashboard("normalState"));
            }
        }


        public void MinimizeApp(string parameter)
        {
 
            if (parameter == "-minimized")
            {
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "Screen Monitoring started & running in background...";
                notifyIcon1.ShowBalloonTip(500);
                Hide();
            }

        }

        private void SetStartup()
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            key.SetValue("StaffMoniteringSystem", Application.ExecutablePath.ToString());
            string ApplicationPath = "\"" + Application.ExecutablePath.ToString() + "\" -minimized";
            key.SetValue("StaffMoniteringSystem", ApplicationPath);
            key.Close();

        }

        // *********************** system startup





         public static String CheckStatus() {
            String status = "";
            try
            {
                WebClient clientck = new WebClient();
                clientck.Credentials = CredentialCache.DefaultCredentials;
                clientck.Headers.Add("Token", Global.TokenY.ToString());
                var responseck = clientck.UploadString(@"https://friendsmatrimony.com/api/admin/check-status", "status");
                var jObjectck = JObject.Parse(responseck);
                status = jObjectck.GetValue("status").ToString();


                return status.ToString(); 

            }
            catch (Exception error)
            {


            }
            return status;
        }


      



        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string mainfolderpath = Path.GetPathRoot(Environment.SystemDirectory);
            System.IO.Directory.CreateDirectory(mainfolderpath + "/sm");

            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            // pictureBox3.Image = bm;

            string path = Path.GetPathRoot(Environment.SystemDirectory);
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

            //string path = Path.GetPathRoot(Environment.SystemDirectory);
            //var timestamp = DateTime.Now.ToFileTime();
            //bm.Save(path + "/sm/" + timestamp + ".jpg", ImageFormat.Jpeg);

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
              WindowState = FormWindowState.Normal;

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
            string path = Path.GetPathRoot(Environment.SystemDirectory);
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

        private void Guna2Button3_Click(object sender, EventArgs e)
        {
            var verion = new Version();
            verion.Show();
        }


        private Point windowLocation;

        private void Guna2GradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.windowLocation = e.Location;
        }

        private void Guna2GradientPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Refers to the Form location (or whatever you trigger the event on)
                this.Location = new Point(
                    (this.Location.X - windowLocation.X) + e.X,
                    (this.Location.Y - windowLocation.Y) + e.Y
                );

                this.Update();
            }
        }

        private void Guna2Button4_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            StreamWriter File = new StreamWriter(path+"/data.dat");
        //    StreamWriter remoteFile = new StreamWriter(@"\\192.168.8.173\Users\Default\Desktop");

        //    remoteFile.Write(Login.quantity + "," + Global.TokenY.ToString());
       //     remoteFile.Close();
            File.Write(Login.quantity +","+ Global.TokenY.ToString());
            File.Close();

        }

        private void Guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                guna2CirclePictureBox1.Image = new Bitmap(open.FileName);
            
            }
        }

        private void Label9_Click(object sender, EventArgs e)
        {
            var network = new Network();
            network.Show();
        }
    }
}
