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

namespace TScript.Dialogs
{
    public partial class ScriptBuilder : Form
    {
        bool scriptVerified = false;
        bool latestSaved;

        Interpreter main;

        public ScriptBuilder()
        {
            InitializeComponent();
            dlgOpen.FileOk += dlgOpen_FileOk;
            dlgSave.FileOk += dlgSave_FileOk;
        }

        void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            rchScript.SaveFile(dlgSave.FileName);
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

        }

        private void Run()
        {

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
            latestSaved = true;
            dlgSave.ShowDialog();
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
            rchScript.LoadFile(dlgOpen.FileName);
        }

        private void rchScript_TextChanged(object sender, EventArgs e)
        {
            latestSaved = false;
        }
    }
}
