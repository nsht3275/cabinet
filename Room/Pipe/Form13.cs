using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Drawing.Imaging;
using Pipe;
using RDLCPrinter;
namespace Room
{
    public partial class Form13 : Form
    {
        public List<zCaiLiao.CaiLiaoEntity> g_allitems = null;
        public List<zCaiLiao.CaiLiaoEntity> g_allorgitems = null;
        public List<zCaiLiao.CaiLiaoEntity> g_allwjitems = null;
        public ArrayList m_grps = new ArrayList();
        public ArrayList m_imgs = new ArrayList();
        public ArrayList m_planks = new ArrayList();
        public ArrayList m_tplanks = new ArrayList();
        public ArrayList m_needplanks = new ArrayList();
        public ArrayList m_cabinetnames = new ArrayList();
        public ArrayList m_imagefiles = new ArrayList();
        public ArrayList m_calwjs = new ArrayList();
        public ArrayList m_wjids = new ArrayList();
        public ArrayList m_qhplanks = new ArrayList();//缺货的板材
        DataTable dt = null;//2020-01-22
        public string m_orderid = "";//订单号
        public string m_orderdate = "";//订单日期
        public string m_orderoutdate = "";//交货日期
        public string m_orderconnecter = "";//联系人
        public string m_orderphone = "";//联系电话
        public string m_ordermemo = "";//订单备注
        public bool isexistmatfile = false;
        public Form13()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            //读入五金
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allwjs.xml";
            if (System.IO.File.Exists(filename))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                if (g_allwjitems != null)
                {
                    if (g_allwjitems.Count > 0)
                    {
                        g_allwjitems.Clear();
                        g_allwjitems = null;
                    }
                }
                g_allwjitems = new List<zCaiLiao.CaiLiaoEntity>(zCaiLiao.CaiLiaoEnter.getInst().SelectAll().ToArray());
            }
            //创建一个默认的值
            string filename1 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\allmats.xml";
            if (System.IO.File.Exists(filename1))
            {
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename1;
                g_allorgitems = new List<zCaiLiao.CaiLiaoEntity>(zCaiLiao.CaiLiaoEnter.getInst().SelectAll().ToArray());
            }
            else
            {
                MessageBox.Show("没有所需的物料,请添加物料后重试!");
                this.Close();
            }
            

