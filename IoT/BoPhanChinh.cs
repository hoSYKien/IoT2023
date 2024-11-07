using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public class BoPhanChinh
    {
        SqlCommand cmd = null;
        private string maKhoa;
        private string tenKhoa;
        private string viTri;
        private string pass;
        public BoPhanChinh()
        {

        }

        public BoPhanChinh(string maKhoa, string tenKhoa, string viTri, string pass)
        {
            this.maKhoa = maKhoa;
            this.tenKhoa = tenKhoa;
            this.viTri = viTri;
            this.pass = pass;
        }
        public List<BoPhanChinh> getListKhoa()
        {
            connection.connect();
            List<BoPhanChinh> list = new List<BoPhanChinh>();
            string selectQuery = "SELECT maKhoa, tenKhoa, viTri, pass FROM khoa";
            SqlCommand command = new SqlCommand(selectQuery, connection.conn);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    // Lấy dữ liệu từ mỗi cột của dòng hiện tại
                    string maKhoa = reader.GetString(0); // Thay 0 bằng số chỉ mục của cột
                    string tenKhoa = reader.GetString(1); // Thay 1 bằng số chỉ mục của cột
                    string viTri = reader.GetString(2); // Thay 1 bằng số chỉ mục của cột
                    string pass = reader.GetString(3); 
                    BoPhanChinh k = new BoPhanChinh(maKhoa, tenKhoa, viTri, pass);
                    list.Add(k);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            connection.disconnect();
            return list;
        }
        public string getPass() 
        {
            return this.pass;
        }
        public string getMaKhoa()
        {
            return this.maKhoa;
        }
        public string getTenKhoa()
        {
            return this.tenKhoa;
        }
        public string getViTri()
        {
            return this.viTri;
        }
    }

}
