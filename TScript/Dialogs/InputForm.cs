using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TScript.Dialogs
{
    public partial class InputForm : Form
    {
        public string Result { get; set; }

        public InputForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtInput.Text == "")
                return;
            else
            {
                Result = txtInput.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void InputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Result == "")
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
