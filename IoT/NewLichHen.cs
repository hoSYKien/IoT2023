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
    public partial class NewLichHen : Form
    {
        string maBS;
        string ngay;
        string gio;
        public NewLichHen(string maBS, string ngay, string gio)
        {
            InitializeComponent();
            this.maBS = maBS;
            this.ngay = ngay;
            this.gio = gio;
        }
        SqlCommand cmd = null;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            connection.connect();
            string query = $"insert into lichHen values ('{maBS}', '{guna2TextBox1.Text}', '{ngay}', '{gio}','')";
            try
            {
                cmd = new SqlCommand(query, connection.conn);
                int row = cmd.ExecuteNonQuery();
                MessageBox.Show("Đặt Lịch Thành Công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: Mã Bệnh Nhân Của Bạn Không Tồn Tại! Hãy Nhập Lai.");// cần chỉnh sửa
            }
            connection.disconnect();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewLichHen_Load(object sender, EventArgs e)
        {
            
        }
    }
}
