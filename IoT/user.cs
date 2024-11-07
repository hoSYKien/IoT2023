using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public partial class user : UserControl
    {
        BacSi bacSi;
        string buttonGio;

        public user(BacSi bacSi)
        {
            InitializeComponent();
            this.bacSi = bacSi;
            Thread thread = new Thread(ChangeButtonValue);
            thread.Start();
            AttachButtonClickEvent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
                
        }
        List<Guna2Button> guna2Buttons = new List<Guna2Button>();
        private void addList()
        {
            //Console.WriteLine("daiwdnawdnawndkjawuidiawndjn dakj");
            guna2Buttons.Add(guna2Button1);
            guna2Buttons.Add(guna2Button2);
            guna2Buttons.Add(guna2Button3);
            guna2Buttons.Add(guna2Button4);
            guna2Buttons.Add(guna2Button5);
            guna2Buttons.Add(guna2Button6);
            guna2Buttons.Add(guna2Button7);
            guna2Buttons.Add(guna2Button8);
            guna2Buttons.Add(guna2Button9);
            guna2Buttons.Add(guna2Button10);
            guna2Buttons.Add(guna2Button11);
            guna2Buttons.Add(guna2Button12);
            guna2Buttons.Add(guna2Button13);
            guna2Buttons.Add(guna2Button14);
            guna2Buttons.Add(guna2Button15);
            guna2Buttons.Add(guna2Button16);
        }
        private void AttachButtonClickEvent()
        {
            guna2Button1.Click += Button_Click;
            guna2Button2.Click += Button_Click;
            guna2Button3.Click += Button_Click;
            guna2Button4.Click += Button_Click;
            guna2Button5.Click += Button_Click;
            guna2Button6.Click += Button_Click;
            guna2Button7.Click += Button_Click;
            guna2Button8.Click += Button_Click;
            guna2Button9.Click += Button_Click;
            guna2Button10.Click += Button_Click;
            guna2Button11.Click += Button_Click;
            guna2Button12.Click += Button_Click;
            guna2Button13.Click += Button_Click;
            guna2Button14.Click += Button_Click;
            guna2Button15.Click += Button_Click;
            guna2Button16.Click += Button_Click;
        }
        private Guna2Button selectedButton;
        private void Button_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = sender as Guna2Button;
            if (clickedButton != null)
            {
                if (selectedButton != null)
                {
                    // Đặt màu của nút trước đó trở lại màu ban đầu
                    selectedButton.FillColor = Color.Transparent;
                }
                clickedButton.FillColor = Color.DarkCyan;
                selectedButton = clickedButton;
                
                NewLichHen newLichHen = new NewLichHen(bacSi.getMaBacSi2(), dateTimePicker1.Value.ToString("yyyy-MM-dd"), clickedButton.Text.Substring(0, 5) );
                newLichHen.ShowDialog();
                UpdateButtonValue("djjdjd");
            }
        }
        private void user_Load(object sender, EventArgs e)
        {
            addList();
            //Console.WriteLine(guna2Buttons.Count);
            guna2Panel1.Visible = true;
            guna2Panel1.Location = new System.Drawing.Point(5, 35);
            guna2Panel2.Visible = false;
            if (this.bacSi.getGioiTinh() == "Nam")
            {
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
            }
            label1.Text = bacSi.getTenBS();
            label2.Text = bacSi.getTenKhoa();
            //label2.Text = bacSi.getMaKhoa();
            label5.Text = bacSi.getChucVu();
            
        }
        private void ChangeButtonValue()
        {
            // Thay đổi giá trị của Guna2Button
            UpdateButtonValue("djjdjd");
        }
        
        private void UpdateButtonValue(string newValue)
        {
            // Kiểm tra xem có cần phải Invoke hay không
            if (InvokeRequired)
            {
                // Nếu cần, gọi lại phương thức này trên luồng giao diện người dùng chính
                Invoke(new Action<string>(UpdateButtonValue), newValue);
                return;
            }

            foreach(Guna2Button button in guna2Buttons)
            {
                button.FillColor = Color.Transparent;
                if (dateTimePicker1.Value.Date < DateTime.Now.Date)
                {
                    button.Enabled = false;
                }
            }
            
            // -------------Kết nối đến cơ sở dữ liệu
            connection.connect();

            // Chuỗi truy vấn SQL
            string query = $"SELECT * FROM lichhen WHERE maBS = '{bacSi.getMaBacSi2()}' AND ngay = '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}'";
            //Console.WriteLine(bacSi.getMaBacSi2());
            //MessageBox.Show(bacSi.getMaBacSi());
            // Thực thi truy vấn
            using (SqlCommand command = new SqlCommand(query, connection.conn))
            {//Console.WriteLine(bacSi.getMaBacSi2());
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    //connection.disconnect();
                    while (reader.Read())
                    {
                        //Console.WriteLine(i++.ToString() +"\n") ;
                        string gio = reader.GetString(3);
                        //int i = 0;
                        //bool check = false;
                        foreach(Guna2Button button in guna2Buttons)
                        {
                            //i++;
                            buttonGio = button.Text.Substring(0, 5);

                            if (gio == buttonGio)
                            {
                                //check = true;
                                //button.Enabled = false;
                                button.FillColor = Color.OrangeRed;
                                //Console.WriteLine("bat cu thu gi");
                                button.Enabled = true;
                            }

                            
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Lỗinnnn: " + ex.Message);
                    //Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Đóng kết nối sau khi hoàn thành
                    //connection.disconnect();
                }
            }
            
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Xử lý khi người dùng đang kéo thanh cuộn
            if (listBox1.Text == "Ca Sáng")
            {
                guna2Panel1.Visible = true;
                guna2Panel1.Location = new System.Drawing.Point(5, 35);
                guna2Panel2.Visible = false;
            }
            else
            {
                guna2Panel1.Visible = false;
                guna2Panel2.Location = new System.Drawing.Point(5, 35);
                guna2Panel2.Visible = true;
            }
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_RightToLeftChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            

        }

        private void listBox1_LocationChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void listBox1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (e.Type == ScrollEventType.ThumbTrack)
                {
                    
                }
                else if (e.Type == ScrollEventType.EndScroll)
                {
                    // Xử lý khi người dùng đã kết thúc kéo thanh cuộn
                    
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateButtonValue("djjdjd");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BacSi bs;
            try
            {
                connection.connect();
                string query = $"exec slBS N'{comboBox1.Text}'";
                SqlCommand command = new SqlCommand(query, connection.conn);
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
                    bacSi = new BacSi(maBS, tenBS, gioiTinh, chucVu, maKhoa2, diaChi, sdt, email);
                    
                }
                user_Load(sender, e);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
