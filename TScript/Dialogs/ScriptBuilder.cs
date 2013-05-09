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

namespace TScript.Dialogs
{
    public partial class ScriptBuilder : Form
    {
        bool scriptVerified = false;

        Interpreter main;

        public ScriptBuilder()
        {
            InitializeComponent();
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

        }

        private void runToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Run();
        }

        private void verifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Verify();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}
