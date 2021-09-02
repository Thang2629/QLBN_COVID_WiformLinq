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
    public partial class FormLichSuHoatDong : Form
    {
        public FormLichSuHoatDong()
        {
            InitializeComponent();
        }
        private CovidDataContext db;
        private void FormLichSuHoatDong_Load(object sender, EventArgs e)
        {
            if (FormUser.user != null)
            {
                lblUser.Text = "Lịch sử hoạt động của: "+FormUser.user.FullName;
            }
            db = new CovidDataContext();
            showData();
        }
        private void showData()
        { 
            var rs = from h in db.User_Activities
                     join u in db.User_Logs on h.UserID equals u.ID
                     where h.UserID == FormUser.user.ID
                     select new
                     {
                         h.IDActivity,
                         u.Username,
                         h.Timestamp,
                         h.Action
                     };
            dataHistory.DataSource = rs;


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var s = from a in db.User_Activities
                    join u in db.User_Logs on a.UserID equals u.ID
                    where a.Timestamp >= dateStart.Value && a.Timestamp <= dateEnd.Value
                    select new
                    {
                        a.IDActivity,
                        u.Username,
                        a.Timestamp,
                        a.Action
                    };
            dataHistory.DataSource = s;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            showData();
        }

        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

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
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void pnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        
    }
}
