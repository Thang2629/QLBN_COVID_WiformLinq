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
        }
        private DataGridViewRow r;
        private CovidDataContext db;
        private void FormNoiDieuTri_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();

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

            showData();
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
                         Address= a.Street + ", " + w.Name_of_ward + ", "+ d.Name_of_district + ", "+ c.Name_of_city   
                     };
            dataPlaceOfTreatment.DataSource = rs;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlace.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nơi điều trị", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlace.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtCapacity.Text))
            {
                MessageBox.Show("Vui lòng nhập sức chứa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCapacity.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtCurrentNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng hiện tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentNumber.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Select();
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
            try
            {
                var newplace = new Place_Of_Treatment();
                var place = db.Place_Of_Treatments.SingleOrDefault(x => x.Name.Equals(txtPlace.Text));
                if(place != null)
                {
                    MessageBox.Show("Tên cơ sở điều trị này đã tồn tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    var add = db.Addresses.SingleOrDefault(a => a.IDCity == (int.Parse(cbxCity.SelectedValue.ToString())) &&
                  a.IDDistrict == (int.Parse(cbxDistrict.SelectedValue.ToString())) && a.IDWard == (int.Parse(cbxWard.SelectedValue.ToString())) &&
                  a.Street.Equals(txtAddress.Text));
                    if (add != null)
                    {
                        MessageBox.Show("Địa chỉ này đã tồn tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        newplace.Name = txtPlace.Text;
                        newplace.Capacity = int.Parse(txtCapacity.Text);
                        newplace.Current_Quantity = int.Parse(txtCurrentNumber.Text);



                        var newAdd = new Address();
                        newAdd.IDCity = int.Parse(cbxCity.SelectedValue.ToString());
                        newAdd.IDDistrict = int.Parse(cbxDistrict.SelectedValue.ToString());
                        newAdd.IDWard = int.Parse(cbxWard.SelectedValue.ToString());
                        newAdd.Street = txtAddress.Text;
                        db.Addresses.InsertOnSubmit(newAdd);
                        db.SubmitChanges();

                        newplace.IDAddress = newAdd.IDAddress;
                        db.Place_Of_Treatments.InsertOnSubmit(newplace);
                        db.SubmitChanges();

                        var userAcitvity = new User_Activity();
                        userAcitvity.UserID = FormLogin.user.ID;
                        userAcitvity.Timestamp = DateTime.Now;
                        userAcitvity.Action = "Thêm nơi điều trị: " + txtPlace.Text;
                        db.User_Activities.InsertOnSubmit(userAcitvity);
                        db.SubmitChanges();
                        showData();
                        MessageBox.Show("Thêm thông tin nơi điều trị thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    
                }
              


                
            }
            catch {
                MessageBox.Show("", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtPlace.Text = null;
            txtCapacity.Text = null;
            txtCurrentNumber.Text = null;
            txtAddress.Text = null;
            cbxCity.SelectedIndex = -1;
            cbxDistrict.SelectedIndex = -1;
            cbxWard.SelectedIndex = -1;


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlace.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nơi điều trị", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlace.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtCapacity.Text))
            {
                MessageBox.Show("Vui lòng nhập sức chứa", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCapacity.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtCurrentNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng hiện tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentNumber.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Select();
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
            try
            {
                var place = db.Place_Of_Treatments.SingleOrDefault(x => x.ID == int.Parse(r.Cells["ID"].Value.ToString()));
                var add = db.Addresses.SingleOrDefault(a => a.IDCity == (int.Parse(cbxCity.SelectedValue.ToString())) &&
              a.IDDistrict == (int.Parse(cbxDistrict.SelectedValue.ToString())) && a.IDWard == (int.Parse(cbxWard.SelectedValue.ToString())) &&
              a.Street.Equals(txtAddress.Text));
                if(add != null)
                {
                    MessageBox.Show("Địa chỉ này đã tồn tại", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    place.Name = txtPlace.Text;
                    place.Capacity = int.Parse(txtCapacity.Text);
                    place.Current_Quantity = int.Parse(txtCurrentNumber.Text);

                    var newAdd = new Address();
                    newAdd.IDCity = int.Parse(cbxCity.SelectedValue.ToString());
                    newAdd.IDDistrict = int.Parse(cbxDistrict.SelectedValue.ToString());
                    newAdd.IDWard = int.Parse(cbxWard.SelectedValue.ToString());
                    newAdd.Street = txtAddress.Text;
                    db.Addresses.InsertOnSubmit(newAdd);
                    db.SubmitChanges();
                    place.IDAddress = newAdd.IDAddress;
                    db.SubmitChanges();

                    var userAcitvity = new User_Activity();
                    userAcitvity.UserID = FormLogin.user.ID;
                    userAcitvity.Timestamp = DateTime.Now;
                    userAcitvity.Action = "Cập nhật nơi điều trị: " + txtPlace.Text;
                    db.User_Activities.InsertOnSubmit(userAcitvity);
                    db.SubmitChanges();


                    showData();
                    MessageBox.Show("Sửa thông tin nơi điều trị thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               

            }
            catch {
                MessageBox.Show("", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            //db.SubmitChanges();

            //showData();
            //MessageBox.Show("Sửa thông tin nơi điều trị thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPlace.Text = null;
            txtCapacity.Text = null;
            txtCurrentNumber.Text = null;
            txtAddress.Text = null;
            cbxCity.SelectedIndex = -1;
            cbxDistrict.SelectedIndex = -1;
            cbxWard.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn nơi điều trị cần xóa", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (
                MessageBox.Show(
                        "Bạn có muốn xóa nơi điều trị này: " + r.Cells["Name"].Value.ToString() + "?",
                        "Xác nhận xóa nơi điều trị",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    ) == DialogResult.Yes
             )
            {
                try
                {
                    var place = db.Place_Of_Treatments.SingleOrDefault(x => x.ID == int.Parse(r.Cells["ID"].Value.ToString()));
                    db.Place_Of_Treatments.DeleteOnSubmit(place);
                    db.SubmitChanges();

                    //Thêm hành động vào lịch sử hoạt động
                    var userAcitvity = new User_Activity();
                    userAcitvity.Action = "Xoá nơi điều trị: " + place.Name;
                    userAcitvity.Timestamp = DateTime.Now;
                    userAcitvity.UserID = FormLogin.user.ID;
                    db.User_Activities.InsertOnSubmit(userAcitvity);

                    db.SubmitChanges();
                    MessageBox.Show("Xóa nơi điều trị thành công", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Xóa nơi điều trị thất bại", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                showData();

                txtPlace.Text = null;
                txtCapacity.Text = null;
                txtCurrentNumber.Text = null;
                txtAddress.Text = null;
                cbxCity.SelectedIndex = -1;
                cbxDistrict.SelectedIndex = -1;
                cbxWard.SelectedIndex = -1;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                showData();
            }
            var rs = from p in db.Place_Of_Treatments
                     join a in db.Addresses on p.IDAddress equals a.IDAddress
                     join c in db.Cities on a.IDCity equals c.IDCity
                     join d in db.Districts on a.IDDistrict equals d.IDDistrict
                     join w in db.Wards on a.IDWard equals w.IDWard
                     where (p.Name == txtPlace.Text) 
                     select new
                     {
                         p.ID,
                         p.Name,
                         p.Capacity,
                         p.Current_Quantity,
                         Address = a.Street + ", " + w.Name_of_ward + ", " + d.Name_of_district + ", " + c.Name_of_city
                     };
            if (rs != null)
            {
                dataPlaceOfTreatment.DataSource = rs;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nơi điều trị", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearch.Select();
                return;
            }
        }

        private void dataPlaceOfTreatment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataPlaceOfTreatment.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                r = dataPlaceOfTreatment.Rows[e.RowIndex];
                dataPlaceOfTreatment.CurrentRow.Selected = true;
                txtCapacity.Text = dataPlaceOfTreatment.Rows[e.RowIndex].Cells["Capacity"].FormattedValue.ToString();
                txtPlace.Text = dataPlaceOfTreatment.Rows[e.RowIndex].Cells["Name"].FormattedValue.ToString();
                txtCurrentNumber.Text = dataPlaceOfTreatment.Rows[e.RowIndex].Cells["Current_Quantity"].FormattedValue.ToString();
                string address = dataPlaceOfTreatment.Rows[e.RowIndex].Cells["Address"].FormattedValue.ToString();
                char[] delimiterChars = { ',' };
                string[] addressList = address.Split(delimiterChars);
                txtAddress.Text = addressList[0].Trim();
                cbxWard.Text = addressList[1].Trim();
                cbxDistrict.Text = addressList[2].Trim();
                cbxCity.Text = addressList[3].Trim();
            }
        }
    }
}
