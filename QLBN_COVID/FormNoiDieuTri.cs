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
    public partial class FormNoiDieuTri : Form
    {
        public FormNoiDieuTri()
        {
            InitializeComponent();
            txtPlace.Validating += TxtPlace_Validating;
            txtAddress.Validating += TxtAddress_Validating;
            txtQuantity.Validating += TxtQuantity_Validating;
            txtNumber.Validating += TxtNumber_Validating;
            var requifield = new RequiredFieldValidatior();
            requifield.AddControl(txtPlace);
            requifield.AddControl(txtAddress);
            requifield.AddControl(txtQuantity);
            requifield.AddControl(txtNumber);

            var UnField5 = new RegexValidator(@"^[0-9]$");
            UnField5.ErrorMessage = "invalid";
            UnField5.AddControl(txtQuantity);
            UnField5.AddControl(txtNumber);

            var UnField6 = new RegexValidator(@"^[a-z][a-z0-9\-_\.]*$");
            UnField6.ErrorMessage = "invalid";
            UnField6.AddControl(txtAddress);

            var UnField7 = new RegexValidator(@"^[a-z][a-z0-9\-_\.]*$");
            UnField7.ErrorMessage = "invalid";
            UnField7.AddControl(txtAddress);



        }

        private void TxtNumber_Validating(object sender, CancelEventArgs e)
        {
            if (txtNumber.Text == "")
            {
                MessageBox.Show("khong dc trong ");
                e.Cancel = true;
            }
        }

        private void TxtQuantity_Validating(object sender, CancelEventArgs e)
        {
            if (txtQuantity.Text == "")
            {
                MessageBox.Show("khong dc trong ");
                e.Cancel = true;
            }
        }

        private void TxtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (txtAddress.Text == "")
            {
                MessageBox.Show("khong dc trong ");
                e.Cancel = true;
            }
        }

        private void TxtPlace_Validating(object sender, CancelEventArgs e)
        {
            if (txtPlace.Text == "")
            {
                MessageBox.Show("khong dc trong ");
                e.Cancel = true;
            }
        }

        private DataGridViewRow r;
        private CovidDataContext db;
        private void FormNoiDieuTri_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();

            showData();
            cbCity.DataSource = db.Cities;
            cbCity.DisplayMember = "Name_of_city";
            cbCity.ValueMember = "IDCity";
            cbCity.SelectedIndex = -1;


            cbxWard.DataSource = db.Wards;
            cbxWard.DisplayMember = "Name_of_ward";
            cbxWard.ValueMember = "IDWard";
            cbxWard.SelectedIndex = -1;

            cbxDistrict.DataSource = db.Districts;
            cbxDistrict.DisplayMember = "Name_of_district";
            cbxDistrict.ValueMember = "IDDistrict";
            cbxDistrict.SelectedIndex = -1;

            dataPlaceOfTreatment.Columns["ID"].HeaderText = "ID";
            dataPlaceOfTreatment.Columns["Name"].HeaderText = "Nơi điều trị";
            dataPlaceOfTreatment.Columns["Capacity"].HeaderText = "Sức chứa";
            dataPlaceOfTreatment.Columns["Current_Quantity"].HeaderText = "Số lượng hiện tại";
            dataPlaceOfTreatment.Columns["Address"].HeaderText = "Địa chỉ";

        }
        private void showData()
        {
            var rs = from p in db.Place_Of_Treatments
                     join a in db.Addresses on p.IDAddress equals a.IDAddress
                     join c in db.Cities on a.IDCity equals c.IDCity
                     join d in db.Districts on a.IDDistrict equals d.IDDistrict
                     join w in db.Wards on a.IDWard equals w.IDWard
                     select new
                     {
                         p.ID,
                         p.Name,
                         p.Capacity,
                         p.Current_Quantity,
                         Address = a.Street + ", " + w.Name_of_ward + ", " + d.Name_of_district + ", " + c.Name_of_city
                     };
            dataPlaceOfTreatment.DataSource = rs;
        }

        private void dataPlaceOfTreatment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dataPlaceOfTreatment.Rows[e.RowIndex];
                txtPlace.Text = r.Cells["name"].Value.ToString();
                txtAddress.Text = r.Cells["address"].Value.ToString();
                txtQuantity.Text = r.Cells["capacity"].Value.ToString();
                txtNumber.Text = r.Cells["current_Quantity"].Value.ToString();

            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn nơi điều trị cần xóa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (
                 MessageBox.Show("Bạn thực sự muốn xóa nơi điều trị  " + r.Cells["name"].Value.ToString() + " ?",
                                      "Xác nhận xóa",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question) == DialogResult.Yes
                )
            {
                try
                {
                    var us = db.Place_Of_Treatments.SingleOrDefault(x => x.Name== r.Cells["name"].Value.ToString());
                    db.Place_Of_Treatments.DeleteOnSubmit(us);
                    db.SubmitChanges();
                    MessageBox.Show("Xóa nơi điều trị thành công!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Xóa nơi điều trị thất bại!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    showData();
                    txtPlace.Text = txtAddress.Text = txtQuantity.Text = txtNumber.Text = null;
                    r = null;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlace.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nơi điều trị", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlace.Select();
                return;
            }
            var p = new Place_Of_Treatment();
            //-----------------------------------------
            p.Name = txtPlace.Text;
            p.Capacity = int.Parse(txtQuantity.Text);
            p.Current_Quantity = int.Parse(txtNumber.Text);
            p.Address.IDCity =int.Parse(cbCity.SelectedIndex.ToString());
            p.Address.IDDistrict = int.Parse(cbxDistrict.SelectedValue.ToString());
            p.Address.IDWard = int.Parse(cbxWard.SelectedValue.ToString());
            p.Address.Street = txtAddress.Text;
            db.Place_Of_Treatments.InsertOnSubmit(p);
            db.SubmitChanges();
            

            MessageBox.Show("Thêm mới phòng thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            showData(); 
            txtPlace.Text = txtQuantity.Text = txtNumber.Text=null;
            cbxDistrict.SelectedIndex = -1;
        }

        private void txtPlace_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
