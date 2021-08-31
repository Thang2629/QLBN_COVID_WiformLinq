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

        private void FormUser_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
            Showdata();

            dataUser.Columns["ID"].HeaderText = "id";
            dataUser.Columns["Username"].HeaderText = "tai khoan";
            dataUser.Columns["Password"].HeaderText = "Mat Khau";
            dataUser.Columns["Role"].HeaderText = "Quyen";
            dataUser.Columns["FullName"].HeaderText = "ten";
            dataUser.Columns["Status"].HeaderText = "trang thai";
        }
    }
}
