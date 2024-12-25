using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        private DataTable dtSinhVien; // Bảng dữ liệu tạm để lưu sinh viên
        private bool isAdding = false; // Cờ để kiểm tra trạng thái thêm mới hoặc sửa

        public frmSinhVien()
        {
            InitializeComponent();
            InitializeDataTable(); // Khởi tạo bảng dữ liệu
            LoadData(); // Tải dữ liệu lên DataGridView
            LoadComboBox(); // Tải dữ liệu cho ComboBox lớp học
        }

        // Hàm khởi tạo DataTable
        private void InitializeDataTable()
        {
            dtSinhVien = new DataTable();
            dtSinhVien.Columns.Add("MaSV");
            dtSinhVien.Columns.Add("HoTen");
            dtSinhVien.Columns.Add("NgaySinh");
            dtSinhVien.Columns.Add("MaLop");

            // Thêm dữ liệu mẫu
            dtSinhVien.Rows.Add("SV001", "Nguyen Van A", "2000-01-01", "Kế Toán");
            dtSinhVien.Rows.Add("SV002", "Tran Thi B", "2001-02-15", "CNTT");
        }

        // Hàm tải dữ liệu vào DataGridView
        private void LoadData()
        {
            dgvSinhVien.DataSource = dtSinhVien; // Đảm bảo DataGridView hiển thị đúng DataTable
        }

        // Xử lý sự kiện thêm sinh viên
        private void btthem_Click(object sender, EventArgs e)
        {
            isAdding = true;
            ClearInput();
            EnableControls(true);
        }

        // Xử lý sự kiện sửa sinh viên
        private void btsua_Click(object sender, EventArgs e)
        {
            isAdding = false;
            EnableControls(true);
        }

        // Xử lý sự kiện xóa sinh viên
        private void btxoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    // Xóa dòng được chọn
                    foreach (DataGridViewRow row in dgvSinhVien.SelectedRows)
                    {
                        dgvSinhVien.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.", "Thông báo");
            }
        }

        // Xử lý sự kiện lưu sinh viên
        private void btluu_Click(object sender, EventArgs e)
        {
            if (isAdding)
            {
                // Thêm mới sinh viên
                dtSinhVien.Rows.Add(txtMaSV.Text, txtHotenSV.Text, dtNgaySinh.Value.ToString("yyyy-MM-dd"), cboLop.Text);
                MessageBox.Show("Thêm mới thành công!", "Thông báo");
            }
            else
            {
                // Sửa thông tin sinh viên
                if (dgvSinhVien.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dgvSinhVien.SelectedRows[0];
                    row.Cells["MaSV"].Value = txtMaSV.Text;
                    row.Cells["HoTen"].Value = txtHotenSV.Text;
                    row.Cells["NgaySinh"].Value = dtNgaySinh.Value.ToString("yyyy-MM-dd");
                    row.Cells["MaLop"].Value = cboLop.Text;
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                }
            }
            EnableControls(false);
            ClearInput();
        }

        // Xử lý sự kiện hủy
        private void btkhong_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            ClearInput();
        }

        // Xử lý sự kiện thoát
        private void btthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Xử lý sự kiện chọn dòng trong DataGridView
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHotenSV.Text = row.Cells["HoTen"].Value.ToString();
                dtNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                cboLop.Text = row.Cells["MaLop"].Value.ToString();
            }
        }

        // Bật hoặc tắt các điều khiển (textbox, combobox, button)
        private void EnableControls(bool enable)
        {
            txtMaSV.Enabled = enable && isAdding; // Chỉ cho nhập MaSV khi thêm mới
            txtHotenSV.Enabled = enable;
            dtNgaySinh.Enabled = enable;
            cboLop.Enabled = enable;
            btluu.Enabled = enable;
            btkhong.Enabled = enable;

            btthem.Enabled = !enable;
            btsua.Enabled = !enable;
            btxoa.Enabled = !enable;
        }

        // Xóa các ô nhập liệu
        private void ClearInput()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtNgaySinh.Value = DateTime.Now;
            cboLop.SelectedIndex = -1;
        }

        // Tải dữ liệu cho ComboBox lớp học
        private void LoadComboBox()
        {
            cboLop.Items.Clear(); // Xóa tất cả các mục trong ComboBox trước khi thêm mới.

            string connectionString = "YourConnectionStringHere"; // Thay bằng chuỗi kết nối của bạn
            string query = "SELECT TenLop FROM Lop"; // Truy vấn lấy tên lớp

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Mở kết nối đến cơ sở dữ liệu

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader(); // Thực thi truy vấn và lấy dữ liệu

                    // Kiểm tra nếu có dữ liệu
                    while (reader.Read())
                    {
                        cboLop.Items.Add(reader["TenLop"].ToString());
                    }

                    // Nếu có ít nhất một lớp, chọn lớp đầu tiên làm mặc định
                    if (cboLop.Items.Count > 0)
                    {
                        cboLop.SelectedIndex = 0; // Chọn lớp đầu tiên
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lớp học: " + ex.Message);
            }
        }

        // Xử lý sự kiện thay đổi lớp học trong ComboBox
        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLop.SelectedItem != null)
            {
                string selectedLop = cboLop.SelectedItem.ToString();

                // Lọc danh sách sinh viên theo lớp
                DataView dv = dtSinhVien.DefaultView;
                dv.RowFilter = $"MaLop = '{selectedLop}'";

                // Cập nhật lại DataGridView
                dgvSinhVien.DataSource = dv;
            }
        }

        private void sinhVienBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void sinhVienBindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
