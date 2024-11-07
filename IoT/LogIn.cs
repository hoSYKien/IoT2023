using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public partial class LogIn : Form  
    {
        string pass;
        Form1 form1;
        Khoa Khoa;  
        public static string maKhoa;
        public LogIn(string pass, Form1 form1, string maKhoa, Khoa khoa)
        {
            InitializeComponent();
            this.form1 = form1; 
            this.pass = pass;
            this.Khoa = khoa;
            //this.Location = new Point();
            this.StartPosition = FormStartPosition.CenterParent;
            LogIn.maKhoa = maKhoa;
            Khoa.maKhoa = maKhoa;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            string x = EncryptionHelper.Decrypt(pass);
            if(x == guna2TextBox1.Text)
            {
                Khoa.checkPass = true;
                this.Khoa.Khoa_Load(sender, e);
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai Mật Khẩu, hãy thử lại", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
