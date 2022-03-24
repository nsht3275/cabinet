using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
namespace Room
{
    public partial class Form5 : Form
    {
        public class CWallSeg
        {
            public CWallSeg() { inx = 0; x1 = y1 = z1 = x2 = y2 = z2 = 0.0; }
            public int inx;
            public double x1;
            public double y1;
            public double z1;
            public double x2;
            public double y2;
            public double z2;
        };

        //立面窗型
        public class CWinType
        {
            public CWinType(double ix, double iy, double iz, int itype) { vx = ix; vy = iy; vz = iz; wintype = itype; }
            public CWinType() { vx = vy = vz = 0.0; wintype = 0; }
            public double vx;
            public double vy;
            public double vz;
            public int    wintype;
        };
        //点类型
        public class CPoint
        {
            public CPoint(double ix, double iy, double iz) { x = ix; y = iy; z = iz; }
            public CPoint() { x = y = z = 0.0; }
            public double x;
            public double y;
            public double z;
        };
        //已有的阳台数据
        public class CExistBalcony
        {
            public CExistBalcony() 
            {
                b_width = b_height = b_theight = b_bheight = ba_length = ba_width = ba_width = hr_bheight = hr_width = hr_thickness = mp_updist = mp_downdist = mp_thickness;
                b_btype = ba_num = 0;
                isallsame = isbarrier = ishandrai = ismidpanel = false;

            }
            public bool   isallsame;
            //基本数据
            public double b_width;
            public double b_height;
            public double b_theight;
            public double b_bheight;
            public int    b_btype;
            //栅栏数据
            public bool   isbarrier;
            public double ba_length;
            public double ba_width;
            public int    ba_num;
            //栏杆的数
            public bool   ishandrai;
            public double hr_bheight;
            public double hr_width;
            public double hr_thickness;
            //中板的数据
            public bool   ismidpanel;
            public double mp_updist;
            public double mp_downdist;
            public double mp_thickness;
        }
        public string memstr = "";
        public ArrayList wintypes;//新生成的立面数据
        public ArrayList orgwintypes;//已经存在的立面数据
        public ArrayList pipepts;
        public double g_thickness = 128;
        public double g_height = 2800;
        //区域的中心点
        public double g_cx = 0;
        public double g_cy = 0;
        public double g_cz = 0;
        public int    g_areaent = 0;
        public CExistBalcony g_balconyalldatas;
        public Form5()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                axEWdraw2.Visible = true;
                axEWdraw2.DisableRectSelect(true);
                axEWdraw2.DisableRightMenu(true);
                label1.Top = 210;
                listView1.Top = 224;
                listView1.Height = 64;
                if (pipepts != null)
                {
                    if (pipepts.Count > 0)
                    {
                        MakePipeFace(g_thickness, g_height, ref pipepts);
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                axEWdraw2.Visible = false;
                label1.Top = 83;
                listView1.Top = 102;
                listView1.Height = 185;

            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            groupBox3.Visible = false;
            Height = 450;
            button2.Top = 390;
            button3.Top = 390;
            
            axEWdraw2.Visible = false;
            axEWdraw2.SetSelectionColor(axEWdraw2.RGBToIndex(255, 0, 0));
            axEWdraw2.DisableRectSelect(true);
            //判断是否已经有数据存在
            if (g_balconyalldatas != null)
            {
                //判断各立面相同或不同
                if (g_balconyalldatas.isallsame)
                {
                    radioButton1.Checked = true;
                    if (orgwintypes.Count == 1)
                    {
                        listView1.Focus();
                        if (((CWinType)orgwintypes[0]).wintype < listView1.Items.Count)
                        {
                            listView1.Items[((CWinType)orgwintypes[0]).wintype].Selected = true;
                            listView1.EnsureVisible(((CWinType)orgwintypes[0]).wintype);
                            switch (((CWinType)orgwintypes[0]).wintype)
                            {
                                case 0:
                                    {
                                        pictureBox1.Load("img_win_1.jpg");
                                    }
                                    break;
                                case 1:
                                    {
                                        pictureBox1.Load("img_win_2.jpg");
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    pictureBox1.Load("img_win_1.jpg");
                    radioButton2.Checked = true;
                }
                //判断基础数据
                textBox1.Text = g_balconyalldatas.b_width.ToString();
                textBox2.Text = g_balconyalldatas.b_height.ToString();
                textBox3.Text = g_balconyalldatas.b_theight.ToString();
                textBox4.Text = g_balconyalldatas.b_bheight.ToString();
                //判断下部墙的类型
                if (g_balconyalldatas.b_btype == 1)
                {
                    comboBox1.SelectedIndex = 1;
                    groupBox3.Visible = true;
                    Height = 564;
                    button2.Top = 501;
                    button3.Top = 501;
                }
                else
                    comboBox1.SelectedIndex = 0;
                //判断是否有栏栅的数据
                if (g_balconyalldatas.isbarrier)
                {
                    checkBox1.Checked = true;
                    textBox5.Text = g_balconyalldatas.ba_length.ToString();
                    textBox6.Text = g_balconyalldatas.ba_width.ToString();
                    comboBox2.SelectedIndex = g_balconyalldatas.ba_num / 3 - 1;
                }
                //判断是否栏杆的数据
                if (g_balconyalldatas.ishandrai)
                {
                    checkBox2.Checked = true;
                    textBox7.Text = g_balconyalldatas.hr_bheight.ToString();
                    textBox8.Text = g_balconyalldatas.hr_width.ToString();
                    textBox9.Text = g_balconyalldatas.hr_thickness.ToString();
                }
                //中板是否有数据
                if (g_balconyalldatas.ismidpanel)
                {
                    checkBox3.Checked = true;
                    textBox10.Text = g_balconyalldatas.mp_updist.ToString();
                    textBox11.Text = g_balconyalldatas.mp_downdist.ToString();
                    textBox12.Text = g_balconyalldatas.mp_thickness.ToString();
                }
                //
            }
            else
            {
                pictureBox1.Load("img_win_1.jpg");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //形成扩展数据
            string tmpstr = "";
            //---判断是否是不同的立面窗型
            if (radioButton2.Checked)
            {//立面窗型不同
                if (orgwintypes != null)
                {
                    int wintypelen = orgwintypes.Count;
                    if (wintypelen > 1)//之所以大于1是因为等于1时是全局,就不需要在判断窗户类型了
                    {
                        for (int i = 0; i < wintypelen; i++)
                        {
                            if (i == 0)
                            {
                                tmpstr = "wintype:" + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vz) + "," + ((CWinType)orgwintypes[i]).wintype.ToString();
                            }
                            else
                            {
                                tmpstr += "," + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)orgwintypes[i]).vz) + "," + ((CWinType)orgwintypes[i]).wintype.ToString();
                            }
                        }
                        tmpstr += ";";
                    }
                    else
                    {
                        wintypelen = wintypes.Count;
                        if (wintypelen > 0)
                        {
                            for (int i = 0; i < wintypelen; i++)
                            {
                                if (i == 0)
                                {
                                    tmpstr = "wintype:" + String.Format("{0:f3}", ((CWinType)wintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vz) + "," + ((CWinType)wintypes[i]).wintype.ToString();
                                }
                                else
                                {
                                    tmpstr += "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vz) + "," + ((CWinType)wintypes[i]).wintype.ToString();
                                }
                            }
                            tmpstr += ";";
                        }
                    }
                }
                else
                {
                    int wintypelen = wintypes.Count;
                    if (wintypelen > 0)
                    {
                        for (int i = 0; i < wintypelen; i++)
                        {
                            if (i == 0)
                            {
                                tmpstr = "wintype:" + String.Format("{0:f3}", ((CWinType)wintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vz) + "," + ((CWinType)wintypes[i]).wintype.ToString();
                            }
                            else
                            {
                                tmpstr += "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vx) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vy) + "," + String.Format("{0:f3}", ((CWinType)wintypes[i]).vz) + "," + ((CWinType)wintypes[i]).wintype.ToString();
                            }
                        }
                        tmpstr += ";";
                    }
                }
            }
            else if (radioButton1.Checked)
            {
                int selinx = 0;
                if (listView1.SelectedItems.Count > 0)
                    selinx = listView1.SelectedItems[0].Index;
                tmpstr = "allwintype:" + selinx.ToString() + ";";
            }
            memstr += tmpstr;
            //阳台基础数据
            tmpstr = "area:1;"+"balcony:" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + "," + textBox4.Text + ","+comboBox1.SelectedIndex.ToString()+";";
            memstr += tmpstr;
            //判断是否是玻璃构成
            if (groupBox3.Visible)
            {
                if (checkBox1.Checked)
                {//栏栅
                    int num = 0;
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            num = 3;
                            break;
                        case 1:
                            num = 6;
                            break;
                        case 2:
                            num = 9;
                            break;

                    }
                    tmpstr = "barrier:" + textBox5.Text + "," + textBox6.Text + "," + num.ToString() + ";";
                    memstr += tmpstr;
                }
                if (checkBox2.Checked)
                {//栏杆
                    tmpstr = "handrail:" + textBox7.Text + "," + textBox8.Text + "," + textBox9.Text + ";";
                    memstr += tmpstr;
                }
                if (checkBox2.Checked)
                {//栏杆
                    tmpstr = "midpanel:" + textBox10.Text + "," + textBox11.Text + "," + textBox12.Text + ";";
                    memstr += tmpstr;
                }
            }
            //MessageBox.Show(memstr);
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (orgwintypes == null)
            {//如果没有原始的窗型,则说明是新建的阳台,需要给定原始值
                //形成扩展数据
                string tmpstr = "";
                int selinx = 0;
                if (listView1.SelectedItems.Count > 0)
                    selinx = listView1.SelectedItems[0].Index;
                tmpstr = "allwintype:" + selinx.ToString() + ";";
                memstr += tmpstr;
                //阳台基础数据
                tmpstr = "area:1;"+"balcony:" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + "," + textBox4.Text + ","+comboBox1.SelectedIndex.ToString()+";";
                memstr += tmpstr;
                this.DialogResult = DialogResult.OK;
            }else
                this.DialogResult = DialogResult.Cancel;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                groupBox3.Visible = true;
                Height = 564;
                button2.Top = 501;
                button3.Top = 501;
            }
            else
            {
                
                Height = 450;
                button2.Top = 390;
                button3.Top = 390;
                groupBox3.Visible = false;
            }
        }

        private void axEWdraw1_Enter(object sender, EventArgs e)
        {

        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            axEWdraw2.DeleteAll();
        }

        private void MakePipeFace(double thickness, double height,ref ArrayList pts)
        {
            if (thickness > 10)
            {
                //
                axEWdraw2.DeleteAll();
                int ent = MakeSection(((CWallSeg)pts[0]).x1, ((CWallSeg)pts[0]).y1, 0,
                                        ((CWallSeg)pts[0]).x2, ((CWallSeg)pts[0]).y2, 0, thickness, height);
                //绘制放样线
                axEWdraw2.ClearIDBuffer();
                int segent = 0;
                for (int i = 0; i < pts.Count; i++)
                {
                    segent = axEWdraw2.Line(new object[] { ((CWallSeg)pts[i]).x1, ((CWallSeg)pts[i]).y1, 0 }, new object[] { ((CWallSeg)pts[i]).x2, ((CWallSeg)pts[i]).y2, 0 });
                    axEWdraw2.AddIDToBuffer(segent);
                }
                int sline = axEWdraw2.Join(-1,-1);
                axEWdraw2.ClearIDBuffer();
                axEWdraw2.EnableWallPipe(true);
                axEWdraw2.SetPipeAlign(false);
                int pipewall = axEWdraw2.Pipe(sline, ent);
                if (pipewall > 0)
                {
                    ArrayList opts = new ArrayList();
                    int segsize = axEWdraw2.GetPolyLineSegmentSize(sline);
                    double x1,y1,z1,x2,y2,z2,vx,vy,vz;
                    x1 = y1 = z1 = x2 = y2 = z2 = vx = vy = vz = 0.0;
                    bool isarc = false;
                    
                    for (int i = 0; i < segsize; i++)
                    {
                        axEWdraw2.GetPolyLineSegment(sline, i, ref x1, ref y1, ref z1, ref x2, ref y2, ref z2, ref vx, ref vy, ref vz, ref isarc);
                        if(i == 0)
                        {
                            opts.Add(x1);
                            opts.Add(y1);
                            opts.Add(z1);
                            
                            opts.Add(x2);
                            opts.Add(y2);
                            opts.Add(z2);
                            
                        }
                        else
                        {
                            opts.Add(x2);
                            opts.Add(y2);
                            opts.Add(z2);
                            
                        }
                    }
                   
                    SetPtsToSolid(pipewall, ref opts);
                    SetThicknessToSolid(pipewall, thickness);//设置墙的厚度 2016-11-03
                    SetHeightToSolid(pipewall, height);//设置墙的厚度 2016-11-03
                    axEWdraw2.SplitWall(pipewall);
                    axEWdraw2.Delete(pipewall);
                    axEWdraw2.Delete(sline);
                    axEWdraw2.Delete(ent);
                }
                axEWdraw2.ZoomALL();
                //axEWdraw2.DisableRightMenu(true);
                //
                int entsize = axEWdraw2.GetEntSize();
                if (entsize > 0)
                {
                    if (wintypes == null)
                        wintypes = new ArrayList();
                    else
                        wintypes.Clear();
                    for (int i = 1; i <= entsize; i++)
                    {
                        ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (ent > 0 && enttype == 66)
                        {
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            axEWdraw2.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            CWinType awintype = new CWinType();
                            awintype.vx = (minx + maxx) / 2.0 - g_cx;
                            awintype.vy = (miny + maxy) / 2.0 - g_cy;
                            awintype.vz = 0;
                            awintype.wintype = 0;
                            wintypes.Add(awintype);
                        }
                    }
                }

            }
        }
        /*SetPtsToSolid从墙实体对象上设置点集
         * 参数:
        * id  输入  墙对象的ID
        * pts 输入  墙对象的点数组
         * 如果设置成功返回true,如果失败返回false
        */
        private bool SetPtsToSolid(long id, ref ArrayList pts)
        {
            int entsize = axEWdraw2.GetEntSize();
            //形成字符串
            string str = "pts:";//顶点段
            for (int i = 0; i < pts.Count; i++)
            {
                str += String.Format("{0:f3}", pts[i]);//保留三位小数四舍五入,可根据情况设置
                if ((i + 1) < pts.Count)
                {
                    str += ",";//分隔符
                }
                else
                    str += ";";

            }
            //查找并设置实体的用户数据为点集
            for (int i = entsize; i >= 1; i--)
            {
                int ent = axEWdraw2.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw2.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "pts:");
                        if (ffinx >= 0)//2016-11-11
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            //string substr = orgstr.Substring(ffinx + 4, feinx - (ffinx + 4));
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            //2016-11-11 如果不为空,且没找到,则直接相加
                            string substr = orgstr + str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw2.SetEntityUserData(ent, str);
                    return true;
                }
            }
            return false;
        }
        //一个字符串,是否包含字符段信息,全字匹配 2017-09-21
        private int IsHaveStrField(string orgstr, string field)
        {
            char lastfield = field[field.Length - 1];
            string tfieldname;
            if (lastfield == ':')
                tfieldname = field;
            else
                tfieldname = field + ":";
            string tfieldname1;
            if (lastfield == ':')
                tfieldname1 = ";" + field;//2017-09-21 主要是避名名称部分重合
            else
                tfieldname1 = ";" + field + ":";//2017-09-21 主要是避名名称部分重合
            int ffinx = orgstr.IndexOf(tfieldname);//段开始的位置
            int ffinx1 = orgstr.IndexOf(tfieldname1);//段开始的位置 2017-09-21
            if (ffinx == 0)
                return 0;
            else if (ffinx1 > 0)
            {
                return ffinx1 + 1;//之所以加1,是为了保持起始位置从第一个字母开始,而不是分隔符;开始
            }
            return -1;
        }

