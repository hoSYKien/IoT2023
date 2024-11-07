using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IoT
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        string[] listMaBS =
        {
            "0001BS", "0004BS", "0008BS", "0013BS", "0023BS", "0024BS",
            "0025BS", "0026BS", "0028BS", "0035BS", "0037BS", "0038BS"
        };
        string[] listMaBN =
        {

            "0001BN", "0002BN", "0003BN", "0004BN", "0005BN", "0006BN", "0007BN", "0008BN",
            "0009BN", "0010BN", "0011BN", "0012BN", "0013BN", "0014BN", "0015BN", "0016BN",
            "0017BN", "0018BN", "0019BN", "0020BN", "0021BN", "0022BN", "0023BN", "0024BN",
            "0025BN", "0026BN", "0027BN", "0028BN", "0029BN", "0030BN", "0031BN", "0032BN",
            "0033BN", "0034BN", "0035BN", "0036BN", "0037BN", "0038BN", "0039BN", "0040BN",
            "0041BN", "0042BN", "0043BN", "0044BN", "0045BN", "0046BN", "0047BN", "0048BN",
            "0049BN", "0050BN", "0051BN", "0052BN", "0053BN", "0054BN", "0055BN", "0056BN",
            "0057BN", "0058BN", "0059BN", "0060BN", "0061BN", "0062BN", "0063BN", "0064BN",
            "0065BN", "0066BN", "0067BN", "0068BN", "0069BN", "0070BN", "0071BN", "0072BN",
            "0073BN", "0074BN", "0075BN", "0076BN", "0077BN", "0078BN", "0079BN", "0080BN",
            "0081BN", "0082BN", "0083BN", "0084BN", "0085BN", "0086BN", "0087BN", "0088BN",
            "0089BN", "0090BN", "0091BN", "0092BN", "0093BN", "0094BN", "0095BN", "0096BN",
            "0097BN", "0098BN", "0099BN", "0100BN", "0101BN", "0102BN", "0103BN", "0104BN",
            "0105BN", "0106BN", "0107BN", "0108BN", "0109BN", "0110BN", "0111BN", "0112BN",
            "0113BN", "0114BN", "0115BN", "0116BN", "0117BN"
        };
        string[] listGio =
        {
            "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
            "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00"
        };
        string[] listNgay =
        {
            /*"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16",
            "17", "18", "19", "20", "21",*/ "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"
        };
        Random Random = new Random();
        string getMaBS()
        {
            return listMaBS[Random.Next(0, listMaBS.Length)];
        }
        string getMaBN()
        {
            return listMaBN[Random.Next(0, listMaBN.Length)];
        }
        string getGio()
        {
            return listGio[Random.Next(0, listGio.Length)];
        }
        string getNgay()
        {
            return listNgay[Random.Next(0, listNgay.Length)];
        }
        SqlCommand cmd = null;
        void bruh()
        {
            for (int i = 0; i < 50; i++)
            {
                connection.connect();
                string query = $"insert into lichHen values ('{getMaBS()}', '{getMaBN()}', '2024-03-{getNgay()}', '{getGio()}','')";
                try
                {
                    cmd = new SqlCommand(query, connection.conn);
                    int row = cmd.ExecuteNonQuery();
                    //MessageBox.Show("Thêm Thành Công");
                    Console.WriteLine("thanh cong");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Lỗi: " + ex.Message);// cần chỉnh sửa
                    Console.WriteLine("cook");
                }
                connection.disconnect();
            }
        }
        private void Start_Load(object sender, EventArgs e)
        {
            //bruh();
            Thread workerThread = new Thread(DoWork);
            workerThread.Start(); 
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void UpdatePictureBoxLocation(Point newPosition)
        {
            if (pictureBox2.InvokeRequired)
            {
                pictureBox2.Invoke((MethodInvoker)(() => pictureBox2.Location = newPosition));
            }
            else
            {
                pictureBox2.Location = newPosition;
            }
        }
        private void DoWork()
        {
            // Điều này là một ví dụ đơn giản về công việc, 
            // trong thực tế, bạn sẽ thay thế nó bằng công việc thực tế của bạn
            int x = pictureBox2.Location.X;
            int y = pictureBox2.Location.Y;
            for (int i = 0; i <= 100; i++)
            {
                // Simulate work by sleeping the thread
                Thread.Sleep(1);
                UpdatePictureBoxLocation(new Point(x + 2 * i, y));
                // Invoke để đảm bảo cập nhật ProgressBar được thực hiện trong luồng UI chính
                Invoke(new Action(() => guna2ProgressBar1.Value = i));
            }
            
        }
        private void guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }
        Form1 form1;
        private void guna2ProgressBar1_ValueChanged_1(object sender, EventArgs e)
        {
            if (guna2ProgressBar1.Value == 1)
            {
                form1 = new Form1();
            }
            if(guna2ProgressBar1.Value == 100)
            {
                try
                {
                    this.Hide();
                    form1.ShowDialog();
                    this.Close();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }
    }
}
