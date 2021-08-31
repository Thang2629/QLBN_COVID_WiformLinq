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
            dataUser.Columns["ID"].HeaderText = "id";
            dataUser.Columns["Username"].HeaderText = "tai khoan";
            dataUser.Columns["Password"].HeaderText = "Mat Khau";
            dataUser.Columns["Role"].HeaderText = "quyen";
            dataUser.Columns["FullName"].HeaderText = "ten";
            dataUser.Columns["Status"].HeaderText = "trang thai";
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
            dataUser.DataSource = ra;
        }
        private void dataUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                r = dataUser.Rows[e.RowIndex];
                txtUser.Text = r.Cells["Username"].Value.ToString();
                txtPass.Text = r.Cells["Password"].Value.ToString();
                txtRole.Text = r.Cells["Role"].Value.ToString();
                txtTen.Text = r.Cells["FullName"].Value.ToString();
                txtStatus.Text = r.Cells["Status"].Value.ToString();
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
                    txtUser.Text = txtPass.Text = txtTen.Text = txtRole.Text = txtStatus.Text = null; 
                    r = null;
                }
            }
        }

        
    }
}
    
