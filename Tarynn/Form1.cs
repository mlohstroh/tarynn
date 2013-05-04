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

namespace Tarynn
{
    public partial class Form1 : Form
    {
        Tarynn.Core.Tarynn t = new Tarynn.Core.Tarynn();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Me:" + textBox1.Text + "\n";
            richTextBox1.Text += "Tarynn:" + t.SubmitQuery(textBox1.Text) + "\n";
            textBox1.Text = "";
        }
    }
}
