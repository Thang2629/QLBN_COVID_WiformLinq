using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
   public class RegexValidator
    {
        ErrorProvider errorProvider = new ErrorProvider();
        public string ErrorMessage { get; set; }

        Regex regex;

        List<Control> listControl = new List<Control>();

        public RegexValidator(string strPattem)
        {
            regex = new Regex(strPattem);

        }
        public void AddControl(Control ctrl)
        {
            listControl.Add(ctrl);
            ctrl.Validating += Control2Validate_Validating;
        }

        private void Control2Validate_Validating(object sender, CancelEventArgs e)
        {
            Control ctrl = (Control)sender;
            if (!regex.IsMatch(ctrl.Text))
            {
                errorProvider.SetError(ctrl, ErrorMessage);
            }
            else
            {
                errorProvider.SetError(ctrl, "");
            }
        }
    }
}

