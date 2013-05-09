namespace Tarynn.Dialogs
{
    partial class RelateQueryDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStatic = new System.Windows.Forms.TextBox();
            this.btnStatic = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSelectedScript = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewScript = new System.Windows.Forms.Button();
            this.btnExistingScript = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstStatements = new System.Windows.Forms.ListBox();
            this.btnExistingStatement = new System.Windows.Forms.Button();
            this.dlgChoose = new System.Windows.Forms.OpenFileDialog();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtStatic);
            this.groupBox1.Controls.Add(this.btnStatic);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Static Response";
            // 
            // txtStatic
            // 
            this.txtStatic.Location = new System.Drawing.Point(7, 20);
            this.txtStatic.Name = "txtStatic";
            this.txtStatic.Size = new System.Drawing.Size(386, 20);
            this.txtStatic.TabIndex = 1;
            // 
            // btnStatic
            // 
            this.btnStatic.Location = new System.Drawing.Point(248, 46);
            this.btnStatic.Name = "btnStatic";
            this.btnStatic.Size = new System.Drawing.Size(145, 23);
            this.btnStatic.TabIndex = 0;
            this.btnStatic.Text = "Write Static Response";
            this.btnStatic.UseVisualStyleBackColor = true;
            this.btnStatic.Click += new System.EventHandler(this.btnStatic_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSelectedScript);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnNewScript);
            this.groupBox2.Controls.Add(this.btnExistingScript);
            this.groupBox2.Location = new System.Drawing.Point(13, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 109);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Script";
            // 
            // lblSelectedScript
            // 
            this.lblSelectedScript.AutoSize = true;
            this.lblSelectedScript.Location = new System.Drawing.Point(131, 54);
            this.lblSelectedScript.Name = "lblSelectedScript";
            this.lblSelectedScript.Size = new System.Drawing.Size(82, 13);
            this.lblSelectedScript.TabIndex = 3;
            this.lblSelectedScript.Text = "Selected Script:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "OR";
            // 
            // btnNewScript
            // 
            this.btnNewScript.Location = new System.Drawing.Point(6, 79);
            this.btnNewScript.Name = "btnNewScript";
            this.btnNewScript.Size = new System.Drawing.Size(121, 23);
            this.btnNewScript.TabIndex = 1;
            this.btnNewScript.Text = "Write New Script";
            this.btnNewScript.UseVisualStyleBackColor = true;
            this.btnNewScript.Click += new System.EventHandler(this.btnNewScript_Click);
            // 
            // btnExistingScript
            // 
            this.btnExistingScript.Location = new System.Drawing.Point(6, 19);
            this.btnExistingScript.Name = "btnExistingScript";
            this.btnExistingScript.Size = new System.Drawing.Size(121, 23);
            this.btnExistingScript.TabIndex = 0;
            this.btnExistingScript.Text = "Choose Existing Script";
            this.btnExistingScript.UseVisualStyleBackColor = true;
            this.btnExistingScript.Click += new System.EventHandler(this.btnExistingScript_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstStatements);
            this.groupBox3.Controls.Add(this.btnExistingStatement);
            this.groupBox3.Location = new System.Drawing.Point(13, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(399, 158);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Exisiting Statements";
            // 
            // lstStatements
            // 
            this.lstStatements.FormattingEnabled = true;
            this.lstStatements.Location = new System.Drawing.Point(7, 19);
            this.lstStatements.Name = "lstStatements";
            this.lstStatements.Size = new System.Drawing.Size(235, 134);
            this.lstStatements.TabIndex = 1;
            // 
            // btnExistingStatement
            // 
            this.btnExistingStatement.Location = new System.Drawing.Point(248, 19);
            this.btnExistingStatement.Name = "btnExistingStatement";
            this.btnExistingStatement.Size = new System.Drawing.Size(145, 44);
            this.btnExistingStatement.TabIndex = 0;
            this.btnExistingStatement.Text = "Relate To Existing Statement";
            this.btnExistingStatement.UseVisualStyleBackColor = true;
            this.btnExistingStatement.Click += new System.EventHandler(this.btnExistingStatement_Click);
            // 
            // dlgChoose
            // 
            this.dlgChoose.FileName = "openFileDialog1";
            this.dlgChoose.Filter = "Tarynn Scripts|*.ynn";
            this.dlgChoose.InitialDirectory = ".";
            this.dlgChoose.Title = "Select Script";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(261, 375);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(145, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // RelateQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 403);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RelateQueryDialog";
            this.Text = "Relate Query";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStatic;
        private System.Windows.Forms.TextBox txtStatic;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExistingScript;
        private System.Windows.Forms.Button btnNewScript;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstStatements;
        private System.Windows.Forms.Button btnExistingStatement;
        private System.Windows.Forms.Label lblSelectedScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog dlgChoose;
        private System.Windows.Forms.Button btnOk;

    }
}