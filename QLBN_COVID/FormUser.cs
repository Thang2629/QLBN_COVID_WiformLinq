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
            cbxStatus.ValueMember = "IDStatus";
            cbxStatus.SelectedIndex = -1;

            cbxRole.DataSource = db.User_Roles;
            cbxRole.DisplayMember = "Rolename";
            cbxRole.ValueMember = "IDRole";
            cbxRole.SelectedIndex = -1;

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
            if(dataUser.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                r = dataUser.Rows[e.RowIndex];
                dataUser.CurrentRow.Selected = true;
                txtUsername.Text = dataUser.Rows[e.RowIndex].Cells["Username"].FormattedValue.ToString();
                txtPass.Text = dataUser.Rows[e.RowIndex].Cells["Password"].FormattedValue.ToString();
                txtName.Text = dataUser.Rows[e.RowIndex].Cells["FullName"].FormattedValue.ToString();
                cbxRole.Text = dataUser.Rows[e.RowIndex].Cells["Rolename"].FormattedValue.ToString();
               cbxStatus.Text = dataUser.Rows[e.RowIndex].Cells["Statusname"].FormattedValue.ToString();

            }    
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Select();
                return;
            }
            if(string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên user", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Select();
                return;
            }
            if (cbxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn quyền", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            var user = new User_Log();
            user.Username = txtUsername.Text;
            user.Password = txtPass.Text;
            user.FullName = txtName.Text;
            user.Role = int.Parse(cbxRole.SelectedValue.ToString());
            user.Status = int.Parse(cbxStatus.SelectedValue.ToString());
            db.User_Logs.InsertOnSubmit(user);
            db.SubmitChanges();

            //Thêm hành động vào lịch sử hoạt động
            var userAcitvity = new User_Activity();
           
            userAcitvity.UserID = FormLogin.user.ID;
            userAcitvity.Timestamp = DateTime.Now;
            userAcitvity.Action = "Thêm User"+ txtUsername.Text;
            db.User_Activities.InsertOnSubmit(userAcitvity);

            db.SubmitChanges();



            showData();
            MessageBox.Show("Thêm mới user thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtUsername.Text = null;
            txtPass.Text = null;
            txtName.Text = null;
            cbxStatus.SelectedIndex = -1;
            cbxRole.SelectedIndex = -1;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên user", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Select();
                return;
            }
            if (cbxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn quyền", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = db.User_Logs.SingleOrDefault(x => x.Username.Equals(txtUsername.Text));
            user.Username = txtUsername.Text;
            user.Password = txtPass.Text;
            user.FullName = txtName.Text;
            user.Role = int.Parse(cbxRole.SelectedValue.ToString());
            user.Status = int.Parse(cbxStatus.SelectedValue.ToString());
            db.SubmitChanges();

            //Thêm hành động vào lịch sử hoạt động
            var userAcitvity = new User_Activity();
            userAcitvity.Action = "Sửa thông tin User: " + txtUsername.Text;
            userAcitvity.Timestamp = DateTime.Now;
            userAcitvity.UserID = FormLogin.user.ID;
            db.User_Activities.InsertOnSubmit(userAcitvity);
            db.SubmitChanges();

            showData();
            MessageBox.Show("Cập nhật user thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtUsername.Text = null;
            txtPass.Text = null;
            txtName.Text = null;
            cbxStatus.SelectedIndex = -1;
            cbxRole.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(r == null)
            {
                MessageBox.Show("Vui lòng chọn user cần xóa", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(
                MessageBox.Show(
                        "Bạn có muốn xóa user: " + r.Cells["Username"].Value.ToString() + "?",
                        "Xác nhận xóa user",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    ) == DialogResult.Yes
             )
            {
                try
                {
                    var user = db.User_Logs.SingleOrDefault(x => x.Username.Equals(txtUsername.Text));
                    db.User_Logs.DeleteOnSubmit(user);
                    db.SubmitChanges();

                    //Thêm hành động vào lịch sử hoạt động
                    var userAcitvity = new User_Activity();
                    userAcitvity.Action = "Xoá User: " + user.FullName;
                    userAcitvity.Timestamp = DateTime.Now;
                    userAcitvity.UserID = FormLogin.user.ID;
                    db.User_Activities.InsertOnSubmit(userAcitvity);

                    db.SubmitChanges();
                    MessageBox.Show("Xóa user thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Xóa user thất bại", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                showData();
                txtUsername.Text = null;
                txtPass.Text = null;
                txtName.Text = null;
                cbxStatus.SelectedIndex = -1;
                cbxRole.SelectedIndex = -1;
            }    
        }
        public static User_Log user;
        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn user cần xem lịch sử hoạt động", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var tk = db.User_Logs.SingleOrDefault(x => x.Username.Equals(txtUsername.Text));
            if(tk != null)
            {
                user = tk;
                FormLichSuHoatDong f = new FormLichSuHoatDong();
                f.Show();
            }    
           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text == " ")
            {
                showData();
            }
            else { 
            var st = from u in db.User_Logs
                     join r in db.User_Roles on u.Role equals r.IDRole
                     join s in db.User_Status on u.Status equals s.IDStatus
                     where (u.Username == txtSearch.Text) || (u.FullName == txtSearch.Text)
                     select new
                     {

                         u.Username,
                         u.Password,
                         u.FullName,
                         r.Rolename,
                         s.Statusname
                     };
            if(st != null)
            {
                dataUser.DataSource = st;
            }
            else
            {
                MessageBox.Show("Không tìm thấy user", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearch.Select();
                return;
            }
            }
        }
    }
}
