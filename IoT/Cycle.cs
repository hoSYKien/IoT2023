using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public partial class Cycle : UserControl
    {
        string pass;
        Form1 form1;
        Khoa khoa;
        public Cycle(BoPhanChinh x, Form1 form1, Khoa khoa)
        {
            InitializeComponent();
            this.form1 = form1;
            label5.Text = x.getTenKhoa();
            label1.Text = x.getMaKhoa();
            label2.Text = x.getViTri();
            pass = x.getPass();
            this.khoa = khoa;
        }

        public Cycle(Phong x, Form1 form1, Khoa khoa) 
        {
            InitializeComponent();
            this.form1 = form1;
            label5.Text = x.getTenPhong();
            label1.Text = x.getMaPhong();
            label2.Text = x.getViTri();
            this.khoa = khoa;
        }

        private void Cycle_Load(object sender, EventArgs e)
        {
            label5.Location = new System.Drawing.Point((panel1.Width - label5.Width) / 2, (panel1.Height - label5.Height) / 2); // Đặt vị trí Label ở trung tâm Panel
            if(Khoa.checkPass == true)
            {
                label5.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            }
            else
            {
                label5.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            }
            
            connection.connect();
            SqlCommand command = new SqlCommand("SELECT dbo.getSoBacSi(@maKhoa)", connection.conn);
            command.Parameters.AddWithValue("@maKhoa", label1.Text);
            // ExecuteScalar sử dụng khi bạn mong đợi kết quả trả về là một giá trị duy nhất.
            int soBacSi = (int)command.ExecuteScalar();
            label3.Text = soBacSi.ToString();
            connection.disconnect();

            connection.connect();
            command = new SqlCommand("SELECT dbo.getSoPhong(@maKhoa)", connection.conn);
            command.Parameters.AddWithValue("@maKhoa", label1.Text);
            // ExecuteScalar sử dụng khi bạn mong đợi kết quả trả về là một giá trị duy nhất.
            int soPhong = (int)command.ExecuteScalar();
            label4.Text = soPhong.ToString();
            connection.disconnect();
        }
        Color color = Color.SkyBlue;
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(color, 1);
            g.DrawLine(pen, 0, panel2.Height / 2, panel2.Width, panel2.Height / 2);
            pen = new Pen(color, 1);
            g.DrawLine(pen, panel2.Width / 4, 0, panel2.Width / 4, panel2.Height);
            pen.Dispose();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (Khoa.checkPass == false)
            {
                LogIn logIn = new LogIn(pass, this.form1, label1.Text, this.khoa);
                logIn.ShowDialog();
                label1.AutoSize = true;
            }
            else
            {
                ControlPhong controlPhong = new ControlPhong(label1.Text);
                this.form1.Hide();
                controlPhong.ShowDialog();
                this.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
