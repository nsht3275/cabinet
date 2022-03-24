using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Pipe;
using Code;
namespace Room
{
    
    public partial class Form10 : Form
    {
        //该变量是为了外部访问该窗体
        public static Form10 g_static_fm10;
        //是否是查看G代码状态
        bool isViewGcode = false;
        //判断正反面
        int g_zf = 0;
        bool isinit = false;
        //当前默认的文件名,是根据单号生成的
        public string m_ewdfilename = "";
        //查看G代码是,临时的文件名
        public string m_ncfilename = "";
        //单号
        public string m_orderid = "";
        //下单日期
        public string m_orderdate = "";
        //下单用户名
        public string m_username = "";
        public int g_allsize = 0;
        //
        public ArrayList imagefiles = new ArrayList();//排版图文件列表
        public ArrayList codefiles = new ArrayList();//条形码文件列表
        public ArrayList cabinetnames = new ArrayList();//柜体名称列表
        public ArrayList matnames = new ArrayList();//材质名称列表
        public ArrayList planksizes = new ArrayList();//板材的大小列表
        public ArrayList matcolors = new ArrayList();//材质名称列表

        //2019-12-20
        public List<zCaiLiao.CaiLiaoEntity> g_allitems = null;
        public List<zCaiLiao.CaiLiaoEntity> g_allitemsbak = null;
        public List<zCaiLiao.CaiLiaoEntity> g_prjallitems = null;
        public double m_curwidth = 1220;//当前宽度
        public double m_curheight = 2240;//当前高度
        public ArrayList m_tplanks = new ArrayList();
        public ArrayList m_needplanks = new ArrayList();
        public bool isexistmatfile = false;
        //
        public ArrayList g_allfaces = new ArrayList();
        public ArrayList g_alltxts = new ArrayList();
        
        public Form10()
        {
            InitializeComponent();
            g_static_fm10 = this;
        }

