using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBN_COVID.DB;

namespace QLBN_COVID
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {

            InitializeComponent();
        }
        public String recapcha;
        public String newcapcha;
        public String validCapcha()
        {
            String capcha;
            Random rd = new Random();
            capcha = rd.Next(1000, 9000).ToString();
            return capcha;
        }
        private void FormLogin_Load(object sender, EventArgs e)
        {

            this.lblCapcha.Text = this.validCapcha();
            this.recapcha = this.lblCapcha.Text;
        }
        private void ptbRefresh_Click(object sender, EventArgs e)
        {
            newcapcha = validCapcha();
            this.lblCapcha.Text = this.newcapcha;
        }

        //private CovidDataContext db;
        public static User_Log user;
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text) || string.IsNullOrEmpty(txtCapcha.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ tài khoản, mật khẩu và mã xác nhận", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Select();
                return;
            }
            CovidDataContext db = new CovidDataContext();
            var tk = db.User_Logs.SingleOrDefault(x => x.Username == txtUser.Text && x.Password == txtPass.Text);
            if (tk != null && (txtCapcha.Text.Equals(recapcha) || txtCapcha.Text.Equals(newcapcha)))
            {
                if(tk.Status == 1)
                {
                    if (tk.Role == 1)
                    {
                        user = tk;

                        this.Hide();
                        FormMain f = new FormMain();
                        f.Show();
                        FormMain.mdiobj.placeOfTreatmentToolStripMenuItem.Enabled = true;
                        FormMain.mdiobj.userToolStripMenuItem.Enabled = true;
                        var userAcitvity = new User_Activity();
                        userAcitvity.Action = "Đăng nhập";
                        userAcitvity.Timestamp = DateTime.Now;
                        userAcitvity.UserID = tk.ID;
                        db.User_Activities.InsertOnSubmit(userAcitvity);
                        db.SubmitChanges();
                    }
                    else
                    {
                        user = tk;
                        this.Hide();
                        FormMain f = new FormMain();
                        f.Show();
                        var userAcitvity = new User_Activity();
                        userAcitvity.Action = "Đăng nhập";
                        userAcitvity.Timestamp = DateTime.Now;
                        userAcitvity.UserID = tk.ID;
                        db.User_Activities.InsertOnSubmit(userAcitvity);
                        db.SubmitChanges();
                    }
                }
                else if(tk.Status == 2)
                {
                    MessageBox.Show("Tài khoản của bạn đã bị khoá\nVui lòng liên hệ trực tiếp với admin để được kiểm tra", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUser.Select();
                    return;
                }
                else if(tk.Status == 3)
                {
                    MessageBox.Show("Tài khoản của bạn đang chờ admin phê duyệt\nVui lòng liên hệ trực tiếp với admin để được phê duyệt ngay lập tức", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUser.Select();
                    return;
                }    
                  

            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại tài khoản, mật khẩu hoặc mã xác nhận", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Select();
                return;
            }

            
        }

    
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkRegistry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormDangKy f = new FormDangKy();
            f.Show();
            linkRegistry.LinkVisited = true;
        }
    }
}
