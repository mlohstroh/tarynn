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
            //nothing
            finalStatement.ScriptName = "";
            FinalQuery.AttachedStatement = finalStatement;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
