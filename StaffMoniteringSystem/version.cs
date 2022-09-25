using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaffMoniteringSystem
{
    public partial class Version : Form
    {
        public Version()
        {
            InitializeComponent();
 
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Guna2CircleButton1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
        }
    }
}
