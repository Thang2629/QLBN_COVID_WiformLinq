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
    public partial class FormDangKy : Form
    {
        public FormDangKy()
        {
            InitializeComponent();
        }
        private CovidDataContext db;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var user = db.User_Logs.SingleOrDefault(x => x.Username.Equals(txtUsername.Text));
            if(user != null)
            {
                MessageBox.Show("Username đã tồn tại vui lòng đổi tên khác", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Select();
                return;
            }
            else
            {
                var tk = new User_Log();
                tk.Username = txtUsername.Text;
                tk.Password = txtPassWord.Text;
                tk.Role = 2;
                tk.Status = 3;
                tk.FullName = txtTen.Text;
                db.User_Logs.InsertOnSubmit(tk);

                db.SubmitChanges();
                MessageBox.Show("Thêm mới tài khoản thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtUsername.Text = txtPassWord.Text = txtTen.Text = null;
            }
           
        }

        private void FormDangKy_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
           
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormLogin f = new FormLogin();
            f.Show();
            linkLogin.LinkVisited = true;
        }

        private void btnExist_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
