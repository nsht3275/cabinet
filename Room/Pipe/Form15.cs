using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Room
{
    public partial class Form15 : Form
    {
        public List<zCaiLiao.CaiLiaoEntity> g_allitems = null;
        public List<zCaiLiao.CaiLiaoEntity> g_allitemsbak = null;
        public List<zCaiLiao.CaiLiaoEntity> g_prjallitems = null;
        public ArrayList m_tplanks = new ArrayList();
        public string m_orderid = "";
        public string m_orderdate = "";
        public Form15()
        {
            InitializeComponent();
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            //读入全局物料
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
            if (System.IO.File.Exists(filename))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                for (int i = 0; i < g_allitems.Count; i++)
                {
                    dataGridView1.Rows.Add(new object[] { false, g_allitems[i].Name, g_allitems[i].Color, g_allitems[i].Width, g_allitems[i].Height, g_allitems[i].Thickness, g_allitems[i].Unit });
                }
            }
            else
            {
                MessageBox.Show("没有所需的物料,请添加物料后重试!");
                this.Close();
            }
            g_allitemsbak = new List<zCaiLiao.CaiLiaoEntity>(g_allitems.ToArray());
            //读入项目的物料
            string filename1 = ConvertPrjIDToEWDPath("prjmats.xml");
            if (!System.IO.File.Exists(filename1))
            {
                //如果文件不存在,则需要创建一个默认的对象
            }
            if (System.IO.File.Exists(filename1))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename1;
                g_prjallitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                //
                for (int i = 0; i < g_prjallitems.Count; i++)
                {
                    if (g_prjallitems[i].IsCheck)
                    {
                        for (int j = 0; j < g_allitemsbak.Count; j++)
                        {
                            if (g_prjallitems[i].Id == g_allitemsbak[j].Id)
                            {
                                dataGridView1.Rows[j].Cells[0].Value = true;
                                break;
                            }
                        }
                    }
                }
                dataGridView1.RefreshEdit();
            }
        }
        private string ConvertPrjIDToEWDPath(string inname = "", bool isnew = true)
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
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + m_orderid + "\\" + m_orderid + ".ewd";
                    else
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + m_orderid + "\\" + m_orderid + ".ewd";
                }
            }
            return filename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            string filename = ConvertPrjIDToEWDPath("prjmats.xml",false);
            zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
            zCaiLiao.CaiLiaoEnter.getInst().DeleteAllBeforeWrite();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                zCaiLiao.CaiLiaoEntity entity = new zCaiLiao.CaiLiaoEntity();

                if (dataGridView1.Rows[i].Cells[0].Value != null)
                    entity.IsCheck = (bool)dataGridView1.Rows[i].Cells[0].Value;
                else
                    entity.IsCheck = false;
                //2019-12-18 记录ID号
                if(g_allitems != null)
                    entity.Id = g_allitemsbak[i].Id;
                //
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
                zCaiLiao.CaiLiaoEnter.getInst().InsertBeforeWrite(entity);
            }
            zCaiLiao.CaiLiaoEnter.getInst().WriteDb();
            this.DialogResult = DialogResult.OK;
        }
    }
}
