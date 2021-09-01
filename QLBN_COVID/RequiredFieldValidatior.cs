using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
   public class RequiredFieldValidatior
    {
        ErrorProvider errorProvider = new ErrorProvider();
        public string ErrorMessage { get; set; }

        List<Control> listControl = new List<Control>();
        public void AddControl(Control ctrl)
        {
            listControl.Add(ctrl);
            ctrl.Validating += Control2Validate_Validating;
        }

        private void Control2Validate_Validating(object sender, CancelEventArgs e)
        {
            Control ctrl = (Control)sender;
            if (ctrl.Text.Length == 0)
            {
                errorProvider.SetError(ctrl, "you must enter imfomation");
            }
            else
            {
                errorProvider.SetError(ctrl, "");
            }
        }
    }
}

