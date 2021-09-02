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
    public partial class FormLichSuDieuTri : Form
    {
        public FormLichSuDieuTri()
        {
            InitializeComponent();
        }
        private CovidDataContext db;
        private void showData()
        {
            var s = from a in db.History_Treatments
                    join u in db.Patients on a.IDBN equals u.CMND
                    join t in db.Place_Of_Treatments on a.IDTreatment equals t.ID
                    join st in db.Status on a.IDStatus equals st.IDStatus
                    where a.IDBN == FormBenhNhan.patient.CMND
                    select new
                    {
                        u.FullName,
                        a.DateStart,
                        a.DateEnd,
                        t.Name,
                        st.Kind_Of_Status
                    };
            dataHistory.DataSource = s;

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var s = from a in db.History_Treatments
                    join u in db.Patients on a.IDBN equals u.CMND
                    join t in db.Place_Of_Treatments on a.IDTreatment equals t.ID
                    join st in db.Status on a.IDStatus equals st.IDStatus
                    where a.DateStart == dateStart.Value && a.DateEnd == dateEnd.Value
                    select new
                    {
                        u.FullName,
                        a.DateStart,
                        a.DateEnd,
                        t.Name,
                        st.Kind_Of_Status
                    };
            dataHistory.DataSource = s;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            showData();
        }
        private Boolean isMaximize = false;
        private void ptbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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
            this.Hide();
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

        private void FormLichSuDieuTri_Load(object sender, EventArgs e)
        { 
            db = new CovidDataContext();
            showData();
        }
    }
}
