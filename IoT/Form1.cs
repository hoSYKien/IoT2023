using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySqlConnector;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IoT
{
    public partial class Form1 : Form
    {
        string maDH2 ="";
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            // Tạo một luồng mới
            Thread thread = new Thread(new ThreadStart(DoWork));
            Function.setUpMQTT();
            //comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // Khởi động luồng
            thread.Start();
            
        }
        SqlCommand cmd;
        private void DoWork()
        {
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            
        }
        private string tenBenhNhan;
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Xử lý khi có phím được nhấn và đã thả
            // e.KeyChar chứa ký tự của phím được nhấn
            if (e.KeyCode == Keys.Enter && label1.Text.Length >= 10)
            {
                try
                {
                    connection.connect();
                    string query = $"select dbo.tenBenhNhan('{label1.Text}')";
                    SqlCommand cmd = new SqlCommand(query, connection.conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Console.WriteLine();
                    while (reader.Read())
                    {
                        // Lấy dữ liệu từ cột đầu tiên của kết quả truy vấn
                        tenBenhNhan = reader.GetString(0);
                        //Console.WriteLine(tenBenhNhan);
                    }
                    if (tenBenhNhan == "Khong Tim Thay MaDH")
                    {
                        Console.WriteLine(label1.Text);
                        this.Alert("Không Tìm Thấy Mã ĐH", FormAlert.enmType.Warning);
                        this.Focus();
                        label1.Text = "";
                    }
                    else
                    {
                        this.Alert(tenBenhNhan, FormAlert.enmType.Success);
                        this.Focus();
                        medicalRecord(label1.Text);
                        label1.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message, FormAlert.enmType.Error);
                }
                


            }
            else if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Chuyển đổi mã phím thành ký tự tương ứng và hiển thị trên label1
                label1.Text = "";

            }
            if(e.KeyCode != Keys.Enter) label1.Text += Convert.ToChar(e.KeyValue);//0000503380
            
            
            

        }
        //0000503380

        string[] ranMaBS =
        {
            "0001BS",
            "0004BS",
            "0008BS",
            "0013BS",
            "0023BS",
            "0024BS",
            "0025BS",
            "0026BS",
            "0028BS",
            "0035BS",
            "0037BS",
            "0038BS"
        };
        string[] randLoca =
        {
            "0001A3",
            "0004A5",
            "0008B4",
            "0013C6",
            "0023B2",
            "0024B1",
            "0025A6",
        };
        Random rand = new Random();
        private void medicalRecord(string maDH)
        {
            maDH2 = maDH;
            int dem = -1;
            string query2 = $"select dbo.sltPCD('{maDH}')";
            try
            {
                connection.connect();
                cmd = new SqlCommand(query2, connection.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dem = reader.GetInt32(0);
                }
                connection.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi1: " + ex.Message);// cần chỉnh sửa
            }
            
            if(dem <= 0)
            {
                int x = 0;
                string getRandMaBS = ranMaBS[rand.Next(0, ranMaBS.Length)];
                string query = $"EXEC dbo.khoiTaoBenhNhan '{Function.getID("phieuChuanDoan", "maPCD", "CD")}','{getRandMaBS}','{maDH}'";
                try 
                {
                    connection.connect();
                    cmd = new SqlCommand(query, connection.conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       x  = reader.GetInt32(6);
                    }
                    MessageBox.Show("Thêm Thành Công");
                    Function.sendData(randLoca[rand.Next(0,randLoca.Length)] + " " + x.ToString(), maDH + "_esp32_receive");
                    
                    
                    connection.disconnect();
                    //reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi2: " + ex.Message);// cần chỉnh sửa
                }
                connection.disconnect();
            }
            
        }
        public static event EventHandler MyEvent;
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Show();
            panel3.Hide();
            //guna2PictureBox3.Location = new System.Drawing.Point(panel1.Height - 81, panel1.Width - 91);
            //this.SetDesktopLocation(10, 10);
            guna2PictureBox2_Click(sender, e);
            // Khởi tạo đối tượng SerialPort và cấu hình các tham số cần thiết
            //LoadData("select maPCD as 'ID', maBS as 'Mã Bác Sĩ', maBN as 'Mã Bệnh Nhân', ngayCD as 'Ngày Chuẩn Đoán', tongTien as 'Thành Tiền', trangThai as 'Trạng Thái', stt from phieuChuanDoan");
        }
        

        private void addUserControl(UserControl usercontrol)
        {
            usercontrol.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(usercontrol);
            usercontrol.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //BacSi BS = new BacSi("Hồ Sỹ Kiên", "Nam", "Giám Đốc", "CC", "Nghệ An","0865734626", "hosykien11a3@gmail.com");

        }

        
        private void LoadData(string query)
        {
            // Thay đổi chuỗi kết nối cho phù hợp với cơ sở dữ liệu của bạn
            // Thay đổi YourTable thành tên bảng thực tế của bạn
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }

        }
        private void LoadData2(string query)
        {
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }
        private void LoadDataBenh(string query)
        {
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView3.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }
        private void LoadDataThuoc(string query)
        {
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView4.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }
        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private string getIDKhoa(string t)
        {
            string c = "";
            if (t == "Cấp Cứu")
                c = "CC";
            else if (t == "Chuẩn Đoán Hình Ảnh")
                c = "CDHA";
            else if (t == "Dị Ứng")
                c = "DU";
            else if (t == "Hồi Sức Cấp Cứu")
                c = "HSCC";
            else if (t == "Khám Bệnh")
                c = "KB";
            else if (t == "Nhi")
                c = "KN";
            else if (t == "Sản")
                c = "KS";
            else if (t == "Nội Cơ-Xương-Khớp")
                c = "NCXK";
            else if (t == "Nội Trú")
                c = "NT1";
            else if (t == "Ngoại Trú")
                c = "NT2";
            else if (t == "Nội Tiết")
                c = "NT3";
            else if (t == "Nội Tổng Hợp")
                c = "NTH1";
            else if (t == "Nội Tiêu Hoá")
                c = "NTH2";
            else if (t == "Nội Tim Mạch")
                c = "NTM";
            else if (t == "Nội Thận-Tiết Niệu")
                c = "NTTN";
            else if (t == "Phục Hồi Chức Năng")
                c = "PHCN";
            else if (t == "Phụ Khoa")
                c = "PK";
            else if (t == "Phẫu Thuật")
                c = "PT";
            return c;
        }
        private string getIDNhomBenh(string t)
        {
            string c = "";

            // Ánh xạ từ loại bệnh sang mã tương ứng
            if (t == "Bệnh đau lưng")
                c = "DL";
            else if (t == "Bệnh đa nang")
                c = "DN";
            else if (t == "Bệnh đau thần kinh toàn thân")
                c = "DTKTT";
            else if (t == "Bệnh hen suyễn")
                c = "HS";
            else if (t == "Bệnh rối loạn tâm thần")
                c = "RLTT";
            else if (t == "Bệnh trầm cảm")
                c = "TC";
            else if (t == "Bệnh tiểu đường")
                c = "TD";
            else if (t == "Bệnh tiêu hóa")
                c = "TH";
            else if (t == "Bệnh tăng huyết áp")
                c = "THA";
            else if (t == "Bệnh tim mạch")
                c = "TM";
            else if (t == "Bệnh truyền nhiễm")
                c = "TN";
            else if (t == "Bệnh trĩ")
                c = "TR";
            else if (t == "Bệnh tiền đình")
                c = "TT";
            else if (t == "Bệnh ung thư")
                c = "UT";
            else if (t == "Bệnh viêm gan")
                c = "VG";
            else if (t == "Bệnh viêm khớp")
                c = "VK";
            else if (t == "Bệnh viêm phổi")
                c = "VP";
            else if (t == "Bệnh viêm thận")
                c = "VT";
            return c;
        }
        public void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            panel1.Show();
            panel3.Hide();
            Khoa khoa = new Khoa(this);
            //MyEvent?.Invoke(this, EventArgs.Empty);
            //Khoa.checkPass = false;
            if (Khoa.checkPass == false)
            {
                addUserControl(khoa);
            }
            else
            {
                panel1.Controls.Clear();
            }

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string query = $"EXEC dbo.SearchPCD @keyword = N'{guna2TextBox7.Text}'";
            LoadData(query);
        }

        private void guna2TextBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Hide();
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            pictureBox1.Show();
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            Khoa.checkPass = false;
            guna2PictureBox2_Click(sender, e);
            //guna2PictureBox2_Click
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            panel1.Show();
            panel3.Hide();
            LichHen lichHen = new LichHen();
            addUserControl(lichHen);
        }
        public void Alert(string message, FormAlert.enmType type)
        {
            FormAlert alert = new FormAlert();
            alert.showAlert(message, type);
        }
        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            panel4.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            panel3.Show();
            panel1.Hide();
            /*string query = "SELECT * FROM dbo.bangPCD();"; // Thay đổi YourTable thành tên bảng thực tế của bạn
            connection.connect();
            try
            {
                // Thực thi truy vấn và lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu từ DataTable vào DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }*/
            //LoadData("select maPCD as 'ID', maBS as 'Mã Bác Sĩ', maBN as 'Mã Bệnh Nhân', ngayCD as 'Ngày Chuẩn Đoán', tongTien as 'Thành Tiền', trangThai as 'Trạng Thái', stt from phieuChuanDoan");
            LoadData($"exec dbo.SearchPCD ''");
        }

        //-------------------------- kéo thả form-----------
        private bool isDragging = false;
        private Point lastCursorPos;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastCursorPos = e.Location;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newCursorPos = this.PointToScreen(e.Location);
                this.Location = new Point(newCursorPos.X - lastCursorPos.X, newCursorPos.Y - lastCursorPos.Y);
            }
        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {
            panel1.Show();
            panel3.Hide();
            FormUpdate formUpdate = new FormUpdate();
            addUserControl(formUpdate);
        }

        

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {
            string query = $"EXEC dbo.SearchPCD @keyword = N'{guna2TextBox7.Text}'";
            LoadData(query);
        }

        

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2ComboBox2.Items.Clear();
            connection.connect();
            string query = $"select tenPhong from phong where maKhoa = '{getIDKhoa(guna2ComboBox1.Text)}'";
            cmd = new SqlCommand(query, connection.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                guna2ComboBox2.Items.Add(reader.GetString(0));
            }
            connection.disconnect();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2ComboBox4.Items.Clear();
            connection.connect();
            string query = $"select tenBenh from benh where maNhomBenh = '{getIDNhomBenh(guna2ComboBox3.Text)}'";
            cmd = new SqlCommand(query, connection.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                guna2ComboBox4.Items.Add(reader.GetString(0));
            }
            connection.disconnect();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            panel4.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            dataGridView1.Visible = true;
        }
        Random random = new Random();
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string query = $"exec dbo.themPK '{Function.getID("PhieuKham", "maPK", "PK") }',{random.Next(100000,500000)},N'{guna2ComboBox2.Text}','{selectedRow.Cells[0].Value.ToString()}'";
                LoadData2(query);
                Console.WriteLine(selectedRow.Cells[0].Value.ToString());
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (guna2ComboBox4.Text != "")
            {
                string querry = $"exec dbo.insLSB N'{guna2ComboBox4.Text}', '{selectedRow.Cells[0].Value.ToString()}', N'{richTextBox1.Text}'";
                Console.WriteLine(guna2ComboBox4.Text);
                Console.WriteLine(selectedRow.Cells[0].Value.ToString());
                Console.WriteLine(richTextBox1.Text);
                Console.WriteLine(querry);
                LoadDataBenh(querry);
            }
        }
        bool click = false;
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (comboBox1.Text != "" && guna2TextBox2.Text != "")
            {
                string query = $"exec dbo.insDT '{Function.getID("donThuoc", "maDT", "DT")}','{comboBox1.Text}','{guna2TextBox2.Text}', '{selectedRow.Cells[0].Value.ToString()}'";
                LoadDataThuoc(query);
            }
        }
        #region 


        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion
        int count = 0;
        Dictionary<string, string> map = new Dictionary<string, string>();
        private Dictionary<string, string> SearchKeyword(string keyword)
        {
            comboBox1.SuspendLayout();
            comboBox1.Items.Clear();
            comboBox1.ResumeLayout();
            
            try
            {
                // Thực hiện tìm kiếm và thêm kết quả vào results
                if (connection.conn.State == ConnectionState.Closed)
                    connection.connect();
                string query = $"exec dbo.SearchThuoc @keyword = N'{keyword}'";
                SqlCommand cmd = new SqlCommand(query, connection.conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        count++;
                        string t = reader.GetString(0);
                        comboBox1.Items.Add(t);
                        map[t] = reader.GetString(1);
                        //results.Add(reader.GetString(0));

                    }
                if (connection.conn.State == ConnectionState.Open)
                    connection.disconnect();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return map;
        }
        

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                string query = $"select * from thuoc where tenthuoc = '{comboBox1.Text}'";
                connection.connect();
                SqlCommand cmd = new SqlCommand(query, connection.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label11.Text = reader.GetString(2);
                    label12.Text = reader.GetInt32(3).ToString() + "VNĐ";
                }
                connection.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        private void comboBox1_TextChanged_1(object sender, EventArgs e)
        {
            comboBox1.MaxDropDownItems = 7;
            //comboBox1.BeginUpdate();
            string keyword = comboBox1.Text;
            //List<string> searchResults = SearchKeyword(keyword);
            int selectionStart = comboBox1.SelectionStart;
            SearchKeyword(comboBox1.Text);
            comboBox1.DroppedDown = true;
            comboBox1.SelectionStart = selectionStart;
            
        }

        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_Validating(object sender, CancelEventArgs e)
        {
            //System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            panel4.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            dataGridView1.Visible = true;

            string query = $"select dbo.locationPCD ('{selectedRow.Cells[0].Value.ToString()}')";
            connection.connect();
            SqlCommand cmd = new SqlCommand(query, connection.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            string t = "";
            while (reader.Read())
            {
                t = reader.GetString(0);
            }
            connection.disconnect();//0000503380

            MessageBox.Show(maDH2);
            Function.sendData(t, maDH2 + "_esp32_receive");
        }

        private void dataGridView1_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                if (selectedRow.Cells[7].Value.ToString() != "true")
                {
                    click = true;
                    panel4.Visible = true;
                    dataGridView2.Visible = true;
                    dataGridView3.Visible = true;
                    dataGridView4.Visible = true;
                    string query = $"exec dbo.slPK '{selectedRow.Cells[0].Value.ToString()}'";
                    string query2 = $"exec dbo.selLSB '{selectedRow.Cells[0].Value.ToString()}'";
                    string query3 = $"exec dbo.slDT '{selectedRow.Cells[0].Value.ToString()}' ";
                    LoadData2(query);
                    LoadDataBenh(query2);
                    LoadDataThuoc(query3);
                    dataGridView1.Visible = false;
                }
            }
            //DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && click == true)
            {
                Console.WriteLine("123321");
                if (selectedRow.Cells[7].Value.ToString() != "true")
                {
                    click = false;
                    Console.WriteLine("daowdnădăd");
                    XacNhan xacNhan = new XacNhan(selectedRow.Cells[0].Value.ToString());
                    xacNhan.ShowDialog();
                    string query = $"EXEC dbo.SearchPCD @keyword = N'{guna2TextBox7.Text}'";
                    LoadData(query);
                }
            }
        }
        //---------------------------------------------------
    }
}
