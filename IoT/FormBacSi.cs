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
    public partial class FormBacSi : UserControl
    {
        private SqlDataAdapter sqlDataAdapter;
        private DataTable dataTable;
        private string check;
        public FormBacSi(string check)
        {
            InitializeComponent();
            this.check = check;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadLichHen()
        {
            try
            {
                connection.connect();
                string t = "";
                string sqlQuery = $"SELECT * FROM lichhen where maKhoa = '{Khoa.maKhoa}'";
                sqlDataAdapter = new SqlDataAdapter(sqlQuery, connection.conn);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                connection.disconnect();
                dataGridView1.Columns[0].Width = 130;
                dataGridView1.Columns[1].Width = 130;
                dataGridView1.Columns[2].Width = 130;
                dataGridView1.Columns[3].Width = 160;
                dataGridView1.Columns[4].Width = 160;
                dataGridView1.Columns[5].Width = 210;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
            
        }

        public void loadBenhNhan()
        {
            connection.connect();
            string sqlQuery = @"EXEC tim_kiem_benh_nhan @maKhoa = " + Khoa.maKhoa;
            sqlDataAdapter = new SqlDataAdapter(sqlQuery, connection.conn);
            dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.disconnect();
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = 130;
            dataGridView1.Columns[2].Width = 130;
            dataGridView1.Columns[3].Width = 160;
            dataGridView1.Columns[4].Width = 160;
            dataGridView1.Columns[5].Width = 210;
        }

        private void FormBacSi_Load(object sender, EventArgs e)
        {
            if(check == "lich hen")
            {
                loadLichHen();
            }
            else if (check == "benh nhan")
            {
                loadBenhNhan();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
