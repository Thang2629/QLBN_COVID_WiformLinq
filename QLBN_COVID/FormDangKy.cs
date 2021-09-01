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
        private DataGridViewRow r;
        private CovidDataContext db;


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
          
        }

        private void btnAdd_Click(object sender, EventArgs e)
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

        private void FormDangKy_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
           
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
