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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length <= 0 || textBox2.Text.Trim().Length <= 0)
            {
                if (textBox1.Text.Trim().Length <= 0)
                    textBox1.Focus();
                else if (textBox2.Text.Trim().Length <= 0)
                    textBox2.Focus();
                MessageBox.Show("客户名与工程名不能为空");
                return;
            }
            SaveXMLDB();
            //创建工程总目录
            string orgpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\EWDProject";
            if (!System.IO.Directory.Exists(orgpath))
            {
                System.IO.Directory.CreateDirectory(orgpath);
            }
            //创建工程总目录下的年份目录
            string yearpath = orgpath+"\\"+ DateTime.Now.ToString("yyyy-MM-dd").Substring(0,2)+label2.Text.Trim().Substring(0,2);
            if (!System.IO.Directory.Exists(yearpath))
            {
                System.IO.Directory.CreateDirectory(yearpath);
            }
            //创建年份目录下项目目录
            string prjpath = yearpath+"\\"+label2.Text.Trim();
            if (!System.IO.Directory.Exists(prjpath))
            {
                System.IO.Directory.CreateDirectory(prjpath);
            }
            this.DialogResult = DialogResult.OK;
        }
        private string GetOrderID()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            label2.Text = GetOrderID();//ID号
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            comboBox2.Text = comboBox2.Items[0].ToString();//客户类型
            comboBox1.Text = comboBox1.Items[0].ToString();//订单类型
            dateTimePicker2.Value = dateTimePicker2.Value.AddDays(14);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void SaveXMLDB()
        {
            //保存的文件名,目录可以根据需要修改
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allorders.xml";

            NetXmlEnt.CodeEnter.getInst().DbFile = filename;
            NetXmlEnt.CodeEntity entity = new NetXmlEnt.CodeEntity();
            entity.Id = label2.Text;//ID号
            entity.ClientClass = comboBox2.Text;//用户类型
            entity.Date = dateTimePicker1.Text;//创建日期
            entity.OrderClass = comboBox1.Text;//订单类型
            entity.Client = textBox1.Text;//用户名
            entity.Project = textBox2.Text;//工程名称
            entity.Contacts = textBox3.Text;//联系人
            entity.Phone = textBox4.Text;//联系电话
            entity.Address = textBox5.Text;//地址
            entity.DDate = dateTimePicker2.Text;
            entity.Memo = textBox6.Text;
            NetXmlEnt.CodeEnter.getInst().Insert(entity);
        }
    }
}
