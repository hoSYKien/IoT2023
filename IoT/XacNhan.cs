using Guna.UI2.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IoT
{
    public partial class XacNhan : Form
    {
        string maPCD;
        public XacNhan(string maPCD)
        {
            InitializeComponent();
            this.maPCD = maPCD;
        }

        private void XacNhan_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string query = $"Update phieuChuanDoan set trangThai = 'true' from phieuChuanDoan where maPCD = '{this.maPCD}'";
            try
            {
                // Thực hiện tìm kiếm và thêm kết quả vào results
                if (connection.conn.State == ConnectionState.Closed)
                    connection.connect();
                SqlCommand cmd = new SqlCommand(query, connection.conn);
                int row = cmd.ExecuteNonQuery();
                if (connection.conn.State == ConnectionState.Open)
                    connection.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            query = $"select dbo.maDH2 ('{this.maPCD}')";
            string maDH = "0000503380";
            try
            {
                // Thực hiện tìm kiếm và thêm kết quả vào results
                if (connection.conn.State == ConnectionState.Closed)
                    connection.connect();
                SqlCommand cmd = new SqlCommand(query, connection.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    maDH = reader.GetString(0);
                }
                if (connection.conn.State == ConnectionState.Open)
                    connection.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Function.sendData("#", maDH + "_esp32_receive");
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
