using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public partial class Khoa : UserControl
    {
        Form1 form1;
        public Khoa(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        } 
        public static bool checkPass = false;
        public static string maKhoa;
        int x = 0, y = 0;
        int check = 0;
        List<UserControl> arr = new List<UserControl> { };
        private void addUserControl(UserControl usercontrol)
        {
            if (arr.Count != 0)
            {
                x = arr[arr.Count - 1].Location.X + arr[arr.Count - 1].Width + 5;
                check = arr[arr.Count - 1].Location.X;
                y = arr[arr.Count - 1].Location.Y;
            }
            else
            {
                x = 0;
                y = 0;
            }
            if (x > this.Width - check/2)
            {
                //label1.Text = check.ToString();
                x = 0;
                y += arr[arr.Count - 1].Height + 5;
            }
            arr.Add(usercontrol);

            usercontrol.Location = new Point(x, y);
            this.Controls.Add(usercontrol);
            usercontrol.BringToFront();
            int newY = this.VerticalScroll.Value + y;
            // Đặt vị trí cuộn để giữ nguyên vị trí hiển thị
            this.AutoScrollPosition = new Point(0, y+ y);
            //logout.Hide();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void Khoa_Load(object sender, EventArgs e)
        {
            BoPhanChinh boPhanChinh = new BoPhanChinh();
            Phong phong = new Phong();
            if(Khoa.checkPass == false)
            {
                foreach (UserControl item in arr)
                {
                    item.Dispose();
                }
                arr.Clear();
                List<BoPhanChinh> list = boPhanChinh.getListKhoa();
                //label1.Text = list.Count.ToString();
                foreach (BoPhanChinh item in list)
                {
                    Cycle cycle = new Cycle(item, this.form1, this);
                    addUserControl(cycle); 
                }
                
                x = 0; y = 0;check = 0;
                //Khoa.checkPass = true;
            }
            else
            {
                List<Phong> list = phong.getPhong();
                
                foreach (UserControl item in arr)
                {
                    item.Visible = false;
                }
                arr.Clear();
                //label1.Text = list.Count.ToString();
                foreach (Phong item in list)
                {
                    //Console.WriteLine(arr);
                    Cycle cycle = new Cycle(item, this.form1, this);
                    addUserControl(cycle);
                }
                //Khoa.checkPass = false;
                
            }            
        }
    }
}
