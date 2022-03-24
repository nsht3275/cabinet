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
    public partial class Form14 : Form
    {
        List<zCaiLiao.CaiLiaoEntity> g_allitems = null;
        public Form14()
        {
            InitializeComponent();
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
                            dataGridView1.Rows.Add(new object[] { GetOrderID(), "", "", "1220", "2440","18","","" });
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
                dataGridView1.Rows.Add(new object[] { GetOrderID(), "", "", "1220", "2440", "18", "", "" });
                DataGridViewCell cell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Column2"];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);

            }
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
            zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
            g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
            for (int i = 0; i < g_allitems.Count; i++)
            {
                dataGridView1.Rows.Add(new object[] {g_allitems[i].Id, g_allitems[i].Name, g_allitems[i].Color, g_allitems[i].Width, g_allitems[i].Height, g_allitems[i].Thickness, g_allitems[i].Unit, g_allitems[i].Memo });
            }
        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string txt = (string)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    if (txt == null)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = GetOrderID();
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //
            if (dataGridView1.CurrentRow != null)
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //保存的文件名,目录可以根据需要修改
            dataGridView1.EndEdit();
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
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
                    entity.Width = dataGridView1.Rows[i].Cells[3].Value.ToString();
                else
                    entity.Width = "";

                if (dataGridView1.Rows[i].Cells[4].Value != null)
                    entity.Height = dataGridView1.Rows[i].Cells[4].Value.ToString();
                else
                    entity.Height = "";

                if (dataGridView1.Rows[i].Cells[5].Value != null)
                    entity.Thickness = dataGridView1.Rows[i].Cells[5].Value.ToString();
                else
                    entity.Thickness = "";

                if (dataGridView1.Rows[i].Cells[6].Value != null)
                    entity.Unit = dataGridView1.Rows[i].Cells[6].Value.ToString();
                else
                    entity.Unit = "";

                if (dataGridView1.Rows[i].Cells[7].Value != null)
                    entity.Memo = dataGridView1.Rows[i].Cells[7].Value.ToString();
                else
                    entity.Memo = "";
                zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
            }
            zCaiLiao.CaiLiaoEnter.getInst().WriteDb();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e != null && e.KeyChar != 0)
            {
                char key = e.KeyChar;
                int value = (int)key;
                if ((value >= 48 && value <= 57) || value == 46 || value == 8 || value == 43 || value == 45)
                    e.Handled = false;
                else
                    e.Handled = true;
            }else
                e.Handled = true;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dataGridView1.CurrentCell.ColumnIndex == 3 ||
                this.dataGridView1.CurrentCell.ColumnIndex == 4 ||
                this.dataGridView1.CurrentCell.ColumnIndex == 5 ||
                this.dataGridView1.CurrentCell.ColumnIndex == 6
                )
            {
                e.Control.KeyPress -= new KeyPressEventHandler(TextBox_KeyPress);
                e.Control.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
            }
            else e.Control.KeyPress -= new KeyPressEventHandler(TextBox_KeyPress);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string str = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                    int inx = str.IndexOf("-");
                    if (inx>0)
                    {
                        MessageBox.Show("无效的输入值");
                        if (e.ColumnIndex == 3)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1220";
                        if (e.ColumnIndex == 4)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "2440";
                        if (e.ColumnIndex == 5)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "18";
                        if (e.ColumnIndex == 6)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                        return;
                    }
                    double val = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (val < 1.0 && (e.ColumnIndex == 3 || e.ColumnIndex == 4))
                    {
                        MessageBox.Show("输入的值过小,将恢复默认值");
                        if (e.ColumnIndex == 3)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1220";
                        if (e.ColumnIndex == 4)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "2440";

                    }
                    else if (e.ColumnIndex == 5 && val <= 1)
                    {
                        MessageBox.Show("输入的值过小,将恢复默认值");
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "18";
                    }
                    else if (e.ColumnIndex == 6 && val < 0)
                    {
                        MessageBox.Show("输入的值过小,将恢复默认值");
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                    }
                }
            }
            else if (e.ColumnIndex == 1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length <= 0)
                    {
                        MessageBox.Show("名称不能为空,恢复默认值");
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "多层板";
                    }
                }
                else MessageBox.Show("名称不能为空,恢复默认值");

            }
            else if (e.ColumnIndex == 2)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length <= 0)
                    {
                        MessageBox.Show("颜色不能为空,恢复默认值");
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "白色";
                    }
                }
                else MessageBox.Show("名称不能为空,恢复默认值"); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Form14_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridView1.EndEdit();
            //保存的文件名,目录可以根据需要修改
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
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
                    entity.Width = dataGridView1.Rows[i].Cells[3].Value.ToString();
                else
                    entity.Width = "";

                if (dataGridView1.Rows[i].Cells[4].Value != null)
                    entity.Height = dataGridView1.Rows[i].Cells[4].Value.ToString();
                else
                    entity.Height = "";

                if (dataGridView1.Rows[i].Cells[5].Value != null)
                    entity.Thickness = dataGridView1.Rows[i].Cells[5].Value.ToString();
                else
                    entity.Thickness = "";

                if (dataGridView1.Rows[i].Cells[6].Value != null)
                    entity.Unit = dataGridView1.Rows[i].Cells[6].Value.ToString();
                else
                    entity.Unit = "";

                if (dataGridView1.Rows[i].Cells[7].Value != null)
                    entity.Memo = dataGridView1.Rows[i].Cells[7].Value.ToString();
                else
                    entity.Memo = "";
                zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
            }
            zCaiLiao.CaiLiaoEnter.getInst().WriteDb();

        }
    }
}
