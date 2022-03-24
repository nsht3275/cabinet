using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Room
{
    public partial class Form9 : Form
    {
        List<NetXmlEnt.CodeEntity> g_allitems = null;
        public string m_orderid = "";
        public string m_orderdate = "";
        public static void DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            if(Directory.Exists(f))//2021-03-25
                                DeleteDir(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(file);
                }

            }
            catch (Exception ex) // 异常处理
            {
                MessageBox.Show(ex.Message.ToString());// 异常信息
            }
        }
        public Form9()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NetXmlEnt.CodeEntity entity = new NetXmlEnt.CodeEntity();
            entity.Id = label2.Text;//ID号
            entity.ClientClass = comboBox2.Text;//用户类型
            entity.Date = dateTimePicker1.Text;//创建日期
            entity.OrderClass = comboBox3.Text;//订单类型
            entity.Client = textBox7.Text;//用户名
            entity.Project = textBox2.Text;//工程名称
            entity.Contacts = textBox3.Text;//联系人
            entity.Phone = textBox4.Text;//联系电话
            entity.Address = textBox5.Text;//地址
            entity.DDate = dateTimePicker2.Text;
            entity.Memo = textBox6.Text;
            if (dataGridView1.Rows.Count <= 0)
                return;
            //
            int index = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1[0, i].Value.ToString() == entity.Id)
                {
                    dataGridView1[1, i].Value = entity.Date;
                    dataGridView1[2, i].Value = entity.Project;
                    dataGridView1[3, i].Value = entity.Contacts;
                    dataGridView1[4, i].Value = entity.Phone;
                    index = i;
                    break;
                }
            }
            //
            NetXmlEnt.CodeEnter.getInst().Update(entity,index);//2020-04-28
            for(int i = 0; i < g_allitems.Count;i++)
            {
                if(g_allitems[i].Id == entity.Id)
                {
                    g_allitems[i].ClientClass = comboBox2.Text;//用户类型
                    g_allitems[i].Date = dateTimePicker1.Text;//创建日期
                    g_allitems[i].OrderClass = comboBox3.Text;//订单类型
                    g_allitems[i].Client = textBox7.Text;//用户名
                    g_allitems[i].Project = textBox2.Text;//工程名称
                    g_allitems[i].Contacts = textBox3.Text;//联系人
                    g_allitems[i].Phone = textBox4.Text;//联系电话
                    g_allitems[i].Address = textBox5.Text;//地址
                    g_allitems[i].DDate = dateTimePicker2.Text;
                    g_allitems[i].Memo = textBox6.Text;
                    break;
                }
            }

            MessageBox.Show("已保存");
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            //保存的文件名,目录可以根据需要修改
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            dateTimePicker4.CustomFormat = "yyyy-MM-dd";
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allorders.xml";
            NetXmlEnt.CodeEnter.getInst().DbFile = filename;
            g_allitems = NetXmlEnt.CodeEnter.getInst().SelectAll();
            
            int tmpjs = 0;
            dataGridView1.Rows.Clear();
            for (int i = g_allitems.Count()-1;i>=0;i--)
            {
                if(tmpjs>=0 && tmpjs<=9)
                {
                    dataGridView1.Rows.Add();
                    int count = dataGridView1.Rows.Count;
                    dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                    dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                    dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                    dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                    dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                }
                if (tmpjs == 0)
                {
                    label2.Text = g_allitems[i].Id;//ID号
                    comboBox2.Text = g_allitems[i].ClientClass;//用户类型
                    dateTimePicker1.Text = g_allitems[i].Date;//创建日期
                    comboBox3.Text = g_allitems[i].OrderClass;////订单类型
                    textBox7.Text  = g_allitems[i].Client ;//用户名
                    textBox2.Text = g_allitems[i].Project;//工程名称
                    textBox3.Text = g_allitems[i].Contacts;//联系人
                    textBox4.Text = g_allitems[i].Phone;//联系电话
                    textBox5.Text = g_allitems[i].Address;//地址
                    dateTimePicker2.Text = g_allitems[i].DDate;
                    textBox6.Text = g_allitems[i].Memo;
                }
                else if (tmpjs >= 10)//只取最近的10个
                    break;
                tmpjs++;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 1)
            {
                textBox1.Visible = false;
                label14.Visible = true;
                dateTimePicker3.Visible = true;
                label15.Visible = true;
                dateTimePicker4.Visible = true;
            }
            else
            {
                textBox1.Visible = true;
                label14.Visible = false;
                dateTimePicker3.Visible = false;
                label15.Visible = false;
                dateTimePicker4.Visible = false;
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Value = dateTimePicker3.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int inx = -1;
            for (int i = g_allitems.Count() - 1; i >= 0; i--)
            {
                inx = dataGridView1.CurrentRow.Index;
                if(dataGridView1[0, inx].Value != null)//避免未初始化完毕,为空时的数据
                {
                    if (g_allitems[i].Id == dataGridView1[0, inx].Value.ToString())
                    {
                        label2.Text = g_allitems[i].Id;//ID号
                        comboBox2.Text = g_allitems[i].ClientClass;//用户类型
                        dateTimePicker1.Text = g_allitems[i].Date;//创建日期
                        comboBox3.Text = g_allitems[i].OrderClass;////订单类型
                        textBox7.Text = g_allitems[i].Client;//用户名
                        textBox2.Text = g_allitems[i].Project;//工程名称
                        textBox3.Text = g_allitems[i].Contacts;//联系人
                        textBox4.Text = g_allitems[i].Phone;//联系电话
                        textBox5.Text = g_allitems[i].Address;//地址
                        dateTimePicker2.Text = g_allitems[i].DDate;
                        textBox6.Text = g_allitems[i].Memo;
                    }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        dataGridView1.Rows.Clear();
                        int inx = -1;
                        for (int i = g_allitems.Count() - 1; i >= 0; i--)
                        {
                            if (g_allitems[i].Id == textBox1.Text.Trim())
                            {
                                dataGridView1.Rows.Add();
                                int count = dataGridView1.Rows.Count;
                                dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                                dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                                dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                                dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                                dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                                dataGridView1.Rows[count - 1].Selected = true;
                                //
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        dataGridView1.Rows.Clear();
                        int inx = -1;
                        DateTime curtime;
                        DateTime stime;
                        DateTime etime;
                        for (int i = g_allitems.Count() - 1; i >= 0; i--)
                        {
                            curtime = Convert.ToDateTime(g_allitems[i].Date);
                            stime = dateTimePicker3.Value.Date;
                            etime = dateTimePicker4.Value.Date;
                            if ((curtime>= stime && curtime<etime) || stime == curtime || etime == curtime)
                            {
                                dataGridView1.Rows.Add();
                                int count = dataGridView1.Rows.Count;
                                dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                                dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                                dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                                dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                                dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                                dataGridView1.Rows[count - 1].Selected = true;
                                //
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        dataGridView1.Rows.Clear();
                        int inx = -1;
                        for (int i = g_allitems.Count() - 1; i >= 0; i--)
                        {
                            if (g_allitems[i].Project == textBox1.Text.Trim())
                            {
                                dataGridView1.Rows.Add();
                                int count = dataGridView1.Rows.Count;
                                dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                                dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                                dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                                dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                                dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                                dataGridView1.Rows[count - 1].Selected = true;
                                //
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        dataGridView1.Rows.Clear();
                        int inx = -1;
                        for (int i = g_allitems.Count() - 1; i >= 0; i--)
                        {
                            if (g_allitems[i].Contacts == textBox1.Text.Trim())
                            {
                                dataGridView1.Rows.Add();
                                int count = dataGridView1.Rows.Count;
                                dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                                dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                                dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                                dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                                dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                                dataGridView1.Rows[count - 1].Selected = true;
                                //
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        dataGridView1.Rows.Clear();
                        int inx = -1;
                        for (int i = g_allitems.Count() - 1; i >= 0; i--)
                        {
                            if (g_allitems[i].Phone == textBox1.Text.Trim())
                            {
                                dataGridView1.Rows.Add();
                                int count = dataGridView1.Rows.Count;
                                dataGridView1[0, count - 1].Value = g_allitems[i].Id;
                                dataGridView1[1, count - 1].Value = g_allitems[i].Date;
                                dataGridView1[2, count - 1].Value = g_allitems[i].Project;
                                dataGridView1[3, count - 1].Value = g_allitems[i].Contacts;
                                dataGridView1[4, count - 1].Value = g_allitems[i].Phone;
                                dataGridView1.Rows[count - 1].Selected = true;
                                //
                            }
                        }
                    }
                    break;
            }
        }

        private string ConvertPrjIDToPath(string inname = "", bool isnew = true)
        {
            string filename = "";
            if (m_orderid.Length > 0)
            {

                string year = DateTime.Now.ToString("yyyy");//g_orderid.Substring(0, 2);
                string oyear = Convert.ToDateTime(m_orderdate).Year.ToString();
                if (inname.Length > 0)
                {
                    if (isnew)
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + m_orderid + "\\" + inname;
                    else
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + m_orderid + "\\" + inname;
                }
                else
                {
                    if (isnew)
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + m_orderid;
                    else
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + m_orderid;
                }
            }
            return filename;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == label2.Text)
                {
                    m_orderid = label2.Text;
                    m_orderdate = dateTimePicker1.Text;
                    string path = ConvertPrjIDToPath("", false);
                    NetXmlEnt.CodeEnter.getInst().Delete(label2.Text);
                    dataGridView1.Rows.RemoveAt(i);
                    string mydoc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    int inx = path.IndexOf(mydoc);
                    if (inx >= 0 && path.Length > mydoc.Length)
                    {
                        DeleteDir(path);
                    }
                    MessageBox.Show("删除成功");
                    break;
                    //
                }
            }

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
