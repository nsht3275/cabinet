using System;
using System.Collections;
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
    public partial class Form12 : Form
    {
        public System.Drawing.Image img = null;
        public ArrayList filelist = new ArrayList();
        public string m_ewdfilename = "";
        public string m_orderid = "";
        public string m_orderdate = "";
        public ArrayList m_grps = new ArrayList();
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
        }
        //生成安装图
        private void MakeAnZhuangTu()
        {
            //分析组
            if (m_ewdfilename.Length > 0)
            {
                if (System.IO.File.Exists(m_ewdfilename) || axEWdraw1.GetEntSize()>0)
                {
                    progressBar1.Value = 10;
                    progressBar1.Refresh();

                    axEWdraw1.OpenEwd(m_ewdfilename);
                    progressBar1.Value = 30;
                    progressBar1.Refresh();
                    int ent = 0;
                    int entsize = axEWdraw1.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        ent = axEWdraw1.GetEntID(i);
                        if (axEWdraw1.IsGroup(ent))
                        {
                            if(axEWdraw1.GetGroupAllIDs(ent))
                            {
                                string grpname = axEWdraw1.GetGroupName(ent);
                                if (grpname.IndexOf("_yt_") >= 0 ||
                                    grpname.IndexOf("_fjia_") >= 0 ||
                                    grpname.IndexOf("_bogu_") >= 0 ||
                                    grpname.IndexOf("_ct_") >= 0)
                                {
                                    continue;
                                }
                                int grpentsize = axEWdraw1.GetIDBufferSize();
                                if(grpentsize>0)
                                {
                                    CMyGrps agrp = new CMyGrps();
                                    agrp.m_name = grpname;
                                    for (int j = 0; j < grpentsize; j++)
                                        agrp.m_ids.Add(axEWdraw1.GetIDBuffer(j));
                                    m_grps.Add(agrp);
                                }
                                axEWdraw1.ClearIDBuffer();
                            }
                        }
                    }
                    if (m_grps.Count <= 0)
                    {
                        this.Close();
                        return;
                    }
                    if (m_grps.Count > 0 || axEWdraw1.GetEntSize()>0)
                    {
                        ArrayList insidecompents = new ArrayList();
                        double progress = 60 / m_grps.Count;
                        vScrollBar1.Minimum = 1;
                        vScrollBar1.Maximum = m_grps.Count;
                        vScrollBar1.Value = 1;
                        for (int i = 0; i < m_grps.Count; i++)
                        {
                            
                            if (((CMyGrps)m_grps[i]).m_name.IndexOf("_yt_") >= 0 ||
                                ((CMyGrps)m_grps[i]).m_name.IndexOf("_fjia_") >= 0 ||
                                ((CMyGrps)m_grps[i]).m_name.IndexOf("_bogu_") >= 0 ||
                                ((CMyGrps)m_grps[i]).m_name.IndexOf("_ct_") >= 0)
                            {
                                continue;
                            }
                            for (int j = 0; j < m_grps.Count; j++)
                            {
                                if (i != j)
                                {
                                    ent = (int)(((CMyGrps)m_grps[j]).m_ids[0]);
                                    axEWdraw1.SetEntityInvisible(ent, true);
                                    
                                }
                                else
                                {
                                    ent = (int)(((CMyGrps)m_grps[j]).m_ids[0]);
                                    GetCabinectInside(ent, ref insidecompents);
                                    axEWdraw1.SetEntityInvisible(ent, false);
                                }
                            }
                            if (insidecompents.Count > 0)
                            {//显示柜体内的对象
                                for (int j = 0; j < insidecompents.Count; j++)
                                {
                                    axEWdraw1.SetEntityInvisible((int)insidecompents[j], false);
                                }
                            }
                            ArrayList ids = new ArrayList();//ID集
                            ArrayList ids2 = new ArrayList();//侧视标注对去金额ID集2
                            ArrayList ids3 = new ArrayList();//原有的板材对象ID集3
                            ArrayList ids4 = new ArrayList();//临时对象ID集4
                            ArrayList ids5 = new ArrayList();//俯视标注对象,ID集5
                            ArrayList ids6 = new ArrayList();//所有新生成的对象,ID集5
                            ArrayList planks = new ArrayList(); //板子的信息(该类的信息内容可自定,例子里只有基本的大小信息)

                            axEWdraw1.SetPorjectViewSkipSpace(false);
                            //调入一个柜体 2018-11-12
                            axEWdraw1.SetBackGroundColor(516);
                            axEWdraw1.SetPerspectiveMode(true);
                            axEWdraw1.SetViewCondition(5);
                            //
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            //
                            double minx1, miny1, minz1, maxx1, maxy1, maxz1;
                            minx1 = miny1 = minz1 = maxx1 = maxy1 = maxz1 = 0.0;

                            /*得到要计算空间体的板材对象
                             * 这里的对象不一定是所有对象,
                             * 而是需要参数空间体计算的板材,例子里因为都是板材,所以都加上了.
                             * 实际操作时得根据需要去除不必要的板材,如小组件,抽屉或不必要的拉条等.不必每个都加.
                            */
                            entsize = ((CMyGrps)m_grps[i]).m_ids.Count;
                            ent = 0;
                            for (int j = 0; j < entsize; j++)
                            {
                                ent = (int)(((CMyGrps)m_grps[i]).m_ids[j]);
                                int type = axEWdraw1.GetEntType(ent);
                                if (axEWdraw1.IsDisplayed(ent))
                                {
                                    if (type != 501 && type != 502 && type != 503 &&
                                         !(type >= 93 && type <= 100)
                                        )
                                    {
                                        //判断每个实体的范围,以求得总的宽高
                                        axEWdraw1.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        //先将板的信息保存下来,以便之后对板的条件进行判断
                                        CPlank plank = new CPlank();
                                        plank.id = ent;
                                        plank.minx = minx;
                                        plank.miny = miny;
                                        plank.minz = minz;
                                        plank.maxx = maxx;
                                        plank.maxy = maxy;
                                        plank.maxz = maxz;
                                        plank.width = maxx - minx;
                                        plank.depth = maxy - miny;
                                        plank.height = maxz - minz;
                                        planks.Add(plank);//存入数据集中

                                        if (j == 0)
                                        {
                                            minx1 = minx;
                                            miny1 = miny;
                                            minz1 = minz;

                                            maxx1 = maxx;
                                            maxy1 = maxy;
                                            maxz1 = maxz;
                                        }
                                        else
                                        {
                                            minx1 = Math.Min(minx1, minx);
                                            miny1 = Math.Min(miny1, miny);
                                            minz1 = Math.Min(minz1, minz);

                                            maxx1 = Math.Max(maxx1, maxx);
                                            maxy1 = Math.Max(maxy1, maxy);
                                            maxz1 = Math.Max(maxz1, maxz);
                                        }
                                    }
                                }
                            }//保存板材信息结束
                            axEWdraw1.ClearIDBuffer();
                            for (int j = 0; j < planks.Count; j++)
                            {
                                axEWdraw1.AddIDToBuffer(((CPlank)planks[j]).id);
                                ids3.Add(((CPlank)planks[j]).id);
                            }
                            /*SeekCabinetSpace是搜索引当前柜体的空间体(Box盒体)
                             * 参数:无
                             * 返回:
                             * 如果成功返回空间体的个数,其它返回0
                             * 注意:
                             * 1.计算前需要将参与计算的板件的ID用addidtobuffer函数加到ID缓存集中
                             * 2.遍历空间体时,可以用GetCabinetSpace根据索引在循环中取得
                             */
                            int spacesize = axEWdraw1.SeekCabinetSpace();
                            int spaceent = 0;
                            int diment = 0;
                            axEWdraw1.SetDimTxt(38);
                            axEWdraw1.SetDimAsz(10);
                            axEWdraw1.SetDimDec(0);
                            axEWdraw1.SetDimClr(0, 0, 0);
                            axEWdraw1.SetDimTxsty("宋体");
                            axEWdraw1.SetDimTAD(10);
                            axEWdraw1.SetDimSkipViewProj(true);
                            for (int j = 0; j < spacesize; j++)
                            {
                                /*GetCabinetSpace根据索引取得空间体(Box盒体)
                                 * 参数:
                                 * inx              索引值
                                 * minx,miny,minz  空间体的最小点
                                 * maxx,maxy,maxz  空间体的最大点
                                 * 返回:
                                 * 如果成功,返回true,其它返回false
                                 */
                                axEWdraw1.GetCabinetSpace(j, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                //以向两行可以不要
                                //横向标注
                                diment = axEWdraw1.LengthDimension(new object[] { minx, miny, minz }, new object[] { maxx, miny, minz }, 0, new object[] { 0, -1.0, 0 }, false, "", new object[] { (minx + maxx) / 2.0, miny, minz + 10 });
                                ids.Add(diment);
                                ids6.Add(diment);//2018-11-12
                                //纵向标注
                                diment = axEWdraw1.LengthDimension(new object[] { minx, miny, minz }, new object[] { minx, miny, maxz }, 0, new object[] { 0, -1.0, 0 }, false, "", new object[] { minx + 10, (miny + maxy) / 2.0, (minz + maxz) / 2.0 });
                                ids.Add(diment);
                                ids6.Add(diment);//2018-11-12
                            }

                            axEWdraw1.ClearIDBuffer();
                            //总宽正向标注
                            diment = axEWdraw1.LengthDimension(new object[] { minx1, miny1, maxz1 }, new object[] { maxx1, miny1, maxz1 }, 0, new object[] { 0, -1.0, 0 }, false, "", new object[] { (minx1 + maxx1) / 2.0, miny1, maxz1 + 20 });
                            ids6.Add(diment);//2018-11-12
                            //总高正向标注
                            diment = axEWdraw1.LengthDimension(new object[] { minx1, miny1, minz1 }, new object[] { minx1, miny1, maxz1 }, 0, new object[] { 0, -1.0, 0 }, false, "", new object[] { minx1 - 20, (miny1 + maxy1) / 2.0, (minz1 + maxz1) / 2.0 });
                            ids6.Add(diment);//2018-11-12

                            //设置右视图投射
                            diment = axEWdraw1.LengthDimension(new object[] { maxx1, miny1, maxz1 }, new object[] { maxx1, maxy1, maxz1 }, 0, new object[] { 1, 0.0, 0 }, false, "", new object[] { maxx, (miny1 + maxy1) / 2.0, maxz1 + 20 });
                            ids2.Add(diment);
                            ids6.Add(diment);//2018-11-12
                            diment = axEWdraw1.LengthDimension(new object[] { maxx1, maxy1, minz1 }, new object[] { maxx1, maxy1, maxz1 }, 0, new object[] { 1, 0.0, 0 }, false, "", new object[] { maxx, maxy1 + 20, (minz1 + maxz1) / 2.0 });
                            ids2.Add(diment);
                            ids6.Add(diment);//2018-11-12

                            //设置俯视图投射
                            diment = axEWdraw1.LengthDimension(new object[] { minx1, maxy1, maxz1 }, new object[] { maxx1, maxy1, maxz1 }, 0, new object[] { 0, 0.0, 1 }, false, "", new object[] { (minx1 + maxx1) / 2.0, maxy1 + 20, maxz1 });
                            ids5.Add(diment);
                            diment = axEWdraw1.LengthDimension(new object[] { minx1, miny1, maxz1 }, new object[] { minx1, maxy1, maxz1 }, 0, new object[] { 0, 0.0, 1 }, false, "", new object[] { minx1 - 20, (miny1 + maxy1) / 2.0, maxz1 });
                            ids5.Add(diment);

                            //复制侧视图 2018-11-12
                            for (int j = 0; j < ids3.Count; j++)
                            {
                                ent = axEWdraw1.Copy((int)ids3[j], new object[] { 0, 0, 0 }, new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, 0 });
                                ids4.Add(ent);
                                ids6.Add(ent);//2018-11-12
                            }
                            for (int j = 0; j < ids2.Count; j++)
                            {
                                ent = axEWdraw1.Copy((int)ids2[j], new object[] { 0, 0, 0 }, new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, 0 });
                                ids4.Add(ent);
                                ids6.Add(ent);//2018-11-12
                            }

                            for (int j = 0; j < ids4.Count; j++)
                            {
                                axEWdraw1.Rotate((int)ids4[j], new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, 0 }, new object[] { 0, 0, 1 }, -90);
                            }

                            for (int j = 0; j < ids2.Count; j++)
                            {
                                axEWdraw1.Delete((int)ids2[j]);
                            }
                            ids2.Clear();
                            ids4.Clear();

                            //俯视图 2018-11-12
                            for (int j = 0; j < ids3.Count; j++)
                            {
                                ent = axEWdraw1.Copy((int)ids3[j], new object[] { minx1, 0, 0 }, new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1) * 2.0, 0, 0 });
                                ids4.Add(ent);
                                ids6.Add(ent);//2018-11-12
                            }

                            for (int j = 0; j < ids5.Count; j++)
                            {
                                ent = axEWdraw1.Copy((int)ids5[j], new object[] { minx1, 0, 0 }, new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1) * 2.0, 0, 0 });
                                ids4.Add(ent);
                                ids6.Add(ent);//2018-11-12
                            }

                            for (int j = 0; j < ids4.Count; j++)
                            {
                                axEWdraw1.Rotate((int)ids4[j], new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, 0 }, new object[] { 1, 0, 0 }, 90);
                                axEWdraw1.MoveTo((int)ids4[j], new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, 0 }, new object[] { maxx1 + (maxx1 - minx1) / 3.0 + (maxy1 - miny1), 0, maxz1 });
                            }

                            for (int j = 0; j < ids5.Count; j++)
                            {
                                axEWdraw1.Delete((int)ids5[j]);
                            }
                            ids5.Clear();

                            /*ProjectViewToBMP投射到BMP图形的函数
                            * 参数:
                            * 文件名     要投射并导出的BMP文件名
                            * 宽度       BMP文件的宽度(注意可以根据自行设定的比例计算得出)
                            * 高度       BMP文件的高度(注意可以根据自行设定的比例计算得出)
                            * 投射的方向 要投射的方向
                            */
                            string bmpfilename = ConvertPrjIDToEWDPath(((CMyGrps)m_grps[i]).m_name + i.ToString() + ".bmp");
                            filelist.Add(bmpfilename);
                            axEWdraw1.ProjectViewToBMP(bmpfilename, 1333, 768, new object[] { 0, -1, 0 });//"d:\\Front.bmp"
                            //
                            for (int j = 0; j < ids6.Count; j++)
                            {
                                axEWdraw1.Delete((int)ids6[j]);//2018-11-12
                            }
                            ids6.Clear();
                            ids3.Clear();
                            ids.Clear();
                            //
                            ids2.Clear();
                            ids4.Clear();
                            ids5.Clear();
                            planks.Clear();
                            //break;
                            progressBar1.Value += (int)progress;
                            progressBar1.Refresh();
                            if (insidecompents.Count > 0)
                            {//显示柜体内的对象
                                for (int j = 0; j < insidecompents.Count; j++)
                                {
                                    axEWdraw1.SetEntityInvisible((int)insidecompents[j], true);
                                }
                            }
                            insidecompents.Clear();

                        }
                        progressBar1.Value = 100;
                        label1.Text = "完成";
                        progressBar1.Refresh();
                        label1.Refresh();
                        axEWdraw1.Visible = false;
                        pictureBox1.Visible = true;
                        vScrollBar1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = true;
                        label4.Visible = true;
                        label1.Text = "当前安装图:";
                        label2.Text = filelist.Count.ToString();
                        label3.Text = "/";
                        label4.Text = vScrollBar1.Maximum.ToString();
                        if (filelist.Count > 0)
                        {
                            vScrollBar1.Value = filelist.Count;
                            vScrollBar1.Refresh();
                            if(img != null)
                                img.Dispose();
                            img = System.Drawing.Image.FromFile((string)filelist[vScrollBar1.Value - 1]);
                            pictureBox1.Image = img;//.Load((string)filelist[vScrollBar1.Value - 1]);
                            
                        }
                        Height = 758;
                        Size size = Screen.PrimaryScreen.WorkingArea.Size;
                        Left = (size.Width - Width) / 2;
                        Top = (size.Height - Height) / 2;
                        WindowState = FormWindowState.Normal;
                    }
                }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MakeAnZhuangTu();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if(filelist.Count>0)
            {
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();
                if (img != null)
                    img.Dispose();
                img = System.Drawing.Image.FromFile((string)filelist[vScrollBar1.Value - 1]);
                pictureBox1.Image = img;//
                label2.Text = vScrollBar1.Value.ToString();
            }
        }

        //取得柜体内部的组对象ID集
        private void GetCabinectInside(int orgid, ref ArrayList ids)
        {
            double gminx, gminy, gminz, gmaxx, gmaxy, gmaxz;
            gminx = gminy = gminz = gmaxx = gmaxy = gmaxz = 0.0;

            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;

            double midx, midy, midz;
            midx = midy = midz = 0.0;
            //
            axEWdraw1.GetEntBoundingBox(orgid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            int ent = 0;
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                if (axEWdraw1.IsGroup(ent) && ent != orgid)
                {
                    axEWdraw1.GetGroupBndPt(ent, 0, ref gminx, ref gminy, ref gminz);//得到组的包围盒的拐点 0：底面左下角三维坐标
                    axEWdraw1.GetGroupBndPt(ent, 6, ref gmaxx, ref gmaxy, ref gmaxz);//得到组的包围盒的拐点 6：顶面右上角三维坐标
                    //
                    midx = (gmaxx + gminx) / 2.0;
                    midy = (gmaxy + gminy) / 2.0;
                    midz = (gmaxz + gminz) / 2.0;
                    if (midx >= minx && midy >= miny && midz >= minz &&
                        midx <= maxx && midy <= maxy && midz <= maxz
                        )
                    {
                        ids.Add(ent);
                    }
                }
            }
        }

        private void Form12_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (img != null)
                img.Dispose();
        }

    }
}
