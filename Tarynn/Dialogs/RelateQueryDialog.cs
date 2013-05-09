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
        }

        private void btnExistingStatement_Click(object sender, EventArgs e)
        {
            finalStatement = new Statement();
            Statement selectedStatement = allStatements[lstStatements.SelectedIndex];
            finalStatement.RelatedId = selectedStatement.Id;
            FinalQuery.AttachedStatement = finalStatement;
        }

        private void btnExistingScript_Click(object sender, EventArgs e)
        {
            dlgChoose.ShowDialog();
            dlgChoose.FileOk += dlgChoose_FileOk;
        }

        void dlgChoose_FileOk(object sender, CancelEventArgs e)
        {
            if (dlgChoose.FileName != "")
            {
                finalStatement = new Statement();
                finalStatement.ScriptName = dlgChoose.FileName;
                FinalQuery.AttachedStatement = finalStatement;
                lblSelectedScript.Text = "Selected Script: " + dlgChoose.FileName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (FinalQuery.AttachedStatement != null)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