        public static void GetAllNCFileDir(string file,ref ArrayList ncfiles)
        {
            try
            {
                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            string extname = System.IO.Path.GetExtension(f);
                            if (extname.ToLower() == ".nc")
                            {
                                ncfiles.Add(f);
                            }
                        }
                    }
                }

            }
            catch (Exception ex) // 异常处理
            {
                MessageBox.Show(ex.Message.ToString());// 异常信息
            }
        }

        private bool AddTPlank(ref CPlankTClass aplank)
        {
            bool isadd = true;
            for (int i = 0; i < m_tplanks.Count; i++)
            {
                if (Math.Abs(((CPlankTClass)m_tplanks[i]).thickness - aplank.thickness) < 0.001)
                {
                    isadd = false;
                    break;
                }
            }
            if (isadd)
            {
                m_tplanks.Add(aplank);
                return true;
            }
            return false;
        }

        //读入物料 2019-12-20
        private void LoadMats()
        {
            string filename = ConvertPrjIDToEWDPath("prjmats.xml",false);
            if (System.IO.File.Exists(filename))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                if (g_allitems != null)
                {
                    if (g_allitems.Count > 0)
                    {
                        g_allitems.Clear();
                        g_allitems = null;
                    }
                }
                g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                isexistmatfile = true;
            }
            else
            {//如果不存在,则创建一个默认的值
                string filename1 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
                if (System.IO.File.Exists(filename1))
                {
                    zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename1;
                    g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                }
                else
                {
                    MessageBox.Show("没有所需的物料,请添加物料后重试!");
                    this.Close();
                }
            }
        }
        /*
         * 判断是否存该板材的规格 2019-12-19
        */
        private bool IsCheckCaiLiao(ref List<zCaiLiao.CaiLiaoEntity> list, ref ArrayList orglist, ref ArrayList outlist, bool isexist)
        {
            if (!isexist)
            {
                string filename = ConvertPrjIDToEWDPath("prjmats.xml",false);
                if (System.IO.File.Exists(filename))
                {
                    isexist = true;
                }
            }
            if (orglist.Count <= 0)
            {
                bool ishavecheck = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsCheck)
                    {
                        ishavecheck = true;
                        return true;
                    }
                }
                return ishavecheck;
            }
            else
            {
                bool result = true;
                if (!isexist)
                {//如果不存在项目的材料对象
                    for (int i = 0; i < orglist.Count; i++)
                    {
                        bool isfind = true;
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (Math.Abs(((CPlankTClass)orglist[i]).thickness - Convert.ToDouble(list[j].Thickness)) < 0.001)
                            {
                                isfind = true;
                                list[j].IsCheck = true;
                                outlist.Add(((CPlankTClass)orglist[i]).thickness);
                                break;
                            }
                        }
                        //if (!isfind)
                    }
                    if (outlist.Count > 0)
                        return false;
                    return result;
                }
                else
                {//如果存在项目的材料对象文件
                    for (int i = 0; i < orglist.Count; i++)
                    {
                        bool isfind = false;
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (list[j].IsCheck)//从已经选择的材料中查找
                            {
                                if (Math.Abs(((CPlankTClass)orglist[i]).thickness - Convert.ToDouble(list[j].Thickness)) < 0.001)
                                {
                                    isfind = true;
                                    break;
                                }
                            }
                        }
                        if (!isfind)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (!list[j].IsCheck)//从非选择的材料中查找
                                {
                                    if (Math.Abs(((CPlankTClass)orglist[i]).thickness - Convert.ToDouble(list[j].Thickness)) < 0.001)
                                    {
                                        isfind = true;
                                        list[j].IsCheck = true;
                                        outlist.Add(((CPlankTClass)orglist[i]).thickness);
                                        break;
                                    }
                                }
                            }
                        }
                        if (!isfind)
                        {//仍然没有找到,则记录该规则的板材
                            if (!isfind)
                                outlist.Add(((CPlankTClass)orglist[i]).thickness);
                        }
                    }
                    if (outlist.Count > 0)
                        return false;
                    return result;
                }
            }
            return false;
        }

        //
        private void Form10_Load(object sender, EventArgs e)
        {
            axEWdraw1.ComposeForceV(false);//是否强制竖型排列
            axEWdraw1.DisableComposeGroove(true);
            axEWdraw1.ComposeMinVSpace(20);//2021-04-09
            axEWdraw1.DisableYXComposePage(true);
            SetTInfo();
            ArrayList invalidlist = new ArrayList();
            textBox1.Visible = false;
            isinit = true;
            axEWdraw1.DisableRightMenu(true);
            axEWdraw1.SetBackGroundColor(axEWdraw1.RGBToIndex(214, 204, 221));
            if (m_ewdfilename.Length > 3)
            {
                int shareentsize = Form1.g_form1.axEWdraw3.GetShareEntSize();
                for (int i = 0; i < shareentsize; i++)
                {
                    axEWdraw1.GetEntFromShare(i, true);
                }
            }
            int entsize = axEWdraw1.GetEntSize();
            axEWdraw1.ReCalWj();
            axEWdraw1.ForceRestorePlankCoordSys();
            m_ncfilename = ConvertPrjIDToEWDPath("ewdraw_out.nc",false);
            entsize = axEWdraw1.GetEntSize();
            double w, h, t,a;
            w = h = t = a = 0.0;
            string cname = "";
            string gname = "";//2020-02-27
            int ent = 0;
            int type = 0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type != 501 && type != 502 && type != 503 &&
                    !(type >= 93 && type <= 100)
                    )
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        axEWdraw1.GetGroupAllIDs(ent);
                        for (int j = 0; j < axEWdraw1.GetIDBufferSize(); j++)
                        {
                            ent = axEWdraw1.GetIDBuffer(j);
                            type = axEWdraw1.GetEntType(ent);
                            if (type != 501 && type != 502 && type != 503 &&
                                !(type >= 93 && type <= 100)
                                )
                            {
                                if (axEWdraw1.GetPlankWHTA(ent, ref w, ref h, ref t, ref a, ref cname, ref gname))
                                {
                                    if (cname.IndexOf("效果") >= 0)
                                    {
                                        invalidlist.Add(ent);
                                        continue;
                                    }
                                    //根据厚度判断板类 2019-12-20
                                    CPlankTClass aptclass = new CPlankTClass();
                                    aptclass.thickness = t;
                                    AddTPlank(ref aptclass);
                                }
                            }
                        }
                        axEWdraw1.ClearIDBuffer();
                    }
                    else
                    {
                        ent = axEWdraw1.GetEntID(i);
                        type = axEWdraw1.GetEntType(ent);
                        if (type != 501 && type != 502 && type != 503 &&
                            !(type >= 93 && type <= 100)
                            )
                        {
                            if (axEWdraw1.GetPlankWHTA(ent, ref w, ref h, ref t, ref a, ref cname, ref gname))
                            {
                                if (cname.IndexOf("效果") >= 0)
                                {
                                    invalidlist.Add(ent);
                                    axEWdraw1.SetEntityInvisible(ent, true);
                                    continue;
                                }
                                //根据厚度判断板类 2019-12-20
                                CPlankTClass aptclass = new CPlankTClass();
                                aptclass.thickness = t;
                                AddTPlank(ref aptclass);
                            }
                        }
                    }
                }
                //
                axEWdraw1.SetEntityInvisible(ent, true);
            }
            //2020-04-03
            if (invalidlist.Count > 0)
            {
                for (int i = 0; i < invalidlist.Count; i++)
                {
                    if (axEWdraw1.IsGroup((int)invalidlist[i]))
                    {
                        axEWdraw1.Explode((int)invalidlist[i]);
                    }
                }
                for (int i = 0; i < invalidlist.Count; i++)
                {
                    axEWdraw1.Delete((int)invalidlist[i]);
                }
            }
            //
            //判断需要添加的物料 2019-12-20
            LoadMats();
            if (g_allitems.Count > 0)
                axEWdraw1.InitComposePage(Convert.ToInt32(g_allitems[0].Width),Convert.ToInt32(g_allitems[0].Height),3);
            m_needplanks.Clear();
            IsCheckCaiLiao(ref g_allitems, ref m_tplanks, ref m_needplanks, isexistmatfile);
            string str = "";
            if (m_needplanks.Count > 0)
            {
                str = "请点击\"选择物料\"按钮,选择板厚分别为:";
                for (int i = 0; i < m_needplanks.Count; i++)
                {
                    str += Convert.ToDouble(m_needplanks[i]).ToString("0.");
                    if (i + 1 < m_needplanks.Count)
                        str += ",";
                }
                str += " 的板材物料";
                label4.Text = str;
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.Text = "";
                label4.ForeColor = Color.Black;
            }
            axEWdraw1.ClearOrgPage();
            if(g_allitems.Count>0)
            {
                for (int i = 0; i < g_allitems.Count; i++)
                {
                    if(g_allitems[i].IsCheck)
                    {
                        axEWdraw1.AddOrgPageWHT(
                            Convert.ToDouble(g_allitems[i].Width),
                            Convert.ToDouble(g_allitems[i].Height),
                            Convert.ToDouble(g_allitems[i].Thickness),
                            g_allitems[i].Name,
                            g_allitems[i].Color
                            );
                    }
                }
            }
            //
            int g_allsize = axEWdraw1.ComposePage();
            //return;
            int inx = axEWdraw1.GetComposeMostPage();
            g_allsize = axEWdraw1.GetComposePageSize();
            //
            vScrollBar1.Minimum = 1;
            vScrollBar1.Maximum = g_allsize;
            if (inx <= -1)
                inx = 0;
            vScrollBar1.Value = inx + 1;
            str = (inx + 1).ToString();
            label1.Text = str;
            str = g_allsize.ToString();
            label3.Text = str;
            axEWdraw1.GetComposePage(inx, true, m_ncfilename);
            axEWdraw1.GetComposePageWH(inx, ref m_curwidth, ref m_curheight);//2019-12-25
            //
            axEWdraw1.AddOne3DVertex(0, 0, -10.0);
            axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);//2019-12-25
            axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);//2019-12-25
            axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);//2019-12-25
            int allent = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            int faceent = axEWdraw1.EntToFace(allent, true);
            axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
            g_allfaces.Add(faceent);
            axEWdraw1.ClearExportID();//2018-06-25
            str = "板厚:" + axEWdraw1.GetComposePageThickness(inx).ToString("0.00")+"mm";
            int txt = axEWdraw1.Text3D(str, "宋体", new object[] {900,2400,1 }, 32, 0, 0);
            axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
            g_alltxts.Add(txt);
            axEWdraw1.SetEntColor(txt, 0);
            int outlinesize = axEWdraw1.GetComposeOutlineSize();

            for (int i = 0; i < outlinesize; i++)
            {
                if (!axEWdraw1.IsPlankHoleOutline(i))
                {
                    faceent = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                    string name = axEWdraw1.GetComposeOutlineName(i);
                    axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                    g_allfaces.Add(faceent);
                    DrawText3D("", name, faceent, 32);//板材说明信息
                }
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));//2018-06-25
            }
            int holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
            {
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeHole(i));//2018-06-25
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeHole(i), axEWdraw1.RGBToIndex(255, 0, 0));
            }
            //槽
            int groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
            {
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeGroove(i));//2018-06-25
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeGroove(i), axEWdraw1.RGBToIndex(0, 255, 0));
            }
            axEWdraw1.ZoomALL();
            g_zf = 0;
            isinit = false;
            Form1.g_form1.axEWdraw3.ClearShare();
            //
            int selfwjsize = axEWdraw1.GetSelfDefWJSize();
            double x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4;
            double nx, ny, nz;
            x1 = y1 = z1 = x2 = y2 = z2 = x3 = y3 = z3 = x4 = y4 = z4 = nx = ny = nz = 0.0;

            for (int i = 0; i < selfwjsize; i++)
            {
                axEWdraw1.GetSelfDefWJRect(i, ref x1, ref y1, ref z1,
                                            ref x2, ref y2, ref z2,
                                            ref x3, ref y3, ref z3,
                                            ref x4, ref y4, ref z4);
                MessageBox.Show(z1.ToString());
                axEWdraw1.GetSelfDefWJNormal(i, ref nx, ref ny, ref nz);
                int tpid = axEWdraw1.GetSelfDefWJPlankID(i);

                double minX, minY, minZ, maxX, maxY, maxZ;
                minX = minY = minZ = maxX = maxY = maxZ = 0.0;
                axEWdraw1.GetEntBoundingBox(tpid, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                double bx, by, bz;
                bx = by = bz = 0.0;
                double dx=0,dy=0,dz=0;
                axEWdraw1.GetSelfDefWJBasePt(i, ref bx, ref by, ref bz);

                axEWdraw1.GetSelfDefWJNormal(i, ref dx, ref dy, ref dz);
                MessageBox.Show(dx.ToString() + "," + dy.ToString() + "," + dz.ToString());

                axEWdraw1.GetSelfDefWJDir1(i, ref dx, ref dy, ref dz);
                MessageBox.Show(dx.ToString() + "," + dy.ToString() + "," + dz.ToString());

                axEWdraw1.GetSelfDefWJDir2(i, ref dx, ref dy, ref dz);
                MessageBox.Show(dx.ToString() + "," + dy.ToString()+","+dz.ToString());
                axEWdraw1.Clear3DPtBuf();
                axEWdraw1.AddOne3DVertex(x1, y1, z1);
                axEWdraw1.AddOne3DVertex(x2, y2, z2);
                axEWdraw1.AddOne3DVertex(x3, y3, z3);
                axEWdraw1.AddOne3DVertex(x4, y4, z4);
                axEWdraw1.PolyLine3D(true);
                axEWdraw1.Clear3DPtBuf();
                axEWdraw1.Point(new object[] { bx, by, bz });
            }
            
        }

        private string ConvertPrjIDToEWDPath(string inname = "",bool isnew = true)
        {
            string filename = "";
            if (m_orderid.Length > 0)
            {

                string year = DateTime.Now.ToString("yyyy");//g_orderid.Substring(0, 2);
                string oyear = Convert.ToDateTime(m_orderdate).Year.ToString();
                if (inname.Length > 0)
                {
                    if(isnew)
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

        private string ConvertPrjIDToPath(bool isnew = true)
        {
            string filename = "";
            if (m_orderid.Length > 0)
            {

                string year = DateTime.Now.ToString("yyyy");//g_orderid.Substring(0, 2);
                string oyear = Convert.ToDateTime(m_orderdate).Year.ToString();
                if (isnew)
                    filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + m_orderid + "\\";
                else
                    filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + m_orderid + "\\";
            }
            return filename;
        }

        private void DrawText3D(string ord, string name, int ID, double fh)
        {
            if (ID > 0)
            {
                double minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = 0;
                axEWdraw1.GetEntBoundingBox(ID, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                double width = maxx - minx;
                double height = maxy - miny;
                double mx, my;
                mx = my = 0;
                string str;
                mx = (maxx + minx) / 2.0;
                my = (maxy + miny) / 2.0;
                if (width > height)
                {//横向
                    str = ord + name;
                    if (str.Length > 0)
                    {
                        double x1 = mx - str.Length * fh / 1.6;
                        double y1 = my + fh / 2.0;
                        int txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1, y1, 1 }, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                        str = Convert.ToUInt32(width).ToString() + " * " + Convert.ToUInt32(height).ToString();
                        x1 = mx - str.Length * fh * 2.0 / 3.0 / 2.0;
                        y1 = my - fh / 2.0;
                        txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1,y1,1}, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                    }
                    else
                    {
                        str = Convert.ToUInt32(width).ToString() + " * " + Convert.ToUInt32(height).ToString();
                        double x1 = mx - str.Length * fh * 2.0 / 3.0 / 2.0;
                        double y1 = my;
                        int txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1,y1,1}, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                    }
                }
                else
                {//纵向
                    str = ord + name;
                    if (str.Length > 0)
                    {
                        double x1 = mx + fh / 2.0;
                        double y1 = my + str.Length * fh / 1.2;
                        int txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1,y1,1}, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                        axEWdraw1.Rotate(txt,new object[] { x1,y1,0.1} , new object[] {0, 0, 1 }, -90);
                        x1 = mx - fh / 2.0;
                        y1 = my + str.Length * fh / 1.3;// 
                        str = Convert.ToUInt32(width).ToString() + " * " + Convert.ToUInt32(height).ToString();
                        txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1, y1, 1 }, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                        axEWdraw1.Rotate(txt, new object[] { x1, y1, 0.1 }, new object[] { 0, 0, 1 }, -90);
                    }
                    else
                    {
                        str = Convert.ToUInt32(width).ToString() + " * " + Convert.ToUInt32(height).ToString();
                        double x1 = mx;
                        double y1 = my + str.Length * fh * 2.0 / 3.0 / 2.0;
                        int txt = axEWdraw1.Text3D(str, "Arial", new object[] { x1,y1,1}, fh, 0, 0);
                        g_alltxts.Add(txt);
                        axEWdraw1.SetEntColor(txt, 0);
                        axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                        axEWdraw1.Rotate(txt, new object[] { x1, y1, 0.1 }, new object[] { 0,0,1}, -90);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: 在此添加控件通知处理程序代码
            if(!axEWdraw1.Visible)
                axEWdraw1.Visible = true;
            if (textBox1.Visible)
                textBox1.Visible = false;
            //删除原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.Delete((int)g_alltxts[i]);
            }
            g_alltxts.Clear();

            if (g_zf == 0)
            {
                int inx = vScrollBar1.Value;//
                axEWdraw1.GetComposePageWH(inx-1, ref m_curwidth, ref m_curheight);//2019-12-25
                button1.Text = "查看正面";
                int ent = 0;
                int nent = 0;
                //删除原有的纹理面
                for (int i = 0; i < g_allfaces.Count; i++)
                {
                    axEWdraw1.Delete((int)g_allfaces[i]);
                }
                g_allfaces.Clear();
                int faceent = 0;
                axEWdraw1.ClearExportID();//2018-06-28
                //镜像原有的轮廓线(翻板)
                int outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int i = 0; i < outlinesize; i++)
                {
                    nent = axEWdraw1.Mirror(ent = axEWdraw1.GetComposeOutline(i), new object[] { 0, 0, 0 }, new object[] { 0, 1, 0 });
                    axEWdraw1.AddEntToExp(nent);//2018-06-28
                    axEWdraw1.Delete(ent);
                    axEWdraw1.MoveTo(nent, new object[] { -m_curwidth, 0, 0 }, new object[] { 0, 0, 0 });
                    g_allfaces.Add(nent);//2018-07-06
                    faceent = axEWdraw1.EntToFace(nent, false);
                    axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                    g_allfaces.Add(faceent);
                    string name = axEWdraw1.GetComposeOutlineName(i);
                    DrawText3D("", name, faceent, 32);//板材说明信息
                }
                //删除原有的洞
                int holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeHole(i));
                //删除原有的槽
                int groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeGroove(i));
                //
                
                axEWdraw1.GetComposePage(inx - 1, false, m_ncfilename);
                if (isViewGcode)
                {
                    if (textBox1.Visible)
                        textBox1.Text = System.IO.File.ReadAllText(m_ncfilename);
                }

                //镜像洞
                holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                {
                    nent = axEWdraw1.Mirror(ent = axEWdraw1.GetComposeHole(i), new object[] { 0,0,0}, new object[] { 0, 1, 0 });
                    axEWdraw1.Delete(ent);
                    axEWdraw1.MoveTo(nent, new object[] { -m_curwidth, 0, 0 }, new object[] { 0, 0, 0 });
                    axEWdraw1.AddEntToExp(nent);//2018-06-28
                    axEWdraw1.SetEntColor(nent, axEWdraw1.RGBToIndex(255, 0, 0));
                    g_allfaces.Add(nent);
                }

                //镜像槽
                groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                {
                    nent = axEWdraw1.Mirror(ent = axEWdraw1.GetComposeGroove(i), new object[] { 0,0,0}, new object[] { 0, 1, 0 });
                    axEWdraw1.Delete(ent);
                    axEWdraw1.MoveTo(nent, new object[] { -m_curwidth, 0, 0 }, new object[] { 0, 0, 0 });
                    axEWdraw1.AddEntToExp(nent);//2018-06-28
                    axEWdraw1.SetEntColor(nent, axEWdraw1.RGBToIndex(0, 255, 0));
                    g_allfaces.Add(nent);
                }
                //
                groovesize = axEWdraw1.GetComposeGrooveSize();
                axEWdraw1.AddOne3DVertex(0, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);
                axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);
                int allent = axEWdraw1.PolyLine3D(true);
                axEWdraw1.Clear3DPtBuf();
                faceent = axEWdraw1.EntToFace(allent, true);
                axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
                g_allfaces.Add(faceent);
                g_zf = 1;
            }
            else if (g_zf == 1)
            {
                button1.Text = "查看反面";
                //判断页数并创建排版对象 2018-06-14
                //删除原有的文字
                for (int i = 0; i < g_alltxts.Count; i++)
                {
                    axEWdraw1.Delete((int)g_alltxts[i]);
                }
                g_alltxts.Clear();
                //删除原有的纹理面
                for (int i = 0; i < g_allfaces.Count; i++)
                {
                    axEWdraw1.Delete((int)g_allfaces[i]);
                }
                g_allfaces.Clear();
                //删除原有的轮廓线
                int outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int i = 0; i < outlinesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeOutline(i));
                //删除原有的洞
                int holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeHole(i));
                //删除原有的槽
                int groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeGroove(i));
                //
                axEWdraw1.GetComposePage(vScrollBar1.Value - 1, true, m_ncfilename);
                if (isViewGcode)
                {
                    if (textBox1.Visible)
                        textBox1.Text = System.IO.File.ReadAllText(m_ncfilename);
                }
                //
                axEWdraw1.AddOne3DVertex(0, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);
                axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);
                int allent = axEWdraw1.PolyLine3D(true);
                axEWdraw1.Clear3DPtBuf();
                int faceent = axEWdraw1.EntToFace(allent, true);
                axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
                g_allfaces.Add(faceent);
                axEWdraw1.ClearExportID();//2018-06-28
                outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int i = 0; i < outlinesize; i++)
                {
                    if (!axEWdraw1.IsPlankHoleOutline(i))
                    {
                        faceent = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                        axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                        g_allfaces.Add(faceent);
                        string name = axEWdraw1.GetComposeOutlineName(i);
                        DrawText3D("", name, faceent, 32);//板材说明信息
                    }
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));//2018-06-28
                }
                holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                {
                    axEWdraw1.SetEntColor(axEWdraw1.GetComposeHole(i), axEWdraw1.RGBToIndex(255, 0, 0));
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeHole(i));//2018-06-28
                }

                //槽
                groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                {
                    axEWdraw1.SetEntColor(axEWdraw1.GetComposeGroove(i), axEWdraw1.RGBToIndex(0, 255, 0));
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeGroove(i));//2018-06-28
                }
                axEWdraw1.ZoomALL();
                g_zf = 0;
            }

        }

        //退出
        private void button4_Click(object sender, EventArgs e)
        {
            //axEWdraw1.ToDrawOrbit();
            axEWdraw1.SaveEwd("d:\\testcompose.ewd");
            return;

            if (isViewGcode)
            {//判断是否是查看G代码状态,如果是,则先退出查看代码状态
                isViewGcode = false;
                if (!axEWdraw1.Visible)
                    axEWdraw1.Visible = true;
                if (textBox1.Visible)
                    textBox1.Visible = false;
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if(!isinit)
            {
                int iPos = vScrollBar1.Value;
                string sss;
                sss = iPos.ToString();
                label1.Text = sss;
                //判断页数并创建排版对象 2018-06-14
                //删除原有的文字
                for (int i = 0; i < g_alltxts.Count; i++)
                {
                    axEWdraw1.Delete((int)(g_alltxts[i]));
                }
                g_alltxts.Clear();
                //删除原有的纹理面
                for (int i = 0; i < g_allfaces.Count; i++)
                {
                    axEWdraw1.Delete((int)(g_allfaces[i]));
                }
                g_allfaces.Clear();
                //删除原有的轮廓线
                int outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int i = 0; i < outlinesize; i++)
                {
                    axEWdraw1.Delete(axEWdraw1.GetComposeOutline(i));
                }
                //删除原有的洞
                int holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeHole(i));
                //删除原有的槽
                int groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeGroove(i));
                //
                axEWdraw1.GetComposePage(iPos - 1, true, m_ncfilename);
                if (isViewGcode)
                {
                    if(textBox1.Visible)
                        textBox1.Text = System.IO.File.ReadAllText(m_ncfilename);
                }
                axEWdraw1.GetComposePageWH(iPos - 1, ref m_curwidth, ref m_curheight);//2019-12-25
                //2018-07-05
                string str;
                str = "板厚:" + ((int)axEWdraw1.GetComposePageThickness(iPos - 1)).ToString("0.00") + "mm";
                int txt = axEWdraw1.Text3D(str, "宋体", new object[] { 900, 2400, 1 }, 32, 0, 0);
                axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
                g_allfaces.Add(txt);
                //
                axEWdraw1.AddOne3DVertex(0, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);
                axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);
                axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);
                int allent = axEWdraw1.PolyLine3D(true);
                axEWdraw1.Clear3DPtBuf();
                int faceent = axEWdraw1.EntToFace(allent, true);
                axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
                g_allfaces.Add(faceent);
                outlinesize = axEWdraw1.GetComposeOutlineSize();
                axEWdraw1.ClearExportID();//2018-06-28
                for (int i = 0; i < outlinesize; i++)
                {
                    if (!axEWdraw1.IsPlankHoleOutline(i))
                    {
                        faceent = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                        axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                        g_allfaces.Add(faceent);
                        string name = axEWdraw1.GetComposeOutlineName(i);
                        DrawText3D("", name, faceent, 32);//板材说明信息

                    }
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));//2018-06-28
                }
                holesize = axEWdraw1.GetComposeHoleSize();
                for (int i = 0; i < holesize; i++)
                {
                    axEWdraw1.SetEntColor(axEWdraw1.GetComposeHole(i), axEWdraw1.RGBToIndex(255, 0, 0));
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeHole(i));//2018-06-28
                }
                //槽
                groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int i = 0; i < groovesize; i++)
                {
                    axEWdraw1.SetEntColor(axEWdraw1.GetComposeGroove(i), axEWdraw1.RGBToIndex(0, 255, 0));
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeGroove(i));//2018-06-28
                }
                axEWdraw1.ZoomALL();
                button1.Text = "查看反面";
            }
        }
        private void GetBMPFile()
        {
            //隐藏孔，槽，面
            //原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.SetEntityInvisible((int)(g_alltxts[i]),true);
            }
            //原有的纹理面
            for (int i = 0; i < g_allfaces.Count; i++)
            {
                axEWdraw1.SetEntityInvisible((int)(g_allfaces[i]),true);
            }
            int holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
                axEWdraw1.SetEntityInvisible(axEWdraw1.GetComposeHole(i),true);
            //原有的槽
            int groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
                axEWdraw1.SetEntityInvisible(axEWdraw1.GetComposeGroove(i),true);
            //轮廓线所对应板材原始ID号
            int outlineorgidsize = axEWdraw1.GetComposeOrgIDSize();
            int orgid = 0;
            int newcopyid = 0;
            int newfaceid = 0;
            string path = ConvertPrjIDToPath(false);
            string filename = "";
            string cfilename = "";//2020-02-27
            string codestr = "";//2020-02-27
            for (int i = 0; i < outlineorgidsize; i++)
            {
                orgid = axEWdraw1.GetComposeOrgID(i);
                newfaceid = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                axEWdraw1.SetEntColor(newfaceid, axEWdraw1.RGBToIndex(0, 0, 0));
                filename = path + "p"+orgid.ToString() + ".bmp";
                axEWdraw1.SaveBMPWithRatio(filename, 128,axEWdraw1.RGBToIndex(255, 255, 255));
                imagefiles.Add(filename);//2020-02-28
                axEWdraw1.Delete(newfaceid);
                //
                cfilename = path + "c" + orgid.ToString() + ".bmp";
                if(orgid.ToString().Length<=5)
                    codestr = m_orderid + orgid.ToString("00000");
                else
                    codestr = m_orderid + orgid.ToString("0000000000");
                BarCode.Code128 _Code = new BarCode.Code128();
                _Code.ValueFont = new Font("宋体", 10);
                System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(codestr, BarCode.Code128.Encode.Code128A);
                imgTemp.Save(cfilename, System.Drawing.Imaging.ImageFormat.Bmp);
                imgTemp.Dispose();
                codefiles.Add(cfilename);//2020-02-28
                double width,height,thickness;
                width = height = thickness = 0.0;
                double area = 0;
                string name = "";
                string cname = "";
                string sizestr = "";
                axEWdraw1.GetPlankWHTA(orgid, ref width, ref height, ref thickness, ref area, ref name, ref cname);
                sizestr = "尺寸:" + width.ToString("0") + "*" + height.ToString("0") + "*" + thickness.ToString("0");
                planksizes.Add(sizestr);
                if(cname.Length>0)
                    cabinetnames.Add("名称:"+cname);
                else
                    cabinetnames.Add("名称:" + "柜子");
                //板子的类型
                string ggname = "";
                GetGGName(thickness,ref cname,ref ggname);
                matnames.Add("板质:"+cname);
                matcolors.Add("规格:"+ggname);
            }
            //恢复
            //原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.SetEntityInvisible((int)(g_alltxts[i]), false);
            }
            //原有的纹理面
            for (int i = 0; i < g_allfaces.Count; i++)
            {
                axEWdraw1.SetEntityInvisible((int)(g_allfaces[i]), false);
            }
            //原有的洞
            for (int i = 0; i < holesize; i++)
                axEWdraw1.SetEntityInvisible(axEWdraw1.GetComposeHole(i), false);
            //原有的槽
            for (int i = 0; i < groovesize; i++)
                axEWdraw1.SetEntityInvisible(axEWdraw1.GetComposeGroove(i), false);

        }

        private bool GetGGName(double t,ref string name,ref string ggname)
        {
            string tname = "";
            string tggname = "";
            double width = 0,height = 0,thickness = 0;
            int size = axEWdraw1.GetOrgPageSize();
            for (int i = 0; i < size; i++)
            {
                axEWdraw1.GetOrgPageWHT(i, ref width, ref height, ref thickness, ref tname, ref tggname);
                if (Math.Abs(thickness - t) < 0.001)
                {
                    name = tname;
                    ggname = tggname;
                    return true;
                }
            }
            name = "多层板";
            ggname = "默认";
            return false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // TODO: 在此添加控件通知处理程序代码
            if (zPubFun.zPubFunLib.g_istriallimit)
            {
                MessageBox.Show("试用版,功能受限,不能输出.");
                return;
            }
            if (!axEWdraw1.Visible)
                axEWdraw1.Visible = true;
            if (textBox1.Visible)
                textBox1.Visible = false;

            if ((vScrollBar1.Value - 1 == axEWdraw1.GetComposeMostPage() && zPubFun.zPubFunLib.g_istriallimit) || !zPubFun.zPubFunLib.g_istriallimit)
            {
                string filename = ConvertPrjIDToEWDPath("ewdraw_out.dxf",false);
                int outlinesize = axEWdraw1.GetComposeOutlineSize();

                for (int i = 0; i < outlinesize; i++)
                {
                    axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));
                }
                axEWdraw1.ExpFile(filename);
                axEWdraw1.ClearExportID();
                if (!System.IO.File.Exists(filename)) return;

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = " /select," + filename;
                System.Diagnostics.Process.Start(psi);
            }
            else MessageBox.Show("试用版,功能受限");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (zPubFun.zPubFunLib.g_istriallimit)//2021-01-29
            {
                if (axEWdraw1.Visible)
                    axEWdraw1.Visible = false;
                if (!textBox1.Visible)
                    textBox1.Visible = true;
                if (vScrollBar1.Value - 1 == axEWdraw1.GetComposeMostPage())
                {
                    string sstate;
                    sstate = button1.Text;
                    if (sstate == "查看反面")
                    {
                        textBox1.Text = System.IO.File.ReadAllText(m_ncfilename);
                        isViewGcode = true;
                    }
                    else
                    {
                        MessageBox.Show("试用版,功能受限");
                        if (!axEWdraw1.Visible)
                            axEWdraw1.Visible = true;
                        if (textBox1.Visible)
                            textBox1.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("试用版,功能受限");
                    if (!axEWdraw1.Visible)
                        axEWdraw1.Visible = true;
                    if (textBox1.Visible)
                        textBox1.Visible = false;

                }
            }
            else
            {
                if (axEWdraw1.Visible)
                    axEWdraw1.Visible = false;
                if (!textBox1.Visible)
                    textBox1.Visible = true;
                textBox1.Text = System.IO.File.ReadAllText(m_ncfilename);
                isViewGcode = true;
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form15 fm15 = new Form15();
            fm15.m_orderid = m_orderid;
            fm15.m_orderdate = m_orderdate;
            fm15.g_prjallitems = g_allitems;
            fm15.m_tplanks = m_tplanks;
            if (fm15.ShowDialog() == DialogResult.OK)
            {
                string filename = ConvertPrjIDToEWDPath("prjmats.xml");
                if (System.IO.File.Exists(filename))
                {
                    zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                    if (g_allitems != null)
                    {

                        if (g_allitems.Count > 0)
                        {
                            g_allitems.Clear();
                            g_allitems = null;
                        }
                    }
                    g_allitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                    m_needplanks.Clear();
                    IsCheckCaiLiao(ref g_allitems, ref m_tplanks, ref m_needplanks, isexistmatfile);
                    if (m_needplanks.Count > 0)
                    {
                        string str = "请点击\"选择物料\"按钮,选择板厚分别为:";
                        for (int i = 0; i < m_needplanks.Count; i++)
                        {
                            str += Convert.ToString(m_needplanks[i]);
                            if (i + 1 < m_needplanks.Count)
                                str += ",";
                        }
                        str += " 的板材物料";
                        label1.Text = str;
                        label1.ForeColor = Color.Red;
                    }
                    else
                    {
                        label1.Text = "";
                        label1.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (zPubFun.zPubFunLib.g_istriallimit)
            {
                MessageBox.Show("试用版,功能受限,不能全部输出.");
                return;
            }
            //删除原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.Delete((int)g_alltxts[i]);
            }
            g_alltxts.Clear();
            //删除原有的纹理面
            for (int i = 0; i < g_allfaces.Count; i++)
            {
                axEWdraw1.Delete((int)g_allfaces[i]);
            }
            g_allfaces.Clear();
            //删除原有的轮廓线
            int outlinesize = axEWdraw1.GetComposeOutlineSize();
            for (int j = 0; j < outlinesize; j++)
                axEWdraw1.Delete(axEWdraw1.GetComposeOutline(j));
            //删除原有的洞
            int holesize = axEWdraw1.GetComposeHoleSize();
            for (int j = 0; j < holesize; j++)
                axEWdraw1.Delete(axEWdraw1.GetComposeHole(j));
            //删除原有的槽
            int groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int j = 0; j < groovesize; j++)
                axEWdraw1.Delete(axEWdraw1.GetComposeGroove(j));
            
            int psize = axEWdraw1.GetComposePageSize();
            string filename = "";
            string pathfile = "";
            pathfile = ConvertPrjIDToEWDPath(filename, false);
            ArrayList ncfiles = new ArrayList();//2021-03-25
            string pathdir = System.IO.Path.GetDirectoryName(pathfile);
            GetAllNCFileDir(pathdir,ref ncfiles);//2021-03-25
            if (ncfiles.Count > 0)
            {
                string singlefile = "";
                string mydoc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                for (int i = 0; i < ncfiles.Count; i++)
                {
                    singlefile = (string)ncfiles[i];
                    int inx = singlefile.IndexOf(mydoc);
                    if (inx >= 0 && singlefile.Length > mydoc.Length)
                    {
                        File.Delete(singlefile);
                    }
                }
            }
            ArrayList tmpfiles = new ArrayList();
            for (int i = 0; i < psize; i++)
            {
                double t = axEWdraw1.GetComposePageThickness(i);
                string name = axEWdraw1.GetComposePageName(i);
                string colorname = axEWdraw1.GetComposePageSName(i);
                filename = (i+1).ToString()+"-A"+"-"+t.ToString("0.")+"-"+name+"-"+colorname+".nc";
                pathfile = ConvertPrjIDToEWDPath(filename,false);
                axEWdraw1.GetComposePage(i, true, pathfile);
                tmpfiles.Add(pathfile);
                //删除原有的轮廓线
                outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int j = 0; j < outlinesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeOutline(j));
                //删除原有的洞
                holesize = axEWdraw1.GetComposeHoleSize();
                for (int j = 0; j < holesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeHole(j));
                //删除原有的槽
                groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int j = 0; j < groovesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeGroove(j));

                filename = (i + 1).ToString() + "-B" + "-" + t.ToString("0.") + "-" + name + "-" + colorname + ".nc";
                pathfile = ConvertPrjIDToEWDPath(filename,false);
                axEWdraw1.GetComposePage(i, false, pathfile);
                tmpfiles.Add(pathfile);
                //删除原有的轮廓线
                outlinesize = axEWdraw1.GetComposeOutlineSize();
                for (int j = 0; j < outlinesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeOutline(j));
                //删除原有的洞
                holesize = axEWdraw1.GetComposeHoleSize();
                for (int j = 0; j < holesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeHole(j));
                //删除原有的槽
                groovesize = axEWdraw1.GetComposeGrooveSize();
                for (int j = 0; j < groovesize; j++)
                    axEWdraw1.Delete(axEWdraw1.GetComposeGroove(j));
                if (zPubFun.zPubFunLib.g_istriallimit)
                {
                    if (i > 2)
                    {
                        MessageBox.Show("试用版,功能受限,不能全部输出.");
                        break;
                    }
                }
            }
            //重绘
            int iPos = vScrollBar1.Value;
            string sss;
            sss = iPos.ToString();
            label1.Text = sss;
            //判断页数并创建排版对象 2018-06-14
            //删除原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.Delete((int)(g_alltxts[i]));
            }
            g_alltxts.Clear();
            //删除原有的纹理面
            for (int i = 0; i < g_allfaces.Count; i++)
            {
                axEWdraw1.Delete((int)(g_allfaces[i]));
            }
            g_allfaces.Clear();
            //删除原有的轮廓线
            outlinesize = axEWdraw1.GetComposeOutlineSize();
            for (int i = 0; i < outlinesize; i++)
            {
                axEWdraw1.Delete(axEWdraw1.GetComposeOutline(i));
            }
            //删除原有的洞
            holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
                axEWdraw1.Delete(axEWdraw1.GetComposeHole(i));
            //删除原有的槽
            groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
                axEWdraw1.Delete(axEWdraw1.GetComposeGroove(i));
            //
            axEWdraw1.GetComposePage(iPos - 1, true, m_ncfilename);
            axEWdraw1.GetComposePageWH(iPos - 1, ref m_curwidth, ref m_curheight);//2019-12-25
            //2018-07-05
            string str;
            str = "板厚:" + ((int)axEWdraw1.GetComposePageThickness(iPos - 1)).ToString("0.00") + "mm";
            int txt = axEWdraw1.Text3D(str, "宋体", new object[] { 900, 2400, 1 }, 32, 0, 0);
            axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
            g_allfaces.Add(txt);
            //
            axEWdraw1.AddOne3DVertex(0, 0, -10.0);
            axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);
            axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);
            axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);
            int allent = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            int faceent = axEWdraw1.EntToFace(allent, true);
            axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
            g_allfaces.Add(faceent);
            outlinesize = axEWdraw1.GetComposeOutlineSize();
            axEWdraw1.ClearExportID();//2018-06-28
            for (int i = 0; i < outlinesize; i++)
            {
                if (!axEWdraw1.IsPlankHoleOutline(i))
                {
                    faceent = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                    axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                    g_allfaces.Add(faceent);
                    string name = axEWdraw1.GetComposeOutlineName(i);
                    DrawText3D("", name, faceent, 32);//板材说明信息
                }
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));//2018-06-28
            }
            holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
            {
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeHole(i), axEWdraw1.RGBToIndex(255, 0, 0));
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeHole(i));//2018-06-28
            }
            //槽
            groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
            {
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeGroove(i), axEWdraw1.RGBToIndex(0, 255, 0));
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeGroove(i));//2018-06-28
            }
            axEWdraw1.ZoomALL();
            button1.Text = "查看反面";
            //
            for (int i = 0; i < tmpfiles.Count; i++)
            {
                if (System.IO.File.Exists((string)tmpfiles[i]))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = " /select," + tmpfiles[i];
                    System.Diagnostics.Process.Start(psi);
                    break;
                }
            }

        }

        private void MakePage(int iPos)
        {
            string sss;
            sss = iPos.ToString();
            label1.Text = sss;
            //判断页数并创建排版对象 2018-06-14
            //删除原有的文字
            for (int i = 0; i < g_alltxts.Count; i++)
            {
                axEWdraw1.Delete((int)(g_alltxts[i]));
            }
            g_alltxts.Clear();
            //删除原有的纹理面
            for (int i = 0; i < g_allfaces.Count; i++)
            {
                axEWdraw1.Delete((int)(g_allfaces[i]));
            }
            g_allfaces.Clear();
            //删除原有的轮廓线
            int outlinesize = axEWdraw1.GetComposeOutlineSize();
            for (int i = 0; i < outlinesize; i++)
            {
                axEWdraw1.Delete(axEWdraw1.GetComposeOutline(i));
            }
            //删除原有的洞
            int holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
                axEWdraw1.Delete(axEWdraw1.GetComposeHole(i));
            //删除原有的槽
            int groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
                axEWdraw1.Delete(axEWdraw1.GetComposeGroove(i));
            //
            axEWdraw1.GetComposePage(iPos - 1, true, m_ncfilename);
            axEWdraw1.GetComposePageWH(iPos - 1, ref m_curwidth, ref m_curheight);//2019-12-25
            //2018-07-05
            string str;
            str = "板厚:" + ((int)axEWdraw1.GetComposePageThickness(iPos - 1)).ToString("0.00") + "mm";
            int txt = axEWdraw1.Text3D(str, "宋体", new object[] { 900, 2400, 1 }, 32, 0, 0);
            axEWdraw1.SetEntWireOfShape(txt, true, 0, 1);
            g_allfaces.Add(txt);
            //
            axEWdraw1.AddOne3DVertex(0, 0, -10.0);
            axEWdraw1.AddOne3DVertex(m_curwidth, 0, -10.0);
            axEWdraw1.AddOne3DVertex(m_curwidth, m_curheight, -10.0);
            axEWdraw1.AddOne3DVertex(0, m_curheight, -10.0);
            int allent = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            int faceent = axEWdraw1.EntToFace(allent, true);
            axEWdraw1.SetEntTexture(faceent, "allplank.jpg", 1, 1, 1, 1, 0, 0);
            g_allfaces.Add(faceent);
            outlinesize = axEWdraw1.GetComposeOutlineSize();
            axEWdraw1.ClearExportID();//2018-06-28
            for (int i = 0; i < outlinesize; i++)
            {
                if (!axEWdraw1.IsPlankHoleOutline(i))
                {
                    faceent = axEWdraw1.EntToFace(axEWdraw1.GetComposeOutline(i), false);
                    axEWdraw1.SetEntTexture(faceent, "plank.jpg", 1, 1, 1, 1, 0, 0);
                    g_allfaces.Add(faceent);
                    string name = axEWdraw1.GetComposeOutlineName(i);
                    DrawText3D("", name, faceent, 32);//板材说明信息
                }
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeOutline(i));//2018-06-28

            }
            holesize = axEWdraw1.GetComposeHoleSize();
            for (int i = 0; i < holesize; i++)
            {
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeHole(i), axEWdraw1.RGBToIndex(255, 0, 0));
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeHole(i));//2018-06-28
            }
            //槽
            groovesize = axEWdraw1.GetComposeGrooveSize();
            for (int i = 0; i < groovesize; i++)
            {
                axEWdraw1.SetEntColor(axEWdraw1.GetComposeGroove(i), axEWdraw1.RGBToIndex(0, 255, 0));
                axEWdraw1.AddEntToExp(axEWdraw1.GetComposeGroove(i));//2018-06-28
            }
            axEWdraw1.ZoomALL();
            button1.Text = "查看反面";
        }

        public void MakeBQData()
        {
            int oldwidth = axEWdraw1.Width;
            int oldheight = axEWdraw1.Height;
            axEWdraw1.Width = oldheight / 2;
            imagefiles.Clear();//排版图文件列表
            codefiles.Clear();//条形码文件列表
            cabinetnames.Clear();//柜体名称列表
            matnames.Clear();//材质名称列表
            planksizes.Clear();//板材的大小列表
            matcolors.Clear();//材质名称列表

            //
            int oldinx = vScrollBar1.Value;
            int size = axEWdraw1.GetComposePageSize();
            for (int i = 1; i <= size; i++)
            {
                MakePage(i);
                GetBMPFile();
            }
            MakePage(oldinx);
            axEWdraw1.Width = oldwidth;
            axEWdraw1.Height = oldheight;
            axEWdraw1.ZoomALL();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //if (zPubFun.zPubFunLib.g_istriallimit)//2020-05-16
            //{
            //    MessageBox.Show("试用版不支持该功能");
            //    return;
            //}
            Form18 fm18 = new Form18();
            fm18.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (zPubFun.zPubFunLib.g_istriallimit)//2020-04-20
            {
                MessageBox.Show("试用版不支持该功能");
                return;
            }
            string path = ConvertPrjIDToEWDPath("Ban", false);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (System.IO.Directory.Exists(path))
            {
                int tmpjs = 0;
                int entsize = axEWdraw1.GetPlankSize();
                int ent = 0;
                string codestr = "";
                for (int i = 0; i < entsize; i++)
                {
                    ent = axEWdraw1.GetPlankID(i);
                    if (ent > 0)
                    {
                        if (ent.ToString().Length <= 5)
                            codestr = path + "\\" + m_orderid + ent.ToString("00000") + ".ban";
                        else
                            codestr = path + "\\" + m_orderid + ent.ToString("0000000000") + ".ban";
                        axEWdraw1.MakePlankBanFile(ent, codestr, 6, 0, 1, 1,0.5);
                        tmpjs++;
                        if(zPubFun.zPubFunLib.g_istriallimit)
                        {
                            if (tmpjs > 3)
                            {
                                MessageBox.Show("试用版,功能受限,不能全部输出.");
                                break;
                            }
                        }
                    }
                }
                System.Diagnostics.Process.Start(path);
            }
        }
        //设置刀具 2020-04-28
        private bool SetTInfo()
        {
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allT.xml";
            List<zDaoJu.DaoJuEntity> allitems = null;
            zDaoJu.DaoJuEnter.getInst().DbFile = filename;
            allitems = zDaoJu.DaoJuEnter.getInst().SelectAll();
            //
            if (allitems.Count > 0)
            {
                axEWdraw1.ClearGT();
                for (int i = 0; i < allitems.Count; i++)
                {
                    if (allitems[i].Begin.Length > 0)
                    {
                        if(allitems[i].Begin.IndexOf("\r")<0)
                            allitems[i].Begin = allitems[i].Begin.Replace("\n", "\r\n");
                    }
                    if (allitems[i].End.Length > 0)
                    {
                        if (allitems[i].End.IndexOf("\r") < 0)
                            allitems[i].End = allitems[i].End.Replace("\n", "\r\n");
                    }

                    if(i == 0)
                        axEWdraw1.AddGT(zPubFun.zPubFunLib.CStr2Int(allitems[i].Id.Substring(1)),
                                        zPubFun.zPubFunLib.CStr2Double(allitems[i].Dia),
                                        zPubFun.zPubFunLib.CStr2Int(allitems[i].S),
                                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F),
                                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F1),
                                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F2),
                                        zPubFun.zPubFunLib.CStr2Int(allitems[i].Up),
                                        allitems[i].Begin,
                                        allitems[i].End, 1);
                    else
                        axEWdraw1.AddGT(zPubFun.zPubFunLib.CStr2Int(allitems[i].Id.Substring(1)),
                        zPubFun.zPubFunLib.CStr2Double(allitems[i].Dia),
                        zPubFun.zPubFunLib.CStr2Int(allitems[i].S),
                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F),
                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F1),
                        zPubFun.zPubFunLib.CStr2Int(allitems[i].F2),
                        zPubFun.zPubFunLib.CStr2Int(allitems[i].Up),
                        allitems[i].Begin,
                        allitems[i].End, 0);
                }
                return true;
            }
            //
            return false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form20 daoju = new Form20();
            daoju.ShowDialog();
            SetTInfo();
        }

    }
    
}