        /*设置墙面的厚度信息 2016-11-13
        * 
        */
        private bool SetThicknessToSolid(long id, double thickness)
        {
            int entsize = axEWdraw2.GetEntSize();
            string str = "thickness:" + String.Format("{0:f3}", thickness) + ";";
            //查找并设置实体的用户数据为点集
            for (int i = entsize; i >= 1; i--)
            {
                int ent = axEWdraw2.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw2.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "thickness:");
                        if (ffinx >= 0)
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            string substr = orgstr + str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw2.SetEntityUserData(ent, str);
                    return true;
                }
            }
            return false;
        }
        /*设置墙面的高度度信息 2016-11-13
         * 
         */
        private bool SetHeightToSolid(long id, double height)
        {
            int entsize = axEWdraw2.GetEntSize();
            string str = "height:" + String.Format("{0:f3}", height) + ";";
            //查找并设置实体的用户数据为点集
            for (int i = entsize; i >= 1; i--)
            {
                int ent = axEWdraw2.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw2.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "height:");
                        if (ffinx >= 0)
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            string substr = orgstr + str;
                            axEWdraw2.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw2.SetEntityUserData(ent, str);
                    return true;
                }
            }
            return false;
        }

        /*MakeSection 根据一个线段的起始点与终止点画出pipe所需截面
         * 参数:
         * x1,y1,z1   线段起点
         * x2,y2,z2   线段终点
         * thickness  墙的厚度
         * 返回值:
         * 如果成功返回ture,其它返回false
         */
        private int MakeSection(double x1, double y1, double z1, double x2, double y2, double z2, double thickness,double h)
        {
            double vx, vy, vz;
            double ox1, oy1, oz1;
            double ox2, oy2, oz2;
            double p1x, p1y, p1z;
            double p2x, p2y, p2z;
            vx = vy = vz = 0.0;
            ox1 = oy1 = oz1 = ox2 = oy2 = oz2 = 0.0;
            p1x = p1y = p1z = p2x = p2y = p2z = 0.0;
            vx = x2 - x1;
            vy = y2 - y1;
            vz = z2 - z1;
            axEWdraw2.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, 90, ref ox1, ref oy1, ref oz1);
            axEWdraw2.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, -90, ref ox2, ref oy2, ref oz2);
            axEWdraw2.Polar(new object[] { x1, y1, z1 }, new object[] { ox2, oy2, oz2 }, thickness / 2.0, ref p1x, ref p1y, ref p1z);
            axEWdraw2.Polar(new object[] { x1, y1, z1 }, new object[] { ox1, oy1, oz1 }, thickness / 2.0, ref p2x, ref p2y, ref p2z);
            axEWdraw2.Clear3DPtBuf();
            axEWdraw2.AddOne3DVertex(p1x, p1y, p1z);
            axEWdraw2.AddOne3DVertex(p2x, p2y, p2z);
            axEWdraw2.AddOne3DVertex(p2x, p2y, p2z + h);
            axEWdraw2.AddOne3DVertex(p1x, p1y, p1z + h);
            int ent = axEWdraw2.PolyLine3D(true);
            axEWdraw2.Clear3DPtBuf();
            int face = axEWdraw2.EntToFace(ent, true);
            return face;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void axEWdraw2_ViewMouseUp(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseUpEvent e)
        {
            if (e.button == 1)
            {
                if (orgwintypes != null)
                {
                    if (orgwintypes.Count > 1)//之所以大于1是因为等于1时是全局,就不需要在判断窗户类型了
                    {
                        double vx, vy, vz;
                        vx = vy = vz = 0.0;
                        int selsize = axEWdraw2.GetSelectEntSize();
                        int selinx = -1;
                        if (selsize > 0)
                        {
                            int ent = axEWdraw2.GetSelectEnt(0);
                            if (ent > 0)
                            {
                                double minx, miny, minz, maxx, maxy, maxz;
                                minx = miny = minz = maxx = maxy = maxz = 0.0;
                                axEWdraw2.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                vx = (minx + maxx) / 2.0 - g_cx;
                                vy = (miny + maxy) / 2.0 - g_cy;
                                vz = 0;
                                double ang = 0;
                                double minang = 0;
                                int mininx = -1;
                                //找到角度最接近的 2017-11-29
                                if (orgwintypes.Count > 1)
                                {
                                    for (int k = 0; k < orgwintypes.Count; k++)
                                    {
                                        //计算两矢量夹角,注意vectorangle返回的是弧角
                                        ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)orgwintypes[k]).vx, ((CWinType)orgwintypes[k]).vy, ((CWinType)orgwintypes[k]).vz });
                                        if (k == 0)
                                        {
                                            minang = ang;
                                            mininx = 0;
                                        }
                                        else if (ang < minang)
                                        {
                                            minang = ang;
                                            mininx = k;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < wintypes.Count; k++)
                                    {
                                        //计算两矢量夹角,注意vectorangle返回的是弧角
                                        ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)wintypes[k]).vx, ((CWinType)wintypes[k]).vy, ((CWinType)wintypes[k]).vz });
                                        if (k == 0)
                                        {
                                            minang = ang;
                                            mininx = 0;
                                        }
                                        else if (ang < minang)
                                        {
                                            minang = ang;
                                            mininx = k;
                                        }
                                    }
                                }
                                if (mininx >= 0)
                                {
                                    int selwintype = -1;
                                    if (orgwintypes.Count > 1)
                                        selwintype = ((CWinType)orgwintypes[mininx]).wintype;
                                    else
                                        selwintype = ((CWinType)wintypes[mininx]).wintype;
                                    if (selwintype < listView1.Items.Count)
                                    {
                                        listView1.Focus();
                                        listView1.Items[selwintype].Selected = true;
                                        listView1.EnsureVisible(selwintype);
                                    }
                                }
                            }
                        }
                    }
                    else if (wintypes.Count > 0)
                    {
                        double ang = 0;
                        double minang = 0;
                        int mininx = -1;
                        double vx, vy, vz;
                        vx = vy = vz = 0.0;
                        for (int k = 0; k < wintypes.Count; k++)
                        {
                            //计算两矢量夹角,注意vectorangle返回的是弧角
                            ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)wintypes[k]).vx, ((CWinType)wintypes[k]).vy, ((CWinType)wintypes[k]).vz });
                            if (k == 0)
                            {
                                minang = ang;
                                mininx = 0;
                            }
                            else if (ang < minang)
                            {
                                minang = ang;
                                mininx = k;
                            }
                        }
                        if (mininx >= 0)
                        {
                            int selwintype = ((CWinType)wintypes[mininx]).wintype;
                            if (selwintype < listView1.Items.Count)
                            {
                                listView1.Focus();
                                listView1.Items[selwintype].Selected = true;
                                listView1.EnsureVisible(selwintype);
                            }
                        }
                    }
                }
                else
                {
                    double ang = 0;
                    double minang = 0;
                    int mininx = -1;
                    double vx, vy, vz;
                    vx = vy = vz = 0.0;
                    for (int k = 0; k < wintypes.Count; k++)
                    {
                        //计算两矢量夹角,注意vectorangle返回的是弧角
                        ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)wintypes[k]).vx, ((CWinType)wintypes[k]).vy, ((CWinType)wintypes[k]).vz });
                        if (k == 0)
                        {
                            minang = ang;
                            mininx = 0;
                        }
                        else if (ang < minang)
                        {
                            minang = ang;
                            mininx = k;
                        }
                    }
                    if (mininx >= 0)
                    {
                        int selwintype = ((CWinType)wintypes[mininx]).wintype;
                        if (selwintype < listView1.Items.Count)
                        {
                            listView1.Focus();
                            listView1.Items[selwintype].Selected = true;
                            listView1.EnsureVisible(selwintype);
                        }
                    }
                }
            }
        }

        private void axEWdraw2_Enter(object sender, EventArgs e)
        {

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            int selsize = axEWdraw2.GetSelectEntSize();
            int selinx = -1;
            if (listView1.SelectedItems.Count > 0)
                selinx = listView1.SelectedItems[0].Index;
            if (selsize > 0)
            {
                for (int i = 0; i < selsize; i++)
                {
                    int ent = axEWdraw2.GetSelectEnt(i);
                    if (ent > 0)
                    {
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw2.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        vx = (minx + maxx) / 2.0 - g_cx;
                        vy = (miny + maxy) / 2.0 - g_cy;
                        vz = 0;
                        //
                        int mininx = -1;
                        double minang = 0.0;
                        double ang = 0.0;

                        if (orgwintypes != null)
                        {
                            int wintypelen = orgwintypes.Count;
                            if (wintypelen > 1)//之所以大于1是因为等于1时是全局,就不需要在判断窗户类型了
                            {
                                for (int j = 0; j < wintypelen; j++)
                                {
                                    ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)orgwintypes[j]).vx, ((CWinType)orgwintypes[j]).vy, ((CWinType)orgwintypes[j]).vz });
                                    if (j == 0)
                                    {
                                        //得到两个矢量的弧度值(注意这里返回的是弧度值)
                                        minang = ang;
                                        mininx = 0;
                                    }
                                    else if (ang < minang)
                                    {
                                        minang = ang;
                                        mininx = j;
                                    }
                                }
                            }
                            else
                            {
                                wintypelen = wintypes.Count;
                                for (int j = 0; j < wintypelen; j++)
                                {
                                    ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)wintypes[j]).vx, ((CWinType)wintypes[j]).vy, ((CWinType)wintypes[j]).vz });
                                    if (j == 0)
                                    {
                                        //得到两个矢量的弧度值(注意这里返回的是弧度值)
                                        minang = ang;
                                        mininx = 0;
                                    }
                                    else if (ang < minang)
                                    {
                                        minang = ang;
                                        mininx = j;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int wintypelen = wintypes.Count;
                            for (int j = 0; j < wintypelen; j++)
                            {
                                ang = axEWdraw2.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((CWinType)wintypes[j]).vx, ((CWinType)wintypes[j]).vy, ((CWinType)wintypes[j]).vz });
                                if (j == 0)
                                {
                                    //得到两个矢量的弧度值(注意这里返回的是弧度值)
                                    minang = ang;
                                    mininx = 0;
                                }
                                else if (ang < minang)
                                {
                                    minang = ang;
                                    mininx = j;
                                }
                            }
                        }
                        if (mininx >= 0)
                        {
                            if (orgwintypes != null)
                            {
                                if(orgwintypes.Count>1)
                                    ((CWinType)orgwintypes[mininx]).wintype = selinx;
                                else
                                    ((CWinType)wintypes[mininx]).wintype = selinx;
                            }
                            else
                                ((CWinType)wintypes[mininx]).wintype = selinx;
                        }
                    }
                }
            }
            switch (selinx)
            {
                case 0:
                    {
                        pictureBox1.Load("img_win_1.jpg");
                    }
                    break;
                case 1:
                    {
                        pictureBox1.Load("img_win_2.jpg");
                    }
                    break;
            }
        }
    }
}
