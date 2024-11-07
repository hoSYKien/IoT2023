using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    class connection
    {
        //MySqlCommand cmd = null;
        //public static MySqlConnection conn;
        public static SqlConnection conn;
        public static SqlCommand cmd;
        public static SqlDataAdapter adt;
        public static string strConnection = @"Data Source = DESKTOP-LE0LV5H\SQLEXPRESS; Initial Catalog = iot2024; Integrated security = True";
        //public static string strConnection = @"Data Source=171.244.38.118\SQLEXPRESS;Initial Catalog = iot2024; User ID = iot2024; password = iot2024; Encrypt=False";
        //public static string strConnection = "Server = sql6.freesqldatabase.com;Port = 3306; Database = sql6685569; User ID = sql6685569; password = WfZhBZZje9;Convert Zero Datetime=True";
        //public static string strConnection = "Server = 127.0.0.1;Port = 3306; Database = iot; User ID = root; password = ;Convert Zero Datetime=True";
        public static void connect()
        {
            try
            {
                conn = new SqlConnection(strConnection);
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Kết Nối: " + ex.Message);// cần chỉnh sửa
            }
        }

        public static void disconnect() 
        {
            try
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi Ngắt Kết Nối: " + ex.Message);// cần chỉnh sửa
            }
            
        }
            
    }
}
