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
            t.EchoEvent += t_EchoEvent;
        }

        private void t_EchoEvent(object sender, TarynnEchoEventArgs e)
        {
            richTextBox1.Text += "Tarynn: " + e.Echo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (text == "")
                return;

            if (text.Contains("run"))
            {
                string[] args = text.Split(':');
                args[1] = args[1].Trim();
                richTextBox1.Text += "Tarynn: " + t.RunScript(args[1]);
            }
            else
            {
                richTextBox1.Text += "Me:" + textBox1.Text + "\n";
                Query q = t.InitialQuery(textBox1.Text);

                if (q.State == QueryState.Unrelated)
                {
                    q = t.RelateQuery(q);
                }
                richTextBox1.Text += "Tarynn: " + q.Respond() + "\n";
            }
        }
    }
}
