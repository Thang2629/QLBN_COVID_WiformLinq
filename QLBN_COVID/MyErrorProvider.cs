using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
    class MyErrorProvider: Component
    {

        ErrorProvider errorProvider = new ErrorProvider();
        public string ErrorMessage { get; set; }
        public string RegExString { get; set; }
        List<Control> listControl = new List<Control>();
        public void AddControl(Control ctrl)
        {
            listControl.Add(ctrl);
            ctrl.Validating += Control2Validate_Validating;
        }

        Control control2Validate;
        [TypeConverter(typeof(ReferenceConverter))]
        public Control ControlToValidate
        {
            get => control2Validate;
            set
            {
                if (control2Validate != null && !DesignMode)

                {
                    control2Validate.Validating -= Control2Validate_Validating;
                }
                control2Validate = value;
                if (control2Validate != null && !DesignMode)
                {
                    control2Validate.Validating += Control2Validate_Validating;
                }
            }
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

