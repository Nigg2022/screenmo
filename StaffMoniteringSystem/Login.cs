using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace StaffMoniteringSystem
{
    public partial class Login : Form
    {
        public static String quantity;
        public static String quantity_to;

 

        public  Login()
        {

            InitializeComponent();

            Load += new EventHandler(Login_Load);


        }




        private async void Login_Load(object sender, EventArgs e)
        {

            label7.Text = "We are working you to get inside Please wait.";

            var useName = Properties.Settings.Default.userName;
            var passWord = Properties.Settings.Default.passUser;
            var userToken = Properties.Settings.Default.tokenUser;
            var displayName = Properties.Settings.Default.displayName;

          
            if (useName != "" && passWord != "")
            {
                String token = await RunAsync(useName, passWord);

                if (token == "")
                {
                    label7.Text = "Login Fail, Contect Administrator.";
                }
                else
                {
                    this.Hide();
                    var Dashboard = new Dashboard(token);
                    Dashboard.Closed += (s, args) => this.Close();
                    Dashboard.Show();
                    
                }



            }
            else
            {
                label7.Text = "Please Login to get inside.";
            }
        }





        private async void Button1_Click(object sender, EventArgs e)
        {

            var ur = "";
            var pas = "";

            if (Properties.Settings.Default.userName == "" && Properties.Settings.Default.passUser =="")
            {

                ur = txtUsername.Text;
                pas = txtPassword.Text;

            }
            else
            {
                
                ur = Properties.Settings.Default.userName;
                pas = Properties.Settings.Default.passUser;
            }

            if (ur == "" || pas == "")
            {
                MessageBox.Show("Email or Password Cannot be Empty");
            }
            else
            {
                String token = await RunAsync(ur, pas);

                if (token == "")
                {
                    label7.Text = "Login Fail, Contect Administrator.";
                }
                else
                {
                    // save user & pass to setting
                    Properties.Settings.Default.userName = ur;
                    Properties.Settings.Default.passUser = pas;
                    
                    Properties.Settings.Default.Save();

                    this.Hide();
                    var Dashboard = new Dashboard(token);

                    Dashboard.Closed += (s, args) => this.Close();
                    Dashboard.Show();
                }

            }
        }



        static async Task<string> RunAsync(String username_f, String password_f)
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://friendsmatrimony.com/api/admin/auth");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //setup login data

                var username = username_f.ToString();
                var password = password_f.ToString();
                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("email", username),
                new KeyValuePair<string, string>("password", password),
                });

                //send request
                HttpResponseMessage responseMessage = await client.PostAsync("", formContent);
                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                
                var jObject = JObject.Parse(responseJson);
                
                var token = jObject.GetValue("token").ToString();


                JObject jo = JObject.Parse(responseJson);
                var mname = (string)jo.SelectToken("manager.name");

                quantity = mname;
                quantity_to = token;

                Properties.Settings.Default.tokenUser = token;
                Properties.Settings.Default.displayName = mname;
                
                Properties.Settings.Default.Save();

                if (token.ToString() != "")
                {
                    return token.ToString();
                }
                else {
                    return "";
                }

            }
            
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
 
        private void PictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

 


    }
}
