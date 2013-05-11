using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TScript;
using System.IO;
using Analytics;

namespace TScript.Dialogs
{
    public partial class ScriptBuilder : Form
    {
        bool scriptVerified = false;
        bool latestSaved;
        public string FileName { get; set; }

        Interpreter main;

        public ScriptBuilder()
        {
            InitializeComponent();
            dlgOpen.FileOk += dlgOpen_FileOk;
            dlgSave.FileOk += dlgSave_FileOk;
        }



        private void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            FileName = dlgSave.FileName;
            File.WriteAllText(FileName, rchScript.Text);
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            Verify();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Verify()
        {
            Save();
            if (FileName == null)
                return;

            main = new Interpreter(FileName);
            if (!main.Validate())
            {
                MessageBox.Show("Script Failed Validation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Status: Failed";
            }
            else
            {
                MessageBox.Show("Script Passed Validation", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Status: Passed";
                scriptVerified = true;
            }
        }

        private void Run()
        {
            if (!scriptVerified)
            {
                Verify();
            }
            lblOutput.Text = "Output: " + main.GetFinalText();
        }

        //save
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void runToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Run();
        }

        private void verifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Verify();
        }

        private void Save()
        {
            try
            {
                latestSaved = true;
                if (FileName != null)
                    File.WriteAllText(FileName, rchScript.Text);
                else
                    dlgSave.ShowDialog();
            }
            catch(IOException ex)
            {
                TConsole.Error(ex.Message);
            }
        }

        //open
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (rchScript.Text != "" && !latestSaved)
            {
                DialogResult result = MessageBox.Show("Do you want to save what you have first?", "TScript", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        Save();
                        return;
                    case System.Windows.Forms.DialogResult.No:
                        rchScript.Text = "";
                        break;
                }
            }
            dlgOpen.ShowDialog();
        }

        private void dlgOpen_FileOk(object sender, CancelEventArgs e)
        {
            FileName = dlgOpen.FileName;
            rchScript.Text = File.ReadAllText(FileName);
        }

        private void rchScript_TextChanged(object sender, EventArgs e)
        {
            latestSaved = false;
            scriptVerified = false;
        }

        private void ScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scriptVerified && latestSaved)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
