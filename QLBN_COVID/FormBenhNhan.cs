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
    public partial class FormBenhNhan : Form
    {
        public FormBenhNhan()
        {
            InitializeComponent();
        }
        private DataGridViewRow r;
        private CovidDataContext db;
        private void cbxPlaceOfTreatment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormBenhNhan_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
            showData();
            dataPatient.Columns["FullName"].HeaderText = "Họ và tên";
            dataPatient.Columns["YearOfBirth"].HeaderText = "Năm sinh";
            dataPatient.Columns["Address"].HeaderText = "Địa chỉ";
            dataPatient.Columns["Kind_Of_Status"].HeaderText = "Trạng thái";
            dataPatient.Columns["Name"].HeaderText = "Nơi điều trị";
            dataPatient.Columns["People_Involved"].HeaderText = "Người liên quan";

        }

        private void showData()
        {
            var rs = from p in db.Patients
                     join a in db.Addresses on p.IDAddress equals a.IDAddress
                     join c in db.Cities on a.IDCity equals c.IDCity
                     join d in db.Districts on a.IDDistrict equals d.IDDistrict
                     join w in db.Wards on a.IDWard equals w.IDWard
                     join s in db.Status on p.IDStatus equals s.IDStatus
                     join t in db.Place_Of_Treatments on p.IDTreatment equals t.ID
                     select new
                     {
                         p.CMND,
                         p.FullName,
                         p.YearOfBirth,
                         Address = a.Street + ", " + w.Name_of_ward + ", " + d.Name_of_district + ", " + c.Name_of_city,
                         s.Kind_Of_Status,
                         t.Name,
                         p.People_Involved
                     };
            dataPatient.DataSource = rs;
        }
        private void dataPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r =dataPatient.Rows[e.RowIndex];
                txtCMND.Text = r.Cells["CMND"].Value.ToString();
                txtFullName.Text = r.Cells["FullName"].Value.ToString();
                txtYearOfBirth.Text = r.Cells["YearOfBirth"].Value.ToString();
                txtAddress.Text = r.Cells["Address"].Value.ToString();
               
            }
        }




        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần xóa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (
                 MessageBox.Show("Bạn thực sự muốn xóa bệnh nhân  " + r.Cells["CMND"].Value.ToString() + " ?",
                                      "Xác nhận xóa",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question) == DialogResult.Yes
                )
            {
                try
                {
                    var us = db.Patients.SingleOrDefault(x => x.CMND == r.Cells["CMND"].Value.ToString());
                    db.Patients.DeleteOnSubmit(us);
                    db.SubmitChanges();
                    MessageBox.Show("Xóa bệnh nhân  thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Xóa bệnh nhân  thất bại!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    showData();
                    txtCMND.Text = txtFullName.Text = txtYearOfBirth.Text = txtAddress.Text = null;
                    r = null;
                }
            }
        }

       
    }
}
