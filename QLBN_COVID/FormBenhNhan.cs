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
            cbxCity.DataSource = db.Cities;
            cbxCity.DisplayMember = "Name_of_city";
            cbxCity.ValueMember = "IDCity";
            cbxCity.SelectedIndex = -1;


            cbxStatus.DataSource = db.Status;
            cbxStatus.DisplayMember = "Kind_of_status";
            cbxStatus.ValueMember = "IdStatus";
            cbxStatus.SelectedIndex = -1;


            cbxWard.DataSource = db.Wards;
            cbxWard.DisplayMember = "Name_of_ward";
            cbxWard.ValueMember = "IDWard";
            cbxWard.SelectedIndex = -1;

            cbxDistrict.DataSource = db.Districts;
            cbxDistrict.DisplayMember = "Name_of_district";
            cbxDistrict.ValueMember = "IDDistrict";
            cbxDistrict.SelectedIndex = -1;


            cbxPlaceOfTreatment.DataSource = db.Place_Of_Treatments;
            cbxPlaceOfTreatment.DisplayMember = "Name";
            cbxPlaceOfTreatment.ValueMember = "ID";
            cbxPlaceOfTreatment.SelectedIndex = -1;
            //------------------------------------------------------------
            cbxPeopleInvolved.DataSource = db.Patients;
            cbxPeopleInvolved.DisplayMember = "People_Involved";
            //cbxPeopleInvolved.ValueMember = "CMND";
            cbxPeopleInvolved.SelectedIndex = -1;
            //------------------------------------------------------------
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
            if (string.IsNullOrEmpty(txtCMND.Text))
            {
                MessageBox.Show("Vui lòng nhập CMND", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCMND.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtYearOfBirth.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYearOfBirth.Select();
                return;
            }

            var nv = new Patient();
            nv.CMND = txtCMND.Text;          
            nv.FullName = txtFullName.Text;
            nv.YearOfBirth = int.Parse(txtYearOfBirth.Text);
            nv.Address.IDWard = int.Parse(cbxWard.SelectedValue.ToString());
            nv.Address.IDDistrict = int.Parse(cbxDistrict.SelectedValue.ToString());
            nv.Address.IDCity = int.Parse(cbxCity.SelectedValue.ToString());
            nv.Address.Street = txtAddress.Text;

            nv.Place_Of_Treatment.Name = cbxPlaceOfTreatment.SelectedValue.ToString();
            nv.Status.Kind_Of_Status = cbxStatus.SelectedIndex.ToString();
            nv.People_Involved = cbxPeopleInvolved.SelectedValue.ToString();
            
            db.Patients.InsertOnSubmit(nv);
            db.SubmitChanges();
            MessageBox.Show("Thêm mới nhân viên thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            showData();
            txtCMND.Text=txtFullName.Text=txtYearOfBirth.Text=txtAddress.Text = null;
            
            cbxPeopleInvolved.SelectedIndex =cbxPlaceOfTreatment.SelectedIndex=cbxCity.SelectedIndex
                =cbxDistrict.SelectedIndex=cbxWard.SelectedIndex=cbxStatus.SelectedIndex= -1;
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
