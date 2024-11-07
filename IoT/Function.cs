using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IoT
{
    internal class Function
    {
        public static SqlCommand cmd;
        public static string getID(string tenBang, string tenCot, string key)
        {
            string t = "0000BS";
            try
            {
                
                connection.connect();
                string query = $"SELECT {tenCot} FROM {tenBang}";
                cmd = new SqlCommand(query, connection.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    t = reader.GetString(0);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            t = t.Substring(0, t.Length - 2);
            connection.disconnect();
            int num;
            int.TryParse(t, out num);
            num++;
            string s = num.ToString();
            while (s.Length + 2 < 6)
            {
                s = "0" + s;
            }
            return s + key;
        }
        public static int check(string tenBang, string tenCot, string key)
        {
            int num = 0;
            try
            {

                connection.connect();
                string query = $"SELECT count(*) FROM {tenBang} where {tenCot} = '{key}'";
                cmd = new SqlCommand(query, connection.conn);
                num = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return num;
        }
        public static void updateBacSi( string colunm0, string colunm1, string colunm2, string colunm3, string colunm4, string colunm5, string colunm6)
        {
            try
            {
                connection.connect();
                string query = $"Update bacSi Set tenBS = '{colunm1}', gioiTinh = '{colunm2}', chucVu = '{colunm3}', diaChi = '{colunm4}', soDienThoai = '{colunm5}', email = '{colunm6}' where maBS = '{colunm0}'";
                cmd = new SqlCommand(query, connection.conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên để cập nhật!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
        }
        public static void updateBenhNhan( string colunm0, string colunm1, string colunm2, string colunm3, string colunm4, string colunm5, string colunm6)
        {
            try
            {
                connection.connect();
                string query = $"Update benhNhan Set tenBS = '{colunm1}', gioiTinh = '{colunm2}', chucVu = '{colunm3}', diaChi = '{colunm4}', soDienThoai = '{colunm5}', email = '{colunm6}' where maBS = '{colunm0}'";
                cmd = new SqlCommand(query, connection.conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên để cập nhật!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
        }
        public static void updateThuoc(string colunm0, string colunm1, string colunm2, string colunm3)
        {
            try
            {
                connection.connect();
                string query = $"Update benhNhan Set tenThuoc = '{colunm1}', donVi = '{colunm2}', giaTien = '{colunm3}' where maBS = '{colunm0}'";
                cmd = new SqlCommand(query, connection.conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên để cập nhật!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
        }
        public static void delete(string tenBang, string tenCot, string key)
        {
            try
            {
                connection.connect();
                string query = $"delete from {tenBang} where {tenCot} = '{key}'";
                cmd = new SqlCommand(query, connection.conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("xoá dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên xoá!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Lỗi khi xoá dữ liệu: " + ex.Message);
            }
        }
        public static MqttClient mqttClient;
        //public static string 
        public static void setUpMQTT()
        {
            try
            {
                string host = "1cbe44e0762541ce800bd4fb8b250296.s1.eu.hivemq.cloud";
                mqttClient = new MqttClient(host, 8883, true, MqttSslProtocols.TLSv1_2, null, null);

                //mqttClient.Subscribe(new string[] { "2" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                string clientId = Guid.NewGuid().ToString();
                byte code = mqttClient.Connect(clientId, "IOT_BL", "123456789");
                if (code == 0)
                {
                    //MessageBox.Show("ok");
                    //mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                    
                }
                else
                {
                    //MessageBox.Show("fail");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void sendData(string t, string topic)
        {
            mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            Task.Run(() =>
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish(topic, Encoding.UTF8.GetBytes(t));
                }

            });
        }
    }
}
