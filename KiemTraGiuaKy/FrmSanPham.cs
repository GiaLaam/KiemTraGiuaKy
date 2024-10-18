using KiemTraGiuaKy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace KiemTraGiuaKy
{
    public partial class FrmSanPham : Form
    {
        SanPhamContextDB context = new SanPhamContextDB();
        public FrmSanPham()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<LoaiSanPham> listloaisanpham = context.LoaiSanPhams.ToList();
            List<SanPham> listsanpham = context.SanPhams.ToList();

            filldgv(listsanpham);
            fillcbb(listloaisanpham);
        }

        private void fillcbb(List<LoaiSanPham> listloaisanpham)
        {
            comboBox.DataSource = listloaisanpham;
            comboBox.DisplayMember = "TenLoai";
            comboBox.SelectedItem = "MaLoai";
        }

        private void filldgv(List<SanPham> listsanpham)
        {
            dataGridView1.Rows.Clear();
            foreach ( SanPham sanpham in listsanpham )
            {
                int newRow = dataGridView1.Rows.Add( sanpham );
                dataGridView1.Rows[newRow].Cells[0].Value = sanpham.MaSanPham;
                dataGridView1.Rows[newRow].Cells[1].Value = sanpham.TenSanPham;
                dataGridView1.Rows[newRow].Cells[2].Value = sanpham.NgayNhap;
                dataGridView1.Rows[newRow].Cells[3].Value = sanpham.LoaiSanPham.TenLoai;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string masp = txtMa.Text;
            string tensp = txtName.Text;
            DateTime ngayNhap;
            if (!DateTime.TryParse(dateTime.Text, out ngayNhap))
            {
                MessageBox.Show("Vui lòng nhập đúng ngày!");
                return;
            }
            string loai = (comboBox.SelectedItem as LoaiSanPham)?.MaLoai;
            if (string.IsNullOrWhiteSpace(masp) || string.IsNullOrWhiteSpace(tensp) || string.IsNullOrWhiteSpace(loai))
            {
                MessageBox.Show("Vui lòng điền đủ thông tin!");
                return;
            }
            SanPham newSanPham = new SanPham
            {
                MaSanPham = masp,
                TenSanPham = tensp,
                NgayNhap = ngayNhap,
                MaLoai = loai
            };
            context.SanPhams.Add(newSanPham);
            try
            {
                context.SaveChanges();
                MessageBox.Show("Thêm sản phẩm thành công!");
                filldgv(context.SanPhams.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string masp = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                SanPham sanPhamToEdit = context.SanPhams.FirstOrDefault(sp => sp.MaSanPham == masp);
                if (sanPhamToEdit != null)
                {
                    sanPhamToEdit.TenSanPham = txtName.Text;
                    DateTime ngayNhap;
                    if (!DateTime.TryParse(dateTime.Text, out ngayNhap))
                    {
                        MessageBox.Show("Ngày không hợp lệ.");
                        return;
                    }
                    sanPhamToEdit.NgayNhap = ngayNhap;
                    string loai = (comboBox.SelectedItem as LoaiSanPham)?.MaLoai;
                    if (string.IsNullOrWhiteSpace(loai))
                    {
                        MessageBox.Show("Vui lòng chọn loại sản phẩm.");
                        return;
                    }
                    sanPhamToEdit.MaLoai = loai;
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Cập nhật sản phẩm thành công.");
                        filldgv(context.SanPhams.ToList());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string masp = txtMa.Text;
            var maspToDelete = context.SanPhams.FirstOrDefault(s=>s.MaSanPham == masp);
            if (maspToDelete != null)
            {
                context.SanPhams.Remove(maspToDelete);
                context.SaveChanges();
                MessageBox.Show("Xóa sản phẩm thành công!");
                filldgv(context.SanPhams.ToList());
                txtMa.Clear();
                txtName.Clear();
                dateTime.Select();
                comboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Mã sản phẩm không tồn tại !");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string masp = txtTim.Text.Trim();
            string tensp = txtName.Text.Trim();

            var query = context.SanPhams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(masp))
            {
                query = query.Where(sp => sp.MaSanPham.Contains(masp));
            }

            if (!string.IsNullOrWhiteSpace(tensp))
            {
                query = query.Where(sp => sp.TenSanPham.Contains(tensp));
            }
            List<SanPham> results = query.ToList();
            filldgv(results);
            if (results.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào.");
            }
        }
    }
}
