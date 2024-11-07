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
    public partial class LichHen : UserControl
    {
        public LichHen()
        {
            InitializeComponent();
        }
        
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
            if (x > this.Width - check / 2)
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
            this.AutoScrollPosition = new Point(0, y + y);
            //logout.Hide();

        }

        private void LichHen_Load(object sender, EventArgs e)
        {
            
            BacSi bacSi = new BacSi();
            //Console.WriteLine(Khoa.maKhoa + "ghhhhhhhhhh");
            List<BacSi> list;
            if (Khoa.checkPass == true)
            {
                list = bacSi.getListBacSi(Khoa.maKhoa);
            }
            else
            {
                list = bacSi.getListBacSi();
            }
            foreach (UserControl item in arr)
            {
                item.Visible = false;
            }
            arr.Clear();
            //label1.Text = list.Count.ToString();
            foreach (BacSi item in list)
            {
                user u = new user(item);
                addUserControl(u);
            }
                //Khoa.checkPass = false;
        }
    }
}
