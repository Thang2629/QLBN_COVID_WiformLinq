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
            cbxStatus.DataSource = db.User_Status;
            cbxStatus.DisplayMember = "Statusname";
            cbxRole.DataSource = db.User_Roles;
            cbxRole.DisplayMember = "Rolename";
            showData();
            dataUser.Columns["Username"].HeaderText = "Tài khoản";
            dataUser.Columns["Password"].HeaderText = "Mật khẩu";
            dataUser.Columns["FullName"].HeaderText = "Tên user";
            dataUser.Columns["Rolename"].HeaderText = "Quyền";
            dataUser.Columns["Statusname"].HeaderText = "Trạng thái";

        }
        private void showData()
        {
            var rs = from u in db.User_Logs
                     join r in db.User_Roles on u.Role equals r.IDRole
                     join s in db.User_Status on u.Status equals s.IDStatus
                     select new
                     {
                         u.Username,
                         u.Password,
                         u.FullName,
                         r.Rolename,
                         s.Statusname
                     };
            dataUser.DataSource = rs;
        }

        private void dataUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataUser.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dataUser.CurrentRow.Selected = true;
                txtUsername.Text = dataUser.Rows[e.RowIndex].Cells["Username"].FormattedValue.ToString();
                txtPass.Text = dataUser.Rows[e.RowIndex].Cells["Password"].FormattedValue.ToString();
                txtName.Text = dataUser.Rows[e.RowIndex].Cells["FullName"].FormattedValue.ToString();
                cbxRole.Text = dataUser.Rows[e.RowIndex].Cells["Rolename"].FormattedValue.ToString();
                cbxStatus.Text = dataUser.Rows[e.RowIndex].Cells["Statusname"].FormattedValue.ToString();

            }
        }
        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (r == null)
        //    {
        //        MessageBox.Show("Vui lòng chọn tài khoản cần xóa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    if (
        //         MessageBox.Show("Bạn thực sự muốn xóa Tài Khoản  " + r.Cells["Username"].Value.ToString() + " ?",
        //                              "Xác nhận xóa",
        //                              MessageBoxButtons.YesNo,
        //                              MessageBoxIcon.Question) == DialogResult.Yes
        //        )
        //    {
        //        try
        //        {
        //            var us = db.User_Logs.SingleOrDefault(x => x.Username == r.Cells["Username"].Value.ToString());
        //            db.User_Logs.DeleteOnSubmit(us);
        //            db.SubmitChanges();
        //            MessageBox.Show("Xóa tài khoản thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Xóa tài khoản thất bại!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        finally
        //        {
        //            showData();
        //            txtUsername.Text = txtPass.Text = txtName.Text = cbxRole.Text = cbxStatus.Text = null; 
        //            r = null;
        //        }
        //    }
        //}

        
    }
}
    
