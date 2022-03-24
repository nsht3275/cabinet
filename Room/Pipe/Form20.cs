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
    public partial class Form20 : Form
    {
        List<zDaoJu.DaoJuEntity> allitems = null;
        public Form20()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "增加")
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                button3.Text = "保存";
                comboBox1.Focus();
            }
            else if (button3.Text == "保存")
            {
                int num = zPubFun.zPubFunLib.CStr2Int(comboBox1.Text);
                if (num <= 0)
                {
                    MessageBox.Show("刀的编号值不可接受");
                    return;
                }
                double dia = zPubFun.zPubFunLib.CStr2Double(comboBox2.Text);
                if (dia < 0.001)
                {
                    MessageBox.Show("刀的直经不可接受");
                    return;
                }
                string beginstr, endstr;
                if (!checkBox1.Checked)
                {
                    int S = zPubFun.zPubFunLib.CStr2Int(textBox3.Text);
                    if(S<=0)
                    {
                        MessageBox.Show("刀的直经不可接受");
                        return;
                    }
                    beginstr = "T" + num.ToString() + "\r\nM03" + " S" + textBox3.Text + "\r\nG43" + " H" + num.ToString();
                    endstr = "M05";
                }
                else
                {
                    beginstr = textBox1.Text;
                    if (beginstr.Length <= 0)
                    {
                        beginstr = "T" + num.ToString() + "\r\nM03" + " S" + textBox3.Text + "\r\nG43" + " H" + num.ToString();
                    }
                    endstr = textBox2.Text;
                    if (endstr.Length <= 0)
                        endstr = "M05";
                }
                //增加保存记录
                string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allT.xml";
                zDaoJu.DaoJuEnter.getInst().DbFile = filename;
                zDaoJu.DaoJuEntity adaoju = new zDaoJu.DaoJuEntity();
                adaoju.Id = "T" + num.ToString();
                adaoju.Dia = dia.ToString("0.00");
                adaoju.Begin = beginstr;
                adaoju.End = endstr;
                adaoju.S = textBox3.Text;
                adaoju.F = textBox4.Text;
                adaoju.F1 = textBox6.Text;
                adaoju.F2 = textBox6.Text;
                adaoju.Up = textBox5.Text;
                adaoju.State = "0";
                if (!checkBox1.Checked)
                    adaoju.isCustom = "0";
                else
                    adaoju.isCustom = "1";
                if (allitems != null)
                {
                    for (int i = 0; i < allitems.Count; i++)
                    {
                        if (allitems[i].Id == adaoju.Id)
                        {
                            MessageBox.Show("已经有重合的刀具编号");
                            return;

                        }
                    }
                }
                //
                zDaoJu.DaoJuEnter.getInst().Insert(adaoju);
                //
                button3.Text = "增加";
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;//2020-04-30
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                //刷新现有刀具列表
                dataGridView1.Rows.Clear();
                if (allitems != null)
                {
                    int icount = allitems.Count;
                    allitems = zDaoJu.DaoJuEnter.getInst().SelectAll();


                    for (int i = 0; i < allitems.Count; i++)
                    {
                        int inx = dataGridView1.Rows.Add();
                        dataGridView1.Rows[inx].Cells[0].Value = allitems[i].Id;
                        dataGridView1.Rows[inx].Cells[1].Value = allitems[i].Dia;
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void Form20_Load(object sender, EventArgs e)
        {
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allT.xml";
            if (System.IO.File.Exists(filename))
            {
                zDaoJu.DaoJuEnter.getInst().DbFile = filename;
                allitems = zDaoJu.DaoJuEnter.getInst().SelectAll();
                for (int i = 0; i < allitems.Count; i++)
                {
                    int inx = dataGridView1.Rows.Add();
                    dataGridView1.Rows[inx].Cells[0].Value = allitems[i].Id;
                    dataGridView1.Rows[inx].Cells[1].Value = allitems[i].Dia;
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (dataGridView1.CurrentRow.Index >= 0)
                {
                    string idstr = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    zDaoJu.DaoJuEnter.getInst().Delete(idstr);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (dataGridView1.CurrentRow.Index >= 0)
                {
                    if (button5.Text == "修改")
                    {
                        button5.Text = "保存";
                        comboBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Substring(1);
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox4.Enabled = true;
                        comboBox1.Focus();
                    }
                    else if (button5.Text == "保存")
                    {
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = false;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        button5.Text = "修改";
                        if (allitems != null)
                        {
                            for (int i = 0; i < allitems.Count; i++)
                            {
                                if (allitems[i].Id == dataGridView1.CurrentRow.Cells[0].Value)
                                {
                                    int num = zPubFun.zPubFunLib.CStr2Int(comboBox1.Text);
                                    if (num <= 0)
                                    {
                                        MessageBox.Show("刀的编号值不可接受");
                                        return;
                                    }
                                    double dia = zPubFun.zPubFunLib.CStr2Double(comboBox2.Text);
                                    if (dia < 0.001)
                                    {
                                        MessageBox.Show("刀的直经不可接受");
                                        return;
                                    }
                                    string beginstr, endstr;
                                    if (!checkBox1.Checked)
                                    {
                                        beginstr = "T" + num.ToString() + "\r\nM03" + " S" + zPubFun.zPubFunLib.CStr2Int(textBox3.Text) + "\r\nG43" + " H" + num.ToString();
                                        endstr = "M05";
                                        allitems[i].isCustom = "0";
                                    }
                                    else
                                    {
                                        beginstr = textBox1.Text;
                                        if (beginstr.Length <= 0)
                                        {
                                            beginstr = "T" + num.ToString() + "\r\nM03" + " S" + zPubFun.zPubFunLib.CStr2Int(textBox3.Text) + "\r\nG43" + " H" + num.ToString();
                                        }
                                        endstr = textBox2.Text;
                                        if (endstr.Length <= 0)
                                            endstr = "M05";
                                        allitems[i].isCustom = "1";
                                    }
                                    zDaoJu.DaoJuEntity adouju = new zDaoJu.DaoJuEntity();
                                    adouju.Id = "T" + num.ToString();
                                    adouju.Dia = dia.ToString("0.00");
                                    adouju.Begin = beginstr;
                                    adouju.End = endstr;
                                    adouju.S = textBox3.Text;
                                    adouju.F = textBox4.Text;
                                    adouju.State = "0";
                                    zDaoJu.DaoJuEnter.getInst().Update(adouju, i);//2020-04-28
                                    dataGridView1.Rows.Clear();
                                    allitems = zDaoJu.DaoJuEnter.getInst().SelectAll();
                                    for (int j = 0; j < allitems.Count; j++)
                                    {
                                        int inx = dataGridView1.Rows.Add();
                                        dataGridView1.Rows[j].Cells[0].Value = allitems[j].Id;
                                        dataGridView1.Rows[j].Cells[1].Value = allitems[j].Dia;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Substring(1);
                comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (allitems.Count > e.RowIndex)
                {
                    textBox3.Text = allitems[e.RowIndex].S;
                    textBox4.Text = allitems[e.RowIndex].F;
                    textBox5.Text = allitems[e.RowIndex].Up;//2020-04-30
                    if (allitems[e.RowIndex].isCustom == "1")
                    {
                        checkBox1.Checked = true;
                        textBox1.Enabled = true;
                        textBox2.Enabled = true;
                        textBox1.Text = allitems[e.RowIndex].Begin;
                        textBox2.Text = allitems[e.RowIndex].End;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                }
            }

        }
    }
}
