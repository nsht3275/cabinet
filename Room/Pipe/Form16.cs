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
    public partial class Form16 : Form
    {
        List<zCaiLiao.CaiLiaoEntity> g_allitems = null;//2019-12-24
        public Form16()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //保存的文件名,目录可以根据需要修改
            dataGridView1.EndEdit();
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allwjs.xml";
            zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
            zCaiLiao.CaiLiaoEnter.getInst().DeleteAllBeforeWrite();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                zCaiLiao.CaiLiaoEntity entity = new zCaiLiao.CaiLiaoEntity();
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                    entity.Id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                else
                    entity.Id = "";
                if (dataGridView1.Rows[i].Cells[1].Value != null)
                    entity.Name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                else
                    entity.Name = "";
                if (dataGridView1.Rows[i].Cells[2].Value != null)
                    entity.Color = dataGridView1.Rows[i].Cells[2].Value.ToString();
                else
                    entity.Color = "";

                if (dataGridView1.Rows[i].Cells[3].Value != null)
                    entity.Unit = dataGridView1.Rows[i].Cells[3].Value.ToString();
                else
                    entity.Unit = "";

                if (dataGridView1.Rows[i].Cells[4].Value != null)
                    entity.Memo = dataGridView1.Rows[i].Cells[4].Value.ToString();
                else
                    entity.Memo = "";

                zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
            }
            zCaiLiao.CaiLiaoEnter.getInst().WriteDb();
            //
            this.DialogResult = DialogResult.OK;
        }
        private string GetOrderID()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string txt = (string)dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value;
                if (txt != null)
                {
                    if (txt.Length > 0)
                    {
                        dataGridView1.Select();
                        dataGridView1.EndEdit();
                        string str1 = "";
                        string str2 = "";
                        string str3 = "";
                        int inx = -1;
                        int col = -1;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            str1 = (string)dataGridView1.Rows[i].Cells[1].Value;
                            str2 = (string)dataGridView1.Rows[i].Cells[2].Value;
                            str3 = (string)dataGridView1.Rows[i].Cells[3].Value;
                            if (str1 != null)
                                str1 = str1.Trim();
                            else
                                str1 = "";
                            if (str2 != null)
                                str2 = str2.Trim();
                            else
                                str2 = "";
                            if (str3 != null)
                                str3 = str3.Trim();
                            else
                                str3 = "";
                            if (str1.Length <= 0)
                            {
                                inx = i;
                                col = 1;
                                break;
                            }
                            else if (str2.Length <= 0)
                            {
                                inx = i;
                                col = 2;
                                break;
                            }
                            else if (str3.Length <= 0)
                            {
                                inx = i;
                                col = 3;
                                break;
                            }
                        }
                        if (inx >= 0)
                        {
                            DataGridViewCell cell = null;
                            switch (col)
                            {
                                case 1:
                                    cell = dataGridView1.Rows[inx].Cells["Column2"];
                                    break;
                                case 2:
                                    cell = dataGridView1.Rows[inx].Cells["Column3"];
                                    break;
                                case 3:
                                    cell = dataGridView1.Rows[inx].Cells["Column4"];
                                    break;
                            }
                            if (cell != null)
                            {
                                dataGridView1.CurrentCell = cell;
                                dataGridView1.BeginEdit(true);
                            }
                            MessageBox.Show("内部不能为空");
                        }
                        else
                        {
                            dataGridView1.Rows.Add(new object[] { GetOrderID(), "", "", "",""});
                            DataGridViewCell cell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Column2"];
                            dataGridView1.CurrentCell = cell;
                            dataGridView1.BeginEdit(true);
                        }
                    }
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = GetOrderID();
                    DataGridViewCell cell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Column2"];
                    dataGridView1.CurrentCell = cell;
                    dataGridView1.BeginEdit(true);
                }
            }
            else
            {
                //dataGridView1.Rows.Add();
                //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = GetOrderID();
                dataGridView1.Rows.Add(new object[] { GetOrderID(), "", "", "", ""});
                DataGridViewCell cell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Column2"];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);

            }

        }

        private void Form16_Load(object sender, EventArgs e)
        {
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allwjs.xml";
            if (System.IO.File.Exists(filename))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                for (int i = 0; i < g_allitems.Count; i++)
                {
                    dataGridView1.Rows.Add(new object[] { g_allitems[i].Id, g_allitems[i].Name, g_allitems[i].Color, g_allitems[i].Unit, g_allitems[i].Memo });
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //保存的文件名,目录可以根据需要修改
            dataGridView1.EndEdit();
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allwjs.xml";
            zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
            zCaiLiao.CaiLiaoEnter.getInst().DeleteAllBeforeWrite();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                zCaiLiao.CaiLiaoEntity entity = new zCaiLiao.CaiLiaoEntity();
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                    entity.Id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                else
                    entity.Id = "";
                if (dataGridView1.Rows[i].Cells[1].Value != null)
                    entity.Name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                else
                    entity.Name = "";
                if (dataGridView1.Rows[i].Cells[2].Value != null)
                    entity.Color = dataGridView1.Rows[i].Cells[2].Value.ToString();
                else
                    entity.Color = "";

                if (dataGridView1.Rows[i].Cells[3].Value != null)
                    entity.Unit = dataGridView1.Rows[i].Cells[3].Value.ToString();
                else
                    entity.Unit = "";

                if (dataGridView1.Rows[i].Cells[4].Value != null)
                    entity.Memo = dataGridView1.Rows[i].Cells[4].Value.ToString();
                else
                    entity.Memo = "";

                zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
            }
            zCaiLiao.CaiLiaoEnter.getInst().WriteDb();

        }

        private void Form16_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool isempty = false;
            if (dataGridView1.Rows.Count > 0)
            {
                string txt = (string)dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value;
                if (txt != null)
                {
                    if (txt.Length > 0)
                    {
                        dataGridView1.Select();
                        dataGridView1.EndEdit();
                        string str1 = "";
                        string str2 = "";
                        string str3 = "";
                        int inx = -1;
                        int col = -1;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            str1 = (string)dataGridView1.Rows[i].Cells[1].Value;
                            str2 = (string)dataGridView1.Rows[i].Cells[2].Value;
                            str3 = (string)dataGridView1.Rows[i].Cells[3].Value;
                            if (str1 != null)
                                str1 = str1.Trim();
                            else
                                str1 = "";
                            if (str2 != null)
                                str2 = str2.Trim();
                            else
                                str2 = "";
                            if (str3 != null)
                                str3 = str3.Trim();
                            else
                                str3 = "";
                            if (str1.Length <= 0)
                            {
                                inx = i;
                                col = 1;
                                break;
                            }
                            else if (str2.Length <= 0)
                            {
                                inx = i;
                                col = 2;
                                break;
                            }
                            else if (str3.Length <= 0)
                            {
                                inx = i;
                                col = 3;
                                break;
                            }
                        }
                        if (inx >= 0)
                        {
                            DataGridViewCell cell = null;
                            switch (col)
                            {
                                case 1:
                                    cell = dataGridView1.Rows[inx].Cells["Column2"];
                                    break;
                                case 2:
                                    cell = dataGridView1.Rows[inx].Cells["Column3"];
                                    break;
                                case 3:
                                    cell = dataGridView1.Rows[inx].Cells["Column4"];
                                    break;
                            }
                            if (cell != null)
                            {
                                dataGridView1.CurrentCell = cell;
                                dataGridView1.BeginEdit(true);
                            }
                            isempty = true;
                        }
                    }
                }
            }
            if (isempty)
            {
                MessageBox.Show("数据内容不能为空,请填写数据或删除空行数据.");
                e.Cancel = true;
            }
            else
            {
                //保存的文件名,目录可以根据需要修改
                dataGridView1.EndEdit();
                string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allwjs.xml";
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                zCaiLiao.CaiLiaoEnter.getInst().DeleteAllBeforeWrite();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    zCaiLiao.CaiLiaoEntity entity = new zCaiLiao.CaiLiaoEntity();
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                        entity.Id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    else
                        entity.Id = "";
                    if (dataGridView1.Rows[i].Cells[1].Value != null)
                        entity.Name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    else
                        entity.Name = "";
                    if (dataGridView1.Rows[i].Cells[2].Value != null)
                        entity.Color = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    else
                        entity.Color = "";

                    if (dataGridView1.Rows[i].Cells[3].Value != null)
                        entity.Unit = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    else
                        entity.Unit = "";

                    if (dataGridView1.Rows[i].Cells[4].Value != null)
                        entity.Memo = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    else
                        entity.Memo = "";

                    zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
                }
                zCaiLiao.CaiLiaoEnter.getInst().WriteDb();
            }
        }
    }
}
