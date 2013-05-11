using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarynn.Core;
using Tarynn.Helpers;
using TScript.Dialogs;

namespace Tarynn.Dialogs
{
    public partial class RelateQueryDialog : Form
    {
        private Statement[] allStatements;
        Statement finalStatement;

        public RelateQueryDialog(Query q)
        {
            InitializeComponent();
            this.allStatements = Statement.All();
            FinalQuery = q;

            //this is the default "failure" result we will use
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            for (int i = 0; i < allStatements.Length; i++)
            {
                lstStatements.Items.Add(allStatements[i].FullText);
            }

            dlgChoose.FileOk += dlgChoose_FileOk;
        }

        public Query FinalQuery { get; set; }

        private void btnStatic_Click(object sender, EventArgs e)
        {
            if (txtStatic.Text == "")
            {
                Toolbox.MessageBoxWithError("You must input something to say back");
                return;
            }

            string staticText = txtStatic.Text;
            finalStatement = new Statement();
            //whatever the query was
            finalStatement.FullText = FinalQuery.OriginalText;
            //just set the static text
            finalStatement.ResponseText = staticText;
            FinalQuery.AttachedStatement = finalStatement;
            lblStatus.Text = "Status: Static Response";
        }

        private void btnExistingStatement_Click(object sender, EventArgs e)
        {
            finalStatement = new Statement();
            Statement selectedStatement = allStatements[lstStatements.SelectedIndex];
            finalStatement.RelatedId = selectedStatement.Id;
            finalStatement.FullText = FinalQuery.OriginalText;
            FinalQuery.AttachedStatement = finalStatement;
            lblStatus.Text = "Status: Existing Statement";
        }

        private void btnExistingScript_Click(object sender, EventArgs e)
        {
            dlgChoose.ShowDialog();            
        }

        void dlgChoose_FileOk(object sender, CancelEventArgs e)
        {
            if (dlgChoose.FileName != "")
            {
                finalStatement = new Statement();
                finalStatement.FullText = FinalQuery.OriginalText;
                finalStatement.ScriptName = dlgChoose.FileName;
                FinalQuery.AttachedStatement = finalStatement;
                lblSelectedScript.Text = "Selected Script: " + dlgChoose.FileName;
                lblStatus.Text = "Status: Using script";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (FinalQuery.AttachedStatement != null)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnNewScript_Click(object sender, EventArgs e)
        {
            ScriptBuilder builder = new ScriptBuilder();
            if (builder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblSelectedScript.Text = "Selected Script: " + builder.FileName;
            }
            finalStatement = new Statement();
            finalStatement.FullText = FinalQuery.OriginalText;
            finalStatement.ScriptName = builder.FileName;
            FinalQuery.AttachedStatement = finalStatement;
            lblStatus.Text = "Status: Using script";
        }
    }
}
