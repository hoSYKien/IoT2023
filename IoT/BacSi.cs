using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Theraot.Core;
namespace IoT
{
    public class BacSi
    {
        SqlCommand cmd = null;
        private string maBS;
        private string tenBS;
        private string gioiTinh;
        private string chucVu;
        private string maKhoa;
        private string diaChi;
        private string sdt;
        private string email;
        public string getMaKhoa()
        {
            return maKhoa ;
        }
        public string getChucVu()
        {
            return chucVu;
        }
        public string getTenBS()
        {
            return tenBS;
        }
        public string getGioiTinh()
        {
            return this.gioiTinh;
        }
        public string getMaBacSi()
        {
            connection.connect();
            string query = "SELECT maBS FROM bacsi";
            cmd = new SqlCommand(query, connection.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            string t = "123321";
            while (reader.Read())
            {
                t = reader.GetString(0);
            }
            t = t.Substring(0, t.Length - 2);
            connection.disconnect();
            int num;
            int.TryParse(t, out num);
            num++;
            string s = num.ToString();
            while(s.Length + 2 < 6)
            {
                s = "0" + s;
            }
            return s + "BS";
        }
        public string getMaBacSi2()
        {
            return this.maBS;
        }
        public string getTenKhoa()
        {
            connection.connect();
            using (SqlCommand command = new SqlCommand("SELECT dbo.getTenKhoa(@MaKhoa)", connection.conn))
            {
                //Console.WriteLine("dawadư"+   this.maKhoa + "    dâd2dưư\n\n");
                // Thêm tham số @MaKhoa vào truy vấn
                command.Parameters.AddWithValue("@MaKhoa", this.maKhoa);
                
                // Thực thi truy vấn và lấy kết quả
                string tenKhoa = (string)command.ExecuteScalar();
                return tenKhoa;
            }
        }
        public BacSi()
        {
            this.maBS = "maBS";
            this.tenBS = "tenBS";
            this.gioiTinh = "Nam";
            this.chucVu = "chucVu";
            this.maKhoa = "maKhoa";
            this.diaChi = "diaChi";
            this.sdt = "sdt";
            this.email = "email";
        }
        public BacSi(string maBS, string tenBS, string gioiTinh, string chucVu, string maKhoa, string diaChi, string sdt, string email)
        {
            this.maBS= maBS;
            this.tenBS= tenBS;
            this.gioiTinh=gioiTinh;
            this.chucVu= chucVu;
            this.maKhoa= maKhoa;
            this.diaChi=diaChi;
            this.sdt=sdt;
            this.email = email;
        }
        public BacSi(string tenBS, string gioiTinh, string chucVu, string diaChi, string sdt, string email)
        {
            this.maBS = getMaBacSi();
            this.tenBS= tenBS;
            this.gioiTinh = gioiTinh;
            this.chucVu= chucVu;
            this.diaChi = diaChi;
            this.sdt= sdt;
            this.email = email;
            insertSQLToTableBacSi();
        }

        public void insertSQLToTableBacSi()
        {
            connection.connect();
            string insertQuery = $"INSERT bacsi (maBS, tenBS, gioiTinh, chucVu, diaChi, soDienThoai, email) VALUES(N'{this.maBS}', N'{this.tenBS}', N'{this.gioiTinh}', N'{this.chucVu}', N'{this.diaChi}', N'{this.sdt}', N'{this.email}')";
            //string insertQuery = "INSERT INTO `bacsi`(`maBS`, `tenBS`, `gioiTinh`, `chucVu`, `maKhoa`, `diaChi`, `soDienThoai`, `email`) VALUES('{value1}', 'Hồ Sỹ Cường', 'Nam', 'Phó Giám Đốc', 'CC', 'Nghệ An', '0865734636', 'hosyCuong2011@gmail.com') ";
            try
            {
                cmd = new SqlCommand(insertQuery, connection.conn);
                int row = cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm Thành Công");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);// cần chỉnh sửa
            }
            connection.disconnect();
        }
        public List<BacSi> getListBacSi(string maKhoa = "")
        {
            List<BacSi> list = new List<BacSi>();
            connection.connect();
            //Console.WriteLine(maKhoa + "akhwdgak,dwhakdbwa");
            string selectQuery = $"SELECT * from bacsi where maKhoa = '{maKhoa}' and capBac = 1 ";
            if( maKhoa == "")
            {
                selectQuery = $"SELECT * from bacsi where capBac = 1 ";
            }
            SqlCommand command = new SqlCommand(selectQuery, connection.conn);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string maBS = reader.GetString(0);
                    string tenBS = reader.GetString(1);
                    string gioiTinh = reader.GetString(2);
                    string chucVu = reader.GetString(3);
                    string maKhoa2 = reader.GetString(4);
                    string diaChi = reader.GetString(5);
                    string sdt = reader.GetString(6);
                    string email = reader.GetString(7);
                    BacSi bs = new BacSi(maBS, tenBS, gioiTinh, chucVu, maKhoa2, diaChi, sdt, email);
                    list.Add(bs);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi 1: " + ex.Message);
            }
            return list;
        }
    }
}
