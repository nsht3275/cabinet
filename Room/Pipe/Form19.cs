using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pipe;

namespace Room
{
    public partial class Form19 : Form
    {
        public int plankid = 0;
        public Form19()
        {
            InitializeComponent();
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            if (plankid > 0)
            {
                int type = Form1.g_form1.axEWdraw3.GetPlankType(plankid);
                double w,h,t,a;
                w = h = t = a = 0.0;
                string name,cname;
                name = "";
                cname = "";
                Form1.g_form1.axEWdraw3.GetPlankWHTA(plankid, ref w, ref h, ref t,ref a, ref name, ref cname);
                switch (type)
                {
                    case 0:
                        {//层板
                            textBox1.Text = w.ToString("0.0");
                            textBox2.Text = h.ToString("0.0");
                            textBox3.Text = t.ToString("0.0");
                        }
                        break;
                    case 1://竖板
                        {
                            label1.Text = "高度";
                            textBox1.Text = w.ToString("0.0");
                            textBox2.Text = h.ToString("0.0");
                            textBox3.Text = t.ToString("0.0");

                        }
                        break;
                    case 2:
                        {//面板
                            label2.Text = "高度";
                            textBox1.Text = w.ToString("0.0");
                            textBox2.Text = h.ToString("0.0");
                            textBox3.Text = t.ToString("0.0");
                        }
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
