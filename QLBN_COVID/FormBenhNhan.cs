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

            cbxStatus.DataSource = db.Status;
            cbxStatus.DisplayMember = "Kind_Of_Status";
            cbxStatus.ValueMember = "IDStatus";
            cbxStatus.SelectedIndex = -1;


            cbxCity.DataSource = db.Cities;
            cbxCity.DisplayMember = "Name_of_city";
            cbxCity.ValueMember = "IDCity";
            cbxCity.SelectedIndex = -1;

            cbxDistrict.DataSource = db.Districts;
            cbxDistrict.DisplayMember = "Name_of_district";
            cbxDistrict.ValueMember = "IDDistrict";
            cbxDistrict.SelectedIndex = -1;

            cbxWard.DataSource = db.Wards;
            cbxWard.DisplayMember = "Name_of_ward";
            cbxWard.ValueMember = "IDWard";
            cbxWard.SelectedIndex = -1;

            cbxPlaceOfTreatment.DataSource = db.Place_Of_Treatments;
            cbxPlaceOfTreatment.DisplayMember = "Name";
            cbxPlaceOfTreatment.ValueMember = "ID";
            cbxPlaceOfTreatment.SelectedIndex = -1;

            var query = (from c in db.Patients
                         select c.CMND);

            cbxPeopleInvolved.DataSource = query;
            cbxPeopleInvolved.SelectedIndex = -1;

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
            if (dataPatient.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                r = dataPatient.Rows[e.RowIndex];
                dataPatient.CurrentRow.Selected = true;
                txtCMND.Text = dataPatient.Rows[e.RowIndex].Cells["CMND"].FormattedValue.ToString();
                txtFullName.Text = dataPatient.Rows[e.RowIndex].Cells["FullName"].FormattedValue.ToString();
                txtYearOfBirth.Text = dataPatient.Rows[e.RowIndex].Cells["YearOfBirth"].FormattedValue.ToString();
                cbxStatus.Text = dataPatient.Rows[e.RowIndex].Cells["Kind_Of_Status"].FormattedValue.ToString();
                cbxPlaceOfTreatment.Text = dataPatient.Rows[e.RowIndex].Cells["Name"].FormattedValue.ToString();
                cbxPeopleInvolved.Text = dataPatient.Rows[e.RowIndex].Cells["People_Involved"].FormattedValue.ToString();
                string address = dataPatient.Rows[e.RowIndex].Cells["Address"].FormattedValue.ToString();
                char[] delimiterChars = { ',' };
                string[] addressList = address.Split(delimiterChars);
                txtAddress.Text = addressList[0].Trim();
                cbxWard.Text = addressList[1].Trim();
                cbxDistrict.Text = addressList[2].Trim();
                cbxCity.Text = addressList[3].Trim();
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            if(txtCMND.Enabled == true)
            {

                
            if (string.IsNullOrEmpty(txtCMND.Text))
            {
                MessageBox.Show("Vui lòng nhập CMND", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCMND.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtYearOfBirth.Text))
            {
                MessageBox.Show("Vui lòng nhập năm sinh", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYearOfBirth.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Select();
                return;
            }
            if (cbxPlaceOfTreatment.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nơi điều trị", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxCity.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxDistrict.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxWard.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxPeopleInvolved.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chọn CMND người liên quan", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var patient = new Patient();
            patient.CMND = txtCMND.Text;
            patient.FullName = txtFullName.Text;
            patient.YearOfBirth = long.Parse(txtYearOfBirth.Text);
            var add = db.Addresses.SingleOrDefault(a => a.IDCity == (int.Parse(cbxCity.SelectedValue.ToString())) &&
            a.IDDistrict == (int.Parse(cbxDistrict.SelectedValue.ToString())) && a.IDWard == (int.Parse(cbxWard.SelectedValue.ToString())) &&
            a.Street.Equals(txtAddress.Text));
            patient.IDAddress = add.IDAddress;
            patient.IDStatus = int.Parse(cbxStatus.SelectedValue.ToString());
            patient.IDTreatment = int.Parse(cbxPlaceOfTreatment.SelectedValue.ToString());
            patient.People_Involved = cbxPeopleInvolved.SelectedValue.ToString();
            db.Patients.InsertOnSubmit(patient);
            db.SubmitChanges();

             var treatment = db.Place_Of_Treatments.SingleOrDefault(x => x.ID == int.Parse(cbxPlaceOfTreatment.SelectedValue.ToString()));
                treatment.Current_Quantity = treatment.Current_Quantity - 1;
                db.SubmitChanges();

            var userAcitvity = new User_Activity();
            userAcitvity.UserID = FormLogin.user.ID;
            userAcitvity.Timestamp = DateTime.Now;
            userAcitvity.Action = "Thêm Bệnh nhân: " + txtFullName.Text;
            db.User_Activities.InsertOnSubmit(userAcitvity);
            db.SubmitChanges();

            showData();
            MessageBox.Show("Thêm mới bệnh nhân thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtCMND.Text = null;
            txtFullName.Text = null;
            txtYearOfBirth.Text = null;
            txtAddress.Text = null;
            cbxCity.SelectedIndex = -1;
            cbxDistrict.SelectedIndex = -1;
            cbxWard.SelectedIndex = -1;
            cbxStatus.SelectedIndex = -1;
            cbxPlaceOfTreatment.SelectedIndex = -1;
            cbxPeopleInvolved.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Vui lòng ấn nút Thêm trước ", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCMND.Text))
            {
                MessageBox.Show("Vui lòng nhập CMND", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCMND.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtYearOfBirth.Text))
            {
                MessageBox.Show("Vui lòng nhập năm sinh", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYearOfBirth.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Select();
                return;
            }
            if (cbxPlaceOfTreatment.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nơi điều trị", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxCity.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxDistrict.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxWard.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxPeopleInvolved.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chọn CMND người liên quan", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patient = db.Patients.SingleOrDefault(x => x.CMND.Equals(txtCMND.Text));
            patient.CMND = txtCMND.Text;
            patient.FullName = txtFullName.Text;
            patient.YearOfBirth = long.Parse(txtYearOfBirth.Text);
            var add = db.Addresses.SingleOrDefault(a => a.IDCity == (int.Parse(cbxCity.SelectedValue.ToString())) &&
            a.IDDistrict == (int.Parse(cbxDistrict.SelectedValue.ToString())) && a.IDWard == (int.Parse(cbxWard.SelectedValue.ToString())) &&
            a.Street.Equals(txtAddress.Text));
            patient.IDAddress = add.IDAddress;
            patient.IDStatus = int.Parse(cbxStatus.SelectedValue.ToString());
            patient.IDTreatment = int.Parse(cbxPlaceOfTreatment.SelectedValue.ToString());
            patient.People_Involved = cbxPeopleInvolved.SelectedValue.ToString();
            db.SubmitChanges();

            //Thêm hành động vào lịch sử hoạt động
            var userAcitvity = new User_Activity();
            userAcitvity.Action = "Sửa thông tin bệnh nhân: " + txtFullName.Text;
            userAcitvity.Timestamp = DateTime.Now;
            userAcitvity.UserID = FormLogin.user.ID;
            db.User_Activities.InsertOnSubmit(userAcitvity);
            db.SubmitChanges();

            //Thêm vào lịch sử điều trị - TODO
            var p = db.Patients.SingleOrDefault(h => h.CMND.Equals(r.Cells["CMND"].Value.ToString()));
            var status = db.Status.SingleOrDefault(s => s.Kind_Of_Status.Equals(r.Cells["Status"].Value.ToString()));
            var treatment = db.Place_Of_Treatments.SingleOrDefault(s => s.Name.Equals(r.Cells["Name"].Value.ToString()));
            var history = new History_Treatment();
            if (p.IDStatus != status.IDStatus)
            {
                history.IDStatus = p.IDStatus;
            }
            if(p.IDTreatment != treatment.ID)
            {
                history.IDTreatment = p.IDTreatment;
            }    


            showData();
            MessageBox.Show("Sửa thông tin bệnh nhân thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);


            txtCMND.Text = null;
            txtFullName.Text = null;
            txtYearOfBirth.Text = null;
            txtAddress.Text = null;
            cbxCity.SelectedIndex = -1;
            cbxDistrict.SelectedIndex = -1;
            cbxWard.SelectedIndex = -1;
            cbxStatus.SelectedIndex = -1;
            cbxPlaceOfTreatment.SelectedIndex = -1;
            cbxPeopleInvolved.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần xóa", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (
                MessageBox.Show(
                        "Bạn có muốn xóa thông tin bệnh nhân: " + r.Cells["FullName"].Value.ToString() + "?",
                        "Xác nhận xóa thông tin bệnh nhân",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    ) == DialogResult.Yes
             )
            {
                try
                {
                    var patient = db.Patients.SingleOrDefault(x => x.CMND.Equals(txtCMND.Text));
                    db.Patients.DeleteOnSubmit(patient);
                    db.SubmitChanges();

                    //Thêm hành động vào lịch sử hoạt động
                    var userAcitvity = new User_Activity();
                    userAcitvity.Action = "Xoá thông tin bệnh nhân: " + patient.FullName;
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

                txtCMND.Text = null;
                txtFullName.Text = null;
                txtYearOfBirth.Text = null;
                txtAddress.Text = null;
                cbxCity.SelectedIndex = -1;
                cbxDistrict.SelectedIndex = -1;
                cbxWard.SelectedIndex = -1;
                cbxStatus.SelectedIndex = -1;
                cbxPlaceOfTreatment.SelectedIndex = -1;
                cbxPeopleInvolved.SelectedIndex = -1;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Vui long nhap thong tin can tim", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            var query = from p in db.Patients
                        join a in db.Addresses on p.IDAddress equals a.IDAddress
                        join c in db.Cities on a.IDCity equals c.IDCity
                        join d in db.Districts on a.IDDistrict equals d.IDDistrict
                        join w in db.Wards on a.IDWard equals w.IDWard
                        join s in db.Status on p.IDStatus equals s.IDStatus
                        join t in db.Place_Of_Treatments on p.IDTreatment equals t.ID
                        where (p.CMND == txtCMND.Text) || (p.FullName == txtFullName.Text)
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
            if (query != null)
            {
                dataPatient.DataSource = query;
            }
            else
            {
                MessageBox.Show("Không tìm thấy bệnh nhân nào", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearch.Select();
                return;
            }
        }
        public static Patient patient;

        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần xem lịch sử điều trị", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var tk = db.Patients.SingleOrDefault(x => x.CMND.Equals(txtCMND.Text));
            if (tk != null)
            {
                patient = tk;
                FormLichSuDieuTri f = new FormLichSuDieuTri();
                f.Show();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            showData();
        }
    }
}