            //读入板材物料
            filename = ConvertPrjIDToEWDPath("prjmats.xml",false);
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
                for (int j = 0; j < g_allitems.Count; j++)//选择板材
                {
                    bool isfind = false;
                    for (int i = 0; i < g_allorgitems.Count; i++)//当前设定
                    {
                        if (g_allitems[j].Name.Trim() == g_allorgitems[i].Name.Trim() &&
                             g_allitems[j].IsCheck
                            )
                        {
                            if (g_allitems[j].Color.Trim() == g_allorgitems[i].Color.Trim())
                            {
                                g_allitems[j].Unit = g_allorgitems[i].Unit;
                                g_allitems[j].Color = g_allorgitems[i].Color;
                                isfind = true;
                                break;
                            }
                        }
                    }
                    if (!isfind)//没找到板材 2020-01-22
                    {
                        m_needplanks.Add(zPubFun.zPubFunLib.CStr2Double(g_allitems[j].Thickness));
                    }
                }
            }
            else
            {
                //复制原有的信息 2020-01-22
                zCaiLiao.CaiLiaoEnter.getInst().DbFile = filename;
                List<zCaiLiao.CaiLiaoEntity> tmpallitems = zCaiLiao.CaiLiaoEnter.getInst().SelectAll();
                if (tmpallitems.Count == 0)
                {
                    for (int i = 0; i < g_allorgitems.Count; i++)
                    {
                        tmpallitems.Add(g_allorgitems[i].Clone() as zCaiLiao.CaiLiaoEntity);
                    }
                }
                zCaiLiao.CaiLiaoEnter.getInst().WriteDb();
                zCaiLiao.CaiLiaoEnter.getInst().DeleteAllBeforeWrite();
                //复制 2020-01-22
                if (g_allitems!=null)
                    g_allitems = null;
                g_allitems = new List<zCaiLiao.CaiLiaoEntity>(g_allorgitems.ToArray());
            }
        }

        //获得五金的价格
        private double GetWjPrice(string name,ref List<zCaiLiao.CaiLiaoEntity> list,double length = 0.0)
        {
            if (name == "衣通" || name == "衣架" || name == "挂衣杆")
            {
                /*
                 * 50CM以内 6.5元
                    51-80CM 11.5元
                    81-100CM 16.5元
                    101-120CM 21.5元
                    121-140CM 26.5元
                    141-160CM 36.5元
                 */
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Name == name)//未来可以再加上种类Color的判断
                    {
                        double tmpprice = GetFDPrice(list[i].Memo, length/10.0, zPubFun.zPubFunLib.CStr2Double(list[i].Unit));
                        if (tmpprice > 0.001)
                            return tmpprice;
                    }
                }
                
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Name == name)
                    {
                        return zPubFun.zPubFunLib.CStr2Double(list[i].Unit);
                    }
                }
            }
            return 0.0;
        }

        //取得分段价格,格式例子为 >50 & <80等
        public double GetFDPrice(string memo,double length,double unit)
        {
            if (memo.Length > 0)
            {
                int st = -1;
                int et = -1;
                int at = -1;
                double sv = 0.0;
                double ev = 0.0;
                bool ishavest = false;
                bool ishaveet = false;
                st = memo.IndexOf(">=");

                if (st < 0)
                    st = memo.IndexOf(">");
                else
                {
                    st++;
                    ishavest = true;
                }

                et = memo.IndexOf("<=");
                if (et < 0)
                    et = memo.IndexOf("<");
                else
                {
                    et++;
                    ishaveet = true;
                }
                if (st >= 0 && et >= 0)
                {
                    at = memo.IndexOf("&");
                    sv = zPubFun.zPubFunLib.CStr2Double(memo.Substring(st + 1, at - st-1));
                    ev = zPubFun.zPubFunLib.CStr2Double(memo.Substring(et+1, memo.Length - et-1));

                    if (ishavest)
                    {
                        if (length > sv || Math.Abs(length - sv) < 0.001)
                        {
                            if (ishaveet)
                            {
                                if (length < ev || Math.Abs(length - ev) < 0.001)
                                {
                                    return zPubFun.zPubFunLib.CStr2Double(unit);
                                }
                            }
                            else
                            {
                                if (length < ev)
                                {
                                    return zPubFun.zPubFunLib.CStr2Double(unit);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (length > sv)
                        {
                            if (ishaveet)
                            {
                                if (length < ev || Math.Abs(length - ev) < 0.001)
                                {
                                    return zPubFun.zPubFunLib.CStr2Double(unit);
                                }
                            }
                            else
                            {
                                if (length < ev)
                                {
                                    return zPubFun.zPubFunLib.CStr2Double(unit);
                                }
                            }
                        }
                    }
                }
                else if (et >= 0)
                {
                    at = memo.IndexOf("&");
                    ev = zPubFun.zPubFunLib.CStr2Double(memo.Substring(et + 1, memo.Length - et - 1));

                    if (ishaveet)
                    {
                        if (length < ev || Math.Abs(length - ev) < 0.001)
                        {
                            return zPubFun.zPubFunLib.CStr2Double(unit);
                        }
                    }
                    else
                    {
                        if (length < ev)
                        {
                            return zPubFun.zPubFunLib.CStr2Double(unit);
                        }
                    }
                }
                else if (st >= 0)
                {
                    at = memo.IndexOf("&");
                    if(at>=0)
                        sv = zPubFun.zPubFunLib.CStr2Double(memo.Substring(st + 1, at - st - 1));
                    else
                        sv = zPubFun.zPubFunLib.CStr2Double(memo.Substring(st + 1, memo.Length - st - 1));
                    if (ishavest)
                    {
                        if (length > sv || Math.Abs(length - ev) < 0.001)
                        {
                            return zPubFun.zPubFunLib.CStr2Double(unit);
                        }
                    }
                    else
                    {
                        if (length > sv)
                        {
                            return zPubFun.zPubFunLib.CStr2Double(unit);
                        }
                    }
                }
            }//
            return 0.0;
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
        /*GetProStrFromEnt从实体中得到某段名的字符串
         * 参数:
         * ent   输入 实体的ID号
         * filed 输入 段名
         * 返回值:
         * 如果成功,则返回该段值内的字符数值.其它返回空字符串.
         */
        private string GetProStrFromEnt(int ent, string field)
        {
            string prostr = "";
            if (ent > 0)
            {
                string str = Form1.g_form1.axEWdraw3.GetEntityUserData(ent);
                if (str.Length > 0)
                {
                    string tfieldname = field + ":";
                    int tlen = tfieldname.Length;
                    int ffinx = IsHaveStrField(str, tfieldname);//段开始的位置
                    //判断是一段,而不是一部分 2017-09-21
                    if (ffinx >= 0)
                    {
                        int feinx = str.IndexOf(";", ffinx);//段结束的位置
                        if (ffinx >= 0 && feinx >= 0)
                        {
                            prostr = str.Substring(ffinx + tlen, feinx - ffinx - tlen);
                        }
                    }
                }
            }
            //返回属性值字符串:如width:128.0;height:2700.0;...取width时,则返回128.0
            return prostr;
        }
        private double GetDblfromProStr(string str)
        {
            if (str.Length > 0)
            {
                int finx = str.IndexOf(',');//排除用逗号隔开的数组形式
                if (finx < 0)
                    return zPubFun.zPubFunLib.CStr2Double(str);
            }
            return 0.0;
        }

        private void AddWj(ref CWJTClass awj,double length = 0.0)
        {
            bool isfind = false;
            for(int i = 0;i < m_calwjs.Count;i++)
            {
                if (((CWJTClass)m_calwjs[i]).name == awj.name)
                {
                    isfind = true;
                    if (((CWJTClass)m_calwjs[i]).name == "衣通")
                    {
                        ((CWJTClass)m_calwjs[i]).length += length;
                    }
                    ((CWJTClass)m_calwjs[i]).num++;
                    break;
                }
            }
            if (!isfind)
            {
                if (awj.name == "衣通")
                {
                    awj.length += length;
                }
                awj.num = 1;
                m_calwjs.Add(awj);
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

        //板材厚度分类
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
        /*
         * 判断是否存该板材的规格 2019-12-19
         */
        private bool IsCheckCaiLiao(ref List<zCaiLiao.CaiLiaoEntity> list,ref ArrayList orglist,ref ArrayList outlist,bool isexist)
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
                if(!isexist)
                {//如果不存在项目的材料对象
                    for (int i = 0; i < orglist.Count; i++)
                    {
                        bool isfind = true;
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (Math.Abs(((CPlankTClass)orglist[i]).thickness - zPubFun.zPubFunLib.CStr2Double(list[j].Thickness)) < 0.001)
                            {
                                isfind = true;
                                list[j].IsCheck = true;
                                outlist.Add(((CPlankTClass)orglist[i]).thickness);
                                break;
                            }
                        }
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
                                if (Math.Abs(((CPlankTClass)orglist[i]).thickness - zPubFun.zPubFunLib.CStr2Double(list[j].Thickness)) < 0.001)
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
                                    if (Math.Abs(((CPlankTClass)orglist[i]).thickness - zPubFun.zPubFunLib.CStr2Double(list[j].Thickness)) < 0.001)
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

        private string GetCabinetName(ref ArrayList list,string name)
        {
            int result = 1;
            for (int i = 0; i < list.Count; i++)
            {
                if (((string)list[i]) == name)
                {
                    result++;
                }
            }
            if (result >= 1)
                list.Add(name);
            return result.ToString("000");
        }
        //获取单位价格
        private double GetUnitPrice(ref List<zCaiLiao.CaiLiaoEntity> list,double t)
        {
            bool isfind = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsCheck)
                {
                    if (Math.Abs(zPubFun.zPubFunLib.CStr2Double(list[i].Thickness) - t) < 0.001)
                    {
                        isfind = true;
                        return zPubFun.zPubFunLib.CStr2Double(list[i].Unit);
                    }
                }
            }
            if (!isfind)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].IsCheck)
                    {
                        if (Math.Abs(zPubFun.zPubFunLib.CStr2Double(list[i].Thickness) - t) < 0.001)
                        {
                            isfind = true;
                            return zPubFun.zPubFunLib.CStr2Double(list[i].Unit);
                        }
                    }
                }
            }
            return 0;
        }
        //获取颜色规格
        private string GetColor(ref List<zCaiLiao.CaiLiaoEntity> list, double t)
        {
            bool isfind = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsCheck)
                {
                    if (Math.Abs(zPubFun.zPubFunLib.CStr2Double(list[i].Thickness) - t) < 0.001)
                    {
                        isfind = true;
                        return list[i].Color;
                    }
                }
            }
            if (!isfind)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].IsCheck)
                    {
                        if (Math.Abs(zPubFun.zPubFunLib.CStr2Double(list[i].Thickness) - t) < 0.001)
                        {
                            isfind = true;
                            return list[i].Color;;
                        }
                    }
                }
            }
            return "";
        }

        private byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
        private string SeekImageFile(string str,ref ArrayList list)
        {
            string imgfile = str;
            string filename = "";
            if (str.Length > 0)
                filename = ConvertPrjIDToEWDPath(str + ".bmp", false);
            else
            {
                return "";
            }
            for (int i = 0; i < list.Count; i++)
            {
                if ((string)list[i] == filename)
                {
                    if (System.IO.File.Exists((string)list[i]))
                        return (string)list[i];
                }
            }
            return "";
        }
        //创建报表
        private void MakeReport()
        {
            string cname = "";
            string gname = "";//2020-02-27
            string oldcname = "";
            string onecname = "";
            this.rptView.Clear();

            this.rptView.LocalReport.DataSources.Clear();
            //清空
            m_planks.Clear();
            m_cabinetnames.Clear();
            m_tplanks.Clear();
            m_imagefiles.Clear();
            m_grps.Clear();
            m_wjids.Clear();
            if (m_imgs.Count > 0)
            {
                for (int i = 0; i < m_imgs.Count; i++)
                {
                    if (m_imgs[i] != null)
                    {
                        ((Bitmap)m_imgs[i]).Dispose();
                    }
                }
            }
            m_imgs.Clear();
            m_calwjs.Clear();
            if (dt != null)
                dt = null;
            //
            //2019-12-24 计算五金
            Form1.g_form1.axEWdraw3.ReCalWj();
            //
            int entsize = Form1.g_form1.axEWdraw3.GetEntSize();
            int ent = 0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = Form1.g_form1.axEWdraw3.GetEntID(i);
                if (ent > 0)
                {
                    if (Form1.g_form1.axEWdraw3.IsGroup(ent))
                    {
                        if (!Form1.g_form1.axEWdraw3.IsDisplayed(ent))
                            Form1.g_form1.axEWdraw3.SetEntityInvisible(ent, false);
                        if (Form1.g_form1.axEWdraw3.GetGroupAllIDs(ent))
                        {
                            int grpentsize = Form1.g_form1.axEWdraw3.GetIDBufferSize();
                            string grpname = Form1.g_form1.axEWdraw3.GetGroupName(ent);
                            string csname = "";
                            if (grpentsize > 0)
                            {
                                CMyGrps agrp = new CMyGrps();

                                for (int j = 0; j < grpentsize; j++)
                                {
                                    if (j == 0)
                                    {
                                        csname = Form1.g_form1.axEWdraw3.GetEntName(Form1.g_form1.axEWdraw3.GetIDBuffer(j));
                                    }
                                    agrp.m_ids.Add(Form1.g_form1.axEWdraw3.GetIDBuffer(j));
                                }
                                agrp.m_name = grpname;
                                if (csname.Length > 0)
                                    agrp.m_csname = csname;
                                m_grps.Add(agrp);
                            }
                            Form1.g_form1.axEWdraw3.ClearIDBuffer();
                            //判断是否为抽屉 2020-01-21
                            if (grpname.IndexOf("_ct_") >= 0)
                            {
                                CWJTClass atclass = new CWJTClass();
                                atclass.id = ent;
                                atclass.name = "滑轨";
                                AddWj(ref atclass);
                                AddWj(ref atclass);
                            }
                            else if (grpname.IndexOf("_yt_") >= 0)
                            {
                                CWJTClass atclass = new CWJTClass();
                                atclass.id = ent;
                                atclass.name = "衣通";
                                
                                double length = 0.0;
                                length = GetDblfromProStr(GetProStrFromEnt(ent, "length"));
                                double dj = GetWjPrice(atclass.name, ref g_allwjitems,length);
                                atclass.total += dj;//2020-02-17
                                if (length > 0.01)
                                    AddWj(ref atclass, length);
                            }
                        }
                    }
                    else
                    {
                        int type = Form1.g_form1.axEWdraw3.GetEntType(ent);
                        if (type == 501 || type == 502 || type == 503)/*三合一*//*木楔*//*二合一*/
                        {
                            CWJTClass atclass = new CWJTClass();
                            atclass.id = ent;
                            if (type == 501)
                                atclass.name = "三合一";
                            else if (type == 502)
                                atclass.name = "木楔";
                            else if (type == 503)
                                atclass.name = "二合一";
                            AddWj(ref atclass);
                            m_wjids.Add(ent);
                            Form1.g_form1.axEWdraw3.SetEntityInvisible(ent, true);//2020-02-20
                        }
                        else if (type == 55)
                        {
                            CWJTClass atclass = new CWJTClass();
                            atclass.id = ent;
                            atclass.name = "衣通";
                            double length = 0.0;
                            length = GetDblfromProStr(GetProStrFromEnt(ent, "length"));
                            if(length>0.01)
                                AddWj(ref atclass,length);
                        }
                    }
                }
            }

            if (m_grps.Count > 0)
            {
                //生成图片
                for (int i = 0; i < m_grps.Count; i++)
                {
                    for (int j = 0; j < m_grps.Count; j++)
                    {
                        if (i != j)
                        {
                            ent = (int)(((CMyGrps)m_grps[j]).m_ids[0]);
                            Form1.g_form1.axEWdraw3.SetEntityInvisible(ent, true);
                        }
                        else
                        {
                            ent = (int)(((CMyGrps)m_grps[j]).m_ids[0]);
                            Form1.g_form1.axEWdraw3.SetEntityInvisible(ent, false);
                        }
                    }
                    Form1.g_form1.axEWdraw3.DisplayTrihedron(false);
                    Form1.g_form1.axEWdraw3.SetViewCondition(5);
                    Form1.g_form1.axEWdraw3.SetGridOn(false);
                    string filename = "";
                    if (((CMyGrps)m_grps[i]).m_name.Length > 0)
                    {
                        filename = ConvertPrjIDToEWDPath(((CMyGrps)m_grps[i]).m_name +"_"+i.ToString()+".bmp", false);
                        ((CMyGrps)m_grps[i]).m_imagefile = ((CMyGrps)m_grps[i]).m_name + "_" + i.ToString() + ".bmp";
                    }
                    else
                    {
                        string tmpstr = "grp_" + i.ToString();
                        filename = ConvertPrjIDToEWDPath(tmpstr + ".bmp", false);
                    }
                    Form1.g_form1.axEWdraw3.SaveViewToBMP(filename, 128, 128, Form1.g_form1.axEWdraw3.RGBToIndex(255, 255, 255));
                    m_imagefiles.Add(filename);
                    Form1.g_form1.axEWdraw3.DisplayTrihedron(true);
                    Form1.g_form1.axEWdraw3.SetViewCondition(8);
                    Form1.g_form1.axEWdraw3.SetGridOn(true);
                }
                //
                for (int i = 0; i < m_grps.Count; i++)
                {
                    ent = ((int)((CMyGrps)m_grps[i]).m_ids[0]);
                    if (ent > 0)
                    {
                        if (Form1.g_form1.axEWdraw3.IsGroup(ent))
                        {
                            if (!Form1.g_form1.axEWdraw3.IsDisplayed(ent))
                                Form1.g_form1.axEWdraw3.SetEntityInvisible(ent, false);
                        }
                    }
                }
                //得到板材具体的信息
                double w, h, t, a;
                w = h = t = a = 0.0;

                for (int i = 0; i < m_grps.Count; i++)
                {
                    if (((CMyGrps)m_grps[i]).m_csname.Length > 0)//2020-03-26 2019-12-23  && oldcname != ((CMyGrps)m_grps[i]).m_csname
                    {
                        oldcname = ((CMyGrps)m_grps[i]).m_csname;
                        onecname = ((CMyGrps)m_grps[i]).m_csname + GetCabinetName(ref m_cabinetnames, ((CMyGrps)m_grps[i]).m_csname);
                    }

                    for (int j = 0; j < ((CMyGrps)m_grps[i]).m_ids.Count; j++)
                    {
                        if (Form1.g_form1.axEWdraw3.GetPlankWHTA(((int)((CMyGrps)m_grps[i]).m_ids[j]), ref w, ref h, ref t, ref a, ref cname,ref gname))
                        {
                            CReportPlank aplk = new CReportPlank();
                            aplk.tcode = ((CMyGrps)m_grps[i]).m_name;
                            aplk.gtmc = onecname;
                            aplk.mokuai = cname;
                            aplk.mingcheng = cname;
                            if (cname.IndexOf("效果") >= 0)
                                continue;
                            if (cname.Trim().Length < 1)
                            {
                                int tmptype = Form1.g_form1.axEWdraw3.GetPlankType(((int)((CMyGrps)m_grps[i]).m_ids[j]));
                                switch (tmptype)
                                {
                                    case 0:
                                        aplk.mokuai = "横板";
                                        aplk.mingcheng = "横板";
                                        break;
                                    case 1:
                                        aplk.mokuai = "竖板";
                                        aplk.mingcheng = "竖板";
                                        break;
                                    case 3:
                                        aplk.mokuai = "竖板";
                                        aplk.mingcheng = "面板";
                                        break;
                                }
                            }

                            aplk.chang = zPubFun.zPubFunLib.CStr2Double(w).ToString("0.");
                            aplk.kuan = zPubFun.zPubFunLib.CStr2Double(h).ToString("0.");
                            aplk.hou = zPubFun.zPubFunLib.CStr2Double(t).ToString("0."); ;//厚
                            aplk.yanse = GetColor(ref g_allitems, zPubFun.zPubFunLib.CStr2Double(aplk.hou));//颜色
                            aplk.shuliang = zPubFun.zPubFunLib.CStr2Double(w * h / 1000.0 / 1000.0).ToString("0.00");//数量 暂时为1
                            aplk.danwei = "m2";
                            aplk.danjia = GetUnitPrice(ref g_allitems, zPubFun.zPubFunLib.CStr2Double(aplk.hou)).ToString("0.0"); //zPubFun.zPubFunLib.CStr2Double(0.0).ToString("0.00");//单价
                            aplk.xiaoji = (zPubFun.zPubFunLib.CStr2Double(aplk.danjia) * zPubFun.zPubFunLib.CStr2Double(aplk.shuliang)).ToString("0.00");//小计
                            aplk.imagefile = ((CMyGrps)m_grps[i]).m_imagefile;//2020-04-14
                            m_planks.Add(aplk);
                            //根据厚度判断板类
                            CPlankTClass aptclass = new CPlankTClass();
                            aptclass.thickness = zPubFun.zPubFunLib.CStr2Double(aplk.hou);
                            AddTPlank(ref aptclass);
                        }
                        else
                        {
                        }
                    }
                }
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
            //写入报表内容
            //第二步：指定报表路径
            this.rptView.LocalReport.ReportPath = "Report1.rdlc";
            //第三步：构造新的DataTable
            if(dt != null)
            {
                dt.Clear();
                dt = null;
            }
            dt = new DataTable("DataTable1");
            dt.Columns.Add("GTMC");
            dt.Columns.Add("MOKUAI");
            dt.Columns.Add("MINGCHENG");
            dt.Columns.Add("YANSE");
            dt.Columns.Add("CHANG");
            dt.Columns.Add("KUAN");
            dt.Columns.Add("HOU");
            dt.Columns.Add("SHULIANG");
            dt.Columns.Add("DANWEI");
            dt.Columns.Add("DANJIA");
            dt.Columns.Add("XIAOJI");
            dt.Columns.Add("IMAGE");
            //
            
            double hj1 = 0.0;
            double hj2 = 0.0;
            oldcname = ((CReportPlank)m_planks[0]).gtmc;
            int js = 0;
            int js1 = 0;
            for (int i = 0; i < m_planks.Count; i++)
            {
                
                if (oldcname != ((CReportPlank)m_planks[i]).gtmc || (i + 1) == m_planks.Count)
                {
                    if((i + 1) == m_planks.Count)
                    {
                        dt.Rows.Add(new object[] {"名称:"+((CReportPlank)m_planks[i]).gtmc,
                            ((CReportPlank)m_planks[i]).mokuai,
                            ((CReportPlank)m_planks[i]).mingcheng,
                            ((CReportPlank)m_planks[i]).yanse,
                            ((CReportPlank)m_planks[i]).chang,
                            ((CReportPlank)m_planks[i]).kuan,
                            ((CReportPlank)m_planks[i]).hou,
                            ((CReportPlank)m_planks[i]).shuliang,
                            ((CReportPlank)m_planks[i]).danwei,
                            ((CReportPlank)m_planks[i]).danjia,
                            ((CReportPlank)m_planks[i]).xiaoji,
                            ""
                            });
                        hj1 += zPubFun.zPubFunLib.CStr2Double(((CReportPlank)m_planks[i]).xiaoji);
                    }
                    //合计
                    dt.Rows.Add(new object[] {"名称:"+oldcname,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "小计",
                                                zPubFun.zPubFunLib.CStr2Double(hj1),
                                                ""
                                                });
                    hj2 += hj1;
                    hj1 = 0.0;
                    oldcname = ((CReportPlank)m_planks[i]).gtmc;

                    js1 = 0;
                    js++;
                    if (js <= m_imagefiles.Count)
                    {
                        string tmpfile = "";
                        if (((CReportPlank)m_planks[i]).imagefile.Length > 0)
                            tmpfile = ConvertPrjIDToEWDPath(((CReportPlank)m_planks[i]).imagefile, false);
                        else
                            tmpfile = SeekImageFile(((CReportPlank)m_planks[i]).tcode, ref m_imagefiles);
                        if(tmpfile.Length>0)
                        {
                            if ((i + 1) != m_planks.Count)
                            {
                                Bitmap img = new Bitmap(tmpfile); //(string)m_imagefiles[js])
                                byte[] buffer = BitmapToBytes(img);
                                dt.Rows.Add(new object[] {"名称:"+((CReportPlank)m_planks[i]).gtmc,
                                                    ((CReportPlank)m_planks[i]).mokuai,
                                                    ((CReportPlank)m_planks[i]).mingcheng,
                                                    ((CReportPlank)m_planks[i]).yanse,
                                                    ((CReportPlank)m_planks[i]).chang,
                                                    ((CReportPlank)m_planks[i]).kuan,
                                                    ((CReportPlank)m_planks[i]).hou,
                                                    ((CReportPlank)m_planks[i]).shuliang,
                                                    ((CReportPlank)m_planks[i]).danwei,
                                                    ((CReportPlank)m_planks[i]).danjia,
                                                    ((CReportPlank)m_planks[i]).xiaoji,
                                                    Convert.ToBase64String(buffer)
                                                    });
                                hj1 += zPubFun.zPubFunLib.CStr2Double(((CReportPlank)m_planks[i]).xiaoji);
                                img.Dispose();
                            }
                        }
                    }

                }
                else
                {
                    hj1 += zPubFun.zPubFunLib.CStr2Double(((CReportPlank)m_planks[i]).xiaoji);

                    if (js1 == 0)
                    {
                        string tmpfile = "";
                        if (((CReportPlank)m_planks[i]).imagefile.Length > 0)
                            tmpfile = ConvertPrjIDToEWDPath(((CReportPlank)m_planks[i]).imagefile, false);
                        else
                            tmpfile = SeekImageFile(((CReportPlank)m_planks[i]).tcode, ref m_imagefiles);

                        if(tmpfile.Length>0)
                        {
                            Bitmap img = new Bitmap(tmpfile); 
                            byte[] buffer = BitmapToBytes(img);
                            dt.Rows.Add(new object[] {"名称:"+((CReportPlank)m_planks[i]).gtmc,
                                                    ((CReportPlank)m_planks[i]).mokuai,
                                                    ((CReportPlank)m_planks[i]).mingcheng,
                                                    ((CReportPlank)m_planks[i]).yanse,
                                                    ((CReportPlank)m_planks[i]).chang,
                                                    ((CReportPlank)m_planks[i]).kuan,
                                                    ((CReportPlank)m_planks[i]).hou,
                                                    ((CReportPlank)m_planks[i]).shuliang,
                                                    ((CReportPlank)m_planks[i]).danwei,
                                                    ((CReportPlank)m_planks[i]).danjia,
                                                    ((CReportPlank)m_planks[i]).xiaoji,
                                                    Convert.ToBase64String(buffer)
                                                    });//Convert.ToBase64String(buffer)
                            img.Dispose();
                        }
                        else
                        {
                            dt.Rows.Add(new object[] {"名称:"+((CReportPlank)m_planks[i]).gtmc,
                            ((CReportPlank)m_planks[i]).mokuai,
                            ((CReportPlank)m_planks[i]).mingcheng,
                            ((CReportPlank)m_planks[i]).yanse,
                            ((CReportPlank)m_planks[i]).chang,
                            ((CReportPlank)m_planks[i]).kuan,
                            ((CReportPlank)m_planks[i]).hou,
                            ((CReportPlank)m_planks[i]).shuliang,
                            ((CReportPlank)m_planks[i]).danwei,
                            ((CReportPlank)m_planks[i]).danjia,
                            ((CReportPlank)m_planks[i]).xiaoji,
                            ""
                            });//
                        }
                    }
                    else
                        dt.Rows.Add(new object[] {"名称:"+((CReportPlank)m_planks[i]).gtmc,
                                                ((CReportPlank)m_planks[i]).mokuai,
                                                ((CReportPlank)m_planks[i]).mingcheng,
                                                ((CReportPlank)m_planks[i]).yanse,
                                                ((CReportPlank)m_planks[i]).chang,
                                                ((CReportPlank)m_planks[i]).kuan,
                                                ((CReportPlank)m_planks[i]).hou,
                                                ((CReportPlank)m_planks[i]).shuliang,
                                                ((CReportPlank)m_planks[i]).danwei,
                                                ((CReportPlank)m_planks[i]).danjia,
                                                ((CReportPlank)m_planks[i]).xiaoji,
                                                ""
                                                });//

                }
                js1++;
            }
            //五金
            hj1 = 0.0;
            if (m_calwjs.Count > 0)
            {
                for (int i = 0; i < m_calwjs.Count; i++)
                {
                    double dj = GetWjPrice(((CWJTClass)m_calwjs[i]).name, ref g_allwjitems, ((CWJTClass)m_calwjs[i]).length);
                    if (((CWJTClass)m_calwjs[i]).name == "衣通")
                    {
                        dt.Rows.Add(new object[] {"五金",
                                                ((CWJTClass)m_calwjs[i]).name,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                ((CWJTClass)m_calwjs[i]).num.ToString(),
                                                "",
                                                dj.ToString("0.0"),
                                                ((CWJTClass)m_calwjs[i]).total.ToString("0.00"),
                                                ""
                                                });

                        hj1 += ((CWJTClass)m_calwjs[i]).total;//
                    }
                    else
                    {
                        dt.Rows.Add(new object[] {"五金",
                                                ((CWJTClass)m_calwjs[i]).name,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                ((CWJTClass)m_calwjs[i]).num.ToString(),
                                                "",
                                                dj.ToString("0.0"),
                                                (dj*((CWJTClass)m_calwjs[i]).num).ToString("0.00"),
                                                ""
                                                });
                        hj1 += dj * ((CWJTClass)m_calwjs[i]).num;
                    }

                }
            }
            dt.Rows.Add(new object[] {"五金",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "小计",
                                        hj1.ToString("0.00"),
                                        ""
                                        });
            hj2 += hj1;
            //汇总
            dt.Rows.Add(new object[] {"五金",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "汇总",
                                                zPubFun.zPubFunLib.CStr2Double(hj2),
                                                ""
                                                });
            //名称不能写错，和报表中的数据集名称一致
            ReportDataSource rdsItem = new ReportDataSource("DataSet1", dt);
            //此处可以有多个数据源
            this.rptView.LocalReport.DataSources.Add(rdsItem);
            if (m_orderconnecter.Length <= 0)
                m_orderconnecter = "无";
            if (m_orderphone.Length <= 0)
                m_orderphone = "无";
            List<ReportParameter> lstParameter = new List<ReportParameter>() {
            new ReportParameter("DDH",m_orderid),
            new ReportParameter("DDRQ",m_orderdate),
            new ReportParameter("JHRQ",m_orderoutdate),
            new ReportParameter("LXR",m_orderconnecter),
            new ReportParameter("LXDH",m_orderphone)
           };

            this.rptView.LocalReport.SetParameters(lstParameter);
            this.rptView.ZoomMode = ZoomMode.Percent;
            this.rptView.ZoomPercent = 100;
            //第五步：刷新报表
            this.rptView.LocalReport.EnableExternalImages = true;
            this.rptView.ZoomPercent = 120;
            this.rptView.RefreshReport();
            //清空五金对象
            if (m_wjids.Count > 0)
            {
                for (int i = 0; i < m_wjids.Count; i++)
                    Form1.g_form1.axEWdraw3.Delete((int)m_wjids[i]);
            }
            if (m_imgs.Count > 0)
            {
                for (int i = 0; i < m_imgs.Count; i++)
                {
                    if (m_imgs[i] != null)
                    {
                        ((Bitmap)m_imgs[i]).Dispose();
                    }
                }
            }
            m_imgs.Clear();   
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            //绘制报表
            MakeReport();
        }

        private void Form13_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form15 fm15 = new Form15();
            fm15.m_orderid = m_orderid;
            fm15.m_orderdate = m_orderdate;
            fm15.g_prjallitems = g_allitems;
            fm15.m_tplanks = m_tplanks;
            if (fm15.ShowDialog() == DialogResult.OK)
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
                MakeReport();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedlg = new SaveFileDialog();
            savedlg.Filter = "Excel文件|*.xls";
            savedlg.RestoreDirectory = true;
            if (savedlg.ShowDialog() == DialogResult.OK)
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rptView.LocalReport.Render(
                   "Excel", null, out mimeType, out encoding, out extension,
                   out streamids, out warnings);

                FileStream fs = new FileStream(savedlg.FileName, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog savedlg = new SaveFileDialog();
            savedlg.Filter = "PDF文件|*.pdf";
            savedlg.RestoreDirectory = true;
            if (savedlg.ShowDialog() == DialogResult.OK)
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rptView.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding, out extension,
                   out streamids, out warnings);

                FileStream fs = new FileStream(savedlg.FileName, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

        }

        private void Form13_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.g_form1.axEWdraw3.ZoomALL();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LocalReport rept = this.rptView.LocalReport;
            double top = 0;//
            double bottom = 0;//
            double left = 0;//
            double right = 0;//

            BillPrint.Run(rept,top,bottom,left,right);
        }
    }
}
