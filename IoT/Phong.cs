using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public class Phong
    {
        private static int dem = 0;
        private string maPhong ;
        private string maKhoa;
        private string tenPhong;
        private string viTri;
        public Phong()
        {
            
        }
        public Phong(string maPhong, string maKhoa, string tenPhong, string viTri)
        { 
            this.maPhong = maPhong;
            this.tenPhong = tenPhong;
            this.maKhoa = maKhoa;
            this.viTri = viTri;
        }
        Random random = new Random();
        private string setMaPhong()
        {
            string[] listMaPhong =
            {
                "CC001",
                "CC002",
                "CC003",
                "CC004",
                "CC005",
                "CDHA001",
                "CDHA002",
                "CDHA003",
                "CDHA004",
                "CDHA005",
                "DU001",
                "DU002",
                "DU003",
                "DU004",
                "DU005",
                "HSCC001",
                "HSCC002",
                "HSCC003",
                "HSCC004",
                "HSCC005",
                "KB001",
                "KB002",
                "KB003",
                "KB004",
                "KB005",
                "KN001",
                "KN002",
                "KN003",
                "KN004",
                "KN005",
                "KS001",
                "KS002",
                "KS003",
                "KS004",
                "KS005",
                "NCXK001",
                "NCXK002",
                "NCXK003",
                "NCXK004",
                "NCXK005",
                "NT1001",
                "NT1002",
                "NT1003",
                "NT1004",
                "NT1005",
                "NT2001",
                "NT2002",
                "NT2003",
                "NT2004",
                "NT2005",
                "NT3001",
                "NT3002",
                "NT3003",
                "NT3004",
                "NT3005",
                "NTH1001",
                "NTH1002",
                "NTH1003",
                "NTH1004",
                "NTH1005",
                "NTH2001",
                "NTH2002",
                "NTH2003",
                "NTH2004",
                "NTH2005",
                "NTM001",
                "NTM002",
                "NTM003",
                "NTM004",
                "NTM005",
                "NTTN001",
                "NTTN002",
                "NTTN003",
                "NTTN004",
                "NTTN005",
                "PHCN001",
                "PHCN002",
                "PHCN003",
                "PHCN004",
                "PHCN005",
                "PK001",
                "PK002",
                "PK003",
                "PK004",
                "PK005",
                "PT001",
                "PT002",
                "PT003",
                "PT004",
                "PT005",

            };
            return listMaPhong[random.Next(0,listMaPhong.Length)];
        }

        public void insertPhong()
        {
            for (int i = 100; i <= 116; i++)
            {
                //Console.WriteLine(i);
                connection.connect();
                string selectQuery = $"UPDATE benhNhan SET maPhong = '{setMaPhong()}' where maBN = '0{i}BN'";
                SqlCommand command = new SqlCommand(selectQuery, connection.conn);
                command.ExecuteNonQuery();
                connection.disconnect();
            }
        }

        public string getMaPhong()
        {
            return maPhong;
        }
        public string getMaKhoa()
        {
            return maKhoa;
        }
        public string getTenPhong()
        { 
            return tenPhong; 
        }
        public string getViTri()
        {
            return viTri;
        }

        public List<Phong> getPhong() 
        {
            
            connection.connect();
            List<Phong> list = new List<Phong>(); 
            string selectQuery = "SELECT maPhong, maKhoa, tenPhong, viTri FROM phong where maKhoa = @value1 ";
            SqlCommand command = new SqlCommand(selectQuery, connection.conn);
            command.Parameters.AddWithValue("@value1", LogIn.maKhoa);
            //command.Parameters.AddWithValue("@value1", "cc");
            
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Lấy dữ liệu từ mỗi cột của dòng hiện tại
                    string maPhong = reader.GetString(0); // Thay 0 bằng số chỉ mục của cột
                    string maKhoa = reader.GetString(1); // Thay 1 bằng số chỉ mục của cột
                    string tenPhong = reader.GetString(2); // Thay 1 bằng số chỉ mục của cột
                    string viTri = reader.GetString(3);
                    Phong k = new Phong(maPhong, maKhoa, tenPhong, viTri);
                    list.Add(k);
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("dajawdad");
                MessageBox.Show("Error: " + ex.Message);
                
            }
            connection.disconnect();
            return list;
        }

    }
}
