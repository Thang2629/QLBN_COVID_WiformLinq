using QLBN_COVID.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
    public partial class FormMain : Form
    {
        public static FormMain mdiobj;
        public FormMain()
        {
            InitializeComponent();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ptbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private Boolean isMaximize = false;
        private void ptbResize_Click(object sender, EventArgs e)
        {
            if (!isMaximize)
            {
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            isMaximize = !isMaximize;
        }

        private void ptbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public User_Log user;
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (FormLogin.user != null)
            {
                lblUserLogin.Text = FormLogin.user.FullName;
            }    
            placeOfTreatmentToolStripMenuItem.Enabled = false;
            userToolStripMenuItem.Enabled = false;
            mdiobj = this;
            //var f = new FormLogin();
            //f.ShowDialog();
            //var nv = f.user;
            //MessageBox.Show("Nhân viên đang đăng nhập: " + nv.FullName,"Welcom");
            //lblUserLogin.Text = nv.FullName;
        }

        private void addForm(Form f)
        {
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            f.TopMost = true;
            grContent.Controls.Clear();
            grContent.Controls.Add(f);
            f.Show();
        }

        private void PatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormBenhNhan();
            addForm(f);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();

            FormLogin fm = new FormLogin();
            fm.Show();
        }

        private void placeOfTreatmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormNoiDieuTri();
            addForm(f);
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormUser();
            addForm(f);
        }
    }
}
