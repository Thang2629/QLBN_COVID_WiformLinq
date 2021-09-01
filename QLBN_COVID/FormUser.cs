using QLBN_COVID.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
        }
        private DataGridViewRow r;
        private CovidDataContext db;
        private void FormUser_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
            Showdata();
            dgvUser.Columns["ID"].HeaderText = "id";
            dgvUser.Columns["Username"].HeaderText = "tai khoan";
            dgvUser.Columns["Password"].HeaderText = "Mat Khau";
            dgvUser.Columns["Role"].HeaderText = "quyen";
            dgvUser.Columns["FullName"].HeaderText = "ten";
        }
        private void Showdata()
        {

            var ra = from a in db.User_Logs
                     select new
                     {
                         a.ID,
                         a.Username,
                         a.Password,
                         a.Role,
                         a.FullName,
                         a.Status
                     };
             dgvUser.DataSource = ra;
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvUser.Rows[e.RowIndex];
                txtUser.Text = r.Cells["Username"].Value.ToString();
                txtPass.Text = r.Cells["Password"].Value.ToString();
                txtRole.Text = r.Cells["Role"].Value.ToString();
                txtTen.Text = r.Cells["FullName"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (
                 MessageBox.Show("Bạn thực sự muốn xóa Tài Khoản  " + r.Cells["Username"].Value.ToString() + " ?",
                                      "Xác nhận xóa",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question) == DialogResult.Yes
                )
            {
                try
                {
                    var us = db.User_Logs.SingleOrDefault(x => x.Username == r.Cells["Username"].Value.ToString());
                    db.User_Logs.DeleteOnSubmit(us);
                    db.SubmitChanges();
                    MessageBox.Show("Xóa tài khoản thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Xóa tài khoản thất bại!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Showdata();
                    txtUser.Text = txtPass.Text = txtTen.Text = txtRole.Text = null;
                    r = null;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản nhân viên", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Select();
                return;
            }
            //var c = db.User_Logs.Where(x => x.Username.Trim().ToLower() == txtUser.Text.Trim().ToLower()).Count();

            //if (c > 0)
            //{
            //    MessageBox.Show("Tài khoản này đã tồn tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtUser.Select();
            //    return;
            //}




            ////Object reference not set to an instance of an object.'
            var nv = new User_Log();
            nv.Username = txtUser.Text;
            nv.Password = txtPass.Text;
            nv.FullName = txtTen.Text;
            nv.Role = int.Parse(txtRole.Text);
            db.User_Logs.InsertOnSubmit(nv);//
            db.SubmitChanges();
            MessageBox.Show("Thêm mới tài khoản thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Showdata();
            txtUser.Text = txtPass.Text = txtTen.Text = txtRole.Text = null;



        }

        private void FormUser_Load_1(object sender, EventArgs e)
        {

        }
    }
   

        
    
}

    
