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

        
    }
}
