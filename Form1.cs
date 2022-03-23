using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rotating
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Form2 form2;
        bool created = false;
        private void button1_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            form2.Show();
            created = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (created)
                if (form2.stopped)
                {
                    form2.Dispose();      
                }
        }
    }
}
