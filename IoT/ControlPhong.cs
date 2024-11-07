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
    public partial class ControlPhong : Form
    {
        string maphong;
        public ControlPhong(string maPhong)
        {
            InitializeComponent();
            this.maphong = maPhong;
        }
        private void LoadDataPhong(string query)
        {
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }
        private void controlPhong_Load(object sender, EventArgs e)
        {
            string query = $"exec dbo.slPKP '{this.maphong}'";
            LoadDataPhong(query);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string query = $"update phieuKham set trangThai = 'true' from phieuKham where maPK = '{selectedRow.Cells[0].Value.ToString()}'";
            connection.connect();
            SqlCommand cmd = new SqlCommand(query, connection.conn);
            int row = cmd.ExecuteNonQuery();

            string query2 = $"select dbo.locationPCD ('{selectedRow.Cells[0].Value.ToString()}')";
            SqlCommand cmd2 = new SqlCommand(query, connection.conn);
            SqlDataReader reader = cmd2.ExecuteReader();
            string t = "";
            while (reader.Read())
            {
                t = reader.GetString(0);
            }
            connection.disconnect();

            string query3 = $"select dbo.maDH  ('{selectedRow.Cells[0].Value.ToString()}')";
            SqlCommand cmd3 = new SqlCommand(query, connection.conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            string maDH = "0000503342";
            while (reader.Read())
            {
                maDH = reader.GetString(0);
            }
            connection.disconnect();

            Function.sendData(t, maDH + "_esp32_receive");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string query = $"exec dbo.slPKP '{this.maphong}'";
            LoadDataPhong(query);
        }
    }
}
