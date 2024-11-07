using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoT
{
    public partial class FormUpdate : UserControl
    {
        public FormUpdate()
        {
            InitializeComponent();
        }
        int check = 0;
        private void FormUpdate_Load(object sender, EventArgs e)
        {
            LoadData("SELECT maBS as 'Mã Bác Sĩ', tenBS as 'Tên', gioiTinh as 'Giới Tính', chucVu as 'Chức Vụ', diaChi as 'Địa Chỉ', soDienThoai as 'SĐT', email FROM bacsi");
            editName();
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        private void editName()
        {
            
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = " ";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox7.Text = "";
            panel1.Visible = true;
            guna2HtmlLabel1.Text = dataGridView1.Columns[1].Name;
            guna2HtmlLabel2.Text = dataGridView1.Columns[2].Name;
            guna2HtmlLabel3.Text = dataGridView1.Columns[3].Name;
            guna2HtmlLabel4.Text = dataGridView1.Columns[4].Name;
            guna2HtmlLabel5.Text = dataGridView1.Columns[5].Name;
            guna2HtmlLabel6.Text = dataGridView1.Columns[6].Name;
        }
        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 1;
            string query = "Select maBN as'Mã Bệnh Nhân', tenBN as 'Tên', gioiTinh as 'Giới Tính', ngaySinh as 'Ngày Sinh', diaChi as 'Địa Chỉ', soDienThoai as 'SĐT', maBHYT as 'BHYT' from benhnhan";
            //dataGridView1.Columns.Clear();
            LoadData(query);
            editName();
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 0;
            LoadData("SELECT maBS as 'Mã Bác Sĩ', tenBS as 'Tên', gioiTinh as 'Giới Tính', chucVu as 'Chức Vụ', diaChi as 'Địa Chỉ', soDienThoai as 'SĐT', email FROM bacsi");
            editName();
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void editname2()
        {

            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox7.Text = "";
            panel1.Visible = false;
            guna2HtmlLabel1.Text = dataGridView1.Columns[1].Name;
            guna2HtmlLabel2.Text = dataGridView1.Columns[2].Name;
            guna2HtmlLabel3.Text = dataGridView1.Columns[3].Name;
        }
        private void thuốcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 2;
            string query = "select maThuoc as 'Mã Thuốc', tenThuoc as 'Tên Thuốc', donVi as 'Đơn Vị', giaTien as 'Giá Tiền' from thuoc";
            LoadData(query);
            editname2();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        string ID;
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            ID = selectedRow.Cells[0].Value.ToString();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                guna2TextBox1.Text = selectedRow.Cells[1].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells[2].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells[3].Value.ToString();
                if (check == 1)
                {
                    
                }
                if(check != 2)
                {
                    guna2TextBox4.Text = selectedRow.Cells[4].Value.ToString();
                    guna2TextBox5.Text = selectedRow.Cells[5].Value.ToString();
                    guna2TextBox6.Text = selectedRow.Cells[6].Value.ToString();
                }
            }
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {
            if(check == 2)
            {
                string query = $"EXEC dbo.Searchthuoc @keyword = N'{guna2TextBox7.Text}'";
                LoadData(query);
            }
            else if(check == 0)
            {
                string query = $"EXEC dbo.SearchBacSi @keyword = N'{guna2TextBox7.Text}'";
                LoadData(query);
            }
            else if (check == 1)
            {
                string query = $"EXEC dbo.SearchBenhNhan @keyword = N'{guna2TextBox7.Text}'";
                LoadData(query);
            }
        }
        SqlCommand cmd = null;
        public void insertSQLToTableBenhNhan()
        {
            connection.connect();
            string insertQuery = $"INSERT benhNhan (maBN, tenBN, ngaySinh, diaChi, soDienThoai, maBHYT) VALUES(N'{Function.getID("benhNhan","maBN","BN")}',N'{guna2TextBox1.Text}', N'{guna2TextBox2.Text}', N'{guna2TextBox3.Text}', N'{guna2TextBox4.Text}', N'{guna2TextBox5.Text}', N'{guna2TextBox6.Text}')";
            //string insertQuery = "INSERT INTO `bacsi`(`maBS`, `tenBS`, `gioiTinh`, `chucVu`, `maKhoa`, `diaChi`, `soDienThoai`, `email`) VALUES('{value1}', 'Hồ Sỹ Cường', 'Nam', 'Phó Giám Đốc', 'CC', 'Nghệ An', '0865734636', 'hosyCuong2011@gmail.com') ";
            try
            {
                cmd = new SqlCommand(insertQuery, connection.conn);
                int row = cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm Thành Công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);// cần chỉnh sửa
            }
            connection.disconnect();
        }
        public void insertSQLToTableThuoc()
        {
            connection.connect();
            string insertQuery = $"INSERT thuoc (maThuoc, tenThuoc, donVi, giaTien) VALUES(N'{Function.getID("thuoc", "maThuoc", "TH")}',N'{guna2TextBox1.Text}', N'{guna2TextBox2.Text}', N'{guna2TextBox3.Text}')";
            //string insertQuery = "INSERT INTO `bacsi`(`maBS`, `tenBS`, `gioiTinh`, `chucVu`, `maKhoa`, `diaChi`, `soDienThoai`, `email`) VALUES('{value1}', 'Hồ Sỹ Cường', 'Nam', 'Phó Giám Đốc', 'CC', 'Nghệ An', '0865734636', 'hosyCuong2011@gmail.com') ";
            try
            {
                cmd = new SqlCommand(insertQuery, connection.conn);
                int row = cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm Thành Công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);// cần chỉnh sửa
            }
            connection.disconnect();
        }
        // thêm
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (check == 0)
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "" || guna2TextBox4.Text == "" || guna2TextBox5.Text == "" || guna2TextBox6.Text == "")
                {
                    MessageBox.Show("Hãy Điền Đầy Đủ Thông Tin");
                }
                else
                {
                    if(Function.check("bacSi","soDienThoai", guna2TextBox5.Text) == 0)
                    {
                        BacSi bacSi = new BacSi(guna2TextBox1.Text, guna2TextBox2.Text, guna2TextBox3.Text, guna2TextBox4.Text, guna2TextBox5.Text, guna2TextBox6.Text);
                    }
                    else
                    {
                        MessageBox.Show("Trùng Lặp Thông Tin");
                    }
                    
                }
            }
            else if (check ==1)
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "" || guna2TextBox4.Text == "" || guna2TextBox5.Text == "" || guna2TextBox6.Text == "")
                {
                    MessageBox.Show("Hãy Điền Đầy Đủ Thông Tin");
                }
                else
                {
                    if (Function.check("benhNhan", "soDienThoai", guna2TextBox5.Text) == 0)
                    {
                        insertSQLToTableBenhNhan();
                    }
                    else
                    {
                        MessageBox.Show("Trùng Lặp Thông Tin");
                    }
                    
                }
            }
            else
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                {
                    MessageBox.Show("Hãy Điền Đầy Đủ Thông Tin");
                }
                else
                {
                    if (Function.check("thuoc", "tenThuoc", guna2TextBox2.Text) == 0)
                    {
                        insertSQLToTableThuoc();
                    }
                    else
                    {
                        MessageBox.Show("Trùng Lặp Thông Tin");
                    }
                }
            }
        }
        // sửa
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "" || guna2TextBox4.Text == "" || guna2TextBox5.Text == "" || guna2TextBox6.Text == "")
            {
                MessageBox.Show("Hãy Điền Đầy Đủ Thông Tin");
            }
            else
            {
                if(check == 0)
                {
                    Function.updateBacSi(ID, guna2TextBox1.Text, guna2TextBox2.Text, guna2TextBox3.Text, guna2TextBox4.Text, guna2TextBox5.Text, guna2TextBox6.Text);
                }
                else if (check == 1)
                {
                    Function.updateBenhNhan(ID, guna2TextBox1.Text, guna2TextBox2.Text, guna2TextBox3.Text, guna2TextBox4.Text, guna2TextBox5.Text, guna2TextBox6.Text);
                }
            }
            if(check == 2)
            {
                if(guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                {
                    MessageBox.Show("Hãy Điền Đầy Đủ Thông Tin");
                }
                else
                {
                    Function.updateThuoc(ID, guna2TextBox1.Text, guna2TextBox2.Text, guna2TextBox3.Text);
                }
            }
        }
        // xoá
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            if(check == 0)
            {
                Function.delete("bacSi", "maBS", ID);
            }
            else if(check == 1)
            {
                Function.delete("benhNhan", "maBN", ID);
            }
            else
            {
                Function.delete("thuoc", "maThuoc", ID);
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
