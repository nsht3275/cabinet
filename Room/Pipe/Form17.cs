using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Room
{
    public partial class Form17 : Form
    {
        public double m_topspace = 40;
        public double m_depthspace = 300;
        public bool   m_isautodepath = true;
        public double m_rad = 15;
        public Form17()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_topspace = Convert.ToDouble(textBox1.Text);
            m_depthspace = Convert.ToDouble(textBox2.Text);
            if (checkBox1.Checked)
                m_isautodepath = true;
            else
                m_isautodepath = false;
            m_rad = Convert.ToDouble(textBox3.Text)/2.0;
            this.DialogResult = DialogResult.OK;
        }

        private void Form17_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
