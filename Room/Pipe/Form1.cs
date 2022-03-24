using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Room;
using System.Runtime.InteropServices;
using Code;

namespace Pipe
{
    
    public partial class Form1 : Form
    {
        //int g_intjs = 0;
        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll")] private static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        /// <summary>
        /// Win32 API Constants for ShowWindowAsync()
        /// </summary>
        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;
        bool g_isactiveupdate = false;
        private class SNode
        {//2019-09-17
            public SNode() { code = 0; name = ""; type = -1; }
            public int code;
            public string name;
            public short type;
        };
        //设计柜体前和后的范围盒 2020-03-20
        CBoundingBox g_beforebox = new CBoundingBox();
        CBoundingBox g_afterbox = new CBoundingBox();
        ArrayList g_beforelist = new ArrayList();//2020-03-23
        ArrayList g_beforelist1 = new ArrayList();//2020-03-30
        public int insertgroupadd = 0;
        //修改尺寸
        public int g_editpent = 0;
        public int g_editptype = -1;
        public double g_editw = 0;
        public double g_edith = 0;
        public double g_editt = 0;
        public double g_edita = 0;
        public double g_oldeditw = 0;
        public double g_oldedith = 0;
        public double g_oldeditt = 0;
        public double g_oldedita = 0;

        //博古架 2020-03-04
        public bool g_isboguins = false;
        public double g_bogubx = 0;
        public double g_boguby = 0;
        public double g_bogubz = 0;
        public double g_boguinsx = 0;
        public double g_boguinsy = 0;
        public double g_boguinsz = 0;
        public bool g_selbogu = false;
        public int g_selboguid = 0;
        public double g_selbogux = 0.0;
        public double g_selboguy = 0.0;
        public double g_selboguz = 0.0;
        //F架
        public bool g_isfjiains = false;//2020-02-21
        public double g_fjiabx = 0;
        public double g_fjiaby = 0;
        public double g_fjiabz = 0;
        public double g_fjiainsx = 0;
        public double g_fjiainsy = 0;
        public double g_fjiainsz = 0;
        public bool g_selfjia = false;//2020-01-08
        public int g_selfjiaid = 0;//2020-01-08
        public double g_selfjiax = 0.0;
        public double g_selfjiay = 0.0;
        public double g_selfjiaz = 0.0;
        //
        public int g_winx = 0;
        public int g_winy = 0;
        public int g_yitongorg = 0;//原衣通
        public int g_yitongmode = 0;//衣通的状态
        public int g_plankcode = -1;//替换为异型板的类型号
        public int g_timerproc = 0;
        public CPlank g_axismoveplank;//单轴移动的板材信息
        public int g_axismovespacesize = 0;//单轴移动后生成的空间体个数
        public int g_axismoveid = 0;//单轴移动后,得到移动板材的ID号
        public ArrayList g_axismoveleftup = new ArrayList();//单轴移动左侧包围盒
        public ArrayList g_axismoverightdown = new ArrayList();//单轴移动右侧包围盒
        public ArrayList g_axismoveidsleftup = new ArrayList();
        public ArrayList g_axismoveidsrightdown = new ArrayList();
        public bool g_isquit = false;
        public bool g_ishide = false;//2020-01-23是否是消隐状态
        public ArrayList g_allhides = new ArrayList();//2020-01-23 消隐对象ID数组
        //当前抽屉的基点位置 2019-12-30
        public bool g_selct = false;//2020-01-08
        public int  g_selctid = 0;//2020-01-08
        public double g_selctx = 0.0;
        public double g_selcty = 0.0;
        public double g_selctz = 0.0;
        public bool g_iswgct = false;//是否是外盖抽屉
        public bool g_isctins = false;//2020-01-03
        public double g_ctbx = 0;
        public double g_ctby = 0;
        public double g_ctbz = 0;
        public double g_ctminx = 0;
        public double g_ctminy = 0;
        public double g_ctminz = 0;
        public double g_ctmaxx = 0;
        public double g_ctmaxy = 0;
        public double g_ctmaxz = 0;
        public double g_ctinsx = 0;
        public double g_ctinsy = 0;
        public double g_ctinsz = 0;
        //
        public double g_sminx = 0;
        public double g_sminy = 0;
        public double g_sminz = 0;
        public double g_smaxx = 0;
        public double g_smaxy = 0;
        public double g_smaxz = 0;
        //
        public double g_ctleft = 0.0;//抽屉左侧
        public double g_ctright = 0.0;//抽屉右侧
        public double g_cttop = 0.0;//抽屉上侧
        public double g_ctbottom = 0.0;//抽屉下侧
        public double g_ctwidth = 0.0;//抽屉宽度
        public double g_ctheight = 0.0;//抽屉高度
        public double g_cmbottom = 0.0;//抽面低于抽盒
        public double g_ctdbmspace = 0;//单边门缝隙
        public double g_ctdepth = 0.0;//抽屉深度
        //
        public static Form1 g_form1;
        string  g_tname = "";//2019-09-17
        bool isexplodecabinet = false;//2019-12-09
        bool issingplankmove = false;//2019-12-06
        int  GDesignMode = -1;//柜体设计状态 2019-12-27
        //2019-09-19 判断单击与双击
        string g_orderid;//订单号 2019-11-14
        string g_orderdate;//订单日期 2019-12-24
        string g_orderoutdtate;//出货日期 2019-12-24
        string g_orderconnecter;//联系人 2019-12-24
        string g_orderphone;//联系电话 2019-12-24
        string g_ordermemo;//订单备注
        string g_orderuser;//用户名称 2020-02-28
        int insertorgplankid = -1;//2019-11-04
        bool isFirstClick = true;
        bool isDoubleClick = false;
        int milliseconds = 0;
        //2019-09-19 是否处理柜体编辑状态
        bool isCabinetDesign = false;
        int g_cabinetdesignid = 0;
        //
        int g_tcode = 0;//2019-09-17
        string[] args;//2019-03-04
        IntPtr g_hand;
        ArrayList g_roofids = new ArrayList();//2018-12-03
        ArrayList g_doorbottoms = new ArrayList();//2018-12-03
        bool isdrawaxismove = false;//是否是X,Y,Z单向轴移动
        int setdrawaxis = -1;//是否已经设置了移动轴(X 或 Y 或 Z)
        bool isdrawrotate = false;//2018-01-08
        int g_rbuttonselent = 0;//右键选择
        bool isdrawbalcony = false;//是否是绘制阳台 2017-11-27
        ArrayList g_balconypts;//阳台线段中心点集 2017-11-27
        ArrayList g_balconysegpts;//阳台线段点集 2017-11-27
        int g_balconyent = 0;//阳台区域对象 2017-11-27
        ArrayList g_balconywins;//将已导入的阳台窗原始数组存入到数组中,这样做可以减少从硬盘导入同一个文件的次数2017-12-05
        ArrayList g_balconyids;//创建的阳台集2017-12-05
        ArrayList g_balconyhidewall;//显示阳台时消隐的墙2017-12-05
        ArrayList g_balconyotherids;//与显示阳台相关的其它实体 2017-12-06
        ArrayList g_balconyconnectpts;//阳台与普通墙的接入点 2017-12-07
        //室内漫游 2017-10-31
        double g_innerx = 0;
        double g_innery = 0;
        double g_innerz = 0;
        bool g_isinnerorbit = false;
        //
        int canceldrawwalljs = 0;//是否中断或继续画墙 2017-03-14
        int movewalljs = 0;//是否按下左键时,移动墙 2017-03-15
        int g_limitrectangle = 0;
        int g_movewallid = 0;
        int g_tmpjs = 0;
        int g_dimposx = -1;
        int g_dimposy = -1;
        int old_dimposx = -1;
        int old_dimposy = -1;
        int g_st = 0;//2017-08-08
        bool isbeginmove = false;//2017-08-08
        bool isdrawmove = false;//2017-08-08
        bool isdrawpan = false;//2017-08-08
        double g_viewscale = 1.0;
        bool isenterchar = false;
        public class MyYXBPara//异型板的参数
        {
            public MyYXBPara()
            {
                m_name = "";
                m_val = 0.0;
            }
            public MyYXBPara(string name,double val)
            {
                m_name = name;
                m_val = val;
            }

            public string m_name;//参数名称
            public double m_val;//参数值
        }
        //异性板
        public class MyYXB
        {
            public MyYXB()
            {
                m_name = "";
            }
            public string m_name;//板材的名称
            public ArrayList m_paras;//板材的参数
        }
        ArrayList g_yxbs;//所有异型板数组
        //2018-11-25 自定义贴图
        public class MyTextureItem
        {
            public MyTextureItem()
            {
                texname = texfilename = teximagname = texbump = texpath = "";
                texwidth = texheight = matlinx = state1 = state2 = 0;
            }
            public string texname;
            public string texfilename;//实际纹理的文件名
            public string teximagname;//纹理文件的缩略图
            public string texbump;//纹理文件的bump
            public string texpath;//纹理文件的路径
            public int texwidth;//纹理的实际宽度
            public int texheight;//纹理的实际高度
            public int matlinx;//材质的索引
            public int state1;//状态1
            public int state2;//状态1
        }
        //2018-11-13 自定义的图版列表参数类
        public class MYImageItem
        {
            public MYImageItem()
            {
                classname = itemname = imagefile = "";
                width = depth = height = ridz = walldist = 0.0;
                state1 = state2 = 0;
            }
            public string classname;//类名
            public string itemname;//子项目名称,如果为空,则说明是总类
            public string filename;//实际文件名
            public string imagefile;//图片文件名
            public string itemdir;//子项简模目录名称 2018-11-05
            public string itemsmoothdir;//子项精模目录名称
            public double width;//宽度X方向
            public double depth;//深度Y方向
            public double height;//高度Z方向
            public double ridz;//离地高度 Z方向
            public double walldist;//贴墙的距离
            public int state1;//状态位1 备用 
            public int state2;//状态位2 备用
        }
        //2018-11-03
        string tmodepath;//资源总目录
        int titemslevel0 = 0;
        int titemslevel1 = 0;
        ArrayList totalclassitems = new ArrayList();//总类的数组
        ArrayList totalclassitems0 = new ArrayList();//总类的数组
        ArrayList totalclassitems1 = new ArrayList();//总类的数组
        ArrayList subclassitems = new ArrayList();//子类的数组
        ArrayList curclassitems0 = new ArrayList();//当前子项列表0,如果有的话
        ArrayList curclassitems1 = new ArrayList();//当前子项列表1,如果有的话
        ArrayList curflooritems = new ArrayList();//地面材质集
        ArrayList curwallitems = new ArrayList();//地面材质集
        string g_subclassname = "";
        //2017-12-01
        private class BalconyWin
        {
            public BalconyWin() { id = 0; grpname = ""; }
            public int id;
            public string grpname;
        }
        /*为墙结构
         * 用途:
         * 1.用在了删除数据结点(delptdata) 2016-11-06
         * 2.单墙是否共享的判断上(此时,inx为一个计数器)
         */
        private class SWallSeg
        {
            public SWallSeg() { id = id1 = 0; inx = 0; x1 = y1 = z1 = x2 = y2 = z2 = vx = vy = vz = 0.0; }
            public int id;
            public int inx;
            public double x1;
            public double y1;
            public double z1;
            public double x2;
            public double y2;
            public double z2;
            public double vx;
            public double vy;
            public double vz;
            public int id1;
        };

        /*为阳台墙结构
         * 用途:
         * 1.用在了删除数据结点(delptdata) 2016-11-06
         * 2.单墙是否共享的判断上(此时,inx为一个计数器)
         */
        private class BalconyWallSeg
        {
            public BalconyWallSeg()
            {
                id = id1 = 0; inx = 0; x1 = y1 = z1 = x2 = y2 = z2 = vx = vy = vz = ang = 0.0; st = 0;
                b_width = b_height = b_theight = b_bheight = 0;
                b_btype = 0;
                isbarrier = ishandrail = ismidpanel = false;
                ba_length = ba_width = 0; ba_num = 3;
                hr_width = hr_thickness = mp_updist = mp_downdist = mp_thickness = 0;
                wintype = 0;
                m_dx = m_dy = m_dz = 0.0;
            }
            public int id;
            public int inx;
            public double x1;
            public double y1;
            public double z1;
            public double x2;
            public double y2;
            public double z2;
            public double vx;
            public double vy;
            public double vz;
            public double ang;//2017-11-29
            public int id1;
            public int st;//2017-11-29
            public int wintype;
            public double b_width;//基础数据:宽度
            public double b_height;//基础数据:高度
            public double b_theight;//基础数据:封阳台高度
            public double b_bheight;//基础数据:离地高度
            public int b_btype;//底部模式
            public bool isbarrier;//是否有栏栅的参数
            public double ba_length;//栏栅的长度
            public double ba_width;//栏栅的宽度
            public int ba_num;//栏栅的数量
            public bool ishandrail;//是否有栏杆的参数
            public double hr_bheight;//栏杆离地参数
            public double hr_width;//栏杆的宽度
            public double hr_thickness;//栏杆的厚度
            public bool ismidpanel;//是否有中板的参数
            public double mp_updist;//中板上边距
            public double mp_downdist;//中板上边距
            public double mp_thickness;//中板厚度
            public double m_dx;//窗户的内朝向 2017-11-30
            public double m_dy;
            public double m_dz;
        };

        //2016-11-14
        int g_singlerooment = 0;//单房间的ID
        //为弧墙结构 2016-10-25
        private class SInxWall
        {
            public SInxWall() { inx = 0; x = y = z = dist = 0.0; }
            public int inx;
            public double x;
            public double y;
            public double z;
            public double dist;
        };

        //全局的墙高与宽
        double g_wallheight = 2800.0;
        double g_wallthickness = 128.0;
        double g_maxz = 3000.0;
        //交互操作的状态控制 2016-10-28
        private enum InteractiveState
        {
            Nothing = 0,
            BeginDrawWall = 1,
            BeginMoveWall = 2,
            MoveingWall = 3,
            EndMoveWall = 4
        };
        //2017-10-13
        private class SPoint
        {
            public SPoint(double ix, double iy, double iz) { x = ix; y = iy; z = iz; }
            public SPoint() { x = y = z = 0.0; }
            public double x;
            public double y;
            public double z;
        };
        //
        InteractiveState g_state;
        int g_id;
        int g_inx;
        double g_x, g_y, g_z;
        int g_wr, g_wg, g_wb;//默认墙的颜色
        double g_bx, g_by, g_bz;//2017-08-08
        double g_mx, g_my, g_mz;//2017-08-08
                                //2016-12-13
        ArrayList singlewallids = new ArrayList();//单墙的ID集变量
        ArrayList dwarfewallids = new ArrayList();//单墙(矮墙)的ID集变量
        ArrayList topbottomids = new ArrayList();//墙的走向实体ID集变量
        //

        //2016-11-01
        string g_bakstr;
        //
        bool isdrawpolyline = false;
        int tmpent = 0;
        int tmppipe = 0;
        int connectent = 0;//链接的实体id
        int connecttype = 0;//连接的类型
        int connectinx = 0;//边接的线段索引
        int drawsegjs = 0;//绘制线段时,点击的次数
        int wantdelent = 0;//要删除的实体,用于连接起始与终止的实体
        int edgeptjs = 0;//在边线点的计数
        bool isseloneent = false;//
        ArrayList otherroomcabinet = new ArrayList();//2017-03-01
        ArrayList tmplist = new ArrayList();
        ArrayList ptslist = new ArrayList();
        ArrayList tlist0 = new ArrayList();//2016-09-02
        ArrayList tlist1 = new ArrayList();//2016-09-02
        ArrayList selids = new ArrayList();//2016-09-27
        int tmppipe1 = 0;//2016-09-18
        bool isdrawabsorb = false;
        int lastsym = 0;
        //2016-09-06
        ArrayList orgidlist = new ArrayList();//原有墙的ID数组
        ArrayList cpidlist = new ArrayList();//复制墙的ID数组
        ArrayList subtractidlist = new ArrayList();//减去洞的墙的ID数组
        ArrayList symbolidlist = new ArrayList();//符号的ID数组
        ArrayList door3dsidlist = new ArrayList();//3DS门的ID数组
        ArrayList singroomhides = new ArrayList();//单房间时清隐的实体信2016-11-09
        ArrayList furniturehides = new ArrayList();//单房间时清隐的实体信2016-11-09
        int g_viewmode = 0;
        public Form1()
        {
            InitializeComponent();
            g_form1 = this;
        }
        public Form1(string[] args)
        {
            this.args = args;
            InitializeComponent();
            g_form1 = this;
        }
        //DrawWall墙的绘制
        private void DrawWall()//button1_Click(object sender, EventArgs e)
        {
            if (!isdrawpolyline)
            {//如果当前不处在画墙的状态,则启动画墙 2017-03-17
                canceldrawwalljs = 0;//2017-03-14 双击右键退出
                isdrawpolyline = false;//2017-03-14 双击右键退出
                timer3.Enabled = false;//2017-03-14 双击右键退出
                //
                tmplist.Clear();//初始化
                tlist0.Clear();
                tlist1.Clear();
                tmpent = 0;//初始化
                tmppipe = 0;//初始化
                tmppipe1 = 0;//初始化 2016-09-18
                drawsegjs = 0;//初始化画线计数
                connecttype = 0;//初始化连接类型
                wantdelent = 0;//要删除的实体,用于连接起始与终止的实体
                connectent = 0;//初始化连接的实体ID
                edgeptjs = 0;//初始化边点计数
                if (!axEWdraw1.IsEndCommand())
                {
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }

                axEWdraw1.SetORTHO(true);
                axEWdraw1.EnableCommandOnLBDown(true);
                axEWdraw1.CancelCommand();
                axEWdraw1.EnableDrawPolylinePipe(true, g_wallthickness, g_wallheight);
                isdrawpolyline = true;
                g_state = InteractiveState.BeginDrawWall;
                axEWdraw1.SetDrawPolylineLenLimit(g_wallthickness);
                axEWdraw1.ToDrawPolyLine();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g_state = InteractiveState.Nothing;
            g_id = 0;
            g_inx = -1;
            g_x = g_y = g_z = 0.0;
            g_wr = g_wg = g_wb = 150;
            axEWdraw1.SetWallDefColor(g_wr, g_wg, g_wb);
            axEWdraw1.SetShapeSmooth(3);
            //
            axEWdraw1.SetBackGroundColor(axEWdraw1.RGBToIndex(255, 255, 245));
            axEWdraw1.SetLayerColor("0", axEWdraw1.RGBToIndex(g_wr, g_wg, g_wb));
            axEWdraw1.AddOne2DVertex(0, 0, 0);
            axEWdraw1.AddOne2DVertex(0, 30000, 0);
            axEWdraw1.AddOne2DVertex(30000, 30000, 0);
            axEWdraw1.AddOne2DVertex(30000, 0, 0);
            g_limitrectangle = axEWdraw1.PolyLine2D(true);
            axEWdraw1.SetEntColor(g_limitrectangle, 1);
            axEWdraw1.DeactivateEnt(g_limitrectangle);
            axEWdraw1.Clear2DPtBuf();
            axEWdraw1.ZoomALL();
            axEWdraw1.SetGridValue(300, 300, 15000, 15000, 0);
            axEWdraw1.SetGridOrgPt(-15000, -15000);
            axEWdraw1.SetGridColor(214, 214, 214, 214, 214, 214);
            axEWdraw1.SetGridOrgColor(true, 214, 214, 214, 214, 214, 214);//2017-08-23
            axEWdraw1.SetGridOn(true);
            axEWdraw1.DisableRightMenu(true);
            axEWdraw1.Delete(g_limitrectangle);
            //
            axEWdraw1.SetSingleWallThickness(g_wallthickness);
            //设置在绘制polyline线时显示动态的长度标注 2016-09-22
            axEWdraw1.EnableDrawPolyLineDymDim(true, 100, 0, 0, 0, 300, g_maxz);
            //设置在吸附窗或门时,如果吸附失败,则删除要吸附的窗或门 2016-10-11
            axEWdraw1.EnableDelGrpWhenAbsorbFail(true);
            //设置在绘制polyline线时自动显示为pipe放样体
            axEWdraw1.EnableWallPipe(true);
            //禁用框选择功能
            axEWdraw1.DisableRectSelect(true);
            axEWdraw1.DisableDelKeyArea(true);
            //设置对齐线有效
            axEWdraw1.EnableDrawWallAlignLine(true, g_wallthickness, 128);
            //设置对齐线颜色
            axEWdraw1.SetDrawPolyLineHintPoint(150, 128, 0.7, 3000);
            //设置绘制polyline时的颜色
            axEWdraw1.EnableDrawPolyLineColor(true, 128);
            //
            g_viewscale = axEWdraw1.GetViewScale();
            axEWdraw1.SetViewScale(g_viewscale * 4.19);
            axEWdraw1.ZoomALL();
            axEWdraw1.StoreView();
            axEWdraw1.EnableRightButtonSel(true);
            axEWdraw1.SetDimDec(0);
            string runpath = System.Windows.Forms.Application.StartupPath;
            string setpath = runpath + "\\mode";
            axEWdraw1.SetSmoothMeshPath(setpath);
            tmodepath = setpath; 
            axEWdraw1.MakeSkeyBox("city");//
            ReadSubClassItems("", ref curclassitems0);
            ReadTotalClassItems(0);
            ReadTotalClassItems(1);
            ReadFloorTextureItems(0, ref curflooritems);
            ReadWallTextureItems(1, ref curwallitems);
            axEWdraw1.SetExtLightMode(2);//设置灯光为暖色调
            g_hand = this.Handle;
            timer7.Enabled = true;
            axEWdraw1.SetHomeMode(true);
            if (this.args != null)
            {
                if (this.args.Length > 0)
                {
                    if (this.args[0] == "testrender")
                    {
                        TestRender();
                    }
                }
            }
            axEWdraw1.ForceAbsorbSkipInsGrp(true);
            //初始化家具
            InitJiaJuKu();
            //初始化异型板
            InitYxbs();
            //设置空间体深度取横板上竖板最小深度
            axEWdraw3.EnableMinDepathSpace(true);
            axEWdraw3.DisableRectSelect(true);
            axEWdraw3.DisableRightMenu(true);
            //定位到家具设计页面之上
            tabControl1.SelectedIndex = 4;
            InitCabinet();   
        }
        //绘制户型
        private void DrawHome()//
        {
            if (timer6.Enabled)//2018-08-03
                timer6.Enabled = false;
            if (g_isinnerorbit)
            {//结束内部环视功能 2017-10-31
                axEWdraw1.EnableInnerOrbit(false, 100, new object[] { g_innerx, g_innery, g_innerz }, new object[] { 0, 1, 0 });
                g_isinnerorbit = false;
            }
            //
            if (!axEWdraw1.IsEndCommand())
            {//如果命令没有结束,则判断,是否有lastsym的实体
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            canceldrawwalljs = -1;
            axEWdraw1.SetPerspectiveMode(false);
            //如果当前有命令,则取消
            axEWdraw1.CancelCommand();
            //判断是否是墙洞状态
            if (g_viewmode == 1)
            {
                int swsize = axEWdraw1.GetSingleWallSize();
                int singleid = 0;
                int singledid = 0;
                int singleorgid = 0;
                int singleinx = -1;
                double sx, sy, sz, ex, ey, ez;
                sx = sy = sz = ex = ey = ez = 0.0;
                double ox, oy, oz;
                ox = oy = oz = 0.0;
                double dist = 0;
                singlewallids.Clear();
                for (int i = 0; i < swsize; i++)
                {
                    axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                    axEWdraw1.SetEntDisplayMode(singleid, 3);
                    axEWdraw1.SetWallColor(singleid, g_wr, g_wg, g_wb);
                    if (!axEWdraw1.IsDisplayed((int)singleid))
                    {
                        axEWdraw1.SetEntityInvisible((int)singleid, false);
                    }
                }
            }
            //2018-13-03
            for (int i = 0; i < g_roofids.Count; i++)
            {
                axEWdraw1.Delete((int)g_roofids[i]);
            }
            g_roofids.Clear();
            //2019-03-13
            for (int i = 0; i < g_doorbottoms.Count; i++)
            {
                axEWdraw1.Delete((int)g_doorbottoms[i]);
            }
            g_doorbottoms.Clear();
            //
            g_viewmode = 0;
            //设置矮墙下的实体不显示
            axEWdraw1.MakeDwarfWallTexSew(false);
            //显示面积对象
            ShowArea();
            //恢复房间
            RestoreRoom();
            //
            SetHoleMode(0);
            //显示符号
            ShowSymbol();
            //显示标注
            ShowDim();
            //显示承重墙,如果有的话
            ShowWeightWall();
            //俯视图
            axEWdraw1.SetViewCondition(1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //西南视图
            axEWdraw1.SetViewCondition(9);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            axEWdraw1.SetViewCondition(10);
        }
        /*MakeSection 根据一个线段的起始点与终止点画出pipe所需截面
         * 参数:
         * x1,y1,z1   线段起点
         * x2,y2,z2   线段终点
         * thickness  墙的厚度
         * h          墙的高度
         * 返回值:
         * 如果成功返回ture,其它返回false
         */
        private int MakeSection(double x1, double y1, double z1, double x2, double y2, double z2, double thickness, double h)
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
            axEWdraw1.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, 90, ref ox1, ref oy1, ref oz1);
            axEWdraw1.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, -90, ref ox2, ref oy2, ref oz2);
            axEWdraw1.Polar(new object[] { x1, y1, z1 }, new object[] { ox2, oy2, oz2 }, thickness / 2.0, ref p1x, ref p1y, ref p1z);
            axEWdraw1.Polar(new object[] { x1, y1, z1 }, new object[] { ox1, oy1, oz1 }, thickness / 2.0, ref p2x, ref p2y, ref p2z);
            axEWdraw1.AddOne3DVertex(p1x, p1y, p1z);
            axEWdraw1.AddOne3DVertex(p2x, p2y, p2z);
            axEWdraw1.AddOne3DVertex(p2x, p2y, p2z + h);
            axEWdraw1.AddOne3DVertex(p1x, p1y, p1z + h);
            int ent = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            int face = axEWdraw1.EntToFace(ent, true);
            return face;
            //return ent;
        }
        /*SetPtsToSolid从墙实体对象上设置点集
         * 参数:
        * id  输入  墙对象的ID
        * pts 输入  墙对象的点数组
         * 如果设置成功返回true,如果失败返回false
        */
        private bool SetPtsToSolid(long id, ref ArrayList pts)
        {
            int entsize = axEWdraw1.GetEntSize();
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
                int ent = axEWdraw1.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw1.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "pts:");
                        if (ffinx >= 0)//2016-11-11
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            //2016-11-11 如果不为空,且没找到,则直接相加
                            string substr = orgstr + str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw1.SetEntityUserData(ent, str);
                    return true;
                }
            }
            return false;
        }
        /*GetPtsFromSolid从墙实体对象上取得点集
         * 参数:
         * id  输入  墙对象的ID
         * pts 输出  墙对象的点数组
         * 返回值:
         * 如果取得成功返回true,如果失败返回false
         */
        private bool GetPtsFromSolid(int id, ref ArrayList pts)
        {
            string orgstr;
            orgstr = axEWdraw1.GetEntityUserData(id);
            if (orgstr != "")
            {
                int ffinx = IsHaveStrField(orgstr, "pts:");
                if (ffinx >= 0)
                {
                    int feinx = orgstr.IndexOf(";", ffinx);
                    string substr = orgstr.Substring(ffinx + 4, feinx - (ffinx + 4));
                    string[] strarr = substr.Split(',');
                    for (int j = 0; j < strarr.Length; j++)
                    {
                        pts.Add(zPubFun.zPubFunLib.CStr2Double(strarr[j]));
                    }
                    if (pts.Count >= 3)
                        return true;
                }
            }
            return false;
        }

        /*GetNearLine 根据给出的点和墙实体的点数据中找到最近的一个点
         * 参数:
         * x,y,z           输入  给出一个点
         * pts            输出  最近对象的所有点数据的数组
         * inx             输出  最近对象中最近的线段索引
         * minx,miny,minz  输出  距离给出点最近的点
         * tol             输入  这是一个距离值,如果大于零,表示如果给出的x,y,z的点距离 点数组中某个点的距离小于tol,则直接将minx,miny,minz设置为数组中的某点.
         *                       如果为0,则没有该特性.
         * 返回值:
         * 执行成功返回最近的距离值.失败返回-1.0.
         */
        private double GetNearLine(double x, double y, double z, ref ArrayList pts, ref int inx, ref double minx, ref double miny, ref double minz, double tol = 128.0)
        {
            int linesize = pts.Count / 3 - 1;
            double mindist = 0.0;
            int tmpjs = 0;
            int segjs = 0;
            for (int i = 0; i < linesize; i++)
            {
                double dist, ox, oy, oz;
                dist = ox = oy = oz = 0.0;
                double x1, y1, z1;
                double x2, y2, z2;
                x1 = (double)pts[i * 3];
                y1 = (double)pts[i * 3 + 1];
                z1 = (double)pts[i * 3 + 2];
                x2 = (double)pts[(i + 1) * 3];
                y2 = (double)pts[(i + 1) * 3 + 1];
                z2 = (double)pts[(i + 1) * 3 + 2];
                if (axEWdraw1.GetDistPtAndLineSeg(new object[] { x, y, z }, new object[] { x1, y1, z1 },
                                                        new object[] { x2, y2, z2 },
                                                      ref dist, ref ox, ref oy, ref oz))
                {
                    if (tmpjs == 0)
                    {
                        mindist = dist;
                        inx = segjs;
                        if (tol > 0)
                        {
                            if (axEWdraw1.PointDistance(x, y, z, x1, y1, z1) < tol)
                            {
                                minx = x1;
                                miny = y1;
                                minz = z1;
                            }
                            else if (axEWdraw1.PointDistance(x, y, z, x2, y2, z2) < tol)
                            {
                                minx = x2;
                                miny = y2;
                                minz = z2;
                            }
                            else
                            {
                                minx = ox;
                                miny = oy;
                                minz = oz;
                            }
                        }
                        else
                        {
                            minx = ox;
                            miny = oy;
                            minz = oz;
                        }
                    }
                    else if (dist < mindist)
                    {
                        mindist = dist;
                        inx = segjs;
                        if (tol > 0)
                        {
                            if (axEWdraw1.PointDistance(x, y, z, x1, y1, z1) < tol)
                            {
                                minx = x1;
                                miny = y1;
                                minz = z1;
                            }
                            else if (axEWdraw1.PointDistance(x, y, z, x2, y2, z2) < tol)
                            {
                                minx = x2;
                                miny = y2;
                                minz = z2;
                            }
                            else
                            {
                                minx = ox;
                                miny = oy;
                                minz = oz;
                            }
                        }
                        else
                        {
                            minx = ox;
                            miny = oy;
                            minz = oz;
                        }
                    }
                    tmpjs++;
                }
                segjs++;
            }
            if (segjs > 0)
                return mindist;
            return -1.0;
        }
        /*FindMinDist 根据给出的点从所有墙实体的点数据中找到最近的一个点
         * 参数:
         * x,y,z           输入  给出一个点
         * id              输出  返回距离最后对象的ID
         * inx             输出  最近对象中最近的线段索引
         * opts            输出  最近对象的所有点数据的数组
         * minx,miny,minz  输出  距离给出点最近的点
         * tol             输入  这是一个距离值,如果大于零,表示如果给出的x,y,z的点距离 点数组中某个点的距离小于tol,则直接将minx,miny,minz设置为数组中的某点.
         *                       如果为0,则没有该特性.
         * 返回值:
         * 找到最近点返回ture,其它返回false.
         */
        private bool FindMinDist(double x, double y, double z, ref int id, ref int inx, ref ArrayList opts, ref double minx, ref double miny, ref double minz, double tol = 128.0)
        {
            int entsize = axEWdraw1.GetEntSize();
            int tmpjs = 0;
            int minid = 0;//距离最近的实体ID
            double mindist = 0.0;
            int mininx = 0;//线段索引
            ArrayList pts = new ArrayList();
            int isfirstpt = -1;
            int issecondpt = -1;
            int swsize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez;
            sx = sy = sz = ex = ey = ez = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            double dist = 0;
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                if (axEWdraw1.GetDistPtAndLineSeg(new object[] { x, y, 0 }, new object[] { sx, sy, 0 },
                                                    new object[] { ex, ey, 0 },
                                                    ref dist, ref ox, ref oy, ref oz))
                {
                    if (tmpjs == 0)
                    {
                        mindist = dist;
                        mininx = 0;
                        minid = singleid;
                        //最近点
                        if (axEWdraw1.PointDistance(ox, oy, oz, sx, sy, sz) < tol)
                        {
                            minx = sx;
                            miny = sy;
                            minz = sz;
                        }
                        else if (axEWdraw1.PointDistance(ox, oy, oz, ex, ey, ez) < tol)
                        {
                            minx = ex;
                            miny = ey;
                            minz = ez;
                        }
                        else
                        {
                            minx = ox;
                            miny = oy;
                            minz = oz;
                        }
                        opts.Clear();
                        //for (int j = 0; j < pts.Count; j++)
                        opts.Add(sx);
                        opts.Add(sy);
                        opts.Add(sz);
                        //
                        opts.Add(ex);
                        opts.Add(ey);
                        opts.Add(ez);

                    }
                    else if (dist < mindist)
                    {
                        mindist = dist;
                        mininx = 0;
                        minid = singleid;
                        //最近点
                        if (axEWdraw1.PointDistance(ox, oy, oz, sx, sy, sz) < tol)
                        {
                            minx = sx;
                            miny = sy;
                            minz = sz;
                        }
                        else if (axEWdraw1.PointDistance(ox, oy, oz, ex, ey, ez) < tol)
                        {
                            minx = ex;
                            miny = ey;
                            minz = ez;
                        }
                        else
                        {
                            minx = ox;
                            miny = oy;
                            minz = oz;
                        }
                        opts.Clear();
                        //for (int j = 0; j < pts.Count; j++)
                        opts.Add(sx);
                        opts.Add(sy);
                        opts.Add(sz);
                        //
                        opts.Add(ex);
                        opts.Add(ey);
                        opts.Add(ez);
                    }
                }
                tmpjs++;
            }
            if (tmppipe > 0)
            {
                pts.Clear();
                if (GetPtsFromSolid(tmppipe, ref pts))//2017-01-10 2016-09-02
                {
                    int tinx = 0;
                    ox = oy = oz = 0.0;
                    dist = GetNearLine(x, y, z, ref pts, ref tinx, ref ox, ref oy, ref oz, tol);
                    if (dist < mindist || tmpjs == 0)
                    {
                        tmpjs++;
                        mindist = dist;
                        mininx = tinx;
                        minid = tmppipe;
                        //最近点
                        minx = ox;
                        miny = oy;
                        minz = oz;
                        opts.Clear();
                        for (int j = 0; j < pts.Count; j++)
                            opts.Add(pts[j]);
                    }
                }
            }
            id = minid;
            inx = mininx;
            if (tmpjs > 0)
                return true;
            return false;
        }

        /*DelSingleWall     删除某段墙
         * id   输入        要删除某段的墙的ID
         * inx  输入        要删除某段墙的索引 从0开始起
         * pts  输入/输出   传入的墙的点数组,以及删除后的点数组
         * id1  输出        如果删除的是墙的首段或尾段,则id1为新创建墙实体的id号为id1
         * id2  输出        如果删除的是墙的中段,则新创建的墙实体有两个,分别是id1,id2
         * 返回值:
         * 返回删除的是 首段,中段,还是尾段.
         * 如果是首段,返回1.
         * 如果是尾段,返回2.
         * 如果是中段,返回3.
         * 其它返回 0.
         */
        private int DelSingleWall(int id, int inx, ref ArrayList pts, ref int id1, ref int id2)
        {
            double px1, py1, pz1;
            double px2, py2, pz2;
            px1 = py1 = pz1 = px2 = py2 = pz2 = 0.0;
            ArrayList opts1 = new ArrayList();
            ArrayList opts2 = new ArrayList();
            int segsize = pts.Count / 3 - 1;
            int type = 0;
            //判断是否是闭合线
            bool isclose = false;
            if (segsize == 1)
            {
                axEWdraw1.Delete(id);
            }
            else if (segsize > 1)
            {
                //判断删除单墙的类型,1为首段,2为尾段,3为中间段
                if (inx == 0)
                {//首段
                    opts1.Clear();
                    for (int i = 3; i < pts.Count; i++)
                        opts1.Add(pts[i]);
                    type = 1;
                    px1 = (double)pts[0];
                    py1 = (double)pts[1];
                    pz1 = (double)pts[2];
                    px2 = (double)pts[3];
                    py2 = (double)pts[4];
                    pz2 = (double)pts[5];
                }
                else if ((inx + 1) == segsize)
                {//尾段
                    opts1.Clear();
                    for (int i = 0; i < pts.Count - 3; i++)
                        opts1.Add(pts[i]);
                    type = 2;
                    px1 = (double)pts[pts.Count - 6];
                    py1 = (double)pts[pts.Count - 5];
                    pz1 = (double)pts[pts.Count - 4];
                    px2 = (double)pts[pts.Count - 3];
                    py2 = (double)pts[pts.Count - 2];
                    pz2 = (double)pts[pts.Count - 1];
                }
                else//中间段
                {
                    if (pts.Count >= 9)
                    {
                        if (Math.Abs((double)pts[0] - (double)pts[pts.Count - 3]) < 0.0001 &&
                            Math.Abs((double)pts[1] - (double)pts[pts.Count - 2]) < 0.0001 &&
                            Math.Abs((double)pts[2] - (double)pts[pts.Count - 1]) < 0.0001
                            )
                            isclose = true;
                    }
                    if (!isclose)
                    {
                        opts1.Clear();
                        for (int i = 0; i < (inx + 1) * 3; i++)
                            opts1.Add(pts[i]);
                        opts2.Clear();
                        for (int i = (inx + 1) * 3; i < pts.Count; i++)
                            opts2.Add(pts[i]);
                        //2016-09-08
                        px1 = (double)pts[inx * 3];
                        py1 = (double)pts[inx * 3 + 1];
                        pz1 = (double)pts[inx * 3 + 2];
                        px2 = (double)pts[(inx + 1) * 3];
                        py2 = (double)pts[(inx + 1) * 3 + 1];
                        pz2 = (double)pts[(inx + 1) * 3 + 2];
                    }
                    else
                    {
                        opts1.Clear();
                        for (int i = 0; i < (inx + 1) * 3; i++)
                            opts1.Add(pts[i]);
                        opts2.Clear();
                        for (int i = (inx + 1) * 3; i < pts.Count; i++)
                            opts2.Add(pts[i]);
                        //段2初始段的的总段数
                        int segnum = opts2.Count / 3 - 1;
                        //追加段1的点数据
                        for (int i = 3; i < opts1.Count; i++)
                            opts2.Add(opts1[i]);
                        opts1.Clear();
                        //2016-09-08
                        px1 = (double)pts[inx * 3];
                        py1 = (double)pts[inx * 3 + 1];
                        pz1 = (double)pts[inx * 3 + 2];
                        px2 = (double)pts[(inx + 1) * 3];
                        py2 = (double)pts[(inx + 1) * 3 + 1];
                        pz2 = (double)pts[(inx + 1) * 3 + 2];
                    }
                    type = 3;
                }
                switch (type)
                {
                    case 1:
                    case 2:
                        {
                            //创建polyline线
                            double x, y, z, x1, y1, z1, x2, y2, z2;
                            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                            axEWdraw1.Clear3DPtBuf();
                            int size = opts1.Count / 3;
                            for (int i = 0; i < size; i++)
                            {
                                x = (double)opts1[i * 3];
                                y = (double)opts1[i * 3 + 1];
                                z = (double)opts1[i * 3 + 2];
                                axEWdraw1.AddOne3DVertex(x, y, z);
                            }
                            int tmppolyline = axEWdraw1.PolyLine3D(false);
                            axEWdraw1.Clear3DPtBuf();
                            //创建截面
                            x1 = (double)opts1[0];
                            y1 = (double)opts1[1];
                            z1 = (double)opts1[2];
                            x2 = (double)opts1[3];
                            y2 = (double)opts1[4];
                            z2 = (double)opts1[5];
                            //最后两个参数分别是墙宽与墙高
                            int section = 0;
                            section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);
                            //创建Pipe对象
                            axEWdraw1.SetPipeAlign(false);
                            id1 = axEWdraw1.Pipe(tmppolyline, section);
                            axEWdraw1.SetEntAbsorb(id1);//2016-09-08
                            axEWdraw1.SetPipeAlign(true);
                            axEWdraw1.Delete(section);
                            axEWdraw1.Delete(tmppolyline);
                            SetPtsToSolid(id1, ref opts1);
                            SetThicknessToSolid(id1, g_wallthickness);//设置墙的厚度 2016-11-03
                            SetHeightToSolid(id1, g_wallheight);//设置墙的厚度 2016-11-03
                            axEWdraw1.SplitWall(id1);//拆分墙 2016-11-26
                            axEWdraw1.Delete(id);//删除原实体(要放在拆墙后) 2016-11-26
                        }
                        break;
                    case 3:
                        {
                            double x, y, z, x1, y1, z1, x2, y2, z2;
                            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                            int size = 0;
                            int tmppolyline = 0;
                            int section = 0;
                            //第一段
                            if (!isclose)
                            {
                                axEWdraw1.Clear3DPtBuf();
                                size = opts1.Count / 3;
                                for (int i = 0; i < size; i++)
                                {
                                    x = (double)opts1[i * 3];
                                    y = (double)opts1[i * 3 + 1];
                                    z = (double)opts1[i * 3 + 2];
                                    axEWdraw1.AddOne3DVertex(x, y, z);
                                }
                                tmppolyline = axEWdraw1.PolyLine3D(false);
                                axEWdraw1.Clear3DPtBuf();
                                //创建截面
                                x1 = (double)opts1[0];
                                y1 = (double)opts1[1];
                                z1 = (double)opts1[2];
                                x2 = (double)opts1[3];
                                y2 = (double)opts1[4];
                                z2 = (double)opts1[5];
                                section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);//最后两个参数分别是墙宽与墙高
                                //创建Pipe对象
                                axEWdraw1.SetPipeAlign(false);
                                id1 = axEWdraw1.Pipe(tmppolyline, section);
                                axEWdraw1.SetEntAbsorb(id1);//2016-09-08
                                axEWdraw1.SetPipeAlign(true);
                                axEWdraw1.Delete(section);
                                axEWdraw1.Delete(tmppolyline);
                                SetPtsToSolid(id1, ref opts1);
                                SetThicknessToSolid(id1, g_wallthickness);//设置墙的厚度 2016-11-03
                                SetHeightToSolid(id1, g_wallheight);//设置墙的厚度 2016-11-03
                            }
                            //第二段
                            axEWdraw1.Clear3DPtBuf();
                            size = opts2.Count / 3;
                            for (int i = 0; i < size; i++)
                            {
                                x = (double)opts2[i * 3];
                                y = (double)opts2[i * 3 + 1];
                                z = (double)opts2[i * 3 + 2];
                                axEWdraw1.AddOne3DVertex(x, y, z);
                            }
                            tmppolyline = axEWdraw1.PolyLine3D(false);
                            axEWdraw1.Clear3DPtBuf();
                            //创建截面
                            x1 = (double)opts2[0];
                            y1 = (double)opts2[1];
                            z1 = (double)opts2[2];
                            x2 = (double)opts2[3];
                            y2 = (double)opts2[4];
                            z2 = (double)opts2[5];
                            section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);//最后两个参数分别是墙宽与墙高
                            //创建Pipe对象
                            axEWdraw1.SetPipeAlign(false);
                            id2 = axEWdraw1.Pipe(tmppolyline, section);
                            axEWdraw1.SetEntAbsorb(id2);//2016-09-08
                            axEWdraw1.SetPipeAlign(true);
                            axEWdraw1.Delete(section);
                            axEWdraw1.Delete(tmppolyline);
                            //
                            SetPtsToSolid(id2, ref opts2);
                            SetThicknessToSolid(id2, g_wallthickness);//设置墙的厚度 2016-11-03
                            SetHeightToSolid(id2, g_wallheight);//设置墙的厚度 2016-11-03
                            //2016-11-26 删除重建后的拆墙
                            if (id1 > 0) axEWdraw1.SplitWall(id1);
                            if (id2 > 0) axEWdraw1.SplitWall(id2);
                            axEWdraw1.Delete(id);//删除原实体(要在拆墙后)
                            axEWdraw1.SetEntityInvisible(id1, true);
                            axEWdraw1.SetEntityInvisible(id2, true);

                        }
                        break;
                }
                //判断组的基点是否在这个被删除的线段上
                int entsize = axEWdraw1.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    int ent = axEWdraw1.GetEntID(i);
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname = axEWdraw1.GetGroupName(ent);
                        if (grpname.IndexOf("door") >= 0)
                        {
                            double gx, gy, gz, dst, ox, oy, oz;
                            gx = gy = gz = dst = ox = oy = oz = 0.0;
                            axEWdraw1.GetGroupInsPt(ent, ref gx, ref gy, ref gz);
                            axEWdraw1.GetDistPtAndLineSeg(new object[] { gx, gy, gz }, new object[] { px1, py1, pz1 }, new object[] { px2, py2, pz2 }, ref dst, ref ox, ref oy, ref oz);
                            if (dst < 0.01)
                            {
                                axEWdraw1.Delete(ent);
                            }
                        }
                    }
                }
            }
            return type;
        }
        private void axEWdraw1_GetProcInfo(object sender, AxEWDRAWLib._DAdrawEvents_GetProcInfoEvent e)
        {
            if (g_isquit)
                return;

            if (e.info == "enter the point")
            {
                label1.Text = "绘制墙面,可输入长度.双击右键结束. 结束后可拖拽已封闭墙面.";
            }
            else if (e.info == "Enter the point")
            {
                label1.Text = "绘制墙面,可输入长度.双击右键结束.结束后可拖拽已封闭墙面.";
            }
            else label1.Text = e.info;
            if (e.info == "")
            {
                listView1.SelectedItems.Clear();
                //2017-02-16
                textBox1.Visible = false;
                if (isdrawpolyline)
                {
                    int size = tmplist.Count / 3;
                    if (size >= 2)
                    {
                        int ent = axEWdraw1.GetEntID(axEWdraw1.GetEntSize());
                        if (ent > 0)
                        {//删除todrawpolyline绘制的polyline多段线
                            int type = axEWdraw1.GetEntType(ent);
                            if (type != 59 && type == 9)
                                axEWdraw1.Delete(ent);
                        }
                        if (tmpent > 0)
                        {//删除创建的polyline多段线
                            int swsize = axEWdraw1.GetSingleWallIDBufferSize();//2018-05-22
                            if (swsize == 0)
                            {
                                swsize = axEWdraw1.GetSingleWallSize();
                                for (int i = 0; i < swsize; i++)
                                {
                                    int id = axEWdraw1.GetSingleWallID(i);
                                    axEWdraw1.SetEntAbsorb(id);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < swsize; i++)
                                {
                                    int id = axEWdraw1.GetSingleWallID(i);
                                    axEWdraw1.SetEntAbsorb(id);
                                }
                            }
                            if (isdrawbalcony)
                            {//是否处于绘制阳台的状态 2017-11-28
                                if (swsize > 0)
                                {//有新墙绘制
                                    if (tmplist.Count > 0)
                                    {
                                        //创建阳光线段中心点数组
                                        if (g_balconypts == null)
                                            g_balconypts = new ArrayList();
                                        else
                                            g_balconypts.Clear();
                                        int plen = tmplist.Count / 3;
                                        //创建外轮廓线段集
                                        if (g_balconysegpts == null)
                                            g_balconysegpts = new ArrayList();
                                        else
                                            g_balconysegpts.Clear();
                                        //
                                        for (int i = 0; i < plen - 1; i++)
                                        {
                                            //
                                            SPoint apt = new SPoint(((double)tmplist[i * 3] + (double)tmplist[(i + 1) * 3]) / 2.0, ((double)tmplist[i * 3 + 1] + (double)tmplist[(i + 1) * 3 + 1]) / 2.0, ((double)tmplist[i * 3 + 2] + (double)tmplist[(i + 1) * 3 + 2]) / 2.0);
                                            Form5.CWallSeg aseg = new Form5.CWallSeg();
                                            aseg.x1 = (double)tmplist[i * 3];
                                            aseg.y1 = (double)tmplist[i * 3 + 1];
                                            aseg.x2 = (double)tmplist[(i + 1) * 3];
                                            aseg.y2 = (double)tmplist[(i + 1) * 3 + 1];
                                            g_balconypts.Add(apt);
                                            g_balconysegpts.Add(aseg);
                                        }
                                    }
                                }
                            }
                            axEWdraw1.Delete(tmpent);
                        }
                        //                        
                        if (tmppipe1 > 0)
                        {//2016-09-18
                            axEWdraw1.SplitWall(tmppipe1);
                            int swsize = axEWdraw1.GetSingleWallIDBufferSize();
                            for (int i = 0; i < swsize; i++)
                            {
                                int id = axEWdraw1.GetSingleWallID(i);
                                axEWdraw1.SetEntAbsorb(id);
                            }
                            axEWdraw1.ZoomALL();
                            axEWdraw1.Delete(tmppipe1);
                            tmppipe1 = 0;
                        }

                        //计算屋面积
                        if (tmppipe > 0 || tmppipe1 > 0)
                        {
                            if (edgeptjs < 2)//如果>=2,则表示边线点,且已经拆墙
                            {
                                axEWdraw1.SplitWall(tmppipe);
                               
                                int swsize = axEWdraw1.GetSingleWallIDBufferSize();
                                for (int i = 0; i < swsize; i++)
                                {
                                    int id = axEWdraw1.GetSingleWallID(i);
                                    axEWdraw1.SetEntAbsorb(id);
                                }
                                
                                MakeHomeArea(g_wallthickness);
                                axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                                axEWdraw1.Delete(tmppipe);
                                tmppipe = 0;
                                
                                swsize = axEWdraw1.GetSingleWallIDBufferSize();
                                for (int i = 0; i < swsize; i++)
                                {
                                    int id = axEWdraw1.GetSingleWallID(i);
                                    axEWdraw1.SetEntAbsorb(id);
                                }
                                
                                DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                            }
                        }
                        //清空临时的点集
                        tmplist.Clear();
                    }
                    else
                    {
                    }
                    isdrawpolyline = false;
                    g_state = InteractiveState.Nothing;
                    //这个定时器的作用是,将原pipe实体消隐 2016-12-14
                    //也起画完墙后,继续画墙,双击退出
                    canceldrawwalljs++;
                    
                    timer3.Enabled = true;
                }
                else if (isdrawabsorb)
                {
                    isdrawabsorb = false;
                    lastsym = 0;
                    axEWdraw1.ClearSelected();
                    timer2.Enabled = true;
                }
                else if (isdrawmove)
                {
                    isdrawmove = false;
                    g_st = 0;
                    //2018-06-20 这里的isgetent只为移动时判断是否是正常结束,而非cancelcommand结束
                    if (e.isGetEnt)
                        lastsym = 0;
                    else
                    {
                        if (lastsym > 0)
                            axEWdraw1.Delete(lastsym);
                    }
                    axEWdraw1.ClearSelected();
                }
                else if (isdrawrotate)
                {
                    isdrawrotate = false;
                    g_st = 0;
                    lastsym = 0;
                    axEWdraw1.ClearSelected();
                }
                else if (isdrawaxismove)
                {
                    g_st = 0;
                    lastsym = 0;
                    isdrawaxismove = false;
                    axEWdraw1.SetDrawAxis(-1);
                    axEWdraw1.ClearSelected();
                }
                else if (timer3.Enabled)//2017-03-14 双击右键退出
                {
                    canceldrawwalljs++;//2017-03-14 双击右键退出
                }
                label1.Text = "";
                g_st = 0;
                isbeginmove = false;//2017-08-08
            }
            else
            {
                if (isdrawmove)
                {
                    if (e.isGetPoint)//是move且是第一个需要的基点
                    {
                        if (g_st == 2)//移动基点自动给出
                        {
                            g_st++;
                            axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                        }
                    }
                }
                else if (isdrawaxismove)//2018-01-22
                {
                    axEWdraw1.ReDrawView();
                }
            }
        }

        private void axEWdraw1_ViewMouseDown(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseDownEvent e)
        {
            //e.button为1 是点下了左键, isdrawpolyline 是画线时的标识
            g_x = Math.Round(e.xCoordinate, 3);
            g_y = Math.Round(e.yCoordinate, 3);
            g_z = Math.Round(e.zCoordinate, 3);
            //取3位小数点 2017-10-19
            double mcex = Math.Round(e.xCoordinate, 3);
            double mcey = Math.Round(e.yCoordinate, 3);
            double mcez = Math.Round(e.zCoordinate, 3);
            if (g_state != InteractiveState.Nothing && !isdrawpolyline)
            {//非绘制墙时的判断
                if (e.button == 1)
                {
                    //左键
                    switch (g_state)
                    {
                        case InteractiveState.MoveingWall:
                            {
                                g_state = InteractiveState.Nothing;
                                //
                                axEWdraw1.Clear3DPtBuf();
                                axEWdraw1.ClearIDBuffer();
                                //
                                axEWdraw1.EndMoveWall();

                                axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                                ReDrawHomeArea(g_wallthickness);

                                if (g_movewallid > 0)
                                {
                                    DelDimWalls(g_movewallid);
                                    g_movewallid = 0;
                                }
                                DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                            }
                            break;
                    }
                }
            }
            else
            {
                if (e.button == 1 && isdrawpolyline)
                {//绘制墙的拼接判断
                    connecttype = 0;//恢复初值
                    if (drawsegjs == 0)//第一个点
                    {
                        //判断是否与已有的墙相接
                        double minx, miny, minz, x1, y1, z1, x2, y2, z2;
                        minx = miny = minz = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                        //
                        if (FindMinDist(mcex, mcey, mcez, ref connectent, ref connectinx, ref ptslist, ref minx, ref miny, ref minz, g_wallthickness))
                        {
                            double dist = axEWdraw1.PointDistance(mcex, mcey, mcez, minx, miny, minz);
                            if (dist <= g_wallthickness && !IsClosePts(ref ptslist))//这里以宽度做范围,可变
                            {
                                //判断在起点还是终点
                                //x1,y1,z1是起点
                                x1 = (double)ptslist[0];
                                y1 = (double)ptslist[1];
                                z1 = (double)ptslist[2];
                                //x2,y2,z2终点
                                x2 = (double)ptslist[ptslist.Count - 3];
                                y2 = (double)ptslist[ptslist.Count - 2];
                                z2 = (double)ptslist[ptslist.Count - 1];
                                //取得距离
                                double dist1 = axEWdraw1.PointDistance(mcex, mcey, mcez, x1, y1, z1);
                                double dist2 = axEWdraw1.PointDistance(mcex, mcey, mcez, x2, y2, z2);
                                if (dist1 < g_wallthickness)//这里以宽度做范围,可变
                                {
                                    //与起点相连
                                    connecttype = 1;
                                    tmplist.Add(x1);
                                    tmplist.Add(y1);
                                    tmplist.Add(z1);
                                    //设置当前绘制polyline的首点
                                    axEWdraw1.SetDrawPolyLinePt(0, x1, y1, z1);
                                    edgeptjs++;

                                }
                                else if (dist2 < g_wallthickness)
                                {
                                    //与终点相连
                                    connecttype = 2;
                                    tmplist.Add(x2);
                                    tmplist.Add(y2);
                                    tmplist.Add(z2);
                                    //设置当前绘制polyline的首点
                                    axEWdraw1.SetDrawPolyLinePt(0, x2, y2, z2);
                                    edgeptjs++;
                                }
                                else
                                {
                                    //与中间段相连,可取minx,miny,minz为新起始点
                                    tmplist.Add(minx);
                                    tmplist.Add(miny);
                                    tmplist.Add(minz);
                                    //设置当前绘制polyline的首点
                                    axEWdraw1.SetDrawPolyLinePt(0, minx, miny, minz);
                                    connecttype = 3;
                                    edgeptjs++;
                                }
                            }
                            else if (dist <= g_wallthickness && IsClosePts(ref ptslist))//这里以宽度做范围,可变
                            {//如果是起点或终点,且已经闭合
                                if (!IsReturnPath(minx, miny, minz, ref tmplist))
                                {
                                    tmplist.Add(minx);
                                    tmplist.Add(miny);
                                    tmplist.Add(minz);
                                    axEWdraw1.SetDrawPolyLinePt(0, minx, miny, minz);
                                    connecttype = 5;//闭合状态
                                    edgeptjs++;
                                }
                                else return;
                            }
                            else
                            {
                                tmplist.Add(minx);
                                tmplist.Add(miny);
                                tmplist.Add(minz);
                                axEWdraw1.SetDrawPolyLinePt(0, minx, miny, minz);
                                edgeptjs++;
                            }
                            //
                            ptslist.Clear();
                        }
                        //
                    }
                    else if (drawsegjs >= 1)
                    {//判断是否与起点形成闭合
                        //判断是否与已有的墙相接
                        double minx, miny, minz, x1, y1, z1, x2, y2, z2;
                        minx = miny = minz = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                        //
                        if (FindMinDist(mcex, mcey, mcez, ref connectent, ref connectinx, ref ptslist, ref minx, ref miny, ref minz, g_wallthickness))
                        {
                            //判断与最近点的距离
                            double dist = axEWdraw1.PointDistance(mcex, mcey, mcez, minx, miny, minz);
                            //判断与起点的距离
                            double dist1 = axEWdraw1.PointDistance((double)tmplist[0], (double)tmplist[1], (double)tmplist[2], minx, miny, minz);
                            if (dist <= g_wallthickness && dist1 <= g_wallthickness && !IsClosePts(ref ptslist))//这里以宽度做范围,可变
                            {
                                connecttype = 4;//闭合状态
                                tmplist.Add(tmplist[0]);
                                tmplist.Add(tmplist[1]);
                                tmplist.Add(tmplist[2]);
                                edgeptjs++;
                                //定义倒数第二个顶点
                                x1 = (double)tmplist[tmplist.Count - 6];
                                y1 = (double)tmplist[tmplist.Count - 5];
                                z1 = (double)tmplist[tmplist.Count - 4];
                                //判断当前线段的方向是否在横向与纵向的容差内(10度) 2016-09-21
                                bool ishvline = true;
                                if (tmplist.Count / 3 >= 2)
                                {
                                    x2 = (double)tmplist[tmplist.Count - 3];
                                    y2 = (double)tmplist[tmplist.Count - 2];
                                    z2 = (double)tmplist[tmplist.Count - 1];
                                    double vx = x2 - x1;
                                    double vy = y2 - y1;
                                    double vz = z2 - z1;
                                    double tang1 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 1, 0, 0 }) / Math.PI * 180.0;
                                    double tang2 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { -1, 0, 0 }) / Math.PI * 180.0;
                                    double tang3 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 0, 1, 0 }) / Math.PI * 180.0;
                                    double tang4 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 0, -1, 0 }) / Math.PI * 180.0;

                                    if (((tang1 > 10 && tang1 < 45) || Math.Abs(tang1 - 45.0) < 0.001) ||
                                         ((tang2 > 10 && tang2 < 45) || Math.Abs(tang2 - 45.0) < 0.001) ||
                                         ((tang3 > 10 && tang3 < 45) || Math.Abs(tang3 - 45.0) < 0.001) ||
                                        ((tang4 > 10 && tang4 < 45) || Math.Abs(tang4 - 45.0) < 0.001)
                                        )
                                    {
                                        ishvline = false;
                                    }

                                }
                                if (ishvline)
                                {
                                    //
                                    if (Math.Abs(y1 - (double)tmplist[1]) < Math.Abs(x1 - (double)tmplist[0]))
                                    {
                                        //水平连接
                                        tmplist[tmplist.Count - 5] = tmplist[1];
                                    }
                                    else
                                    {
                                        //垂直连接
                                        tmplist[tmplist.Count - 6] = tmplist[0];
                                    }
                                }
                            }
                            else
                            {//在边线上的点
                                //判断是否与最近体的起点或终点相连
                                bool isproc = false;
                                if ((tmppipe > 0 && tmppipe != connectent) ||
                                    (tmppipe == 0 && connectent > 0 && drawsegjs == 1)
                                    )
                                {
                                    x1 = (double)ptslist[0];
                                    y1 = (double)ptslist[1];
                                    z1 = (double)ptslist[2];
                                    x2 = (double)ptslist[ptslist.Count - 3];
                                    y2 = (double)ptslist[ptslist.Count - 2];
                                    z2 = (double)ptslist[ptslist.Count - 1];
                                    //判断是否和起点相同
                                    if (axEWdraw1.PointDistance(x1, y1, z1, mcex, mcey, mcez) < g_wallthickness)
                                    {
                                        tmplist.Add(x1);
                                        tmplist.Add(y1);
                                        tmplist.Add(z1);
                                        for (int i = 0; i < ptslist.Count; i++)
                                        {
                                            //tmplist.Add(ptslist[i]);
                                            if (i == 2)
                                            {
                                                if (IsHVLineSeg(ref tmplist, g_wallthickness * 1.5))//2017-05-21
                                                    ProcPtsHVByPt((double)tmplist[tmplist.Count - 6], (double)tmplist[tmplist.Count - 5], (double)tmplist[tmplist.Count - 4], minx, miny, minz, ref tmplist);
                                            }
                                        }
                                        isproc = true;
                                    }
                                    else if (axEWdraw1.PointDistance(x2, y2, z2, mcex, mcey, mcez) < g_wallthickness)
                                    {
                                        int tptssize = ptslist.Count / 3 - 1;
                                        for (int i = tptssize; i >= 0; i--)
                                        {
                                            tmplist.Add(ptslist[i * 3]);
                                            tmplist.Add(ptslist[i * 3 + 1]);
                                            tmplist.Add(ptslist[i * 3 + 2]);
                                            if (i == tptssize)
                                            {
                                                if (IsHVLineSeg(ref tmplist, g_wallthickness * 1.5))//2017-05-21
                                                    ProcPtsHVByPt((double)tmplist[tmplist.Count - 6], (double)tmplist[tmplist.Count - 5], (double)tmplist[tmplist.Count - 4], minx, miny, minz, ref tmplist);
                                            }
                                        }
                                        isproc = true;
                                    }
                                    if (isproc)
                                    {
                                        connecttype = 6;//闭合状态
                                    }
                                }
                                if (!isproc)
                                {
                                    //计算最近点与最后一个点的距离,不得重合
                                    double dist2 = axEWdraw1.PointDistance((double)tmplist[tmplist.Count - 3], (double)tmplist[tmplist.Count - 2], (double)tmplist[tmplist.Count - 1], minx, miny, minz);
                                    //判断不重合,且在距边线一定距离内
                                    if (dist <= g_wallthickness && dist2 > 0.001)
                                    {
                                        //如果是在自身,判断是否与最后一点线的方向相反,因为如果相反,则有重合,这是不允许的
                                        if (IsReturnPath(minx, miny, minz, ref tmplist))
                                            return;
                                        else
                                        {
                                            connecttype = 5;//闭合状态
                                            //查找是否与ptlist中的点相近
                                            //---对点集操作前先复制一个,处理后有重合线时恢复 2017-05-21
                                            ArrayList baklist = new ArrayList();
                                            baklist = (ArrayList)tmplist.Clone();
                                            if (IsHVLineSeg(ref tmplist, g_wallthickness * 1.5))//2017-05-21
                                                ProcPtsHVByPt(mcex, mcey, mcez, minx, miny, minz, ref tmplist);
                                            if (IsOverPolyLine(ref tmplist))
                                            {
                                                tmplist.Clear();
                                                tmplist = (ArrayList)baklist.Clone();
                                                //MessageBox.Show("restore");
                                            }
                                            baklist.Clear();
                                            //判断绘制的墙是否重合2018-08-03
                                            if (tmplist.Count == 3)
                                            {
                                                if (IsOverDrawLine((double)tmplist[tmplist.Count - 3], (double)tmplist[tmplist.Count - 2], minx, miny, ref ptslist))
                                                {
                                                    axEWdraw1.CancelCommand();
                                                    return;
                                                }
                                                else
                                                {
                                                    tmplist.Add(minx);
                                                    tmplist.Add(miny);
                                                    tmplist.Add(minz);
                                                }
                                            }
                                            else
                                            {
                                                tmplist.Add(minx);
                                                tmplist.Add(miny);
                                                tmplist.Add(minz);
                                            }
                                            //判断是否与当前边线的角度相同,如果相同,则以边线两端点的最近一处作为绘制点 2017-12-27
                                            double vx1, vy1, vx2, vy2;
                                            vx1 = vy1 = vx2 = vy2 = 0;
                                            if (ptslist.Count >= 6 && tmplist.Count >= 6)
                                            {
                                                vx1 = (double)ptslist[3] - (double)ptslist[0];
                                                vy1 = (double)ptslist[4] - (double)ptslist[1];
                                                vx2 = (double)tmplist[tmplist.Count - 3] - (double)tmplist[tmplist.Count - 6];
                                                vy2 = (double)tmplist[tmplist.Count - 2] - (double)tmplist[tmplist.Count - 5];
                                                //注意这里的vectorangle返回的是两矢量的弧度值[0..PI] 2017-12-27
                                                double ang = axEWdraw1.VectorAngle(new object[] { vx1, vy1, 0 }, new object[] { vx2, vy2, 0 });
                                                if (ang < 0.001 || Math.Abs(ang - Math.PI) < 0.001)
                                                {
                                                    double tdist1 = axEWdraw1.PointDistance((double)tmplist[tmplist.Count - 6], (double)tmplist[tmplist.Count - 5], 0,
                                                                            (double)ptslist[0], (double)ptslist[1], 0);
                                                    double tdist2 = axEWdraw1.PointDistance((double)tmplist[tmplist.Count - 6], (double)tmplist[tmplist.Count - 5], 0,
                                                                            (double)ptslist[3], (double)ptslist[4], 0);
                                                    if (tdist1 < tdist2)
                                                    {
                                                        tmplist[tmplist.Count - 3] = (double)ptslist[0];
                                                        tmplist[tmplist.Count - 2] = (double)ptslist[1];
                                                        tmplist[tmplist.Count - 1] = (double)ptslist[2];
                                                    }
                                                    else
                                                    {
                                                        tmplist[tmplist.Count - 3] = (double)ptslist[3];
                                                        tmplist[tmplist.Count - 2] = (double)ptslist[4];
                                                        tmplist[tmplist.Count - 1] = (double)ptslist[5];
                                                    }
                                                }
                                            }
                                            //
                                            edgeptjs++;
                                        }
                                    }
                                    else
                                    {
                                        //
                                        connecttype = 5;//闭合状态
                                        if (!IsReturnPath(mcex, mcey, mcez, ref tmplist))
                                        {
                                            //ProcPtsHVByPt(mcex, mcey, mcez, minx,miny,minz, ref tmplist);
                                            tmplist.Add(mcex);
                                            tmplist.Add(mcey);
                                            tmplist.Add(mcez);
                                        }
                                        else return;

                                    }
                                    //根据修正后的点,重设polyline线
                                    int ptssize = tmplist.Count / 3;
                                    int drawptssize = axEWdraw1.GetDrawPolyLineSize() + 1;
                                    if (ptssize < drawptssize)
                                    {
                                        for (int i = 0; i < ptssize; i++)
                                        {
                                            axEWdraw1.SetDrawPolyLinePt(i, (double)tmplist[i * 3], (double)tmplist[i * 3 + 1], (double)tmplist[i * 3 + 2]);
                                        }
                                    }
                                }
                            }
                        }//
                    }
                    else
                        connecttype = 0;
                    //
                    if (connecttype == 0)
                    {
                        //判断水平或垂直
                        //定义根据最后一个点,定义新点
                        if (tmplist.Count >= 1)
                        {
                            double x, y, z, x1, y1, z1;
                            x = y = z = x1 = y1 = z1 = 0.0;
                            x1 = (double)tmplist[tmplist.Count - 3];
                            y1 = (double)tmplist[tmplist.Count - 2];
                            z1 = (double)tmplist[tmplist.Count - 1];
                            //判断当前线段的方向是否在横向与纵向的容差内(10度) 2016-09-21
                            if (drawsegjs > 0)//2017-05-20 修改此处是为了只在非首个点是判断
                            {
                                bool ishvline = true;
                                double vx = mcex - x1;
                                double vy = mcey - y1;
                                double vz = mcez - z1;
                                double tang1 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 1, 0, 0 }) / Math.PI * 180.0;
                                double tang2 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { -1, 0, 0 }) / Math.PI * 180.0;
                                double tang3 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 0, 1, 0 }) / Math.PI * 180.0;
                                double tang4 = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { 0, -1, 0 }) / Math.PI * 180.0;
                                if (((tang1 > 10 && tang1 < 45) || Math.Abs(tang1 - 45.0) < 0.001) ||
                                     ((tang2 > 10 && tang2 < 45) || Math.Abs(tang2 - 45.0) < 0.001) ||
                                     ((tang3 > 10 && tang3 < 45) || Math.Abs(tang3 - 45.0) < 0.001) ||
                                    ((tang4 > 10 && tang4 < 45) || Math.Abs(tang4 - 45.0) < 0.001)
                                    )
                                {
                                    ishvline = false;
                                }
                                //
                                if (ishvline)
                                {
                                    if (Math.Abs(mcex - x1) > 0.0001 || Math.Abs(mcey - y1) > 0.0001)
                                    {
                                        if (Math.Abs(mcex - x1) < Math.Abs(mcey - y1)) // 0.0001 && > 0.0001
                                        {
                                            //垂直连接
                                            x = x1;
                                            y = mcey;
                                            z = mcez;
                                            if (!IsReturnPath(x, y, z, ref tmplist))
                                            {
                                                tmplist.Add(x);
                                                tmplist.Add(y);
                                                tmplist.Add(z);
                                            }
                                            else return;

                                        }
                                        else if (Math.Abs(mcey - y1) < Math.Abs(mcex - x1))
                                        {
                                            //水平连接
                                            x = mcex;
                                            y = y1;
                                            z = mcez;
                                            if (!IsReturnPath(x, y, z, ref tmplist))
                                            {
                                                tmplist.Add(x);
                                                tmplist.Add(y);
                                                tmplist.Add(z);
                                            }
                                            else return;
                                        }
                                        else if (!IsReturnPath(mcex, mcey, mcez, ref tmplist))
                                        {
                                            tmplist.Add(mcex);
                                            tmplist.Add(mcey);
                                            tmplist.Add(mcez);
                                        }
                                    }
                                }
                                else if (!IsReturnPath(mcex, mcey, mcez, ref tmplist))
                                {
                                    tmplist.Add(mcex);
                                    tmplist.Add(mcey);
                                    tmplist.Add(mcez);
                                }
                                else
                                    return;//两点相同,不处理
                            }
                        }
                        else
                        {
                            if (!IsReturnPath(mcex, mcey, mcez, ref tmplist))
                            {
                                double gx, gy, gz;
                                gx = gy = gz = 0.0;
                                //段起始的第一点,如果不在其它实体上,则尝试捕获到grid上.
                                axEWdraw1.ConvertToGrid(mcex, mcey, mcez, ref gx, ref gy, ref gz);
                                axEWdraw1.SetDrawPolyLinePt(0, gx, gy, gz);
                                tmplist.Add(gx);
                                tmplist.Add(gy);
                                tmplist.Add(gz);
                            }
                        }
                    }
                    else if ((connecttype == 1 || connecttype == 2) && drawsegjs >= 1)
                    {//进入该条件的的线有一点与现有实体的起点或终点相连
                        if (!IsReturnPath(mcex, mcey, mcez, ref tmplist))
                        {
                            tmplist.Add(mcex);
                            tmplist.Add(mcey);
                            tmplist.Add(mcez);
                        }
                        else return;
                    }
                    drawsegjs++;
                    //将下一步处理放在定时器内,之所以这样作是因为在事件中要尽可能避免大量的操作,避免阻塞
                    timer1.Enabled = true;
                }
                else
                {
                    //判断是否选中墙,是否移墙 2017-03-15
                    if (e.button == 1)
                    {
                        if (g_tmpjs >= 2 && isdrawmove)
                        {
                            int wx1, wy1, wx2, wy2;
                            wx1 = wy1 = wx2 = wy2 = 0;
                            //2017-01-21
                            g_mx = mcex;
                            g_my = mcey;
                            g_mz = mcez;
                            //
                            axEWdraw1.Coordinate2Screen(g_bx, g_by, g_bz, ref wx1, ref wy1);
                            axEWdraw1.Coordinate2Screen(g_x, g_y, g_z, ref wx2, ref wy2);
                            if (isdrawmove)
                            {
                                if (g_st > 1 && (Math.Abs(wx2 - wx1) > 2 || Math.Abs(wy2 - wy1) > 2))
                                {
                                    g_st = 0;
                                    axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                                    isdrawmove = false;
                                    timer2.Enabled = false;
                                }
                                else
                                {
                                    timer4.Enabled = false;
                                    g_st = 0;
                                    isdrawmove = false;
                                    timer2.Enabled = false;
                                    axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                                }
                            }
                        }
                        else
                        {
                            ////判断是否选中了墙,是否要启动的拖动墙 2017-03-15
                            if (g_viewmode == 0)
                            {
                                movewalljs = 0;
                                timer4.Enabled = true;
                                g_mx = g_x;
                                g_my = g_y;
                                g_mz = g_z;
                            }
                            else
                            {//2018-01-18
                                if (!isdrawrotate)
                                {
                                    movewalljs = 0;
                                    timer4.Enabled = true;
                                    g_mx = g_x;
                                    g_my = g_y;
                                    g_mz = g_z;
                                }
                            }
                        }
                    }
                    else if (e.button == 2)//2018-01-22
                    {
                        if (isdrawaxismove)
                        {
                            if (!axEWdraw1.IsEndCommand())
                            {
                                axEWdraw1.CancelCommand();
                                isdrawaxismove = false;
                            }
                        }
                    }
                }
            }
        }

        //生成墙的操作
        private void timer1_Tick(object sender, EventArgs e)
        {
            //先将定时器停止
            timer1.Enabled = false;
            //如果之前用临时对象,则删除
            if (tmpent > 0 || tmppipe > 0)
            {
                axEWdraw1.Delete(tmpent);
                axEWdraw1.Delete(tmppipe);
            }
            if ((connecttype == 1 || connecttype == 2 || connecttype == 4) && drawsegjs == 1)
            {//如果第一个点是在另一条线的起点或终点上
                wantdelent = connectent;
            }
            //如果超过两点,则处理
            if (drawsegjs >= 2)
            {
                if (!IsCloseAndOutPts(ref tmplist, ref tlist0, ref tlist1))
                {
                    //得到点的个数
                    int size = tmplist.Count / 3;
                    //创建polyline线
                    double x, y, z, x1, y1, z1, x2, y2, z2;
                    x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                    axEWdraw1.Clear3DPtBuf();
                    for (int i = 0; i < size; i++)
                    {
                        x = (double)tmplist[i * 3];
                        y = (double)tmplist[i * 3 + 1];
                        z = (double)tmplist[i * 3 + 2];
                        axEWdraw1.AddOne3DVertex(x, y, z);
                    }
                    if (!IsClosePts(ref tmplist))
                        tmpent = axEWdraw1.PolyLine3D(false);
                    else
                        tmpent = axEWdraw1.PolyLine3D(true);
                    axEWdraw1.Clear3DPtBuf();
                    //取得第一个段的方向
                    double vx, vy, vz;
                    vx = vy = vz = 0.0;
                    int tmpinx = 0;
                    axEWdraw1.GetWallSegVector(tmpent, 0, new object[] { (double)tmplist[tmpinx * 3], (double)tmplist[tmpinx * 3 + 1], (double)tmplist[tmpinx * 3 + 2] }, ref vx, ref vy, ref vz);
                    //创建截面
                    x1 = (double)tmplist[0];
                    y1 = (double)tmplist[1];
                    z1 = (double)tmplist[2];
                    x2 = (double)tmplist[3];
                    y2 = (double)tmplist[4];
                    z2 = (double)tmplist[5];
                    //最后两个参数分别是墙宽与墙高
                    int section = 0;
                    section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);
                    //创建Pipe对象
                    axEWdraw1.SetPipeAlign(false);
                    //底面
                    tmppipe = axEWdraw1.Pipe(tmpent, section);
                    //
                    SetPtsToSolid(tmppipe, ref tmplist);
                    SetThicknessToSolid(tmppipe, g_wallthickness);//设置墙的厚度 2016-11-03
                    SetHeightToSolid(tmppipe, g_wallheight);//设置墙的厚度 2016-11-03
                    axEWdraw1.SetPipeAlign(true);
                    //删除的截面
                    axEWdraw1.Delete(section);
                }
                else
                {
                    //-------------------------分割后的第一段-------------------------------------------
                    //得到点的个数
                    int size = tlist0.Count / 3;
                    //创建polyline线
                    double x, y, z, x1, y1, z1, x2, y2, z2;
                    x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                    axEWdraw1.Clear3DPtBuf();
                    if (IsClosePts(ref tlist0))
                    {
                        for (int i = 0; i < size - 1; i++)
                        {
                            x = (double)tlist0[i * 3];
                            y = (double)tlist0[i * 3 + 1];
                            z = (double)tlist0[i * 3 + 2];
                            axEWdraw1.AddOne3DVertex(x, y, z);
                        }
                        axEWdraw1.AddOne3DVertex((double)tlist0[0], (double)tlist0[1], (double)tlist0[2]);
                        tmpent = axEWdraw1.PolyLine3D(true);
                    }
                    else
                    {
                        for (int i = 0; i < size; i++)
                        {
                            x = (double)tlist0[i * 3];
                            y = (double)tlist0[i * 3 + 1];
                            z = (double)tlist0[i * 3 + 2];
                            axEWdraw1.AddOne3DVertex(x, y, z);
                        }
                        tmpent = axEWdraw1.PolyLine3D(false);
                    }
                    axEWdraw1.Clear3DPtBuf();
                    //创建截面
                    x1 = (double)tlist0[0];
                    y1 = (double)tlist0[1];
                    z1 = (double)tlist0[2];
                    x2 = (double)tlist0[3];
                    y2 = (double)tlist0[4];
                    z2 = (double)tlist0[5];
                    //判断是否有弧点,如果有则得到第一段方向后计算截面
                    int section = 0;
                    section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);//最后两个参数分别是墙宽与墙高
                    //创建Pipe对象
                    axEWdraw1.SetPipeAlign(false);
                    tmppipe = axEWdraw1.Pipe(tmpent, section);
                    axEWdraw1.SetEntAbsorb(tmppipe);//2016-09-08
                    //设置点集到墙的实体中
                    SetPtsToSolid(tmppipe, ref tlist0);
                    SetThicknessToSolid(tmppipe, g_wallthickness);//设置墙的厚度 2016-11-03
                    SetHeightToSolid(tmppipe, g_wallheight);//设置墙的厚度 2016-11-03
                    axEWdraw1.SetPipeAlign(true);
                    //删除的截面
                    axEWdraw1.Delete(section);
                    axEWdraw1.Delete(tmpent);
                    tmpent = 0;
                    //-------------------------分割后的第二段-------------------------------------------
                    size = tlist1.Count / 3;
                    //创建polyline线
                    axEWdraw1.Clear3DPtBuf();
                    if (IsClosePts(ref tlist1))
                    {
                        for (int i = 0; i < size - 1; i++)
                        {
                            x = (double)tlist1[i * 3];
                            y = (double)tlist1[i * 3 + 1];
                            z = (double)tlist1[i * 3 + 2];
                            axEWdraw1.AddOne3DVertex(x, y, z);
                        }
                        axEWdraw1.AddOne3DVertex((double)tlist0[0], (double)tlist0[1], (double)tlist0[2]);
                        tmpent = axEWdraw1.PolyLine3D(true);
                    }
                    else
                    {
                        for (int i = 0; i < size; i++)
                        {
                            x = (double)tlist1[i * 3];
                            y = (double)tlist1[i * 3 + 1];
                            z = (double)tlist1[i * 3 + 2];
                            axEWdraw1.AddOne3DVertex(x, y, z);
                        }
                        tmpent = axEWdraw1.PolyLine3D(false);
                    }
                    axEWdraw1.Clear3DPtBuf();
                    //创建截面
                    x1 = (double)tlist1[0];
                    y1 = (double)tlist1[1];
                    z1 = (double)tlist1[2];
                    x2 = (double)tlist1[3];
                    y2 = (double)tlist1[4];
                    z2 = (double)tlist1[5];
                    section = MakeSection(x1, y1, z1, x2, y2, z2, g_wallthickness, g_wallheight);//最后两个参数分别是墙宽与墙高
                    //创建Pipe对象
                    axEWdraw1.SetPipeAlign(false);
                    ////底面
                    tmppipe1 = axEWdraw1.Pipe(tmpent, section);
                    //设置点集到墙的实体中
                    SetPtsToSolid(tmppipe1, ref tlist1);
                    SetThicknessToSolid(tmppipe, g_wallthickness);//设置墙的厚度 2016-11-03
                    SetHeightToSolid(tmppipe, g_wallheight);//设置墙的厚度 2016-11-03
                    axEWdraw1.SetPipeAlign(true);
                    axEWdraw1.SetEntAbsorb(tmppipe1);//2016-09-08
                    //删除的截面
                    axEWdraw1.Delete(section);
                    axEWdraw1.Delete(tmpent);
                    tmpent = 0;
                    axEWdraw1.CancelCommand();
                }
                if (connecttype == 4 || connecttype == 6)
                {
                    axEWdraw1.CancelCommand();
                }
                //如果在画某段线时,有两次点击了边线,则说明它是一个闭合线,可以结束.
                if (edgeptjs >= 2)
                {
                    //
                    axEWdraw1.CancelCommand();
                    if (tmppipe > 0)
                    {
                        axEWdraw1.SplitWall(tmppipe);
                        int swsize = axEWdraw1.GetSingleWallIDBufferSize();
                        for (int i = 0; i < swsize; i++)
                        {
                            int id = axEWdraw1.GetSingleWallID(i);
                            axEWdraw1.SetEntAbsorb(id);
                        }
                        //
                        MakeHomeArea(g_wallthickness);
                        axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                        swsize = axEWdraw1.GetSingleWallIDBufferSize();
                        for (int i = 0; i < swsize; i++)
                        {
                            int id = axEWdraw1.GetSingleWallID(i);
                            axEWdraw1.SetEntAbsorb(id);
                        }
                        DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                        axEWdraw1.Delete(tmppipe);
                        tmppipe = 0;
                    }
                }
            }
        }

        //删除单墙体 2016-11-26
        private void DelSingleWall(int id, int inx)
        {
            ArrayList opts = new ArrayList();
            double x, y, z;
            x = y = z = 0.0;
            int id1 = 0;
            int id2 = 0;
            int deltype = 0;
            double minx, miny, minz;
            minx = miny = minz = 0.0;
            //结束当前未完的命令
            if (id > 0 && inx >= 0)
            {
                GetPtsFromSolid(id, ref opts);
                //删除墙段
                deltype = DelSingleWall(id, inx, ref opts, ref id1, ref id2);
                if (id > 0)
                {
                    DelDimWalls(id);
                    DelWeightWall(id); //删除重力墙2016-10-13
                    //判断删除的是否是首部或尾部 2016-11-07
                    int tmplen = opts.Count / 3;
                    if (tmplen >= 3)
                    {
                        if (inx == 0 || inx == (tmplen - 2))
                        {
                            if (inx == 0)
                            {
                                DelPtData(id, (double)opts[0], (double)opts[1], (double)opts[2]);
                            }
                            else
                            {
                                DelPtData(id, (double)opts[opts.Count - 3], (double)opts[opts.Count - 2], (double)opts[opts.Count - 1]);
                            }
                        }
                    }
                    else if (tmplen == 2)
                    {
                        DelPtData(id, (double)opts[0], (double)opts[1], (double)opts[2]);
                        DelPtData(id, (double)opts[3], (double)opts[4], (double)opts[5]);
                    }
                    //
                }
                if (id1 > 0)
                {
                    DimWalls(id1, "Arial", 150, 0, 0, 0, g_wallthickness * 3.0, g_maxz);//2016-09-18
                    DelWeightWall(id1); //2016-10-13
                }
                if (id2 > 0)
                {
                    DimWalls(id2, "Arial", 150, 0, 0, 0, g_wallthickness * 3.0, g_maxz);//2016-09-18
                    DelWeightWall(id2); //2016-10-13
                }
                //判断是否还有墙面
                int pipejs = 0;
                int entsize = axEWdraw1.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    tmpent = axEWdraw1.GetEntID(i);
                    int enttype = axEWdraw1.GetEntType(tmpent);
                    if (enttype == 59)
                    {
                        pipejs++;
                    }
                }
                if (pipejs > 0)
                {
                    MakeHomeArea(g_wallthickness);
                }
            }
        }
        private void axEWdraw1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool isvalid = false;
            if (!textBox1.Focused || !textBox1.Visible || !textBox1.Enabled)
            {
                //MessageBox.Show(e.KeyCode.ToString());
                switch (e.KeyCode)
                {
                    case Keys.D0:
                        textBox1.Text = "0";
                        isvalid = true;
                        break;
                    case Keys.D1:
                        textBox1.Text = "1";
                        isvalid = true;
                        break;
                    case Keys.D2:
                        textBox1.Text = "2";
                        isvalid = true;
                        break;
                    case Keys.D3:
                        textBox1.Text = "3";
                        isvalid = true;
                        break;
                    case Keys.D4:
                        textBox1.Text = "4";
                        isvalid = true;
                        break;
                    case Keys.D5:
                        textBox1.Text = "5";
                        isvalid = true;
                        break;
                    case Keys.D6:
                        textBox1.Text = "6";
                        isvalid = true;
                        break;
                    case Keys.D7:
                        textBox1.Text = "7";
                        isvalid = true;
                        break;
                    case Keys.D8:
                        textBox1.Text = "8";
                        isvalid = true;
                        break;
                    case Keys.D9:
                        textBox1.Text = "9";
                        isvalid = true;
                        break;
                    case Keys.Decimal:
                    case Keys.OemPeriod:
                        textBox1.Text = ".";
                        isvalid = true;
                        break;
                    case Keys.Subtract:
                    case Keys.OemMinus:
                        textBox1.Text = "-";
                        isvalid = true;
                        break;
                    case Keys.ProcessKey://2021-09-15
                        textBox1.Text = "";
                        isvalid = true;
                        break;
                }
                
                if (isvalid && (isdrawpolyline || isdrawabsorb || isdrawbalcony || isdrawmove || isdrawrotate))//2021-09-15
                {
                    textBox1.Visible = true;
                    textBox1.Top = g_dimposy + axEWdraw1.Top - textBox1.Height;
                    textBox1.Left = axEWdraw1.Left + g_dimposx - 10;
                    textBox1.Focus();
                    //设置光标的位置到文本尾 
                    textBox1.Select(textBox1.TextLength, 0);
                    //滚动到控件光标处 
                    textBox1.ScrollToCaret();
                    old_dimposx = g_dimposx;
                    old_dimposy = g_dimposy;
                    isenterchar = true;
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (g_state == InteractiveState.Nothing)
                {
                    if (axEWdraw1.Focused)
                    {
                        //判断是否有已经选择的实体
                        int selentsize = axEWdraw1.GetSelectEntSize();
                        for (int i = 0; i < selentsize; i++)
                        {
                            int ent = axEWdraw1.GetSelectEnt(i);
                            int enttype = axEWdraw1.GetEntType(ent);
                            if (enttype == 66)
                            {//2016-11-26 单墙
                                //取得要删除墙的信息
                                int singlewallsize = axEWdraw1.GetSingleWallSize();
                                if (singlewallsize > 0)
                                {
                                    int singleid = 0;
                                    int singledid = 0;
                                    int singleorgid = 0;
                                    int singleinx = -1;
                                    double sx, sy, sz, ex, ey, ez;
                                    double sx1, sy1, sz1, ex1, ey1, ez1;
                                    sx = sy = sz = ex = ey = ez = 0.0;
                                    sx1 = sy1 = sz1 = ex1 = ey1 = ez1 = 0.0;


                                    double ox, oy, oz;
                                    ox = oy = oz = 0.0;
                                    double dist = 0;
                                    for (int j = 0; j < singlewallsize; j++)
                                    {
                                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                                        if (singleid == ent)
                                        {
                                            break;
                                        }
                                    }
                                    axEWdraw1.DelSingleWall(ent);
                                    DelDimWalls(ent);//删除与墙关联的标注 2017-08-17
                                    //判断与要删除墙起点相关的墙面
                                    MakeHomeArea(g_wallthickness);
                                    axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                                    singlewallsize = axEWdraw1.GetSingleWallSize();
                                    int tmpjs = 0;
                                    int ent1, ent2;
                                    ent1 = ent2 = 0;
                                    bool isunion = false;
                                    //axEWdraw1.Point(new object[]{sx, sy, 3000});
                                    for (int j = 0; j < singlewallsize; j++)
                                    {
                                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx1, ref sy1, ref sz1, ref ex1, ref ey1, ref ez1);
                                        if (axEWdraw1.PointDistance(sx, sy, sz, sx1, sy1, sz1) < 0.001 ||
                                            axEWdraw1.PointDistance(sx, sy, sz, ex1, ey1, ez1) < 0.001
                                            )
                                        {
                                            if (tmpjs == 0)
                                            {
                                                ent1 = singleid;
                                            }
                                            else if (tmpjs >= 1)
                                            {
                                                ent2 = singleid;
                                                break;
                                            }
                                            tmpjs++;
                                        }
                                    }

                                    if (ent1 > 0 && ent2 > 0)
                                    {
                                        axEWdraw1.ClearSelected();

                                        axEWdraw1.UnionSingleWall(ent1, ent2);
                                        isunion = true;
                                    }
                                    if (isunion)
                                    {
                                        axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                                        ReDrawHomeArea(g_wallthickness);
                                        DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                                    }
                                    isunion = false;
                                    ent1 = ent2 = 0;
                                    tmpjs = 0;
                                    for (int j = 0; j < singlewallsize; j++)
                                    {
                                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx1, ref sy1, ref sz1, ref ex1, ref ey1, ref ez1);
                                        if (axEWdraw1.PointDistance(ex, ey, ez, sx1, sy1, sz1) < 0.001 ||
                                            axEWdraw1.PointDistance(ex, ey, ez, ex1, ey1, ez1) < 0.001
                                            )
                                        {
                                            if (tmpjs == 0)
                                            {
                                                ent1 = singleid;
                                            }
                                            else if (tmpjs >= 1)
                                            {
                                                ent2 = singleid;
                                                break;
                                            }
                                            tmpjs++;
                                        }
                                    }

                                    if (ent1 > 0 && ent2 > 0)
                                    {
                                        axEWdraw1.UnionSingleWall(ent1, ent2);
                                        isunion = true;
                                    }

                                    if (isunion)
                                    {
                                        axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                                        ReDrawHomeArea(g_wallthickness);
                                        DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                                    }
                                    break;
                                }
                            }
                            else if (axEWdraw1.IsGroup(ent))
                            {
                                //得到组名
                                string grpname = axEWdraw1.GetGroupName(ent);
                                //判断是否是符号组
                                if (grpname.IndexOf("symbol") >= 0)
                                {//如果是,则删除
                                    axEWdraw1.RemoveSymbolFromWall(ent);
                                }
                                else
                                {
                                    if (IsFurniture(grpname))
                                    {
                                        if (!axEWdraw1.IsEndCommand())
                                        {
                                            axEWdraw1.CancelCommand();
                                            if (isdrawaxismove)
                                                isdrawaxismove = false;
                                        }

                                        axEWdraw1.Delete(ent);
                                    }
                                }
                            }
                            //
                        }
                    }
                }
            }

        }

        /*IsReturnPath 判断是否是无效折返路径
         * 参数:
         * x,y,z  输入  给出的点(当前点击的点)
         * list   输入  给出的当前画线时的点数组
         * 返回值:
         * 如果是无效的的折返路径,则返回的true,其它返回false
         */
        private bool IsReturnPath(double x, double y, double z, ref ArrayList list)
        {
            double dx1 = 0;
            double dy1 = 0;
            double dz1 = 0;
            if (list.Count >= 3)
            {
                dx1 = x - (double)tmplist[list.Count - 3];
                dy1 = y - (double)tmplist[tmplist.Count - 2];
                dz1 = z - (double)tmplist[tmplist.Count - 1];
            }
            else return false;
            if (list.Count >= 6)
            {
                double dx2 = (double)list[list.Count - 3] - (double)list[list.Count - 6];
                double dy2 = (double)list[list.Count - 2] - (double)list[list.Count - 5];
                double dz2 = (double)list[list.Count - 1] - (double)list[list.Count - 4];
                double ang = axEWdraw1.VectorAngle(new object[] { dx1, dy1, dz1 }, new object[] { dx2, dy2, dz2 });
                if (Math.Abs(ang - Math.PI) < 0.0001)
                    return true;
            }
            return false;
        }
        /*IsClosePts 判断点集是否是一个闭合体
         * 参数:
         * list  输入 要判断的点数组
         * 返回值:
         * 如果是闭合的返回true,其它返回false.
         */
        private bool IsClosePts(ref ArrayList list)
        {
            int tmpptsize = list.Count / 3;
            if (tmpptsize >= 4)
            {
                double x1, y1, z1, x2, y2, z2;
                x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                x1 = (double)list[0];
                y1 = (double)list[1];
                z1 = (double)list[2];
                //
                x2 = (double)list[list.Count - 3];
                y2 = (double)list[list.Count - 2];
                z2 = (double)list[list.Count - 1];
                if (axEWdraw1.PointDistance(x1, y1, z1, x2, y2, z2) < 0.001)
                    return true;
            }
            return false;
        }

        /*判断线段点集是否是闭合后又多出一部分
         * list    [输入]  数据点集
         * list0   [输出]  分割后的第一部分的点集
         * list1   [输出]  分割后的第二部分的点集(如果有的话)
         */
        private bool IsCloseAndOutPts(ref ArrayList list, ref ArrayList list0, ref ArrayList list1)
        {
            int tmpptsize = list.Count / 3;
            if (tmpptsize >= 4)
            {
                double x1, y1, z1, x2, y2, z2;
                x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                x1 = (double)list[0];
                y1 = (double)list[1];
                z1 = (double)list[2];
                //
                x2 = (double)list[list.Count - 3];
                y2 = (double)list[list.Count - 2];
                z2 = (double)list[list.Count - 1];
                if (axEWdraw1.PointDistance(x1, y1, z1, x2, y2, z2) < 0.001)
                    return false;
                else
                {
                    //最后一点与之前的点形成闭合
                    for (int i = 1; i < tmpptsize - 1; i++)
                    {
                        x1 = (double)list[i * 3];
                        y1 = (double)list[i * 3 + 1];
                        z1 = (double)list[i * 3 + 2];
                        if (axEWdraw1.PointDistance(x1, y1, z1, x2, y2, z2) < 0.001)
                        {
                            double x, y, z;
                            x = y = z = 0;
                            list0.Clear();
                            list1.Clear();
                            //
                            for (int j = i; j >= 0; j--)
                            {
                                x = (double)list[j * 3];
                                y = (double)list[j * 3 + 1];
                                z = (double)list[j * 3 + 2];
                                list1.Add(x);
                                list1.Add(y);
                                list1.Add(z);
                            }
                            //
                            for (int j = tmpptsize - 1; j >= i; j--)
                            {
                                x = (double)list[j * 3];
                                y = (double)list[j * 3 + 1];
                                z = (double)list[j * 3 + 2];
                                list0.Add(x);
                                list0.Add(y);
                                list0.Add(z);
                            }
                            return true;
                        }
                    }
                    //第一点与之后的点形的闭合
                    x1 = (double)list[0];
                    y1 = (double)list[1];
                    z1 = (double)list[2];
                    //
                    for (int i = 1; i < tmpptsize; i++)
                    {
                        x2 = (double)list[i * 3];
                        y2 = (double)list[i * 3 + 1];
                        z2 = (double)list[i * 3 + 2];
                        if (axEWdraw1.PointDistance(x1, y1, z1, x2, y2, z2) < 0.001)
                        {
                            double x, y, z;
                            x = y = z = 0;
                            list0.Clear();
                            list1.Clear();
                            for (int j = 0; j <= i; j++)
                            {
                                x = (double)list[j * 3];
                                y = (double)list[j * 3 + 1];
                                z = (double)list[j * 3 + 2];
                                list0.Add(x);
                                list0.Add(y);
                                list0.Add(z);
                            }
                            for (int j = i; j < tmpptsize; j++)
                            {
                                x = (double)list[j * 3];
                                y = (double)list[j * 3 + 1];
                                z = (double)list[j * 3 + 2];
                                list1.Add(x);
                                list1.Add(y);
                                list1.Add(z);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /*ProcPtsHVByPt 处理点集在给定点条件下在某点之前线段的横向或竖向值
         * 参数:
         * orgx,orgy,orgz 输入        为当前点击的点值(可视情况设置)
         * minx,miny,minz 输入        为当前最近点(可视情况设置)
         * list           输入/输出   输入当前绘制的线段的点集,根据条件设定数组中某段的横向或竖向值
         */
        private void ProcPtsHVByPt(double orgx, double orgy, double ogrz,
                                double minx, double miny, double minz,
                                ref ArrayList list)
        {
            //查找是否与ptlist中的点相近
            if (Math.Abs(orgy - (double)tmplist[list.Count - 2]) < Math.Abs(orgx - (double)list[tmplist.Count - 3]))
            {//水平
                //判断角度
                if (Math.Abs(Math.Abs(orgy - (double)list[list.Count - 2]) - Math.Abs(miny - (double)list[list.Count - 2])) > 0.001)
                {
                    int tmpptsize = list.Count / 3 - 1;
                    if (tmpptsize == 0)
                    {
                        list[list.Count - 2] = miny;
                    }
                    else
                    {
                        for (int i = tmpptsize; i >= 1; i--)
                        {
                            if (Math.Abs((double)list[i * 3 + 1] - (double)list[(i - 1) * 3 + 1]) < Math.Abs((double)list[i * 3] - (double)list[(i - 1) * 3]))
                            {
                                list[i * 3 + 1] = miny;
                                list[(i - 1) * 3 + 1] = miny;
                            }
                            else break;//遇到第一个不同方向的则直接退出
                        }
                    }
                    //判断当前的
                    tmplist[tmplist.Count - 2] = miny;
                }
            }
            else if (Math.Abs(orgx - (double)list[list.Count - 3]) < Math.Abs(orgy - (double)list[list.Count - 2]))
            {//垂直
                //判断角度
                if (Math.Abs(Math.Abs(orgx - (double)tmplist[tmplist.Count - 3]) - Math.Abs(minx - (double)tmplist[tmplist.Count - 3])) > 0.001)
                {
                    int tmpptsize = list.Count / 3 - 1;
                    if (tmpptsize == 0)
                    {
                        list[list.Count - 3] = minx;
                    }
                    else
                    {
                        for (int i = tmpptsize; i >= 1; i--)
                        {
                            if (Math.Abs((double)list[i * 3] - (double)list[(i - 1) * 3]) < Math.Abs((double)list[i * 3 + 1] - (double)list[(i - 1) * 3 + 1]))
                            {
                                list[i * 3] = minx;
                                list[(i - 1) * 3] = minx;
                            }
                            else break;//遇到第一个不同方向的则直接退出
                        }
                    }
                    //判断当前的
                    tmplist[tmplist.Count - 3] = minx;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                isenterchar = false;
                if (textBox1.Text.Length > 0)//2021-09-15
                {
                    double val = Convert.ToDouble(textBox1.Text);
                    if (Math.Abs(val) > 0.0000001)
                    {
                        axEWdraw1.SendCommandStr(textBox1.Text);
                    }
                }
                textBox1.Visible = false;
            }
            else
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }
        /*SetHoleMode设置显示方式
         * 参数:
         * mode  输入  0表示2D图方式(俯视) 1表示透明墙洞方式 2表示家具实体方式
         * 返回值:
         * 无
         */
        private void SetHoleMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    {
                        //俯视图
                        if (subtractidlist.Count > 0)
                        {
                            for (int i = 0; i < subtractidlist.Count; i++)
                                axEWdraw1.Delete((int)subtractidlist[i]);
                        }
                        if (cpidlist.Count > 0)
                        {
                            for (int i = 0; i < cpidlist.Count; i++)
                                axEWdraw1.Delete((int)cpidlist[i]);
                        }
                        if (orgidlist.Count > 0)
                        {
                            for (int i = 0; i < orgidlist.Count; i++)
                            {
                                //判断是否是原数据,如果是59的pipe原对象,则不显示2016-12-20
                                int enttype = axEWdraw1.GetEntType((int)orgidlist[i]);
                                if (enttype != 59)
                                    axEWdraw1.SetEntityInvisible((int)orgidlist[i], false);
                                else
                                    axEWdraw1.SetEntityInvisible((int)orgidlist[i], true);
                            }
                        }
                        if (symbolidlist.Count > 0)
                        {
                            for (int i = 0; i < symbolidlist.Count; i++)
                            {
                                axEWdraw1.SetEntityInvisible((int)symbolidlist[i], false);
                            }
                        }
                        //删除已经插入的门 2016-08-08
                        if (door3dsidlist.Count > 0)
                        {
                            for (int i = 0; i < door3dsidlist.Count; i++)
                            {
                                axEWdraw1.Delete((int)door3dsidlist[i]);
                            }
                            door3dsidlist.Clear();
                        }
                    }
                    break;
                case 1:
                    {
                        if (subtractidlist.Count > 0)
                        {
                            for (int i = 0; i < subtractidlist.Count; i++)
                                axEWdraw1.Delete((int)subtractidlist[i]);
                        }
                        if (cpidlist.Count > 0)
                        {
                            for (int i = 0; i < cpidlist.Count; i++)
                                axEWdraw1.Delete((int)cpidlist[i]);
                        }
                        if (orgidlist.Count > 0)
                        {
                            for (int i = 0; i < orgidlist.Count; i++)
                                axEWdraw1.SetEntityInvisible((int)orgidlist[i], false);
                        }
                        symbolidlist.Clear();
                        cpidlist.Clear();
                        orgidlist.Clear();
                        //
                        //
                        CopyWall();
                        if (orgidlist.Count > 0)
                        {
                            for (int i = 0; i < orgidlist.Count; i++)
                                axEWdraw1.SetEntityInvisible((int)orgidlist[i], true);
                        }
                        MakeAllHole();
                        if (symbolidlist.Count > 0)
                        {
                            for (int i = 0; i < symbolidlist.Count; i++)
                                axEWdraw1.SetEntityInvisible((int)symbolidlist[i], true);
                        }

                        if (subtractidlist.Count > 0)
                        {//减洞实体,设置透明度
                            for (int i = 0; i < subtractidlist.Count; i++)
                            {
                                axEWdraw1.SetTransparency((int)subtractidlist[i], 0.7);
                            }
                        }
                        if (cpidlist.Count > 0)
                        {//即使没有减洞,也设置透明度
                            for (int i = 0; i < cpidlist.Count; i++)
                            {
                                axEWdraw1.SetTransparency((int)cpidlist[i], 0.7);
                            }
                        }
                        //删除已经插入的门 2016-08-08
                        if (door3dsidlist.Count > 0)
                        {
                            for (int i = 0; i < door3dsidlist.Count; i++)
                            {
                                axEWdraw1.Delete((int)door3dsidlist[i]);
                            }
                            door3dsidlist.Clear();
                        }
                    }
                    break;
                case 2:
                    {
                        if (subtractidlist.Count > 0)
                        {
                            for (int i = 0; i < subtractidlist.Count; i++)
                                axEWdraw1.Delete((int)subtractidlist[i]);
                        }
                        if (cpidlist.Count > 0)
                        {
                            for (int i = 0; i < cpidlist.Count; i++)
                                axEWdraw1.Delete((int)cpidlist[i]);
                        }
                        if (orgidlist.Count > 0)
                        {
                            for (int i = 0; i < orgidlist.Count; i++)
                                axEWdraw1.SetEntityInvisible((int)orgidlist[i], false);
                        }
                        //删除已经插入的门 2016-08-08
                        if (door3dsidlist.Count > 0)
                        {
                            for (int i = 0; i < door3dsidlist.Count; i++)
                            {
                                axEWdraw1.Delete((int)door3dsidlist[i]);
                            }

                        }
                        //
                        symbolidlist.Clear();
                        cpidlist.Clear();
                        orgidlist.Clear();
                        door3dsidlist.Clear();
                        //
                        //
                        CopyWall();
                        if (orgidlist.Count > 0)
                        {
                            for (int i = 0; i < orgidlist.Count; i++)
                                axEWdraw1.SetEntityInvisible((int)orgidlist[i], true);
                        }
                        MakeAllHole();
                        if (symbolidlist.Count > 0)
                        {
                            for (int i = 0; i < symbolidlist.Count; i++)
                            {
                                int ent = (int)symbolidlist[i];
                                if (axEWdraw1.IsGroup(ent))
                                {
                                    double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                    double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                    orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                    orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                    //取得宽度
                                    double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                    //取得厚度
                                    double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                    //取得高度
                                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                    //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                    //也就是说,这些参数与3ds文件是一对一的关系.
                                    //2018-11-14
                                    string grpname = axEWdraw1.GetGroupName(ent);
                                    string oname = "";
                                    string filename = GetSimpleModeFromGrpName(grpname, ref subclassitems, ref oname);
                                    string ngrpname = "door_" + oname;
                                    //"m1.3ds","door_1"
                                    int grpid = Import3DsDoor(filename, ngrpname, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, thickness, height, true);
                                    if (grpid > 0)
                                    {

                                        axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                        axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                        new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                        SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                        SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                        //增加到数组中
                                        door3dsidlist.Add(grpid);
                                    }
                                }
                                axEWdraw1.SetEntityInvisible((int)symbolidlist[i], true);
                            }
                        }
                    }
                    break;
            }
        }
        /*MakeAllHole函数 遍历所有实体,找到已定义符号,并创建为墙洞
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private int MakeAllHole()
        {
            int tmpjs = 0;//记录创建洞的个数
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname = axEWdraw1.GetGroupName(ent);
                        int inx = grpname.IndexOf("symbol");
                        if (inx >= 0)
                        {//如果是符号组
                            string[] strarr = grpname.Split('_');
                            if (strarr[1] == "door")
                            {//判断是否是门
                                symbolidlist.Add(ent);
                                MakeDoorHole(ent, ref cpidlist);
                                tmpjs++;
                            }
                        }
                    }
                    else
                    {//2016-12-16
                        int type = axEWdraw1.GetEntType(ent);
                        if (type == 66 || type == 68)
                        {
                            singlewallids.Add(ent);
                            axEWdraw1.SetEntityInvisible(ent, true);
                        }
                    }
                }
            }
            return tmpjs;
        }
        //MakeDoorHole 根据一个符号(以组为单位),创建一个洞
        /*参数:
         * door     输入  门的ID号
         * idlist   输入  需要减去门洞的墙的ID数组集
         * 返回值:
         * 成功则返回带门洞的墙,其它返回0.
         */
        private int MakeDoorHole(int door, ref ArrayList idlist)
        {
            //组的轴信息
            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
            //洞的宽,厚度,高,离地位置
            double width, thickness, height, ridz;
            //搜索洞所在墙的ID
            double x, y, z, minx, miny, minz;
            int wall, inx;
            x = y = z = minx = miny = minz = 0.0;
            wall = inx = 0;
            tmplist.Clear();
            //           
            width = thickness = height = ridz = 0.0;
            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
            //取得宽度
            width = GetDblfromProStr(GetProStrFromEnt(door, "width"));
            //取得厚度
            thickness = GetDblfromProStr(GetProStrFromEnt(door, "thickness"));
            //取得高度
            height = GetDblfromProStr(GetProStrFromEnt(door, "height"));
            //取得离地高度
            ridz = GetDblfromProStr(GetProStrFromEnt(door, "ridz"));
            //
            axEWdraw1.GetGroupAxis(door, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
            //
            if (FindMinDistWithIDList(orgx, orgy, orgz, ref wall, ref inx, ref tmplist, ref minx, ref miny, ref minz, ref idlist, g_wallthickness))
            {
                //之所以加0.002是因为避免面与面重合,这样做不影响洞的生成,因为多出去的部分是要减的部分.
                int tbox = axEWdraw1.Box(new object[] { 0, 0, 0 }, width, thickness + 0.002, height);
                //计算中心点
                double midx = width / 2.0;
                double midy = (thickness + 0.002) / 2.0;
                double midz = 0.0;
                axEWdraw1.Ax3TrasfWithZAsYAxis(tbox, new object[] { midx, midy, midz }, new object[] { 0, 1, 0 }/*前项*/, new object[] { -1, 0, 0 },
                                                new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                if (wall > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(wall);
                    int subent = axEWdraw1.Subtract(wall, tbox);
                    axEWdraw1.SetEntityUserData(subent, str);
                    cpidlist.Add(subent);
                    subtractidlist.Add(subent);//2016-09-06
                    return subent;
                }

            }
            return 0;
        }
        /*GetDblfromProStr将字段字符串值变为双精度值
         * 参数:
         * str 输入 表示数值的字符串
         * 返回值:
         * 返回转换后数值
         */
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
                string str = axEWdraw1.GetEntityUserData(ent);
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
        /*Import3DsDoor 函数导入3DS门
         * 参数:
         * filePath                 输入  文件路径(为了方便调试,请把3DS文件与纹理都先放到bin目录下的debug或release中)
         * grpname                  输入  组名(在例子中门是以door开头的之后是 _ 下划线符号 然后是门的类型,表数字表示. 比如:door_1)
         * xsite,ysite,zsite        输入  3DS导入后,变成组的基点,统一是0,0,0
         * xoff                     输入  门套包边X轴方向长出墙壁的距离(一侧) ,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff                     输入  如果门套长上长出的锁和门板厚度,则用该值表示,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff1                    输入  门套包边Y轴方向长出墙壁的距离(一侧),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * zoff                     输入  门套包边Z轴方向长出墙洞顶部的距离(顶部),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * lenth,thickness,height   输入  门在吸附时定义的宽度(长),厚度,高度,定义在组的自定义数据中,从自定义数据中读.
         * isoutwarddoor            输入  保留参数,用来区分内开或外开.如果导入3DS文件与MakeDoorSymbol(或其它创建符号函数)所创建符号一致,那就不用这个变量.
         * 返回值:
         * 如果成功返回创建的 3DS模型组的ID,其它返回0.
         */
        private int Import3DsDoor(string filePath, string grpname,
                                double xsite, double ysite, double zsite,
                                double xoff, double yoff, double yoff1, double zoff,
                                double length, double thickness, double height,/*这是门符号定义的长,厚度,高度*/
                                bool isoutwarddoor = false /*这个参数表示是否是外开门(如果是true),默认是内开门,这个和符号的初始相同*/
                                )
        {
            int impsize = 0;
            int ent0 = 0;
            double x, y, z, x1, y1, z1, x2, y2, z2;
            double minx, miny, minz, maxx, maxy, maxz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw1.ClearIDBuffer();
            ////重新创建一个带3DS网面的组
            int group = 0;
            //如果没有导入过,则从文件导入并创建组,这样可以保证相同的组,只导入一次
            //    //1.导入3DS模型,注意这里的3DS模型最好是最简模型
            //    //2.导入的3DS模型,一定要都是前项
            //    //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
            impsize = axEWdraw1.Imp3DSWithTexture(filePath);
            //创建组
            for (int i = 1; i <= impsize; i++)
            {
                ent0 = axEWdraw1.GetImpEntID(i);
                axEWdraw1.AddIDToBuffer(ent0);
            }
            group = axEWdraw1.MakeGroup(grpname, new object[] { xsite, ysite, zsite });
            //
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { xsite, ysite, zsite });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            double midx = (x2 + x1) / 2.0;
            double midy = (y2 + y1 + yoff) / 2.0;
            double midz = 0;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //计算轴向缩放系数
            double xscale = length / ((x2 - x1) - xoff * 2.0);
            double yscale = thickness / ((y2 - y1) - yoff - yoff1 * 2.0);
            double zscale = height / ((z2 - z1) - zoff);
            //
            axEWdraw1.MeshScaleByXYZAxis3(group, xscale, yscale, zscale);
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            return group;
        }

        /*Import3DsWindow 函数导入3DS窗
         * 参数:
         * filePath                 输入  文件路径(为了方便调试,请把3DS文件与纹理都先放到bin目录下的debug或release中)
         * grpname                  输入  组名(在例子中门是以door开头的之后是 _ 下划线符号 然后是门的类型,表数字表示. 比如:door_1)
         * xsite,ysite,zsite        输入  3DS导入后,变成组的基点,统一是0,0,0
         * xoff                     输入  门套包边X轴方向长出墙壁的距离(一侧) ,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff                     输入  如果门套长上长出的锁和门板厚度,则用该值表示,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff1                    输入  门套包边Y轴方向长出墙壁的距离(一侧),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * zoff                     输入  门套包边Z轴方向长出墙洞顶部的距离(顶部),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * lenth,thickness,height   输入  门在吸附时定义的宽度(长),厚度,高度,定义在组的自定义数据中,从自定义数据中读.
         * isoutwarddoor            输入  保留参数,用来区分内开或外开.如果导入3DS文件与MakeDoorSymbol(或其它创建符号函数)所创建符号一致,那就不用这个变量.
         * 返回值:
         * 如果成功返回创建的 3DS模型组的ID,其它返回0.
         */
        private int Import3DsWindow(string filePath, string grpname,
                                double xsite, double ysite, double zsite,
                                double xoff, double yoff, double yoff1, double zoff,
                                double length, double thickness, double height,/*这是门符号定义的长,厚度,高度*/
                                bool isoutwarddoor = false /*这个参数表示是否是外开门(如果是true),默认是内开门,这个和符号的初始相同*/
                                )
        {
            int impsize = 0;
            int ent0 = 0;
            double x, y, z, x1, y1, z1, x2, y2, z2;
            double minx, miny, minz, maxx, maxy, maxz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw1.ClearIDBuffer();
            ////重新创建一个带3DS网面的组
            int group = 0;
            string[] namearr = grpname.Split('_');
            string orggrpname = "window_" + namearr[1];
            //
            int orggrp = 0;//IsExistGroup(orggrpname);
                           //if (orggrp <= 0)
                           //{//如果没有导入过,则从文件导入并创建组,这样可以保证相同的组,只导入一次
                           //1.导入3DS模型,注意这里的3DS模型最好是最简模型
                           //2.导入的3DS模型,一定要都是前项
                           //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
            impsize = axEWdraw1.Imp3DSWithTexture(filePath);
            //创建组
            for (int i = 1; i <= impsize; i++)
            {
                ent0 = axEWdraw1.GetImpEntID(i);
                axEWdraw1.AddIDToBuffer(ent0);
            }
            group = axEWdraw1.MakeGroup(orggrpname, new object[] { xsite, ysite, zsite });
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { xsite, ysite, zsite });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            double midx = (x2 + x1) / 2.0;
            double midy = (y2 + y1 + yoff) / 2.0;
            double midz = 0;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //计算轴向缩放系数
            double xscale = length / ((x2 - x1) - xoff * 2.0);
            double yscale = thickness / ((y2 - y1) - yoff - yoff1 * 2.0);
            double zscale = height / ((z2 - z1) - zoff);
            //
            axEWdraw1.MeshScaleByXYZAxis3(group, xscale, yscale, zscale);
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            return group;
        }

        /*Import3DsBayWin 函数导入3DS飘窗
         * 参数:
         * filePath                 输入  文件路径(为了方便调试,请把3DS文件与纹理都先放到bin目录下的debug或release中)
         * grpname                  输入  组名(在例子中门是以door开头的之后是 _ 下划线符号 然后是门的类型,表数字表示. 比如:door_1)
         * xsite,ysite,zsite        输入  3DS导入后,变成组的基点,统一是0,0,0
         * xoff                     输入  门套包边X轴方向长出墙壁的距离(一侧) ,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff                     输入  如果门套长上长出的锁和门板厚度,则用该值表示,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff1                    输入  门套包边Y轴方向长出墙壁的距离(一侧),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * zoff                     输入  门套包边Z轴方向长出墙洞顶部的距离(顶部),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * lenth,thickness,depth,
         * height                   输入  门在吸附时定义的宽度(长),厚度,高度,定义在组的自定义数据中,从自定义数据中读.
         * isside                   输入  飘窗左右两侧是否需要画墙
         * isoutwarddoor            输入  保留参数,用来区分内开或外开.如果导入3DS文件与MakeBayWinSymbol(或其它创建符号函数)所创建符号一致,那就不用这个变量.
         * 返回值:
         * 如果成功返回创建的 3DS模型组的ID,其它返回0.
         */
        private int Import3DsBayWin(string filePath, string grpname,
                                double xsite, double ysite, double zsite,
                                double xoff, double yoff, double yoff1, double zoff,
                                double length, double thickness, double depth, double height,/*这是门符号定义的长,厚度,高度*/
                                bool isside,
                                bool isoutwarddoor = false /*这个参数表示是否是外开门(如果是true),默认是内开门,这个和符号的初始相同*/
                                )
        {
            int impsize = 0;
            int ent0 = 0;
            double x, y, z, x1, y1, z1, x2, y2, z2;
            double minx, miny, minz, maxx, maxy, maxz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw1.ClearIDBuffer();
            ////重新创建一个带3DS网面的组
            int group = 0;
            string[] namearr = grpname.Split('_');
            string orggrpname = "";
            if (namearr[0] == "baywin")
            {//判断类型
                orggrpname = "bw" + "_" + "org" + "_" + namearr[1];
            }//else ...etc. 也可以在些增窗户或其它的判断

            //
            int orggrp = 0;// IsExistGroup(orggrpname);
                           //if (orggrp <= 0)
                           //{//如果没有导入过,则从文件导入并创建组,这样可以保证相同的组,只导入一次
                           //1.导入3DS模型,注意这里的3DS模型最好是最简模型
                           //2.导入的3DS模型,一定要都是前项
                           //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
            impsize = axEWdraw1.Imp3DSWithTexture(filePath);
            //创建组
            for (int i = 1; i <= impsize; i++)
            {
                ent0 = axEWdraw1.GetImpEntID(i);
                axEWdraw1.AddIDToBuffer(ent0);
            }
            //
            orggrp = axEWdraw1.MakeGroup(orggrpname, new object[] { xsite, ysite, zsite });
            //
            group = orggrp;
            //创建底,顶,左,右墙
            int ent1, ent2, ent3, ent4, ent5, tmpent;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            ent1 = ent2 = ent3 = ent4 = ent5 = tmpent = 0;
            double midx, midy, midz;

            midx = length / 2.0;
            midy = 0;
            midz = 0;
            //创建底
            ent4 = axEWdraw1.Box(new object[] { 0, 0, -thickness }, length, depth, thickness);
            axEWdraw1.SetEntColor(ent4, axEWdraw1.RGBToIndex(255, 255, 255));
            //创建顶
            ent5 = axEWdraw1.Box(new object[] { 0, 0, height }, length, depth, thickness);
            axEWdraw1.SetEntColor(ent5, axEWdraw1.RGBToIndex(255, 255, 255));//设置墙体颜色,这里是白色,要和顶底的颜色相同
            //检测窗户对象的范围
            ent1 = group;//
            axEWdraw1.GetEntBoundingBox(ent1, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            double xbl = length / (maxx - minx);
            double ybl = thickness / (maxy - miny);
            double zbl = height / (maxz - minz);
            axEWdraw1.MoveTo(ent1, new object[] { minx, miny, minz }, new object[] { 0, 0, 0 });
            axEWdraw1.MeshScaleByXYZAxis3(ent1, xbl, 1.0, zbl);
            axEWdraw1.GetEntBoundingBox(ent1, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            axEWdraw1.MoveTo(ent1, new object[] { minx, miny, minz }, new object[] { 0, depth - (maxy - miny), 0 });
            long meshinx = -1;
            long grpinx = -1;
            if (isside)
            {
                //左侧
                ent2 = axEWdraw1.Box(new object[] { -thickness, 0, -thickness }, thickness, depth, height + thickness * 2.0);
                ent3 = axEWdraw1.Box(new object[] { length, 0, -thickness }, thickness, depth, height + thickness * 2.0);
                axEWdraw1.SetEntColor(ent2, axEWdraw1.RGBToIndex(255, 255, 255));
                axEWdraw1.SetEntColor(ent3, axEWdraw1.RGBToIndex(255, 255, 255));
            }
            //重新组合组
            axEWdraw1.Explode(ent1);
            int expldsize = axEWdraw1.GetExplodeIDBufferSize();
            for (int i = 0; i < expldsize; i++)
            {
                tmpent = axEWdraw1.GetExplodeIDBuffer(i);
                axEWdraw1.AddIDToBuffer(tmpent);
            }
            axEWdraw1.AddIDToBuffer(ent4);
            axEWdraw1.AddIDToBuffer(ent5);
            if (ent2 > 0)
                axEWdraw1.AddIDToBuffer(ent2);
            if (ent3 > 0)
                axEWdraw1.AddIDToBuffer(ent3);
            orggrp = axEWdraw1.MakeGroup(orggrpname, new object[] { xsite, ysite, zsite });
            axEWdraw1.GetEntBoundingBox(orggrp, ref x1, ref y1, ref z1, ref x2, ref y2, ref z2);
            //
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { xsite, ysite, zsite });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            midx = (x2 + x1) / 2.0;
            midy = y1 + yoff + thickness / 2.0 - thickness;//
            midz = thickness;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            axEWdraw1.ClearIDBuffer();
            return group;
        }

        /*CopyWall在原位复制墙面,目的是保留原有的墙面,在开墙洞后,再恢复原有墙面
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private int CopyWall()
        {
            cpidlist.Clear();
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (!axEWdraw1.IsGroup(ent))//不是组
                    {
                        int type = axEWdraw1.GetEntType(ent);
                        if (type == 59)
                        {
                            //判断是否是承重墙.程序只处理的承重墙 2016-10-13
                            string weightwallstr = "";
                            weightwallstr = GetProStrFromEnt(ent, "weightwall");
                            if (weightwallstr == "")
                            {
                                //
                                int cpent = axEWdraw1.Copy(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });//原位复制
                                string str = axEWdraw1.GetEntityUserData(cpent);
                                orgidlist.Add(ent);
                                cpidlist.Add(cpent);
                            }
                        }
                    }
                }
            }
            return cpidlist.Count;
        }

        /*FindMinDistWithIDList 在指定的实体范围内根据给出的点从所有墙实体的点数据中找到最近的一个点
         * 参数:
         * x,y,z           输入  给出一个点
         * id              输出  返回距离最后对象的ID
         * inx             输出  最近对象中最近的线段索引
         * opts            输出  最近对象的所有点数据的数组
         * minx,miny,minz  输出  距离给出点最近的点
         * idlist          输入  给定的实体ID数据,以指定搜索实体的范围
         * tol             输入  这是一个距离值,如果大于零,表示如果给出的x,y,z的点距离 点数组中某个点的距离小于tol,则直接将minx,miny,minz设置为数组中的某点.
         *                       如果为0,则没有该特性.
         * 返回值:
         * 找到最近点返回ture,其它返回false.
         */
        private bool FindMinDistWithIDList(double x, double y, double z, ref int id, ref int inx, ref ArrayList opts,
                            ref double minx, ref double miny, ref double minz, ref ArrayList idlist, double tol = 128.0)
        {
            string str;
            int entsize = idlist.Count;
            int tmpjs = 0;
            int minid = 0;//距离最近的实体ID
            double mindist = 0.0;
            int mininx = 0;//线段索引
            ArrayList pts = new ArrayList();
            //查找并设置实体的用户数据为点集
            for (int i = 0; i < entsize; i++)
            {
                int ent = Convert.ToInt32(idlist[i]);
                if (!axEWdraw1.IsGroup(ent))
                {
                    int enttype = axEWdraw1.GetEntType(ent);
                    pts.Clear();
                    if (enttype == 59 || enttype == 73)
                    {
                        if (GetPtsFromSolid(ent, ref pts))
                        {
                            int tinx = 0;
                            double ox, oy, oz;
                            ox = oy = oz = 0.0;
                            double dist = GetNearLine(x, y, z, ref pts, ref tinx, ref ox, ref oy, ref oz, tol);
                            if (tmpjs == 0)
                            {
                                mindist = dist;
                                mininx = tinx;
                                minid = ent;
                                //最近点
                                minx = ox;
                                miny = oy;
                                minz = oz;
                                opts.Clear();
                                for (int j = 0; j < pts.Count; j++)
                                    opts.Add(pts[j]);
                            }
                            else if (dist < mindist)
                            {
                                mindist = dist;
                                mininx = tinx;
                                minid = ent;
                                //最近点
                                minx = ox;
                                miny = oy;
                                minz = oz;
                                opts.Clear();
                                for (int j = 0; j < pts.Count; j++)
                                    opts.Add(pts[j]);
                            }
                            pts.Clear();
                            tmpjs++;
                        }
                    }
                }
            }
            id = minid;
            inx = mininx;
            if (tmpjs > 0)
                return true;
            return false;
        }

        /*MakeDoorSymbol 在俯视下创建门的符号,以组为基础.也就是说一个组是一个符号.
         * 参数:
         * name      输入 门的名称(组名)
         * width     输入 宽度
         * thickness 输入 厚度
         * height    输入 高度
         * ridz      输入 底部距地面的Z值
         * maxz      输入 最高z值,一定要高于墙高
         * 返回值:
         * 成功则返回符号组的ID,其它返回0
         */
        private int MakeDoorSymbol(string name, double width, double thickness, double height, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, width, thickness, 0);
            int rect1 = axEWdraw1.Rectangle(0, 0, width, thickness, maxz);
            axEWdraw1.SetEntColor(rect1, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(rect1, 2);
            int sector = 0;
            if (!isoutwarddoor)
                sector = axEWdraw1.Sector(new object[] { width, thickness, 0 }, width, 90, 180);
            else
                sector = axEWdraw1.Sector(new object[] { width, 0, 0 }, width, 180, 270);
            axEWdraw1.SetEntColor(sector, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(sector, 2);
            //axEWdraw1.MoveTo(sector, new object[] { 0, 0, 0 }, new object[] { 0, 0, maxz });
            axEWdraw1.SetEntColor(rect, axEWdraw1.RGBToIndex(0, 0, 0));
            int frect = axEWdraw1.EntToFace(rect, false);
            int fsector = axEWdraw1.EntToFace(sector, false);

            int prect = axEWdraw1.Prism(frect, maxz, new object[] { 0, 0, 1 });
            int psector = axEWdraw1.Prism(fsector, maxz, new object[] { 0, 0, 1 });
            axEWdraw1.Delete(frect);
            axEWdraw1.Delete(fsector);
            axEWdraw1.SetEntColor(prect, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(prect, 0.3);
            axEWdraw1.SetEntColor(psector, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(psector, 0.6);//2017-09-21

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(prect);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(psector);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect);
            axEWdraw1.AddIDToBuffer(rect1);
            axEWdraw1.AddIDToBuffer(sector);
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { width / 2.0, thickness / 2.0, 0 });
            //设置组的数据
            string str = "width:" + width.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "height:" + height.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            axEWdraw1.ClearIDBuffer();
            return group;
        }
        /*HideSymbol隐藏符号
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private void HideSymbol()
        {
            //消隐符号组
            if (symbolidlist.Count > 0)
            {
                for (int i = 0; i < symbolidlist.Count; i++)
                {
                    axEWdraw1.SetEntityInvisible((int)symbolidlist[i], false);
                }
            }
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname = axEWdraw1.GetGroupName(ent);
                        int inx = grpname.IndexOf("symbol");
                        if (inx >= 0)
                        {//如果是符号组
                            axEWdraw1.SetEntityInvisible(ent, true);
                        }
                    }
                }
            }
        }
        /*HideSymbol显示符号
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private void ShowSymbol()
        {
            //消隐符号组
            if (symbolidlist.Count > 0)
            {
                for (int i = 0; i < symbolidlist.Count; i++)
                {
                    axEWdraw1.SetEntityInvisible((int)symbolidlist[i], true);
                }
            }
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname = axEWdraw1.GetGroupName(ent);
                        int inx = grpname.IndexOf("symbol");
                        if (inx >= 0)
                        {//如果是符号组
                            axEWdraw1.SetEntityInvisible(ent, false);
                        }
                    }
                }
            }
        }

        /*HideFurniture隐藏符号
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private void HideFurniture()
        {
            //家具组
            furniturehides.Clear();
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname = axEWdraw1.GetGroupName(ent);

                        if (grpname.IndexOf("cabinet") >= 0 ||
                            grpname.IndexOf("chuang") >= 0 ||
                            grpname.IndexOf("selfdraw") >= 0 //2018-01-31
                            )
                        {//如果是符号组
                            axEWdraw1.SetEntityInvisible(ent, true);
                            furniturehides.Add(ent);
                        }
                    }
                }
            }
        }

        /*IsExistGroup根据组名判断,是否有该组存在
         * 参数:
         * grpname 组名
         * 返回值:
         * 如果存在,则返回id,其它返回0
         */
        private int IsExistGroup(string grpname)
        {
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        string grpname1 = axEWdraw1.GetGroupName(ent);
                        if (grpname == grpname1)
                            return ent;
                    }
                }
            }
            return 0;
        }
        /*DelDimWalls 删除与该墙面绑定的标注
         * 参数:
         * ent  输入
         * 返回值:
         * 无
         */
        private void DelDimWalls(int ent)
        {
            //搜索现有的实体中的与该实体绑定的标注
            int entsize = axEWdraw1.GetEntSize();
            int enttype = -1;
            int tmpent = 0;
            int diment = 0;
            string str = "";
            ArrayList idlist = new ArrayList();
            for (int i = 1; i <= entsize; i++)
            {
                tmpent = axEWdraw1.GetEntID(i);
                enttype = axEWdraw1.GetEntType(tmpent);
                if (enttype >= 93 && enttype <= 100)
                {
                    str = GetProStrFromEnt(tmpent, "id");
                    if (str != "")
                    {
                        diment = Convert.ToInt32(str);
                        if (diment == ent)
                        {
                            idlist.Add(tmpent);
                            break;
                        }
                        else
                        {//如果不是等于的实体,则判断该标注所关联的该实体是否存在 2017-01-21
                            if (!axEWdraw1.IsExistEnt(diment))
                            {
                                idlist.Add(tmpent);
                                break;
                            }
                        }
                    }

                }
            }
            if (idlist.Count > 0)
            {
                for (int i = 0; i < idlist.Count; i++)
                    axEWdraw1.Delete((int)idlist[i]);
            }
        }
        /*DimWalls 标注已经绘制的墙面 2016-09-18
         * 参数:
         * ent     输入 需要绘制标注的墙实体
         * font    输入 标注的字体
         * fheight 输入 标注字体的大小
         * r,g,b   输入 标注颜色
         * off     输入 标注偏移的距离(墙厚的2至3倍比较合适)
         * maxz    输入 最大的Z值(一定要高于墙高)
         * 返回值:
         * 无
         */
        private void DimWalls(int ent, string font, double fheight, int r, int g, int b, double off, double maxz = 0.0)
        {
            if (ent > 0)
            {
                ArrayList tmpptslist = new ArrayList();

                if (GetPtsFromSolid(ent, ref tmpptslist))
                {
                    if (tmpptslist.Count >= 3)
                    {
                        //搜索现有的实体中的与该实体绑定的标注
                        int entsize = axEWdraw1.GetEntSize();
                        int enttype = -1;
                        int tmpent = 0;
                        string str = "";
                        ArrayList idlist = new ArrayList();
                        //绘制标注前,删除之前已经与该实体绑定的标注对象
                        for (int i = 1; i <= entsize; i++)
                        {
                            tmpent = axEWdraw1.GetEntID(i);
                            enttype = axEWdraw1.GetEntType(tmpent);
                            if (enttype >= 93 && enttype <= 100)
                            {
                                str = GetProStrFromEnt(tmpent, "id");
                                if (str != "")
                                {
                                    if (Convert.ToInt32(str) == ent)
                                    {
                                        idlist.Add(tmpent);
                                    }
                                }
                            }
                        }
                        if (idlist.Count > 0)
                        {
                            for (int i = 0; i < idlist.Count; i++)
                                axEWdraw1.Delete((int)idlist[i]);
                        }
                        idlist.Clear();
                        //
                        //求中心点
                        double sumx, sumy, sumz, midx, midy, midz;
                        sumx = sumy = sumz = midx = midy = midz = 0.0;
                        int len = tmpptslist.Count / 3;
                        for (int i = 0; i < len; i++)
                        {
                            sumx += (double)tmpptslist[i * 3];
                            sumy += (double)tmpptslist[i * 3 + 1];
                            sumz += (double)tmpptslist[i * 3 + 2];
                        }
                        midx = sumx / (double)len;
                        midy = sumy / (double)len;
                        midz = sumz / (double)len;
                        //计算每一线段标注的方向
                        double x1, y1, z1, x2, y2, z2, lmx, lmy, lmz, vx, vy, vz;
                        x1 = y1 = z1 = x2 = y2 = z2 = lmx = lmy = lmz = vx = vy = vz = 0.0;
                        for (int i = 0; i < len - 1; i++)
                        {

                            x1 = (double)tmpptslist[i * 3];
                            y1 = (double)tmpptslist[i * 3 + 1];
                            z1 = (double)tmpptslist[i * 3 + 2];

                            x2 = (double)tmpptslist[(i + 1) * 3];
                            y2 = (double)tmpptslist[(i + 1) * 3 + 1];
                            z2 = (double)tmpptslist[(i + 1) * 3 + 2];
                            lmx = (x1 + x2) / 2.0;
                            lmy = (y1 + y2) / 2.0;
                            lmz = (z1 + z2) / 2.0;
                            //从线段中心指向区域中心点的方向
                            vx = midx - lmx;
                            vy = midy - lmy;
                            vz = midz - lmz;
                            //判断标注的方向
                            double ox, oy, oz, ox1, oy1, oz1, ox2, oy2, oz2, vx1, vy1, vz1, vx2, vy2, vz2;
                            ox = oy = oz = ox1 = oy1 = oz1 = ox2 = oy2 = oz2 = vx1 = vy1 = vz1 = vx2 = vy2 = vz2 = 0.0;
                            axEWdraw1.Polar(new object[] { lmx, lmy, lmz }, new object[] { x2 - lmx, y2 - lmy, z2 - lmz }, off, ref ox, ref oy, ref oz);
                            axEWdraw1.RotatePoint(ox, oy, oz, lmx, lmy, lmz, 0, 0, 1, 90, ref ox1, ref oy1, ref oz1);
                            axEWdraw1.RotatePoint(ox, oy, oz, lmx, lmy, lmz, 0, 0, 1, -90, ref ox2, ref oy2, ref oz2);
                            vx1 = ox1 - lmx;
                            vy1 = oy1 - lmy;
                            vz1 = oz1 - lmy;

                            vx2 = ox2 - lmx;
                            vy2 = oy2 - lmy;
                            vz2 = oz2 - lmy;
                            //
                            double ang1 = axEWdraw1.VectorAngle(new object[] { vx1, vy1, vz1 }, new object[] { vx, vy, vz });
                            double ang2 = axEWdraw1.VectorAngle(new object[] { vx2, vy2, vz2 }, new object[] { vx, vy, vz });
                            if (ang1 > ang2)
                            {
                                ox = ox1;
                                oy = oy1;
                                oz = oz1;
                            }
                            else
                            {
                                ox = ox2;
                                oy = oy2;
                                oz = oz2;
                            }
                            //设置标注文字大小
                            axEWdraw1.SetDimTxt(fheight);
                            axEWdraw1.SetDimAsz(fheight / 3.0);
                            //设置标注的字体
                            axEWdraw1.SetDimTxsty(font);
                            //设置标注的颜色
                            axEWdraw1.SetDimClr(r, g, b);
                            //设置标注文字高于标注线的距离
                            axEWdraw1.SetDimTAD(50);
                            //
                            int diment = 0;
                            if (Math.Abs(maxz) > 0.00001)
                                diment = axEWdraw1.LengthDimension(new object[] { x1, y1, maxz }, new object[] { x2, y2, z2 }, 0, new object[] { 0, 0, 1 }, false, "", new object[] { ox, oy, maxz });
                            else
                                diment = axEWdraw1.LengthDimension(new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 }, 0, new object[] { 0, 0, 1 }, false, "", new object[] { ox, oy, oz });
                            //将该标注与要标注的实体陈绑定(方法是在标注的数据中写入该实体的ID号)
                            if (diment > 0)
                                axEWdraw1.SetEntityUserData(diment, "id:" + ent.ToString() + ";" + "segnum:" + i.ToString() + ";");//2016-10-28 增加段的索引值
                        }
                        //
                    }
                }
                tmpptslist.Clear();
            }
        }

        /*HideDim隐藏注类实体
         * 参数:
         * 无
         * 返回值:
         * 无
         */
        private void HideDim()
        {
            //搜索现有的实体中的与该实体绑定的标注
            int entsize = axEWdraw1.GetEntSize();
            int enttype = -1;
            int tmpent = 0;
            ArrayList idlist = new ArrayList();
            for (int i = 1; i <= entsize; i++)
            {
                tmpent = axEWdraw1.GetEntID(i);
                enttype = axEWdraw1.GetEntType(tmpent);
                if (enttype >= 93 && enttype <= 100)
                {
                    axEWdraw1.SetEntityInvisible(tmpent, true);
                }
            }
        }

        /*ShowDim隐藏注类实体
        * 参数:
        * 无
        * 返回值:
        * 无
        */
        private void ShowDim()
        {
            //搜索现有的实体中的与该实体绑定的标注
            int entsize = axEWdraw1.GetEntSize();
            int enttype = -1;
            int tmpent = 0;
            ArrayList idlist = new ArrayList();
            for (int i = 1; i <= entsize; i++)
            {
                tmpent = axEWdraw1.GetEntID(i);
                enttype = axEWdraw1.GetEntType(tmpent);
                if (enttype >= 93 && enttype <= 100)
                {
                    axEWdraw1.SetEntityInvisible(tmpent, false);
                }
            }
        }
        /* MakeAhomeArea 取得由墙围成的屋内(封闭)面积 2016-09-30
         * 参数:
         * walltickness 输入 墙的厚度
         */
        private int MakeHomeArea(double walltickness)
        {
            int areaent = 0;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            double midx, midy;
            double tox, toy, toz;
            tox = toy = toz = 0.0;
            midx = midy = 0.0;
            int fontheight = 150;
            int textent = 0;
            int entsize = 0;
            int ent = 0;
            axEWdraw1.Clear3DPtBuf();
            axEWdraw1.ClearIDBuffer();
            /*GetHomeArea 函数是用来根据现有的墙数据(即含有pts数据的PIPE对象),自动生成面积对象
             * 参数:
             * walltickneww 输入 墙厚
             * 返回值:
             * 所创建面积对象的个数
             * 注意:
             * 1.生成的多个面积对象的ID会被写入idbuffer内,可以用GetIDBuffer取出
             * 2.生成面积对象的同时,也会生成与对象相对的点信息,主要是用来确认文字的位置.这些点信息会被写入到3DPtBuf,可以用Get3DPtBuf取出.
             */
            axEWdraw1.GetHomeArea(walltickness);
            //获得创建的面积对象个数
            int areaentsize = axEWdraw1.GetIDBufferSize();
            //得到面积对象的面积值
            for (int i = 0; i < areaentsize; i++)
            {
                areaent = axEWdraw1.GetIDBuffer(i);

                //axEWdraw1.SetTransparency(areaent, 0.9);
                double area = axEWdraw1.GetEntArea(areaent) / 1000000;//按平方米
                string str = String.Format("{0:f2}", area) + "M²";
                int strlen = str.Length;
                axEWdraw1.GetEntBoundingBox(areaent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                //计算地板的重复率
                double ureapt = (maxx - minx) / 1600.0;
                double vreapt = (maxy - miny) / 1600.0;
                axEWdraw1.SetEntTexture(areaent, "floor_2.png", 1, 1, ureapt, vreapt, 0, 0);
                midx = (minx + maxx) / 2.0;
                midy = (miny + maxy) / 2.0;
                //
                if (axEWdraw1.Get3DPtBufSize() > i)
                {
                    axEWdraw1.Get3DPtBuf(i, ref tox, ref toy, ref toz);
                    textent = axEWdraw1.Text3D(str, "Arial", new object[] { tox - (strlen * fontheight * 0.75 / 2.0), toy, 1.0 }, fontheight, 0, 0);
                }
                else
                    textent = axEWdraw1.Text3D(str, "Arial", new object[] { midx - (strlen * fontheight * 0.75 / 2.0), midy, 1.0 }, fontheight, 0, 0);
                axEWdraw1.SetEntColor(textent, axEWdraw1.RGBToIndex(0, 0, 0));
                //设置所创建的面积对象与文字为面积类型
                SetProStrFromEnt(areaent, "area", 1);
                SetProStrFromEnt(textent, "id", areaent);//2016-11-15
                axEWdraw1.DeactivateEnt(textent);
                //如果是自由绘制阳台,则获得地面对象的外围点,并判断阳台所属的地面区域//2017-11-28
                if (isdrawbalcony)
                {
                    ArrayList tmppts = new ArrayList();
                    GetPtsFromSolid(areaent, ref tmppts);
                    int plen = tmppts.Count / 3 - 1;

                    for (int j = 0; j < plen; j++)
                    {
                        SPoint apt = new SPoint(((double)tmppts[j * 3] + (double)tmppts[(j + 1) * 3]) / 2.0, ((double)tmppts[j * 3 + 1] + (double)tmppts[(j + 1) * 3 + 1]) / 2.0, 0);
                        for (int k = 0; k < g_balconypts.Count; k++)
                        {
                            double dist = axEWdraw1.PointDistance((double)apt.x, (double)apt.y, 0, ((SPoint)(g_balconypts[k])).x, ((SPoint)(g_balconypts[k])).y, 0);
                            if (dist < g_wallthickness)
                            {
                                g_balconyent = areaent;
                                break;
                            }
                        }
                        if (g_balconyent > 0)
                            break;
                    }
                    tmppts.Clear();
                }
            }
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.Clear3DPtBuf();
            return 0;//
        }

        /*HideArea隐藏面积对象
        * 参数:
        * 无
        * 返回值:
        * 无
        */
        private void HideArea()
        {
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(ent);
                    int inx = IsHaveStrField(str, "area:");
                    if (inx >= 0)
                    {//如果是符号组
                        axEWdraw1.SetEntityInvisible(ent, true);
                    }
                }
            }
        }
        /*ShowArea隐藏面积对象
        * 参数:
        * 无
        * 返回值:
        * 无
        */
        private void ShowArea()
        {
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(ent);
                    int inx = IsHaveStrField(str, "area:");
                    if (inx >= 0)
                    {//如果是符号组
                        axEWdraw1.SetEntityInvisible(ent, false);
                    }
                }
            }
        }

        //在平面下增加符号,如:门或其它类型
        private void DrawDoor(string doorname, double width, double thickness, double height, double ridz)//button6_Click_1(object sender, EventArgs e)
        {
            canceldrawwalljs = -1;
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                string grpname = "symbol_door_" + doorname;//2018-11-14
                lastsym = MakeDoorSymbol(grpname, width, g_wallthickness, height, ridz, g_maxz, true);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbHigh(false, 0);
                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");
        }

        private void axEWdraw1_ViewMouseUp(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseUpEvent e)
        {
            if (e.button == 2 && isdrawabsorb)
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
                isdrawabsorb = false;
            }
            else if (e.button == 1 && !isdrawabsorb && !isdrawpolyline && !isseloneent && g_state != InteractiveState.MoveingWall && !isdrawmove)//2017-03-15
            {

                movewalljs = -1;//2017-03-15 表示当前如果是移动墙的准备状态,则取消
                timer2.Enabled = true;
                timer4.Enabled = false;//2017-08-08
                g_st = 0;
                if (isdrawpan)
                {
                    isdrawpan = false;
                    axEWdraw1.CancelCommand();
                }
                else
                {
                    if (!axEWdraw1.IsEndCommand())
                    {
                        if (isdrawaxismove)
                        {
                            isdrawaxismove = false;
                            g_mx = e.xCoordinate;
                            g_my = e.yCoordinate;
                            g_mz = e.zCoordinate;
                            axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                            isdrawaxismove = false;
                        }
                    }
                }
            }
            else if (e.button == 1 && movewalljs >= 1 && g_state == InteractiveState.MoveingWall)
            {
                movewalljs++;//改变移墙的状态位 2017-03-15
                timer4.Enabled = true;
            }
            else if (g_state == InteractiveState.MoveingWall && e.button == 2)
            {
                axEWdraw1.CancelMoveWall();
                g_state = InteractiveState.Nothing;
            }
            else
            {
                int wx1, wy1, wx2, wy2;
                wx1 = wy1 = wx2 = wy2 = 0;
                //2017-01-21
                g_mx = Math.Round(e.xCoordinate, 3);
                g_my = Math.Round(e.yCoordinate, 3);
                g_mz = Math.Round(e.zCoordinate, 3);
                //
                axEWdraw1.Coordinate2Screen(g_bx, g_by, g_bz, ref wx1, ref wy1);
                axEWdraw1.Coordinate2Screen(g_x, g_y, g_z, ref wx2, ref wy2);
                if (isdrawmove || isdrawabsorb)
                {
                    if (g_st > 1 && (Math.Abs(wx2 - wx1) > 2 || Math.Abs(wy2 - wy1) > 2))
                    {
                        g_st = 0;
                        axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                        isdrawmove = false;
                        isdrawabsorb = false;
                        timer2.Enabled = false;
                        //2018-06-20 如果是正常结束,则lastsym恢复为初值
                        if (lastsym > 0)
                            lastsym = 0;
                    }
                    else
                    {
                        g_st = 0;
                        isdrawmove = false;
                        isdrawabsorb = false;
                        timer2.Enabled = false;
                        axEWdraw1.ToSetDrawPoint(new object[] { g_mx, g_my, g_mz });//2017-01-20
                        //2018-06-20 如果是正常结束,则lastsym恢复为初值
                        if (lastsym > 0)
                            lastsym = 0;
                    }
                }
                else if (axEWdraw1.IsEndCommand())
                {//2017-12-26 右键菜单
                    if (e.button == 2)
                    {
                        if (canceldrawwalljs != 1)
                        {//避免与双击右键结束绘制墙产生冲突,如果之前已经将双击右键结束绘制墙去除,那么就不需要这个条件了.
                            int size = axEWdraw1.GetSelectEntSize();
                            if (size > 0)
                            {
                                g_rbuttonselent = axEWdraw1.GetSelectEnt(0);
                                contextMenuStrip1.Show(axEWdraw1, new Point(e.xWin, e.yWin));
                            }
                            else
                                g_rbuttonselent = 0;
                        }
                    }
                }
            }

        }

        //在这个时钟内处理点击可移动的门或其它类型符号
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;//2018-06-20 位置换在最前面
            g_st++;//2018-06-20
        }

        //墙洞模式
        private void WallHoleView()
        {
            if (g_viewmode == 0)
            {
                if (!axEWdraw1.IsEndCommand())
                {//如果命令没有结束,则判断,是否有lastsym的实体
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }
                canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
                axEWdraw1.CancelCommand();
                g_viewmode = 1;
                try
                {
                    //创建阳台
                    MakeBalcony(ref g_balconyids);
                    int len = axEWdraw1.GetEntSize();
                    //从区域对象中得到墙线坐标信息,并判断是否是共享墙面
                    int ent, type, topbootom;
                    ent = 0;
                    type = -1;
                    topbootom = 0;
                    for (int i = 1; i <= len; i++)
                    {
                        ent = axEWdraw1.GetEntID(i);
                        type = axEWdraw1.GetEntType(ent);
                        if (type == 50)
                        {
                            string str = GetProStrFromEnt(ent, "area");
                            if (str.Length > 0)
                            {
                                if (Convert.ToInt32(str) == 1)
                                {
                                    //取得范围的顶底面
                                    topbottomids.Add(topbootom);
                                }
                            }
                        }
                    }
                    //
                    axEWdraw1.SetPerspectiveMode(true);
                    HideDim();//消隐标注
                    HideArea();//清隐面积对象
                    HideWeightWall();//消隐承重墙
                    HideSymbol();//清隐符号
                    HideFurniture();//清家具
                    int swsize = axEWdraw1.GetSingleWallSize();
                    int singleid = 0;
                    int singledid = 0;
                    int singleorgid = 0;
                    int singleinx = -1;
                    double sx, sy, sz, ex, ey, ez;
                    sx = sy = sz = ex = ey = ez = 0.0;
                    double ox, oy, oz;
                    ox = oy = oz = 0.0;
                    double dist = 0;
                    singlewallids.Clear();
                    for (int i = 0; i < swsize; i++)
                    {
                        axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                        axEWdraw1.SetEntDisplayMode(singleid, 1);
                        axEWdraw1.SetWallColor(singleid, 255, 255, 255);
                        singlewallids.Add(singleid);
                    }
                }
                finally
                {
                    axEWdraw1.SetViewCondition(8);
                }
                if (timer6.Enabled)//2018-08-03
                    timer6.Enabled = false;
                axEWdraw1.ToDrawOrbit();
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            axEWdraw1.CancelCommand();
            axEWdraw1.SetViewCondition(8);
            HideSymbol();//消隐符号
            HideDim();//消隐标注
            axEWdraw1.ToDrawOrbit();
        }

        private void axEWdraw1_ViewKeyUp(object sender, AxEWDRAWLib._DAdrawEvents_ViewKeyUpEvent e)
        {
            if (selids.Count > 0)
            {
                int tmpent, tmpent1, tmpent2, enttype, entsize;
                string str;
                tmpent = tmpent1 = tmpent2 = entsize = 0;
                enttype = -1;
                entsize = axEWdraw1.GetEntSize();
                ArrayList delids = new ArrayList();
                for (int i = 1; i <= entsize; i++)
                {
                    tmpent = axEWdraw1.GetEntID(i);
                    enttype = axEWdraw1.GetEntType(tmpent);
                    if ((enttype >= 93 && enttype <= 100) || enttype == 59 || enttype == 68)//2016-10-13 增加对承重墙的ID数据的判断
                    {
                        str = GetProStrFromEnt(tmpent, "id");
                        if (str != "")
                        {
                            tmpent1 = Convert.ToInt32(str);
                            for (int j = 0; j < selids.Count; j++)
                            {
                                tmpent2 = (int)selids[j];
                                if (tmpent1 == tmpent2)
                                {
                                    delids.Add(tmpent);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < delids.Count; i++)
                {
                    tmpent = (int)delids[i];
                    axEWdraw1.Delete(tmpent);
                }
                selids.Clear();
                //判断是否还有墙面
                int pipejs = 0;
                entsize = axEWdraw1.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    tmpent = axEWdraw1.GetEntID(i);
                    enttype = axEWdraw1.GetEntType(tmpent);
                    if (enttype == 59)
                    {
                        pipejs++;
                    }
                }
                if (pipejs > 0)
                {
                    MakeHomeArea(g_wallthickness);
                }
            }
        }

        /*ShowWeightWall隐藏面积对象
           * 参数:
           * 无
           * 返回值:
           * 无
        */
        private void ShowWeightWall()
        {
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(ent);
                    int inx = IsHaveStrField(str, "weightwall:");
                    if (inx >= 0)
                    {//如果是符号组
                        axEWdraw1.SetEntityInvisible(ent, false);
                        axEWdraw1.DeactivateEnt(ent);
                    }
                }
            }
        }

        /*HideWeightWall隐藏面积对象
           * 参数:
           * 无
           * 返回值:
           * 无
        */
        private void HideWeightWall()
        {
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(ent);
                    int inx = IsHaveStrField(str, "weightwall:");
                    if (inx >= 0)
                    {//如果是符号组
                        axEWdraw1.SetEntityInvisible(ent, true);
                    }
                }
            }
        }

        /*DelWeightWall隐藏面积对象
           * 参数:
           * id 输入 要删除的实体ID
           * 返回值:
           * 无
       */
        private void DelWeightWall(int id)
        {
            int entsize = axEWdraw1.GetEntSize();
            ArrayList tidlist = new ArrayList();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    string str = axEWdraw1.GetEntityUserData(ent);
                    int inx = IsHaveStrField(str, "weightwall:");
                    if (inx >= 0)
                    {//如果是符号组
                        string fstr = GetProStrFromEnt(ent, "id");
                        int tid = Convert.ToInt32(fstr);
                        if (tid == id)
                        {
                            tidlist.Add(tid);
                        }
                    }
                }
            }
            for (int i = 0; i < tidlist.Count; i++)
            {
                axEWdraw1.RemoveWallLimitSeg((int)tidlist[i]);//2016-10-14
                axEWdraw1.Delete((int)tidlist[i]);
            }
            //
            tidlist.Clear();
        }

        //根据ID号与段的索引,删除与之对应的标注
        private bool DelDimBySegInx(int id, int index)
        {
            //搜索现有的实体中的与该实体绑定的标注
            int entsize = axEWdraw1.GetEntSize();
            int enttype = -1;
            int tmpent = 0;
            string str = "";
            ArrayList idlist = new ArrayList();
            for (int i = 1; i <= entsize; i++)
            {
                tmpent = axEWdraw1.GetEntID(i);
                enttype = axEWdraw1.GetEntType(tmpent);
                if (enttype >= 93 && enttype <= 100)
                {
                    str = GetProStrFromEnt(tmpent, "id");
                    if (str != "")
                    {
                        if (Convert.ToInt32(str) == id)
                        {
                            str = GetProStrFromEnt(tmpent, "segnum");
                            if (Convert.ToInt32(str) == index)
                            {
                                idlist.Add(tmpent);
                            }
                        }
                    }
                }
            }
            if (idlist.Count > 0)
            {
                for (int i = 0; i < idlist.Count; i++)
                    axEWdraw1.Delete((int)idlist[i]);
            }
            return false;
        }

        /*设置墙面的厚度信息 2016-11-13
         * 
         */
        private bool SetThicknessToSolid(long id, double thickness)
        {
            int entsize = axEWdraw1.GetEntSize();
            string str = "thickness:" + String.Format("{0:f3}", thickness) + ";";
            //查找并设置实体的用户数据为点集
            for (int i = entsize; i >= 1; i--)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw1.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "thickness:");
                        if (ffinx >= 0)
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            string substr = orgstr + str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw1.SetEntityUserData(ent, str);
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
            int entsize = axEWdraw1.GetEntSize();
            string str = "height:" + String.Format("{0:f3}", height) + ";";
            //查找并设置实体的用户数据为点集
            for (int i = entsize; i >= 1; i--)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (ent == id)
                {
                    string orgstr = axEWdraw1.GetEntityUserData(ent);
                    if (orgstr != "")
                    {
                        int ffinx = IsHaveStrField(orgstr, "height:");
                        if (ffinx >= 0)
                        {
                            int feinx = orgstr.IndexOf(";", ffinx);
                            string substr = orgstr.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            substr += str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                        else
                        {
                            string substr = orgstr + str;
                            axEWdraw1.SetEntityUserData(ent, substr);
                        }
                    }
                    else
                        axEWdraw1.SetEntityUserData(ent, str);
                    return true;
                }
            }
            return false;
        }

        /*MakeSection 根据一个线段的起始点与终止点与给定的方向画出pipe所需截面 2016-10-31
         * 参数:
         * vx,vy,vz   线段或弧的朝向
         * x1,y1,z1   线段起点
         * x2,y2,z2   线段终点
         * thickness  墙的厚度
         * h          墙的高度
         * 返回值:
         * 如果成功返回ture,其它返回false
         */
        private int MakeSection(double vx, double vy, double vz, double x1, double y1, double z1, double x2, double y2, double z2, double thickness, double h)
        {
            double ox1, oy1, oz1;
            double ox2, oy2, oz2;
            double p1x, p1y, p1z;
            double p2x, p2y, p2z;
            ox1 = oy1 = oz1 = ox2 = oy2 = oz2 = 0.0;
            p1x = p1y = p1z = p2x = p2y = p2z = 0.0;
            axEWdraw1.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, 90, ref ox1, ref oy1, ref oz1);
            axEWdraw1.RotateVector(vx, vy, vz, x1, y1, z1, 0, 0, 1, -90, ref ox2, ref oy2, ref oz2);
            axEWdraw1.Polar(new object[] { x1, y1, z1 }, new object[] { ox2, oy2, oz2 }, thickness / 2.0, ref p1x, ref p1y, ref p1z);
            axEWdraw1.Polar(new object[] { x1, y1, z1 }, new object[] { ox1, oy1, oz1 }, thickness / 2.0, ref p2x, ref p2y, ref p2z);
            axEWdraw1.AddOne3DVertex(p1x, p1y, p1z);
            axEWdraw1.AddOne3DVertex(p2x, p2y, p2z);
            axEWdraw1.AddOne3DVertex(p2x, p2y, p2z + h);
            axEWdraw1.AddOne3DVertex(p1x, p1y, p1z + h);
            int ent = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            int face = axEWdraw1.EntToFace(ent, true);
            return face;
        }

        //处理两个不同实体间的相同结点 2016-11-06
        private void ProcKnote(int id1, int id2)
        {
            double x, y, z, x1, y1, z1, x2, y2, z2, dist, ox, oy, oz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = dist = ox = oy = oz = 0.0;
            ArrayList pts1 = new ArrayList();
            ArrayList pts2 = new ArrayList();
            GetPtsFromSolid(id1, ref pts1);
            GetPtsFromSolid(id2, ref pts2);
            ArrayList inspts = new ArrayList();
            if (pts1.Count > 0 && pts2.Count > 0)
            {
                //计算ID1在ID2上的点
                int segnum1 = pts1.Count / 3 - 1;
                int segnum2 = pts2.Count / 3 - 1;
                for (int i = 0; i < segnum1 + 1; i++)
                {
                    for (int j = 0; j < segnum2; j++)
                    {
                        x = (double)pts1[i * 3]; y = (double)pts1[i * 3 + 1]; z = (double)pts1[i * 3 + 2];
                        x1 = (double)pts2[j * 3]; y1 = (double)pts2[j * 3 + 1]; z1 = (double)pts2[j * 3 + 2];
                        x2 = (double)pts2[(j + 1) * 3]; y2 = (double)pts2[(j + 1) * 3 + 1]; z2 = (double)pts2[(j + 1) * 3 + 2];
                        axEWdraw1.GetDistPtAndLineSeg(new object[] { x, y, z }, new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 }, ref dist, ref ox, ref oy, ref oz);
                        if (dist < 0.001)
                        {
                            if (axEWdraw1.PointDistance(x1, y1, z1, ox, oy, oz) > 0.001 && axEWdraw1.PointDistance(x2, y2, z2, ox, oy, oz) > 0.001)
                            {
                                //这时的SInxWall结构是作为一个插入点的类处理的
                                SInxWall ainspt = new SInxWall();
                                ainspt.inx = j;
                                ainspt.x = ox;
                                ainspt.y = oy;
                                ainspt.z = oz;
                                ainspt.dist = axEWdraw1.PointDistance(x1, y1, z1, ox, oy, oz);
                                inspts.Add(ainspt);
                            }
                            break;
                        }
                    }
                }
                bool IsSameInx = false;
                if (inspts.Count >= 2)
                {
                    if (((SInxWall)inspts[0]).inx == ((SInxWall)inspts[1]).inx)
                    {
                        IsSameInx = true;
                    }
                }
                if (IsSameInx)
                {
                    myDistCompare InxWallCompare = new myDistCompare();
                    inspts.Sort(InxWallCompare);
                }
                else
                {
                    myInxWallCompare InxWallCompare = new myInxWallCompare();
                    inspts.Sort(InxWallCompare);
                }
                //处理插入点
                int tmpjs = 0;
                for (int i = 0; i < inspts.Count; i++)
                {
                    SInxWall ainspt = (SInxWall)inspts[i];
                    pts2.Insert((ainspt.inx + tmpjs + 1) * 3, ainspt.x);
                    pts2.Insert((ainspt.inx + tmpjs + 1) * 3 + 1, ainspt.y);
                    pts2.Insert((ainspt.inx + tmpjs + 1) * 3 + 2, ainspt.z);
                    tmpjs++;
                }
                SetPtsToSolid(id2, ref pts2);
                double thickness = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(id2, "thickness"));
                double height = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(id2, "height"));
                axEWdraw1.ReBuildWall(id2, thickness, height);
                if (tmpjs > 0)
                    axEWdraw1.SplitWall(id2);//2016-11-12
                else
                {
                    if (!axEWdraw1.IsHaveSingleWall(id2))//如果没有交点,且该实体没有拆分过
                        axEWdraw1.SplitWall(id2);
                }
                //
                inspts.Clear();
                //
                //计算ID2在ID1上的点
                for (int i = 0; i < segnum2 + 1; i++)
                {
                    for (int j = 0; j < segnum1; j++)
                    {
                        x = (double)pts2[i * 3]; y = (double)pts2[i * 3 + 1]; z = (double)pts2[i * 3 + 2];
                        x1 = (double)pts1[j * 3]; y1 = (double)pts1[j * 3 + 1]; z1 = (double)pts1[j * 3 + 2];
                        x2 = (double)pts1[(j + 1) * 3]; y2 = (double)pts1[(j + 1) * 3 + 1]; z2 = (double)pts1[(j + 1) * 3 + 2];
                        axEWdraw1.GetDistPtAndLineSeg(new object[] { x, y, z }, new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 }, ref dist, ref ox, ref oy, ref oz);
                        if (dist < 0.001)
                        {
                            if (axEWdraw1.PointDistance(x1, y1, z1, ox, oy, oz) > 0.001 && axEWdraw1.PointDistance(x2, y2, z2, ox, oy, oz) > 0.001)
                            {
                                //这时的SInxWall结构是作为一个插入点的类处理的
                                SInxWall ainspt = new SInxWall();
                                ainspt.inx = j;
                                ainspt.x = ox;
                                ainspt.y = oy;
                                ainspt.z = oz;
                                inspts.Add(ainspt);
                            }
                            break;
                        }
                    }
                }
                //处理插入点
                tmpjs = 0;
                //点按索引排序
                myInxWallCompare InxWallCompare1 = new myInxWallCompare();
                inspts.Sort(InxWallCompare1);
                //
                for (int i = 0; i < inspts.Count; i++)
                {
                    SInxWall ainspt = (SInxWall)inspts[i];
                    pts1.Insert((ainspt.inx + tmpjs + 1) * 3, ainspt.x);
                    pts1.Insert((ainspt.inx + tmpjs + 1) * 3 + 1, ainspt.y);
                    pts1.Insert((ainspt.inx + tmpjs + 1) * 3 + 2, ainspt.z);
                    tmpjs++;
                }
                thickness = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(id2, "thickness"));
                height = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(id2, "height"));
                SetPtsToSolid(id1, ref pts1);
                axEWdraw1.ReBuildWall(id1, thickness, height);
                if (tmpjs > 0)
                    axEWdraw1.SplitWall(id1);//2016-11-12    
                else
                {//判断是否已经折墙
                    if (!axEWdraw1.IsHaveSingleWall(id1))
                        axEWdraw1.SplitWall(id1);//2016-11-12
                }
                //
                pts1.Clear();
                pts2.Clear();
                inspts.Clear();
            }
        }

        //删除一个ID实体墙的点的数据,并删除相关可删除节点 2016-11-06
        private void DelPtData(int id, double x, double y, double z)
        {
            ArrayList pts = new ArrayList();
            ArrayList pts1 = new ArrayList();
            ArrayList segs = new ArrayList();
            int entsize = axEWdraw1.GetEntSize();
            int ent = 0;
            double dst = 0.0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                if (id != ent)
                {
                    int type = axEWdraw1.GetEntType(ent);
                    if (type == 59)
                    {
                        GetPtsFromSolid(ent, ref pts);
                        int ptsize = pts.Count / 3;
                        for (int j = 0; j < ptsize; j++)
                        {
                            dst = axEWdraw1.PointDistance(x, y, z, (double)pts[j * 3], (double)pts[j * 3 + 1], (double)pts[j * 3 + 2]);
                            if (dst < 0.001)
                            {
                                if ((j - 1) >= 0)
                                {
                                    SWallSeg aseg = new SWallSeg();
                                    aseg.id = ent;
                                    aseg.inx = j - 1;
                                    aseg.x1 = (double)pts[(j - 1) * 3];
                                    aseg.y1 = (double)pts[(j - 1) * 3 + 1];
                                    aseg.z1 = (double)pts[(j - 1) * 3 + 2];
                                    //
                                    aseg.x2 = (double)pts[j * 3];
                                    aseg.y2 = (double)pts[j * 3 + 1];
                                    aseg.z2 = (double)pts[j * 3 + 2];

                                    aseg.vx = aseg.x2 - aseg.x1;
                                    aseg.vy = aseg.y2 - aseg.y1;
                                    aseg.vz = aseg.z2 - aseg.z1;
                                    segs.Add(aseg);
                                }
                                if ((j + 1) < ptsize)
                                {
                                    SWallSeg aseg = new SWallSeg();
                                    aseg.id = ent;
                                    aseg.inx = j;

                                    aseg.x1 = (double)pts[j * 3];
                                    aseg.y1 = (double)pts[j * 3 + 1];
                                    aseg.z1 = (double)pts[j * 3 + 2];

                                    aseg.x2 = (double)pts[(j + 1) * 3];
                                    aseg.y2 = (double)pts[(j + 1) * 3 + 1];
                                    aseg.z2 = (double)pts[(j + 1) * 3 + 2];
                                    //
                                    aseg.vx = aseg.x2 - aseg.x1;
                                    aseg.vy = aseg.y2 - aseg.y1;
                                    aseg.vz = aseg.z2 - aseg.z1;
                                    segs.Add(aseg);
                                }
                            }
                        }
                    }
                }//
            }
            if (segs.Count > 0)
            {
                double ang = 0.0;
                bool isdel = true;
                if (segs.Count > 2)
                {
                    for (int i = 0; i < segs.Count; i++)
                    {
                        if (i == 0)
                        {
                            ang = axEWdraw1.VectorAngle(new object[] { ((SWallSeg)segs[0]).vx, ((SWallSeg)segs[0]).vy, ((SWallSeg)segs[0]).vz }, new object[] { ((SWallSeg)segs[1]).vx, ((SWallSeg)segs[1]).vy, ((SWallSeg)segs[1]).vz });
                        }
                        else if (i > 1)
                        {
                            double organg = axEWdraw1.VectorAngle(new object[] { ((SWallSeg)segs[0]).vx, ((SWallSeg)segs[0]).vy, ((SWallSeg)segs[0]).vz }, new object[] { ((SWallSeg)segs[i]).vx, ((SWallSeg)segs[i]).vy, ((SWallSeg)segs[i]).vz });
                            if (!(Math.Abs(ang - organg) < 0.001 || Math.Abs(Math.Abs(ang - organg) - Math.PI) < 0.001))
                            {
                                isdel = false;
                                break;
                            }
                        }
                    }
                }
                else if (segs.Count == 2)
                {
                    ang = axEWdraw1.VectorAngle(new object[] { ((SWallSeg)segs[0]).vx, ((SWallSeg)segs[0]).vy, ((SWallSeg)segs[0]).vz }, new object[] { ((SWallSeg)segs[1]).vx, ((SWallSeg)segs[1]).vy, ((SWallSeg)segs[1]).vz });
                    if (!(Math.Abs(ang) < 0.001 || Math.Abs(ang - Math.PI) < 0.001))
                        isdel = false;

                }
                else
                {
                    isdel = false;
                }
                //
                if (isdel)
                {
                    int oldid = 0;
                    for (int i = 0; i < segs.Count; i++)
                    {
                        if (oldid != ((SWallSeg)segs[i]).id)
                        {
                            //
                            GetPtsFromSolid(((SWallSeg)segs[i]).id, ref pts1);
                            int ptsize = pts1.Count / 3;
                            for (int j = 0; j < ptsize; j++)
                            {
                                dst = axEWdraw1.PointDistance(x, y, z, (double)pts1[j * 3], (double)pts1[j * 3 + 1], (double)pts1[j * 3 + 2]);
                                if (dst < 0.001)
                                {
                                    pts1.RemoveRange(j * 3, 3);
                                    break;
                                    //
                                }
                            }
                            //
                            double thickness = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(((SWallSeg)segs[i]).id, "thickness"));
                            double height = zPubFun.zPubFunLib.CStr2Double(GetProStrFromEnt(((SWallSeg)segs[i]).id, "height"));
                            SetPtsToSolid(((SWallSeg)segs[i]).id, ref pts1);
                            axEWdraw1.ReBuildWall(((SWallSeg)segs[i]).id, thickness, height);
                            axEWdraw1.SplitWall(((SWallSeg)segs[i]).id);//2016-12-16
                                                                        //
                            pts1.Clear();
                            oldid = ((SWallSeg)segs[i]).id;
                        }
                    }
                }
            }
            //清空
            pts.Clear();
            segs.Clear();
        }

        /*SetProStrFromEnt从实体中设置某段名的字符串(浮点值)
         * 参数:
         * ent   输入 实体的ID号
         * filed 输入 段名
         * val   输入 实际值
         * 返回值:
         * 如果成功,则返true.其它false.
         */
        private bool SetProStrFromEnt(int ent, string field, double value)
        {
            string prostr = "";
            if (ent > 0)
            {
                string str = axEWdraw1.GetEntityUserData(ent);
                if (str.Length > 0)
                {
                    string tfieldname = field + ":";
                    int tlen = tfieldname.Length;
                    int ffinx = IsHaveStrField(str, tfieldname);//段开始的位置 2017-09-21
                    if (ffinx >= 0)
                    {
                        int feinx = str.IndexOf(";", ffinx);//段结束的位置
                        if (feinx >= 0)
                        {
                            prostr = str.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            prostr += tfieldname + String.Format("{0:f3}", value) + ";";
                            axEWdraw1.SetEntityUserData(ent, prostr);
                            return true;
                        }
                    }
                    else
                    {
                        prostr = str + tfieldname + String.Format("{0:f3}", value) + ";";
                        axEWdraw1.SetEntityUserData(ent, prostr);
                        return true;
                    }
                }
            }
            return false;
        }

        /*SetProStrFromEnt从实体中设置某段名的字符串(整数值)
         * 参数:
         * ent   输入 实体的ID号
         * filed 输入 段名
         * val   输入 实际值
         * 返回值:
         * 如果成功,则返true.其它false.
         */
        private bool SetProStrFromEnt(int ent, string field, int value)
        {
            string prostr = "";
            if (ent > 0)
            {
                string str = axEWdraw1.GetEntityUserData(ent);
                if (str.Length > 0)
                {
                    string tfieldname = field + ":";
                    int tlen = tfieldname.Length;
                    int ffinx = IsHaveStrField(str, tfieldname);//段开始的位置 2017-09-21
                    if (ffinx >= 0)
                    {
                        int feinx = str.IndexOf(";", ffinx);//段结束的位置
                        if (ffinx >= 0 && feinx >= 0)
                        {
                            prostr = str.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            prostr += tfieldname + value.ToString() + ";";
                            axEWdraw1.SetEntityUserData(ent, prostr);
                            return true;
                        }
                    }
                    else
                    {
                        prostr = str + tfieldname + value.ToString() + ";";
                        axEWdraw1.SetEntityUserData(ent, prostr);
                        return true;
                    }
                }
                else
                {
                    string tfieldname = field + ":";
                    prostr = str + tfieldname + value.ToString() + ";";
                    axEWdraw1.SetEntityUserData(ent, prostr);
                }
            }
            return false;
        }

        /*SetProStrFromEnt从实体中设置某段名的字符串(字符串)
         * 参数:
         * ent   输入 实体的ID号
         * filed 输入 段名
         * val   输入 实际值
         * 返回值:
         * 如果成功,则返true.其它false.
         */
        private bool SetProStrFromEnt(int ent, string field, string value)
        {
            string prostr = "";
            if (ent > 0)
            {
                string str = axEWdraw1.GetEntityUserData(ent);
                if (str.Length > 0)
                {
                    string tfieldname = field + ":";
                    int tlen = tfieldname.Length;
                    int ffinx = IsHaveStrField(str, tfieldname);//段开始的位置 2017-09-21
                    if (ffinx >= 0)
                    {
                        int feinx = str.IndexOf(";", ffinx);//段结束的位置
                        if (ffinx >= 0 && feinx >= 0)
                        {
                            prostr = str.Remove(ffinx, feinx - ffinx + 1);//2016-10-08
                            prostr += tfieldname + value + ";";
                            axEWdraw1.SetEntityUserData(ent, prostr);
                            return true;
                        }
                    }
                    else
                    {
                        prostr = str + tfieldname + value + ";";
                        axEWdraw1.SetEntityUserData(ent, prostr);
                        return true;
                    }
                }
                else
                {
                    string tfieldname = field + ":";
                    prostr = str + tfieldname + value + ";";
                    axEWdraw1.SetEntityUserData(ent, prostr);
                }
            }
            return false;
        }


        private void axEWdraw1_ViewMouseMove(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseMoveEvent e)
        {
            g_x = Math.Round(e.xCoordinate, 3); g_y = Math.Round(e.yCoordinate, 3); g_z = Math.Round(e.zCoordinate, 3);
            if (!this.Focused)
            {
                if (!textBox1.Visible)
                {
                    if (!axEWdraw1.Focused)
                        axEWdraw1.Focus();
                }
            }
            if (isdrawpolyline)
            {//2019-02-09
                if (!axEWdraw1.Focused && !textBox1.Visible)
                {
                    axEWdraw1.Focus();
                }
            }

            switch (g_state)
            {//为之后的消息处理做准备
                case InteractiveState.MoveingWall:
                    {
                        axEWdraw1.MoveWall(e.xCoordinate, e.yCoordinate);
                    }
                    break;

            }
            //2018-01-24
            if (isdrawaxismove || isdrawrotate)
            {
                g_dimposx = e.xWin;
                g_dimposy = e.yWin;
            }
        }

        private static string InputBox(string Caption, string Hint, string Default)
        {
            Form InputForm = new Form();
            InputForm.MinimizeBox = false;
            InputForm.MaximizeBox = false;
            InputForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            InputForm.StartPosition = FormStartPosition.CenterScreen;
            InputForm.Width = 260;
            InputForm.Height = 150;

            InputForm.Text = Caption;
            Label lbl = new Label();
            lbl.Text = Hint;
            lbl.Left = 10;
            lbl.Top = 20;
            lbl.Parent = InputForm;
            lbl.AutoSize = true;
            TextBox tb = new TextBox();
            tb.Left = 30;
            tb.Top = 45;
            tb.Width = 160;
            tb.Parent = InputForm;
            tb.Text = Default;
            tb.SelectAll();
            Button btnok = new Button();
            btnok.Left = 30;
            btnok.Top = 80;
            btnok.Parent = InputForm;
            btnok.Text = "确定";
            InputForm.AcceptButton = btnok;//回车响应  

            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button();
            btncancal.Left = 120;
            btncancal.Top = 80;
            btncancal.Parent = InputForm;
            btncancal.Text = "取消";
            btncancal.DialogResult = DialogResult.Cancel;
            try
            {
                if (InputForm.ShowDialog() == DialogResult.OK)
                {
                    return tb.Text;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                InputForm.Dispose();
            }
        }

        //绘制单间
        private void SingleRoomView()//button15_Click(object sender, EventArgs e)
        {
            if (g_viewmode == 0)
            {//需要从平面图转向单间
                if (!axEWdraw1.IsEndCommand())
                {//如果命令没有结束,则判断,是否有lastsym的实体
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }
                canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
                axEWdraw1.CancelCommand();
                label1.Text = "请选择一个房间区域";
                g_viewmode = 2;
                if (ProcSingleRoom())
                {
                    label1.Text = "";
                    axEWdraw1.ToDrawOrbit();
                }
                else
                {
                    g_viewmode = 0;
                    label1.Text = "";
                    int ent = 0;
                    int type = -1;
                    string str;
                    int tmpv = 0;
                    int entsize = axEWdraw1.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        ent = axEWdraw1.GetEntID(i);
                        type = axEWdraw1.GetEntType(ent);
                        if (type == 50)
                        {
                            str = GetProStrFromEnt(ent, "area");
                            if (str.Length > 0)
                            {
                                tmpv = Convert.ToInt32(str);
                                if (tmpv == 1)
                                {
                                    axEWdraw1.DeactivateEnt(ent);
                                }
                            }
                        }
                    }
                    //
                    axEWdraw1.SetGridValue(300, 300, 15000, 15000, 0);
                    axEWdraw1.SetGridOrgPt(-15000, -15000);
                    axEWdraw1.SetGridOn(true);
                }
            }
        }
        //隐藏单间地面
        private void HideRoof()
        {
            int ent = 0;
            int isroof = 0;
            string str = "";
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                str = GetProStrFromEnt(ent, "floor");
                if (str.Length > 0)
                {
                    isroof = Convert.ToInt32(str);
                    if (isroof == 1)
                    {
                        axEWdraw1.SetEntityInvisible(ent, true);
                    }
                }
            }
        }
        //隐藏单个方间 2016-11-14
        private void RestoreRoom()
        {
            HideRoof();
            int ent = 0;
            string str = "";
            axEWdraw1.ClearSelected();
            axEWdraw1.EnableHideWall(false);
            //恢复隐藏的平面实体
            for (int i = 0; i < singroomhides.Count; i++)
            {
                ent = (int)singroomhides[i];
                axEWdraw1.SetEntityInvisible(ent, false);
                //
                str = GetProStrFromEnt(ent, "area");
                if (str.Length > 0)
                {
                    if (Convert.ToInt32(str) == 1)
                    {
                        axEWdraw1.DeactivateEnt(ent);
                    }
                }
            }
            //恢复隐藏的平面租符号
            for (int i = 0; i < symbolidlist.Count; i++)
            {
                ent = (int)symbolidlist[i];
                axEWdraw1.SetEntityInvisible(ent, false);
            }
            //删除门窗定义
            for (int i = 0; i < door3dsidlist.Count; i++)
            {
                axEWdraw1.Delete((int)door3dsidlist[i]);
            }
            //显示面积文字
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                int type = axEWdraw1.GetEntType(ent);
                if (ent > 0 && type == 78)//显示文字
                {
                    axEWdraw1.SetEntityInvisible(ent, false);
                    axEWdraw1.DeactivateEnt(ent);
                }
                else if (ent > 0 && type == 68)
                {
                    axEWdraw1.SetEntityInvisible(ent, true);
                }
            }
            //改变单墙实体集颜色
            if (singlewallids.Count > 0)
            {
                for (int i = 0; i < singlewallids.Count; i++)
                {
                    axEWdraw1.SetWallColor((int)singlewallids[i], g_wr, g_wg, g_wb);//设置墙体颜色,这里是白色,要和顶底的颜色相同
                    if (!axEWdraw1.IsDisplayed((int)singlewallids[i]))
                    {
                        int type = axEWdraw1.GetEntType((int)singlewallids[i]);
                        if (type != 68)
                        {
                            axEWdraw1.SetSingleWallToOutSide((int)singlewallids[i], false);
                            axEWdraw1.SetEntityInvisible((int)singlewallids[i], false);
                        }

                    }
                }
            }
            singlewallids.Clear();
            //清隐矮墙
            if (dwarfewallids.Count > 0)
            {
                for (int i = 0; i < dwarfewallids.Count; i++)
                {
                    axEWdraw1.SetWallColor((int)dwarfewallids[i], g_wr, g_wg, g_wb);//设置墙体颜色,这里是白色,要和顶底的颜色相同
                    axEWdraw1.SetEntityInvisible((int)dwarfewallids[i], true);
                }
            }
            dwarfewallids.Clear();
            //清除走向实体
            if (topbottomids.Count > 0)
            {
                for (int i = 0; i < topbottomids.Count; i++)
                {
                    axEWdraw1.Delete((int)topbottomids[i]);
                }
            }
            if (otherroomcabinet.Count > 0)
            {
                for (int i = 0; i < otherroomcabinet.Count; i++)
                {
                    axEWdraw1.SetEntityInvisible((int)otherroomcabinet[i], false);
                }
            }
            if (furniturehides.Count > 0)
            {
                for (int i = 0; i < furniturehides.Count; i++)
                {
                    axEWdraw1.SetEntityInvisible((int)furniturehides[i], false);//设置墙体颜色,这里是白色,要和顶底的颜色相同
                }
            }
            //判断是否有阳台对象 2017-12-06
            if (g_balconyids != null)
            {
                if (g_balconyids.Count > 0)
                {
                    for (int i = 0; i < g_balconyids.Count; i++)
                        axEWdraw1.Delete((int)g_balconyids[i]);
                }
            }
            if (g_balconyotherids != null)
            {
                if (g_balconyotherids.Count > 0)
                {
                    for (int i = 0; i < g_balconyotherids.Count; i++)
                        axEWdraw1.Delete((int)g_balconyotherids[i]);
                }
                g_balconyotherids.Clear();
            }
            //
            if (g_balconyids != null)
                g_balconyids.Clear();
            if (g_balconypts != null)
                g_balconypts.Clear();
            if (g_balconysegpts != null)
                g_balconysegpts.Clear();
            if (g_balconyconnectpts != null)
                g_balconyconnectpts.Clear();
            if (g_balconyhidewall != null)
                g_balconyhidewall.Clear();
            g_balconyent = 0;
            //
            otherroomcabinet.Clear();
            topbottomids.Clear();
            symbolidlist.Clear();
            door3dsidlist.Clear();
            singroomhides.Clear();
            furniturehides.Clear();//2017-08-17
            //显示边界框
            axEWdraw1.SetEntityInvisible(g_limitrectangle, false);
            axEWdraw1.DeactivateEnt(g_limitrectangle);
            //
            axEWdraw1.SetGridOn(true);//打开栅格
        }

        private bool ProcSingleRoom()
        {
            int entsize = axEWdraw1.GetEntSize();
            int ent = 0;
            int topbootom = 0;
            int type = -1;
            int tmpv = 0;
            string str = "";
            double x, y, z, vx, vy, vz;
            int rooment = 0;
            int floorent = 0;
            x = y = z = vx = vy = vz = 0.0;
            axEWdraw1.ClearSelected();
            singlewallids.Clear();//清空单墙ID缓存
            topbottomids.Clear();//走向实体ID缓存
                                 //为了选择一个区域对象,使不可选的区域对象可选
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    str = GetProStrFromEnt(ent, "area");
                    if (str.Length > 0)
                    {
                        tmpv = Convert.ToInt32(str);
                        if (tmpv == 1)
                        {
                            axEWdraw1.ActivateEnt(ent);
                        }
                    }
                }
            }
            //设置纹理实际大小
            axEWdraw1.SetTexFaceTextureUVSize(true, 1000, g_wallheight);
            //选择一个一个房间的区域
            int selarea = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            if (selarea > 0)
            {
                //关闭网格显示
                axEWdraw1.SetGridOn(false);
                //不显示其它区域内的家具 2017-03-01
                HideSingleRoomCabinet(selarea, ref otherroomcabinet);
                //判断是否为区域象
                str = GetProStrFromEnt(selarea, "area");
                if (str.Length > 0)
                {
                    tmpv = Convert.ToInt32(str);
                    if (tmpv == 1)
                    {
                        //取得范围的顶底面
                        topbottomids.Add(topbootom);
                        //
                        //取得该范围的单墙,并将所有单墙的ID号放在buffer中
                        axEWdraw1.GetSingleWallsFromArea(selarea);
                        //设置透明度为0,因为有纹理,就不用透明了
                        axEWdraw1.ActivateEnt(selarea);
                        axEWdraw1.SetTransparency(selarea, 0);
                        //设置纹理
                        if (!axEWdraw1.IsHaveTexture(selarea))//如果没有纹理,就给定一个默认的纹理
                        {
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            //得到地板的范围
                            axEWdraw1.GetEntBoundingBox(selarea, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            //计算地板的重复率
                            double ureapt = (maxx - minx) / 1600.0;
                            double vreapt = (maxy - miny) / 1600.0;
                            axEWdraw1.SetEntTexture(selarea, "floor_2.png", 1, 1, ureapt, vreapt, 0, 0);
                        }
                        //                         
                        int swallsize = axEWdraw1.GetIDBufferSize();
                        int swallent = 0;

                        //该房间墙面所涉及的门窗 2017-02-17
                        ArrayList symbols = new ArrayList();
                        //
                        if (swallsize > 0)
                        {
                            axEWdraw1.SetSingleWallToOutSide(0, false);//在这个函数中ID是0时,为初始化所有外墙
                            for (int i = 0; i < swallsize; i++)
                            {
                                swallent = axEWdraw1.GetIDBuffer(i);
                                if (swallent > 0)
                                {
                                    int swinx = 0;//2017-02-17
                                    int swsymsize = 0;//2017-02-17
                                    swsymsize = axEWdraw1.GetSingleWallSymbolSize(swallent, ref swinx);
                                    if (swsymsize > 0)
                                    {
                                        for (int j = 0; j < swsymsize; j++)
                                        {
                                            int symid = axEWdraw1.GetSingleWallSymbol(swinx, j);
                                            symbols.Add(symid);
                                        }
                                    }
                                    singlewallids.Add(swallent);
                                    //得到单墙的方向
                                    axEWdraw1.GetDirOfSingleWall(selarea, swallent, ref vx, ref vy, ref vz);
                                    //设置单墙为外墙(在单间的情况下,全局有另一种判断)
                                    axEWdraw1.SetSingleWallToOutSide(swallent, true);
                                    //设置墙体颜色,这里是白色,要和顶底的颜色相同
                                    axEWdraw1.SetWallColor(swallent, 255, 255, 255);
                                    //设置壁纸颜色
                                    if (!axEWdraw1.IsHaveTexFaceByDir(swallent, vx, vy, vz))
                                        axEWdraw1.SetSingleWallInsideTexture(swallent, "wall_def.jpg", 0, 0, 5, 5, 1.0, 1.0);
                                }
                            }
                        }

                        //消隐其它实体
                        entsize = axEWdraw1.GetEntSize();
                        for (int i = 1; i <= entsize; i++)
                        {
                            ent = axEWdraw1.GetEntID(i);
                            type = axEWdraw1.GetEntType(ent);
                            bool isfswid = false;//判断是否是该房间的墙面
                            if (type == 66 || type == 68)//单墙类型
                            {
                                for (int j = 0; j < singlewallids.Count; j++)
                                {
                                    int swid = (int)singlewallids[j];
                                    if (swid == ent)
                                    {
                                        isfswid = true;
                                        break;
                                    }
                                }
                                if (!isfswid)
                                {
                                    if (type != 68)
                                    {
                                        singlewallids.Add(ent);
                                        axEWdraw1.SetEntityInvisible(ent, true);
                                    }
                                }
                            }
                            else if (type != 66 && type != 68)
                            {
                                if (axEWdraw1.IsGroup(ent))
                                {
                                    //判断是否是符号组
                                    string grpname = axEWdraw1.GetGroupName(ent);
                                    int inx = grpname.IndexOf("symbol");
                                    if (inx >= 0)
                                    {//如果是符号组
                                     //判断是否是该房间内的门窗 2017-02-17
                                        bool isinsidesym = false;
                                        if (symbols.Count > 0)
                                        {
                                            for (int j = 0; j < symbols.Count; j++)
                                            {
                                                if (((int)symbols[j]) == ent)
                                                {
                                                    isinsidesym = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //如果是房间内的门窗
                                        if (isinsidesym)
                                        {
                                            //
                                            string[] strarr = grpname.Split('_');
                                            if (strarr[1] == "door")
                                            {//判断是否是门
                                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                                //取得宽度
                                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                                //取得厚度
                                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                                //取得高度
                                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                                //也就是说,这些参数与3ds文件是一对一的关系.
                                                //2018-11-14
                                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                                string oname = "";
                                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                                string ngrpname = "door_" + oname;
                                                //
                                                int grpid = Import3DsDoor(filename, ngrpname, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, thickness, height, true);
                                                if (grpid > 0)
                                                {

                                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                                    //
                                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                                       //增加到数组中
                                                    door3dsidlist.Add(grpid);
                                                }
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                            else if (strarr[1] == "window")
                                            {
                                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                                //取得宽度
                                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                                //取得厚度
                                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                                //取得高度
                                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                                //也就是说,这些参数与3ds文件是一对一的关系.
                                                //2018-11-15
                                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                                string oname = "";
                                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                                string ngrpname = "window_" + oname;
                                                //
                                                int grpid = Import3DsWindow(filename, ngrpname, 0, 0, 0, 0, 0, 0, 0, width, thickness, height, true);
                                                if (grpid > 0)
                                                {

                                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                                    //
                                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                                       //增加到数组中
                                                    door3dsidlist.Add(grpid);
                                                }
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                            else if (strarr[1] == "baywin")
                                            {
                                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                                //取得宽度
                                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                                //取得厚度
                                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                                //取得高度
                                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                                //飘窗的深度
                                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                                //也就是说,这些参数与3ds文件是一对一的关系.
                                                int grpid = Import3DsBayWin("window.3ds", "baywin_1", 0, 0, 0, 0, 0, 0, 0, width, g_wallthickness, depth, height, true, false);
                                                if (grpid > 0)
                                                {

                                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                                    //
                                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                                       //增加到数组中
                                                    door3dsidlist.Add(grpid);
                                                }
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                            else if (strarr[1] == "sevenwin")
                                            {
                                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                                //取得宽度
                                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                                //取得厚度
                                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                                //取得高度
                                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                                //飘窗的深度
                                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                                //也就是说,这些参数与3ds文件是一对一的关系.
                                                int grpid = Import3DsSevenWin(ent, "ldc.3ds", "sevenwin_1");
                                                if (grpid > 0)
                                                {
                                                    //增加到数组中
                                                    door3dsidlist.Add(grpid);
                                                }
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                            else if (strarr[1] == "profilewin")
                                            {
                                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                                //取得宽度
                                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                                //取得厚度
                                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                                if (thickness < 0.001)
                                                    thickness = g_wallthickness;
                                                //取得高度
                                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                                //也就是说,这些参数与3ds文件是一对一的关系.
                                                int grpid = Import3DsProfileWin("profilewin.3ds", "profilewin_1", 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, 80, height, true);
                                                if (grpid > 0)
                                                {

                                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                                    //
                                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                                       //增加到数组中
                                                    door3dsidlist.Add(grpid);
                                                }
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                            else if (strarr[1] == "hole")
                                            {
                                                axEWdraw1.SetEntityInvisible(ent, true);
                                            }
                                        }
                                        else
                                        {
                                            axEWdraw1.SetEntityInvisible(ent, true);
                                        }
                                    }
                                }
                                else
                                {
                                    if (ent != selarea)
                                        axEWdraw1.SetEntityInvisible(ent, true);
                                }
                            }
                        }
                        symbols.Clear();//清空房间内门窗的ID集2017-02-07
                        //创建阳台
                        MakeBalcony(ref g_balconyids);
                        //启动消隐墙
                        axEWdraw1.EnableHideWall(true);
                        //
                        axEWdraw1.ClearSelected();
                        //
                        if (!timer6.Enabled)//2018-08-03
                            timer6.Enabled = true;

                    }//
                }
                else return false;
            }

            //关闭纹理的实际大小设置
            axEWdraw1.SetTexFaceTextureUVSize(false, 1000, g_wallheight);
            //
            entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    str = GetProStrFromEnt(ent, "area");
                    if (str.Length > 0)
                    {
                        tmpv = Convert.ToInt32(str);
                        if (tmpv == 1)
                        {
                            axEWdraw1.DeactivateEnt(ent);
                        }
                    }
                }
            }
            //
            //
            axEWdraw1.SetViewCondition(8);
            axEWdraw1.SetPerspectiveMode(true);
            axEWdraw1.ZoomALL();
            return true;
        }
        //显示全局时,先要处理哪些墙显示,那些墙不显示
        private void ProcAllSingleWall()
        {
            int ent = 0;
            int type = -1;
            int topbootom = 0;
            double x1, y1, x2, y2, mx, my;
            x1 = y1 = x2 = y2 = mx = my = 0.0;
            g_viewmode = 3;//全局
            //关闭网格
            axEWdraw1.SetGridOn(false);
            //消隐其它实体
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                bool isfswid = false;//判断是否是该房间的墙面
                if (type != 66 && type != 68)
                {
                    if (!axEWdraw1.IsGroup(ent))
                    {
                        axEWdraw1.SetEntityInvisible(ent, true);
                    }
                }
            }
            //设置纹理的真实大小
            axEWdraw1.SetTexFaceTextureUVSize(true, 1000, g_wallheight);
            int len = axEWdraw1.GetEntSize();
            //2018-10-24 记录地面对象
            ArrayList tmpareas = new ArrayList();
            //从区域对象中得到墙线坐标信息,并判断是否是共享墙面
            ArrayList singlewalls = new ArrayList();
            for (int i = 1; i <= len; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    string str = GetProStrFromEnt(ent, "area");
                    if (str.Length > 0)
                    {
                        tmpareas.Add(ent);//2018-10-24
                        int roof = axEWdraw1.MakeRoof(ent, g_wallheight, 100);
                        if (roof > 0)
                            axEWdraw1.SetRGBColor(roof, 220, 220, 220);
                        g_roofids.Add(roof);//2018-12-03
                        //
                        if (Convert.ToInt32(str) == 1)
                        {
                            ArrayList pts = new ArrayList();
                            GetPtsFromSolid(ent, ref pts);
                            if (pts.Count > 0)
                            {
                                int ptslen = pts.Count / 3 - 1;
                                for (int j = 0; j < ptslen; j++)
                                {
                                    x1 = (double)pts[j * 3];
                                    y1 = (double)pts[j * 3 + 1];
                                    x2 = (double)pts[(j + 1) * 3];
                                    y2 = (double)pts[(j + 1) * 3 + 1];
                                    mx = (x1 + x2) / 2.0;
                                    my = (y1 + y2) / 2.0;
                                    int finx = -1;
                                    if (singlewalls.Count > 0)
                                    {
                                        for (int k = 0; k < singlewalls.Count; k++)
                                        {
                                            if (Math.Abs(mx - ((SWallSeg)singlewalls[k]).vx) < 0.001 &&
                                                Math.Abs(my - ((SWallSeg)singlewalls[k]).vy) < 0.001
                                                )//这里的vx,vy当保存中间点使用
                                            {//相同的线段
                                                ((SWallSeg)singlewalls[k]).inx++;
                                                finx = k;
                                                break;
                                            }
                                        }
                                        if (finx >= 0)
                                        {
                                            ((SWallSeg)singlewalls[finx]).id1 = ent;//第二个相关id
                                        }
                                        else
                                        {
                                            SWallSeg aswall = new SWallSeg();
                                            aswall.x1 = x1;
                                            aswall.y1 = y1;
                                            aswall.x2 = x2;
                                            aswall.y2 = y2;
                                            aswall.vx = mx;
                                            aswall.vy = my;
                                            aswall.id = ent;
                                            singlewalls.Add(aswall);
                                        }
                                    }
                                    else
                                    {
                                        SWallSeg aswall = new SWallSeg();
                                        aswall.x1 = x1;
                                        aswall.y1 = y1;
                                        aswall.x2 = x2;
                                        aswall.y2 = y2;
                                        aswall.vx = mx;
                                        aswall.vy = my;
                                        aswall.id = ent;
                                        singlewalls.Add(aswall);
                                    }
                                    //                                     
                                }
                            }
                            pts.Clear();
                            if (!axEWdraw1.IsDisplayed(ent))
                            {
                                axEWdraw1.SetEntityInvisible(ent, false);
                            }
                            //
                            axEWdraw1.SetTransparency(ent, 0);
                            if (!axEWdraw1.IsHaveTexture(ent))//如果没有纹理,就给定一个默认的纹理
                            {
                                double minx, miny, minz, maxx, maxy, maxz;
                                minx = miny = minz = maxx = maxy = maxz = 0.0;
                                axEWdraw1.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                //计算地板的重复率
                                double ureapt = (maxx - minx) / 1600.0;
                                double vreapt = (maxy - miny) / 1600.0;
                                axEWdraw1.SetEntTexture(ent, "floor_2.png", 1, 1, ureapt, vreapt, 0, 0);
                            }
                            //取得范围的顶底面
                            topbottomids.Add(topbootom);
                        }
                    }
                }
                else if (type != 66 && type != 68)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        //判断是否是符号组
                        string grpname = axEWdraw1.GetGroupName(ent);
                        int inx = grpname.IndexOf("symbol");
                        if (inx >= 0)
                        {//如果是符号组
                            string[] strarr = grpname.Split('_');
                            if (strarr[1] == "door")
                            {//判断是否是门
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                //2018-11-14
                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                string oname = "";
                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                string ngrpname = "door_" + oname;
                                //
                                int grpid = Import3DsDoor(filename, ngrpname, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, thickness, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "window")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                //2018-11-15
                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                string oname = "";
                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                string ngrpname = "window_" + oname;
                                //"window.3ds", "window_1"
                                int grpid = Import3DsWindow(filename, ngrpname, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, thickness, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "baywin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //飘窗的深度
                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsBayWin("window.3ds", "baywin_1", 0, 0, 0, 0, 0, 0, 0, width, g_wallthickness, depth, height, true, false);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "sevenwin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //飘窗的深度
                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsSevenWin(ent, "ldc.3ds", "sevenwin_1");
                                if (grpid > 0)
                                {
                                    //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "profilewin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                if (thickness < 0.001)
                                    thickness = g_wallthickness;
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsProfileWin("profilewin.3ds", "profilewin_1", 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, 80, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "hole")
                            {
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                        }
                    }//
                }
            }//
             //踢脚线
            for (int i = 0; i < tmpareas.Count; i++)
            {
                int bbdent = axEWdraw1.MakeBaseBoard((int)tmpareas[i], 30, 80, 0, 128.0);
                if (bbdent > 0)
                {
                    axEWdraw1.SetRGBColor(bbdent, 220, 220, 220);
                    axEWdraw1.SetExtMatl(bbdent, 3, 220, 220, 220, "", "", 0, 1, 1);
                }

                g_roofids.Add(bbdent);//2018-12-03
            }

            //判断哪些是外墙面
            //---得到单墙的个数
            int singlesize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez, vx, vy, vz;
            sx = sy = sz = ex = ey = ez = vx = vy = vz = 0.0;
            //得到单墙的信息,并设置墙的基本颜色
            for (int j = 0; j < singlesize; j++)
            {
                axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                axEWdraw1.SetWallColor(singleid, 255, 255, 255);
            }
            //
            for (int i = 0; i < singlewalls.Count; i++)
            {
                if (((SWallSeg)singlewalls[i]).inx == 0)
                {
                    //根据中间点,找到单墙
                    //
                    for (int j = 0; j < singlesize; j++)
                    {
                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                        if ((Math.Abs(((SWallSeg)singlewalls[i]).x1 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x2 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - ey) < 0.001)
                            ||
                        (Math.Abs(((SWallSeg)singlewalls[i]).x2 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x1 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - ey) < 0.001)
                        )
                        {
                            ////此处是外墙处理
                            singlewallids.Add(singleid);//增加到缓存
                            axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id, singleid, ref vx, ref vy, ref vz);
                            axEWdraw1.SetSingleWallToOutSide(singleid, true);
                            if (!axEWdraw1.IsHaveTexFaceByDir(singleid, vx, vy, vz))
                            {
                                axEWdraw1.SetSingleWallInsideTexture(singleid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    //此外是共享墙,内墙
                    for (int j = 0; j < singlesize; j++)
                    {
                        //得到墙信息
                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                        if ((Math.Abs(((SWallSeg)singlewalls[i]).x1 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x2 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - ey) < 0.001)
                            ||
                        (Math.Abs(((SWallSeg)singlewalls[i]).x2 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x1 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - ey) < 0.001)
                        )
                        {
                            singlewallids.Add(singleid);//增加到缓存
                                                        //非外墙 2017-02-06
                            axEWdraw1.SetSingleWallToOutSide(singleid, false);
                            //得到墙的方向
                            axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id, singleid, ref vx, ref vy, ref vz);
                            if (!axEWdraw1.IsHaveTexFaceByDir(singleid, vx, vy, vz))
                            {
                                //设置一般单墙的纹理
                                axEWdraw1.SetSingleWallInsideTexture(singleid, "wall_def.jpg", 0, 0, 1, 1, 1, 1);
                            }
                            if (((SWallSeg)singlewalls[i]).id1 > 0)
                            {
                                axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id1, singleid, ref vx, ref vy, ref vz);
                                if (!axEWdraw1.IsHaveTexFaceByDir(singleid, vx, vy, vz))
                                {
                                    //设置一般墙内墙面的纹理
                                    axEWdraw1.SetSingleWallInsideTexture(singleid, "wall_def.jpg", 0, 0, 1, 1, 1, 1);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            //创建阳台
            MakeBalcony(ref g_balconyids);
            //关闭墙面纹理实际大小设置
            axEWdraw1.SetTexFaceTextureUVSize(false, 1000, g_wallheight);
        }

        //显示全局时,先要处理哪些墙显示,那些墙不显示
        private void ProcAllSingleDwarfWall()
        {
            int ent = 0;
            int type = -1;
            double x1, y1, x2, y2, mx, my;
            x1 = y1 = x2 = y2 = mx = my = 0.0;
            //关闭网格线
            axEWdraw1.SetGridOn(false);
            //关闭纹理实际大小的设置
            axEWdraw1.SetTexFaceTextureUVSize(true, 1000, g_wallheight);
            //消隐其它实体
            int entsize = axEWdraw1.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type != 68 && type != 66 && !axEWdraw1.IsGroup(ent))
                {
                    axEWdraw1.SetEntityInvisible(ent, true);
                }
            }
            int len = axEWdraw1.GetEntSize();
            //得到区域对象的墙坐标信息
            ArrayList singlewalls = new ArrayList();
            for (int i = 1; i <= len; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    string str = GetProStrFromEnt(ent, "area");
                    if (str.Length > 0)
                    {
                        if (Convert.ToInt32(str) == 1)
                        {
                            ArrayList pts = new ArrayList();
                            GetPtsFromSolid(ent, ref pts);
                            if (pts.Count > 0)
                            {
                                int ptslen = pts.Count / 3 - 1;
                                for (int j = 0; j < ptslen; j++)
                                {
                                    x1 = (double)pts[j * 3];
                                    y1 = (double)pts[j * 3 + 1];
                                    x2 = (double)pts[(j + 1) * 3];
                                    y2 = (double)pts[(j + 1) * 3 + 1];
                                    mx = (x1 + x2) / 2.0;
                                    my = (y1 + y2) / 2.0;
                                    int finx = -1;
                                    if (singlewalls.Count > 0)
                                    {
                                        for (int k = 0; k < singlewalls.Count; k++)
                                        {
                                            if (Math.Abs(mx - ((SWallSeg)singlewalls[k]).vx) < 0.001 &&
                                                Math.Abs(my - ((SWallSeg)singlewalls[k]).vy) < 0.001
                                                )
                                            {//相同的线段
                                                ((SWallSeg)singlewalls[k]).inx++;
                                                finx = k;
                                                break;
                                            }
                                        }
                                        if (finx >= 0)
                                        {
                                            ((SWallSeg)singlewalls[finx]).id1 = ent;
                                        }
                                        else
                                        {
                                            SWallSeg aswall = new SWallSeg();
                                            aswall.x1 = x1;
                                            aswall.y1 = y1;
                                            aswall.x2 = x2;
                                            aswall.y2 = y2;
                                            aswall.vx = mx;
                                            aswall.vy = my;
                                            aswall.id = ent;
                                            singlewalls.Add(aswall);

                                        }
                                    }
                                    else
                                    {
                                        SWallSeg aswall = new SWallSeg();
                                        aswall.x1 = x1;
                                        aswall.y1 = y1;
                                        aswall.x2 = x2;
                                        aswall.y2 = y2;
                                        aswall.vx = mx;
                                        aswall.vy = my;
                                        aswall.id = ent;
                                        singlewalls.Add(aswall);
                                    }
                                    //                                     
                                }
                            }
                            pts.Clear();
                            if (!axEWdraw1.IsDisplayed(ent))
                            {
                                axEWdraw1.SetEntityInvisible(ent, false);
                            }
                            axEWdraw1.SetTransparency(ent, 0);
                            if (!axEWdraw1.IsHaveTexture(ent))//如果没有纹理,就给定一个默认的纹理
                            {
                                axEWdraw1.SetEntTexture(ent, "floor_2.png", 1, 1, 5, 5, 0, 0);
                            }
                            //
                        }
                    }
                }
                else if (type != 66 && type != 68)
                {
                    if (axEWdraw1.IsGroup(ent))
                    {
                        //判断是否是符号组
                        string grpname = axEWdraw1.GetGroupName(ent);
                        int inx = grpname.IndexOf("symbol");
                        if (inx >= 0)
                        {//如果是符号组
                            string[] strarr = grpname.Split('_');
                            if (strarr[1] == "door")
                            {//判断是否是门
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                //2018-11-14
                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                string oname = "";
                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                string ngrpname = "door_" + oname;
                                //
                                int grpid = Import3DsDoor(filename, ngrpname, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, thickness, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "window")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                //2018-11-15
                                string grpname0 = axEWdraw1.GetGroupName(ent);
                                string oname = "";
                                string filename = GetSimpleModeFromGrpName(grpname0, ref subclassitems, ref oname);
                                string ngrpname = "window_" + oname;
                                //
                                int grpid = Import3DsWindow(filename, ngrpname, 0, 0, 0, 36.0, 60.0, 16.0, 36.0, width, thickness, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "baywin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //飘窗的深度
                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsBayWin("window.3ds", "baywin_1", 0, 0, 0, 0, 0, 0, 0, width, 32, depth, height, true, false);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "sevenwin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //飘窗的深度
                                double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsSevenWin(ent, "ldc.3ds", "sevenwin_1");
                                if (grpid > 0)
                                {
                                    //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "profilewin")
                            {
                                double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                double orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                                orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                                //取得宽度
                                double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                                //取得厚度
                                double thickness = GetDblfromProStr(GetProStrFromEnt(ent, "thickness"));
                                if (thickness < 0.001)
                                    thickness = g_wallthickness;
                                //取得高度
                                double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                                //从第六个参数到第九个依次是:横向包边宽度,锁在门套外的距离,门套厚度,顶部包边的高度差,这些参数可以由数据库提供
                                //也就是说,这些参数与3ds文件是一对一的关系.
                                int grpid = Import3DsProfileWin("profilewin.3ds", "profilewin_1", 0, 0, 0, 0.0, 0.0, 0.0, 0.0, width, 80, height, true);
                                if (grpid > 0)
                                {

                                    axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                    axEWdraw1.GetGroupAxis(grpid, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                                    axEWdraw1.Ax3TrasfWithZAsYAxis(grpid, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                                    new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                                    //
                                    axEWdraw1.SetEntityUserData(grpid, "width:" + width.ToString() + ";");
                                    axEWdraw1.SetGroupRelateID(grpid, ent);
                                    SetProStrFromEnt(grpid, "id", ent);//2017-03-09 将两个组关联在一起
                                    SetProStrFromEnt(ent, "id", grpid);//2017-03-09 将两个组关联在一起
                                                                       //增加到数组中
                                    door3dsidlist.Add(grpid);
                                }
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                            else if (strarr[1] == "hole")
                            {
                                axEWdraw1.SetEntityInvisible(ent, true);
                            }
                        }
                    }
                }
            }//
             //判断哪些是外墙面
            int singlesize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez, vx, vy, vz;
            sx = sy = sz = ex = ey = ez = vx = vy = vz = 0.0;
            //
            for (int i = 0; i < singlewalls.Count; i++)
            {
                if (((SWallSeg)singlewalls[i]).inx == 0)
                {
                    //外墙
                    for (int j = 0; j < singlesize; j++)
                    {
                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                        if ((Math.Abs(((SWallSeg)singlewalls[i]).x1 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x2 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - ey) < 0.001)
                            ||
                        (Math.Abs(((SWallSeg)singlewalls[i]).x2 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x1 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - ey) < 0.001)
                        )
                        {
                            axEWdraw1.SetSingleWallToOutSide(singleid, true);//设置为一般外墙
                            //设置墙的颜色为白色
                            axEWdraw1.SetWallColor(singleid, 255, 255, 255);
                            //获取墙的方向
                            axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id, singleid, ref vx, ref vy, ref vz);
                            //设置墙为外墙
                            //axEWdraw1.SetSingleWallToOutSide(singleid, true);
                            //增加到缓存
                            singlewallids.Add(singleid);
                            //判断墙是否已经有纹理
                            if (!axEWdraw1.IsHaveTexFaceByDir(singleid, vx, vy, vz))
                            {
                                axEWdraw1.SetSingleWallInsideTexture(singleid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < singlesize; j++)
                    {
                        axEWdraw1.GetSingleWallInfo(j, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                        if ((Math.Abs(((SWallSeg)singlewalls[i]).x1 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x2 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - ey) < 0.001)
                            ||
                        (Math.Abs(((SWallSeg)singlewalls[i]).x2 - sx) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y2 - sy) < 0.001 &&
                        Math.Abs(((SWallSeg)singlewalls[i]).x1 - ex) < 0.001 && Math.Abs(((SWallSeg)singlewalls[i]).y1 - ey) < 0.001)
                        )
                        {
                            axEWdraw1.SetSingleWallToOutSide(singleid, false);//设置为一般内墙
                            //设置一般墙为不显示
                            axEWdraw1.SetEntityInvisible(singleid, true);
                            //设置矮墙为显示
                            axEWdraw1.SetEntityInvisible(singledid, false);
                            //设置墙的颜色
                            axEWdraw1.SetWallColor(singledid, 255, 255, 255);
                            //得到墙的外方向
                            axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id, singleid, ref vx, ref vy, ref vz);
                            //2017-02-06
                            singlewallids.Add(singleid);//增加到缓存
                            dwarfewallids.Add(singledid);//增加矮墙

                            //设置纹理
                            if (!axEWdraw1.IsHaveTexFaceByDir(singledid, vx, vy, vz))
                            {
                                axEWdraw1.SetDwarfWallInsideTexture(singledid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                                if (((SWallSeg)singlewalls[i]).id1 > 0)
                                {
                                    axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id1, singleid, ref vx, ref vy, ref vz);
                                    if (!axEWdraw1.IsHaveTexFaceByDir(singledid, vx, vy, vz))
                                    {
                                        axEWdraw1.SetDwarfWallInsideTexture(singledid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                                    }
                                }
                            }
                            else if (!axEWdraw1.IsHaveTexFaceByDir(singledid, -vx, -vy, -vz))//因为矮墙的两面都要显示 2018-07-06
                            {
                                axEWdraw1.SetDwarfWallInsideTexture(singledid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                                if (((SWallSeg)singlewalls[i]).id1 > 0)
                                {
                                    axEWdraw1.GetDirOfSingleWall(((SWallSeg)singlewalls[i]).id1, singleid, ref vx, ref vy, ref vz);
                                    if (!axEWdraw1.IsHaveTexFaceByDir(singledid, vx, vy, vz))
                                    {
                                        axEWdraw1.SetDwarfWallInsideTexture(singledid, "wall_def.jpg", 0, 0, 5, 5, 1, 1);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            //创建阳台
            MakeBalcony(ref g_balconyids);
            //关闭纹理实际大小的设置
            axEWdraw1.SetTexFaceTextureUVSize(false, 1000, g_wallheight);
        }
        //全局视图
        private void GlobalView()
        {
            if (g_viewmode == 0)
            {
                if (!axEWdraw1.IsEndCommand())
                {//如果命令没有结束,则判断,是否有lastsym的实体
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }
                canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
                axEWdraw1.CancelCommand();
                //设定视图的模式
                g_viewmode = 3;
                ProcAllSingleWall();
                MakeDoorBottom("doorstone_2.jpg", 1.0);//2019-03-08
                if (!g_isinnerorbit)//2018-02-01
                    axEWdraw1.EnableHideWall(true);
                axEWdraw1.SetPerspectiveMode(true);
                axEWdraw1.SetCameraAngle(60);//2019-03-07
                if (!timer6.Enabled)//2018-08-03
                    timer6.Enabled = true;
                if (!g_isinnerorbit)
                {
                    axEWdraw1.SetViewCondition(8);
                    axEWdraw1.ZoomALL();
                    axEWdraw1.ToDrawOrbit();
                }
                else
                {

                    if (g_balconyhidewall != null)
                    {////2018-02-01
                        if (g_balconyhidewall.Count > 0)
                        {////2018-01-19
                            for (int i = 0; i < g_balconyhidewall.Count; i++)
                                axEWdraw1.SetSingleWallToOutSide((int)g_balconyhidewall[i], false);
                        }
                    }
                    //打开内部环视
                    axEWdraw1.EnableInnerOrbit(true, 100, new object[] { g_innerx, g_innery, g_innerz }, new object[] { 0, 1, 0 });
                }
            }
        }

        //半墙模式
        private void HalfWallView()
        {
            if (g_viewmode == 0)
            {
                if (!axEWdraw1.IsEndCommand())
                {//如果命令没有结束,则判断,是否有lastsym的实体
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }
                canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
                axEWdraw1.CancelCommand();
                //设定视图的模式
                g_viewmode = 4;
                ProcAllSingleDwarfWall();
                if (g_balconyconnectpts != null)
                {
                    if (g_balconyconnectpts.Count > 0)
                    {
                        axEWdraw1.Clear3DPtBuf();
                        for (int i = 0; i < g_balconyconnectpts.Count; i++)
                            axEWdraw1.AddOne3DVertex(((SPoint)g_balconyconnectpts[i]).x, ((SPoint)g_balconyconnectpts[i]).y, ((SPoint)g_balconyconnectpts[i]).z);
                        axEWdraw1.DisableSewingByPtInDwarf(true);
                    }
                }
                axEWdraw1.MakeDwarfWallTexSew(true);//2017-03-10 改进矮墙效果
                if (g_balconyconnectpts != null)
                {
                    axEWdraw1.Clear3DPtBuf();
                    axEWdraw1.DisableSewingByPtInDwarf(false);
                }
                axEWdraw1.EnableHideWall(true);
                axEWdraw1.SetViewCondition(8);
                axEWdraw1.ZoomALL();
                axEWdraw1.SetPerspectiveMode(true);
                if (!timer6.Enabled)//2018-08-03
                    timer6.Enabled = true;
                axEWdraw1.ToDrawOrbit();
            }
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
            if (tmppipe > 0)
                axEWdraw1.SetEntityInvisible(tmppipe, true);//2016-11-29
            if (tmppipe1 > 0)
                axEWdraw1.SetEntityInvisible(tmppipe1, true);//2016-11-29
            if (canceldrawwalljs == 1 && !isdrawbalcony)
            {//2017-03-15 连续画墙
                textBox1.Visible = false;//2017-07-31
                axEWdraw1.ClearSelected();//2017-03-20
                canceldrawwalljs = 0;
                //画墙
                tmplist.Clear();//初始化
                tlist0.Clear();
                tlist1.Clear();
                tmpent = 0;//初始化
                tmppipe = 0;//初始化
                tmppipe1 = 0;//初始化 2016-09-18
                drawsegjs = 0;//初始化画线计数
                connecttype = 0;//初始化连接类型
                wantdelent = 0;//要删除的实体,用于连接起始与终止的实体
                connectent = 0;//初始化连接的实体ID
                edgeptjs = 0;//初始化边点计数
                axEWdraw1.SetORTHO(true);
                axEWdraw1.EnableCommandOnLBDown(true);
                axEWdraw1.CancelCommand();
                axEWdraw1.EnableDrawPolylinePipe(true, g_wallthickness, g_wallheight);
                isdrawpolyline = true;
                g_state = InteractiveState.BeginDrawWall;
                axEWdraw1.SetDrawPolylineLenLimit(g_wallthickness);
                axEWdraw1.ToDrawPolyLine();
            }
            else if (isdrawbalcony)
            {
                if (g_balconyent > 0)
                {
                    isdrawbalcony = false;
                    Form5 input = new Form5();
                    if (input != null)
                    {
                        input.g_height = g_wallheight;//2018-12-10
                        if (input.pipepts == null)
                            input.pipepts = new ArrayList();
                        if (g_balconysegpts.Count > 0)
                        {
                            input.g_height = g_wallheight;
                            input.g_thickness = g_wallthickness;
                            input.g_areaent = g_balconyent;
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            axEWdraw1.GetEntBoundingBox(input.g_areaent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            input.g_cx = (minx + maxx) / 2.0;
                            input.g_cy = (miny + maxy) / 2.0;
                            input.g_cz = (minz + maxz) / 2.0;
                            input.pipepts = g_balconysegpts;
                        }
                    }
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        if (input.memstr.Length > 0)
                        {
                            ArrayList tlist = new ArrayList();
                            GetPtsFromSolid(g_balconyent, ref tlist);
                            axEWdraw1.SetEntityUserData(g_balconyent, input.memstr);
                            SetPtsToSolid(g_balconyent, ref tlist);
                            tlist.Clear();
                            input.pipepts.Clear();
                            g_balconysegpts.Clear();
                        }
                    }
                }
            }
        }


        /*DimAddWalls 标注新增绘制的墙面 2016-09-18
        * 参数:
        * font    输入 标注的字体
        * fheight 输入 标注字体的大小
        * r,g,b   输入 标注颜色
        * off     输入 标注偏移的距离(墙厚的2至3倍比较合适)
        * maxz    输入 最大的Z值(一定要高于墙高)
        * 返回值:
        * 无
        */
        private void DimAddWalls(string font, double fheight, int r, int g, int b, double off, double maxz = 0.0)
        {
            //搜索现有的实体中的与该实体绑定的标注
            //
            int addwallsize = axEWdraw1.GetSingleWallIDBufferSize();
            for (int i = 0; i < addwallsize; i++)
            {
                int ent = axEWdraw1.GetSingleWallID(i);
                if (ent >= 1)
                {
                    ArrayList tmpptslist = new ArrayList();
                    if (GetSingleWallPtsFromSolid(ent, ref tmpptslist))
                    {
                        if (tmpptslist.Count >= 3 && axEWdraw1.IsExistEnt(ent))
                        {
                            //搜索现有的实体中的与该实体绑定的标注

                            //
                            //求中心点
                            double sumx, sumy, sumz, midx, midy, midz;
                            sumx = sumy = sumz = midx = midy = midz = 0.0;
                            int len = tmpptslist.Count / 3;
                            for (int k = 0; k < len; k++)
                            {
                                sumx += (double)tmpptslist[k * 3];
                                sumy += (double)tmpptslist[k * 3 + 1];
                                sumz += (double)tmpptslist[k * 3 + 2];
                            }
                            midx = sumx / (double)len;
                            midy = sumy / (double)len;
                            midz = sumz / (double)len;
                            //计算每一线段标注的方向
                            double x1, y1, z1, x2, y2, z2, lmx, lmy, lmz, vx, vy, vz;
                            x1 = y1 = z1 = x2 = y2 = z2 = lmx = lmy = lmz = vx = vy = vz = 0.0;
                            for (int k = 0; k < len - 1; k++)
                            {
                                x1 = (double)tmpptslist[k * 3];
                                y1 = (double)tmpptslist[k * 3 + 1];
                                z1 = (double)tmpptslist[k * 3 + 2];

                                x2 = (double)tmpptslist[(k + 1) * 3];
                                y2 = (double)tmpptslist[(k + 1) * 3 + 1];
                                z2 = (double)tmpptslist[(k + 1) * 3 + 2];
                                lmx = (x1 + x2) / 2.0;
                                lmy = (y1 + y2) / 2.0;
                                lmz = (z1 + z2) / 2.0;
                                //从线段中心指向区域中心点的方向
                                vx = midx - lmx;
                                vy = midy - lmy;
                                vz = midz - lmz;
                                //判断标注的方向
                                double ox, oy, oz, ox1, oy1, oz1, ox2, oy2, oz2, vx1, vy1, vz1, vx2, vy2, vz2;
                                ox = oy = oz = ox1 = oy1 = oz1 = ox2 = oy2 = oz2 = vx1 = vy1 = vz1 = vx2 = vy2 = vz2 = 0.0;
                                axEWdraw1.Polar(new object[] { lmx, lmy, lmz }, new object[] { x2 - lmx, y2 - lmy, z2 - lmz }, -off, ref ox, ref oy, ref oz);
                                axEWdraw1.RotatePoint(ox, oy, oz, lmx, lmy, lmz, 0, 0, 1, 90, ref ox1, ref oy1, ref oz1);
                                axEWdraw1.RotatePoint(ox, oy, oz, lmx, lmy, lmz, 0, 0, 1, -90, ref ox2, ref oy2, ref oz2);
                                vx1 = ox1 - lmx;
                                vy1 = oy1 - lmy;
                                vz1 = oz1 - lmy;

                                vx2 = ox2 - lmx;
                                vy2 = oy2 - lmy;
                                vz2 = oz2 - lmy;
                                //
                                double ang1 = axEWdraw1.VectorAngle(new object[] { vx1, vy1, vz1 }, new object[] { vx, vy, vz });
                                double ang2 = axEWdraw1.VectorAngle(new object[] { vx2, vy2, vz2 }, new object[] { vx, vy, vz });
                                if (ang1 > ang2)
                                {
                                    ox = ox1;
                                    oy = oy1;
                                    oz = oz1;
                                }
                                else
                                {
                                    ox = ox2;
                                    oy = oy2;
                                    oz = oz2;
                                }
                                //设置标注文字大小
                                axEWdraw1.SetDimTxt(fheight);
                                axEWdraw1.SetDimAsz(fheight / 3.0);
                                //设置标注的字体
                                axEWdraw1.SetDimTxsty(font);
                                //设置标注的颜色
                                axEWdraw1.SetDimClr(r, g, b);
                                //设置标注文字高于标注线的距离
                                axEWdraw1.SetDimTAD(50);
                                //
                                int diment = 0;
                                if (Math.Abs(maxz) > 0.00001)
                                    diment = axEWdraw1.LengthDimension(new object[] { x1, y1, maxz }, new object[] { x2, y2, z2 }, 0, new object[] { 0, 0, 1 }, false, "", new object[] { ox, oy, maxz });
                                else
                                    diment = axEWdraw1.LengthDimension(new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 }, 0, new object[] { 0, 0, 1 }, false, "", new object[] { ox, oy, oz });
                                //将该标注与要标注的实体陈绑定(方法是在标注的数据中写入该实体的ID号)
                                if (diment > 0)
                                {
                                    axEWdraw1.SetEntityUserData(diment, "id:" + ent.ToString() + ";" + "segnum:" + i.ToString() + ";");//2016-10-28 增加段的索引值
                                    SetProStrFromEnt(ent, "id", diment);//2017-03-09 将两个组关联在一起
                                }
                            }
                            //
                        }
                    }
                    //
                    tmpptslist.Clear();
                    axEWdraw1.SetEntAbsorb(ent);
                }
                //绘制标注前,删除之前已经与该实体绑定的标注对象
                int enttype = -1;
                int tmpent = 0;
                int diment1 = 0;
                string str = "";
                ArrayList idlist = new ArrayList();
                idlist.Clear();
                int entsize = axEWdraw1.GetEntSize();
                for (int k = 1; k <= entsize; k++)
                {
                    tmpent = axEWdraw1.GetEntID(k);
                    enttype = axEWdraw1.GetEntType(tmpent);
                    if (enttype >= 93 && enttype <= 100)
                    {
                        str = GetProStrFromEnt(tmpent, "id");
                        if (str != "")
                        {
                            diment1 = Convert.ToInt32(str);
                            if (!axEWdraw1.IsExistEnt(diment1))
                            {
                                idlist.Add(tmpent);
                            }
                        }
                    }
                }
                if (idlist.Count > 0)
                {
                    for (int k = 0; k < idlist.Count; k++)
                        axEWdraw1.Delete((int)idlist[k]);
                }
            }
        }
        private int ReDrawHomeArea(double walltickness)
        {
            int areaent = 0;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            double midx, midy;
            double tox, toy, toz;
            tox = toy = toz = 0.0;
            midx = midy = 0.0;
            int fontheight = 120;
            int textent = 0;
            int entsize = 0;
            int ent = 0;
            //获得创建的面积对象个数
            int areaentsize = axEWdraw1.GetIDBufferSize();
            //得到面积对象的面积值
            for (int i = 0; i < areaentsize; i++)
            {
                areaent = axEWdraw1.GetIDBuffer(i);

                //axEWdraw1.SetTransparency(areaent, 0.9);
                double area = axEWdraw1.GetEntArea(areaent) / 1000000;//按平方米
                string str = String.Format("{0:f2}", area) + "M²";
                int strlen = str.Length;
                axEWdraw1.GetEntBoundingBox(areaent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                //计算地板的重复率
                double ureapt = (maxx - minx) / 1600.0;
                double vreapt = (maxy - miny) / 1600.0;
                axEWdraw1.SetEntTexture(areaent, "floor_2.png", 1, 1, ureapt, vreapt, 0, 0);
                axEWdraw1.SetEntAbsorb(areaent);//2019-03-06
                midx = (minx + maxx) / 2.0;
                midy = (miny + maxy) / 2.0;
                //
                if (axEWdraw1.Get3DPtBufSize() > i)
                {
                    axEWdraw1.Get3DPtBuf(i, ref tox, ref toy, ref toz);
                    textent = axEWdraw1.Text3D(str, "Arial", new object[] { tox - (strlen * fontheight * 0.75 / 2.0), toy, 1.0 }, fontheight, 0, 0);
                }
                else
                    textent = axEWdraw1.Text3D(str, "Arial", new object[] { midx - (strlen * fontheight * 0.75 / 2.0), midy, 1.0 }, fontheight, 0, 0);
                axEWdraw1.SetEntColor(textent, axEWdraw1.RGBToIndex(0, 0, 0));
                //设置所创建的面积对象与文字为面积类型
                SetProStrFromEnt(areaent, "area", 1);
                SetProStrFromEnt(textent, "id", areaent);//2016-11-15
                axEWdraw1.DeactivateEnt(textent);
            }
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.Clear3DPtBuf();
            return 0;
        }



        private bool GetSingleWallPtsFromSolid(int ent, ref ArrayList pts)
        {
            int swsize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez;
            sx = sy = sz = ex = ey = ez = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            double dist = 0;
            pts.Clear();
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                if (singleid == ent)
                {
                    pts.Add(sx);
                    pts.Add(sy);
                    pts.Add(0.0);
                    pts.Add(ex);
                    pts.Add(ey);
                    pts.Add(0.0);
                    return true;
                }
            }
            return false;
        }

        private void HideSingleRoomCabinet(int areaent, ref ArrayList ids)
        {
            int entsize = 0;
            int ent = 0;
            int i = 0;
            double x, y, z;
            int enttype = -1;
            x = y = z = 0;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            string grpname;
            if (areaent > 0)
            {
                enttype = axEWdraw1.GetEntType(areaent);
                if (enttype == 50)
                {
                    entsize = axEWdraw1.GetEntSize();
                    for (i = 1; i <= entsize; i++)
                    {
                        ent = axEWdraw1.GetEntID(i);
                        if (axEWdraw1.IsGroup(ent))
                        {
                            grpname = axEWdraw1.GetGroupName(ent);
                            if (!(grpname.IndexOf("door") >= 0 || grpname.IndexOf("window") >= 0
                                || grpname.IndexOf("d_org") >= 0 || grpname.IndexOf("w_org") >= 0
                                || grpname.IndexOf("sevenwin_org") >= 0 //2017-10-25
                                || grpname.IndexOf("balconywin_org") >= 0 //2017-12-06
                                || grpname.IndexOf("profilewin_org") >= 0 //2018-01-19
                                )
                                )
                            {
                                axEWdraw1.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                x = (minx + maxx) / 2.0;
                                y = (miny + maxy) / 2.0;
                                if (!axEWdraw1.Is2DPtInsideArea(x, y, areaent))//之所以增一个函数来判断是为了对非矩形也能适应
                                {
                                    ids.Add(ent);
                                }
                            }
                        }
                    }
                    for (i = 0; i < ids.Count; i++)
                        axEWdraw1.SetEntityInvisible((int)ids[i], true);
                }
            }
        }
        //移墙的状态控制 2017-03-15
        private void timer4_Tick(object sender, EventArgs e)
        {
            timer4.Enabled = false;
            int selentsize = axEWdraw1.GetSelectEntSize();
            if (selentsize == 1)
            {
                int selent = axEWdraw1.GetSelectEnt(0);
                int type = axEWdraw1.GetEntType(selent);
                if (type == 66)
                {
                    if (movewalljs == 1)
                    {
                        //移动墙             
                        g_state = InteractiveState.BeginMoveWall;
                        axEWdraw1.ClearSelected();
                        if (selent > 0)
                        {
                            ArrayList pts = new ArrayList();
                            GetPtsFromSolid(selent, ref pts);
                            double sx = (double)pts[0];
                            double sy = (double)pts[1];
                            double ex = (double)pts[3];
                            double ey = (double)pts[3 + 1];
                            if (IsWallInArea(sx, sy, ex, ey) && g_viewmode == 0)
                            {
                                axEWdraw1.ClearSelected();
                                axEWdraw1.BeginMoveWall(sx, sy, ex, ey, g_wallthickness);
                                g_state = InteractiveState.MoveingWall;
                            }
                            else
                            {
                                g_state = InteractiveState.Nothing;
                                axEWdraw1.ClearSelected();
                            }
                        }
                    }
                    if (movewalljs != -1)
                        timer4.Enabled = true;
                    movewalljs++;
                }
                else
                {
                    if (g_st > 1)
                    {
                        timer4.Enabled = false;
                        int selsize = axEWdraw1.GetSelectEntSize();
                        if (!isdrawabsorb)
                        {
                            if (selsize > 0)
                            {
                                int ent = axEWdraw1.GetSelectEnt(0);
                                if (axEWdraw1.IsGroup(ent))
                                {
                                    //取得组名
                                    string grpname = axEWdraw1.GetGroupName(ent);
                                    //判断是否是门 窗 阳台等等靠墙面吸附
                                    bool tmpisabsorb = IsAbsorbEnt(grpname);
                                    if (grpname.IndexOf("door") >= 0 || grpname.IndexOf("window") >= 0 || grpname.IndexOf("baywin") >= 0 ||
                                        grpname.IndexOf("profilewin") >= 0 || //2018-07-06
                                        tmpisabsorb)
                                    {
                                        //判断是否是平面图
                                        double vx, vy, vz;
                                        vx = vy = vz = 0.0;
                                        axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
                                        if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)//如果是平面,则可以移动
                                        {
                                            isdrawabsorb = true;
                                            axEWdraw1.EnableCheckAbsorbIns(false);
                                            if (tmpisabsorb)
                                                axEWdraw1.EnableAbsorbDepth(false, 0);
                                            else
                                                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                                            axEWdraw1.EnableAbsorbHigh(false, 0);//2017-08-17
                                            axEWdraw1.SetORTHO(false);
                                            axEWdraw1.EnableOrthoHVMode(false);
                                            axEWdraw1.CancelCommand();
                                            if (!isseloneent)//2017-09-20
                                            {
                                                if (grpname.IndexOf("sevenwin") < 0)//不能是七型飘窗 2017-10-16
                                                    axEWdraw1.ToDrawAbsorb();
                                            }
                                        }
                                        else if (tmpisabsorb)//如果tmpisabsorb为true,则说明是家具类的对象,可以选择吸附或移动 2018-01-18
                                        {
                                            if (IsFurniture(grpname))
                                            {
                                                //
                                                if (g_viewmode == 0)
                                                {
                                                    isdrawmove = true;
                                                    axEWdraw1.ToDrawMove();
                                                }
                                                else
                                                {//启用单项轴移动
                                                    if (!isdrawaxismove)
                                                    {
                                                        isdrawmove = true;
                                                        axEWdraw1.ToDrawMove();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        g_tmpjs = 0;
                                        isdrawmove = true;
                                        if (!isseloneent)//2017-09-20
                                        {
                                            if (grpname.IndexOf("sevenwin") < 0)//不能是七型飘窗 2017-10-16
                                            {
                                                if (IsFurniture(grpname))
                                                {
                                                    axEWdraw1.ToDrawMove();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {//2017-03-16
                                    if (type == 50)
                                    {
                                        string memo = axEWdraw1.GetEntityUserData(ent);
                                        int inx = IsHaveStrField(memo, "area:");
                                        if (inx >= 0)
                                        {
                                            if (!isbeginmove)
                                            {
                                                isdrawpan = true;
                                                if (g_viewmode == 0)//只有在绘制户型模式下才可用
                                                    axEWdraw1.ToDrawPan();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!isbeginmove)
                                {
                                    isdrawpan = true;
                                    if (g_viewmode == 0)//只有在绘制户型模式下才可用
                                    {
                                        if (axEWdraw1.IsEndCommand())//2018-01-31
                                            axEWdraw1.ToDrawPan();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        g_st++;
                        timer4.Enabled = true;//2017-08-08
                    }
                }
            }
            else if (movewalljs == 3 && g_state == InteractiveState.MoveingWall)
            {//结束移动墙 2017-03-15
                movewalljs = 0;
                g_state = InteractiveState.Nothing;
                //
                axEWdraw1.Clear3DPtBuf();
                axEWdraw1.ClearIDBuffer();
                //
                if (axEWdraw1.EndMoveWall())
                {
                    axEWdraw1.MakeSingleWallFromArea(g_wallheight);
                    ReDrawHomeArea(g_wallthickness);

                    if (g_movewallid > 0)
                    {
                        DelDimWalls(g_movewallid);
                        g_movewallid = 0;
                    }
                    DimAddWalls("Arial", 150, 0, 0, 0, g_wallthickness * 1.12, g_maxz);
                }
            }
        }
        //判断点集是否有重合或线段有重合 2017-05-21
        private bool IsOverPolyLine(ref ArrayList pts)
        {
            int ptsize = pts.Count / 3;
            for (int i = 0; i < ptsize - 1; i++)
            {
                if (axEWdraw1.PointDistance((double)pts[i * 3], (double)pts[i * 3 + 1], 0, (double)pts[(i + 1) * 3], (double)pts[(i + 1) * 3 + 1], 0) < 0.001)
                    return true;
                for (int j = 0; j < ptsize - 1; j++)
                {
                    if (i != j)
                    {
                        if (axEWdraw1.IsLineSegOver((double)pts[i * 3], (double)pts[i * 3 + 1], (double)pts[(i + 1) * 3], (double)pts[(i + 1) * 3 + 1],
                                                (double)pts[j * 3], (double)pts[j * 3 + 1], (double)pts[(j + 1) * 3], (double)pts[(j + 1) * 3 + 1])
                            )
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //判断是否是可算做横线或竖线 2017-05-21
        private bool IsHVLineSeg(ref ArrayList pts, double tol)
        {
            double x1, y1, x2, y2;
            x1 = y1 = x2 = y2 = 0.0;
            if (pts.Count > 3)
            {
                x1 = (double)pts[pts.Count - 3];
                y1 = (double)pts[pts.Count - 2];
                x2 = (double)pts[pts.Count - 6];
                y2 = (double)pts[pts.Count - 5];

                if (Math.Abs(x1 - x2) > Math.Abs(y1 - y2))
                {
                    if (Math.Abs(y1 - y2) >= tol)
                        return false;
                }
                else
                {
                    if (Math.Abs(x1 - x2) >= tol)
                        return false;
                }
            }
            else return false;
            return true;
        }
        /*MakeWindowSymbol 在俯视下创建窗的符号,以组为基础.也就是说一个组是一个符号.
         * 参数:
         * name      输入 门的名称(组名)
         * width     输入 宽度
         * thickness 输入 厚度
         * height    输入 高度
         * ridz      输入 底部距地面的Z值
         * maxz      输入 最高z值,一定要高于墙高
         * 返回值:
         * 成功则返回符号组的ID,其它返回0
         */
        private int MakeWindowSymbol(string name, double width, double thickness, double height, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, width, thickness, 0);
            int rect1 = axEWdraw1.Rectangle(0, thickness / 4.0, width, thickness / 2.0 + thickness / 4.0, maxz);
            axEWdraw1.SetEntLineWidth(rect1, 2);
            axEWdraw1.SetEntColor(rect, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntColor(rect1, axEWdraw1.RGBToIndex(0, 0, 0));
            int fwindow = axEWdraw1.EntToFace(rect, false);
            int window = axEWdraw1.Prism(fwindow, maxz, new object[] { 0, 0, 1 });

            axEWdraw1.Delete(fwindow);
            axEWdraw1.SetEntColor(window, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(window, 0.3);

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(window);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect);
            axEWdraw1.AddIDToBuffer(rect1);
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { width / 2.0, thickness / 2.0, 0 });
            //设置组的数据
            string str = "width:" + width.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "height:" + height.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            axEWdraw1.ClearIDBuffer();
            return group;
        }

        /*MakeBayWinSymbol 在俯视下创建飘窗的符号,以组为基础.也就是说一个组是一个符号.
         * 参数:
         * name      输入 门的名称(组名)
         * width     输入 宽度
         * thickness 输入 厚度
         * depth     输入 深度
         * height    输入 高度
         * ridz      输入 底部距地面的Z值
         * maxz      输入 最高z值,一定要高于墙高
         * 返回值:
         * 成功则返回符号组的ID,其它返回0
         */
        private int MakeBayWinSymbol(string name, double width, double thickness, double depth, double height, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, width + thickness * 2, depth, 0);
            axEWdraw1.SetEntColor(rect, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(rect, 2);
            int fwindow = axEWdraw1.EntToFace(rect, false);
            int window = axEWdraw1.Prism(fwindow, maxz, new object[] { 0, 0, 1 });
            axEWdraw1.Delete(fwindow);
            axEWdraw1.SetEntColor(window, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(window, 0.3);

            int rect1 = axEWdraw1.Rectangle(thickness, 0, width + thickness, depth - thickness, 0);
            int rect2 = axEWdraw1.Rectangle(thickness, 0, width + thickness, depth - thickness, maxz);
            axEWdraw1.SetEntLineWidth(rect1, 2);
            axEWdraw1.SetEntColor(rect1, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(rect2, 2);
            axEWdraw1.SetEntColor(rect2, axEWdraw1.RGBToIndex(0, 0, 0));

            int fwindow1 = axEWdraw1.EntToFace(rect1, false);
            int window1 = axEWdraw1.Prism(fwindow1, maxz, new object[] { 0, 0, 1 });
            axEWdraw1.Delete(fwindow1);
            axEWdraw1.SetEntColor(window1, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(window1, 0.3);

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(window);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect);
            axEWdraw1.AddIDToBuffer(window1);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect1);
            axEWdraw1.AddIDToBuffer(rect2);

            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { width / 2.0, thickness / 2.0, 0 });
            //设置组的数据
            string str = "width:" + width.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "height:" + height.ToString() + ";" + "depth:" + depth.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            axEWdraw1.ClearIDBuffer();
            return group;
        }

        //DrawWindow
        private void DrawWindow(string name, double width, double thickness, double height, double ridz)//
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            //
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                string grpname = "symbol_window_" + name;//2018-11-15
                lastsym = MakeWindowSymbol(grpname, width, g_wallthickness, height, ridz, g_maxz, true);
                axEWdraw1.EnableAbsorbHigh(true, ridz);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");

        }


        private void DrawBayWin()
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                lastsym = MakeBayWinSymbol("symbol_baywin_1", 1450, g_wallthickness, 800, 1700, 0, g_maxz, true);
                axEWdraw1.EnableAbsorbHigh(true, 800);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");
        }

        /*MakeHoleSymbol 在俯视下创建门的符号,以组为基础.也就是说一个组是一个符号.
         * 参数:
         * name      输入 门的名称(组名)
         * width     输入 宽度
         * thickness 输入 厚度
         * height    输入 高度
         * ridz      输入 底部距地面的Z值
         * maxz      输入 最高z值,一定要高于墙高
         * 返回值:
         * 成功则返回符号组的ID,其它返回0
         */
        private int MakeHoleSymbol(string name, double width, double thickness, double height, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, width, thickness, 0);
            int rect1 = axEWdraw1.Rectangle(0, 0, width, thickness, maxz);
            axEWdraw1.SetEntColor(rect1, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(rect1, 2);
            axEWdraw1.SetEntLineType(rect1, 1);

            int frect = axEWdraw1.EntToFace(rect, false);

            int fhole = axEWdraw1.Prism(frect, maxz, new object[] { 0, 0, 1 });
            //
            axEWdraw1.SetEntColor(fhole, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(fhole, 0.3);

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(fhole);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect);
            axEWdraw1.AddIDToBuffer(rect1);
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { width / 2.0, thickness / 2.0, 0 });
            //设置组的数据
            string str = "width:" + width.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "height:" + height.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.Delete(frect);
            return group;
        }

        /*MakePillar 在俯视下创建门的符号,以组为基础.也就是说一个组是一个符号.
        * 参数:
        * name      输入 门的名称(组名)
        * width     输入 宽度
        * thickness 输入 厚度
        * height    输入 高度
        * ridz      输入 底部距地面的Z值
        * maxz      输入 最高z值,一定要高于墙高
        * 返回值:
        * 成功则返回符号组的ID,其它返回0
        */
        private int MakePillar(string name, double length, double width, double height, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, length, width, 0);
            int frect = axEWdraw1.EntToFace(rect, true);
            int fhole = axEWdraw1.Prism(frect, height, new object[] { 0, 0, 1 });
            axEWdraw1.SetEntColor(fhole, axEWdraw1.RGBToIndex(255, 255, 255));

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(fhole);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,pillar表示这是墙柱的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { 0, 0, 0 });
            //设置组的数据
            string str = "xlength:" + String.Format("{0:f3}", length) + ";" + "ylength:" + String.Format("{0:f3}", width) + ";" + "zlength:" + String.Format("{0:f3}", height) + ";"; // "width:" + length.ToString() + ";" + "thickness:" + width.ToString() + ";" + "height:" + height.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";"
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.Delete(frect);
            return group;
        }



        private void axEWdraw1_PolyLineDimMove(object sender, AxEWDRAWLib._DAdrawEvents_PolyLineDimMoveEvent e)
        {
            bool ismove = false;
            if (g_dimposx == -1 || g_dimposy == -1)
            {
                ismove = true;
                g_dimposx = e.x;
                g_dimposy = e.y;
            }
            else
            {
                if (Math.Abs(e.x - g_dimposx) > 2 || Math.Abs(e.y - g_dimposy) > 2)
                {
                    ismove = true;
                    g_dimposx = e.x;
                    g_dimposy = e.y;
                }
            }
            if (textBox1.Visible && textBox1.Focused && isenterchar)
            {
                if (Math.Abs(g_dimposx - old_dimposx) > 16)
                {
                    textBox1.Visible = false;
                    textBox1.Refresh();
                    axEWdraw1.Focus();
                }
                else if (Math.Abs(g_dimposy - old_dimposy) > 16)
                {
                    textBox1.Visible = false;
                    textBox1.Refresh();
                    axEWdraw1.Focus();
                }
            }
        }

        private void DrawWallHole()
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                lastsym = MakeHoleSymbol("symbol_hole_1", 900, g_wallthickness, 2000, 0, g_maxz, true);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbHigh(false, 0);
                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");

        }
        //画墙柱
        private void DrawPillar()
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                    axEWdraw1.Delete(lastsym);
            }
            axEWdraw1.CancelCommand();

            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                lastsym = MakePillar("pillar_1", 256, 256, g_wallheight, 0, g_maxz, true);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbHigh(false, 0);
                axEWdraw1.EnableAbsorbDepth(false, 0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (GDesignMode == 1 && listView1.Items.Count>0)
            {
                //计算取得抽屉的基点 2019-12-30
                CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
                g_isctins = true;
            }
            else if (GDesignMode == 2)
            {
                double rad = 0.0;
                if (dataGridView1.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1[0, i].Value.ToString() == "半径")
                        {
                            rad = zPubFun.zPubFunLib.CStr2Double(dataGridView1[1, i].Value);
                        }
                    }
                }
                int selsize = axEWdraw3.GetSelectEntSize();
                if (selsize > 0)
                {
                    for (int i = 0; i < selsize; i++)
                    {
                        int ent = axEWdraw3.GetSelectEnt(i);
                        int enttype = axEWdraw3.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            if (!axEWdraw3.IsGroup(ent))
                            {
                                if(rad>0.001)
                                    MakeYXB(g_plankcode, ent,rad);//,double nrad = -1.0
                                else
                                    MakeYXB(g_plankcode, ent);//,double nrad = -1.0
                            }
                        }

                    }
                    //
                }
            }else if (GDesignMode == 3)//F架
            {//2020-02-21
                
                g_isfjiains = true;
            }
            else if (GDesignMode == 4)//博古架
            {
                g_isboguins = true;
            }

        }
        //摆放床
        private void DrawBed()
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (lastsym > 0)
            {
                axEWdraw1.Delete(lastsym);
                lastsym = 0;
            }
            axEWdraw1.CancelCommand();
            listView3.SelectedItems.Clear();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                double minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = 0.0;
                if (checkBox1.CheckState == CheckState.Checked)
                {
                    isdrawabsorb = true;//2017-09-20
                    isdrawmove = false;//2017-09-20
                    axEWdraw1.ClearSelected();//2016-09-08
                    lastsym = Import3DsObj("sample\\ctgzh1\\ctgzh1.3DS", "ctgzh1", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    axEWdraw1.GetEntBoundingBox(lastsym, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    double xlength = maxx - minx;
                    double ylength = maxy - miny;
                    double zlength = maxz - minz;
                    string memstr = "xlength:" + String.Format("{0:f3}", xlength) + ";" + "ylength:" + String.Format("{0:f3}", ylength) + ";" + "zlength:" + String.Format("{0:f3}", zlength) + ";";
                    axEWdraw1.SetEntityUserData(lastsym, memstr);
                    axEWdraw1.EnableAbsorbHigh(true, 0);//2017-09-30
                    axEWdraw1.EnableCheckAbsorbIns(false);
                    axEWdraw1.EnableAbsorbDepth(false, 0);
                    axEWdraw1.SetORTHO(false);
                    axEWdraw1.EnableOrthoHVMode(false);
                    axEWdraw1.CancelCommand();
                    axEWdraw1.AddOrRemoveSelect(lastsym);
                    axEWdraw1.ToDrawAbsorb();
                }
                else
                {
                    isdrawabsorb = false;
                    isdrawmove = true;
                    axEWdraw1.ClearSelected();//2016-09-08

                    lastsym = axEWdraw1.InsertGroupFromFile("chuang_01.ewd", new object[] { 0, 0, 0 });//MakeBayWinSymbol("baywin1", 1450, g_wallthickness, 800, 1700, 0, g_maxz, true);
                    axEWdraw1.GetEntBoundingBox(lastsym, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    double xlength = maxx - minx;
                    double ylength = maxy - miny;
                    double zlength = maxz - minz;
                    string memstr = "xlength:" + String.Format("{0:f3}", xlength) + ";" + "ylength:" + String.Format("{0:f3}", ylength) + ";" + "zlength:" + String.Format("{0:f3}", zlength) + ";";
                    axEWdraw1.SetEntityUserData(lastsym, memstr);
                    axEWdraw1.EnableAbsorbHigh(false, 0);
                    axEWdraw1.EnableCheckAbsorbIns(false);
                    axEWdraw1.EnableAbsorbDepth(false, 0);
                    axEWdraw1.SetORTHO(false);
                    axEWdraw1.EnableOrthoHVMode(false);
                    axEWdraw1.CancelCommand();
                    axEWdraw1.AddOrRemoveSelect(lastsym);

                    g_mx = (minx + maxx) / 2.0;
                    g_my = (miny + maxy) / 2.0;
                    g_mz = minz;
                    g_st = 2;
                    g_tmpjs = 0;
                    axEWdraw1.ToDrawMove();
                }
            }
            else
                MessageBox.Show("请在平面下绘制");
        }
        //摆放柜子
        private void DrawCabinet()//
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            axEWdraw1.ClearSelected();
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                axEWdraw1.ClearSelected();//2016-09-08
                double minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = 0.0;

                if (checkBox1.CheckState == CheckState.Checked)
                {
                    listView3.SelectedItems.Clear();
                    isdrawabsorb = true;
                    isdrawmove = false;

                    lastsym = axEWdraw1.InsertGroupFromFile("cabinet_01.ewd", new object[] { 0, 0, 0 });//MakeBayWinSymbol("baywin1", 1450, g_wallthickness, 800, 1700, 0, g_maxz, true);
                    axEWdraw1.GetEntBoundingBox(lastsym, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    double xlength = maxx - minx;
                    double ylength = maxy - miny;
                    double zlength = maxz - minz;
                    string memstr = "xlength:" + String.Format("{0:f3}", xlength) + ";" + "ylength:" + String.Format("{0:f3}", ylength) + ";" + "zlength:" + String.Format("{0:f3}", zlength) + ";";
                    axEWdraw1.SetEntityUserData(lastsym, memstr);
                    axEWdraw1.EnableAbsorbHigh(true, 0);//2017-09-30
                    axEWdraw1.EnableCheckAbsorbIns(false);
                    axEWdraw1.EnableAbsorbDepth(false, 0);
                    axEWdraw1.SetORTHO(false);
                    axEWdraw1.EnableOrthoHVMode(false);
                    axEWdraw1.CancelCommand();
                    axEWdraw1.AddOrRemoveSelect(lastsym);
                    axEWdraw1.ToDrawAbsorb();
                }
                else
                {
                    listView3.SelectedItems.Clear();
                    isdrawmove = true;
                    isdrawabsorb = false;
                    lastsym = axEWdraw1.InsertGroupFromFile("cabinet_01.ewd", new object[] { 0, 0, 0 });//
                    axEWdraw1.GetEntBoundingBox(lastsym, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    double xlength = maxx - minx;
                    double ylength = maxy - miny;
                    double zlength = maxz - minz;
                    string memstr = "xlength:" + String.Format("{0:f3}", xlength) + ";" + "ylength:" + String.Format("{0:f3}", ylength) + ";" + "zlength:" + String.Format("{0:f3}", zlength) + ";";
                    axEWdraw1.EnableAbsorbHigh(false, 0);
                    axEWdraw1.EnableCheckAbsorbIns(false);
                    axEWdraw1.EnableAbsorbDepth(false, 0);
                    axEWdraw1.SetORTHO(false);
                    axEWdraw1.EnableOrthoHVMode(false);
                    axEWdraw1.CancelCommand();
                    axEWdraw1.AddOrRemoveSelect(lastsym);
                    g_mx = (minx + maxx) / 2.0;
                    g_my = (miny + maxy) / 2.0;
                    g_mz = minz;
                    g_st = 2;
                    g_tmpjs = 0;
                    axEWdraw1.ToDrawMove();
                }
            }
            else
                MessageBox.Show("请在平面下绘制");
        }

        private bool IsAbsorbEnt(string name)
        {
            if (name.IndexOf("cabinet") >= 0 ||
                name.IndexOf("chuang") >= 0 ||
                name.IndexOf("pillar") >= 0 ||
                name.IndexOf("selfdraw") >= 0 || //2018-01-31
                name.IndexOf("czyzh") >= 0
                )
            {
                if (!checkBox1.Checked)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private bool IsWallInArea(double sx, double sy, double ex, double ey)
        {
            int ent = 0;
            int type = -1;
            int entsize = axEWdraw1.GetEntSize();
            int tmpjs = 0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);

                if (type == 50)
                {
                    ArrayList tmppts = new ArrayList();
                    GetPtsFromSolid(ent, ref tmppts);
                    int len = tmppts.Count / 3;
                    if (len > 1)
                    {
                        for (int j = 0; j < len - 1; j++)
                        {
                            if (
                                    (Math.Abs(sx - (double)tmppts[j * 3]) < 0.001 &&
                                    Math.Abs(sy - (double)tmppts[j * 3 + 1]) < 0.001 &&
                                    Math.Abs(ex - (double)tmppts[(j + 1) * 3]) < 0.001 &&
                                    Math.Abs(ey - (double)tmppts[(j + 1) * 3 + 1]) < 0.001
                                    )
                                    ||
                                    (Math.Abs(ex - (double)tmppts[j * 3]) < 0.001 &&
                                    Math.Abs(ey - (double)tmppts[j * 3 + 1]) < 0.001 &&
                                    Math.Abs(sx - (double)tmppts[(j + 1) * 3]) < 0.001 &&
                                    Math.Abs(sy - (double)tmppts[(j + 1) * 3 + 1]) < 0.001
                                    )
                                )
                            {
                                tmpjs++;
                            }
                        }
                    }
                    tmppts.Clear();
                }
            }
            if (tmpjs > 0)
                return true;
            return false;
        }
        private void listView3_ItemDrag(object sender, ItemDragEventArgs e)
        {
            foreach (ListViewItem lvi in listView3.SelectedItems)  //选中项遍历 
            {
                if (lvi.Index >= 0 && titemslevel1 == 0)
                {
                    string cname = IndexToItemName(1, lvi.Index);
                    if (cname.Length > 0)
                    {
                        curclassitems1.Clear();
                        ReadSubClassItems(cname, ref curclassitems1);
                        if (curclassitems1.Count > 0)
                        {
                            CreatSubClassItems(1, ref curclassitems1, 1);
                            titemslevel1 = 1;
                        }
                    }
                }
                else if (lvi.Index == 0 && titemslevel1 == 1)
                {
                    CreatSubClassItems(1, ref totalclassitems1, 0);
                    titemslevel1 = 0;
                }
                else if (lvi.Index != 0 && titemslevel1 == 1)
                {//2018-11-12
                    if (IsHaveRoom())
                    {
                        DrawObj(lvi.Index, ref curclassitems1);
                    }
                    else MessageBox.Show("请先绘制户型");
                }
                break;
            }
        }

        private void listView3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView3.SelectedItems)  //选中项遍历 
            {
                if (lvi.Index >= 0 && titemslevel1 == 0)
                {
                    string cname = IndexToItemName(1, lvi.Index);
                    if (cname.Length > 0)
                    {
                        curclassitems1.Clear();
                        ReadSubClassItems(cname, ref curclassitems1);
                        if (curclassitems1.Count > 0)
                        {
                            CreatSubClassItems(1, ref curclassitems1, 1);
                            titemslevel1 = 1;
                        }
                    }
                }
                else if (lvi.Index == 0 && titemslevel1 == 1)
                {
                    CreatSubClassItems(1, ref totalclassitems1, 0);
                    titemslevel1 = 0;
                }
                else if (lvi.Index != 0 && titemslevel1 == 1)
                {//2018-11-12
                    if (IsHaveRoom())
                    {
                        DrawObj(lvi.Index, ref curclassitems1);
                    }
                    else MessageBox.Show("请先绘制户型");
                }
                break;
            }

        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)  //选中项遍历 
            {
                if (g_viewmode == 0)
                {
                    if (titemslevel0 == 0)
                    {
                        switch (lvi.Index)
                        {
                            case 0:
                                {
                                    //绘制墙体
                                    DrawWall();
                                }
                                break;
                            case 1:
                                {
                                    if (lvi.Index >= 0 && titemslevel0 == 0)
                                    {
                                        string cname = IndexToItemName(0, lvi.Index);
                                        g_subclassname = cname;
                                        if (cname.Length > 0)
                                        {
                                            curclassitems0.Clear();
                                            ReadSubClassItems(cname, ref curclassitems0);
                                            if (curclassitems0.Count > 0)
                                            {
                                                CreatSubClassItems(0, ref curclassitems0, 1);
                                                titemslevel0 = 1;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (lvi.Index >= 0 && titemslevel0 == 0)
                                    {
                                        string cname = IndexToItemName(0, lvi.Index);
                                        g_subclassname = cname;
                                        if (cname.Length > 0)
                                        {
                                            curclassitems0.Clear();
                                            ReadSubClassItems(cname, ref curclassitems0);
                                            if (curclassitems0.Count > 0)
                                            {
                                                CreatSubClassItems(0, ref curclassitems0, 1);
                                                titemslevel0 = 1;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制飘窗
                                        DrawBayWin();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 4:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制墙洞
                                        DrawWallHole();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 5:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制墙柱
                                        DrawPillar();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 6:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制七字型飘窗
                                        DrawSevenWin();
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制拱墙洞
                                        DrawWallArchriseHole();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 8:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //自由绘制阳台 2017-11-27
                                        DrawBalcony();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 9:
                                {//2018-01-05
                                    if (IsHaveRoom())
                                    {
                                        DrawProfileWin();
                                    }
                                }
                                break;
                        }
                    }
                    else if (titemslevel0 == 1)
                    {//如果是子对象状态
                        if (lvi.Index == 0 && titemslevel0 == 1)
                        {
                            CreatSubClassItems(0, ref totalclassitems0, 0);
                            titemslevel0 = 0;
                        }
                        else if (lvi.Index != 0 && titemslevel0 == 1)
                        {//2018-11-12
                            if (IsHaveRoom())
                            {
                                //
                                switch (g_subclassname)
                                {
                                    case "门":
                                        {
                                            string grpname = Path.GetFileNameWithoutExtension(((MYImageItem)curclassitems0[lvi.Index - 1]).filename);
                                            double w, t, h, r;
                                            w = t = h = r = 0.0;
                                            GetWTHRFromGrpName(grpname, ref curclassitems0, ref w, ref t, ref h, ref r);
                                            DrawDoor(grpname, w, t, h, r);
                                        }
                                        break;
                                    case "窗户":
                                        {
                                            string grpname = Path.GetFileNameWithoutExtension(((MYImageItem)curclassitems0[lvi.Index - 1]).filename);
                                            double w, t, h, r;
                                            w = t = h = r = 0.0;
                                            GetWTHRFromGrpName(grpname, ref curclassitems0, ref w, ref t, ref h, ref r);
                                            DrawWindow(grpname, w, t, h, r);
                                        }
                                        break;
                                }
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                    }
                }
            }
            if (!axEWdraw1.IsEndCommand())
                axEWdraw1.Focus();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)  //选中项遍历 
            {
                if (g_viewmode == 0)
                {
                    if (titemslevel0 == 0)
                    {
                        switch (lvi.Index)
                        {
                            case 0:
                                {
                                    //绘制墙体
                                    DrawWall();
                                }
                                break;
                            case 1:
                                {
                                    if (lvi.Index >= 0 && titemslevel0 == 0)
                                    {

                                        string cname = IndexToItemName(0, lvi.Index);
                                        g_subclassname = cname;
                                        if (cname.Length > 0)
                                        {
                                            curclassitems0.Clear();
                                            ReadSubClassItems(cname, ref curclassitems0);
                                            if (curclassitems0.Count > 0)
                                            {
                                                CreatSubClassItems(0, ref curclassitems0, 1);
                                                titemslevel0 = 1;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (lvi.Index >= 0 && titemslevel0 == 0)
                                    {
                                        string cname = IndexToItemName(0, lvi.Index);
                                        g_subclassname = cname;
                                        if (cname.Length > 0)
                                        {
                                            curclassitems0.Clear();
                                            ReadSubClassItems(cname, ref curclassitems0);
                                            if (curclassitems0.Count > 0)
                                            {
                                                CreatSubClassItems(0, ref curclassitems0, 1);
                                                titemslevel0 = 1;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制飘窗
                                        DrawBayWin();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 4:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制墙洞
                                        DrawWallHole();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 5:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制墙柱
                                        DrawPillar();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 6:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制七字型飘窗
                                        DrawSevenWin();
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //绘制拱墙洞
                                        DrawWallArchriseHole();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 8:
                                {
                                    if (IsHaveRoom())
                                    {
                                        //自由绘制阳台 2017-11-27
                                        DrawBalcony();
                                    }
                                    else MessageBox.Show("请先绘制户型");
                                }
                                break;
                            case 9:
                                {//2018-01-05
                                    if (IsHaveRoom())
                                    {
                                        DrawProfileWin();
                                    }
                                }
                                break;
                        }
                    }
                    else if (titemslevel0 == 1)
                    {//如果是子对象状态

                        if (lvi.Index == 0 && titemslevel0 == 1)
                        {
                            CreatSubClassItems(0, ref totalclassitems0, 0);
                            titemslevel0 = 0;
                        }
                        else if (lvi.Index != 0 && titemslevel0 == 1)
                        {//2018-11-12
                            if (IsHaveRoom())
                            {
                                switch (g_subclassname)
                                {
                                    case "门":
                                        {
                                            string grpname = Path.GetFileNameWithoutExtension(((MYImageItem)curclassitems0[lvi.Index - 1]).filename);
                                            double w, t, h, r;
                                            w = t = h = r = 0.0;
                                            GetWTHRFromGrpName(grpname, ref curclassitems0, ref w, ref t, ref h, ref r);
                                            DrawDoor(grpname, w, t, h, r);
                                        }
                                        break;
                                    case "窗户":
                                        {
                                            string grpname = Path.GetFileNameWithoutExtension(((MYImageItem)curclassitems0[lvi.Index - 1]).filename);
                                            double w, t, h, r;
                                            w = t = h = r = 0.0;
                                            GetWTHRFromGrpName(grpname, ref curclassitems0, ref w, ref t, ref h, ref r);
                                            DrawWindow(grpname, w, t, h, r);
                                        }
                                        break;
                                }
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                    }
                }
            }
            if (!axEWdraw1.IsEndCommand())
                axEWdraw1.Focus();
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView2.SelectedItems)  //选中项遍历 
            {
                switch (lvi.Index)
                {
                    case 0:
                        {
                            //绘制户型
                            if (IsHaveRoom())
                            {
                                DrawHome();
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                        break;
                    case 1:
                        {
                            //全局
                            if (IsHaveRoom())
                            {

                                GlobalView();
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                        break;
                    case 2:
                        {
                            //单间
                            if (IsHaveRoom())
                            {
                                SingleRoomView();
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                        break;
                    case 3:
                        {
                            //半墙
                            if (IsHaveRoom())
                            {
                                HalfWallView();
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                        break;
                    case 4:
                        {
                            //墙洞
                            if (IsHaveRoom())
                            {
                                WallHoleView();
                            }
                            else MessageBox.Show("请先绘制户型");
                        }
                        break;
                    case 5:
                        {
                            //清空视图
                            if (g_viewmode == 0 && !g_isinnerorbit)
                            {
                                if (timer6.Enabled)//2018-08-03
                                    timer6.Enabled = false;
                                //如果有未结束的命令,则结束命令
                                canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
                                if (!axEWdraw1.IsEndCommand())
                                {
                                    axEWdraw1.CancelCommand();
                                }
                                //清空数据
                                axEWdraw1.DeleteAll();
                                //2016-12-13
                                singlewallids.Clear();
                                dwarfewallids.Clear();
                                topbottomids.Clear();

                                otherroomcabinet.Clear();
                                tmplist.Clear();
                                ptslist.Clear();
                                tlist0.Clear();
                                tlist1.Clear();
                                selids.Clear();


                                orgidlist.Clear();
                                cpidlist.Clear();
                                subtractidlist.Clear();
                                symbolidlist.Clear();
                                door3dsidlist.Clear();
                                singroomhides.Clear();
                                listView2.SelectedItems.Clear();
                                //2017-12-15 清空自由绘制阳台的参数
                                g_balconyent = 0;
                                if (g_balconypts != null)
                                    g_balconypts.Clear();
                                if (g_balconysegpts != null)
                                    g_balconysegpts.Clear();

                                if (g_balconywins != null)
                                    g_balconywins.Clear();
                                if (g_balconyids != null)
                                    g_balconyids.Clear();
                                if (g_balconyhidewall != null)
                                    g_balconyhidewall.Clear();
                                if (g_balconyotherids != null)
                                    g_balconyotherids.Clear();
                                if (g_balconyconnectpts != null)
                                    g_balconyconnectpts.Clear();
                                //
                                g_state = InteractiveState.Nothing;
                                g_id = 0;
                                g_inx = -1;
                                g_x = g_y = g_z = 0.0;
                                g_wr = g_wg = g_wb = 150;
                                //
                                axEWdraw1.SetWallDefColor(g_wr, g_wg, g_wb);
                                axEWdraw1.SetShapeSmooth(3);
                                //
                                axEWdraw1.SetBackGroundColor(axEWdraw1.RGBToIndex(255, 255, 245));
                                axEWdraw1.SetLayerColor("0", axEWdraw1.RGBToIndex(g_wr, g_wg, g_wb));

                                axEWdraw1.SetGridValue(300, 300, 15000, 15000, 0);
                                axEWdraw1.SetGridOrgPt(-15000, -15000);
                                axEWdraw1.SetGridColor(214, 214, 214, 214, 214, 214);
                                axEWdraw1.SetGridOn(true);
                                axEWdraw1.DisableRightMenu(true);

                                //
                                axEWdraw1.SetSingleWallThickness(g_wallthickness);
                                //设置在绘制polyline线时显示动态的长度标注 2016-09-22
                                axEWdraw1.EnableDrawPolyLineDymDim(true, 100, 0, 0, 0, 300, g_maxz);
                                //设置在吸附窗或门时,如果吸附失败,则删除要吸附的窗或门 2016-10-11
                                axEWdraw1.EnableDelGrpWhenAbsorbFail(true);
                                //设置在承重墙限制的类型,也就是如果组的名称中有此字符,则限制 2016-10-14
                                axEWdraw1.AddWallLimitClass("door");
                                axEWdraw1.AddWallLimitClass("window");
                                axEWdraw1.AddWallLimitClass("baywin");
                                axEWdraw1.AddWallLimitClass("hole");
                                //
                                axEWdraw1.EnableWallPipe(true);
                                //
                                axEWdraw1.DisableRectSelect(true);
                                axEWdraw1.DisableDelKeyArea(true);
                                //
                                axEWdraw1.EnableDrawWallAlignLine(true, g_wallthickness, 128);
                                axEWdraw1.SetDrawPolyLineHintPoint(150, 128, 0.7, 3000);
                                axEWdraw1.EnableDrawPolyLineColor(true, 128);
                                //
                                axEWdraw1.SetPerspectiveMode(false);
                                axEWdraw1.RestoreView();
                                g_viewmode = 0;
                            }
                            else MessageBox.Show("只能在绘制户型时清空设计");
                        }
                        break;
                    case 6:
                        {//将视图中的实体撑满
                            if (!g_isinnerorbit)
                                axEWdraw1.ZoomALL();
                            else MessageBox.Show("漫游模式下,不支持该功能");
                        }
                        break;
                    case 7:
                        {//环视
                            if (g_viewmode != 0 && !g_isinnerorbit)
                            {
                                if (IsHaveRoom())
                                {
                                    axEWdraw1.ToDrawOrbit();
                                }
                                else MessageBox.Show("请先绘制户型");
                            }
                            else MessageBox.Show("只有全局,单间,半墙,墙洞模式的三维状态下使用");
                        }
                        break;
                }
                break;
            }
            if (!axEWdraw1.IsEndCommand())
                axEWdraw1.Focus();
        }

        //判断当前视图中是否已经存在画好的户型
        private bool IsHaveRoom()
        {
            int entsize = axEWdraw1.GetEntSize();
            //形成字符串
            int ent = 0;
            int type = 0;
            string str;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    str = GetProStrFromEnt(ent, "area");
                    if (str.Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private class myInxWallCompare : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((SInxWall)x).inx - ((SInxWall)y).inx;
            }
        }
        private class myDistCompare : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                if (((SInxWall)x).dist < ((SInxWall)y).dist)
                    return -1;
                else if (((SInxWall)x).dist > ((SInxWall)y).dist)
                    return 1;
                else
                    return 0;
            }
        }
        /*GetIntfromProStr将字段字符串值变为整值
         * 参数:
         * str 输入 表示数值的字符串
         * 返回值:
         * 返回转换后数值
         */
        private int GetIntfromProStr(string str)
        {
            if (str.Length > 0)
            {
                int finx = str.IndexOf(',');//排除用逗号隔开的数组形式
                if (finx < 0)
                    return Convert.ToInt32(str);
            }
            return 0;
        }

        /*根据一个点找到最近的墙端点,并返回与该最近端点相关的墙的信息 2017-09-22
         * 如果是拐点,且只有两条相互成直角的水平与垂直墙相关,则判断符合条件
         * x,y,z      给定的点
         * thickness  当前墙的厚度
         * hneg,vneg  分别为水平(X轴),垂直(Y轴)方向的正负,1表示下,-1表示负值
         * ix,iy,iz   计算得到的内部拐角的点
         * fx,fy,fz   计算得到的最近墙端点,也就是相互垂直墙的共点
         * id1        七字型飘窗的相关的墙段1
         * id2        七字型飘窗的相关的墙段2
         */
        private bool GetCornerByPt(double x, double y, double z,
                                  double thickness,
                                  ref int hneg, ref int vneg,
                                  ref double ix, ref double iy, ref double iz,
                                  ref double fx, ref double fy, ref double fz,
                                  ref int id1, ref int id2
                                )
        {
            ArrayList owall = new ArrayList();
            int swsize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez;
            sx = sy = sz = ex = ey = ez = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            double dist = 0;
            int tmpjs = 0;
            double mindist;
            mindist = 0.0;
            ArrayList allwall = new ArrayList();
            //求距离最近的墙端点
            int mininx = -1;
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                SWallSeg awall = new SWallSeg();
                awall.id = singleid;
                awall.inx = tmpjs;
                awall.x1 = sx;
                awall.y1 = sy;
                awall.z1 = sz;
                awall.x2 = ex;
                awall.y2 = ey;
                awall.z2 = ez;
                allwall.Add(awall);

                if (axEWdraw1.GetDistPtAndLineSeg(new object[] { x, y, 0 }, new object[] { sx, sy, 0 },
                                                    new object[] { ex, ey, 0 },
                                                    ref dist, ref ox, ref oy, ref oz))
                {
                    //将墙的起始与终止端点放入到数据集内,之后会用到
                    if (tmpjs == 0)
                    {
                        mindist = dist;
                        mininx = i;
                    }
                    else if (dist < mindist)
                    {
                        mindist = dist;
                        mininx = i;
                    }
                    tmpjs++;
                }
            }
            //选择最近的墙段,在墙端最近的点
            double cx, cy, cz;
            cx = cy = cz = 0.0;
            bool ishv1 = false;
            if (mininx >= 0)
            {
                if (axEWdraw1.PointDistance(((SWallSeg)allwall[mininx]).x1, ((SWallSeg)allwall[mininx]).y1, 0, x, y, 0) < axEWdraw1.PointDistance(((SWallSeg)allwall[mininx]).x2, ((SWallSeg)allwall[mininx]).y2, 0, x, y, 0))
                {
                    cx = ((SWallSeg)allwall[mininx]).x1;
                    cy = ((SWallSeg)allwall[mininx]).y1;
                    cz = ((SWallSeg)allwall[mininx]).z1;
                    if (Math.Abs(((SWallSeg)allwall[mininx]).x2 - ((SWallSeg)allwall[mininx]).x1) < 0.001)
                    {
                        //水平
                        ishv1 = true;
                        SWallSeg awall = new SWallSeg();
                        awall.x1 = ((SWallSeg)allwall[mininx]).x1;
                        awall.y1 = ((SWallSeg)allwall[mininx]).y1;
                        awall.z1 = ((SWallSeg)allwall[mininx]).z1;
                        awall.x2 = ((SWallSeg)allwall[mininx]).x2;
                        awall.y2 = ((SWallSeg)allwall[mininx]).y2;
                        awall.z2 = ((SWallSeg)allwall[mininx]).z2;
                        awall.vx = ((SWallSeg)allwall[mininx]).x2 - ((SWallSeg)allwall[mininx]).x1;
                        awall.vy = ((SWallSeg)allwall[mininx]).y2 - ((SWallSeg)allwall[mininx]).y1;
                        awall.vz = ((SWallSeg)allwall[mininx]).z2 - ((SWallSeg)allwall[mininx]).z1;
                        awall.id = ((SWallSeg)allwall[mininx]).id;
                        awall.inx = mininx;
                        owall.Add(awall);
                    }
                    else if (Math.Abs(((SWallSeg)allwall[mininx]).y2 - ((SWallSeg)allwall[mininx]).y1) < 0.001)
                    {
                        //垂直
                        ishv1 = true;
                        SWallSeg awall = new SWallSeg();
                        awall.x1 = ((SWallSeg)allwall[mininx]).x1;
                        awall.y1 = ((SWallSeg)allwall[mininx]).y1;
                        awall.z1 = ((SWallSeg)allwall[mininx]).z1;
                        awall.x2 = ((SWallSeg)allwall[mininx]).x2;
                        awall.y2 = ((SWallSeg)allwall[mininx]).y2;
                        awall.z2 = ((SWallSeg)allwall[mininx]).z2;
                        awall.vx = ((SWallSeg)allwall[mininx]).x2 - ((SWallSeg)allwall[mininx]).x1;
                        awall.vy = ((SWallSeg)allwall[mininx]).y2 - ((SWallSeg)allwall[mininx]).y1;
                        awall.vz = ((SWallSeg)allwall[mininx]).z2 - ((SWallSeg)allwall[mininx]).z1;
                        awall.id = ((SWallSeg)allwall[mininx]).id;
                        awall.inx = mininx;
                        owall.Add(awall);
                    }
                }
                else
                {
                    cx = ((SWallSeg)allwall[mininx]).x2;
                    cy = ((SWallSeg)allwall[mininx]).y2;
                    cz = ((SWallSeg)allwall[mininx]).z2;
                    if (Math.Abs(((SWallSeg)allwall[mininx]).x2 - ((SWallSeg)allwall[mininx]).x1) < 0.001)
                    {
                        //水平
                        ishv1 = true;
                        SWallSeg awall = new SWallSeg();
                        awall.x1 = ((SWallSeg)allwall[mininx]).x2;
                        awall.y1 = ((SWallSeg)allwall[mininx]).y2;
                        awall.z1 = ((SWallSeg)allwall[mininx]).z2;
                        awall.x2 = ((SWallSeg)allwall[mininx]).x1;
                        awall.y2 = ((SWallSeg)allwall[mininx]).y1;
                        awall.z2 = ((SWallSeg)allwall[mininx]).z1;
                        awall.vx = ((SWallSeg)allwall[mininx]).x1 - ((SWallSeg)allwall[mininx]).x2;
                        awall.vy = ((SWallSeg)allwall[mininx]).y1 - ((SWallSeg)allwall[mininx]).y2;
                        awall.vz = ((SWallSeg)allwall[mininx]).z1 - ((SWallSeg)allwall[mininx]).z2;
                        awall.id = ((SWallSeg)allwall[mininx]).id;
                        awall.inx = mininx;
                        owall.Add(awall);
                    }
                    else if (Math.Abs(((SWallSeg)allwall[mininx]).y2 - ((SWallSeg)allwall[mininx]).y1) < 0.001)
                    {
                        //垂直
                        ishv1 = true;
                        SWallSeg awall = new SWallSeg();
                        awall.x1 = ((SWallSeg)allwall[mininx]).x2;
                        awall.y1 = ((SWallSeg)allwall[mininx]).y2;
                        awall.z1 = ((SWallSeg)allwall[mininx]).z2;
                        awall.x2 = ((SWallSeg)allwall[mininx]).x1;
                        awall.y2 = ((SWallSeg)allwall[mininx]).y1;
                        awall.z2 = ((SWallSeg)allwall[mininx]).z1;
                        awall.vx = ((SWallSeg)allwall[mininx]).x1 - ((SWallSeg)allwall[mininx]).x2;
                        awall.vy = ((SWallSeg)allwall[mininx]).y1 - ((SWallSeg)allwall[mininx]).y2;
                        awall.vz = ((SWallSeg)allwall[mininx]).z1 - ((SWallSeg)allwall[mininx]).z2;
                        awall.id = ((SWallSeg)allwall[mininx]).id;
                        awall.inx = mininx;
                        owall.Add(awall);
                    }
                }//
                //根据已经判定的墙的端点,判断与该端点相同的墙
                double dist1, dist2;
                dist1 = dist2 = 0.0;
                for (int i = 0; i < allwall.Count; i++)
                {
                    if (i != mininx)
                    {
                        dist1 = axEWdraw1.PointDistance(((SWallSeg)allwall[i]).x1, ((SWallSeg)allwall[i]).y1, 0, cx, cy, 0);
                        dist2 = axEWdraw1.PointDistance(((SWallSeg)allwall[i]).x2, ((SWallSeg)allwall[i]).y2, 0, cx, cy, 0);
                        if (dist1 < 0.001 ||
                             dist2 < 0.001
                            )
                        {
                            if (dist1 < dist2)
                            {
                                if (Math.Abs(((SWallSeg)allwall[i]).x2 - ((SWallSeg)allwall[i]).x1) < 0.001)
                                {
                                    //水平
                                    ishv1 = true;
                                    SWallSeg awall = new SWallSeg();
                                    awall.x1 = ((SWallSeg)allwall[i]).x1;
                                    awall.y1 = ((SWallSeg)allwall[i]).y1;
                                    awall.z1 = ((SWallSeg)allwall[i]).z1;
                                    awall.x2 = ((SWallSeg)allwall[i]).x2;
                                    awall.y2 = ((SWallSeg)allwall[i]).y2;
                                    awall.z2 = ((SWallSeg)allwall[i]).z2;
                                    awall.vx = ((SWallSeg)allwall[i]).x2 - ((SWallSeg)allwall[i]).x1;
                                    awall.vy = ((SWallSeg)allwall[i]).y2 - ((SWallSeg)allwall[i]).y1;
                                    awall.vz = ((SWallSeg)allwall[i]).z2 - ((SWallSeg)allwall[i]).z1;
                                    awall.id = ((SWallSeg)allwall[i]).id;
                                    awall.inx = mininx;
                                    owall.Add(awall);
                                }
                                else if (Math.Abs(((SWallSeg)allwall[i]).y2 - ((SWallSeg)allwall[i]).y1) < 0.001)
                                {
                                    //垂直
                                    ishv1 = true;
                                    SWallSeg awall = new SWallSeg();
                                    awall.x1 = ((SWallSeg)allwall[i]).x1;
                                    awall.y1 = ((SWallSeg)allwall[i]).y1;
                                    awall.z1 = ((SWallSeg)allwall[i]).z1;
                                    awall.x2 = ((SWallSeg)allwall[i]).x2;
                                    awall.y2 = ((SWallSeg)allwall[i]).y2;
                                    awall.z2 = ((SWallSeg)allwall[i]).z2;
                                    awall.vx = ((SWallSeg)allwall[i]).x2 - ((SWallSeg)allwall[i]).x1;
                                    awall.vy = ((SWallSeg)allwall[i]).y2 - ((SWallSeg)allwall[i]).y1;
                                    awall.vz = ((SWallSeg)allwall[i]).z2 - ((SWallSeg)allwall[i]).z1;
                                    awall.id = ((SWallSeg)allwall[i]).id;
                                    awall.inx = mininx;
                                    owall.Add(awall);
                                }
                            }
                            else
                            {
                                if (Math.Abs(((SWallSeg)allwall[i]).x2 - ((SWallSeg)allwall[i]).x1) < 0.001)
                                {
                                    //水平
                                    ishv1 = true;
                                    SWallSeg awall = new SWallSeg();
                                    awall.x1 = ((SWallSeg)allwall[i]).x2;
                                    awall.y1 = ((SWallSeg)allwall[i]).y2;
                                    awall.z1 = ((SWallSeg)allwall[i]).z2;
                                    awall.x2 = ((SWallSeg)allwall[i]).x1;
                                    awall.y2 = ((SWallSeg)allwall[i]).y1;
                                    awall.z2 = ((SWallSeg)allwall[i]).z1;
                                    awall.vx = ((SWallSeg)allwall[i]).x1 - ((SWallSeg)allwall[i]).x2;
                                    awall.vy = ((SWallSeg)allwall[i]).y1 - ((SWallSeg)allwall[i]).y2;
                                    awall.vz = ((SWallSeg)allwall[i]).z1 - ((SWallSeg)allwall[i]).z2;
                                    awall.id = ((SWallSeg)allwall[i]).id;
                                    awall.inx = mininx;
                                    owall.Add(awall);
                                }
                                else if (Math.Abs(((SWallSeg)allwall[i]).y2 - ((SWallSeg)allwall[i]).y1) < 0.001)
                                {
                                    //垂直
                                    ishv1 = true;
                                    SWallSeg awall = new SWallSeg();
                                    awall.x1 = ((SWallSeg)allwall[i]).x2;
                                    awall.y1 = ((SWallSeg)allwall[i]).y2;
                                    awall.z1 = ((SWallSeg)allwall[i]).z2;
                                    awall.x2 = ((SWallSeg)allwall[i]).x1;
                                    awall.y2 = ((SWallSeg)allwall[i]).y1;
                                    awall.z2 = ((SWallSeg)allwall[i]).z1;
                                    awall.vx = ((SWallSeg)allwall[i]).x1 - ((SWallSeg)allwall[i]).x2;
                                    awall.vy = ((SWallSeg)allwall[i]).y1 - ((SWallSeg)allwall[i]).y2;
                                    awall.vz = ((SWallSeg)allwall[i]).z1 - ((SWallSeg)allwall[i]).z2;
                                    awall.id = ((SWallSeg)allwall[i]).id;
                                    awall.inx = mininx;
                                    owall.Add(awall);
                                }
                            }
                        }
                    }
                }
                //判断个数是否符合条件,如果超两个说明不符合条件
                if (owall.Count == 2)
                {
                    //判断两个墙段的角度是否是90度
                    double ang = axEWdraw1.VectorAngle(new object[] { ((SWallSeg)owall[0]).vx, ((SWallSeg)owall[0]).vy, ((SWallSeg)owall[0]).vz },
                                                       new object[] { ((SWallSeg)owall[1]).vx, ((SWallSeg)owall[1]).vy, ((SWallSeg)owall[1]).vz }
                                                        );
                    if (Math.Abs(Math.PI / 2.0 - ang) < 0.001)
                    {
                        allwall.Clear();
                        //水平
                        if (Math.Abs(((SWallSeg)owall[0]).vx) > 0.001 && Math.Abs(((SWallSeg)owall[0]).vy) < 0.001)
                        {
                            if (((SWallSeg)owall[0]).vx > 0)
                                hneg = 1;
                            else
                                hneg = -1;
                        }
                        if (Math.Abs(((SWallSeg)owall[0]).vy) > 0.001 && Math.Abs(((SWallSeg)owall[0]).vx) < 0.001)
                        {//垂直
                            if (((SWallSeg)owall[0]).vy > 0)
                                vneg = 1;
                            else
                                vneg = -1;
                        }

                        //水平
                        if (Math.Abs(((SWallSeg)owall[1]).vx) > 0.001 && Math.Abs(((SWallSeg)owall[1]).vy) < 0.001)
                        {
                            if (((SWallSeg)owall[1]).vx > 0)
                                hneg = 1;
                            else
                                hneg = -1;
                        }
                        if (Math.Abs(((SWallSeg)owall[1]).vy) > 0.001 && Math.Abs(((SWallSeg)owall[1]).vx) < 0.001)
                        {//垂直
                            if (((SWallSeg)owall[1]).vy > 0)
                                vneg = 1;
                            else
                                vneg = -1;
                        }

                        //计算内向拐点,也就是墙向部拐点的位置
                        if (hneg > 0 && vneg > 0)
                        {//左下角
                            ix = ((SWallSeg)owall[0]).x1 + thickness / 2.0;
                            iy = ((SWallSeg)owall[0]).y1 + thickness / 2.0;
                        }
                        else if (hneg < 0 && vneg > 0)
                        {//右下角
                            ix = ((SWallSeg)owall[0]).x1 - thickness / 2.0;
                            iy = ((SWallSeg)owall[0]).y1 + thickness / 2.0;
                        }
                        else if (hneg < 0 && vneg < 0)
                        {//右上角
                            ix = ((SWallSeg)owall[0]).x1 - thickness / 2.0;
                            iy = ((SWallSeg)owall[0]).y1 - thickness / 2.0;
                        }
                        else if (hneg > 0 && vneg < 0)
                        {//左上角
                            ix = ((SWallSeg)owall[0]).x1 + thickness / 2.0;
                            iy = ((SWallSeg)owall[0]).y1 - thickness / 2.0;
                        }
                        iz = z;
                        fx = ((SWallSeg)owall[0]).x1;
                        fy = ((SWallSeg)owall[0]).y1;
                        fz = z;
                        id1 = ((SWallSeg)owall[0]).id;
                        id2 = ((SWallSeg)owall[1]).id;
                        owall.Clear();
                        return true;
                    }
                    else
                        owall.Clear();
                }
                else
                    owall.Clear();
            }
            allwall.Clear();
            return false;
        }
        /*创建七字型飘窗 2017-09-22
         * 参数:
         * cx,cy,cz  是放置飘窗的内拐点的位置
         * hwidth    是横向的宽度(长度),这里的横向是指X轴方向,正值表示正方向,负值表示负方向.
         * vwidth    是竖向的宽度(长度),这里的竖向是指Y轴方向,正值表示正方向,负值表示负方向.
         * thickness 是飘窗向外延伸墙的厚度,注意这里的厚度只是飘窗延伸的厚度,不是全局墙厚.
         * height    是飘窗的不带上下墙厚的内部空间(洞)的高度
         * depth     是深度,也就是从墙内到墙处的深度
         * ridz      是z值的位置,注意,这里的位置是指七字型飘窗底部墙厚以上的Z值位置.
         * fx,fy,fz  墙端点,也就是相互垂直墙的共点
         */
        private int MakeSevenWinSymbol(double cx, double cy, double cz, double hwidth, double vwidth,
                                        double thickness, double height, double depth, double ridz,
                                        double fx, double fy, double fz, int id1, int id2)
        {
            int bottom, face, sevenwin;
            double x, y, z, x1, y1, z1;
            double vx, vy, vz, vx1, vy1, vz1;
            bottom = face = sevenwin = 0;
            x = y = z = x1 = y1 = z1 = 0;
            vx = vy = vz = vx1 = vy1 = vz1 = 0;
            double maxz = 3000;
            //首先判断该位置是已经有七字窗,如果有则删除该七型窗的符号
            int existent = IsExistSevenWin(fx, fy);
            if (existent > 0)
            {
                axEWdraw1.RemoveSymbolFromWall(existent);
            }
            //临时点数据信息
            ArrayList tmpsevenpts = new ArrayList();
            //创建7型窗的基本底部形状
            axEWdraw1.Clear3DPtBuf();
            axEWdraw1.Polar(new object[] { cx, cy, cz }, new object[] { 1, 0, 0 }, hwidth, ref x, ref y, ref z);
            //内边,横向作第一点
            axEWdraw1.AddOne3DVertex(x, y, z);
            tmpsevenpts.Add(new SPoint(x, y, z));
            //内边,拐点作为第二点
            axEWdraw1.AddOne3DVertex(cx, cy, cz);
            tmpsevenpts.Add(new SPoint(cx, cy, cz));
            //内边,竖向作第三点
            axEWdraw1.Polar(new object[] { cx, cy, cz }, new object[] { 0, 1, 0 }, vwidth, ref x, ref y, ref z);
            axEWdraw1.AddOne3DVertex(x, y, z);
            tmpsevenpts.Add(new SPoint(x, y, z));
            //判断深度的方向
            int cornertype = 0;
            if (hwidth > 0 && vwidth > 0)
            {//左下
                //横向方向
                vx = -1.0;
                vy = 0;
                vz = 0;
                //竖向方向
                vx1 = 0;
                vy1 = -1;
                vz1 = 0;
                cornertype = 1;
            }
            else if (hwidth > 0 && vwidth < 0)
            {//左上
                //横向方向
                vx = -1.0;
                vy = 0;
                vz = 0;
                //竖向方向
                vx1 = 0;
                vy1 = 1;
                vz1 = 0;
                cornertype = 2;
            }
            else if (hwidth < 0 && vwidth < 0)
            {//右上
                vx = 1.0;
                vy = 0;
                vz = 0;

                vx1 = 0;
                vy1 = 1;
                vz1 = 0;
                cornertype = 3;
            }
            else if (hwidth < 0 && vwidth > 0)
            {//右下 
                vx = 1.0;
                vy = 0;
                vz = 0;

                vx1 = 0;
                vy1 = -1;
                vz1 = 0;
                cornertype = 4;
            }
            else
            {
                vx = 1.0;
                vy = 0;
                vz = 0;

                vx1 = 0;
                vy1 = -1;
                vz1 = 0;
            }
            //处边,竖向第一点
            axEWdraw1.Polar(new object[] { x, y, z }, new object[] { vx, vy, vz }, depth, ref x1, ref y1, ref z1);
            axEWdraw1.AddOne3DVertex(x1, y1, z1);
            tmpsevenpts.Add(new SPoint(x1, y1, z1));
            x = x1;
            y = y1;
            z = z1;
            //处边,竖向第二点(横向起始点)
            axEWdraw1.Polar(new object[] { x, y, z }, new object[] { vx1, vy1, vz1 }, Math.Abs(vwidth) + depth, ref x1, ref y1, ref z1);
            axEWdraw1.AddOne3DVertex(x1, y1, z1);
            tmpsevenpts.Add(new SPoint(x1, y1, z1));
            x = x1;
            y = y1;
            z = z1;
            //处边,竖向第二点(横向起始点)
            axEWdraw1.Polar(new object[] { x, y, z }, new object[] { -vx, -vy, -vz }, Math.Abs(hwidth) + depth, ref x1, ref y1, ref z1);
            axEWdraw1.AddOne3DVertex(x1, y1, z1);
            tmpsevenpts.Add(new SPoint(x1, y1, z1));
            x = x1;
            y = y1;
            z = z1;
            //七型飘窗的轮廓线,一定是需要.之后做数据分析使用.
            bottom = axEWdraw1.PolyLine3D(true);
            axEWdraw1.Clear3DPtBuf();
            axEWdraw1.SetEntColor(bottom, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(bottom, 2);
            //
            face = axEWdraw1.EntToFace(bottom, false);
            sevenwin = axEWdraw1.Prism(face, height, new object[] { 0, 0, 1 });
            axEWdraw1.SetEntColor(sevenwin, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.Delete(face);
            axEWdraw1.SetTransparency(sevenwin, 0.3);
            axEWdraw1.MoveTo(sevenwin, new object[] { 0, 0, 0 }, new object[] { 0, 0, ridz });
            int top = axEWdraw1.Copy(bottom, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000.0 });
            axEWdraw1.SetEntColor(top, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(top, 2);
            //绘制符号中线,只是辅助线也可以不要
            ArrayList lineids = new ArrayList();
            ProcSevenWinSymbolLine(cornertype, depth, thickness, 3, ref tmpsevenpts, ref lineids);
            tmpsevenpts.Clear();
            //创建七字型飘窗的符号组
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(sevenwin);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(bottom);
            axEWdraw1.AddIDToBuffer(top);
            for (int i = 0; i < lineids.Count; i++)
                axEWdraw1.AddIDToBuffer((int)lineids[i]);
            lineids.Clear();
            //
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup("symbol_sevenwin_1", new object[] { cx, cy, ridz });
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.SetGroupInsPt(group, new object[] { fx, fy, ridz });
            //设置组的数据
            string str = "sevenwin:1;" + "hwidth:" + hwidth.ToString() + ";" + "vwidth:" + vwidth.ToString() + ";" + "height:" + height.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "depth:" + depth.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            str += "cornerpt:" + String.Format("{0:f3}", fx) + "," + String.Format("{0:f3}", fy) + "," + String.Format("{0:f3}", fz) + ";";
            str += "cornertype:" + cornertype.ToString() + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //判断与之相关的墙面
            ArrayList relateids = new ArrayList();
            if (GetSevenWinRelateIDs(fx, fy, cornertype, vwidth, hwidth, thickness, ref relateids) > 0)
            {
                for (int i = 0; i < relateids.Count; i++)
                {
                    axEWdraw1.AddSymbolToSingleWall(group, (int)relateids[i]);
                    axEWdraw1.UpdateSingleWall((int)relateids[i]);
                }
            }
            relateids.Clear();
            //
            return group;
        }

        //是否已经存在七字窗
        private int IsExistSevenWin(double x, double y)
        {
            int entsize = axEWdraw1.GetEntSize();
            int ent = 0;
            double ix, iy, iz;
            ix = iy = iz = 0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                if (axEWdraw1.IsGroup(ent))
                {
                    string grpname = axEWdraw1.GetGroupName(ent);
                    if (grpname.IndexOf("symbol_sevenwin") >= 0)
                    {//如果是七型飘窗
                        axEWdraw1.GetGroupInsPt(ent, ref ix, ref iy, ref iz);
                        if (axEWdraw1.PointDistance(x, y, 0, ix, iy, 0) < 0.001)
                        {
                            return ent;
                        }
                    }
                }
            }
            return 0;
        }
        //查找与七字窗相关的单墙集
        private int GetSevenWinRelateIDs(double x, double y, int cornertype, double vwidth, double hwidth, double thickness, ref ArrayList ids)
        {
            double v_x1, v_y1, v_x2, v_y2, h_x1, h_y1, h_x2, h_y2;//竖向与横向的判断线
            v_x1 = v_y1 = v_x2 = v_y2 = h_x1 = h_y1 = h_x2 = h_y2 = 0.0;
            switch (cornertype)
            {
                case 1:
                    {//左下
                        v_x1 = x;
                        v_y1 = y;
                        v_x2 = x;
                        v_y2 = y + thickness / 2.0 + vwidth;

                        h_x1 = x;
                        h_y1 = y;
                        h_x2 = x + thickness / 2.0 + hwidth;
                        h_y2 = y;
                    }
                    break;
                case 2:
                    {//左上
                        v_x1 = x;
                        v_y1 = y;
                        v_x2 = x;
                        v_y2 = y - thickness / 2.0 + vwidth;

                        h_x1 = x;
                        h_y1 = y;
                        h_x2 = x + thickness / 2.0 + hwidth;
                        h_y2 = y;
                    }
                    break;
                case 3:
                    {//右上
                        v_x1 = x;
                        v_y1 = y;
                        v_x2 = x;
                        v_y2 = y - thickness / 2.0 + vwidth;

                        h_x1 = x;
                        h_y1 = y;
                        h_x2 = x - thickness / 2.0 + hwidth;
                        h_y2 = y;
                    }
                    break;
                case 4:
                    {//右下
                        v_x1 = x;
                        v_y1 = y;
                        v_x2 = x;
                        v_y2 = y + thickness / 2.0 + vwidth;

                        h_x1 = x;
                        h_y1 = y;
                        h_x2 = x - thickness / 2.0 + hwidth;
                        h_y2 = y;
                    }
                    break;

            }
            int swsize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez;
            sx = sy = sz = ex = ey = ez = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                if (axEWdraw1.Is2DLineOver(h_x1, h_y1, h_x2, h_y2, sx, sy, ex, ey))
                {
                    ids.Add(singleid);
                }
                else if (axEWdraw1.Is2DLineOver(v_x1, v_y1, v_x2, v_y2, sx, sy, ex, ey))
                {
                    ids.Add(singleid);
                }
            }
            //
            return ids.Count;
        }

        //处理7字窗沿墙外墙的线,如果是为生成顶底,也可以不用这个函数.不用该函数的话(或off为0)嵌完全入到墙内.
        private void ProcSevenWinPolyline(int cornertype, double off, ref ArrayList sevenwinpolyline)
        {
            double dist = Math.Sqrt(off * off * 2.0);
            double x, y, z;
            x = y = z = 0;
            switch (cornertype)
            {
                case 1:
                    {//左下
                        ((SPoint)sevenwinpolyline[0]).y -= off;
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenwinpolyline[1]).x, ((SPoint)sevenwinpolyline[1]).y, ((SPoint)sevenwinpolyline[1]).z },
                                        new object[] { -1, -1, 0 }, dist, ref x, ref y, ref z);
                        ((SPoint)sevenwinpolyline[1]).x = x;
                        ((SPoint)sevenwinpolyline[1]).y = y;
                        ((SPoint)sevenwinpolyline[1]).z = z;
                        ((SPoint)sevenwinpolyline[2]).x -= off;
                    }
                    break;
                case 2:
                    {//左上
                        ((SPoint)sevenwinpolyline[0]).y += off;
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenwinpolyline[1]).x, ((SPoint)sevenwinpolyline[1]).y, ((SPoint)sevenwinpolyline[1]).z },
                                        new object[] { -1, 1, 0 }, dist, ref x, ref y, ref z);
                        axEWdraw1.Point(new object[] { x, y, z });
                        ((SPoint)sevenwinpolyline[1]).x = x;
                        ((SPoint)sevenwinpolyline[1]).y = y;
                        ((SPoint)sevenwinpolyline[1]).z = z;
                        ((SPoint)sevenwinpolyline[2]).x -= off;
                    }
                    break;
                case 3:
                    {//右上
                        ((SPoint)sevenwinpolyline[0]).y += off;
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenwinpolyline[1]).x, ((SPoint)sevenwinpolyline[1]).y, ((SPoint)sevenwinpolyline[1]).z },
                                        new object[] { 1, 1, 0 }, dist, ref x, ref y, ref z);
                        axEWdraw1.Point(new object[] { x, y, z });
                        ((SPoint)sevenwinpolyline[1]).x = x;
                        ((SPoint)sevenwinpolyline[1]).y = y;
                        ((SPoint)sevenwinpolyline[1]).z = z;
                        ((SPoint)sevenwinpolyline[2]).x += off;
                    }
                    break;
                case 4:
                    {//右下
                        ((SPoint)sevenwinpolyline[0]).y -= off;
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenwinpolyline[1]).x, ((SPoint)sevenwinpolyline[1]).y, ((SPoint)sevenwinpolyline[1]).z },
                                        new object[] { 1, -1, 0 }, dist, ref x, ref y, ref z);
                        axEWdraw1.Point(new object[] { x, y, z });
                        ((SPoint)sevenwinpolyline[1]).x = x;
                        ((SPoint)sevenwinpolyline[1]).y = y;
                        ((SPoint)sevenwinpolyline[1]).z = z;
                        ((SPoint)sevenwinpolyline[2]).x += off;
                    }
                    break;
            }
        }
        //从符号到七字型飘窗的创建 2017-10-10
        private int Import3DsSevenWin(int symbolid, string filePath, string grpname)
        {
            if (axEWdraw1.IsGroup(symbolid))
            {
                string gname = axEWdraw1.GetGroupName(symbolid);
                if (gname.IndexOf("symbol_sevenwin") >= 0)
                {
                    double height = 0;
                    double thickness = 0;
                    int cornertype = 0;
                    string hstr = GetProStrFromEnt(symbolid, "height");
                    string tstr = GetProStrFromEnt(symbolid, "thickness");
                    string ctstr = GetProStrFromEnt(symbolid, "cornertype");
                    if (hstr.Length > 0)
                        height = GetDblfromProStr(hstr);
                    if (tstr.Length > 0)
                        thickness = GetDblfromProStr(tstr);
                    if (ctstr.Length > 0)
                        cornertype = GetIntfromProStr(ctstr);
                    double ix, iy, iz;
                    ix = iy = iz = 0.0;
                    axEWdraw1.GetGroupInsPt(symbolid, ref ix, ref iy, ref iz);
                    axEWdraw1.ClearIDBuffer();
                    ArrayList polylinepts = new ArrayList();
                    int bottom = 0;
                    if (axEWdraw1.GetGroupAllIDs(symbolid))
                    {
                        //线段信息数据
                        ArrayList segments = new ArrayList();
                        //
                        int idsize = axEWdraw1.GetIDBufferSize();
                        for (int i = 0; i < idsize; i++)
                        {
                            int ent = axEWdraw1.GetIDBuffer(i);
                            if (ent > 0)
                            {
                                if (axEWdraw1.GetEntType(ent) == 9)
                                {
                                    bottom = ent;
                                    //分析3d polyline线数据
                                    int segnum = axEWdraw1.GetPolyLineSegmentSize(ent);
                                    if (segnum > 0)
                                    {
                                        double x1, y1, z1, x2, y2, z2, vx, vy, vz;
                                        x1 = y1 = z1 = x2 = y2 = z2 = vx = vy = vz = 0.0;
                                        bool isarc = false;

                                        for (int j = 0; j < segnum; j++)
                                        {
                                            if (axEWdraw1.GetPolyLineSegment(ent, j, ref x1, ref y1, ref z1, ref x2, ref y2, ref z2, ref vx, ref vy, ref vz, ref isarc))
                                            {
                                                SWallSeg seg = new SWallSeg();//这里的swallseg只作记录直线段数据用
                                                seg.x1 = x1;
                                                seg.y1 = y1;
                                                seg.z1 = z1;
                                                seg.x2 = x2;
                                                seg.y2 = y2;
                                                seg.z2 = z2;
                                                segments.Add(seg);
                                                //
                                                SPoint apt = new SPoint();
                                                apt.x = x1;
                                                apt.y = y1;
                                                apt.z = z1;
                                                polylinepts.Add(apt);

                                            }
                                        }

                                    }
                                    break;
                                }
                            }
                        }
                        if (bottom > 0)
                        {
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            //形成底面
                            axEWdraw1.Clear3DPtBuf();
                            ProcSevenWinPolyline(cornertype, 12, ref polylinepts);
                            for (int k = 0; k < polylinepts.Count; k++)
                            {
                                axEWdraw1.AddOne3DVertex(((SPoint)polylinepts[k]).x, ((SPoint)polylinepts[k]).y, ((SPoint)polylinepts[k]).z);
                            }
                            int bpolyline = axEWdraw1.PolyLine3D(true);
                            axEWdraw1.Clear3DPtBuf();
                            polylinepts.Clear();
                            axEWdraw1.GetEntBoundingBox(bpolyline, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            axEWdraw1.MoveTo(bpolyline, new object[] { minz, miny, minz }, new object[] { minz, miny, iz - thickness });
                            int bface = axEWdraw1.EntToFace(bpolyline, true);
                            int bent = axEWdraw1.Prism(bface, thickness, new object[] { 0, 0, 1 });
                            axEWdraw1.Delete(bface);
                            //顶面
                            int tent = axEWdraw1.Copy(bent, new object[] { minz, miny, iz - thickness }, new object[] { minz, miny, iz + height });


                            axEWdraw1.SetEntColor(bent, axEWdraw1.RGBToIndex(255, 255, 255));
                            axEWdraw1.SetEntColor(tent, axEWdraw1.RGBToIndex(255, 255, 255));

                            //导入3DS文件
                            int impsize = 0;
                            int ent0 = 0;
                            string[] namearr = grpname.Split('_');
                            string orggrpname = "";
                            if (namearr[0] == "sevenwin")
                            {//判断类型
                                orggrpname = "sevenwin" + "_" + "org" + "_" + namearr[1];
                            }//else ...etc. 也可以在些增窗户或其它的判断
                            axEWdraw1.ClearIDBuffer();
                            int orgsevenwin = 0;
                            int orggrp = IsExistGroup(orggrpname);
                            if (orggrp <= 0)
                            {
                                impsize = axEWdraw1.Imp3DSWithTexture(filePath);
                                //创建组
                                for (int i = 1; i <= impsize; i++)
                                {
                                    ent0 = axEWdraw1.GetImpEntID(i);
                                    axEWdraw1.AddIDToBuffer(ent0);
                                }
                                //
                                orgsevenwin = axEWdraw1.MakeGroup(orggrpname, new object[] { 0, 0, 0 });

                            }
                            else
                                orgsevenwin = orggrp;
                            axEWdraw1.ClearIDBuffer();
                            //竖向外窗
                            int vsevenwinsub = axEWdraw1.Copy(orgsevenwin, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
                            axEWdraw1.GetEntBoundingBox(vsevenwinsub, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            axEWdraw1.MoveTo(vsevenwinsub, new object[] { minx, miny, minz }, new object[] { 0, 0, 0 });
                            axEWdraw1.SetGroupInsPt(vsevenwinsub, new object[] { 0, 0, 0 });
                            //竖向外窗与墙相连部份
                            int vsevenwinsub_cwall = axEWdraw1.Copy(orgsevenwin, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
                            axEWdraw1.GetEntBoundingBox(vsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            axEWdraw1.MoveTo(vsevenwinsub_cwall, new object[] { minx, miny, minz }, new object[] { 0, 0, 0 });
                            axEWdraw1.SetGroupInsPt(vsevenwinsub_cwall, new object[] { 0, 0, 0 });

                            //横向外窗
                            int hsevenwinsub = axEWdraw1.Copy(orgsevenwin, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
                            axEWdraw1.GetEntBoundingBox(hsevenwinsub, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            axEWdraw1.MoveTo(hsevenwinsub, new object[] { minx, miny, minz }, new object[] { 0, 0, 0 });
                            axEWdraw1.SetGroupInsPt(hsevenwinsub, new object[] { 0, 0, 0 });

                            //横向外窗与墙相连部份
                            int hsevenwinsub_cwall = axEWdraw1.Copy(orgsevenwin, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
                            axEWdraw1.GetEntBoundingBox(hsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            axEWdraw1.MoveTo(hsevenwinsub_cwall, new object[] { minx, miny, minz }, new object[] { 0, 0, 0 });
                            axEWdraw1.SetGroupInsPt(hsevenwinsub_cwall, new object[] { 0, 0, 0 });

                            //计算竖向外窗的宽度
                            double vwidth = axEWdraw1.PointDistance(((SWallSeg)segments[3]).x1, ((SWallSeg)segments[3]).y1, ((SWallSeg)segments[3]).z1, ((SWallSeg)segments[3]).x2, ((SWallSeg)segments[3]).y2, ((SWallSeg)segments[3]).z2);
                            axEWdraw1.MeshScaleByXYZAxis3(vsevenwinsub, vwidth / (maxx - minx), 1.0, height / (maxz - minz));
                            //竖向与墙连部分的宽度
                            double vwidth_cwall = axEWdraw1.PointDistance(((SWallSeg)segments[2]).x1, ((SWallSeg)segments[2]).y1, ((SWallSeg)segments[2]).z1, ((SWallSeg)segments[2]).x2, ((SWallSeg)segments[2]).y2, ((SWallSeg)segments[2]).z2);
                            axEWdraw1.MeshScaleByXYZAxis3(vsevenwinsub_cwall, (vwidth_cwall - thickness) / (maxx - minx), 1.0, height / (maxz - minz));


                            //计算横向外窗的宽度
                            double hwidth = axEWdraw1.PointDistance(((SWallSeg)segments[4]).x1, ((SWallSeg)segments[4]).y1, ((SWallSeg)segments[4]).z1, ((SWallSeg)segments[4]).x2, ((SWallSeg)segments[4]).y2, ((SWallSeg)segments[4]).z2);
                            axEWdraw1.MeshScaleByXYZAxis3(hsevenwinsub, hwidth / (maxx - minx), 1.0, height / (maxz - minz));
                            //横向与墙连部分的宽度
                            double hwidth_cwall = axEWdraw1.PointDistance(((SWallSeg)segments[5]).x1, ((SWallSeg)segments[5]).y1, ((SWallSeg)segments[5]).z1, ((SWallSeg)segments[5]).x2, ((SWallSeg)segments[5]).y2, ((SWallSeg)segments[5]).z2);
                            axEWdraw1.MeshScaleByXYZAxis3(hsevenwinsub_cwall, (hwidth_cwall - thickness) / (maxx - minx), 1.0, height / (maxz - minz));
                            switch (cornertype)
                            {
                                case 1:
                                    {//左下
                                        //竖向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                                                        new object[] { ((SWallSeg)segments[3]).x1, ((SWallSeg)segments[3]).y1, iz }, new object[] { -1, 0, 0 }, new object[] { 0, -1, 0 });

                                        //竖向与墙边接
                                        axEWdraw1.GetEntBoundingBox(vsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub_cwall, new object[] { maxx, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { -1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[2]).x2, ((SWallSeg)segments[2]).y2, iz }, new object[] { 0, 1, 0 }, new object[] { -1, 0, 0 });

                                        //横向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                           new object[] { ((SWallSeg)segments[4]).x1, ((SWallSeg)segments[4]).y1, iz }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 });

                                        //横向与墙边接
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub_cwall, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[5]).x1, ((SWallSeg)segments[5]).y1, iz }, new object[] { 1, 0, 0 }, new object[] { 0, 1, 0 });
                                    }
                                    break;
                                case 2:
                                    {//左上
                                        //竖向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                                                        new object[] { ((SWallSeg)segments[3]).x2, ((SWallSeg)segments[3]).y2, iz }, new object[] { -1, 0, 0 }, new object[] { 0, 1, 0 });

                                        //竖向与墙边接

                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub_cwall, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { -1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[2]).x2, ((SWallSeg)segments[2]).y2, iz }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 });

                                        //横向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                           new object[] { ((SWallSeg)segments[4]).x2, ((SWallSeg)segments[4]).y2, iz }, new object[] { 0, 1, 0 }, new object[] { -1, 0, 0 });

                                        //横向与墙边接
                                        axEWdraw1.GetEntBoundingBox(hsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub_cwall, new object[] { maxx, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[5]).x1, ((SWallSeg)segments[5]).y1, iz }, new object[] { 1, 0, 0 }, new object[] { 0, -1, 0 });

                                    }
                                    break;
                                case 3:
                                    {//右上
                                        //竖向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                                                        new object[] { ((SWallSeg)segments[3]).x1, ((SWallSeg)segments[3]).y1, iz }, new object[] { 1, 0, 0 }, new object[] { 0, 1, 0 });

                                        //竖向与墙边接
                                        axEWdraw1.GetEntBoundingBox(hsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub_cwall, new object[] { maxx, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { -1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[2]).x2, ((SWallSeg)segments[2]).y2, iz }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 });

                                        //横向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                           new object[] { ((SWallSeg)segments[4]).x1, ((SWallSeg)segments[4]).y1, iz }, new object[] { 0, 1, 0 }, new object[] { -1, 0, 0 });

                                        //横向与墙边接
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub_cwall, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                            new object[] { ((SWallSeg)segments[5]).x1, ((SWallSeg)segments[5]).y1, iz }, new object[] { -1, 0, 0 }, new object[] { 0, -1, 0 });

                                    }
                                    break;
                                case 4:
                                    {//右下
                                        //竖向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                                                        new object[] { ((SWallSeg)segments[3]).x2, ((SWallSeg)segments[3]).y2, iz }, new object[] { 1, 0, 0 }, new object[] { 0, -1, 0 });

                                        ////竖向与墙边接
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(vsevenwinsub_cwall, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { -1, 0, 0 },
                                        new object[] { ((SWallSeg)segments[2]).x2, ((SWallSeg)segments[2]).y2, iz }, new object[] { 0, 1, 0 }, new object[] { -1, 0, 0 });

                                        ////横向
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub, new object[] { 0, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                           new object[] { ((SWallSeg)segments[4]).x2, ((SWallSeg)segments[4]).y2, iz }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 });

                                        ////横向与墙边接
                                        axEWdraw1.GetEntBoundingBox(hsevenwinsub_cwall, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        axEWdraw1.Ax3TrasfWithZAsYAxis(hsevenwinsub_cwall, new object[] { maxx, 0, 0 }, new object[] { 0, -1, 0 }, new object[] { 1, 0, 0 },
                                            new object[] { ((SWallSeg)segments[5]).x1, ((SWallSeg)segments[5]).y1, iz }, new object[] { -1, 0, 0 }, new object[] { 0, -1, 0 });

                                    }
                                    break;
                            }
                            //
                            //重新创建组
                            axEWdraw1.ClearIDBuffer();
                            axEWdraw1.Explode(hsevenwinsub);
                            int expldsize = axEWdraw1.GetExplodeIDBufferSize();
                            for (int i = 0; i < expldsize; i++)
                            {
                                tmpent = axEWdraw1.GetExplodeIDBuffer(i);
                                axEWdraw1.AddIDToBuffer(tmpent);
                            }

                            axEWdraw1.Explode(vsevenwinsub);
                            expldsize = axEWdraw1.GetExplodeIDBufferSize();
                            for (int i = 0; i < expldsize; i++)
                            {
                                tmpent = axEWdraw1.GetExplodeIDBuffer(i);
                                axEWdraw1.AddIDToBuffer(tmpent);
                            }

                            axEWdraw1.Explode(vsevenwinsub_cwall);
                            expldsize = axEWdraw1.GetExplodeIDBufferSize();
                            for (int i = 0; i < expldsize; i++)
                            {
                                tmpent = axEWdraw1.GetExplodeIDBuffer(i);
                                axEWdraw1.AddIDToBuffer(tmpent);
                            }

                            axEWdraw1.Explode(hsevenwinsub_cwall);
                            expldsize = axEWdraw1.GetExplodeIDBufferSize();
                            for (int i = 0; i < expldsize; i++)
                            {
                                tmpent = axEWdraw1.GetExplodeIDBuffer(i);
                                axEWdraw1.AddIDToBuffer(tmpent);
                            }

                            axEWdraw1.AddIDToBuffer(tent);
                            axEWdraw1.AddIDToBuffer(bent);
                            int grpid = axEWdraw1.MakeGroup(orggrpname, new object[] { ix, iy, iz });
                            segments.Clear();
                            axEWdraw1.SetEntityInvisible(orgsevenwin, true);
                            return grpid;
                        }
                    }
                }
            }
            return 0;
        }
        //处理7字窗符号中的线,包括墙外线与窗中线
        private void ProcSevenWinSymbolLine(int cornertype, double depth, double thickness, int spacenum, ref ArrayList sevenpolyline, ref ArrayList lineids)
        {
            ArrayList outpolyline = new ArrayList();
            double dist = Math.Sqrt(thickness * thickness * 2.0);
            double space = thickness / spacenum;
            double x, y, z;
            x = y = z = 0;
            int ent = 0;
            switch (cornertype)
            {
                case 1:
                    {//左下
                        SPoint pt1 = new SPoint();
                        pt1.x = ((SPoint)sevenpolyline[0]).x;
                        pt1.y = ((SPoint)sevenpolyline[0]).y - thickness;
                        pt1.z = ((SPoint)sevenpolyline[0]).z;
                        outpolyline.Add(pt1);

                        SPoint pt2 = new SPoint();
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[1]).x, ((SPoint)sevenpolyline[1]).y, ((SPoint)sevenpolyline[1]).z },
                        new object[] { -1, -1, 0 }, dist, ref x, ref y, ref z);
                        pt2.x = x;
                        pt2.y = y;
                        pt2.z = z;
                        outpolyline.Add(pt2);

                        SPoint pt3 = new SPoint();
                        pt3.x = ((SPoint)sevenpolyline[2]).x - thickness;
                        pt3.y = ((SPoint)sevenpolyline[2]).y;
                        pt3.z = ((SPoint)sevenpolyline[2]).z;
                        outpolyline.Add(pt1);
                        //绘制外墙线
                        axEWdraw1.Clear3DPtBuf();
                        axEWdraw1.AddOne3DVertex(pt1.x, pt1.y, pt1.z);
                        axEWdraw1.AddOne3DVertex(pt2.x, pt2.y, pt2.z);
                        axEWdraw1.AddOne3DVertex(pt3.x, pt3.y, pt3.z);
                        ent = axEWdraw1.PolyLine3D(false);
                        lineids.Add(ent);
                        outpolyline.Clear();
                        axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                        axEWdraw1.SetEntLineWidth(ent, 2);
                        axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                        //处理中线
                        double spdst = Math.Sqrt(space * space * 2.0);
                        for (int i = 1; i < 4; i++)
                        {

                            ArrayList midpolyline = new ArrayList();
                            //
                            SPoint mpt1 = new SPoint();
                            mpt1.x = pt1.x - space * i;
                            mpt1.y = pt1.y;
                            mpt1.z = pt1.z;
                            outpolyline.Add(mpt1);
                            //
                            SPoint mpt2 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[5]).x, ((SPoint)sevenpolyline[5]).y, ((SPoint)sevenpolyline[5]).z },
                            new object[] { -1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt2.x = x;
                            mpt2.y = y;
                            mpt2.z = z;
                            outpolyline.Add(mpt2);
                            //
                            SPoint mpt3 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[4]).x, ((SPoint)sevenpolyline[4]).y, ((SPoint)sevenpolyline[4]).z },
                            new object[] { 1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt3.x = x;
                            mpt3.y = y;
                            mpt3.z = z;
                            outpolyline.Add(mpt3);
                            //
                            SPoint mpt4 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[3]).x, ((SPoint)sevenpolyline[3]).y, ((SPoint)sevenpolyline[3]).z },
                            new object[] { 1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt4.x = x;
                            mpt4.y = y;
                            mpt4.z = z;
                            outpolyline.Add(mpt4);
                            //
                            SPoint mpt5 = new SPoint();
                            mpt5.x = pt3.x;
                            mpt5.y = pt3.y - space * i;
                            mpt5.z = pt3.z;
                            outpolyline.Add(mpt5);
                            axEWdraw1.Clear3DPtBuf();
                            for (int j = 0; j < outpolyline.Count; j++)
                            {
                                axEWdraw1.AddOne3DVertex(((SPoint)outpolyline[j]).x, ((SPoint)outpolyline[j]).y, ((SPoint)outpolyline[j]).z);
                            }
                            ent = axEWdraw1.PolyLine3D(false);
                            lineids.Add(ent);
                            axEWdraw1.SetEntLineWidth(ent, 2);
                            if (i == 3)
                                axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                            axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                            outpolyline.Clear();
                        }
                    }
                    break;
                case 2:
                    {//左上
                        SPoint pt1 = new SPoint();
                        pt1.x = ((SPoint)sevenpolyline[0]).x;
                        pt1.y = ((SPoint)sevenpolyline[0]).y + thickness;
                        pt1.z = ((SPoint)sevenpolyline[0]).z;
                        outpolyline.Add(pt1);

                        SPoint pt2 = new SPoint();
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[1]).x, ((SPoint)sevenpolyline[1]).y, ((SPoint)sevenpolyline[1]).z },
                        new object[] { -1, 1, 0 }, dist, ref x, ref y, ref z);
                        pt2.x = x;
                        pt2.y = y;
                        pt2.z = z;
                        outpolyline.Add(pt2);

                        SPoint pt3 = new SPoint();
                        pt3.x = ((SPoint)sevenpolyline[2]).x - thickness;
                        pt3.y = ((SPoint)sevenpolyline[2]).y;
                        pt3.z = ((SPoint)sevenpolyline[2]).z;
                        outpolyline.Add(pt1);
                        //绘制外墙线
                        axEWdraw1.Clear3DPtBuf();
                        axEWdraw1.AddOne3DVertex(pt1.x, pt1.y, pt1.z);
                        axEWdraw1.AddOne3DVertex(pt2.x, pt2.y, pt2.z);
                        axEWdraw1.AddOne3DVertex(pt3.x, pt3.y, pt3.z);
                        ent = axEWdraw1.PolyLine3D(false);
                        lineids.Add(ent);
                        outpolyline.Clear();
                        axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                        axEWdraw1.SetEntLineWidth(ent, 2);
                        axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                        //处理中线
                        double spdst = Math.Sqrt(space * space * 2.0);
                        for (int i = 1; i < 4; i++)
                        {

                            ArrayList midpolyline = new ArrayList();
                            //
                            SPoint mpt1 = new SPoint();
                            mpt1.x = pt1.x - space * i;
                            mpt1.y = pt1.y;
                            mpt1.z = pt1.z;
                            outpolyline.Add(mpt1);
                            //
                            SPoint mpt2 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[5]).x, ((SPoint)sevenpolyline[5]).y, ((SPoint)sevenpolyline[5]).z },
                            new object[] { -1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt2.x = x;
                            mpt2.y = y;
                            mpt2.z = z;
                            outpolyline.Add(mpt2);
                            //
                            SPoint mpt3 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[4]).x, ((SPoint)sevenpolyline[4]).y, ((SPoint)sevenpolyline[4]).z },
                            new object[] { 1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt3.x = x;
                            mpt3.y = y;
                            mpt3.z = z;
                            outpolyline.Add(mpt3);
                            //
                            SPoint mpt4 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[3]).x, ((SPoint)sevenpolyline[3]).y, ((SPoint)sevenpolyline[3]).z },
                            new object[] { 1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt4.x = x;
                            mpt4.y = y;
                            mpt4.z = z;
                            outpolyline.Add(mpt4);
                            //
                            SPoint mpt5 = new SPoint();
                            mpt5.x = pt3.x;
                            mpt5.y = pt3.y + space * i;
                            mpt5.z = pt3.z;
                            outpolyline.Add(mpt5);
                            axEWdraw1.Clear3DPtBuf();
                            for (int j = 0; j < outpolyline.Count; j++)
                            {
                                axEWdraw1.AddOne3DVertex(((SPoint)outpolyline[j]).x, ((SPoint)outpolyline[j]).y, ((SPoint)outpolyline[j]).z);
                            }
                            ent = axEWdraw1.PolyLine3D(false);
                            lineids.Add(ent);
                            axEWdraw1.SetEntLineWidth(ent, 2);
                            if (i == 3)
                                axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                            axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                            outpolyline.Clear();
                        }
                    }
                    break;
                case 3:
                    {//右上
                        SPoint pt1 = new SPoint();
                        pt1.x = ((SPoint)sevenpolyline[0]).x;
                        pt1.y = ((SPoint)sevenpolyline[0]).y + thickness;
                        pt1.z = ((SPoint)sevenpolyline[0]).z;
                        outpolyline.Add(pt1);

                        SPoint pt2 = new SPoint();
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[1]).x, ((SPoint)sevenpolyline[1]).y, ((SPoint)sevenpolyline[1]).z },
                        new object[] { 1, 1, 0 }, dist, ref x, ref y, ref z);
                        pt2.x = x;
                        pt2.y = y;
                        pt2.z = z;
                        outpolyline.Add(pt2);

                        SPoint pt3 = new SPoint();
                        pt3.x = ((SPoint)sevenpolyline[2]).x + thickness;
                        pt3.y = ((SPoint)sevenpolyline[2]).y;
                        pt3.z = ((SPoint)sevenpolyline[2]).z;
                        outpolyline.Add(pt1);
                        //绘制外墙线
                        axEWdraw1.Clear3DPtBuf();
                        axEWdraw1.AddOne3DVertex(pt1.x, pt1.y, pt1.z);
                        axEWdraw1.AddOne3DVertex(pt2.x, pt2.y, pt2.z);
                        axEWdraw1.AddOne3DVertex(pt3.x, pt3.y, pt3.z);
                        ent = axEWdraw1.PolyLine3D(false);
                        lineids.Add(ent);
                        outpolyline.Clear();
                        axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                        axEWdraw1.SetEntLineWidth(ent, 2);
                        axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                        //处理中线
                        double spdst = Math.Sqrt(space * space * 2.0);
                        for (int i = 1; i < 4; i++)
                        {
                            ArrayList midpolyline = new ArrayList();
                            //
                            SPoint mpt1 = new SPoint();
                            mpt1.x = pt1.x + space * i;
                            mpt1.y = pt1.y;
                            mpt1.z = pt1.z;
                            outpolyline.Add(mpt1);
                            //
                            SPoint mpt2 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[5]).x, ((SPoint)sevenpolyline[5]).y, ((SPoint)sevenpolyline[5]).z },
                            new object[] { 1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt2.x = x;
                            mpt2.y = y;
                            mpt2.z = z;
                            outpolyline.Add(mpt2);
                            //
                            SPoint mpt3 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[4]).x, ((SPoint)sevenpolyline[4]).y, ((SPoint)sevenpolyline[4]).z },
                            new object[] { -1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt3.x = x;
                            mpt3.y = y;
                            mpt3.z = z;
                            outpolyline.Add(mpt3);
                            //
                            SPoint mpt4 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[3]).x, ((SPoint)sevenpolyline[3]).y, ((SPoint)sevenpolyline[3]).z },
                            new object[] { -1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt4.x = x;
                            mpt4.y = y;
                            mpt4.z = z;
                            outpolyline.Add(mpt4);
                            //
                            SPoint mpt5 = new SPoint();
                            mpt5.x = pt3.x;
                            mpt5.y = pt3.y + space * i;
                            mpt5.z = pt3.z;
                            outpolyline.Add(mpt5);
                            axEWdraw1.Clear3DPtBuf();
                            for (int j = 0; j < outpolyline.Count; j++)
                            {
                                axEWdraw1.AddOne3DVertex(((SPoint)outpolyline[j]).x, ((SPoint)outpolyline[j]).y, ((SPoint)outpolyline[j]).z);
                            }
                            ent = axEWdraw1.PolyLine3D(false);
                            lineids.Add(ent);
                            axEWdraw1.SetEntLineWidth(ent, 2);
                            if (i == 3)
                                axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                            axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                            outpolyline.Clear();
                        }
                    }
                    break;
                case 4:
                    {//右下
                        SPoint pt1 = new SPoint();
                        pt1.x = ((SPoint)sevenpolyline[0]).x;
                        pt1.y = ((SPoint)sevenpolyline[0]).y - thickness;
                        pt1.z = ((SPoint)sevenpolyline[0]).z;
                        outpolyline.Add(pt1);

                        SPoint pt2 = new SPoint();
                        axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[1]).x, ((SPoint)sevenpolyline[1]).y, ((SPoint)sevenpolyline[1]).z },
                        new object[] { 1, -1, 0 }, dist, ref x, ref y, ref z);
                        pt2.x = x;
                        pt2.y = y;
                        pt2.z = z;
                        outpolyline.Add(pt2);

                        SPoint pt3 = new SPoint();
                        pt3.x = ((SPoint)sevenpolyline[2]).x + thickness;
                        pt3.y = ((SPoint)sevenpolyline[2]).y;
                        pt3.z = ((SPoint)sevenpolyline[2]).z;
                        outpolyline.Add(pt1);
                        //绘制外墙线
                        axEWdraw1.Clear3DPtBuf();
                        axEWdraw1.AddOne3DVertex(pt1.x, pt1.y, pt1.z);
                        axEWdraw1.AddOne3DVertex(pt2.x, pt2.y, pt2.z);
                        axEWdraw1.AddOne3DVertex(pt3.x, pt3.y, pt3.z);
                        ent = axEWdraw1.PolyLine3D(false);
                        lineids.Add(ent);
                        outpolyline.Clear();
                        axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                        axEWdraw1.SetEntLineWidth(ent, 2);
                        axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                        //处理中线
                        double spdst = Math.Sqrt(space * space * 2.0);
                        for (int i = 1; i < 4; i++)
                        {
                            ArrayList midpolyline = new ArrayList();
                            //
                            SPoint mpt1 = new SPoint();
                            mpt1.x = pt1.x + space * i;
                            mpt1.y = pt1.y;
                            mpt1.z = pt1.z;
                            outpolyline.Add(mpt1);
                            //
                            SPoint mpt2 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[5]).x, ((SPoint)sevenpolyline[5]).y, ((SPoint)sevenpolyline[5]).z },
                            new object[] { 1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt2.x = x;
                            mpt2.y = y;
                            mpt2.z = z;
                            outpolyline.Add(mpt2);
                            //
                            SPoint mpt3 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[4]).x, ((SPoint)sevenpolyline[4]).y, ((SPoint)sevenpolyline[4]).z },
                            new object[] { -1, 1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt3.x = x;
                            mpt3.y = y;
                            mpt3.z = z;
                            outpolyline.Add(mpt3);
                            //
                            SPoint mpt4 = new SPoint();
                            axEWdraw1.Polar(new object[] { ((SPoint)sevenpolyline[3]).x, ((SPoint)sevenpolyline[3]).y, ((SPoint)sevenpolyline[3]).z },
                            new object[] { -1, -1, 0 }, spdst * i, ref x, ref y, ref z);
                            mpt4.x = x;
                            mpt4.y = y;
                            mpt4.z = z;
                            outpolyline.Add(mpt4);
                            //
                            SPoint mpt5 = new SPoint();
                            mpt5.x = pt3.x;
                            mpt5.y = pt3.y - space * i;
                            mpt5.z = pt3.z;
                            outpolyline.Add(mpt5);
                            axEWdraw1.Clear3DPtBuf();
                            for (int j = 0; j < outpolyline.Count; j++)
                            {
                                axEWdraw1.AddOne3DVertex(((SPoint)outpolyline[j]).x, ((SPoint)outpolyline[j]).y, ((SPoint)outpolyline[j]).z);
                            }
                            ent = axEWdraw1.PolyLine3D(false);
                            lineids.Add(ent);
                            axEWdraw1.SetEntLineWidth(ent, 2);
                            if (i == 3)
                                axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(0, 0, 0));
                            axEWdraw1.MoveTo(ent, new object[] { 0, 0, 0 }, new object[] { 0, 0, 3000 });
                            outpolyline.Clear();
                        }
                    }
                    break;
            }
        }
        //绘制七型飘窗
        private void DrawSevenWin()
        {
            canceldrawwalljs = -1;
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            listView3.SelectedItems.Clear();
            //
            double x, y, z, ix, iy, iz, fx, fy, fz;
            double hwidth, vwidth, height;
            int id1, id2;
            id1 = id2 = 0;
            x = y = z = ix = iy = iz = fx = fy = fz = 0;
            hwidth = vwidth = height = 0;
            hwidth = 2400;//横向宽度
            vwidth = 1200;//纵向宽度
            isseloneent = true;
            axEWdraw1.CancelCommand();
            label1.Text = "选择要摆放七字型飘窗的墙的拐角处:";
            int ent1 = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            if (!axEWdraw1.IsGroup(ent1))
            {//判断是否是组
                axEWdraw1.ClearSelected();
                int hneg, vneg;
                hneg = vneg = 0;
                if (GetCornerByPt(x, y, z, 128, ref hneg, ref vneg, ref ix, ref iy, ref iz, ref fx, ref fy, ref fz, ref id1, ref id2))
                {
                    int grpid = MakeSevenWinSymbol(ix, iy, iz, hwidth * hneg, vwidth * vneg, 128, 1700, 800, 600, fx, fy, fz, id1, id2);
                }
                else MessageBox.Show("请选择相互垂直的墙面拐角处摆放七字型飘窗!");
            }
            isseloneent = false;
            label1.Text = "";
        }

        /*MakeArchriseHoleSymbol 在俯视下创建拱形墙洞的符号,以组为基础.也就是说一个组是一个符号.
         * 参数:
         * name      输入 门的名称(组名)
         * width     输入 宽度
         * thickness 输入 厚度
         * height    输入 高度
         * archrise  输入 拱高
         * ridz      输入 底部距地面的Z值
         * maxz      输入 最高z值,一定要高于墙高
         * 返回值:
         * 成功则返回符号组的ID,其它返回0
         */
        private int MakeArchriseHoleSymbol(string name, double width, double thickness, double height, double archrise, double ridz, double maxz, bool isoutwarddoor = false)
        {
            int rect = axEWdraw1.Rectangle(0, 0, width, thickness, 0);
            int rect1 = axEWdraw1.Rectangle(0, 0, width, thickness, maxz);
            axEWdraw1.SetEntColor(rect1, axEWdraw1.RGBToIndex(0, 0, 0));
            axEWdraw1.SetEntLineWidth(rect1, 2);
            axEWdraw1.SetEntLineType(rect1, 1);

            int frect = axEWdraw1.EntToFace(rect, false);
            int fhole = axEWdraw1.Prism(frect, maxz, new object[] { 0, 0, 1 });
            axEWdraw1.SetEntColor(fhole, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(fhole, 0.3);

            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(fhole);//组的第一个要是一个三维体,也就是合并且拉伸出来的那个面
            axEWdraw1.AddIDToBuffer(rect);
            axEWdraw1.AddIDToBuffer(rect1);
            //这里的名称的规律是非常重要的,symbol表示主是一个符号组,door表示这是门的大类(也可以是window门或其它大类),1表示大类中的一种,这是用"_"符号隔开
            int group = axEWdraw1.MakeGroup(name, new object[] { width / 2.0, thickness / 2.0, 0 });
            //设置组的数据
            string str = "width:" + width.ToString() + ";" + "thickness:" + thickness.ToString() + ";" + "height:" + height.ToString() + ";" + "archrise:" + archrise.ToString() + ";" + "ridz:" + ridz + ";" + "maxz:" + maxz + ";";
            axEWdraw1.SetEntityUserData(group, str);
            //
            if (!isoutwarddoor)
                axEWdraw1.SetGroupPlaneByBoxFace(group, 1);
            else
                axEWdraw1.SetGroupPlaneByBoxFace(group, 2);
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.Delete(frect);
            return group;
        }

        //绘制拱形墙洞 2017-10-27
        private void DrawWallArchriseHole()
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                isdrawabsorb = true;
                axEWdraw1.ClearSelected();//2016-09-08
                //这里的拱形墙洞的拱高450
                lastsym = MakeArchriseHoleSymbol("symbol_hole_2", 900, g_wallthickness, 2000, 450, 0, g_maxz, true);
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbHigh(false, 0);
                axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x, y, z;
            x = y = z = 0;
            label1.Text = "选择要漫游房间,其中的起点位置:";
            int ent = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            int enttype = axEWdraw1.GetEntType(ent);
            if (enttype == 50)
            {
                string str = axEWdraw1.GetEntityUserData(ent);
                if (str.IndexOf("area:") >= 0)
                {
                    g_innerx = x;
                    g_innery = y;
                    g_innerz = 1700;
                    g_isinnerorbit = true;
                    axEWdraw1.ClearSelected();
                    if (IsHaveRoom())
                    {
                        GlobalView();
                    }
                    else MessageBox.Show("请先绘制户型");
                }
            }
            label1.Text = "";
        }
        /*SetFurnitureWidth设置家具类宽度 2017-11-16
         * ent 要设置家具对象的ID(一个对象的XYZ轴方向是由创建对象组时所选择的吸附面决定的)
         */
        private void SetFurnitureWidth(int ent)
        {
            if (axEWdraw1.IsGroup(ent))
            {//如果是组
                ArrayList relatewalls = new ArrayList();
                string grpname = axEWdraw1.GetGroupName(ent);
                if (grpname.IndexOf("symbol") < 0)
                {//不是门窗类
                    double minx, miny, minz, maxx, maxy, maxz;
                    minx = miny = minz = maxx = maxy = maxz = 0.0;
                    axEWdraw1.ClearSelected();
                    Form2 input = new Form2();
                    //取得原始的宽度
                    input.textBox1.Text = GetProStrFromEnt(ent, "xlength");
                    input.textBox2.Text = GetProStrFromEnt(ent, "ylength");
                    input.textBox3.Text = GetProStrFromEnt(ent, "zlength");
                    string oldstr1 = input.textBox1.Text;
                    string oldstr2 = input.textBox2.Text;
                    string oldstr3 = input.textBox3.Text;
                    //显示对话框修改宽度
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        //如果X轴有变化,则修改X轴方向的宽度
                        if (oldstr1 != input.textBox1.Text)
                        {
                            double xlength = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);
                            if (xlength > 1 && zPubFun.zPubFunLib.CStr2Double(oldstr1) > 1)
                            {
                                double xscale = xlength / zPubFun.zPubFunLib.CStr2Double(oldstr1);
                                if (axEWdraw1.FurnitureScaleByXYZAxis(ent, 0, xscale))
                                {
                                    SetProStrFromEnt(ent, "xlength", xlength);
                                }
                            }
                            else MessageBox.Show("过小");

                        }
                        //如果Y轴有变化,则修改Y轴方向的宽度
                        if (oldstr2 != input.textBox2.Text)
                        {
                            double ylength = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                            if (ylength > 1 && zPubFun.zPubFunLib.CStr2Double(oldstr2) > 1)
                            {
                                double yscale = ylength / zPubFun.zPubFunLib.CStr2Double(oldstr2);
                                axEWdraw1.FurnitureScaleByXYZAxis(ent, 1, yscale);
                                SetProStrFromEnt(ent, "ylength", ylength);
                            }
                            else MessageBox.Show("过小");
                        }
                        //如果Z轴有变化,则修改Z轴方向的宽度
                        if (oldstr3 != input.textBox3.Text)
                        {
                            double zlength = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);
                            if (zlength > 1 && zPubFun.zPubFunLib.CStr2Double(oldstr3) > 1)
                            {

                                double zscale = zlength / zPubFun.zPubFunLib.CStr2Double(oldstr3);
                                axEWdraw1.FurnitureScaleByXYZAxis(ent, 2, zscale);
                                SetProStrFromEnt(ent, "zlength", zlength);
                            }
                            else MessageBox.Show("过小");
                        }
                    }
                }
                else MessageBox.Show("这不是一个家具类的对象");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            canceldrawwalljs = -1;//该行要放在cancel的后面.如果正在画墙,则结束 2017-03-14
            axEWdraw1.CancelCommand();
            //
            isseloneent = true;
            double x, y, z;
            x = y = z = 0.0;
            int ent = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                SetFurnitureWidth(ent);
            }
            isseloneent = false;
        }

        /*SetSymbolWidth 设置门窗符号的宽,高参数.2017-11-22
         * ent           为要设置参数的门窗类对象
         */
        private void SetSymbolWidth(int ent)
        {
            if (axEWdraw1.IsGroup(ent))
            {//如果是组
                ArrayList relatewalls = new ArrayList();
                string grpname = axEWdraw1.GetGroupName(ent);
                if (grpname.IndexOf("symbol_door") >= 0)
                {//是门的组
                    double x, y, z;
                    x = y = z = 0.0;
                    double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                    axEWdraw1.GetGroupInsPt(ent, ref x, ref y, ref z);
                    Form3 input = new Form3();
                    input.textBox1.Text = width.ToString();
                    input.textBox2.Text = height.ToString();
                    input.textBox3.Text = z.ToString();
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        width = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);
                        height = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                        z = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);
                        if (width >= 100 && width <= 2500 && height >= 100 && height <= 2500)
                        {

                            //取得与这个符号相关的墙的ID
                            axEWdraw1.ClearIDBuffer();
                            axEWdraw1.GetWallFromSymbol(ent);
                            int idsize = axEWdraw1.GetIDBufferSize();
                            for (int i = 0; i < idsize; i++)
                            {
                                relatewalls.Add(axEWdraw1.GetIDBuffer(i));
                            }
                            axEWdraw1.ClearIDBuffer();
                            //创建新的门符号
                            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz, orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                            int ngrp = MakeDoorSymbol(grpname, width, g_wallthickness, height, z, g_maxz, true);
                            axEWdraw1.SetGroupDepth(ngrp, g_wallthickness / 2.0);
                            axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                            axEWdraw1.GetGroupAxis(ngrp, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                            if (Math.Abs(orgz - z) > 0.001)
                                orgz = Math.Abs(z);
                            axEWdraw1.Ax3TrasfWithZAsYAxis(ngrp, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                            new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(false);
                            axEWdraw1.RemoveSymbolFromWall(ent);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.AddSymbolToSingleWall(ngrp, (int)relatewalls[i]);
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(true);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.UpdateSingleWall((int)relatewalls[i]);
                        }
                        else MessageBox.Show("参数输入错误");
                    }
                }
                else if (grpname.IndexOf("symbol_window") >= 0)
                {//是窗的组
                    double x, y, z;
                    x = y = z = 0.0;
                    double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                    axEWdraw1.GetGroupInsPt(ent, ref x, ref y, ref z);
                    Form3 input = new Form3();
                    input.textBox1.Text = width.ToString();
                    input.textBox2.Text = height.ToString();
                    input.textBox3.Text = z.ToString();
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        if (width >= 100 && width <= 2500 && height >= 100 && height <= 2500)
                        {
                            width = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);
                            height = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                            z = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);
                            //取得与这个符号相关的墙的ID
                            axEWdraw1.ClearIDBuffer();
                            axEWdraw1.GetWallFromSymbol(ent);
                            int idsize = axEWdraw1.GetIDBufferSize();
                            for (int i = 0; i < idsize; i++)
                            {
                                relatewalls.Add(axEWdraw1.GetIDBuffer(i));
                            }
                            axEWdraw1.ClearIDBuffer();
                            //创建新的门符号
                            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz, orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                            int ngrp = MakeWindowSymbol(grpname, width, g_wallthickness, height, z, g_maxz, true);
                            axEWdraw1.SetGroupDepth(ngrp, g_wallthickness / 2.0);
                            axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                            axEWdraw1.GetGroupAxis(ngrp, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                            if (Math.Abs(orgz - z) > 0.001)
                                orgz = Math.Abs(z);
                            axEWdraw1.Ax3TrasfWithZAsYAxis(ngrp, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                            new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(false);
                            axEWdraw1.RemoveSymbolFromWall(ent);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.AddSymbolToSingleWall(ngrp, (int)relatewalls[i]);
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(true);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.UpdateSingleWall((int)relatewalls[i]);
                        }
                        else MessageBox.Show("参数输入错误");
                    }
                }
                else if (grpname.IndexOf("symbol_hole") >= 0)
                {//是门的组
                    double x, y, z;
                    x = y = z = 0.0;
                    double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                    double archrise = GetDblfromProStr(GetProStrFromEnt(ent, "archrise"));
                    axEWdraw1.GetGroupInsPt(ent, ref x, ref y, ref z);
                    Form3 input = new Form3();
                    input.textBox1.Text = width.ToString();
                    input.textBox2.Text = height.ToString();
                    input.textBox3.Text = z.ToString();
                    if (Math.Abs(archrise) > 0.001)
                    {
                        input.textBox4.ReadOnly = false;
                        input.textBox4.Text = archrise.ToString();
                    }
                    else
                    {
                        input.textBox4.ReadOnly = true;
                    }
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        if (width >= 100 && width <= 2500 && height >= 100 && height <= 2500)
                        {
                            width = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);
                            height = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                            z = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);
                            archrise = zPubFun.zPubFunLib.CStr2Double(input.textBox4.Text);
                            //取得与这个符号相关的墙的ID
                            axEWdraw1.ClearIDBuffer();
                            axEWdraw1.GetWallFromSymbol(ent);
                            int idsize = axEWdraw1.GetIDBufferSize();
                            for (int i = 0; i < idsize; i++)
                            {
                                relatewalls.Add(axEWdraw1.GetIDBuffer(i));
                            }
                            axEWdraw1.ClearIDBuffer();
                            //创建新的门符号
                            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz, orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                            int ngrp = 0;
                            if (archrise > 0.1)
                                ngrp = MakeArchriseHoleSymbol("symbol_hole_2", width, g_wallthickness, height, archrise, 0, g_maxz, true);
                            else
                                ngrp = MakeHoleSymbol("symbol_hole_1", width, g_wallthickness, height, 0, g_maxz, true);
                            axEWdraw1.SetGroupDepth(ngrp, g_wallthickness / 2.0);
                            axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                            axEWdraw1.GetGroupAxis(ngrp, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                            if (Math.Abs(orgz - z) > 0.001)
                                orgz = Math.Abs(z);
                            axEWdraw1.Ax3TrasfWithZAsYAxis(ngrp, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                            new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(false);
                            axEWdraw1.RemoveSymbolFromWall(ent);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.AddSymbolToSingleWall(ngrp, (int)relatewalls[i]);
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(true);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.UpdateSingleWall((int)relatewalls[i]);
                        }
                        else MessageBox.Show("参数输入错误");
                    }
                }
                else if (grpname.IndexOf("symbol_baywin") >= 0)
                {//是门的组
                    double x, y, z;
                    x = y = z = 0.0;
                    double width = GetDblfromProStr(GetProStrFromEnt(ent, "width"));
                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                    double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                    axEWdraw1.GetGroupInsPt(ent, ref x, ref y, ref z);
                    Form3 input = new Form3();
                    input.label7.Text = "飘窗深度";
                    input.textBox1.Text = width.ToString();
                    input.textBox2.Text = height.ToString();
                    input.textBox3.Text = z.ToString();
                    if (Math.Abs(depth) > 0.001)
                    {
                        input.textBox4.ReadOnly = false;
                        input.textBox4.Text = depth.ToString();
                    }
                    else
                    {
                        input.textBox4.ReadOnly = true;
                    }
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        if (width >= 100 && width <= 2500 && height >= 100 && height <= 2500)
                        {
                            width = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);
                            height = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                            z = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);
                            depth = zPubFun.zPubFunLib.CStr2Double(input.textBox4.Text);
                            //取得与这个符号相关的墙的ID
                            axEWdraw1.ClearIDBuffer();
                            axEWdraw1.GetWallFromSymbol(ent);
                            int idsize = axEWdraw1.GetIDBufferSize();
                            for (int i = 0; i < idsize; i++)
                            {
                                relatewalls.Add(axEWdraw1.GetIDBuffer(i));
                            }
                            axEWdraw1.ClearIDBuffer();
                            //创建新的门符号
                            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz, orgx1, orgy1, orgz1, dx1, dy1, dz1, xdx1, xdy1, xdz1;
                            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = orgx1 = orgy1 = orgz1 = dx1 = dy1 = dz1 = xdx1 = xdy1 = xdz1 = 0.0;
                            int ngrp = MakeBayWinSymbol("symbol_baywin_1", width, g_wallthickness, depth, height, z, g_maxz, true);
                            axEWdraw1.SetGroupDepth(ngrp, g_wallthickness / 2.0);
                            axEWdraw1.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                            axEWdraw1.GetGroupAxis(ngrp, ref orgx1, ref orgy1, ref orgz1, ref dx1, ref dy1, ref dz1, ref xdx1, ref xdy1, ref xdz1);
                            if (Math.Abs(orgz - z) > 0.001)
                                orgz = Math.Abs(z);
                            axEWdraw1.Ax3TrasfWithZAsYAxis(ngrp, new object[] { orgx1, orgy1, orgz1 }, new object[] { dx1, dy1, dz1 }, new object[] { xdx1, xdy1, xdz1 },
                                                            new object[] { orgx, orgy, orgz }, new object[] { dx, dy, dz }, new object[] { xdx, xdy, xdz });
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(false);
                            axEWdraw1.RemoveSymbolFromWall(ent);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.AddSymbolToSingleWall(ngrp, (int)relatewalls[i]);
                            axEWdraw1.SetUpdateWallWhenRemoveSymbol(true);
                            for (int i = 0; i < relatewalls.Count; i++)
                                axEWdraw1.UpdateSingleWall((int)relatewalls[i]);
                        }
                        else MessageBox.Show("参数输入错误");
                    }
                }
                else if (grpname.IndexOf("symbol_sevenwin") >= 0)
                {//是门的组
                    double x1, y1, z1;
                    x1 = y1 = z1 = 0.0;
                    double hwidth = Math.Abs(GetDblfromProStr(GetProStrFromEnt(ent, "hwidth")));
                    double vwidth = Math.Abs(GetDblfromProStr(GetProStrFromEnt(ent, "vwidth")));
                    double height = GetDblfromProStr(GetProStrFromEnt(ent, "height"));
                    double depth = GetDblfromProStr(GetProStrFromEnt(ent, "depth"));
                    axEWdraw1.GetGroupInsPt(ent, ref x1, ref y1, ref z1);
                    Form4 input = new Form4();
                    input.textBox1.Text = hwidth.ToString();//2018-03-23
                    input.textBox3.Text = vwidth.ToString();//2018-03-23
                    input.textBox2.Text = height.ToString();
                    input.textBox4.Text = z1.ToString();
                    input.textBox5.Text = depth.ToString();

                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        hwidth = zPubFun.zPubFunLib.CStr2Double(input.textBox1.Text);//2018-03-23
                        vwidth = zPubFun.zPubFunLib.CStr2Double(input.textBox3.Text);//2018-03-23
                        height = zPubFun.zPubFunLib.CStr2Double(input.textBox2.Text);
                        z1 = zPubFun.zPubFunLib.CStr2Double(input.textBox4.Text);
                        depth = zPubFun.zPubFunLib.CStr2Double(input.textBox5.Text);
                        input.textBox5.Text = depth.ToString();

                        if (hwidth > g_wallthickness && vwidth > g_wallthickness && depth > g_wallthickness * 2.0)
                        {

                            double x, y, z, ix, iy, iz, fx, fy, fz;
                            int hneg, vneg, id1, id2;
                            x = y = z = ix = iy = iz = fx = fy = fz = 0.0;
                            hneg = vneg = id1 = id2 = 0;
                            axEWdraw1.GetGroupInsPt(ent, ref x, ref y, ref z);
                            if (GetCornerByPt(x, y, z, g_wallthickness, ref hneg, ref vneg, ref ix, ref iy, ref iz, ref fx, ref fy, ref fz, ref id1, ref id2))
                            {
                                axEWdraw1.SetUpdateWallWhenRemoveSymbol(false);
                                axEWdraw1.RemoveSymbolFromWall(ent);
                                axEWdraw1.SetUpdateWallWhenRemoveSymbol(true);
                                if (Math.Abs(z - z1) > 0.001)
                                    z = z1;
                                int grpid = MakeSevenWinSymbol(ix, iy, 0, hwidth * hneg, vwidth * vneg, g_wallthickness, height, depth, z, fx, fy, fz, id1, id2);
                            }
                            else MessageBox.Show("请选择相互垂直的墙面拐角处摆放七字型飘窗!");
                        }
                        else MessageBox.Show("参数输入错误");
                    }
                }//
            }
        }

        //DrawBalcony以画墙的方式自由绘制阳台 2017-11-27
        private void DrawBalcony()
        {
            if (!isdrawbalcony)
            {//如果当前不处在画墙的状态,则启动画墙 2017-03-17
                canceldrawwalljs = 0;//2017-03-14 双击右键退出
                isdrawpolyline = false;//2017-03-14 双击右键退出
                timer3.Enabled = false;//2017-03-14 双击右键退出
                //
                tmplist.Clear();//初始化
                tlist0.Clear();
                tlist1.Clear();
                tmpent = 0;//初始化
                tmppipe = 0;//初始化
                tmppipe1 = 0;//初始化 2016-09-18
                drawsegjs = 0;//初始化画线计数
                connecttype = 0;//初始化连接类型
                wantdelent = 0;//要删除的实体,用于连接起始与终止的实体
                connectent = 0;//初始化连接的实体ID
                edgeptjs = 0;//初始化边点计数
                if (!axEWdraw1.IsEndCommand())
                {
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }

                axEWdraw1.SetORTHO(true);
                axEWdraw1.EnableCommandOnLBDown(true);
                axEWdraw1.CancelCommand();
                axEWdraw1.EnableDrawPolylinePipe(true, g_wallthickness, g_wallheight);
                isdrawpolyline = true;
                isdrawbalcony = true;//自由绘制阳台
                g_state = InteractiveState.BeginDrawWall;
                axEWdraw1.SetDrawPolylineLenLimit(g_wallthickness);
                axEWdraw1.ToDrawPolyLine();
            }
        }
        /*GetBalconyBaseFromSolid 得到实体中阳台的基础数据 2017-11-29
         * id id号
         * str 字段名称
         * datas 返回的数据
         */
        private bool GetBalconyDatasFromSolid(int id, string str, ref ArrayList datas)
        {
            string orgstr;
            orgstr = axEWdraw1.GetEntityUserData(id);
            if (orgstr != "")
            {
                int ffinx = IsHaveStrField(orgstr, str);
                if (ffinx >= 0)
                {
                    int feinx = orgstr.IndexOf(";", ffinx);
                    int slen = str.Length + 1;
                    string substr = orgstr.Substring(ffinx + slen, feinx - (ffinx + slen));
                    string[] strarr = substr.Split(',');
                    for (int j = 0; j < strarr.Length; j++)
                    {
                        datas.Add(zPubFun.zPubFunLib.CStr2Double(strarr[j]));
                    }
                    if (datas.Count >= 1)
                        return true;
                }
                else return false;
            }
            return false;
        }

        /*得到现存阳台全部数据 2017-12-04
         * bent        [in]  阳台区域对象
         * abdata      [out] 阳台的参数
         * orgwintypes [out] 阳台各立面窗型参数
         * wallpts     [out] 阳台各立面点数据
        */
        private void GetBalconyAllData(int bent, ref Form5.CExistBalcony abdata,ref ArrayList orgwintypes,ref ArrayList wallpts)
        {
            string memstr = "";
            int ent, type;
            int ffinx = 0;
            ArrayList otherids = new ArrayList();
            ArrayList balconywalls = new ArrayList();//立面集
            ArrayList innerwalls = new ArrayList();
            ArrayList balconywintypes = new ArrayList();//域块各立面的窗型
            ent = type = 0;
            //判断输出变量是否是空值
            if (abdata == null)
            {
                abdata = new Form5.CExistBalcony();
            }
            if (orgwintypes == null)
                orgwintypes = new ArrayList();
            if (wallpts == null)
                wallpts = new ArrayList();
            //
            int entsize = axEWdraw1.GetEntSize();

            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                ffinx = IsHaveStrField(memstr, "balcony");

                if (type == 50 && ent != bent)
                {
                    memstr = axEWdraw1.GetEntityUserData(ent);
                    if (ffinx < 0)
                    {
                        ffinx = IsHaveStrField(memstr, "area");
                        if (ffinx >= 0)
                            otherids.Add(ent);
                    }
                }
            }
            double mx, my, mz, mx1, my1, mz1;
            mx = my = mz = mx1 = my1 = mz1 = 0.0;
            bool isallsametpe = true;
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            memstr = axEWdraw1.GetEntityUserData(bent);
            ffinx = IsHaveStrField(memstr, "balcony");
            if (ffinx >= 0)
            {//为阳台对象
                //分析阳台对象外墙立面
                //---构造墙段
                ArrayList tptslist = new ArrayList();
                GetPtsFromSolid(bent, ref tptslist);
                //分析参数
                balconywintypes.Clear();
                //得到基本数据
                ArrayList datas = new ArrayList();
                GetBalconyDatasFromSolid(bent, "balcony", ref datas);
                //基本阳台数据
                abdata.b_width = (double)datas[0];//宽
                abdata.b_height = (double)datas[1];//高
                abdata.b_theight = (double)datas[2];//封阳台高
                abdata.b_bheight = (double)datas[3];//离地高度
                abdata.b_btype = (int)((double)datas[4]);//阳台底部高度
                datas.Clear();
                //栏栅数据
                datas.Clear();
                abdata.isbarrier = false;
                if (GetBalconyDatasFromSolid(bent, "barrier", ref datas))
                {
                    abdata.ba_length = (double)datas[0];//长
                    abdata.ba_width = (double)datas[1];//宽
                    abdata.ba_num = (int)((double)datas[2]);//数量
                    abdata.isbarrier = true;
                }
                //栏杆数据
                datas.Clear();
                abdata.ishandrai = false;
                if (GetBalconyDatasFromSolid(bent, "handrail", ref datas))
                {
                    abdata.hr_bheight = (double)datas[0];//离地高
                    abdata.hr_width = (double)datas[1];//宽
                    abdata.hr_thickness = (double)datas[2];//厚
                    abdata.ishandrai = true;
                }
                //中数据
                datas.Clear();
                abdata.ismidpanel = false;
                if (GetBalconyDatasFromSolid(bent, "midpanel", ref datas))
                {
                    abdata.mp_updist = (double)datas[0];//上边距
                    abdata.mp_downdist = (double)datas[1];//下边距
                    abdata.mp_thickness = (double)datas[2];//厚
                    abdata.ismidpanel = true;
                }
                datas.Clear();
                //
                memstr = axEWdraw1.GetEntityUserData(bent);
                ffinx = IsHaveStrField(memstr, "allwintype");
                if (ffinx >= 0)
                {//所有立面窗型都相同
                    //窗型
                    Form5.CWinType awintype = new Form5.CWinType();
                    awintype.wintype = GetIntfromProStr(GetProStrFromEnt(bent, "allwintype"));
                    isallsametpe = true;
                    abdata.isallsame = true;
                    orgwintypes.Add(awintype);
                }
                else
                {//立面窗型不相同
                    //判断是否有wintype的数据
                    ffinx = IsHaveStrField(memstr, "wintype");
                    if (ffinx >= 0)
                    {
                        GetBalconyDatasFromSolid(bent, "wintype", ref datas);
                        int len1 = datas.Count / 4;
                        for (int k = 0; k < len1; k++)
                        {
                            /*声明一个SInxWall对象,记录段的方向与窗型
                             * 之所以是方向,是因为避免位移造成的不法匹配的问题
                             */
                            SInxWall apt = new SInxWall();
                            apt.x = (double)datas[k * 4];
                            apt.y = (double)datas[k * 4 + 1];
                            apt.z = (double)datas[k * 4 + 2];
                            apt.inx = (int)((double)datas[k * 4 + 3]);//该值为窗型
                            balconywintypes.Add(apt);
                        }
                        datas.Clear();
                        //
                        isallsametpe = false;
                        //计算该区域中心点的位置
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw1.GetEntBoundingBox(bent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        mx = (minx + maxx) / 2.0;
                        my = (miny + maxy) / 2.0;
                        mz = 0;
                    }

                }
                if (ptslist.Count > 0)
                {
                    int len = tptslist.Count / 3 - 1;
                    int segjs = 0;
                    bool isskipseg = false;
                    int skipseginx = -1;
                    for (int j = 0; j < len; j++)
                    {
                        BalconyWallSeg aseg = new BalconyWallSeg();
                        aseg.x1 = (double)tptslist[j * 3];
                        aseg.y1 = (double)tptslist[j * 3 + 1];
                        aseg.z1 = (double)tptslist[j * 3 + 2];
                        aseg.x2 = (double)tptslist[(j + 1) * 3];
                        aseg.y2 = (double)tptslist[(j + 1) * 3 + 1];
                        aseg.z2 = (double)tptslist[(j + 1) * 3 + 2];
                        aseg.id = bent;
                        if (!IsShareWall(ref otherids, ref aseg))
                        {//非内墙
                            Form5.CWinType awintype = new Form5.CWinType();
                            if (!isallsametpe)
                            {//立面窗型不一样
                                mx1 = (aseg.x1 + aseg.x2) / 2.0;
                                my1 = (aseg.y1 + aseg.y2) / 2.0;
                                vx = mx1 - mx;
                                vy = my1 - my;
                                vz = 0;
                                double ang = 0;
                                double minang = 0;
                                int mininx = -1;
                                //找到角度最接近的 2017-11-29
                                for (int k = 0; k < balconywintypes.Count; k++)
                                {
                                    //计算两矢量夹角,注意vectorangle返回的是弧角
                                    ang = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((SInxWall)balconywintypes[k]).x, ((SInxWall)balconywintypes[k]).y, ((SInxWall)balconywintypes[k]).z });
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
                                    aseg.wintype = ((SInxWall)balconywintypes[mininx]).inx;
                                }
                                awintype.wintype = aseg.wintype;
                                awintype.vx = vx;
                                awintype.vy = vy;
                                awintype.vz = 0;
                                orgwintypes.Add(awintype);
                            }
                            Form5.CWallSeg awallseg = new Form5.CWallSeg();
                            awallseg.x1 = aseg.x1;
                            awallseg.y1 = aseg.y1;
                            awallseg.z1 = 0;
                            awallseg.x2 = aseg.x2;
                            awallseg.y2 = aseg.y2;
                            awallseg.z2 = 0;
                            wallpts.Add(awallseg);
                            segjs++;
                        }
                    }
                }
                balconywalls.Clear();
                tptslist.Clear();
                innerwalls.Clear();
                balconywintypes.Clear();
            }
        }


        /*MakeBalcony 创建阳台 2017-11-29
         * ids [输出] 创建的阳台组对象集(可能不至一个阳台)
         */
        private void MakeBalcony(ref ArrayList ids)
        {
            int ent, type;
            int ffinx = 0;
            ent = type = 0;
            int entsize = axEWdraw1.GetEntSize();
            ArrayList balconywalls = new ArrayList();//立面集
            ArrayList balconyids = new ArrayList();
            ArrayList otherids = new ArrayList();
            ArrayList balconywintypes = new ArrayList();//域块各立面的窗型
            ArrayList innerwalls = new ArrayList();
            string memstr = "";
            //
            if (g_balconyhidewall == null)
                g_balconyhidewall = new ArrayList();
            if (g_balconyconnectpts == null)
                g_balconyconnectpts = new ArrayList();

            g_balconyconnectpts.Clear();
            g_balconyhidewall.Clear();
            axEWdraw1.ClearIDBuffer();
            //
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                type = axEWdraw1.GetEntType(ent);
                if (type == 50)
                {
                    memstr = axEWdraw1.GetEntityUserData(ent);
                    ffinx = IsHaveStrField(memstr, "balcony");
                    if (ffinx >= 0)
                    {//为阳台对象
                        balconyids.Add(ent);
                    }
                    else
                    {
                        ffinx = IsHaveStrField(memstr, "area");
                        if (ffinx >= 0)
                            otherids.Add(ent);

                    }
                }
            }
            double b_width, b_height, b_theight, b_bheight,
                ba_length, ba_width, hr_bheight, hr_width, hr_thickness,
                mp_updist, mp_downdist, mp_thickness;


            double mx, my, mz, mx1, my1, mz1;
            int b_btype = 0;
            int wintype = 0;
            int ba_num = 0;
            b_width = b_height = b_theight = b_bheight = ba_length = ba_width = hr_bheight = hr_width = hr_thickness = mp_updist = mp_downdist = mp_thickness = 0.0;
            mx = my = mz = mx1 = my1 = mz1 = 0.0;
            bool isallsametpe = true;
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            for (int i = 0; i < balconyids.Count; i++)
            {
                //分析阳台对象外墙立面
                //---构造墙段
                ArrayList tptslist = new ArrayList();
                GetPtsFromSolid((int)balconyids[i], ref tptslist);
                //分析参数
                balconywintypes.Clear();
                //得到基本数据
                ArrayList datas = new ArrayList();
                GetBalconyDatasFromSolid((int)balconyids[i], "balcony", ref datas);
                //基本阳台数据
                b_width = (double)datas[0];//宽
                b_height = (double)datas[1];//高
                b_theight = (double)datas[2];//封阳台高
                b_bheight = (double)datas[3];//离地高度
                b_btype = (int)((double)datas[4]);//阳台底部高度
                datas.Clear();
                //栏栅数据
                datas.Clear();
                bool isbarrier = false;
                if (GetBalconyDatasFromSolid((int)balconyids[i], "barrier", ref datas))
                {
                    ba_length = (double)datas[0];//长
                    ba_width = (double)datas[1];//宽
                    ba_num = (int)((double)datas[2]);//数量
                    isbarrier = true;
                }
                //栏杆数据
                datas.Clear();
                bool ishandrai = false;
                if (GetBalconyDatasFromSolid((int)balconyids[i], "handrail", ref datas))
                {
                    hr_bheight = (double)datas[0];//离地高
                    hr_width = (double)datas[1];//宽
                    hr_thickness = (double)datas[2];//厚
                    ishandrai = true;
                }
                //中数据
                datas.Clear();
                bool ismidpanel = false;
                if (GetBalconyDatasFromSolid((int)balconyids[i], "midpanel", ref datas))
                {
                    mp_updist = (double)datas[0];//上边距
                    mp_downdist = (double)datas[1];//下边距
                    mp_thickness = (double)datas[2];//厚
                    ismidpanel = true;
                }
                datas.Clear();
                //
                memstr = axEWdraw1.GetEntityUserData((int)balconyids[i]);
                ffinx = IsHaveStrField(memstr, "allwintype");
                if (ffinx >= 0)
                {//所有立面窗型都相同
                    //窗型
                    wintype = GetIntfromProStr(GetProStrFromEnt((int)balconyids[i], "allwintype"));
                    isallsametpe = true;

                }
                else
                {//立面窗型不相同
                    //判断是否有wintype的数据
                    ffinx = IsHaveStrField(memstr, "wintype");
                    if (ffinx >= 0)
                    {
                        GetBalconyDatasFromSolid((int)balconyids[i], "wintype", ref datas);
                        int len1 = datas.Count / 4;
                        for (int k = 0; k < len1; k++)
                        {
                            /*声明一个SInxWall对象,记录段的方向与窗型
                             * 之所以是方向,是因为避免位移造成的不法匹配的问题
                             */
                            SInxWall apt = new SInxWall();
                            apt.x = (double)datas[k * 4];
                            apt.y = (double)datas[k * 4 + 1];
                            apt.z = (double)datas[k * 4 + 2];
                            apt.inx = (int)((double)datas[k * 4 + 3]);//该值为窗型
                            balconywintypes.Add(apt);
                        }
                        datas.Clear();
                        //
                        isallsametpe = false;
                        //计算该区域中心点的位置
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw1.GetEntBoundingBox((int)balconyids[i], ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        mx = (minx + maxx) / 2.0;
                        my = (miny + maxy) / 2.0;
                        mz = 0;
                    }

                }
                if (ptslist.Count > 0)
                {
                    int len = tptslist.Count / 3 - 1;
                    for (int j = 0; j < len; j++)
                    {
                        BalconyWallSeg aseg = new BalconyWallSeg();
                        aseg.x1 = (double)tptslist[j * 3];
                        aseg.y1 = (double)tptslist[j * 3 + 1];
                        aseg.z1 = (double)tptslist[j * 3 + 2];
                        aseg.x2 = (double)tptslist[(j + 1) * 3];
                        aseg.y2 = (double)tptslist[(j + 1) * 3 + 1];
                        aseg.z2 = (double)tptslist[(j + 1) * 3 + 2];
                        aseg.id = (int)balconyids[i];
                        //基本阳台数据
                        aseg.b_width = b_width;//宽
                        aseg.b_height = b_height;//高
                        aseg.b_theight = b_theight;//封阳台高
                        aseg.b_bheight = b_bheight;//离地高度
                        aseg.b_btype = b_btype;//阳台底部高度
                        if (isbarrier)
                        {
                            aseg.ba_length = ba_length;
                            aseg.ba_width = ba_width;
                            aseg.ba_num = ba_num;
                            aseg.isbarrier = true;
                        }
                        else
                            aseg.isbarrier = false;

                        if (ishandrai)
                        {
                            aseg.hr_bheight = hr_bheight;
                            aseg.hr_width = hr_width;
                            aseg.hr_thickness = hr_thickness;
                            aseg.ishandrail = true;
                        }
                        else
                            aseg.ishandrail = false;

                        if (ismidpanel)
                        {
                            aseg.mp_updist = mp_updist;
                            aseg.mp_downdist = mp_downdist;
                            aseg.mp_thickness = mp_thickness;
                            aseg.ismidpanel = true;
                        }
                        else
                            aseg.ismidpanel = false;

                        if (!IsShareWall(ref otherids, ref aseg))
                        {//非内墙
                            if (isallsametpe)
                            {//所有立面窗型都一样
                                //窗型
                                aseg.wintype = GetIntfromProStr(GetProStrFromEnt((int)balconyids[i], "allwintype"));
                            }
                            else
                            {//立面窗型不一样
                                mx1 = (aseg.x1 + aseg.x2) / 2.0;
                                my1 = (aseg.y1 + aseg.y2) / 2.0;
                                vx = mx1 - mx;
                                vy = my1 - my;
                                vz = 0;
                                double ang = 0;
                                double minang = 0;
                                int mininx = -1;
                                //找到角度最接近的 2017-11-29
                                for (int k = 0; k < balconywintypes.Count; k++)
                                {
                                    //计算两矢量夹角,注意vectorangle返回的是弧角
                                    ang = axEWdraw1.VectorAngle(new object[] { vx, vy, vz }, new object[] { ((SInxWall)balconywintypes[k]).x, ((SInxWall)balconywintypes[k]).y, ((SInxWall)balconywintypes[k]).z });
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
                                    aseg.wintype = ((SInxWall)balconywintypes[mininx]).inx;
                                }
                            }
                            balconywalls.Add(aseg);
                        }
                        else
                        {//记录下内墙的线段,之后会在创建窗户的时候用于判断 2017-12-01
                            if (innerwalls == null)
                                innerwalls = new ArrayList();
                            SWallSeg ainnerwall = new SWallSeg();
                            ainnerwall.x1 = aseg.x1;
                            ainnerwall.y1 = aseg.y1;
                            ainnerwall.x2 = aseg.x2;
                            ainnerwall.y2 = aseg.y2;
                            innerwalls.Add(ainnerwall);
                        }
                        //

                    }
                }
                tptslist.Clear();
                //创建阳台(立面窗实体)
                if (balconywalls.Count > 0)
                {
                    //grpids为组成一个阳台所有实体的ID集
                    ArrayList grpids = new ArrayList();
                    for (int j = 0; j < balconywalls.Count; j++)
                    {
                        BalconyWallSeg aseg = (BalconyWallSeg)balconywalls[j];
                        if (((BalconyWallSeg)balconywalls[j]).b_btype == 0)
                        {//砖墙基本数据
                            MakeBalconyWalls(ref aseg, 0, g_wallthickness, ref innerwalls, ref grpids);
                        }
                        else
                        {//玻璃墙
                            MakeBalconyWalls(ref aseg, ((BalconyWallSeg)balconywalls[j]).b_btype, g_wallthickness, ref innerwalls, ref grpids);
                        }
                    }
                    //创建组
                    axEWdraw1.ClearIDBuffer();
                    for (int j = 0; j < grpids.Count; j++)
                    {
                        axEWdraw1.AddIDToBuffer((int)grpids[j]);
                    }
                    int balcondyent = axEWdraw1.MakeGroup("balcony_1", new object[] { 0, 0, 0 });
                    if (ids == null)
                        ids = new ArrayList();
                    ids.Add(balcondyent);
                    grpids.Clear();
                }
                //清空
                balconywalls.Clear();
                innerwalls.Clear();
                balconywintypes.Clear();
            }
            balconyids.Clear();
            otherids.Clear();
        }

        /*MakeBalconyWalls创建阳台的墙与玻璃
         * aseg         一段阳台墙的立面
         * btype        是一般的砖墙0 或是玻璃
         * thickness    厚度
         * num          一段立面可以放一个窗,也可以放多个,num是个数(均分)
         */
        private bool MakeBalconyWalls(ref BalconyWallSeg aseg, int btype, double thickness, ref ArrayList innerwalls, ref ArrayList grpids, int num = 1)
        {
            int swsize = axEWdraw1.GetSingleWallSize();
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;

            double mx, my, mx1, my1;
            double sx, sy, sz, ex, ey, ez, ex1, ey1, ez1;
            sx = sy = sz = ex = ey = ez = ex1 = ey1 = ez1 = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            double dist = 0;
            mx = (aseg.x1 + aseg.x2) / 2.0;
            my = (aseg.y1 + aseg.y2) / 2.0;
            if (grpids == null)
                grpids = new ArrayList();
            ArrayList ids = new ArrayList();
            ArrayList otherids = new ArrayList();
            int ent = 0;
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                mx1 = (sx + ex) / 2.0;
                my1 = (sy + ey) / 2.0;
                if (axEWdraw1.PointDistance(mx, my, 0, mx1, my1, 0) < 0.001)
                {//找到相对应的墙
                    //原地复制墙,并将复制的墙重新设置高度
                    int cent = axEWdraw1.Copy(singledid, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
                    if (cent > 0)
                    {
                        //炸碎墙,并取得底部的面
                        if (axEWdraw1.Explode(cent))
                        {
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            int epsize = axEWdraw1.GetExplodeIDBufferSize();
                            for (int j = 0; j < epsize; j++)
                            {
                                ent = axEWdraw1.GetExplodeIDBuffer(j);
                                axEWdraw1.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                if (Math.Abs(minz) < 0.001 && Math.Abs(maxz) < 0.001)
                                {
                                    ids.Add(ent);
                                }
                                else
                                {
                                    otherids.Add(ent);
                                }
                            }
                        }
                    }
                    //有底部面的时候,且等于1个. 2017-11-30
                    if (ids.Count == 1)
                    {
                        if (btype == 0)
                            ent = axEWdraw1.Prism((int)ids[0], aseg.b_height, new object[] { 0, 0, 1 });
                        else
                            ent = axEWdraw1.Prism((int)ids[0], 25, new object[] { 0, 0, 1 });//这里的25是将阳台围墙做为水泥基,这个25是可以根据情况改的
                        grpids.Add(ent);//增加到组
                        if (btype == 0)
                            axEWdraw1.SetEntTexture(ent, "balcony.jpg", 1, 1, 1, 1, 0, 0);//这是阳台砖墙的纹理,可以自行改变
                        else
                            axEWdraw1.SetEntColor(ent, axEWdraw1.RGBToIndex(255, 255, 255));
                        //将原有的墙消隐 2017-11-30
                        axEWdraw1.SetEntityInvisible(singleid, true);
                        g_balconyhidewall.Add(singleid);
                    }
                    //删除不需要的面
                    for (int j = 0; j < otherids.Count; j++)
                        axEWdraw1.Delete((int)otherids[j]);
                    //删除不需要的面
                    for (int j = 0; j < ids.Count; j++)
                        axEWdraw1.Delete((int)ids[j]);
                    break;
                }
            }
            //创建窗户
            int innerinx = -1;
            if (IsBolconyInnerWallPts(aseg.x1, aseg.y1, aseg.z1, ref innerwalls, ref innerinx))
            {
                g_balconyconnectpts.Add(new SPoint(aseg.x1, aseg.y1, aseg.z1));
                axEWdraw1.Polar(new object[] { aseg.x1, aseg.y1, aseg.z1 }, new object[] { aseg.x2 - aseg.x1, aseg.y2 - aseg.y1, aseg.z2 - aseg.z1 }, thickness / 2.0, ref sx, ref sy, ref sz);
                aseg.x1 = sx;
                aseg.y1 = sy;
                aseg.z1 = sz;
                //对得内墙部分的纹理
                string texfile = "";
                double usize, vsize;
                usize = vsize = 0;
                if (g_viewmode != 4 && g_viewmode != 1)
                {
                    GetWallFaceTex(((SWallSeg)innerwalls[innerinx]).x1, ((SWallSeg)innerwalls[innerinx]).y1,
                                    ((SWallSeg)innerwalls[innerinx]).x2, ((SWallSeg)innerwalls[innerinx]).y2,
                                        sx, sy, ref texfile, ref usize, ref vsize);
                    int sewing = MakeSewingFace(
                                sx, sy,
                                ((SWallSeg)innerwalls[innerinx]).x2 - ((SWallSeg)innerwalls[innerinx]).x1, ((SWallSeg)innerwalls[innerinx]).y2 - ((SWallSeg)innerwalls[innerinx]).y1, 0,
                                g_wallthickness,
                                g_wallheight,
                                texfile,
                                usize,
                                vsize
                                );

                    if (sewing > 0)
                    {
                        if (g_balconyotherids == null)
                            g_balconyotherids = new ArrayList();
                        g_balconyotherids.Add(sewing);
                    }
                }
            }
            if (IsBolconyInnerWallPts(aseg.x2, aseg.y2, aseg.z2, ref innerwalls, ref innerinx))
            {
                g_balconyconnectpts.Add(new SPoint(aseg.x2, aseg.y2, aseg.z2));
                axEWdraw1.Polar(new object[] { aseg.x2, aseg.y2, aseg.z2 }, new object[] { aseg.x1 - aseg.x2, aseg.y1 - aseg.y2, aseg.z1 - aseg.z2 }, thickness / 2.0, ref ex, ref ey, ref ez);
                aseg.x2 = ex;
                aseg.y2 = ey;
                aseg.z2 = ez;
                //对得内墙部分的纹理
                string texfile = "";
                double usize, vsize;
                usize = vsize = 0;
                if (g_viewmode != 4 && g_viewmode != 1)
                {
                    GetWallFaceTex(((SWallSeg)innerwalls[innerinx]).x1, ((SWallSeg)innerwalls[innerinx]).y1,
                                ((SWallSeg)innerwalls[innerinx]).x2, ((SWallSeg)innerwalls[innerinx]).y2,
                                    ex, ey, ref texfile, ref usize, ref vsize);
                    int sewing = MakeSewingFace(
                                ex, ey,
                                ((SWallSeg)innerwalls[innerinx]).x1 - ((SWallSeg)innerwalls[innerinx]).x2, ((SWallSeg)innerwalls[innerinx]).y1 - ((SWallSeg)innerwalls[innerinx]).y2, 0,
                                g_wallthickness,
                                g_wallheight,
                                texfile,
                                usize,
                                vsize
                                );
                    if (sewing > 0)
                    {
                        if (g_balconyotherids == null)
                            g_balconyotherids = new ArrayList();
                        g_balconyotherids.Add(sewing);
                    }
                }
            }
            //重算中心点
            mx = (aseg.x1 + aseg.x2) / 2.0;
            my = (aseg.y1 + aseg.y2) / 2.0;

            //判断窗户的内朝向
            sx = aseg.x2 - aseg.x1;//这里的sx,sy是阳台立面的方向
            sy = aseg.y2 - aseg.y1;
            axEWdraw1.RotateVector(aseg.x2 - aseg.x1, aseg.y2 - aseg.y1, 0, mx, my, 0, 0, 0, 1, 90, ref ox, ref oy, ref oz);
            axEWdraw1.Polar(new object[] { mx, my, 0 }, new object[] { ox, oy, oz }, thickness, ref ex, ref ey, ref ez);
            if (axEWdraw1.Is2DPtInsideArea(ex, ey, aseg.id))
            {
                aseg.m_dx = ex - mx;
                aseg.m_dy = ey - my;
                aseg.m_dz = 0;
            }
            else
            {
                aseg.m_dx = mx - ex;
                aseg.m_dy = my - ey;
                aseg.m_dz = 0;
            }
            //窗户中心点的位置
            double sp = axEWdraw1.PointDistance(aseg.x1, aseg.y1, 0, aseg.x2, aseg.y2, 0) / num;
            string filepath, grpname;
            double width = 0;
            filepath = grpname = "";
            for (int i = 0; i < num; i++)
            {
                axEWdraw1.Polar(new object[] { aseg.x1, aseg.y1, 0 }, new object[] { sx, sy, 0 }, sp * i, ref ex, ref ey, ref ez);
                axEWdraw1.Polar(new object[] { aseg.x1, aseg.y1, 0 }, new object[] { sx, sy, 0 }, sp * (i + 1), ref ex1, ref ey1, ref ez1);
                //计算中心点位置
                mx = (ex + ex1) / 2.0;
                my = (ey + ey1) / 2.0;
                width = axEWdraw1.PointDistance(ex, ey, ez, ex1, ey1, ez1);
                //根据窗型得以3DS文件,以及组名
                if (ConvertWinTypeTo3DSGrp(aseg.wintype, ref filepath, ref grpname))
                {
                    int grpwin = Import3DSBalcony(filepath, grpname,
                                        thickness,
                                        width,
                                        aseg.b_theight - aseg.b_height,
                                        mx, my, aseg.b_height,
                                        aseg.m_dx, aseg.m_dy, aseg.m_dz);
                    //这里的炸碎导入数组,是为了让阳台成为一个整体
                    axEWdraw1.ClearExplodeIDBuffer();
                    axEWdraw1.Explode(grpwin);
                    int exsize = axEWdraw1.GetExplodeIDBufferSize();
                    for (int j = 0; j < exsize; j++)
                    {
                        int grpsubent = axEWdraw1.GetExplodeIDBuffer(j);
                        grpids.Add(grpsubent);//增加到组
                    }
                    //
                }
            }
            ArrayList tpts = new ArrayList();
            //判断是否有栏杆,如果有计算栏杆位置
            if (aseg.isbarrier)
            {
                //根据数量计算间隔的距离
                sp = (axEWdraw1.PointDistance(aseg.x1, aseg.y1, 0, aseg.x2, aseg.y2, 0) - aseg.ba_length) / (aseg.ba_num - 1);
                //计算栏栅的起点,第一个栏杆位置的点,表示的是底部中心点,用其它3DS对象对齐时,也要以这个底部中心点对齐(先把长,宽,高缩放正确).
                sx = aseg.x2 - aseg.x1;//这里的sx,sy表示方向
                sy = aseg.y2 - aseg.y1;
                axEWdraw1.Polar(new object[] { aseg.x1, aseg.y1, 0 }, new object[] { sx, sy, 0 }, aseg.ba_length / 2.0, ref ox, ref oy, ref oz);
                for (int i = 0; i < aseg.ba_num; i++)
                {
                    axEWdraw1.Polar(new object[] { ox, oy, oz }, new object[] { sx, sy, 0 }, sp * i, ref ex, ref ey, ref ez);
                    //将位置点保存在数组中,之后的中板会用到
                    SPoint apt = new SPoint(ex, ey, ez);
                    tpts.Add(apt);
                    //
                    //创建栏栅对象,这里以BOX替代了,用其它3DS对象对齐时,也要以这个底部中心点对齐(先把长,宽,高缩放正确).
                    ent = axEWdraw1.Box(new object[] { 0, 0, 0 }, aseg.ba_length, aseg.ba_width, aseg.b_height);
                    axEWdraw1.MoveTo(ent, new object[] { aseg.ba_length / 2.0, aseg.ba_length / 2.0, 0 }, new object[] { ex, ey, ez });
                    grpids.Add(ent);//增加到组
                }
            }
            //判断是否有栏杆
            if (aseg.ishandrail)
            {
                sp = axEWdraw1.PointDistance(aseg.x1, aseg.y1, 0, aseg.x2, aseg.y2, 0);
                ent = axEWdraw1.Box(new object[] { 0, 0, 0 }, sp, aseg.hr_width, aseg.hr_thickness);
                axEWdraw1.AxisAlign(ent,
                    new object[] { 0, aseg.b_width / 2.0, aseg.hr_thickness }, new object[] { sp, aseg.b_width / 2.0, aseg.hr_thickness }, new object[] { 0, 0, 1 },
                    new object[] { aseg.x1, aseg.y1, aseg.hr_bheight }, new object[] { aseg.x2, aseg.y2, aseg.hr_bheight }, new object[] { 0, 0, 1 });
                grpids.Add(ent);//增加到组

            }
            if (aseg.ismidpanel)
            {
                //判断是否有中板
                for (int i = 0; i < tpts.Count - 1; i++)
                {
                    //根据栏栅计算中板的宽度
                    width = axEWdraw1.PointDistance(((SPoint)tpts[i]).x, ((SPoint)tpts[i]).y, ((SPoint)tpts[i]).z, ((SPoint)tpts[i + 1]).x, ((SPoint)tpts[i + 1]).y, ((SPoint)tpts[i + 1]).z) - aseg.ba_length;
                    ent = axEWdraw1.Box(new object[] { 0, 0, 0 }, width, aseg.mp_thickness, aseg.hr_bheight - aseg.mp_updist - aseg.mp_downdist);
                    //计算栏栅的起点,第一个栏杆位置的点,表示的是底部中心点,用其它3DS对象对齐时,也要以这个底部中心点对齐(先把长,宽,高缩放正确).
                    sx = ((SPoint)tpts[i + 1]).x - ((SPoint)tpts[i]).x;//这里的sx,sy表示方向
                    sy = ((SPoint)tpts[i + 1]).y - ((SPoint)tpts[i]).y;
                    axEWdraw1.Polar(new object[] { ((SPoint)tpts[i]).x, ((SPoint)tpts[i]).y, 0 }, new object[] { sx, sy, 0 }, aseg.ba_length / 2.0, ref ex, ref ey, ref ez);
                    axEWdraw1.Polar(new object[] { ex, ey, 0 }, new object[] { sx, sy, 0 }, width, ref ex1, ref ey1, ref ez1);

                    axEWdraw1.AxisAlign(ent,
                                       new object[] { 0, aseg.mp_thickness / 2.0, aseg.hr_bheight - aseg.mp_updist - aseg.mp_downdist }, new object[] { width, aseg.mp_thickness / 2.0, aseg.hr_bheight - aseg.mp_updist - aseg.mp_downdist }, new object[] { 0, 0, 1 },
                                       new object[] { ex, ey, aseg.hr_bheight - aseg.mp_updist }, new object[] { ex1, ey1, aseg.hr_bheight - aseg.mp_updist }, new object[] { 0, 0, 1 });
                    axEWdraw1.SetTransparency(ent, 0.8);
                    grpids.Add(ent);//增加到组
                }
            }
            //
            ids.Clear();
            otherids.Clear();
            tpts.Clear();
            return false;
        }
        /*判断一个点是否是阳台区域内的内部墙端点 2017-12-01
         * x,y,z      要判断的点
         * innerwalls 内墙段集
         */
        private bool IsBolconyInnerWallPts(double x, double y, double z, ref ArrayList innerwalls, ref int inx)
        {
            for (int i = 0; i < innerwalls.Count; i++)
            {

                if ((Math.Abs(((SWallSeg)innerwalls[i]).x1 - x) < 0.001 &&
                    Math.Abs(((SWallSeg)innerwalls[i]).y1 - y) < 0.001
                    )
                    ||
                    (Math.Abs(((SWallSeg)innerwalls[i]).x2 - x) < 0.001 &&
                    Math.Abs(((SWallSeg)innerwalls[i]).y2 - y) < 0.001
                    )
                    )
                {
                    inx = i;
                    return true;
                }
            }
            return false;
        }

        /*
         */
        private int MakeSewingFace(
                            double x, double y,
                            double vx, double vy, double vz,
                            double thickness,
                            double height,
                            string texfile,
                            double usize,
                            double vsize
                            )
        {
            double x1, y1, z1, x2, y2, z2;
            x1 = y1 = z1 = x2 = y2 = z2 = 0;
            axEWdraw1.Polar(new object[] { x, y, 0 }, new object[] { vx, vy, vz }, thickness / 2.0, ref x1, ref y1, ref z1);
            axEWdraw1.Polar(new object[] { x, y, 0 }, new object[] { -vx, -vy, -vz }, thickness / 2.0, ref x2, ref y2, ref z2);
            int ent = axEWdraw1.Line(new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 });
            int face = axEWdraw1.Prism(ent, height, new object[] { 0, 0, 1 });
            axEWdraw1.SetTexFaceTextureUVSize(true, usize, vsize);
            axEWdraw1.SetTexFaceOfSolid(texfile, 0, 0, 1, 1, 1, 1, 0);
            axEWdraw1.TextureFaceOfSolid(face, new object[] { (x1 + x2) / 2.0, (y1 + y2) / 2.0, height / 2.0 });
            axEWdraw1.SetTexFaceTextureUVSize(false, usize, vsize);

            if (face > 0)
                return face;
            return 0;
        }
        /*GetWallFaceTex 根据起点与终点以及参考点得到相对应墙的纹理 2017-12-05
         * x1,y1  [in]  起点
         * x2,y2  [in]  终点
         * x,y    [in]  参考点
         * texfile[out] 得到的纹理文件名
         * usize  [out] 得到的U纹理的大小
         * vsize  [out] 得到的V纹理的大小
         */
        private bool GetWallFaceTex(double x1, double y1,
                                    double x2, double y2,
                                    double x, double y,
                                    ref string texfile,
                                    ref double usize,
                                    ref double vsize
                                    )
        {
            int singleid, singledid, singleorgid, singleinx;
            double sx, sy, sz, ex, ey, ez;
            double UOrigin, VOrigin, URepeat, VRepeat, ScaleU, ScaleV, transparency, USize, VSize;
            UOrigin = VOrigin = URepeat = VRepeat = ScaleU = ScaleV = transparency = USize = VSize = 0;

            singleid = singledid = singleorgid = singleinx = 0;
            sx = sy = sz = ex = ey = ez = 0.0;
            int swsize = axEWdraw1.GetSingleWallSize();
            for (int i = 0; i < swsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                if (
                        (Math.Abs(x1 - sx) < 0.001 &&
                         Math.Abs(y1 - sy) < 0.001 &&
                         Math.Abs(x2 - ex) < 0.001 &&
                         Math.Abs(y2 - ey) < 0.001
                        )
                        ||
                        (Math.Abs(x2 - sx) < 0.001 &&
                         Math.Abs(y2 - sy) < 0.001 &&
                         Math.Abs(x1 - ex) < 0.001 &&
                         Math.Abs(y1 - ey) < 0.001
                        )
                   )
                {
                    texfile = axEWdraw1.GetTexOfFaceByPt(singleid, new object[] { x, y, 0 },
                                                ref UOrigin, ref VOrigin, ref URepeat, ref VRepeat, ref ScaleU, ref ScaleV,
                                                ref transparency, ref USize, ref VSize);
                    usize = USize;
                    vsize = VSize;
                    return true;
                }
            }
            return false;
        }
        /*ConvertWinTypeTo3DSGrp 根据窗型得以3DS文件,以及组名
         * wintype  窗型
         * filePath 3DS文件名
         * grpname  组名
         */
        private bool ConvertWinTypeTo3DSGrp(int wintype, ref string filePath, ref string grpname)
        {
            switch (wintype)
            {
                case 0:
                    {
                        filePath = "window.3ds";
                        grpname = "balcony_win1";
                    }
                    break;
                case 1:
                    {
                        filePath = "ldc.3ds";
                        grpname = "balcony_win2";
                    }
                    break;

            }
            if (filePath.Length > 0)
                return true;
            return false;
        }
        /*Import3DSBalcony 导入窗户 2017-12-01
         * filePath  3DS文件名
         * grpname   窗户的组名称(同一类窗户,组名相同)
         * thickness 厚度
         * width     宽度
         * height    高度
         * cx,cy,cz  目标点
         * dx,dy,dz  朝向
         */
        private int Import3DSBalcony(string filePath, string grpname,
                                        double thickness,
                                        double width,
                                        double height,
                                        double cx, double cy, double cz,
                                        double dx, double dy, double dz)
        {
            double x, y, z, x1, y1, z1, x2, y2, z2;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            if (g_balconywins == null)
                g_balconywins = new ArrayList();
            string orggrpname = "balconywin_org_" + grpname;
            //在已导入的模型中查找是否已存在该阳台窗模型
            int orggrpid = 0;
            for (int i = 0; i < g_balconywins.Count; i++)
            {
                if (((BalconyWin)g_balconywins[i]).grpname == orggrpname)
                {
                    orggrpid = ((BalconyWin)g_balconywins[i]).id;
                    break;
                }
            }
            //
            int ent0 = 0;
            if (orggrpid == 0)
            {
                //1.导入3DS模型,注意这里的3DS模型最好是最简模型
                //2.导入的3DS模型,一定要都是前项
                //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
                int impsize = axEWdraw1.Imp3DSWithTexture(filePath);
                //创建组
                for (int i = 1; i <= impsize; i++)
                {
                    ent0 = axEWdraw1.GetImpEntID(i);
                    axEWdraw1.AddIDToBuffer(ent0);
                }
                orggrpid = axEWdraw1.MakeGroup(orggrpname, new object[] { 0, 0, 0 });
                //2017-12-06
                BalconyWin abalcony = new BalconyWin();
                abalcony.id = orggrpid;
                abalcony.grpname = orggrpname;
                g_balconywins.Add(abalcony);
                //
                axEWdraw1.SetEntityInvisible(orggrpid, true);
                //从原始组成复制
            }
            int group = axEWdraw1.Copy(orggrpid, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { 0, 0, 0 });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            double midx = (x2 + x1) / 2.0;
            double midy = (y2 + y1) / 2.0;
            double midz = 0;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //计算轴向缩放系数
            double xscale = width / (x2 - x1);
            double yscale = thickness / (y2 - y1);
            double zscale = height / (z2 - z1);
            //
            axEWdraw1.MeshScaleByXYZAxis3(group, xscale, yscale, zscale);
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            //
            axEWdraw1.Ax1AlignWithZAsYAxis(group, new object[] { midx, midy, 0 }, new object[] { 0, -1, 0 },
                                     new object[] { cx, cy, cz }, new object[] { dx, dy, dz }
                                     );
            if (group > 0)
                return group;
            return 0;
        }
        //判断一个阳台立面墙段除本区域外,是否也在其它区域内出现 2017-11-29
        private bool IsShareWall(ref ArrayList ids, ref BalconyWallSeg aseg)
        {
            double mx = (aseg.x1 + aseg.x2) / 2.0;
            double my = (aseg.y1 + aseg.y2) / 2.0;
            double mx1, my1;
            mx1 = my1 = 0;
            ArrayList tlist = new ArrayList();
            for (int i = 0; i < ids.Count; i++)
            {
                GetPtsFromSolid((int)ids[i], ref tlist);
                if (tlist.Count > 0)
                {
                    int len = tlist.Count / 3 - 1;
                    for (int j = 0; j < len; j++)
                    {
                        mx1 = ((double)tlist[j * 3] + (double)tlist[(j + 1) * 3]) / 2.0;
                        my1 = ((double)tlist[j * 3 + 1] + (double)tlist[(j + 1) * 3 + 1]) / 2.0;
                        if (axEWdraw1.PointDistance(mx, my, 0, mx1, my1, 0) < 0.001)
                        {
                            aseg.st = 1;//说明是内部墙壁,不用再做放置窗的处理
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /*判断是否是一个区域的墙 2017-12-15
         * wallid 墙的ID号
         * 返回:
         * 如果是一个区域墙,则返回该区域的ID,其它返回0
         */
        private int IsAreaWall(int wallid)
        {
            int singlewallsize = 0;
            int singleid = 0;
            int singledid = 0;
            int singleorgid = 0;
            int singleinx = -1;
            double sx, sy, sz, ex, ey, ez, mx, my;
            sx = sy = sz = ex = ey = ez = mx = my = 0.0;
            double ox, oy, oz;
            ox = oy = oz = 0.0;
            singlewallsize = axEWdraw1.GetSingleWallSize();
            for (int i = 0; i < singlewallsize; i++)
            {
                axEWdraw1.GetSingleWallInfo(i, ref singleid, ref singledid, ref singleorgid, ref singleinx, ref sx, ref sy, ref sz, ref ex, ref ey, ref ez);
                if (singleid == wallid)
                {
                    mx = (sx + ex) / 2.0;
                    my = (sy + ey) / 2.0;
                    break;
                }
            }
            int ent = 0;
            int entsize = axEWdraw1.GetEntSize();
            ArrayList tptslist = new ArrayList();
            double mx1, my1;
            mx1 = my1 = 0.0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                if (ent > 0)
                {
                    if (axEWdraw1.GetEntType(ent) == 50)
                    {
                        if (GetPtsFromSolid(ent, ref tptslist))
                        {
                            int ptsize = tptslist.Count / 3 - 1;
                            for (int j = 0; j < ptsize; j++)
                            {
                                mx1 = ((double)tptslist[j * 3] + (double)tptslist[(j + 1) * 3]) / 2.0;
                                my1 = ((double)tptslist[j * 3 + 1] + (double)tptslist[(j + 1) * 3 + 1]) / 2.0;
                                if (axEWdraw1.PointDistance(mx, my, 0, mx1, my1, 0) < 0.001)
                                {
                                    return ent;
                                }
                            }
                        }

                    }
                }
            }
            return 0;
        }

        /*从一个Polyline的多段线对象得到异型窗的点数据 2018-01-05
         * ent 要转换成数据的多段线对象
         * 返回值:
         * 如果成功,返回异型窗的字符数据
         */
        private string MakeProfileWinData(int ent)
        {
            int type = axEWdraw1.GetEntType(ent);
            if (type == 7 || type == 9)
            {
                int tmpjs1, tmpjs2;
                tmpjs1 = tmpjs2 = 0;
                double spx, spy, spz, epx, epy, epz, vx, vy, vz;
                spx = spy = spz = epx = epy = epz = vx = vy = vz = 0.0;
                bool isarc = false;
                string ptsstr = "pts:";
                string arcptsstr = "arcwinpts:";
                int segsize = axEWdraw1.GetPolyLineSegmentSize(ent);
                for (int i = 0; i < segsize; i++)
                {
                    axEWdraw1.GetPolyLineSegment(ent, i, ref spx, ref spy, ref spz, ref epx, ref epy, ref epz, ref vx, ref vy, ref vz, ref isarc);
                    if ((i + 1) == segsize)
                    {
                        ptsstr += String.Format("{0:f3}", spx) + "," + String.Format("{0:f3}", spy) + "," + String.Format("{0:f3}", spz) + ",";
                        ptsstr += String.Format("{0:f3}", epx) + "," + String.Format("{0:f3}", epy) + "," + String.Format("{0:f3}", epz);
                        ptsstr += ";";
                    }
                    else
                        ptsstr += String.Format("{0:f3}", spx) + "," + String.Format("{0:f3}", spy) + "," + String.Format("{0:f3}", spz) + ",";
                    tmpjs1++;

                    if (isarc)
                    {
                        arcptsstr += i.ToString() + "," + String.Format("{0:f3}", vx) + "," + String.Format("{0:f3}", vy) + "," + String.Format("{0:f3}", vz);
                        if ((i + 1) == segsize)
                            arcptsstr += ";";
                        else
                            arcptsstr += ",";
                        tmpjs2++;
                    }
                }
                if (arcptsstr.Length > 0)
                {
                    if (arcptsstr[arcptsstr.Length - 1] == ',')
                    {
                        arcptsstr = arcptsstr.Remove(arcptsstr.Length - 1);
                        arcptsstr += ";";
                    }
                }
                if (tmpjs1 > 0 && tmpjs2 > 0)
                    return "profilewin:1;" + ptsstr + arcptsstr;
                else if (tmpjs1 > 0 && tmpjs2 <= 0)
                    return "profilewin:1;" + ptsstr;
                else return "";
            }
            return "";
        }

        /*Import3DsProfileWin 函数导入3DS异型窗
         * 参数:
         * filePath                 输入  文件路径(为了方便调试,请把3DS文件与纹理都先放到bin目录下的debug或release中)
         * grpname                  输入  组名(在例子中门是以door开头的之后是 _ 下划线符号 然后是门的类型,表数字表示. 比如:door_1)
         * xsite,ysite,zsite        输入  3DS导入后,变成组的基点,统一是0,0,0
         * xoff                     输入  门套包边X轴方向长出墙壁的距离(一侧) ,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff                     输入  如果门套长上长出的锁和门板厚度,则用该值表示,在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * yoff1                    输入  门套包边Y轴方向长出墙壁的距离(一侧),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * zoff                     输入  门套包边Z轴方向长出墙洞顶部的距离(顶部),在数据库或数据文件中定义读出,与唯一个3DS造型相对.
         * lenth,thickness,height   输入  门在吸附时定义的宽度(长),厚度,高度,定义在组的自定义数据中,从自定义数据中读.
         * isoutwarddoor            输入  保留参数,用来区分内开或外开.如果导入3DS文件与MakeDoorSymbol(或其它创建符号函数)所创建符号一致,那就不用这个变量.
         * 返回值:
         * 如果成功返回创建的 3DS模型组的ID,其它返回0.
         */
        private int Import3DsProfileWin(string filePath, string grpname,
                                double xsite, double ysite, double zsite,
                                double xoff, double yoff, double yoff1, double zoff,
                                double length, double thickness, double height,/*这是门符号定义的长,厚度,高度*/
                                bool isoutwarddoor = false /*这个参数表示是否是外开门(如果是true),默认是内开门,这个和符号的初始相同*/
                                )
        {
            int impsize = 0;
            int ent0 = 0;
            double x, y, z, x1, y1, z1, x2, y2, z2;
            double minx, miny, minz, maxx, maxy, maxz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw1.ClearIDBuffer();
            ////重新创建一个带3DS网面的组
            int group = 0;
            string[] namearr = grpname.Split('_');
            string orggrpname = "";
            if (namearr[0] == "profilewin")
            {//判断类型
                orggrpname = "profilewin" + "_" + "org" + "_" + namearr[1];
            }//else ...etc. 也可以在些增窗户或其它的判断

            //
            int orggrp = IsExistGroup(orggrpname);
            if (orggrp <= 0)
            {//如果没有导入过,则从文件导入并创建组,这样可以保证相同的组,只导入一次
                //1.导入3DS模型,注意这里的3DS模型最好是最简模型
                //2.导入的3DS模型,一定要都是前项
                //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
                impsize = axEWdraw1.Imp3DSWithTexture(filePath);
                //创建组
                for (int i = 1; i <= impsize; i++)
                {
                    ent0 = axEWdraw1.GetImpEntID(i);
                    axEWdraw1.AddIDToBuffer(ent0);
                }
                orggrp = axEWdraw1.MakeGroup(orggrpname, new object[] { xsite, ysite, zsite });
                //
                axEWdraw1.SetEntityInvisible(orggrp, true);
                //从原始组成复制
                group = axEWdraw1.Copy(orggrp, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
            }
            else //如果已经导入,则直接复制
                group = axEWdraw1.Copy(orggrp, new object[] { 0, 0, 0 }, new object[] { 0, 0, 0 });
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { xsite, ysite, zsite });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            double midx = (x2 + x1) / 2.0;
            double midy = (y2 + y1 + yoff) / 2.0;
            double midz = 0;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //计算轴向缩放系数
            double xscale = length / ((x2 - x1) - xoff * 2.0);
            double yscale = thickness / ((y2 - y1) - yoff - yoff1 * 2.0);
            double zscale = height / ((z2 - z1) - zoff);
            //
            axEWdraw1.MeshScaleByXYZAxis3(group, xscale, yscale, zscale);
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            return group;
        }

        /*绘制异型窗 2018-01-05
         */
        private void DrawProfileWin()
        {
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            canceldrawwalljs = -1;
            if (!axEWdraw1.IsEndCommand())
            {
                if (lastsym > 0)
                {
                    axEWdraw1.Delete(lastsym);
                    lastsym = 0;
                }
            }
            axEWdraw1.CancelCommand();
            //绘制轮廓线 2018-01-04
            axEWdraw1.ClearIDBuffer();
            int seg = axEWdraw1.Line(new object[] { 0, 0, 0 }, new object[] { 1600, 0, 0 });
            axEWdraw1.AddIDToBuffer(seg);
            seg = axEWdraw1.Line(new object[] { 1600, 0, 0 }, new object[] { 1600, 0, 600 });
            axEWdraw1.AddIDToBuffer(seg);
            seg = axEWdraw1.Line(new object[] { 1600, 0, 600 }, new object[] { 800, 0, 600 });
            axEWdraw1.AddIDToBuffer(seg);
            seg = axEWdraw1.Line(new object[] { 800, 0, 600 }, new object[] { 800, 0, 1200 });
            axEWdraw1.AddIDToBuffer(seg);
            seg = axEWdraw1.Arc3P(new object[] { 800, 0, 1200 }, new object[] { 400, 0, 1600 }, new object[] { 0, 0, 1200 });
            axEWdraw1.AddIDToBuffer(seg);
            seg = axEWdraw1.Line(new object[] { 0, 0, 1200 }, new object[] { 0, 0, 0 });
            axEWdraw1.AddIDToBuffer(seg);
            //焊接
            int ent = axEWdraw1.Join(-1, -1);
            axEWdraw1.ClearIDBuffer();
            //生成面
            int fac = axEWdraw1.EntToFace(ent, false);
            //拉伸
            int obj = axEWdraw1.Prism(fac, 128, new object[] { 0, 1, 0 });
            //转换轮廓线为数据信息
            string memo = MakeProfileWinData(ent);
            axEWdraw1.Delete(ent);
            axEWdraw1.Delete(fac);//2018-07-06
            //绘制符号
            axEWdraw1.GetEntBoundingBox(obj, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            axEWdraw1.Clear3DPtBuf();
            axEWdraw1.AddOne3DVertex(0, 0, 3000);
            axEWdraw1.AddOne3DVertex(maxx - minx, 0, 3000);
            axEWdraw1.AddOne3DVertex(maxx - minx, g_wallthickness, 3000);
            axEWdraw1.AddOne3DVertex(0, g_wallthickness, 3000);
            int topwire = axEWdraw1.PolyLine3D(true);
            int topent = axEWdraw1.EntToFace(topwire, true);
            axEWdraw1.SetEntColor(topent, axEWdraw1.RGBToIndex(255, 255, 255));
            axEWdraw1.SetTransparency(topent, 0.3);
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.AddIDToBuffer(obj);
            axEWdraw1.AddIDToBuffer(topent);
            lastsym = axEWdraw1.MakeGroup("symbol_profilewin_0", new object[] { 0, 0, 0 });

            axEWdraw1.SetGroupInsPt(lastsym, new object[] { (minx + maxx) / 2.0, (miny + maxy) / 2.0, 0 });
            memo += "width:" + String.Format("{0:f3}", maxx - minx) + ";";
            memo += "height:" + String.Format("{0:f3}", maxz - minz) + ";";
            memo += "thickness:" + String.Format("{0:f3}", maxy - miny) + ";";//2018-07-06
            axEWdraw1.SetEntityUserData(lastsym, memo);
            axEWdraw1.SetGroupPlaneByBoxFace(lastsym, 2);
            isdrawabsorb = true;
            axEWdraw1.EnableCheckAbsorbIns(false);
            axEWdraw1.EnableAbsorbHigh(true, 800);
            axEWdraw1.EnableAbsorbDepth(true, g_wallthickness / 2.0);
            axEWdraw1.SetORTHO(false);
            axEWdraw1.EnableOrthoHVMode(false);
            axEWdraw1.CancelCommand();
            axEWdraw1.AddOrRemoveSelect(lastsym);
            axEWdraw1.ToDrawAbsorb();
        }
        //判断是否是家具类 2018-01-18
        private bool IsFurniture(string name)
        {
            //可以在些增加自定义家具类的名称
            if (name.IndexOf("cabinet") >= 0 ||
                name.IndexOf("chuang") >= 0 ||
                name.IndexOf("pillar") >= 0 ||
                name.IndexOf("chugui") >= 0 || //2018-12-04
                name.IndexOf("selfdraw") >= 0 ||//2018-01-31
                name.IndexOf("czyzh") >= 0 ||//2019-02-26
                name.IndexOf("chuanglian") >= 0 ||//2019-02-26
                name.IndexOf("ctgzh") >= 0 ||//2019-02-26
                name.IndexOf("czh") >= 0 ||//2019-02-26
                name.IndexOf("dsgzh") >= 0 ||//2019-02-26
                name.IndexOf("men") >= 0 ||//2019-02-26
                name.IndexOf("sfcjzh") >= 0 ||//2019-02-26
                name.IndexOf("zshua") >= 0
                )
            {
                return true;
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            canceldrawwalljs = -1;//该行要放在cancel的后面.如果正在画墙,则结束 2017-03-14
            axEWdraw1.CancelCommand();
            //
            isseloneent = true;
            double x, y, z;
            x = y = z = 0.0;
            int ent = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                axEWdraw1.ClearSelected();
                SetSymbolWidth(ent);
            }
            isseloneent = false;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            double x, y, z;
            x = y = z = 0;
            int ent = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                axEWdraw1.ClearSelected();
                int enttype = axEWdraw1.GetEntType(ent);
                if (enttype != 50 && enttype == 66)
                {//如果选择的是墙的话,则判断该墙是否是阳台的立面 2017-12-15
                    int areaent = IsAreaWall(ent);
                    if (areaent > 0)
                    {
                        ent = areaent;
                        enttype = 50;
                    }
                }
                if (enttype == 50)
                {
                    string memstr = axEWdraw1.GetEntityUserData(ent);
                    if (IsHaveStrField(memstr, "balcony") >= 0)
                    {
                        Form5 input = new Form5();
                        if (input != null)
                        {
                            input.g_height = g_wallheight;//2018-12-10
                            if (input.g_balconyalldatas == null)
                                input.g_balconyalldatas = new Form5.CExistBalcony();
                            if (input.orgwintypes == null)
                                input.orgwintypes = new ArrayList();
                            if (input.pipepts == null)
                                input.pipepts = new ArrayList();
                            GetBalconyAllData(ent, ref input.g_balconyalldatas, ref input.orgwintypes, ref input.pipepts);
                            input.g_height = g_wallheight;
                            input.g_thickness = g_wallthickness;
                            input.g_areaent = ent;
                            double minx, miny, minz, maxx, maxy, maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            axEWdraw1.GetEntBoundingBox(input.g_areaent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            input.g_cx = (minx + maxx) / 2.0;
                            input.g_cy = (miny + maxy) / 2.0;
                            input.g_cz = (minz + maxz) / 2.0;
                            if (input.ShowDialog() == DialogResult.OK)
                            {
                                if (input.memstr.Length > 0)
                                {
                                    ArrayList tlist = new ArrayList();
                                    GetPtsFromSolid(ent, ref tlist);
                                    axEWdraw1.SetEntityUserData(ent, input.memstr);
                                    SetPtsToSolid(ent, ref tlist);
                                    tlist.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            axEWdraw1.ZoomALL();
            axEWdraw1.ToDrawOrbit();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (g_rbuttonselent > 0)
            {
                MessageBox.Show(g_rbuttonselent.ToString());
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (g_rbuttonselent > 0)
            {

                if (axEWdraw1.IsGroup(g_rbuttonselent))
                {
                    MessageBox.Show(axEWdraw1.GetGroupName(g_rbuttonselent));
                }
                else
                    MessageBox.Show("这不是一个组");
            }
        }


        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (g_rbuttonselent > 0)
            {
                if (axEWdraw1.IsGroup(g_rbuttonselent))
                {
                    string grpname = axEWdraw1.GetGroupName(g_rbuttonselent);
                    if (IsFurniture(grpname))
                    {
                        isdrawrotate = true;
                        axEWdraw1.ToDrawAxisRotate();
                    }
                }
                else
                    MessageBox.Show("这不是一个组");
            }

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            setdrawaxis = 1;
            if (axEWdraw1.IsEndCommand())
            {
                if (!isdrawaxismove)
                {
                    double x, y, z;
                    x = y = z = 0.0;
                    int ent = 0;
                    int selsize = axEWdraw1.GetSelectEntSize();
                    if (selsize > 0)
                    {
                        ent = axEWdraw1.GetSelectEnt(0);
                        if (ent > 0)
                        {
                            if (axEWdraw1.IsGroup(ent))
                            {
                                string grpname = axEWdraw1.GetGroupName(ent);
                                if (IsFurniture(grpname))
                                {
                                    isdrawaxismove = true;
                                    axEWdraw1.SetDrawAxis(setdrawaxis);
                                    axEWdraw1.ToDrawAxisMove();
                                }
                            }
                        }

                    }
                }
                else
                {
                    axEWdraw1.CancelCommand();
                    isdrawaxismove = false;
                    setdrawaxis = -1;
                }
            }//

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            setdrawaxis = 0;
            if (axEWdraw1.IsEndCommand())
            {
                if (!isdrawaxismove)
                {
                    double x, y, z;
                    x = y = z = 0.0;
                    int ent = 0;
                    int selsize = axEWdraw1.GetSelectEntSize();
                    if (selsize > 0)
                    {
                        ent = axEWdraw1.GetSelectEnt(0);
                        if (ent > 0)
                        {
                            if (axEWdraw1.IsGroup(ent))
                            {
                                string grpname = axEWdraw1.GetGroupName(ent);
                                if (IsFurniture(grpname))
                                {
                                    isdrawaxismove = true;
                                    axEWdraw1.SetDrawAxis(setdrawaxis);
                                    axEWdraw1.ToDrawAxisMove();
                                }
                            }
                        }

                    }
                }
                else
                {
                    axEWdraw1.CancelCommand();
                    isdrawaxismove = false;
                    setdrawaxis = -1;
                }
            }//
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            setdrawaxis = 2;
            if (axEWdraw1.IsEndCommand())
            {
                if (g_viewmode == 3 || g_viewmode == 2)
                {
                    if (!isdrawaxismove)
                    {
                        double x, y, z;
                        x = y = z = 0.0;
                        int ent = 0;
                        int selsize = axEWdraw1.GetSelectEntSize();
                        if (selsize > 0)
                        {
                            ent = axEWdraw1.GetSelectEnt(0);
                            if (ent > 0)
                            {
                                if (axEWdraw1.IsGroup(ent))
                                {
                                    string grpname = axEWdraw1.GetGroupName(ent);
                                    if (IsFurniture(grpname))
                                    {
                                        isdrawaxismove = true;
                                        axEWdraw1.SetDrawAxis(setdrawaxis);
                                        axEWdraw1.ToDrawAxisMove();
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        axEWdraw1.CancelCommand();
                        isdrawaxismove = false;
                        setdrawaxis = -1;
                    }
                }
                else MessageBox.Show("请在三维模式下使用该功能.");
            }//

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (axEWdraw1.IsEndCommand())
            {
                if (g_viewmode == 3 || g_viewmode == 2)
                {
                    if (!isdrawaxismove)
                    {
                        double x, y, z;
                        x = y = z = 0.0;
                        int ent = 0;
                        int selsize = axEWdraw1.GetSelectEntSize();
                        if (selsize > 0)
                        {
                            ent = axEWdraw1.GetSelectEnt(0);
                            if (ent > 0)
                            {
                                if (axEWdraw1.IsGroup(ent))
                                {
                                    string grpname = axEWdraw1.GetGroupName(ent);
                                    if (IsFurniture(grpname))
                                    {
                                        isdrawaxismove = true;
                                        axEWdraw1.ToDrawAxisMove();
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        isdrawaxismove = false;
                    }
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (IsHaveRoom())
            {
                canceldrawwalljs = -1;
                if (!axEWdraw1.IsEndCommand())
                {
                    if (lastsym > 0)
                    {
                        axEWdraw1.Delete(lastsym);
                        lastsym = 0;
                    }
                }
                axEWdraw1.CancelCommand();
                //2018-01-31
                Form6 selfdraw = new Form6();
                if (selfdraw.ShowDialog() == DialogResult.Yes)
                {
                    double cx, cy, cz;
                    cx = cy = cz = 0.0;
                    if (zPubFun.zPubFunLib.g_istriallimit)//2020-04-20
                    {
                        MessageBox.Show("试用版不支持该功能");
                    }
                    else
                    {
                        axEWdraw1.Screen2Coordinate(axEWdraw1.Width / 2, axEWdraw1.Height / 2, ref cx, ref cy, ref cz);
                        axEWdraw1.InsertGroupFromFile("selfdraw.ewd", new object[] { cx, cy, cz });
                        if (File.Exists("selfdraw.ewd"))
                            File.Delete("selfdraw.ewd");
                    }
                }
            }
            else MessageBox.Show("请先绘制户型");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //2018-01-31
            double x1, y1, z1, x2, y2, z2;
            x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            double dist = axEWdraw1.GetDist(ref x1, ref y1, ref z1, ref x2, ref y2, ref z2);
            if (dist > 0)
                label1.Text = "测试的距离为:" + String.Format("{0:f3}", dist);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            g_isquit = true;
            axEWdraw1.UnloadPlugIn();
            //2018-01-31
            if (!axEWdraw1.IsEndCommand())
                axEWdraw1.CancelCommand();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            axEWdraw1.ExpObjWithTexture("F:\\testexpobj\\test2\\模型\\测试\\test.obj");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_2(object sender, EventArgs e)
        {
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            if (lastsym > 0)
            {
                axEWdraw1.Delete(lastsym);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            axEWdraw1.OpenEwd("test.ewd");
        }

        //判断线是否重合2018-08-01 
        private bool IsOverDrawLine(double sx, double sy, double ex, double ey, ref ArrayList pts)
        {
            int ptslen = pts.Count / 3 - 1;
            for (int i = 0; i < ptslen; i++)
            {
                if (axEWdraw1.IsLineSegOver(sx, sy, ex, ey,
                                        (double)pts[i * 3], (double)pts[i * 3 + 1], (double)pts[(i + 1) * 3], (double)pts[(i + 1) * 3 + 1])
                    )
                {
                    return true;
                }
            }
            return false;
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            //2018-08-03
            double atx, aty, atz, eyex, eyey, eyez;
            atx = aty = atz = eyex = eyey = eyez = 0.0;
            timer6.Enabled = false;
            if (g_viewmode == 3 || g_viewmode == 2)
            {
                axEWdraw1.GetCamera(ref atx, ref aty, ref atz, ref eyex, ref eyey, ref eyez);
                //在此处可以判断摄像机位置是否发现变化,如果发生变化,则进入相应操作即可
                //...此处的代码要尽可能的精简,快捷
                //
            }
            timer6.Enabled = true;
        }

        private int Import3DsObj(string filePath, string grpname,
                        double xsite, double ysite, double zsite,
                        double xoff, double yoff, double yoff1, double zoff,
                        double length, double thickness, double height,/*这是门符号定义的长,厚度,高度*/
                        bool ismid = false /*这个参数表示是否是外开门(如果是true),默认是内开门,这个和符号的初始相同*/
                        )
        {
            int impsize = 0;
            int ent0 = 0;
            double x, y, z, x1, y1, z1, x2, y2, z2;
            double minx, miny, minz, maxx, maxy, maxz;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw1.ClearIDBuffer();
            ////重新创建一个带3DS网面的组
            int group = 0;
            //
            int orggrp = 0;

            //1.导入3DS模型,注意这里的3DS模型最好是最简模型
            //2.导入的3DS模型,一定要都是前项
            //3.导入3DS模型的同时,要将各中偏移量从数据库(或文件)中对应模型读出.如:xoff,yoff...等.
            //int st = System.Environment.TickCount;
            impsize = axEWdraw1.Imp3DSWithTexture(filePath);
            //创建组
            for (int i = 1; i <= impsize; i++)
            {
                ent0 = axEWdraw1.GetImpEntID(i);
                axEWdraw1.AddIDToBuffer(ent0);
            }
            group = axEWdraw1.MakeGroup(grpname, new object[] { xsite, ysite, zsite });
            //
            //得到组的包围盒的拐点 3：底面左上角三维坐标
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            axEWdraw1.MoveTo(group, new object[] { x, y, z }, new object[] { xsite, ysite, zsite });
            //
            axEWdraw1.GetGroupBndPt(group, 0, ref x1, ref y1, ref z1);//得到组的包围盒的拐点 0：底面左下角三维坐标
            axEWdraw1.GetGroupBndPt(group, 6, ref x2, ref y2, ref z2);//得到组的包围盒的拐点 6：顶面右上角三维坐标

            double midx = (x2 + x1) / 2.0;
            double midy = (y2 + y1 + yoff) / 2.0;
            double midz = 0;
            //设置中线中点为组的基点(可以根据分类的不同而不同.窗与门都可以以中线中点)
            axEWdraw1.GetGroupBndPt(group, 3, ref x, ref y, ref z);
            if (ismid)
                axEWdraw1.SetGroupInsPt(group, new object[] { midx, midy, midz });//设置组的基点？应该是用来吸附用的
            else
                axEWdraw1.SetGroupInsPt(group, new object[] { x, y, z });//设置组的基点？应该是用来吸附用的
            axEWdraw1.ClearIDBuffer();
            //计算轴向缩放系数
            double xscale = 0.0;
            if (length > 0.001)
                xscale = length / ((x2 - x1) - xoff * 2.0);
            else
                xscale = 1.0;

            double yscale = 0.0;
            if (thickness > 0.001)
                yscale = thickness / ((y2 - y1) - yoff - yoff1 * 2.0);
            else
                yscale = 1.0;

            double zscale = 0.0;
            if (height > 0.001)
                zscale = height / ((z2 - z1) - zoff);
            else
                zscale = 1.0;

            axEWdraw1.MeshScaleByXYZAxis3(group, xscale, yscale, zscale);
            //
            axEWdraw1.SetGroupPlaneByBoxFace(group, 2);//将包围盒的某一面，设置为组的吸附面(方向面) 1:前2：后3：左4：右5：上6：下
            return group;
        }
        //开始渲染功能
        private void StartRender(object sender, EventArgs e)
        {
            if (g_viewmode == 3)
            {
                if (zPubFun.zPubFunLib.g_istriallimit)//2020-04-20
                {
                    MessageBox.Show("试用版不支持该功能");
                    return;
                }
                label1.Text = "渲染进行中...";
                label1.Refresh();
                axEWdraw1.CancelCommand();
                axEWdraw1.ClearIDBuffer();
                if (axEWdraw1.GetIDBufferSize() > 0)
                {
                    for (int i = 0; i < axEWdraw1.GetIDBufferSize(); i++)
                    {
                        g_roofids.Add(axEWdraw1.GetIDBuffer(i));
                    }
                }
                axEWdraw1.ClearIDBuffer();
                string pngfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\test.png";
                axEWdraw1.StartExtRender(1330, 750, pngfile);
                label1.Text = "";
            }
            else
                MessageBox.Show("请在漫游状态下选择视角后再渲染");
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView4.SelectedItems)  //选中项遍历 
            {
                if (lvi.Index >= 0)
                {
                    if (curflooritems.Count > 0 && lvi.Index < curflooritems.Count)
                    {
                        label1.Text = "选择要设置的地面:";
                        double x, y, z;
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        x = y = z = 0.0;
                        int areaent = axEWdraw1.GetOneEntSel(ref x, ref y, ref z);
                        if (areaent > 0)
                        {
                            string memo = axEWdraw1.GetEntityUserData(areaent);
                            if (IsHaveStrField(memo, "area") >= 0)
                            {
                                axEWdraw1.GetEntBoundingBox(areaent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                //计算地板的重复率
                                double ureapt = (maxx - minx) / ((MyTextureItem)curflooritems[lvi.Index]).texwidth;
                                double vreapt = (maxy - miny) / ((MyTextureItem)curflooritems[lvi.Index]).texheight;
                                string file;
                                string bumpfile;
                                if (((MyTextureItem)curflooritems[lvi.Index]).texpath.Length > 0)
                                {
                                    file = tmodepath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texpath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texfilename;
                                    bumpfile = tmodepath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texpath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texbump;
                                }
                                else
                                {
                                    file = tmodepath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texfilename;
                                    bumpfile = tmodepath + "\\" + ((MyTextureItem)curflooritems[lvi.Index]).texbump;
                                }

                                axEWdraw1.SetEntTexture(areaent, file, 1, 1, ureapt, vreapt, 0, 0);
                                //设置扩展数据
                                axEWdraw1.SetExtMatl(areaent, ((MyTextureItem)curflooritems[lvi.Index]).matlinx, 240, 240, 240, file, bumpfile, 0, ureapt, vreapt);
                                if (((MyTextureItem)curflooritems[lvi.Index]).state1 == 1)
                                {//反射
                                    SetProStrFromEnt(areaent, "reflect", 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        //读取家具类的总类 2018-11-03
        private void ReadTotalClassItems(int inx)
        {
            totalclassitems.Clear();
            string name = "titems" + inx.ToString() + ".dat";
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string strReadline;
            while ((strReadline = read.ReadLine()) != null)
            {
                string[] str = strReadline.Split(';');
                if (str.Length > 0)
                {
                    if (!(str[0][0] == '/' && str[0][1] == '/'))
                    {//
                        MYImageItem aitem = new MYImageItem();
                        aitem.classname = str[0];
                        aitem.itemname = str[1];
                        aitem.filename = str[2];
                        aitem.imagefile = str[3];
                        aitem.width = zPubFun.zPubFunLib.CStr2Double(str[6]);
                        aitem.depth = zPubFun.zPubFunLib.CStr2Double(str[7]);
                        aitem.height = zPubFun.zPubFunLib.CStr2Double(str[8]);
                        aitem.ridz = zPubFun.zPubFunLib.CStr2Double(str[9]);
                        aitem.walldist = zPubFun.zPubFunLib.CStr2Double(str[10]);
                        aitem.state1 = Convert.ToInt32(str[11]);
                        aitem.state2 = Convert.ToInt32(str[12]);
                        totalclassitems.Add(aitem);
                    }
                }
                // strReadline即为按照行读取的字符串
            }
            if (totalclassitems.Count > 0)
            {
                switch (inx)
                {
                    case 0:
                        {
                            listView1.Items.Clear();
                            imageList1.Images.Clear();
                            listView1.ShowItemToolTips = true;
                        }
                        break;
                    case 1:
                        {
                            listView3.Items.Clear();
                            imageList3.Images.Clear();
                            listView3.ShowItemToolTips = true;
                        }
                        break;
                }
                for (int i = 0; i < totalclassitems.Count; i++)
                {
                    System.Drawing.Image image;
                    try
                    {
                        image = Image.FromFile(tmodepath + "\\" + ((MYImageItem)totalclassitems[i]).imagefile);
                        switch (inx)
                        {
                            case 0:
                                {

                                    imageList1.Images.Add(((MYImageItem)totalclassitems[i]).classname, image);
                                    ListViewItem aitem = listView1.Items.Add(((MYImageItem)totalclassitems[i]).classname, i);
                                    aitem.ToolTipText = ((MYImageItem)totalclassitems[i]).classname;
                                }
                                break;
                            case 1:
                                {

                                    imageList3.Images.Add(((MYImageItem)totalclassitems[i]).classname, image);
                                    ListViewItem aitem = listView3.Items.Add(((MYImageItem)totalclassitems[i]).classname, i);
                                    aitem.ToolTipText = ((MYImageItem)totalclassitems[i]).classname;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            read.Close();
            fs.Close();
            switch (inx)
            {
                case 0:
                    totalclassitems0.Clear();
                    totalclassitems0.AddRange(totalclassitems);
                    break;
                case 1:
                    totalclassitems1.Clear();
                    totalclassitems1.AddRange(totalclassitems);
                    break;

            }
            totalclassitems.Clear();
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView5.SelectedItems)  //选中项遍历 
            {
                if (lvi.Index >= 0)
                {
                    if (lvi.Index >= 0 && lvi.Index < curwallitems.Count)
                    {
                        string imagefile;
                        string imagebump;
                        if (((MyTextureItem)curwallitems[lvi.Index]).texpath.Length > 0)
                        {
                            imagefile = tmodepath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texpath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texfilename;
                            imagebump = tmodepath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texpath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texbump;
                        }
                        else
                        {
                            imagefile = tmodepath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texfilename;
                            imagebump = tmodepath + "\\" + ((MyTextureItem)curwallitems[lvi.Index]).texbump;
                        }

                        axEWdraw1.SetTexFaceOfSolid(imagefile, 0, 0, 1, 1, 1, 1, 0);
                        axEWdraw1.SetTexFaceTextureUVSize(true, ((MyTextureItem)curwallitems[lvi.Index]).texwidth, ((MyTextureItem)curwallitems[lvi.Index]).texheight);
                        axEWdraw1.SetTexFaceExtMatl(((MyTextureItem)curwallitems[lvi.Index]).matlinx, 230, 230, 230, imagefile, imagebump, 0, 1, 1);
                        axEWdraw1.ToDrawTexFaceOfSolid();
                    }
                }
            }
        }

        /*根据索引返回类的名称 2018-11-08
        * 
        */
        private string IndexToItemName(int tinx, int inx)
        {
            switch (tinx)
            {
                case 0:
                    {
                        if (inx >= 0 && inx < totalclassitems0.Count)
                        {
                            return ((MYImageItem)totalclassitems0[inx]).classname;
                        }
                    }
                    break;
                case 1:
                    {
                        if (inx >= 0 && inx < totalclassitems1.Count)
                        {
                            return ((MYImageItem)totalclassitems1[inx]).classname;
                        }

                    }
                    break;

            }
            return "";
        }


        /*读入子项 2018-11-08 
        * cname 是子项的名称,如果是空的话,则为初始化所有子项
        * list  如果cname不为空,则在list中返回cname对应的子项集
        */
        private void ReadSubClassItems(string cname, ref ArrayList list)
        {
            if (cname.Length == 0)
            {
                subclassitems.Clear();
                string name = "tsubitems.dat";
                FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(fs, Encoding.Default);
                string strReadline;
                while ((strReadline = read.ReadLine()) != null)
                {
                    string[] str = strReadline.Split(';');
                    if (str.Length > 0)
                    {
                        if (!(str[0][0] == '/' && str[0][1] == '/'))
                        {//
                            MYImageItem aitem = new MYImageItem();
                            aitem.classname = str[0];
                            aitem.itemname = str[1];
                            aitem.filename = str[2];
                            aitem.imagefile = str[3];
                            aitem.itemdir = str[4];
                            aitem.itemsmoothdir = str[5];
                            aitem.width = zPubFun.zPubFunLib.CStr2Double(str[6]);
                            aitem.depth = zPubFun.zPubFunLib.CStr2Double(str[7]);
                            aitem.height = zPubFun.zPubFunLib.CStr2Double(str[8]);
                            aitem.ridz = zPubFun.zPubFunLib.CStr2Double(str[9]);
                            aitem.walldist = zPubFun.zPubFunLib.CStr2Double(str[10]);
                            aitem.state1 = Convert.ToInt32(str[11]);
                            aitem.state2 = Convert.ToInt32(str[12]);
                            subclassitems.Add(aitem);
                        }
                    }
                    // strReadline即为按照行读取的字符串
                }
                read.Close();
                fs.Close();
            }
            else
            {
                for (int i = 0; i < subclassitems.Count; i++)
                {
                    if (((MYImageItem)subclassitems[i]).classname == cname)
                    {
                        list.Add(((MYImageItem)subclassitems[i]));
                    }
                }
            }
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool isvalid = false;
            if (!textBox1.Focused || !textBox1.Visible || !textBox1.Enabled)
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                        textBox1.Text = "0";
                        isvalid = true;
                        break;
                    case Keys.D1:
                        textBox1.Text = "1";
                        isvalid = true;
                        break;
                    case Keys.D2:
                        textBox1.Text = "2";
                        isvalid = true;
                        break;
                    case Keys.D3:
                        textBox1.Text = "3";
                        isvalid = true;
                        break;
                    case Keys.D4:
                        textBox1.Text = "4";
                        isvalid = true;
                        break;
                    case Keys.D5:
                        textBox1.Text = "5";
                        isvalid = true;
                        break;
                    case Keys.D6:
                        textBox1.Text = "6";
                        isvalid = true;
                        break;
                    case Keys.D7:
                        textBox1.Text = "7";
                        isvalid = true;
                        break;
                    case Keys.D8:
                        textBox1.Text = "8";
                        isvalid = true;
                        break;
                    case Keys.D9:
                        textBox1.Text = "9";
                        isvalid = true;
                        break;
                    case Keys.Decimal:
                    case Keys.OemPeriod:
                        textBox1.Text = ".";
                        isvalid = true;
                        break;
                    case Keys.Subtract:
                    case Keys.OemMinus:
                        textBox1.Text = "-";
                        isvalid = true;
                        break;
                    case Keys.ProcessKey://2021-09-15
                        textBox1.Text = "";
                        isvalid = true;
                        break;
                }
                if (isvalid && (isdrawpolyline || isdrawabsorb || isdrawbalcony || isdrawmove || isdrawrotate))//2021-09-15
                {
                    textBox1.Visible = true;
                    textBox1.Top = g_dimposy + axEWdraw1.Top - textBox1.Height;
                    textBox1.Left = axEWdraw1.Left + g_dimposx - 10;
                    textBox1.Focus();
                    //设置光标的位置到文本尾 
                    textBox1.Select(textBox1.TextLength, 0);
                    //滚动到控件光标处 
                    textBox1.ScrollToCaret();
                    old_dimposx = g_dimposx;
                    old_dimposy = g_dimposy;
                    isenterchar = true;
                }
            }

        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            timer7.Enabled = false;
            if (!axEWdraw1.IsEndCommand())
            {
                if (isdrawpolyline)
                {
                    if (g_hand != GetForegroundWindow())
                    {
                        ActivateWindow(g_hand);
                    }
                }
            }
            timer7.Enabled = true;
        }

        private void axEWdraw1_MouseMoveEvent(object sender, AxEWDRAWLib._DAdrawEvents_MouseMoveEvent e)
        {
            if (!axEWdraw1.Focused && !textBox1.Visible)
                axEWdraw1.Focus();
        }

        /*根据子项集创建listview的子项 2018-11-08 
        * cname 是上层项的名称
        * list 则在list中返回cname对应的子项集
        */
        private void CreatSubClassItems(int inx, ref ArrayList list, int level = 0)
        {
            if (list.Count > 0)
            {
                switch (inx)
                {
                    case 0:
                        {


                            listView1.Items.Clear();
                            imageList1.Images.Clear();
                            listView1.ShowItemToolTips = true;

                            if (level == 1)
                            {
                                System.Drawing.Image image;
                                image = Image.FromFile(tmodepath + "\\" + "上层.png");
                                imageList1.Images.Add("上层", image);
                                ListViewItem aitem = listView1.Items.Add("上层", 0);
                                aitem.ToolTipText = "上层";
                            }
                        }
                        break;
                    case 1:
                        {
                            listView3.Items.Clear();
                            imageList3.Images.Clear();
                            listView3.ShowItemToolTips = true;
                            if (level == 1)
                            {
                                System.Drawing.Image image;
                                image = Image.FromFile(tmodepath + "\\" + "上层.png");
                                imageList3.Images.Add("上层", image);
                                ListViewItem aitem = listView3.Items.Add("上层", 0);
                                aitem.ToolTipText = "上层";
                            }
                        }
                        break;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    System.Drawing.Image image;
                    try
                    {
                        string filename;
                        if (level == 1)
                            filename = tmodepath + "\\" + ((MYImageItem)list[i]).itemdir + "\\" + Path.GetFileNameWithoutExtension(((MYImageItem)list[i]).imagefile) + "\\" + ((MYImageItem)list[i]).imagefile;
                        else
                            filename = tmodepath + "\\" + ((MYImageItem)list[i]).imagefile;
                        image = Image.FromFile(filename);
                        switch (inx)
                        {
                            case 0:
                                {
                                    imageList1.Images.Add(((MYImageItem)list[i]).classname, image);
                                    ListViewItem aitem = null;
                                    if (level == 0)
                                        aitem = listView1.Items.Add(((MYImageItem)list[i]).classname, i);
                                    else
                                        aitem = listView1.Items.Add(((MYImageItem)list[i]).classname, i + 1);
                                    aitem.ToolTipText = ((MYImageItem)list[i]).classname;
                                }
                                break;
                            case 1:
                                {
                                    imageList3.Images.Add(((MYImageItem)list[i]).classname, image);
                                    ListViewItem aitem = null;
                                    if (level == 0)
                                        aitem = listView3.Items.Add(((MYImageItem)list[i]).classname, i);
                                    else
                                        aitem = listView3.Items.Add(((MYImageItem)list[i]).classname, i + 1);
                                    aitem.ToolTipText = ((MYImageItem)list[i]).classname;
                                }
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void DrawObj(int inx, ref ArrayList list)//button24_Click(object sender, EventArgs e)
        {
            canceldrawwalljs = -1;//如果正在画墙,则结束 2017-03-14
            if (lastsym > 0)
            {
                axEWdraw1.Delete(lastsym);
                lastsym = 0;
            }
            axEWdraw1.CancelCommand();
            listView3.SelectedItems.Clear();
            //
            double vx, vy, vz;
            vx = vy = vz = 0.0;
            axEWdraw1.GetViewProj(ref vx, ref vy, ref vz);
            if (Math.Abs(vx) < 0.001 && Math.Abs(vy) < 0.001 && Math.Abs(vz) > 0.001)
            {
                double minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = 0.0;
                isdrawabsorb = true;//2017-09-20
                isdrawmove = false;//2017-09-20
                axEWdraw1.ClearSelected();//2016-09-08
                //从列表中查找3DS文件 2018-11-09
                string filename = tmodepath + "\\" + ((MYImageItem)list[inx - 1]).itemdir + "\\" + Path.GetFileNameWithoutExtension(((MYImageItem)list[inx - 1]).filename) + "\\" + ((MYImageItem)list[inx - 1]).filename;
                string grpname = Path.GetFileNameWithoutExtension(((MYImageItem)list[inx - 1]).filename);
                lastsym = Import3DsObj(filename, grpname, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                axEWdraw1.GetEntBoundingBox(lastsym, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                double xlength = maxx - minx;
                double ylength = maxy - miny;
                double zlength = maxz - minz;
                string memstr = "xlength:" + String.Format("{0:f3}", xlength) + ";" + "ylength:" + String.Format("{0:f3}", ylength) + ";" + "zlength:" + String.Format("{0:f3}", zlength) + ";";
                axEWdraw1.SetEntityUserData(lastsym, memstr);
                if (Math.Abs(((MYImageItem)list[inx - 1]).ridz) > 0.001)
                    axEWdraw1.EnableAbsorbHigh(true, Math.Abs(((MYImageItem)list[inx - 1]).ridz));//2017-09-30
                else
                    axEWdraw1.EnableAbsorbHigh(true, 0);//2017-09-30
                axEWdraw1.EnableCheckAbsorbIns(false);
                axEWdraw1.EnableAbsorbDepth(false, 0);
                axEWdraw1.SetORTHO(false);
                axEWdraw1.EnableOrthoHVMode(false);
                axEWdraw1.CancelCommand();
                axEWdraw1.AddOrRemoveSelect(lastsym);
                axEWdraw1.ToDrawAbsorb();
            }
            else
                MessageBox.Show("请在平面下绘制");
        }

        //根据组名获得3DS简模的文件名 2018-11-14
        string GetSimpleModeFromGrpName(string grpname, ref ArrayList list, ref string oname)
        {
            string filename = "";
            string[] strarr = grpname.Split('_');
            if (strarr.Length == 3)
            {
                string name = strarr[2];
                oname = name;
                string sname = name + ".3ds";
                string path = "";
                for (int i = 0; i < list.Count; i++)
                {
                    if (sname == ((MYImageItem)list[i]).filename)
                    {
                        path = ((MYImageItem)list[i]).itemdir;
                        break;
                    }
                }
                if (tmodepath.Length > 0)
                {
                    filename = tmodepath + "\\" + path + "\\" + name + "\\" + name + ".3ds";
                    return filename;
                }
            }
            else if (strarr.Length == 2)
            {
                string name = strarr[1];
                oname = name;
                string sname = name + ".3ds";
                string path = "";
                for (int i = 0; i < list.Count; i++)
                {
                    if (sname == ((MYImageItem)list[i]).filename)
                    {
                        path = ((MYImageItem)list[i]).itemdir;
                        break;
                    }
                }
                if (tmodepath.Length > 0)
                {
                    filename = tmodepath + path + "\\" + name + "\\" + name + ".3ds";
                    return filename;
                }
            }
            return filename;
        }
        //根据组名获得3DS简模的宽深高离地 2018-11-14
        bool GetWTHRFromGrpName(string grpname, ref ArrayList list,
                                ref double width,
                                ref double thickness,
                                ref double height,
                                ref double ridz
                                )
        {
            string sname = grpname + ".3ds";
            for (int i = 0; i < list.Count; i++)
            {
                if (sname == ((MYImageItem)list[i]).filename)
                {
                    width = ((MYImageItem)list[i]).width;
                    thickness = ((MYImageItem)list[i]).depth;
                    if (thickness < 0.001)
                    {
                        thickness = g_wallthickness;
                    }
                    height = ((MYImageItem)list[i]).height;
                    ridz = ((MYImageItem)list[i]).ridz;
                    return true;
                }
            }
            return false;
        }
        //读取Texture类 2018-11-03
        private void ReadFloorTextureItems(int inx, ref ArrayList list)
        {
            totalclassitems.Clear();
            string name = "textures" + inx.ToString() + ".dat";
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string strReadline;
            while ((strReadline = read.ReadLine()) != null)
            {
                string[] str = strReadline.Split(';');
                if (str.Length > 0)
                {
                    if (!(str[0][0] == '/' && str[0][1] == '/'))
                    {//
                        MyTextureItem aitem = new MyTextureItem();
                        aitem.texname = str[0];
                        aitem.teximagname = str[1];
                        aitem.texfilename = str[1];
                        aitem.texbump = str[2];
                        aitem.texpath = str[3];
                        aitem.texwidth = Convert.ToInt32(str[4]);
                        aitem.texheight = Convert.ToInt32(str[5]);
                        aitem.matlinx = Convert.ToInt32(str[6]);
                        aitem.state1 = Convert.ToInt32(str[7]);
                        aitem.state2 = Convert.ToInt32(str[8]);
                        list.Add(aitem);
                    }
                }
            }
            read.Close();
            fs.Close();
            //显示到列表内
            if (list.Count > 0)
            {
                listView4.Items.Clear();
                imageList4.Images.Clear();
                listView4.ShowItemToolTips = true;
                for (int i = 0; i < list.Count; i++)
                {
                    System.Drawing.Image image;
                    try
                    {
                        if (((MyTextureItem)list[i]).texpath.Length > 0)
                            image = Image.FromFile(tmodepath + "\\" + ((MyTextureItem)list[i]).texpath + "\\" + ((MyTextureItem)list[i]).texfilename);
                        else
                            image = Image.FromFile(tmodepath + "\\" + ((MyTextureItem)list[i]).texfilename);
                        imageList4.Images.Add(((MyTextureItem)list[i]).texname, image);
                        ListViewItem aitem = listView4.Items.Add(((MyTextureItem)list[i]).texname, i);

                        aitem.ToolTipText = ((MyTextureItem)list[i]).texname;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }//

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && (GDesignMode == 1 || GDesignMode == -1 || GDesignMode == 3 || GDesignMode == 4))
            {
                int tcode = ((SNode)e.Node.Tag).code;
                g_tcode = tcode;
                int type = ((SNode)e.Node.Tag).type;
                g_tname = ((SNode)e.Node.Tag).name;
                if (type == 1)
                {
                    //调用模型
                    axEWdraw2.DeleteAll();
                    axEWdraw2.CreatePlugIn(tcode);
                    //纹理
                    int entsize = axEWdraw2.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        int ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            //注意这里的名称是保留了原来三维家具库中从属的柜体名称,也可以根据情况换成自己的.
                            axEWdraw2.SetPlankCName(ent, g_tname);//2019-12-16
                        }
                    }

                    axEWdraw2.ZoomALL();
                    axEWdraw2.SetViewCondition(8);
                    //
                    int size = axEWdraw2.GetPlugInParameterSize(tcode);
                    double val = 0;
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < size; i++)
                    {
                        string name = axEWdraw2.GetPlugInParameter(i, ref val);
                        dataGridView1.Rows.Add();
                        int count = dataGridView1.Rows.Count;
                        dataGridView1[0, count - 1].Value = name;
                        dataGridView1[1, count - 1].Value = val.ToString("0.00");
                    }
                    if (dataGridView1.ColumnCount > 0)
                    {
                        dataGridView1.Columns[0].Width = 120;
                        dataGridView1.Columns[1].Width = 73;
                    }
                }
            }
            else if (GDesignMode == 2)
            {
                g_plankcode = (int)e.Node.Tag;
                if (g_plankcode >= 0)
                {
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < g_yxbs.Count; i++)
                    {
                        if (i == g_plankcode)
                        {
                            MyYXB ayxb = (MyYXB)g_yxbs[i];
                            for (int j = 0; j < ayxb.m_paras.Count; j++)
                            {
                                if (((MyYXBPara)ayxb.m_paras[j]).m_name != "宽度" &&
                                    ((MyYXBPara)ayxb.m_paras[j]).m_name != "深度"
                                    )
                                {
                                    dataGridView1.Rows.Add();
                                    int count = dataGridView1.Rows.Count;
                                    dataGridView1[0, count - 1].Value = ((MyYXBPara)ayxb.m_paras[j]).m_name;
                                    dataGridView1[1, count - 1].Value = ((MyYXBPara)ayxb.m_paras[j]).m_val;
                                }

                            }
                        }
                    }
                    
                    MakePreYXB(g_plankcode);
                    
                }
            }

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.CurrentCell;
            if (GDesignMode == 1 || GDesignMode == -1 || GDesignMode == 3)
            {
                axEWdraw2.SetPlugInParameter(cell.RowIndex, zPubFun.zPubFunLib.CStr2Double(cell.Value));
                //调用模型
                axEWdraw2.DeleteAll();
                if (g_tcode > 0)
                {
                    axEWdraw2.CreatePlugIn(g_tcode);
                    //纹理
                    if (g_selctid > 0)
                        axEWdraw2.ClearShare();
                    int entsize = axEWdraw2.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        int ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            if (g_selctid > 0)
                            {
                                axEWdraw2.AddEntToShare(ent);
                            }
                        }
                    }
                    axEWdraw2.ZoomALL();
                    //判断是否是已创建抽屉的参数
                    if (g_selctid > 0)
                    {
                        double minX, minY, minZ, maxX, maxY, maxZ;
                        minX = minY = minZ = maxX = maxY = maxZ = 0;
                        int winx, winy;
                        winx = winy = 0;
                        //得到选中抽屉所在空间体的位置 2020-01-13
                        GetSpaceByGrp(g_selctid, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ, ref winx, ref winy);
                        //2019-12-30
                        g_sminx = minX;
                        g_sminy = minY;
                        g_sminz = minZ;
                        g_smaxx = maxX;
                        g_smaxy = maxY;
                        g_smaxz = maxZ;
                        double octminz, octmaxz;
                        octminz = octmaxz = 0.0;
                        //得到抽屉的在空间体的的上下界 2010-01-13
                        GetCTMinMaxZ(g_selctid, g_sminx, g_sminy, g_sminz,
                                     g_smaxx, g_smaxy, g_smaxz, g_ctinsz, ref octminz, ref octmaxz);

                        CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
                        //
                        if (Math.Abs(Math.Abs(g_ctmaxx - g_ctminx) - Math.Abs(g_smaxx - g_sminx)) > 0.001)
                        {
                            double nwidth = Math.Abs(g_smaxx - g_sminx);
                            double ndepth = Math.Abs(g_smaxy - g_sminy);
                            double nheight = 0;
                            //判断高自适应
                            if (Math.Abs(octmaxz - octminz) > 0 && checkBox3.Checked)
                                nheight = Math.Abs(octmaxz - octminz) - g_ctbottom - g_cttop;//计算高度(减低与顶空) 2020-01-14
                            else
                                nheight = Math.Abs(g_smaxz - g_sminz);
                            if (checkBox2.Checked && !checkBox4.Checked && !checkBox3.Checked)//只有宽度,默认
                                UpdateCT(nwidth, g_ctheight, g_ctdepth, true);
                            else if (!checkBox2.Checked && checkBox4.Checked && !checkBox3.Checked)//只有深度
                                UpdateCT(g_ctwidth, g_ctheight, ndepth, true);
                            else if (!checkBox2.Checked && !checkBox4.Checked && checkBox3.Checked)//只有高度
                                UpdateCT(g_ctwidth, nheight, g_ctdepth, true);
                            else if (checkBox2.Checked && checkBox4.Checked && !checkBox3.Checked)//宽度与深度
                                UpdateCT(nwidth, g_ctheight, ndepth, true);
                            else if (!checkBox2.Checked && checkBox4.Checked && checkBox3.Checked)//深度与高度
                                UpdateCT(g_ctwidth, nheight, ndepth, true);
                            else if (checkBox2.Checked && !checkBox4.Checked && checkBox3.Checked)//高度与宽度
                                UpdateCT(nwidth, nheight, g_ctheight, true);
                            else if (checkBox2.Checked && checkBox4.Checked && checkBox3.Checked)//长,宽,深都自适应
                                UpdateCT(nwidth, nheight, ndepth, true);
                        }

                        double oinsx, oinsy, oinsz;
                        oinsx = oinsy = oinsz = 0.0;
                        string grpname = "grp_ct_" + g_tcode.ToString();
                        int grpid = 0;
                        axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
                        if (grpid > 0)
                        {
                            g_isctins = false;//2020-01-03
                            CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
                            axEWdraw3.GetGroupInsPt(g_selctid, ref oinsx, ref oinsy, ref oinsz);
                            //
                            if (Math.Abs(maxX - minX) > 0)
                            {
                                double insx, insy, insz;
                                insx = insy = insz = 0.0;
                                axEWdraw3.EyeLineInsAABBs(winx, winy, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                                g_ctinsx = insx;
                                g_ctinsy = insy;
                                g_ctinsz = insz;
                                axEWdraw3.ClearSelected();
                            }
                            //
                            axEWdraw3.SetGroupInsPt(grpid, new object[] { g_ctbx, g_ctby, g_ctbz });
                            axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);
                            //判断是否自适应高 2020-01-14
                            if (checkBox3.Checked)
                                axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { oinsx, oinsy, octminz + g_ctbottom });
                            else
                                axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { oinsx, oinsy, oinsz });
                            TreeNode node = treeView1.SelectedNode;
                            SaveCTParamaters(grpid, node.Text);
                            axEWdraw3.Delete(g_selctid);
                            g_selctid = 0;
                            axEWdraw3.ClearSelected();
                        }
                        g_selctid = 0;
                        axEWdraw3.ClearSelected();
                    }
                    else if (g_selfjiaid > 0)
                    {
                        double minX, minY, minZ, maxX, maxY, maxZ;
                        minX = minY = minZ = maxX = maxY = maxZ = 0;
                        int winx, winy;
                        winx = winy = 0;
                        //得到选中抽屉所在空间体的位置 2020-01-13
                        GetSpaceByGrp(g_selfjiaid, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ, ref winx, ref winy);
                        //2019-12-30
                        g_sminx = minX;
                        g_sminy = minY;
                        g_sminz = minZ;
                        g_smaxx = maxX;
                        g_smaxy = maxY;
                        g_smaxz = maxZ;
                        TreeNode node = treeView1.SelectedNode;
                        SaveCTParamaters(g_selfjiaid, node.Text);
                        string str = GetProStrFromEnt_N3(g_selfjiaid, "宽度");
                        string str1 = GetProStrFromEnt_N3(g_selfjiaid, "深度");
                        double tmpwidth = GetDblfromProStr(str);
                        double tmpdepth = GetDblfromProStr(str1);
                        if (checkBox2.Checked)
                            tmpwidth = g_smaxx - g_sminx;
                        if (checkBox4.Checked)
                            tmpdepth = g_smaxy - g_sminy;
                        UpdateFjiaWidth(g_selfjiaid,
                            g_sminx,//2020-04-13
                            tmpwidth,
                            tmpdepth
                            );
                    }
                    else if (g_selboguid > 0)//2020-03-04
                    {
                        double minX, minY, minZ, maxX, maxY, maxZ;
                        minX = minY = minZ = maxX = maxY = maxZ = 0;
                        int winx, winy;
                        winx = winy = 0;
                        //得到选中抽屉所在空间体的位置 2020-01-13
                        GetSpaceByGrp(g_selboguid, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ, ref winx, ref winy);
                        //2019-12-30
                        g_sminx = minX;
                        g_sminy = minY;
                        g_sminz = minZ;
                        g_smaxx = maxX;
                        g_smaxy = maxY;
                        g_smaxz = maxZ;
                        TreeNode node = treeView1.SelectedNode;
                        SaveCTParamaters(g_selboguid, node.Text);
                        UpdateBOGUWidth(g_selboguid,
                            g_sminx,//2020-04-13
                            g_smaxx - g_sminx,
                            g_smaxy - g_sminy,
                            g_smaxz - g_sminz
                            );
                    }
                }
            }
            else if (GDesignMode == 2)
            {
                if (g_plankcode >= 0)
                {
                    MakePreYXB(g_plankcode, zPubFun.zPubFunLib.CStr2Double(cell.Value));
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void ReadWallTextureItems(int inx, ref ArrayList list)
        {
            totalclassitems.Clear();
            string name = "textures" + inx.ToString() + ".dat";
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs, Encoding.Default);
            string strReadline;
            while ((strReadline = read.ReadLine()) != null)
            {
                string[] str = strReadline.Split(';');
                if (str.Length > 0)
                {
                    if (!(str[0][0] == '/' && str[0][1] == '/'))
                    {//
                        MyTextureItem aitem = new MyTextureItem();
                        aitem.texname = str[0];
                        aitem.teximagname = str[1];
                        aitem.texfilename = str[1];
                        aitem.texbump = str[2];
                        aitem.texpath = str[3];
                        aitem.texwidth = Convert.ToInt32(str[4]);
                        aitem.texheight = Convert.ToInt32(str[5]);
                        aitem.matlinx = Convert.ToInt32(str[6]);
                        aitem.state1 = Convert.ToInt32(str[7]);
                        aitem.state2 = Convert.ToInt32(str[8]);
                        list.Add(aitem);
                    }
                }
            }
            read.Close();
            fs.Close();
            //显示到列表内
            if (list.Count > 0)
            {
                listView5.Items.Clear();
                imageList5.Images.Clear();
                listView5.ShowItemToolTips = true;
                for (int i = 0; i < list.Count; i++)
                {
                    System.Drawing.Image image;
                    try
                    {
                        //Font font;
                        if (((MyTextureItem)list[i]).texpath.Length > 0)
                            image = Image.FromFile(tmodepath + "\\" + ((MyTextureItem)list[i]).texpath + "\\" + ((MyTextureItem)list[i]).texfilename);
                        else
                            image = Image.FromFile(tmodepath + "\\" + ((MyTextureItem)list[i]).texfilename);
                        imageList5.Images.Add(((MyTextureItem)list[i]).texname, image);
                        ListViewItem aitem = listView5.Items.Add(((MyTextureItem)list[i]).texname, i);

                        aitem.ToolTipText = ((MyTextureItem)list[i]).texname;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }//
        private void InitCabinet()
        {
            axEWdraw1.Visible = false;
            axEWdraw3.Visible = true;
            listView2.Visible = false;
            listView7.Visible = true;//2019-12-02
            //
            checkBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            //
            button16.Visible = true;
            button19.Visible = true;
            button20.Visible = true;
            button23.Visible = true;//2020-01-25
            button24.Visible = true;
            button25.Visible = true;
            button26.Visible = true;
            //
            button27.Visible = true;
            button28.Visible = true;
            button29.Visible = true;
            button30.Visible = true;
            button31.Visible = true;
            button42.Visible = true;//2020-03-24
            button43.Visible = true;
            button44.Visible = true;
            //
            label1.Text = "创建或打开订单后,\"双击\"左侧预览区可向工作区增加柜体.";
            Text = "家具拆单";
            axEWdraw2.EnableWJWhenCreatePlugIn(false);
            treeView1.Focus();//2019-12-27
            groupBox1.Visible = true;//2020-03-24
            groupBox2.Visible = true;
            groupBox3.Visible = true;
            axEWdraw2.DisableRightMenu(true);
            axEWdraw2.DisableDelKey(true);
        }
        //绘制户型初始
        private void InitDrawWall()
        {
            axEWdraw1.Visible = true;
            axEWdraw3.Visible = false;
            listView7.Visible = false;//2019-12-02
            listView2.Visible = true;
            checkBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            //
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            button17.Visible = false;
            button18.Visible = false;
            button19.Visible = false;
            button20.Visible = false;
            button23.Visible = false;//2020-01-25
            button24.Visible = false;
            button25.Visible = false;
            button26.Visible = false;
            button27.Visible = false;
            button28.Visible = false;
            button29.Visible = false;
            button30.Visible = false;
            button31.Visible = false;
            button32.Visible = false;//2020-02-17
            button42.Visible = false;
            button43.Visible = false;
            button44.Visible = false;
            groupBox1.Visible = false;//2020-03-24
            groupBox2.Visible = false;
            groupBox3.Visible = false;

            //
            label1.Text = "绘制墙体后,增加家具.";
            Text = "绘制户型";
        }
        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 4)
            {
                //初始化家具库
                InitCabinet();
            }
            else
            {
                //初始画墙
                InitDrawWall();
            }
        }

        private void axEWdraw2_DblClick(object sender, EventArgs e)
        {
            if (g_orderid == null)
            {
                Form8 input = new Form8();
                if (input.ShowDialog() == DialogResult.OK)
                {
                    g_orderid = input.label2.Text;
                    g_orderdate = input.dateTimePicker1.Text;
                    g_orderoutdtate = input.dateTimePicker2.Text;
                    g_orderconnecter = input.textBox3.Text;
                    g_orderphone = input.textBox4.Text;
                    g_orderuser = input.textBox1.Text;//2020-02-28
                    axEWdraw3.AddFurnitureOrder(input.label2.Text);
                    axEWdraw3.DeleteAll();
                    Text = "家具拆单---" + "订单号:" + g_orderid;
                    //
                    int entsize = axEWdraw2.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        int ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501 && enttype != 502 && enttype != 503)
                            axEWdraw2.AddEntToShare(ent);
                    }

                    string grpname = "grp_" + g_tcode.ToString();
                    bool isperspect = false;
                    if (axEWdraw3.GetEntSize() == 0)
                        isperspect = true;
                    double minx, miny, minz, maxx, maxy, maxz;
                    minx = miny = minz = maxx = maxy = maxz = 0.0;
                    axEWdraw3.GetAllBoundingBox(ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    if (axEWdraw3.GetEntSize() == 0)
                    {
                        maxx = maxy = maxz = 0;
                    }
                    int grpid = 0;
                    axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
                    if (grpid > 0)
                    {
                        axEWdraw3.SetGroupInsPt(grpid, new object[] { 0, 0, 0 });
                        axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);
                        axEWdraw3.MoveTo(grpid, new object[] { 0, 0, 0 }, new object[] { maxx, maxy, 0 });
                    }

                    if (axEWdraw3.GetEntSize() > 0)
                        axEWdraw3.SetPerspectiveMode(true);
                    //
                    axEWdraw2.ClearShare();
                    axEWdraw3.ZoomALL();//2020-03-24

                }
            }
        }

        private void axEWdraw2_ViewMouseDown(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseDownEvent e)
        {
        }

        private void axEWdraw3_ViewMouseUp(object sender, AxEWDRAWLib._DAdrawEvents_ViewMouseUpEvent e)
        {
            if (e.button == 2)
            {//2020-03-13
                if (axEWdraw3.IsBeginUndo())
                    axEWdraw3.EndUndo();
            }
            if (e.button == 1)
            {
                if (isFirstClick)
                {
                    isFirstClick = false;
                    if (!g_isctins && GDesignMode == 1)//非插入模式
                    {
                        int entselsize = axEWdraw3.GetSelectEntSize();
                        if (entselsize > 0)
                        {
                            int ent = axEWdraw3.GetSelectEnt(0);
                            if (axEWdraw3.IsGroup(ent))
                            {
                                string grpname = axEWdraw3.GetGroupName(ent);
                                if (grpname.IndexOf("_ct_") >= 0)
                                {
                                    GetCTParamaters(ent);
                                    g_selct = true;//2020-01-09
                                    //取得抽屉的信息
                                    g_selctid = ent;
                                    axEWdraw3.GetGroupInsPt(ent,ref g_selctx,ref g_selcty,ref g_selctz);
                                    treeView1.Focus();
                                    treeView1.Refresh();
                                }else g_selctid = 0;
                            }else g_selctid = 0;
                        }
                        else g_selctid = 0;

                    }
                    else if (!g_isfjiains && GDesignMode == 3)//非插入模式
                    {
                        int entselsize = axEWdraw3.GetSelectEntSize();
                        if (entselsize > 0)
                        {
                            int ent = axEWdraw3.GetSelectEnt(0);
                            if (axEWdraw3.IsGroup(ent))
                            {
                                string grpname = axEWdraw3.GetGroupName(ent);
                                if (grpname.IndexOf("_fjia_") >= 0)
                                {
                                    ArrayList paralist = null;
                                    GetCTParamaters_N(ent, ref paralist);
                                    g_selfjia = true;//2020-01-09
                                    //取得抽屉的信息
                                    g_selfjiaid = ent;
                                    axEWdraw3.GetGroupInsPt(ent, ref g_selfjiax, ref g_selfjiay, ref g_selfjiaz);
                                    treeView1.Focus();
                                    treeView1.Refresh();
                                }
                                else g_selfjiaid = 0;
                            }
                            else g_selfjiaid = 0;
                        }
                        else g_selfjiaid = 0;
                    }
                    else if (!g_isboguins && GDesignMode == 4)
                    {
                        int entselsize = axEWdraw3.GetSelectEntSize();
                        if (entselsize > 0)
                        {
                            int ent = axEWdraw3.GetSelectEnt(0);
                            if (axEWdraw3.IsGroup(ent))
                            {
                                string grpname = axEWdraw3.GetGroupName(ent);
                                if (grpname.IndexOf("_bogu_") >= 0)
                                {
                                    ArrayList paralist = null;
                                    GetCTParamaters_N(ent, ref paralist);
                                    g_selbogu = true;//2020-01-09
                                    //取得抽屉的信息
                                    g_selboguid = ent;
                                    axEWdraw3.GetGroupInsPt(ent, ref g_selbogux, ref g_selboguy, ref g_selboguz);
                                    treeView1.Focus();
                                    treeView1.Refresh();
                                }
                                else g_selboguid = 0;
                            }
                            else g_selboguid = 0;
                        }
                        else g_selboguid = 0;
                    }
                    // 开始计算双击的时间.
                    if (axEWdraw3.IsEndCommand() && !issingplankmove)//2020-02-02
                        timer8.Start();
                    else if (!isFirstClick)
                        isFirstClick = true;
                    if (insertorgplankid > 0)
                    {
                        double minX, minY, minZ, maxX, maxY, maxZ;
                        minX = minY = minZ = maxX = maxY = maxZ = 0;
                        axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                        axEWdraw3.GetNearestCabinetSpace(e.xWin, e.yWin, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                        axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                        if (Math.Abs(maxX - minX) > 0)
                        {
                            axEWdraw3.BeginUndo(true);
                            int tmpent = axEWdraw3.PutPlankToSpace(insertorgplankid, 18, minX, minY, minZ, maxX, maxY, maxZ);
                            axEWdraw3.EndUndo();
                            axEWdraw3.SetEntTexture(tmpent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            insertorgplankid = -1;
                            g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();
                        }
                        axEWdraw3.ClearSelected();
                    }
                    else
                    {
                        if (GDesignMode == 1 && g_isctins)
                        {//抽屉状态
                            g_isctins = false;//2020-01-03
                            double minX, minY, minZ, maxX, maxY, maxZ;
                            minX = minY = minZ = maxX = maxY = maxZ = 0;
                            axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                            axEWdraw3.GetNearestCabinetSpace(e.xWin, e.yWin, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                            axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                            //2019-12-30
                            g_sminx = minX;
                            g_sminy = minY;
                            g_sminz = minZ;
                            g_smaxx = maxX;
                            g_smaxy = maxY;
                            g_smaxz = maxZ;
                            //
                            if (Math.Abs(maxX - minX) > 0)
                            {
                                double insx,insy,insz;
                                insx = insy = insz = 0.0;
                                axEWdraw3.EyeLineInsAABBs(e.xWin, e.yWin, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                                g_ctinsx = insx;
                                g_ctinsy = insy;
                                g_ctinsz = insz;
                                axEWdraw3.ClearSelected();
                                g_timerproc = 1;//2020-01-31
                                timer9.Enabled = true;
                            }
                        }
                        else if (GDesignMode == 3 && g_isfjiains)
                        {//2020-02-21
                            g_isfjiains = false;//2020-01-03
                            double minX, minY, minZ, maxX, maxY, maxZ;
                            minX = minY = minZ = maxX = maxY = maxZ = 0;
                            axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                            axEWdraw3.GetNearestCabinetSpace(e.xWin, e.yWin, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                            axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                            //2019-12-30
                            g_sminx = minX;
                            g_sminy = minY;
                            g_sminz = minZ;
                            g_smaxx = maxX;
                            g_smaxy = maxY;
                            g_smaxz = maxZ;
                            //
                            if (Math.Abs(maxX - minX) > 0)
                            {
                                double insx, insy, insz;
                                insx = insy = insz = 0.0;
                                axEWdraw3.EyeLineInsAABBs(e.xWin, e.yWin, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                                g_fjiainsx = insx;
                                g_fjiainsy = insy;
                                g_fjiainsz = insz;
                                axEWdraw3.ClearSelected();
                                g_timerproc = 6;//2020-01-31
                                timer9.Enabled = true;
                            }
                        }else if (GDesignMode == 4 && g_isboguins)
                        {//2020-03-04
                            g_isboguins = false;//2020-01-03
                            double minX, minY, minZ, maxX, maxY, maxZ;
                            minX = minY = minZ = maxX = maxY = maxZ = 0;
                            axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                            axEWdraw3.GetNearestCabinetSpace(e.xWin, e.yWin, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                            axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                            //2019-12-30
                            g_sminx = minX;
                            g_sminy = minY;
                            g_sminz = minZ;
                            g_smaxx = maxX;
                            g_smaxy = maxY;
                            g_smaxz = maxZ;
                            //
                            if (Math.Abs(maxX - minX) > 0)
                            {
                                double insx, insy, insz;
                                insx = insy = insz = 0.0;
                                axEWdraw3.EyeLineInsAABBs(e.xWin, e.yWin, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                                g_boguinsx = insx;
                                g_boguinsy = insy;
                                g_boguinsz = insz;
                                axEWdraw3.ClearSelected();
                                g_timerproc = 7;//2020-03-04
                                timer9.Enabled = true;
                            }
                        }
                        else if (g_yitongmode > 0)
                        {
                            g_winx = e.xWin;
                            g_winy = e.yWin;
                            switch (g_yitongmode)
                            {
                                case 1:
                                    {//修改
                                        g_timerproc = 4;//2020-01-13
                                        timer9.Enabled = true;
                                    }
                                    break;
                                case 2:
                                    {//插入
                                        g_timerproc = 5;//2020-02-13
                                        timer9.Enabled = true;
                                    }
                                    break;
                            }
                        }else if (isCabinetDesign)
                        {
                            if (axEWdraw3.IsEndCommand())
                            {
                                int tmpselsize = axEWdraw3.GetSelectEntSize();
                                if (tmpselsize == 1)
                                {
                                    int tmpent = axEWdraw3.GetSelectEnt(0);
                                    if (tmpent > 0)
                                    {
                                        double w, h, t, a;
                                        string name, cname;
                                        w = h = t = a = 0.0;
                                        name = "";
                                        cname = "";
                                        g_editpent = tmpent;
                                        if (!axEWdraw3.IsGroup(tmpent))
                                        {
                                            g_editptype = axEWdraw3.GetPlankType(tmpent);
                                            axEWdraw3.GetPlankWHTA(tmpent, ref w, ref h, ref t, ref a, ref name, ref cname);
                                            if (groupBox1.Visible)
                                            {
                                                label5.Text = name;
                                                textBox3.Text = w.ToString("0.0");
                                                textBox4.Text = h.ToString("0.0");
                                                textBox5.Text = t.ToString("0.0");
                                                textBox9.Text = (a / 1000000.0).ToString("0.00");
                                                switch (g_editptype)
                                                {
                                                    case 0:
                                                        {//层板
                                                            label6.Text = "宽度";
                                                            label7.Text = "深度";
                                                        }
                                                        break;
                                                    case 1:
                                                        {//竖板
                                                            label6.Text = "高度";
                                                            label7.Text = "深度";
                                                        }
                                                        break;
                                                    case 2:
                                                        {//面板
                                                            label6.Text = "宽度";
                                                            label7.Text = "高度";
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else g_editptype = -1;
                                }
                            }
                        }
                        else if (!isCabinetDesign)
                        {
                            if (axEWdraw3.IsEndCommand())
                            {
                                int tmpselsize = axEWdraw3.GetSelectEntSize();
                                if (tmpselsize == 1)
                                {
                                    int tmpent = axEWdraw3.GetSelectEnt(0);
                                    if (axEWdraw3.IsGroup(tmpent))
                                    {
                                        double minx,miny,minz,maxx,maxy,maxz;
                                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                                        axEWdraw3.GetEntBoundingBox(tmpent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                                        groupBox1.Text = "柜体信息";
                                        textBox3.Text = (maxx - minx).ToString("0.00");
                                        textBox4.Text = (maxy - miny).ToString("0.00");
                                        label4.Text = "柜体名称";
                                        label5.Text = axEWdraw3.GetEntName(tmpent);
                                        g_editpent = tmpent;//2020-03-24
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //确认是否强应双击
                    if (milliseconds < SystemInformation.DoubleClickTime)
                    {
                        isDoubleClick = true;
                    }
                }
                //判断是否是单轴板材移动 2020-01-31
                if (issingplankmove)
                {
                    int selsize = axEWdraw3.GetSelectEntSize();
                    if (selsize > 0)
                    {
                        g_axismoveid = axEWdraw3.GetSelectEnt(0);
                        if (g_axismoveid > 0)
                        {
                            double minx,miny,minz,maxx,maxy,maxz;
                            minx = miny = minz = maxx = maxy = maxz = 0.0;
                            axEWdraw3.GetEntBoundingBox(g_axismoveid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            if (g_axismoveplank == null)
                                g_axismoveplank = new CPlank();
                            g_axismoveplank.minx = minx;
                            g_axismoveplank.miny = miny;
                            g_axismoveplank.minz = minz;
                            g_axismoveplank.maxx = maxx;
                            g_axismoveplank.maxy = maxy;
                            g_axismoveplank.maxz = maxz;
                            g_axismoveplank.id = g_axismoveid;
                            g_timerproc = 2;
                            timer9.Enabled = true;
                        }
                    }
                }
            }
            else if(e.button == 2)
            {
                if(issingplankmove)
                {
                    issingplankmove = false;//2019-12-06
                    axEWdraw3.EnableMoveSinglePlank(false);
                    axEWdraw3.EnableSinglePlankAxisMove(false);
                }
                g_yitongmode = 0;//插入衣通 2020-02-14
                g_yitongorg = 0;//2020-02-14
            }
            axEWdraw3.Focus();//2010-01-13
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            milliseconds += 20;
            // 判断双击的时间限制.
            if (milliseconds >= 300)//双击的时间在300毫秒以内
            {
                timer8.Stop();
                if (isDoubleClick)
                {
                    if (!axEWdraw3.IsEndCommand() || issingplankmove)
                    {
                        if (!isFirstClick)
                            isFirstClick = true;
                        return;
                    }
                    if (!isCabinetDesign)
                    {
                        if(g_orderid != null)
                        {
                            if (axEWdraw3.GetSelectEntSize() > 0)
                            {
                                int ent = axEWdraw3.GetSelectEnt(0);
                                if (ent > 0)
                                {
                                    //
                                    if (axEWdraw3.IsGroup(ent))//2020-03-08
                                    {
                                        string grpname = axEWdraw3.GetGroupName(ent);
                                        string[] strarr = grpname.Split('_');
                                        if (strarr.Length <= 2)//2020-03-18
                                        {
                                            axEWdraw3.Width = 925;
                                            button40.Visible = false;//2020-03-19
                                            button35.Visible = true;//2020-03-24
                                            button37.Enabled = true;//2020-03-24
                                            button12.Visible = true;
                                            button13.Visible = true;
                                            button14.Visible = true;
                                            button15.Visible = true;
                                            button16.Visible = true;
                                            button17.Visible = true;
                                            button18.Visible = true;
                                            button32.Visible = true;//2020-02-17
                                            button23.Visible = false;//2020-01-25
                                            button24.Visible = false;
                                            button25.Visible = false;
                                            button26.Visible = false;
                                            //
                                            isCabinetDesign = true;
                                            InitOrgPlanks(true);
                                            RestoreHideObjs();//2020-01-23
                                            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                            axEWdraw3.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                            //
                                            GetCabinetBndBeforeModify(ent, ref g_beforebox, ref g_beforelist,ref g_beforelist1);//2020-03-23
                                            axEWdraw3.SetCabinetDesignMode(ent, true);
                                            g_cabinetdesignid = ent;
                                            axEWdraw3.SetViewCondition(5);
                                            axEWdraw3.ClearSelected();
                                            g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();//空间体
                                            button22.PerformClick();//2020-02-01
                                            button21.PerformClick();//2020-02-01
                                            //
                                            button27.Left -= 100;
                                            button28.Left -= 100;
                                            button29.Left -= 100;
                                            button30.Left -= 100;
                                            button31.Left -= 100;
                                            button42.Left -= 100;//2020-03-24
                                            button43.Left -= 100;
                                            button44.Left -= 100;
                                            listView7.Items[3].ImageIndex = 8;
                                        }
                                    }
                                }
                            }//
                        }
                        else
                        {
                            MessageBox.Show("请先创建或打开订单,再增加柜体到工作区.");
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    g_selct = false;
                    treeView1.Focus();
                    axEWdraw3.Focus();//2020-01-13
                }

                // 允许响应单击.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }

        private void listView6_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView6.SelectedItems)  //选中项遍历 
            {
                if (lvi.Index >= 0)
                {
                    insertorgplankid = axEWdraw2.GetOrgPlanksID(lvi.Index);
                    break;
                }
            }
        }

        public void ActivateWindow(IntPtr hWnd)
        {
            if (hWnd == GetForegroundWindow())
                return;

            IntPtr ThreadID1 = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            IntPtr ThreadID2 = GetWindowThreadProcessId(hWnd, IntPtr.Zero);

            if (ThreadID1 != ThreadID2)
            {
                AttachThreadInput(ThreadID1, ThreadID2, 1);
                SetForegroundWindow(hWnd);
                AttachThreadInput(ThreadID1, ThreadID2, 0);
            }
            else
            {
                SetForegroundWindow(hWnd);
            }

            if (IsIconic(hWnd))
            {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }
            else
            {
                ShowWindowAsync(hWnd, SW_SHOWNORMAL);
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
                        if (allitems[i].Begin.IndexOf("\r") < 0)
                            allitems[i].Begin = allitems[i].Begin.Replace("\n", "\r\n");
                    }
                    if (allitems[i].End.Length > 0)
                    {
                        if (allitems[i].End.IndexOf("\r") < 0)
                            allitems[i].End = allitems[i].End.Replace("\n", "\r\n");
                    }

                    if (i == 0)
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

        private void listView7_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView7.SelectedItems)  //选中项遍历 
            {
                switch (lvi.Index)
                {
                    case 0:
                        {
                            HideWJ();//消隐五金 2020-03-20
                            Form8 input = new Form8();
                            if (input.ShowDialog() == DialogResult.OK)
                            {
                                g_orderid = input.label2.Text;
                                g_orderdate = input.dateTimePicker1.Text;
                                g_orderoutdtate = input.dateTimePicker2.Text;
                                g_orderconnecter = input.textBox3.Text;
                                g_orderphone = input.textBox4.Text;
                                g_orderuser = input.textBox1.Text;//2020-02-28
                                axEWdraw3.AddFurnitureOrder(input.label2.Text);
                                axEWdraw3.DeleteAll();
                                Text = "家具拆单---" + "订单号:" + g_orderid;
                                ResetInitJiaJuKu();//2020-04-15
                            }
                        }
                        break;
                    case 1:
                        {
                            HideWJ();//消隐五金 2020-03-20
                            if (isCabinetDesign)
                            {//如果已经是设计状态,则关闭设计状态
                                axEWdraw3.ClearAllUndoRedo();
                                isCabinetDesign = false;
                                //
                                if (GDesignMode == 2 || GDesignMode == 1 || GDesignMode == 3)//2020-02-17
                                {
                                    GDesignMode = -1;
                                    ResetInitJiaJuKu();
                                }
                                button35.Visible = false;//2020-03-24
                                button37.Enabled = false;//2020-03-24
                                //恢复消陷的对象
                                RestoreHideObjs();//2020-01-23
                                //
                                if (g_cabinetdesignid > 0)
                                    axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                                axEWdraw3.SetViewCondition(8);
                                //
                                button12.Visible = false;
                                button13.Visible = false;
                                button14.Visible = false;
                                button15.Visible = false;
                                button17.Visible = false;
                                button18.Visible = false;
                                button23.Visible = true;//2020-01-25
                                button24.Visible = true;
                                button25.Visible = true;
                                button26.Visible = true;
                                button32.Visible = false;//2020-02-17
                                //
                                button27.Left += 100;
                                button28.Left += 100;
                                button29.Left += 100;
                                button30.Left += 100;
                                button31.Left += 100;
                                button42.Left += 100;//2020-03-24
                                button43.Left += 100;
                                button44.Left += 100;

                                //
                                button40.Visible = true;
                                button38.Visible = false;
                                //
                                InitOrgPlanks(false);
                                listView7.Items[3].ImageIndex = 2;
                            }
                            Form9 input = new Form9();
                            if (input.ShowDialog() == DialogResult.OK)
                            {
                                if (input.label2.Text.Length > 0)
                                {
                                    //
                                    g_orderid = input.label2.Text;
                                    g_orderdate = input.dateTimePicker1.Text;
                                    g_orderoutdtate = input.dateTimePicker2.Text;
                                    g_orderconnecter = input.textBox3.Text;
                                    g_orderphone = input.textBox4.Text;
                                    g_orderuser = input.textBox7.Text;//2020-02-28
                                    //
                                    Text = "家具拆单---" + "订单号:" + g_orderid;
                                    axEWdraw3.AddFurnitureOrder(input.label2.Text);
                                    string filename = ConvertPrjIDToEWDPath("", false);
                                    if (filename.Length > 0)
                                    {
                                        axEWdraw3.OpenEwd(filename);
                                        axEWdraw3.SetPerspectiveMode(true);
                                        axEWdraw3.SetViewCondition(8);
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        {//保存
                            SaveWork();
                        }
                        break;
                    case 3:
                        {//柜体设计
                            HideWJ();//消隐五金 2020-03-20
                            RestoreExplodeCabinet();//2020-04-20
                            if (isCabinetDesign)
                            {//如果已经是设计状态 2019-12-03
                                label1.Text = "";
                                axEWdraw3.ClearAllUndoRedo();
                                isCabinetDesign = false;
                                //
                                if (GDesignMode == 2 || GDesignMode == 1 || GDesignMode == 3)//2020-02-17
                                {
                                    GDesignMode = -1;
                                    ResetInitJiaJuKu();
                                }
                                button35.Visible = false;//2020-03-24
                                button37.Enabled = false;//2020-03-24
                                button40.Visible = true;//2020-03-19
                                //恢复消陷的对象
                                RestoreHideObjs();//2020-01-23
                                //
                                if (g_cabinetdesignid > 0)
                                {
                                    axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                                    axEWdraw3.GetEntBoundingBox(g_cabinetdesignid, ref g_afterbox.minx, ref g_afterbox.miny, ref g_afterbox.minz, ref g_afterbox.maxx, ref g_afterbox.maxy, ref g_afterbox.maxz);
                                    MoveCabinetAfterModify(g_cabinetdesignid, ref g_beforebox, ref g_afterbox, ref g_beforelist, ref g_beforelist1);//2020-03-23
                                }
                                axEWdraw3.SetViewCondition(8);
                                //
                                button12.Visible = false;
                                button13.Visible = false;
                                button14.Visible = false;
                                button15.Visible = false;
                                button17.Visible = false;
                                button18.Visible = false;
                                button23.Visible = true;//2020-01-25
                                button24.Visible = true;
                                button25.Visible = true;
                                button26.Visible = true;
                                button32.Visible = false;//2020-02-17
                                button38.Visible = false;
                                //
                                button27.Left += 100;
                                button28.Left += 100;
                                button29.Left += 100;
                                button30.Left += 100;
                                button31.Left += 100;
                                button42.Left += 100;//2020-03-24
                                button43.Left += 100;
                                button44.Left += 100;
                                //
                                InitOrgPlanks(false);
                                listView7.Items[3].ImageIndex = 2;
                            }
                            else
                            {
                                if (axEWdraw3.GetSelectEntSize() > 0)
                                {
                                    if (g_orderid != null)//2020-03-18
                                    {
                                        if (axEWdraw3.GetSelectEntSize() > 0)
                                        {
                                            int ent = axEWdraw3.GetSelectEnt(0);
                                            if (ent > 0)
                                            {
                                                //
                                                if (axEWdraw3.IsGroup(ent))//2020-03-08
                                                {
                                                    string grpname = axEWdraw3.GetGroupName(ent);
                                                    string[] strarr = grpname.Split('_');
                                                    if (strarr.Length <= 2)//2020-03-18
                                                    {
                                                        axEWdraw3.Width = 925;
                                                        button40.Visible = false;//2020-03-19
                                                        button35.Visible = true;//2020-03-24
                                                        button37.Enabled = true;//2020-03-24
                                                        groupBox1.Visible = true;
                                                        groupBox2.Visible = true;
                                                        groupBox3.Visible = true;

                                                        button12.Visible = true;
                                                        button13.Visible = true;
                                                        button14.Visible = true;
                                                        button15.Visible = true;
                                                        button16.Visible = true;
                                                        button17.Visible = true;
                                                        button18.Visible = true;
                                                        button32.Visible = true;//2020-02-17
                                                        button23.Visible = false;//2020-01-25
                                                        button24.Visible = false;
                                                        button25.Visible = false;
                                                        button26.Visible = false;
                                                        
                                                        //
                                                        isCabinetDesign = true;
                                                        InitOrgPlanks(true);
                                                        RestoreHideObjs();//2020-01-23
                                                        double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
                                                        orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
                                                        axEWdraw3.GetGroupAxis(ent, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                                                        //
                                                        GetCabinetBndBeforeModify(ent, ref g_beforebox, ref g_beforelist, ref g_beforelist1);//2020-03-23
                                                        axEWdraw3.SetCabinetDesignMode(ent, true);
                                                        g_cabinetdesignid = ent;
                                                        axEWdraw3.SetViewCondition(5);
                                                        axEWdraw3.ClearSelected();
                                                        g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();//空间体
                                                        button22.PerformClick();//2020-02-01
                                                        button21.PerformClick();//2020-02-01
                                                        //
                                                        button27.Left -= 100;
                                                        button28.Left -= 100;
                                                        button29.Left -= 100;
                                                        button30.Left -= 100;
                                                        button31.Left -= 100;
                                                        button42.Left -= 100;//2020-03-24
                                                        button43.Left -= 100;
                                                        button44.Left -= 100;

                                                        listView7.Items[3].ImageIndex = 8;
                                                    }
                                                }
                                            }
                                        }//
                                    }
                                    else
                                    {
                                        MessageBox.Show("请先创建或打开订单,再增加柜体到工作区.");
                                    }
                                }
                                else label1.Text = "先选择要修改的柜体,再使用该功能";

                            }
                        }
                        break;
                    case 4:
                        {
                            HideWJ();//消隐五金 2020-03-20
                            RestoreExplodeCabinet();//2020-04-20
                            //恢复消陷的对象
                            RestoreHideObjs();//2020-01-23

                            //
                            if (isCabinetDesign)
                            {//如果已经是设计状态 2019-12-03
                                axEWdraw3.ClearAllUndoRedo();
                                isCabinetDesign = false;
                                //
                                if (GDesignMode == 2 || GDesignMode == 1 || GDesignMode == 3)//2020-02-17
                                {
                                    GDesignMode = -1;
                                    ResetInitJiaJuKu();
                                }
                                button35.Visible = false;//2020-03-24
                                button37.Enabled = false;//2020-03-24
                                //恢复消陷的对象
                                RestoreHideObjs();//2020-01-23
                                //
                                if (g_cabinetdesignid > 0)
                                    axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                                axEWdraw3.SetViewCondition(8);
                                //
                                button12.Visible = false;
                                button13.Visible = false;
                                button14.Visible = false;
                                button15.Visible = false;
                                button17.Visible = false;
                                button18.Visible = false;
                                button23.Visible = true;//2020-01-25
                                button24.Visible = true;
                                button25.Visible = true;
                                button26.Visible = true;
                                button32.Visible = false;//2020-02-17
                                //
                                button27.Left += 100;
                                button28.Left += 100;
                                button29.Left += 100;
                                button30.Left += 100;
                                button31.Left += 100;
                                button42.Left += 100;//2020-03-24
                                button43.Left += 100;
                                button44.Left += 100;

                                //
                                button40.Visible = true;
                                button38.Visible = false;
                                //
                                InitOrgPlanks(false);
                                listView7.Items[3].ImageIndex = 2;
                            }
                            axEWdraw3.DeleteAll();
                            ResetInitJiaJuKu();//2020-04-15
                        }
                        break;
                    case 5:
                        {
                            HideWJ();//消隐五金 2020-03-20
                            RestoreExplodeCabinet();//2020-04-20
                            //恢复消陷的对象
                            RestoreHideObjs();//2020-01-23
                            //
                            if (g_orderid != null)
                            {
                                if (g_orderid.Length > 0)
                                {
                                    string filename = ConvertPrjIDToEWDPath("", false);
                                    if (filename.Length > 0)
                                    {
                                        if (isCabinetDesign)
                                        {//如果已经是设计状态 2019-12-03
                                            if (g_cabinetdesignid > 0)
                                                axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                                            axEWdraw3.SetViewCondition(8);
                                            //
                                            button12.Visible = false;
                                            button13.Visible = false;
                                            button14.Visible = false;
                                            button15.Visible = false;
                                            button16.Visible = true;
                                            button17.Visible = false;
                                            button18.Visible = false;
                                            button23.Visible = true;//2020-01-25
                                            button24.Visible = true;
                                            button25.Visible = true;
                                            button26.Visible = true;
                                            button32.Visible = false;//2020-02-17
                                            //
                                            button27.Left += 100;
                                            button28.Left += 100;
                                            button29.Left += 100;
                                            button30.Left += 100;
                                            button31.Left += 100;
                                            button42.Left += 100;//2020-03-24
                                            button43.Left += 100;
                                            button44.Left += 100;

                                            //
                                            isCabinetDesign = false;
                                            InitOrgPlanks(false);
                                            listView7.Items[3].ImageIndex = 2;
                                            listView7.Refresh();
                                            int entsize = axEWdraw3.GetEntSize();
                                            for (int i = 1; i <= entsize; i++)
                                            {
                                                int ent = axEWdraw3.GetEntID(i);
                                                if (axEWdraw3.IsGroup(ent))
                                                {
                                                    axEWdraw3.ClearIDBuffer();
                                                    axEWdraw3.GetGroupAllIDs(ent);
                                                    int idsize = axEWdraw3.GetIDBufferSize();
                                                    for (int j = 0; j < idsize; j++)
                                                    {
                                                        axEWdraw3.AddEntToShare(axEWdraw3.GetIDBuffer(j));
                                                    }
                                                    axEWdraw3.ClearIDBuffer();
                                                }
                                                else
                                                    axEWdraw3.AddEntToShare(ent);
                                            }
                                            Refresh();
                                        }
                                        else
                                        {
                                            int entsize = axEWdraw3.GetEntSize();
                                            for (int i = 1; i <= entsize; i++)
                                            {
                                                int ent = axEWdraw3.GetEntID(i);
                                                if (axEWdraw3.IsGroup(ent))
                                                {
                                                    axEWdraw3.ClearIDBuffer();
                                                    axEWdraw3.GetGroupAllIDs(ent);
                                                    int idsize = axEWdraw3.GetIDBufferSize();
                                                    for (int j = 0; j < idsize; j++)
                                                    {
                                                        axEWdraw3.AddEntToShare(axEWdraw3.GetIDBuffer(j));
                                                    }
                                                    axEWdraw3.ClearIDBuffer();
                                                }
                                                else
                                                    axEWdraw3.AddEntToShare(ent);
                                            }
                                        }
                                        if (g_orderid == null)
                                        {
                                            MessageBox.Show("创建或打开订单后,再使用该功能.");
                                            return;
                                        }
                                        Form10 fm10 = new Form10();
                                        fm10.m_ewdfilename = filename;
                                        fm10.m_ncfilename = ConvertPrjIDToEWDPath("ewdraw_out.nc",false);
                                        fm10.m_orderid = g_orderid;
                                        fm10.m_orderdate = g_orderdate;
                                        fm10.m_username = g_orderuser;//2020-02-28
                                        if (fm10.ShowDialog() == DialogResult.OK)
                                        {

                                        }
                                        ResetInitJiaJuKu();//2020-04-16
                                    }
                                }
                            }
                            else MessageBox.Show("创建或打开订单后,再使用该功能.");
                        }
                        break;
                    case 6:
                        {
                            Form20 daoju = new Form20();
                            daoju.ShowDialog();
                            SetTInfo();
                        }
                        break;
                    case 7:
                        {
                            //if (zPubFun.zPubFunLib.g_istriallimit)//2020-05-16
                            //{
                            //    MessageBox.Show("试用版不支持该功能");
                            //    return;
                            //}
                            HideWJ();//消隐五金 2020-03-20
                            //恢复消陷的对象
                            RestoreHideObjs();//2020-01-23
                            //
                            //判断是否处理设计状态,如果是设计状态,则先保存文件
                            string filename = ConvertPrjIDToEWDPath("",false);
                            if (filename.Length > 0)
                            {
                                if (isCabinetDesign)
                                {//如果已经是设计状态 2019-12-03
                                    if (g_cabinetdesignid > 0)
                                        axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                                    axEWdraw3.SetViewCondition(8);
                                    //
                                    button12.Visible = false;
                                    button13.Visible = false;
                                    button14.Visible = false;
                                    button15.Visible = false;
                                    button16.Visible = false;
                                    button17.Visible = false;
                                    button18.Visible = false;
                                    button23.Visible = false;//2020-01-25
                                    button24.Visible = false;
                                    button25.Visible = false;
                                    button26.Visible = false;
                                    button32.Visible = false;//2020-02-17
                                    //
                                    button27.Left += 100;
                                    button28.Left += 100;
                                    button29.Left += 100;
                                    button30.Left += 100;
                                    button31.Left += 100;
                                    button42.Left += 100;//2020-03-24
                                    button43.Left += 100;
                                    button44.Left += 100;

                                    //
                                    button23.Visible = true;//2020-01-25
                                    button24.Visible = true;
                                    button25.Visible = true;
                                    button26.Visible = true;
                                    button16.Visible = true;
                                    //
                                    isCabinetDesign = false;
                                    InitOrgPlanks(false);
                                    listView7.Items[3].ImageIndex = 2;
                                    listView7.Refresh();
                                }
                            }
                            else return;//2020-07-01
                            //
                            Form13 fm13 = new Form13();
                            fm13.m_orderid = g_orderid;
                            fm13.m_orderdate = g_orderdate;
                            fm13.m_orderoutdate = g_orderoutdtate;
                            fm13.m_orderconnecter = g_orderconnecter;
                            fm13.m_orderphone = g_orderphone;
                            Point scrpt = PointToScreen(axEWdraw3.Location);
                            scrpt.X -= 50;
                            scrpt.Y -= 100;
                            fm13.Location = scrpt; 
                            if (fm13.ShowDialog() == DialogResult.OK)
                            {
                            }
                            ResetInitJiaJuKu();//2020-04-16
                        }
                        break;
                
                }
            }
        }

         private void MakeDoorBottom(string texfile, double thickness)
        {
            int i = 0;
            int ent = 0;
            string grpname;
            int entsize = axEWdraw1.GetEntSize();
            double x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4;
            x1 = y1 = z1 = x2 = y2 = z2 = x3 = y3 = z3 = x4 = y4 = z4 = 0.0; ;
            for (i = 1; i <= entsize; i++)
            {
                ent = axEWdraw1.GetEntID(i);
                if (axEWdraw1.IsGroup(ent))
                {
                    grpname = axEWdraw1.GetGroupName(ent);
                    if (grpname.IndexOf("door") >= 0 && grpname.IndexOf("symbol") < 0)
                    {
                        //取得底部四个角点的信息
                        axEWdraw1.GetGroupBndPt(ent, 0, ref x1, ref y1, ref z1);
                        axEWdraw1.GetGroupBndPt(ent, 1, ref x2, ref y2, ref z2);
                        axEWdraw1.GetGroupBndPt(ent, 2, ref x3, ref y3, ref z3);
                        axEWdraw1.GetGroupBndPt(ent, 3, ref x4, ref y4, ref z4);
                        axEWdraw1.Clear3DPtBuf();
                        axEWdraw1.AddOne3DVertex(x1, y1, z1);
                        axEWdraw1.AddOne3DVertex(x2, y2, z2);
                        axEWdraw1.AddOne3DVertex(x3, y3, z3);
                        axEWdraw1.AddOne3DVertex(x4, y4, z4);
                        int ent1 = axEWdraw1.PolyLine3D(true);

                        int ent2 = axEWdraw1.EntToFace(ent1, true);
                        int ent3 = 0;
                        if (thickness > 0.001)
                            ent3 = axEWdraw1.Prism(ent2, thickness, new object[] { 0, 0, 1 });
                        else
                            ent3 = ent2;
                        if (texfile.Length > 0)
                        {
                            axEWdraw1.SetEntTexture(ent3, texfile, 1, 1, 1, 1, 0, 0);
                            g_doorbottoms.Add(ent3);
                        }

                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if(isCabinetDesign)
            {
                label1.Text = "选择要插入竖板的位置.";
                insertorgplankid = 21070;
            }
            else
                MessageBox.Show("请在柜体设计状态下使用");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (isCabinetDesign)
            {
                label1.Text = "选择要插入横板的位置.";
                insertorgplankid = 21669;
            }
            else
                MessageBox.Show("请在柜体设计状态下使用");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (isCabinetDesign)
            {
                label1.Text = "选择要设置的板材.";
                //设置板厚
                double x, y, z;
                x = y = z = 0.0;
                int selsize = axEWdraw3.GetSelectEntSize();
                int ent = 0;
                string tval = "";
                double dval = 0.0;
                if (selsize == 1)
                {
                    tval = InputBox("输入板材厚度:", "厚度", "18");
                    dval = zPubFun.zPubFunLib.CStr2Double(tval);
                    ent = axEWdraw3.GetSelectEnt(0);
                    if (ent > 0)
                    {
                        if (dval > 2 && dval < 50)
                        {
                            GetSpaceEnts(ent);//2020-03-07
                            axEWdraw3.CabinetDesignPlankThickness(ent, dval);
                            SetCompByPlank(ent);
                        }
                    }
                }
                else
                {
                    label1.Text = "请选择要设置厚度的板材";
                    ent = axEWdraw3.GetOneEntSel(ref x, ref y, ref z);
                    if (ent > 0)
                    {
                        tval = InputBox("输入板材厚度:", "厚度", "18");
                        dval = zPubFun.zPubFunLib.CStr2Double(tval);
                        if (dval > 2 && dval < 50)
                        {
                            GetSpaceEnts(ent);
                            axEWdraw3.CabinetDesignPlankThickness(ent, dval);
                            SetCompByPlank(ent);//2020-03-07
                            axEWdraw3.ClearSelected();
                        }
                    }
                }
            }
            else
                MessageBox.Show("请在柜体设计状态下使用");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (isCabinetDesign)
            {
                int selsize = axEWdraw3.GetSelectEntSize();
                if (selsize > 0)
                {
                    g_axismoveid = axEWdraw3.GetSelectEnt(0);
                    if (g_axismoveid > 0)
                    {
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw3.GetEntBoundingBox(g_axismoveid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        if (g_axismoveplank == null)
                            g_axismoveplank = new CPlank();
                        g_axismoveplank.minx = minx;
                        g_axismoveplank.miny = miny;
                        g_axismoveplank.minz = minz;
                        g_axismoveplank.maxx = maxx;
                        g_axismoveplank.maxy = maxy;
                        g_axismoveplank.maxz = maxz;
                        g_axismoveplank.id = g_axismoveid;
                        g_timerproc = 2;
                        timer9.Enabled = true;
                    }
                }

                label1.Text = "选择要移动的板材.";
                axEWdraw3.EnableMoveSinglePlank(true);
                axEWdraw3.EnableSinglePlankAxisMove(true);
                issingplankmove = true;
                axEWdraw3.ToDrawAxisMove();
            }
            else
                MessageBox.Show("请在柜体设计状态下使用");
        }

        private void axEWdraw3_EndAxisMove(object sender, AxEWDRAWLib._DAdrawEvents_EndAxisMoveEvent e)
        {
            axEWdraw3.EnableMoveSinglePlank(false);
            axEWdraw3.EnableSinglePlankAxisMove(false);
            issingplankmove = false;//2019-12-06
            g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();
            if (g_axismoveid > 0)
            {
                double minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = 0.0;
                axEWdraw3.GetEntBoundingBox(g_axismoveid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                if (g_axismoveplank == null)
                    g_axismoveplank = new CPlank();
                g_axismoveplank.minx = minx;
                g_axismoveplank.miny = miny;
                g_axismoveplank.minz = minz;
                g_axismoveplank.maxx = maxx;
                g_axismoveplank.maxy = maxy;
                g_axismoveplank.maxz = maxz;
                g_timerproc = 3;
                axEWdraw3.ReDrawView();
                timer9.Enabled = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            HideWJ(false);//消隐五金 2020-03-20
            //恢复消陷的对象
            RestoreHideObjs();//2020-01-23
            //
            if (!isexplodecabinet)
            {
                isexplodecabinet = true;
                axEWdraw3.ClearIDBuffer();
                int entsize = axEWdraw3.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    int ent = axEWdraw3.GetEntID(i);
                    int enttype = axEWdraw3.GetEntType(ent);
                    if (enttype != 501 && enttype != 502 && enttype != 503)
                        axEWdraw3.AddIDToBuffer(ent);
                }
                axEWdraw3.CabinetExplode(100.0);
                axEWdraw3.ZoomALL();
            }
            else
            {
                isexplodecabinet = false;
                int entsize = axEWdraw3.GetIDBufferSize();
                double x, y, z;
                x = y = z = 0.0;
                for (int i = 0; i < entsize; i++)
                {
                    int ent = axEWdraw3.GetIDBuffer(i);
                    axEWdraw3.Get3DPtBuf(i, ref x, ref y, ref z);
                    axEWdraw3.MoveTo(ent, new object[] { x, y, z }, new object[] { 0, 0, 0 });
                }
                axEWdraw3.Clear3DPtBuf();
                axEWdraw3.ClearIDBuffer();
                axEWdraw3.ZoomALL();
            }
        }
        //恢复炸开柜体 2020-04-20
        private void RestoreExplodeCabinet()
        {
            if (isexplodecabinet)
            {
                isexplodecabinet = false;
                int entsize = axEWdraw3.GetIDBufferSize();
                double x, y, z;
                x = y = z = 0.0;
                for (int i = 0; i < entsize; i++)
                {
                    int ent = axEWdraw3.GetIDBuffer(i);
                    axEWdraw3.Get3DPtBuf(i, ref x, ref y, ref z);
                    axEWdraw3.MoveTo(ent, new object[] { x, y, z }, new object[] { 0, 0, 0 });
                }
                axEWdraw3.Clear3DPtBuf();
                axEWdraw3.ClearIDBuffer();
                axEWdraw3.ZoomALL();
            }
        }
        private void listView7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            SaveWork();
            HideWJ();//消隐五金 2020-03-20
            if (g_orderid == null)
            {
                MessageBox.Show("创建或打开订单后,再使用该功能.");
                return;
            }
            if (zPubFun.zPubFunLib.g_istriallimit)
                return;
            //
            Form12 fm12 = new Form12();//生成安装图
            string filename = ConvertPrjIDToEWDPath("",false);
            fm12.m_ewdfilename = filename;
            fm12.m_orderid = g_orderid;
            fm12.m_orderdate = g_orderdate;
            fm12.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (!g_ishide)
            {
                int entsize = axEWdraw3.GetSelectEntSize();
                if (entsize > 0)
                {
                    for (int i = 0; i < entsize; i++)
                    {
                        int ent = axEWdraw3.GetSelectEnt(i);
                        axEWdraw3.SetEntityInvisible(ent,true);
                        g_allhides.Add(ent);
                        ArrayList insidecompents = new ArrayList();
                        GetCabinectInside(ent, ref insidecompents);//2020-03-30
                        if (insidecompents.Count > 0)
                        {
                            for (int j = 0; j < insidecompents.Count; j++)
                            {
                                ent = (int)insidecompents[j];
                                g_allhides.Add(ent);
                                axEWdraw3.SetEntityInvisible(ent, true);
                            }
                        }
                        insidecompents.Clear();
                        insidecompents = null;
                    }
                    g_ishide = true;
                    axEWdraw3.ClearSelected();
                    button16.ImageIndex = 16;
                    toolTip1.SetToolTip(button16, "结束消隐");
                }
            }
            else
            {
                if (g_allhides.Count > 0)
                {
                    for (int i = 0; i < g_allhides.Count; i++)
                    {
                        int ent = (int)g_allhides[i];
                        axEWdraw3.SetEntityInvisible(ent, false);
                    }
                    g_ishide = false;
                    g_allhides.Clear();
                    axEWdraw3.ClearSelected();
                    button16.ImageIndex = 17;
                    toolTip1.SetToolTip(button16, "消隐");
                }
            }

        }

        private void RestoreHideObjs()
        {
            if (g_allhides.Count > 0)
            {
                for (int i = 0; i < g_allhides.Count; i++)
                {
                    int ent = (int)g_allhides[i];
                    axEWdraw3.SetEntityInvisible(ent, false);
                }
                g_ishide = false;
                g_allhides.Clear();
                axEWdraw3.ClearSelected();
            }
        }

        //测试渲染
        private void TestRender()
        {
            axEWdraw1.OpenEwd("test_render.ewd");
            axEWdraw1.SetViewCondition(1);

            double x, y, z;
            x = y = z = 0;
            int ent = 3;
            int enttype = axEWdraw1.GetEntType(ent);
            if (enttype == 50)
            {
                string str = axEWdraw1.GetEntityUserData(ent);
                if (str.IndexOf("area:") >= 0)
                {
                    g_innerx = 13611.56;
                    g_innery = 14130.32;
                    g_innerz = 796.44;
                    g_isinnerorbit = true;
                    axEWdraw1.ClearSelected();
                    if (IsHaveRoom())
                    {
                        GlobalView();
                        axEWdraw1.SetCamera(new object[] { 13692.14, 17228.22, 596.44 }, new object[] { 13611.56, 13130.32, 896.44 });
                    }
                    axEWdraw1.CancelCommand();
                    axEWdraw1.ClearIDBuffer();
                    if (axEWdraw1.GetIDBufferSize() > 0)
                    {
                        for (int i = 0; i < axEWdraw1.GetIDBufferSize(); i++)
                        {
                            g_roofids.Add(axEWdraw1.GetIDBuffer(i));
                        }
                    }
                    axEWdraw1.ClearIDBuffer();
                    string pngfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\test.png";
                    axEWdraw1.StartExtRender(1330, 750, pngfile);//
                    //绘制户型
                    if (IsHaveRoom())
                    {
                        DrawHome();
                    }
                    
                }
            }
        }

        private void CreateTwo(TreeNode node, int id,ArrayList limitnames = null,bool isskipcomp = false)
        {
            int itemsize = 0;
            axEWdraw2.GetPlugInTree(id, ref itemsize);
            ArrayList subnodes = new ArrayList();

            for (int i = 0; i < itemsize; i++)
            {
                SNode asubnode = new SNode();
                asubnode.name = axEWdraw2.GetPlugInTreeItem(i, ref asubnode.code, ref asubnode.type);
                subnodes.Add(asubnode);
            }
            if (!isskipcomp)
            {
                for (int i = 0; i < subnodes.Count; i++)
                {
                    if (limitnames == null)
                    {
                        TreeNode nd = new TreeNode();
                        nd.Text = ((SNode)subnodes[i]).name;
                        nd.Tag = subnodes[i];//((SNode)subnodes[i]).code;
                        CreateTwo(nd, ((SNode)subnodes[i]).code,limitnames,isskipcomp);
                        node.Nodes.Add(nd);
                    }
                    else
                    {
                        if (limitnames.Count > 0)
                        {
                            bool isfind = false;
                            for (int j = 0; j < limitnames.Count; j++)
                            {
                                if (((string)limitnames[j]) == ((SNode)subnodes[i]).name)
                                {
                                    isfind = true;
                                    break;
                                }
                            }
                            if (isfind)
                            {
                                TreeNode nd = new TreeNode();
                                nd.Text = ((SNode)subnodes[i]).name;
                                nd.Tag = subnodes[i];//
                                CreateTwo(nd, ((SNode)subnodes[i]).code);
                                node.Nodes.Add(nd);
                            }

                        }
                        else
                        {
                            TreeNode nd = new TreeNode();
                            nd.Text = ((SNode)subnodes[i]).name;
                            nd.Tag = subnodes[i];//
                            CreateTwo(nd, ((SNode)subnodes[i]).code, limitnames, isskipcomp);
                            node.Nodes.Add(nd);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < subnodes.Count; i++)
                {
                    if (limitnames == null)
                    {
                        TreeNode nd = new TreeNode();
                        nd.Text = ((SNode)subnodes[i]).name;
                        nd.Tag = subnodes[i];//
                        CreateTwo(nd, ((SNode)subnodes[i]).code, limitnames, isskipcomp);
                        node.Nodes.Add(nd);
                    }
                    else
                    {
                        if (limitnames.Count > 0)
                        {
                            bool isfind = false;
                            for (int j = 0; j < limitnames.Count; j++)
                            {
                                if (((string)limitnames[j]) == ((SNode)subnodes[i]).name)
                                {
                                    isfind = true;
                                    break;
                                }
                            }
                            if (!isfind)
                            {
                                TreeNode nd = new TreeNode();
                                nd.Text = ((SNode)subnodes[i]).name;
                                nd.Tag = subnodes[i];//
                                CreateTwo(nd, ((SNode)subnodes[i]).code,limitnames, isskipcomp);
                                node.Nodes.Add(nd);
                            }
                        }
                    }
                }
            }
            subnodes.Clear();
        }


        //初始化家具库
        private void InitJiaJuKu()
        {
            string orgpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet";
            if (!System.IO.Directory.Exists(orgpath))
            {
                System.IO.Directory.CreateDirectory(orgpath);
                if(!System.IO.File.Exists(orgpath + "\\allT.xml"))
                    System.IO.File.Copy("allT.xml", orgpath + "\\allT.xml");
                if (!System.IO.File.Exists(orgpath + "\\allwjs.xml"))
                    System.IO.File.Copy("allwjs.xml", orgpath + "\\allwjs.xml");
                if (!System.IO.File.Exists(orgpath + "\\allmats.xml"))
                    System.IO.File.Copy("allmats.xml", orgpath + "\\allmats.xml");
            }

            axEWdraw2.SetWireOfShape(axEWdraw2.RGBToIndex(128, 128, 128), 1);
            axEWdraw2.SetBackGroundImage("bj.bmp");
            axEWdraw2.SetGridValue(200, 200, 3000, 3000, 0);
            axEWdraw2.SetGridColor(128, 128, 128, 128, 128, 128);
            axEWdraw2.SetGridOrgColor(true, 255, 0, 0, 0, 255, 0);
            axEWdraw2.SetGridOn(true);

            axEWdraw2.LoadPlugIn("Jiaju.dll");
            //
            TreeNode root = new TreeNode();
            root.Text = "家具模型库";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //
            ArrayList tmpstrarr = new ArrayList();
            //这些类别一定要跳过,因为功能件是需要单列的
            tmpstrarr.Add("抽屉");
            tmpstrarr.Add("功能件");//2020-03-04
            tmpstrarr.Add("一");//2020-03-04
            tmpstrarr.Add("二");//2020-03-04
            tmpstrarr.Add("三");//2020-03-04
            tmpstrarr.Add("四");//2020-03-04
            tmpstrarr.Add("五");//2020-03-04
            tmpstrarr.Add("六");//2020-03-04
            CreateTwo(root, 1245,tmpstrarr,true);
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //MiddleLeft
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "基础柜");
            treeView1.SelectedNode = node;
            treeView1.Focus();//2019-12-27
            axEWdraw2.SetPerspectiveMode(true);
            //
            axEWdraw3.EnableWireOfShape(true);
            axEWdraw3.SetWireOfShape(axEWdraw3.RGBToIndex(0, 0, 0), 2);
            axEWdraw3.SetBackGroundImage("bj.bmp");
            axEWdraw3.DisplayTrihedron(true);
            axEWdraw3.SetGridValue(200, 200, 20000, 20000, 0);
            axEWdraw3.SetGridColor(128, 128, 128, 128, 128, 128);
            axEWdraw3.SetGridOrgColor(true, 255, 0, 0, 0, 255, 0);
            axEWdraw3.SetGridOn(true);
            axEWdraw3.SetViewCondition(8);
            axEWdraw3.Visible = false;
            listView7.Visible = false;//2019-12-02
            //2019-12-04
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            button17.Visible = false;
            button18.Visible = false;
            button19.Visible = false;
            button20.Visible = false;
            button32.Visible = false;//2020-02-17
            //
            button27.Visible = false;
            button28.Visible = false;
            button29.Visible = false;
            button30.Visible = false;
            button31.Visible = false;
            button42.Visible = false;//2020-03-24
            button43.Visible = false;
            button44.Visible = false;
            button27.Left += 100;
            button28.Left += 100;
            button29.Left += 100;
            button30.Left += 100;
            button31.Left += 100;
            button42.Left += 100;
            button43.Left += 100;//2020-03-24
            button44.Left += 100;//2020-03-24
        }

        //重置家具库 2020-03-05
        private void ResetInitJiaJuKu()
        {
            GDesignMode = -1;
            treeView1.Nodes.Clear();
            dataGridView1.Rows.Clear();
            //
            TreeNode root = new TreeNode();
            root.Text = "家具模型库";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //跳过单列的功能件
            ArrayList tmpstrarr = new ArrayList();
            tmpstrarr.Add("抽屉");
            tmpstrarr.Add("功能件");//2020-03-04
            tmpstrarr.Add("一");//2020-03-04
            tmpstrarr.Add("二");//2020-03-04
            tmpstrarr.Add("三");//2020-03-04
            tmpstrarr.Add("四");//2020-03-04
            tmpstrarr.Add("五");//2020-03-04
            tmpstrarr.Add("六");//2020-03-04
            CreateTwo(root, 1245, tmpstrarr, true);
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "基础柜");
            treeView1.SelectedNode = node;
            treeView1.Focus();//2019-12-27
            //
            button35.Visible = false;//2020-03-24
            button37.Enabled = false;//2020-03-24
            button38.Visible = false;
            button40.Visible = true;//2020-03-25
            axEWdraw3.SetViewCondition(8);

        }

        private TreeNode FindNode(TreeNode tnParent, string strValue)
        {
            if (tnParent == null) return null;
            if (tnParent.Text == strValue) return tnParent;
            else if (tnParent.Nodes.Count == 0) return null;
            TreeNode tnCurrent, tnCurrentPar;
            //Init node
            tnCurrentPar = tnParent;
            tnCurrent = tnCurrentPar.FirstNode;
            while (tnCurrent != null && tnCurrent != tnParent)
            {
                while (tnCurrent != null)
                {
                    if (tnCurrent.Text == strValue) return tnCurrent;
                    else if (tnCurrent.Nodes.Count > 0)
                    {
                        //Go into the deepest node in current sub-path
                        tnCurrentPar = tnCurrent;
                        tnCurrent = tnCurrent.FirstNode;
                    }
                    else if (tnCurrent != tnCurrentPar.LastNode)
                    {
                        //Goto next sible node
                        tnCurrent = tnCurrent.NextNode;
                    }
                    else
                        break;
                }
                //Go back to parent node till its has next sible node
                while (tnCurrent != tnParent && tnCurrent == tnCurrentPar.LastNode)
                {
                    tnCurrent = tnCurrentPar;
                    tnCurrentPar = tnCurrentPar.Parent;
                }
                //Goto next sible node
                if (tnCurrent != tnParent)
                    tnCurrent = tnCurrent.NextNode;
            }
            return null;
        }
        private void InitOrgPlanks(bool isOn)
        {
            if (isOn)
            {
                GDesignMode = 0;
                listView6.Top = 40;//2019-12-27
                listView6.Height = 565;//2019-12-27
                listView6.Visible = true;
                treeView1.Visible = false;
                dataGridView1.Visible = false;
                axEWdraw2.Top = 700;
                button21.Visible = true;
                button22.Visible = true;
                button33.Visible = true;
                button34.Visible = true;//2020-03-04
                //2019-12-30
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                //
                string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet";
                ReadPlanksFromFile("orgplanks.ewd", dir, 128, ref listView1);
            }
            else
            {
                GDesignMode = -1;
                listView6.Visible = false;
                treeView1.Visible = true;
                dataGridView1.Visible = true;
                button21.Visible = false;
                button22.Visible = false;
                button33.Visible = false;
                button34.Visible = false;//2020-03-04
                axEWdraw2.Top = 8;
                axEWdraw2.Height = 215;
                axEWdraw2.Width = 233;
                //2019-12-30
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;

            }
        }
        private void ReadPlanksFromFile(string filename, string path, int width, ref ListView list)
        {
            string orgfilename;
            orgfilename = path + "\\orgplanks.ewd";
            if (System.IO.File.Exists(orgfilename))
            {
                axEWdraw2.OpenOrgPlanks(orgfilename);
                axEWdraw3.OpenOrgPlanks(orgfilename);
                if (System.IO.Directory.Exists(path))
                {
                    int psize = axEWdraw2.GetOrgPlanksSize();
                    int pid = 0;
                    string name = "";
                    string rname = "";
                    string dir = path + "\\PlankImages";
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    if (System.IO.Directory.Exists(dir))
                    {
                        listView6.Items.Clear();
                        imageList6.Images.Clear();
                        listView6.ShowItemToolTips = true;
                        string oname = "";
                        for (int i = 0; i < psize; i++)
                        {
                            pid = axEWdraw2.GetOrgPlanksID(i);
                            name = axEWdraw2.GetNameFromOrgPlanks(i);
                            if (name.Length > 0)
                            {
                                rname = dir + "\\" + name + pid.ToString() + ".bmp";
                                oname = name + pid.ToString();
                            }
                            else
                            {
                                rname = dir + "\\" + pid.ToString() + ".bmp";
                                oname = pid.ToString();
                            }
                            if (!System.IO.File.Exists(rname))
                                axEWdraw2.GetBMPFromOrgPlanks(pid, rname, width, axEWdraw2.RGBToIndex(160, 160, 160), axEWdraw2.RGBToIndex(255, 255, 255));
                            if (System.IO.File.Exists(rname))
                            {
                                System.Drawing.Image image;
                                image = Image.FromFile(rname);
                                if (imageList6.ImageSize.Width != image.Width || imageList6.ImageSize.Height != image.Height)
                                    imageList6.ImageSize = new Size(image.Width, image.Height);

                                imageList6.Images.Add(oname, image);
                                ListViewItem aitem = listView6.Items.Add(oname, i);

                                aitem.ToolTipText = oname;

                            }
                        }
                    }
                }
            }
        }
        private string ConvertPrjIDToEWDPath(string inname = "",bool isnew = true)
        {
            string filename = "";
            if (g_orderid == null)
            {
                MessageBox.Show("创建或打开订单后,再使用该功能");
                return filename;//2020-07-01
            }
            if(g_orderid.Length>0)
            {
                
                string year = DateTime.Now.ToString("yyyy");//g_orderid.Substring(0, 2);
                string oyear = Convert.ToDateTime(g_orderdate).Year.ToString();
                if(inname.Length>0)//如果有文件,则判断目录是原有的目录,而非新目录
                {
                    if(isnew)
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + g_orderid + "\\" + inname;
                    else
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + g_orderid + "\\" + inname;
                }
                   
                else
                {
                    if (isnew)
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + year + "\\" + g_orderid + "\\" + g_orderid + ".ewd";
                    else
                        filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\" + oyear + "\\" + g_orderid + "\\" + g_orderid + ".ewd";
                }
                   
            }
            return filename;
        }
        private string GetOrderID()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            Form14 fm14 = new Form14();
            fm14.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            Form16 fm16 = new Form16();
            fm16.ShowDialog();
            
        }

        //得到抽屉的基点 2019-12-26
        private bool GetDrawerBasePt(int id,
            bool isinside,//是否是内嵌抽屉
            double left,
            double front,
            double bottom,
            ref double x,ref double y,ref double z)
        {
            double minx,miny,minz,maxx,maxy,maxz;
            minx=miny=minz=maxx=maxy=maxz=0.0;
            if (axEWdraw1.IsGroup(id))
            {
                axEWdraw1.GetEntBoundingBox(id, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                if (isinside)
                {
                    x = minx;
                    y = miny;
                    z = minz;
                }
                else
                {
                    x = minx - left;
                    y = miny + front;
                    z = minz + bottom;
                }
            }
            return false;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            label1.Text = "插入对应的抽屉";
            HideWJ();//消隐五金 2020-03-20
            GDesignMode = 1;
            //
            listView6.Visible = false;
            treeView1.Visible = true;
            dataGridView1.Visible = true;
            //2019-12-30
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            button38.Visible = false;//2020-03-20
            button40.Visible = false;//2020-03-19
            //
            axEWdraw2.Top = 42;
            axEWdraw2.Height = 182;
            axEWdraw2.Width = 233;
            //
            treeView1.Nodes.Clear();
            TreeNode root = new TreeNode();
            root.Text = "抽屉";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //2020-01-23
            ArrayList tmpstrarr = new ArrayList();
            tmpstrarr.Add("抽屉");
            CreateTwo(root, 1245, tmpstrarr,false);
            //
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "内嵌一抽_定位");
            treeView1.SelectedNode = node;
            treeView1.Focus();
        }

        //计算抽屉基点 2019-12-30
        private void CalCTBasePt(ref double x,ref double y,ref double z,
                                 ref double minx,ref double miny,ref double minz,
                                 ref double maxx,ref double maxy,ref double maxz
                                )
        {
            g_ctleft = 0;//抽屉左侧
            g_ctright = 0;//抽屉右侧
            g_cttop = 0;//抽屉上侧
            g_ctbottom = 0;//抽屉下侧
            g_ctwidth = 0;//抽屉宽度
            g_ctheight = 0;//抽屉高度
            g_cmbottom = 0;//抽面低于抽盒
            g_ctdbmspace = 0;//单边门缝隙
            double left, thickness, right, up, bottom, width, height,cmbottom,depth,dbmspace;
            left = thickness = bottom = right = up = width = height = cmbottom = depth = dbmspace = 0.0;
            minx=miny=minz=maxx=maxy=maxz = 0.0;
            axEWdraw2.GetAllBoundingBox(ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            TreeNode node = treeView1.SelectedNode;
            
            
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "板厚")
                    {
                        thickness = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "门板左盖")
                    {
                        left = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "门板下盖")
                    {
                        bottom = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "门板上盖")
                    {
                        up = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "门板右盖")
                    {
                        right = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "宽度")
                    {
                        width = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "高度")
                    {
                        height = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "抽面下低于抽盒")
                    {
                        cmbottom = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "深度")
                    {
                        depth = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "单边门缝隙")
                    {
                        dbmspace = zPubFun.zPubFunLib.CStr2Double(dataGridView1.Rows[i].Cells[1].Value);
                    }

                }
                g_ctleft = left;//抽屉左侧
                g_ctright = right;//抽屉右侧
                g_cttop = up;//抽屉上侧
                g_ctbottom = bottom;//抽屉下侧
                g_ctwidth = width;//抽屉宽度
                g_ctheight = height;//抽屉高度
                g_cmbottom = cmbottom;//抽面低于抽盒
                g_ctdepth = depth;//抽屉深度
                g_ctdbmspace = dbmspace;//单边门缝隙
            }
            //
            if (node.Text.IndexOf("内嵌") >= 0 || node.Text.IndexOf("裤抽") >= 0 || node.Text.IndexOf("多宝格") >= 0)
            {
                if (node.Text.IndexOf("定位") >= 0)//2020-01-21
                {
                    x = minx;
                    y = miny;
                    z = minz;
                }
                else
                {
                    x = minx - g_ctdbmspace;
                    y = miny;
                    z = minz - g_ctdbmspace;
                }
                g_iswgct = false;
            }
            else if ((node.Text.IndexOf("抽") >= 0 && node.Text.IndexOf("外盖") >= 0) ||
                    node.Text.IndexOf("裤抽") >= 0 || node.Text.IndexOf("多宝格") >= 0
                )
            {
                g_iswgct = true;
                x = minx + left - g_ctdbmspace;
                y = miny + thickness;
                z = minz + bottom - g_ctdbmspace;

            }
        }

        //计算F架基点 2019-02-21
        private void CalFJIABasePt(ref double x, ref double y, ref double z,
                                 ref double minx, ref double miny, ref double minz,
                                 ref double maxx, ref double maxy, ref double maxz
                                )
        {
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw2.GetAllBoundingBox(ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            x = minx;
            y = miny;
            z = minz;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            label1.Text = "将选中的板材变为异型板";
            HideWJ();//消隐五金 2020-03-20
            GDesignMode = 2;
            //
            listView6.Visible = false;
            treeView1.Visible = true;
            dataGridView1.Visible = true;
            //2019-12-30
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            button38.Visible = true;//2020-03-20
            button40.Visible = false;//2020-03-19
            //
            axEWdraw2.Top = 42;
            axEWdraw2.Height = 182;
            axEWdraw2.Width = 233;
            //
            treeView1.Nodes.Clear();
            dataGridView1.Rows.Clear();
            axEWdraw2.DeleteAll();
            checkBox3.Checked = false;
            checkBox4.Checked = true;
            if (g_yxbs.Count > 0)
            {
                TreeNode rootnode = new TreeNode();
                rootnode.Text = "异型板";
                rootnode.Tag = -1;//((SNode)subnodes[i]).code;

                treeView1.Nodes.Add(rootnode);
                for (int i = 0; i < g_yxbs.Count; i++)
                {
                    TreeNode nd = new TreeNode();
                    nd.Text = ((MyYXB)g_yxbs[i]).m_name;
                    nd.Tag = i;//
                    rootnode.Nodes.Add(nd);
                }
                rootnode.ExpandAll();
                TreeNode node = FindNode(rootnode, "右弧板");
                treeView1.SelectedNode = node;
                treeView1.Focus();//2019-12-27

            }
        }

        //更新宽度与高度 //2019-12-30
        private void UpdateCT(double width, double height,double depth = 0.0,bool isaddshare = false)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (isaddshare)//2010-01-13
                    axEWdraw2.ClearShare();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "宽度")
                    {
                        dataGridView1.Rows[i].Cells[1].Value = width.ToString("0.00");
                        axEWdraw2.SetPlugInParameter(i, width);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "高度")
                    {
                        dataGridView1.Rows[i].Cells[1].Value = height.ToString("0.00");
                        axEWdraw2.SetPlugInParameter(i, height);
                    }
                    else if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "深度")
                    {
                        dataGridView1.Rows[i].Cells[1].Value = depth.ToString("0.00");
                        axEWdraw2.SetPlugInParameter(i, depth);
                    }
                }
            }
            DataGridViewCell cell = dataGridView1.CurrentCell;
            //调用模型
            axEWdraw2.DeleteAll();
            axEWdraw2.ClearShare();
            if (g_tcode > 0)
            {
                axEWdraw2.CreatePlugIn(g_tcode);
                //纹理
                int entsize = axEWdraw2.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    int ent = axEWdraw2.GetEntID(i);
                    int enttype = axEWdraw2.GetEntType(ent);
                    if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                    {
                        axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                        if (isaddshare)//2010-01-13
                            axEWdraw2.AddEntToShare(ent);
                    }
                }
                axEWdraw2.ZoomALL();
            }
        }

        //计算板材左右的空间体 2020-01-31
        private void GetPlankLeftRight(int ent)
        {

        }

        //直接更新宽度 2020-01-31
        private void UpdateCTWidth(int ctent,double minX, double minY, double minZ, 
            double maxX, double maxY, double maxZ)
        {
            if (GDesignMode != 1)
                button22.PerformClick();
            if (axEWdraw3.IsGroup(ctent))
            {
                string grpname = axEWdraw3.GetGroupName(ctent);
                if (grpname.IndexOf("_ct_") >= 0)
                {
                    GetCTParamaters(ctent);
                    g_selct = true;//2020-01-09
                    //取得抽屉的信息
                    g_selctid = ctent;
                    axEWdraw3.GetGroupInsPt(ctent, ref g_selctx, ref g_selcty, ref g_selctz);
                    treeView1.Focus();
                    treeView1.Refresh();
                }
                else return;
            }
            //定位宽度设置 2020-02-01
            g_sminx = minX;
            g_sminy = minY;
            g_sminz = minZ;
            g_smaxx = maxX;
            g_smaxy = maxY;
            g_smaxz = maxZ;
            double nwidth = Math.Abs(g_smaxx - g_sminx);

            if (dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "宽度")
                    {
                        dataGridView1.Rows[i].Cells[1].Value = nwidth.ToString("0.00");
                        dataGridView1.Rows[i].Cells[1].Selected = true;
                        break;
                    }
                }
            }
            //
            DataGridViewCell cell = dataGridView1.CurrentCell;
            axEWdraw2.SetPlugInParameter(cell.RowIndex, zPubFun.zPubFunLib.CStr2Double(cell.Value));
            //调用模型
            axEWdraw2.DeleteAll();
            if (g_tcode > 0)
            {
                axEWdraw2.CreatePlugIn(g_tcode);
                //纹理
                if (g_selctid > 0)
                    axEWdraw2.ClearShare();
                int entsize = axEWdraw2.GetEntSize();
                for (int i = 1; i <= entsize; i++)
                {
                    int ent = axEWdraw2.GetEntID(i);
                    int enttype = axEWdraw2.GetEntType(ent);
                    if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                    {
                        axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                        if (g_selctid > 0)
                        {
                            axEWdraw2.AddEntToShare(ent);
                        }
                    }
                }
                axEWdraw2.ZoomALL();
                //判断是否是已创建抽屉的参数
                if (g_selctid > 0)
                {
                    int winx, winy;
                    winx = winy = 0;
                    //得到选中抽屉所在空间体的位置 2020-01-13 这里需要改
                    //2019-12-30
                    double octminz, octmaxz;
                    octminz = octmaxz = 0.0;
                    //得到抽屉的在空间体的的上下界 2010-01-13
                    GetCTMinMaxZ(g_selctid, g_sminx, g_sminy, g_sminz,
                                 g_smaxx, g_smaxy, g_smaxz, g_ctinsz, ref octminz, ref octmaxz);

                    CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
                    double oinsx, oinsy, oinsz;
                    oinsx = oinsy = oinsz = 0.0;
                    string grpname = "grp_ct_" + g_tcode.ToString();
                    int grpid = 0;
                    axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
                    if (grpid > 0)
                    {
                        g_isctins = false;//2020-01-03
                        CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);

                        axEWdraw3.GetGroupInsPt(g_selctid, ref oinsx, ref oinsy, ref oinsz);

                        //
                        if (Math.Abs(maxX - minX) > 0)
                        {
                            double insx, insy, insz;
                            insx = insy = insz = 0.0;
                            axEWdraw3.EyeLineInsAABBs(winx, winy, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                            g_ctinsx = insx;
                            g_ctinsy = insy;
                            g_ctinsz = insz;
                            axEWdraw3.ClearSelected();
                        }
                        //
                        axEWdraw3.SetGroupInsPt(grpid, new object[] { g_ctbx, g_ctby, g_ctbz });
                        axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);
                        //判断是否自适应高 2020-01-14
                        if (checkBox3.Checked)
                            axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { g_sminx, oinsy, octminz + g_ctbottom});//
                        else
                            axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { g_sminx, oinsy, oinsz});//
                        TreeNode node = treeView1.SelectedNode;
                        SaveCTParamaters(grpid, node.Text);
                        axEWdraw3.Delete(g_selctid);
                        g_selctid = 0;
                        axEWdraw3.ClearSelected();
                    }
                    g_selctid = 0;
                    axEWdraw3.ClearSelected();
                    axEWdraw3.UpdateView();
                }
            }
        }

        //保存抽屉参数 2019-12-30
        private void SaveCTParamaters(int ent,string name)
        {
            string memo = "名称:"+name+";";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    memo += dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() + ":" + dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() + ";";
                }
                if (memo.Length > 0)
                {
                    axEWdraw3.SetEntityUserData(ent, memo);
                }
            }
        }

        //取得抽屉的参数 2019-12-31
        private void GetCTParamaters(int ent)
        {
            string memo = axEWdraw3.GetEntityUserData(ent);
            if (memo.Length > 0)
            {
                string[] strarr = memo.Split(';');
                if (strarr.Length > 0)
                {
                    int tcode = 0;
                    for (int i = 0; i < strarr.Length-1; i++)
                    {
                        if (strarr[i].Length > 0)
                        {
                            string[] strarr1 = strarr[i].Split(':');
                            if (strarr1[0] == "名称")
                            {
                                TreeNode node = FindNode(treeView1.Nodes[0], strarr1[1]);
                                if (node != null)
                                {
                                    treeView1.SelectedNode = node;
                                    treeView1.Focus();//2019-12-27
                                    treeView1.Refresh();
                                    //
                                    tcode = ((SNode)node.Tag).code;
                                    g_tcode = tcode;
                                    int type = ((SNode)node.Tag).type;
                                    g_tname = ((SNode)node.Tag).name;
                                    int size = axEWdraw2.GetPlugInParameterSize(tcode);
                                    double val = 0;
                                    dataGridView1.Rows.Clear();
                                    for (int j = 0; j < size; j++)
                                    {
                                        string name = axEWdraw2.GetPlugInParameter(j, ref val);
                                        dataGridView1.Rows.Add();
                                        int count = dataGridView1.Rows.Count;
                                        dataGridView1[0, count - 1].Value = name;
                                        dataGridView1[1, count - 1].Value = val.ToString("0.00");
                                    }
                                    if (dataGridView1.ColumnCount > 0)
                                    {
                                        dataGridView1.Columns[0].Width = 120;
                                        dataGridView1.Columns[1].Width = 73;
                                    }
                                }
                            }
                            else
                            {
                                string name = strarr1[0];
                                string value = strarr1[1];
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                        if (dataGridView1.Rows[j].Cells[0].Value.ToString() == name)
                                        {
                                            dataGridView1.Rows[j].Cells[1].Value = value;
                                            axEWdraw2.SetPlugInParameter(j, zPubFun.zPubFunLib.CStr2Double(value));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //调用模型
                    axEWdraw2.DeleteAll();
                    axEWdraw2.CreatePlugIn(tcode);
                    //纹理
                    int entsize = axEWdraw2.GetEntSize();
                    for (int j = 1; j <= entsize; j++)
                    {
                        ent = axEWdraw2.GetEntID(j);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                        }
                    }
                    axEWdraw2.SetViewCondition(8);
                    axEWdraw3.Focus();
                }
            }
        }

        private void GetCTParamaters(int ent,ref double left,ref double right,ref double up,ref double bottom,ref double thickness)
        {
            string memo = axEWdraw3.GetEntityUserData(ent);
            if (memo.Length > 0)
            {
                string[] strarr = memo.Split(';');
                if (strarr.Length > 0)
                {
                    int tcode = 0;
                    for (int i = 0; i < strarr.Length - 1; i++)
                    {
                        if (strarr[i].Length > 0)
                        {
                            string[] strarr1 = strarr[i].Split(':');
                            string name = strarr1[0];
                            string value = strarr1[1];
                            if (name == "板厚")
                            {
                                thickness = zPubFun.zPubFunLib.CStr2Double(value);
                            }else if (name == "门板右盖")
                            {
                                right = zPubFun.zPubFunLib.CStr2Double(value);
                            }
                            else if (name == "门板左盖")
                            {
                                left = zPubFun.zPubFunLib.CStr2Double(value);
                            }
                            else if (name == "门板下盖")
                            {
                                bottom = zPubFun.zPubFunLib.CStr2Double(value);
                            }
                            else if (name == "门板上盖")
                            {
                                up = zPubFun.zPubFunLib.CStr2Double(value);
                            }
                        }
                    }
                }
            }
            else
            {//如果没有信息
                left = g_ctleft;//抽屉左侧
                right = g_ctright;//抽屉右侧
                up = g_cttop;//抽屉上侧
                bottom = g_ctbottom;//抽屉下侧
            }
        }
        //获得抽屉与侧向板材间相交的部分 2020-01-06
        private bool GetCTInsCB(ref int atype,ref double dist)
        {
            int ptype = 0;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            int psize = axEWdraw3.GetDesignPlankSize();
            for(int i = 0; i <psize;i++)
            {
                axEWdraw3.GetDesignPlankBnd(i, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz,ref ptype);
                if (ptype == 1)//侧板
                {
                    double ominx,ominy,ominz,omaxx,omaxy,omaxz;
                    ominx = ominy = ominz = omaxx = omaxy = omaxz = 0.0;
                    if (axEWdraw3.GetInsAABBs(g_ctminx, g_ctminy, g_ctminz, g_ctmaxx, g_ctmaxy, g_ctmaxz, minx, miny, minz, maxx, maxy, maxz,
                                            ref ominx, ref ominy, ref ominz, ref omaxx, ref omaxy, ref omaxz))
                    {
                        dist = (miny - g_ctinsy);
                        atype = 1;//X轴为0,Y轴为1,Z轴为2
                        return true;
                    }
                }
            }
            return false;
        }

        //抽屉相交判断 2020-01-06
        private bool IsCTIns(double iminx, double iminy, double iminz, double imaxx, double imaxy, double imaxz,int selfent,ref double dist)
        {
            double left = 0;
            double right = 0;
            double up = 0;
            double bottom = 0;
            double thickness = 0;
            GetCTParamaters(selfent, ref left, ref right, ref up, ref bottom,ref thickness);
            iminx += left;
            iminy += thickness;
            iminz += bottom;
            imaxx -= right;
            imaxz -= up;
            int entsize = axEWdraw3.GetEntSize();
            int ent = 0;
            double minx,miny,minz,maxx,maxy,maxz;
            minx =miny =minz =maxx = maxy =maxz = 0.0;
            double ominx, ominy, ominz, omaxx, omaxy, omaxz, tmpdist;
            ominx = ominy = ominz = omaxx = omaxy = omaxz = tmpdist = 0.0;
            ArrayList tmpids = new ArrayList();
            
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw3.GetEntID(i);
                if (axEWdraw3.IsDisplayed(ent))
                {
                    CPlank aplk = new CPlank();            
                    if (axEWdraw3.IsGroup(ent) && ent != selfent)
                    {
                        axEWdraw3.GetGroupBndPt(ent, 0, ref minx, ref miny, ref minz);//得到组的包围盒的拐点 0：底面左下角三维坐标
                        axEWdraw3.GetGroupBndPt(ent, 6, ref maxx, ref maxy, ref maxz);//得到组的包围盒的拐点 6：顶面右上角三维坐标
                        //
                        aplk.id = ent;
                        aplk.minx = minx;
                        aplk.miny = miny;
                        aplk.minz = minz;
                        aplk.maxx = maxx;
                        aplk.maxy = maxy;
                        aplk.maxz = maxz;
                        tmpids.Add(aplk);
                    }
                    else
                    {
                        axEWdraw3.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        aplk.id = ent;
                        aplk.minx = minx;
                        aplk.miny = miny;
                        aplk.minz = minz;
                        aplk.maxx = maxx;
                        aplk.maxy = maxy;
                        aplk.maxz = maxz;
                        tmpids.Add(aplk);
                    }

                }
            }
            for (int i = 0; i < tmpids.Count; i++)
            {
                if (axEWdraw3.GetInsAABBs(iminx + 0.001, iminy + 0.001, iminz + 0.001, imaxx - 0.001, imaxy - 0.001, imaxz - 0.001,
                    ((CPlank)tmpids[i]).minx, ((CPlank)tmpids[i]).miny, ((CPlank)tmpids[i]).minz, ((CPlank)tmpids[i]).maxx, ((CPlank)tmpids[i]).maxy, ((CPlank)tmpids[i]).maxz, 
                    ref ominx, ref ominy, ref ominz, ref omaxx, ref omaxy, ref omaxz))
                {
                    double tmpmidz = (ominz + omaxz) / 2.0;
                    //先测试Z值
                    if (Math.Abs(tmpmidz - ((CPlank)tmpids[i]).maxz) < Math.Abs(tmpmidz - ((CPlank)tmpids[i]).minz))
                    {
                        tmpdist = ((CPlank)tmpids[i]).maxz - iminz+up;
                    }
                    else
                    {
                        tmpdist = -(imaxz - ((CPlank)tmpids[i]).minz)-up;
                    }
                    break;
                }
            }
            //重新调整Z值
            iminz += tmpdist;
            imaxz += tmpdist;
            //重新判断
            for (int i = 0; i < tmpids.Count; i++)
            {

                if (axEWdraw3.GetInsAABBs(iminx + 0.001, iminy + 0.001, iminz + 0.001, imaxx - 0.001, imaxy - 0.001, imaxz - 0.001,
                    ((CPlank)tmpids[i]).minx, ((CPlank)tmpids[i]).miny, ((CPlank)tmpids[i]).minz, ((CPlank)tmpids[i]).maxx, ((CPlank)tmpids[i]).maxy, ((CPlank)tmpids[i]).maxz,
                    ref ominx, ref ominy, ref ominz, ref omaxx, ref omaxy, ref omaxz))
                {
                    return true;
                }
            }
            dist = tmpdist-g_ctdbmspace;
            return false;
        }

        //根据组的插入点,计算空间体
        private bool GetSpaceByGrp(int grpent, ref double minX, ref double minY, ref double minZ, ref double maxX, ref double maxY, ref double maxZ,ref int winx,ref int winy)
        {
            if (axEWdraw3.IsGroup(grpent))
            {
                double insx,insy,insz;
                insx = insy = insz = 0;
                double x1,y1,z1,x2,y2,z2;
                x1=y1=z1=x2=y2=z2= 0.0;
                int intx,inty;
                intx = inty = 0;
                axEWdraw3.GetGroupBndPt(grpent, 0, ref x1, ref y1, ref z1);
                axEWdraw3.GetGroupBndPt(grpent, 6, ref x2, ref y2, ref z2);
                insx = (x1 + x2) / 2.0;
                insy = (y1 + y2) / 2.0;
                insz = (z1 + z2) / 2.0;
                axEWdraw3.Coordinate2Screen(insx, insy, insz, ref intx, ref inty);
                winx = intx;
                winy = inty;
                minX = minY = minZ = maxX = maxY = maxZ = 0.0;
                axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                axEWdraw3.GetNearestCabinetSpace(intx, inty, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                return true;
            }
            return false;
        }

        private void timer9_Tick(object sender, EventArgs e)
        {
            double octminz, octmaxz;
            octminz = octmaxz = 0.0;
            timer9.Enabled = false;
            switch (g_timerproc)
            {
                case 1:
                    {
                        axEWdraw3.BeginUndo(true);
                        //判断抽屉的空间体是否在适应柜体的空间
                        if (Math.Abs(Math.Abs(g_ctmaxx - g_ctminx) - Math.Abs(g_smaxx - g_sminx)) > 0.001)
                        {
                            //计算取得抽屉的基点 2019-12-30
                            CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);

                            //2020-01-13 判断抽屉的上下界
                            GetCTMinMaxZ(0, g_sminx, g_sminy, g_sminz,
                                         g_smaxx, g_smaxy, g_smaxz, g_ctinsz, ref octminz, ref octmaxz);
                            //
                            double nwidth = Math.Abs(g_smaxx - g_sminx);
                            double ndepth = Math.Abs(g_smaxy - g_sminy);
                            double nheight = Math.Abs(octmaxz - octminz) - g_ctbottom - g_cttop;//Math.Abs(g_smaxz - g_sminz);

                            if (checkBox2.Checked && !checkBox4.Checked && !checkBox3.Checked)//只有宽度,默认
                                UpdateCT(nwidth, g_ctheight, g_ctdepth);
                            else if (!checkBox2.Checked && checkBox4.Checked && !checkBox3.Checked)//只有深度
                                UpdateCT(g_ctwidth, g_ctheight, ndepth);
                            else if (!checkBox2.Checked && !checkBox4.Checked && checkBox3.Checked)//只有高度
                                UpdateCT(g_ctwidth, nheight, g_ctdepth);
                            else if (checkBox2.Checked && checkBox4.Checked && !checkBox3.Checked)//宽度与深度
                                UpdateCT(nwidth, g_ctheight, ndepth);
                            else if (!checkBox2.Checked && checkBox4.Checked && checkBox3.Checked)//深度与高度
                                UpdateCT(g_ctwidth, nheight, ndepth);
                            else if (checkBox2.Checked && !checkBox4.Checked && checkBox3.Checked)//高度与宽度
                                UpdateCT(nwidth, nheight, g_ctdepth);
                            else if (checkBox2.Checked && checkBox4.Checked && checkBox3.Checked)//长,宽,深都自适应
                                UpdateCT(nwidth, nheight, ndepth);

                            //计算取得抽屉的基点 2019-12-30
                            CalCTBasePt(ref g_ctbx, ref g_ctby, ref g_ctbz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
                        }

                        int entsize = axEWdraw2.GetEntSize();
                        for (int i = 1; i <= entsize; i++)
                        {
                            int ent = axEWdraw2.GetEntID(i);
                            int enttype = axEWdraw2.GetEntType(ent);
                            if (enttype != 501 && enttype != 502 && enttype != 503)
                                axEWdraw2.AddEntToShare(ent);
                        }

                        string grpname = "grp_ct_" + g_tcode.ToString();
                        bool isperspect = false;
                        if (axEWdraw3.GetEntSize() == 0)
                            isperspect = true;
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw3.GetAllBoundingBox(ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        if (axEWdraw3.GetEntSize() == 0)
                        {
                            maxx = maxy = maxz = 0;
                        }
                        int grpid = 0;
                        axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
                        if (grpid > 0)
                        {
                            int tmptype = -1;
                            double tmpdist = 0.0;
                            //
                            axEWdraw3.SetGroupInsPt(grpid, new object[] { g_ctbx, g_ctby, g_ctbz });
                            axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);

                            //重置包围盒
                            g_ctminx += (g_sminx - g_ctbx);
                            g_ctminy += (g_sminy - g_ctby);
                            g_ctminz += (g_ctinsz - g_ctbz);
                            //
                            g_ctmaxx += (g_sminx - g_ctbx);
                            g_ctmaxy += (g_sminy - g_ctby);
                            g_ctmaxz += (g_ctinsz - g_ctbz);
                            if (g_iswgct)//是否是外盖抽屉
                            {
                                //
                                GetCTInsCB(ref tmptype, ref tmpdist);
                                g_ctminy += tmpdist;
                                g_ctmaxy += tmpdist;
                            }
                            double tmpdist1 = 0;
                            //
                            TreeNode node = treeView1.SelectedNode;
                            SaveCTParamaters(grpid, node.Text);
                            if (!IsCTIns(g_ctminx, g_ctminy, g_ctminz, g_ctmaxx, g_ctmaxy, g_ctmaxz, grpid, ref tmpdist1) || checkBox3.Checked)//checkbox3为高自适应
                            {
                                if (checkBox3.Checked)
                                    axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { g_sminx, g_sminy + tmpdist, octminz + g_ctbottom });
                                else
                                    axEWdraw3.MoveTo(grpid, new object[] { g_ctbx, g_ctby, g_ctbz }, new object[] { g_sminx, g_sminy + tmpdist, g_ctinsz + tmpdist1 });


                            }
                            else
                            {
                                axEWdraw3.Delete(grpid);
                                MessageBox.Show("与其它实体相交!");
                            }
                        }

                        if (axEWdraw3.GetEntSize() > 0)
                            axEWdraw3.SetPerspectiveMode(true);
                        //
                        axEWdraw2.ClearShare();
                        if (axEWdraw3.IsBeginUndo())//2020-03-16
                            axEWdraw3.EndUndo();
                    }
                    break;
                case 2:
                    {
                        double minx,miny,minz,maxx,maxy,maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        int tmpent = 0;
                        int tmptype = axEWdraw3.GetPlankType(g_axismoveid);
                        for (int i = 0; i < g_axismovespacesize; i++)
                        {
                            axEWdraw3.GetCabinetSpace(i, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            //判断左,右侧
                            double midx = (minx + maxx) / 2.0;
                            double midy = (miny + maxy) / 2.0;
                            double midz = (minz + maxz) / 2.0;
                            if (tmptype == 1)
                            {
                                if (midx < g_axismoveplank.minx)
                                {//左侧
                                    if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                               g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                               g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                                    {
                                        CPlank abnd = new CPlank();
                                        abnd.minx = minx;
                                        abnd.miny = miny;
                                        abnd.minz = minz;
                                        //
                                        abnd.maxx = maxx;
                                        abnd.maxy = maxy;
                                        abnd.maxz = maxz;
                                        g_axismoveleftup.Add(abnd);
                                    }
                                }
                                else
                                {//右侧
                                    CPlank abnd = new CPlank();
                                    abnd.minx = minx;
                                    abnd.miny = miny;
                                    abnd.minz = minz;
                                    //
                                    abnd.maxx = maxx;
                                    abnd.maxy = maxy;
                                    abnd.maxz = maxz;
                                    g_axismoverightdown.Add(abnd);
                                }
                            }
                            else if (tmptype == 0)
                            {
                                if (midz > g_axismoveplank.maxz)
                                {//左侧
                                    if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                               g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                               g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                                    {
                                        CPlank abnd = new CPlank();
                                        abnd.minx = minx;
                                        abnd.miny = miny;
                                        abnd.minz = minz;
                                        //
                                        abnd.maxx = maxx;
                                        abnd.maxy = maxy;
                                        abnd.maxz = maxz;
                                        g_axismoveleftup.Add(abnd);
                                    }
                                }
                                else
                                {//右侧
                                    CPlank abnd = new CPlank();
                                    abnd.minx = minx;
                                    abnd.miny = miny;
                                    abnd.minz = minz;
                                    //
                                    abnd.maxx = maxx;
                                    abnd.maxy = maxy;
                                    abnd.maxz = maxz;
                                    g_axismoverightdown.Add(abnd);
                                }
                            }
                        }
                        int entsize = axEWdraw3.GetEntSize();
                        for (int i = 1; i <= entsize; i++)
                        {
                            tmpent = axEWdraw3.GetEntID(i);
                            if (tmpent > 0)
                            {
                                if (axEWdraw3.IsGroup(tmpent))
                                {
                                    string name = axEWdraw3.GetGroupName(tmpent);
                                    if (name.IndexOf("_ct_") >= 0 || name.IndexOf("_yt_") >= 0 || name.IndexOf("_fjia_") >= 0 || name.IndexOf("_bogu_") >= 0)
                                    {
                                        double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                                        tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                                        axEWdraw3.GetGroupBndPt(tmpent, 0,ref tmpx1, ref tmpy1, ref tmpz1);
                                        axEWdraw3.GetGroupBndPt(tmpent, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                                        double midx = (tmpx1 + tmpx2) / 2.0;
                                        double midy = (tmpy1 + tmpy2) / 2.0;
                                        double midz = (tmpz1 + tmpz2) / 2.0;
                                        //左侧
                                        for (int j = 0; j < g_axismoveleftup.Count; j++)
                                        {
                                            if (midx > ((CPlank)g_axismoveleftup[j]).minx && midy > ((CPlank)g_axismoveleftup[j]).miny && midz > ((CPlank)g_axismoveleftup[j]).minz &&
                                                midx < ((CPlank)g_axismoveleftup[j]).maxx && midy < ((CPlank)g_axismoveleftup[j]).maxy && midz < ((CPlank)g_axismoveleftup[j]).maxz
                                                )
                                            {
                                                SNode node = new SNode();//2020-02-14
                                                node.code = tmpent;
                                                if (name.IndexOf("_ct_") >= 0)
                                                    node.type = 0;//抽屉类型
                                                else if (name.IndexOf("_yt_") >= 0)
                                                    node.type = 1;//衣通类型
                                                else if (name.IndexOf("_fjia_") >= 0)//F架
                                                    node.type = 2;
                                                else if (name.IndexOf("_bogu_") >= 0)//博古
                                                    node.type = 3;
                                                g_axismoveidsleftup.Add(node);//tmpent
                                            }
                                        }
                                        //右侧
                                        for (int j = 0; j < g_axismoverightdown.Count; j++)
                                        {
                                            if (midx > ((CPlank)g_axismoverightdown[j]).minx && midy > ((CPlank)g_axismoverightdown[j]).miny && midz > ((CPlank)g_axismoverightdown[j]).minz &&
                                                midx < ((CPlank)g_axismoverightdown[j]).maxx && midy < ((CPlank)g_axismoverightdown[j]).maxy && midz < ((CPlank)g_axismoverightdown[j]).maxz
                                                )
                                            {
                                                SNode node = new SNode();//2020-02-14
                                                node.code = tmpent;
                                                if (name.IndexOf("_ct_") >= 0)
                                                    node.type = 0;//抽屉类型
                                                else if (name.IndexOf("_yt_") >= 0)
                                                    node.type = 1;//抽屉类型
                                                else if(name.IndexOf("_fjia_")>=0)//F架
                                                    node.type = 2;
                                                else if (name.IndexOf("_bogu_") >= 0)//F架
                                                    node.type = 3;
                                                g_axismoveidsrightdown.Add(node);//tmpent
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        g_axismoveleftup.Clear();
                        g_axismoverightdown.Clear();

                    }
                    break;
                case 3:
                    {
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        int tmpent = 0;
                        int tmptype = axEWdraw3.GetPlankType(g_axismoveid);
                        for (int i = 0; i < g_axismovespacesize; i++)
                        {
                            axEWdraw3.GetCabinetSpace(i, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                            //判断左,右侧
                            double midx = (minx + maxx) / 2.0;
                            double midy = (miny + maxy) / 2.0;
                            double midz = (minz + maxz) / 2.0;

                            if (tmptype == 1)
                            {
                                if (midx < g_axismoveplank.minx)
                                {//左侧
                                    if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                               g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                               g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                                    {
                                        CPlank abnd = new CPlank();
                                        abnd.minx = minx;
                                        abnd.miny = miny;
                                        abnd.minz = minz;
                                        //
                                        abnd.maxx = maxx;
                                        abnd.maxy = maxy;
                                        abnd.maxz = maxz;
                                        g_axismoveleftup.Add(abnd);
                                    }
                                }
                                else
                                {//右侧
                                    CPlank abnd = new CPlank();
                                    abnd.minx = minx;
                                    abnd.miny = miny;
                                    abnd.minz = minz;
                                    //
                                    abnd.maxx = maxx;
                                    abnd.maxy = maxy;
                                    abnd.maxz = maxz;
                                    g_axismoverightdown.Add(abnd);
                                }
                            }
                            else if (tmptype == 0)
                            {
                                if (midz > g_axismoveplank.maxz)
                                {//左侧
                                    if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                               g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                               g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                                    {
                                        CPlank abnd = new CPlank();
                                        abnd.minx = minx;
                                        abnd.miny = miny;
                                        abnd.minz = minz;
                                        //
                                        abnd.maxx = maxx;
                                        abnd.maxy = maxy;
                                        abnd.maxz = maxz;
                                        g_axismoveleftup.Add(abnd);
                                    }
                                }
                                else
                                {//右侧
                                    CPlank abnd = new CPlank();
                                    abnd.minx = minx;
                                    abnd.miny = miny;
                                    abnd.minz = minz;
                                    //
                                    abnd.maxx = maxx;
                                    abnd.maxy = maxy;
                                    abnd.maxz = maxz;
                                    g_axismoverightdown.Add(abnd);
                                }
                            }
                        }
                        if (g_axismoveidsleftup.Count > 0)
                        {
                            //左侧
                            for (int i = 0; i < g_axismoveidsleftup.Count; i++)
                            {
                                double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                                tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                                axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsleftup[i]).code, 0, ref tmpx1, ref tmpy1, ref tmpz1);
                                axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsleftup[i]).code, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                                for (int j = 0; j < g_axismoveleftup.Count; j++)
                                {
                                    if (axEWdraw3.IsIntersectAABBs(tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2,
                                                                ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                                                ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz))
                                    {
                                        if (((SNode)g_axismoveidsleftup[i]).type == 0)
                                        {
                                            UpdateCTWidth(((SNode)g_axismoveidsleftup[i]).code,
                                                ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                                ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz);
                                        }
                                        else if (((SNode)g_axismoveidsleftup[i]).type == 1)//衣通
                                        {
                                            UpdateYitong(((SNode)g_axismoveidsleftup[i]).code, ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                               ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz);
                                        }else if(((SNode)g_axismoveidsleftup[i]).type == 2)//F架
                                        {
                                            UpdateFjiaWidth(((SNode)g_axismoveidsleftup[i]).code,
                                                ((CPlank)g_axismoveleftup[j]).minx, //2020-04-13
                                                ((CPlank)g_axismoveleftup[j]).maxx-((CPlank)g_axismoveleftup[j]).minx,
                                                ((CPlank)g_axismoveleftup[j]).maxy - ((CPlank)g_axismoveleftup[j]).miny
                                                );
                                        }else if (((SNode)g_axismoveidsleftup[i]).type == 3)//博古
                                        {
                                            UpdateBOGUWidth(((SNode)g_axismoveidsleftup[i]).code,
                                                ((CPlank)g_axismoveleftup[j]).minx,//2020-04-13
                                                ((CPlank)g_axismoveleftup[j]).maxx - ((CPlank)g_axismoveleftup[j]).minx,
                                                ((CPlank)g_axismoveleftup[j]).maxy - ((CPlank)g_axismoveleftup[j]).miny,
                                                ((CPlank)g_axismoveleftup[j]).maxz - ((CPlank)g_axismoveleftup[j]).minz
                                                );
                                        }
                                    }
                                }

                            }
                        }
                        if (g_axismoveidsrightdown.Count > 0)
                        {
                            //右侧
                            for (int i = 0; i < g_axismoveidsrightdown.Count; i++)
                            {
                                double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                                tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                                axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsrightdown[i]).code, 0, ref tmpx1, ref tmpy1, ref tmpz1);
                                axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsrightdown[i]).code, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                                for (int j = 0; j < g_axismoverightdown.Count; j++)
                                {
                                    if (axEWdraw3.IsIntersectAABBs(tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2,
                                                                ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                                                ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz))
                                    {
                                        if (((SNode)g_axismoveidsrightdown[i]).type == 0)
                                        {
                                            UpdateCTWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                                ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                                ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz);
                                        }
                                        else if (((SNode)g_axismoveidsrightdown[i]).type == 1)//衣通
                                        {//衣通
                                            UpdateYitong(((SNode)g_axismoveidsrightdown[i]).code, ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                                ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz);
                                        }

                                        else if (((SNode)g_axismoveidsrightdown[i]).type == 2)//F架
                                        {
                                            UpdateFjiaWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                                ((CPlank)g_axismoverightdown[j]).minx,//2020-04-13
                                                ((CPlank)g_axismoverightdown[j]).maxx - ((CPlank)g_axismoverightdown[j]).minx,
                                                ((CPlank)g_axismoverightdown[j]).maxy - ((CPlank)g_axismoverightdown[j]).miny
                                                );
                                        }
                                        else if (((SNode)g_axismoveidsrightdown[i]).type == 3)//博古
                                        {
                                            UpdateBOGUWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                                ((CPlank)g_axismoverightdown[j]).minx,//2020-04-13
                                                ((CPlank)g_axismoverightdown[j]).maxx - ((CPlank)g_axismoverightdown[j]).minx,
                                                ((CPlank)g_axismoverightdown[j]).maxy - ((CPlank)g_axismoverightdown[j]).miny,
                                                ((CPlank)g_axismoverightdown[j]).maxz - ((CPlank)g_axismoverightdown[j]).minz
                                                );
                                        }
                                    }
                                }
                            }
                        }
                        g_axismoveleftup.Clear();
                        g_axismoverightdown.Clear();
                        if (axEWdraw3.IsBeginUndo())//2020-03-16
                            axEWdraw3.EndUndo();

                    }
                    break;
                case 4:
                case 5:
                    {
                        Form17 fm17 = new Form17();
                        if (fm17.ShowDialog() == DialogResult.OK)
                        {
                            double minX, minY, minZ, maxX, maxY, maxZ;
                            minX = minY = minZ = maxX = maxY = maxZ = 0.0;
                            double insx, insy, insz;
                            insx = insy = insz = 0.0;
                            axEWdraw3.SetCabinetSpaceDepthLimit(100.001);//2020-04-13
                            axEWdraw3.GetNearestCabinetSpace(g_winx, g_winy, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                            axEWdraw3.EyeLineInsAABBs(g_winx, g_winy, minX, minY, minZ, maxX, maxY, maxZ, ref insx, ref insy, ref insz);
                            axEWdraw3.SetCabinetSpaceDepthLimit(0.0);//2020-04-13
                            //创建衣通 2020-02-13
                            axEWdraw3.BeginUndo(true);
                            MakeYitong(minX, maxX, maxZ, minY, maxY,fm17.m_topspace, fm17.m_depthspace, fm17.m_rad, fm17.m_isautodepath);
                            if (axEWdraw3.IsBeginUndo())//2020-03-17
                                axEWdraw3.EndUndo();

                        }

                    }
                    break;
                case 6:
                    {
                        
                        double fjiawidth = g_smaxx - g_sminx;
                        double fjiadepath = g_smaxy - g_sminy;
                        axEWdraw3.BeginUndo(true);
                        UpdateFjiaWidth(0, g_sminx,fjiawidth, fjiadepath);//2020-04-13
                        if (axEWdraw3.IsBeginUndo())//2020-03-17
                            axEWdraw3.EndUndo();
                    }
                    break;
                case 7:
                    {//2020-03-04
                        double boguwidth = g_smaxx - g_sminx;
                        double bogudepath = g_smaxy - g_sminy;
                        double boguheight = g_smaxz - g_sminz;
                        axEWdraw3.BeginUndo(true);
                        UpdateBOGUWidth(0, g_sminx,boguwidth, bogudepath, boguheight);//2020-04-13
                        if (axEWdraw3.IsBeginUndo())//2020-03-17
                            axEWdraw3.EndUndo();
                    }
                    break;
            }
            g_timerproc = 0;
        }
        //取得抽屉有效的上下界
        private void GetCTMinMaxZ(int selfent,double minx,double miny,double minz,
                                  double maxx,double maxy,double maxz,double orgz,ref double octminz,ref double octmaxz)
        {
            double left = 0;
            double right = 0;
            double up = 0;
            double bottom = 0;
            double thickness = 0;
            GetCTParamaters(selfent, ref left, ref right, ref up, ref bottom, ref thickness);
            int entsize = axEWdraw3.GetEntSize();
            int ent = 0;
            double ominx, ominy, ominz, omaxx, omaxy, omaxz;
            ominx = ominy = ominz = omaxx = omaxy = omaxz = 0.0;
            double gminx, gminy, gminz, gmaxx, gmaxy, gmaxz;
            gminx = gminy = gminz = gmaxx = gmaxy = gmaxz = 0.0;

            double midx, midy, midz;
            midx = midy = midz = 0.0;
            double orgmidz = orgz;//(ominz + omaxz) / 2.0;
            bool isminz = false;
            bool ismaxz = false;
            int tmpjs1 = 0;
            int tmpjs2 = 0;
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw3.GetEntID(i);
                if (axEWdraw3.IsDisplayed(ent))
                {
                    CPlank aplk = new CPlank();
                    if (axEWdraw3.IsGroup(ent) && ent != selfent)
                    {
                        axEWdraw3.GetGroupBndPt(ent, 0, ref gminx, ref gminy, ref gminz);//得到组的包围盒的拐点 0：底面左下角三维坐标
                        axEWdraw3.GetGroupBndPt(ent, 6, ref gmaxx, ref gmaxy, ref gmaxz);//得到组的包围盒的拐点 6：顶面右上角三维坐标
                        //
                        midx = (gmaxx + gminx) / 2.0;
                        midy = (gmaxy + gminy) / 2.0;
                        midz = (gmaxz + gminz) / 2.0;
                        if (midx >= minx && midy >= miny && midz >= minz &&
                            midx <= maxx && midy <= maxy && midz <= maxz
                            )
                        {
                            if (orgmidz < midz)
                            {

                                if (tmpjs1 == 0)
                                {
                                    octmaxz = gminz;
                                    ismaxz = true;
                                }
                                else if (octmaxz > gminz)
                                {
                                    octmaxz = gminz;
                                }
                                tmpjs1++;
                            }
                            else
                            {
                                if (tmpjs2 == 0)
                                {
                                    octminz = gmaxz;
                                    isminz = true;
                                }
                                else if(octmaxz<gminz)
                                {
                                    octminz = gmaxz;
                                }
                                tmpjs2++;
                            }
                        }
                    }
                }
            }
            if (!ismaxz)
                octmaxz = maxz;
            if(!isminz)
                octminz = minz;
        }

        //取得柜体内部的组对象ID集
        private void GetCabinectInside(int orgid,ref ArrayList ids)
        {
            double gminx, gminy, gminz, gmaxx, gmaxy, gmaxz;
            gminx = gminy = gminz = gmaxx = gmaxy = gmaxz = 0.0;

            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;

            double midx, midy, midz;
            midx = midy = midz = 0.0;
            //
            axEWdraw3.GetEntBoundingBox(orgid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            int ent = 0;
            int entsize = axEWdraw3.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                ent = axEWdraw3.GetEntID(i);
                if (axEWdraw3.IsDisplayed(ent))
                {
                    if (axEWdraw3.IsGroup(ent) && ent != orgid)
                    {
                        axEWdraw3.GetGroupBndPt(ent, 0, ref gminx, ref gminy, ref gminz);//得到组的包围盒的拐点 0：底面左下角三维坐标
                        axEWdraw3.GetGroupBndPt(ent, 6, ref gmaxx, ref gmaxy, ref gmaxz);//得到组的包围盒的拐点 6：顶面右上角三维坐标
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
        }
        private void button23_Click(object sender, EventArgs e)
        {
            double x1, y1, z1, x2, y2, z2;
            x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            HideWJ();//消隐五金 2020-03-20
            axEWdraw3.SetOSNAPLimit(20);
            axEWdraw3.SetOSNAPMode(0);
            axEWdraw3.EnableGroupOsnapOnlyBnd(true);
            label1.Text = "选择要移动的柜体:";
            int movent = axEWdraw3.GetOneEntSel(ref x1, ref y1, ref z1);
            if (g_isquit)
            {
                return;
            }
            label1.Text = "选择要移动的角点:";
            if (axEWdraw3.GetPoint(ref x1, ref y1, ref z1))
            {
                if (!g_isquit)
                {
                    label1.Text = "选择目标角点:";
                    if (axEWdraw3.GetPoint(ref x2, ref y2, ref z2))
                    {
                        if (!g_isquit)
                        {
                            ArrayList insidecompents = new ArrayList();
                            GetCabinectInside(movent, ref insidecompents);//2020-04-09
                            axEWdraw3.BeginUndo(false);
                            //2020-04-09
                            if (insidecompents.Count > 0)
                            {
                                for (int i = 0; i < insidecompents.Count; i++)
                                {
                                    axEWdraw3.MoveTo((int)insidecompents[i], new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 });
                                }
                            }
                            //
                            axEWdraw3.MoveTo(movent, new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 });
                            axEWdraw3.EndUndo();
                            axEWdraw3.ClearSelected();
                            label1.Text = "";
                        }
                    }
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            double x, y, z, x1, y1, z1, x2, y2, z2;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0;
            HideWJ();//消隐五金 2020-03-20
            int ent = axEWdraw3.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                axEWdraw3.BeginUndo(false);
                ArrayList ids = new ArrayList();
                GetCabinectInside(ent, ref ids);
                axEWdraw3.GetGroupInsPt(ent, ref x, ref y, ref z);
                axEWdraw3.GetGroupBndPt(ent, 0, ref x1, ref y1, ref z1);
                axEWdraw3.GetGroupBndPt(ent, 2, ref x2, ref y2, ref z2);
                x = (x1 + x2) / 2.0;//2020-04-24
                y = (y1 + y2) / 2.0;
                ids.Add(ent);
                if (ids.Count > 0)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (axEWdraw3.IsGroup((int)ids[i]))
                        {

                            axEWdraw3.Rotate((int)ids[i], new object[] { x, y, z }, new object[] { 0, 0, 1 }, -90);
                        }
                    }
                }
                ids = null;
                axEWdraw3.EndUndo();
            }

        }

        private void button25_Click(object sender, EventArgs e)
        {
            double x, y, z,x1,y1,z1,x2,y2,z2;
            x = y = z = x1= y1 = z1 = x2 = y2 = z2 = 0;
            HideWJ();//消隐五金 2020-03-20
            int ent = axEWdraw3.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                axEWdraw3.BeginUndo(false);
                ArrayList ids = new ArrayList();
                GetCabinectInside(ent, ref ids);
                axEWdraw3.GetGroupInsPt(ent, ref x, ref y, ref z);
                axEWdraw3.GetGroupBndPt(ent, 0, ref x1, ref y1, ref z1);
                axEWdraw3.GetGroupBndPt(ent, 2, ref x2, ref y2, ref z2);
                x = (x1+x2)/2.0;//2020-04-24
                y = (y1 + y2) / 2.0;
                ids.Add(ent);
                if (ids.Count > 0)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (axEWdraw3.IsGroup((int)ids[i]))
                        {
                            axEWdraw3.Rotate((int)ids[i], new object[] { x, y, z }, new object[] { 0, 0, 1 }, 90);
                        }
                    }
                }
                ids = null;
                axEWdraw3.EndUndo();
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            double x, y, z, x1, y1, z1, x2, y2, z2;
            x = y = z = x1 = y1 = z1 = x2 = y2 = z2 = 0;
            HideWJ();//消隐五金 2020-03-20
            int ent = axEWdraw3.GetOneEntSel(ref x, ref y, ref z);
            if (ent > 0)
            {
                axEWdraw3.BeginUndo(false);
                ArrayList ids = new ArrayList();
                GetCabinectInside(ent, ref ids);
                axEWdraw3.GetGroupInsPt(ent, ref x, ref y, ref z);
                axEWdraw3.GetGroupBndPt(ent, 0, ref x1, ref y1, ref z1);
                axEWdraw3.GetGroupBndPt(ent, 2, ref x2, ref y2, ref z2);
                x = (x1 + x2) / 2.0;//2020-04-24
                y = (y1 + y2) / 2.0;
                ids.Add(ent);
                if (ids.Count > 0)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (axEWdraw3.IsGroup((int)ids[i]))
                        {

                            axEWdraw3.Rotate((int)ids[i], new object[] { x, y, z }, new object[] { 0, 0, 1 }, 180);
                        }
                    }
                }
                ids = null;
                axEWdraw3.EndUndo();
            }

        }

        private void button27_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.SetViewCondition(5);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.SetViewCondition(6);

        }

        private void button29_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.SetViewCondition(3);

        }

        private void button30_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.SetViewCondition(4);

        }

        private void button31_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.SetViewCondition(8);
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 13)
            {
                textBox2.Visible = false;
                axEWdraw3.Focus();
                axEWdraw3.SendCommandStr(textBox2.Text);
            }
        }

        private void axEWdraw3_EndAxisMoveEnt(object sender, AxEWDRAWLib._DAdrawEvents_EndAxisMoveEntEvent e)
        {
            g_axismoveid = e.ent;
            
        }

        private int GetWJNum(double val)
        {
            if (val < 100.0 || Math.Abs(val - 100.0) < 0.001)
                return 1;
            else if ((val > 100.0 && val < 400) || Math.Abs(val - 400.0) < 0.001)
                return 2;
            else if ((val > 400.0 && val < 600) || Math.Abs(val - 600.0) < 0.001)
                return 3;
            else if (val > 600.0)
                return 4;
            return 2;
        }

        private int GetArcYXB(int eid,double fillet, bool isv0, bool isv1, bool isv2, bool isv3,bool ischamfer = false)
        {
            axEWdraw3.BeginUndo(true);
            double w, h;
            double thickness = 18.0;
            double width ,height,area;
            w = h = width = height = area = 0;
            string cname = "";
            string gname = "";//2020-02-27
            if (eid > 0)
            {
                axEWdraw3.GetPlankWHTA(eid, ref width, ref height, ref thickness, ref area, ref cname,ref gname);
                w = width;
                h = height;
            }
            else return 0;


            double minval = Math.Min(w, h);
            if (fillet > minval)
                fillet = minval * 0.75;
            //
            if (fillet > minval) return 0;
            if ((isv0 && isv3) && (fillet * 2.0 > h))
                return 0;
            if ((isv1 && isv2) && (fillet * 2.0 > h))
                return 0;

            if ((isv0 && isv1) && (fillet * 2.0 > w))
                return 0;
            if ((isv2 && isv3) && (fillet * 2.0 > w))
                return 0;

            int rect = axEWdraw3.Rectangle(0, 0, w, h, 0);
            axEWdraw3.SetEntityInvisible(rect, true);
            //逆时针顶点计算
            //顶点一
            SPoint v0 = new SPoint();
            v0.x = 0;
            v0.y = 0;
            v0.z = 0;
            //顶点二
            SPoint v1 = new SPoint();
            v0.x = w;
            v0.y = 0;
            v0.z = 0;
            //顶点三
            SPoint v2 = new SPoint();
            v0.x = w;
            v0.y = h;
            v0.z = 0;
            //顶点四
            SPoint v3 = new SPoint();
            v0.x = 0;
            v0.y = h;
            v0.z = 0;
            //
            double rx0, ry0, rz0, rx1, ry1, rz1;
            rx0 = ry0 = rz0 = rx1 = ry1 = rz1 = 0.0;
            if (isv0)
            {
                rx0 = 0;
                ry0 = 0.001;
                rx1 = 0.001;
                ry1 = 0.0;
                if(!ischamfer)
                    axEWdraw3.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw3.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet,fillet);
            }
            if(isv1)
            {
                rx0 = w-0.001;
                ry0 = 0;
                rx1 = w;
                ry1 = 0.001;
                if (!ischamfer)
                    axEWdraw3.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw3.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            if (isv2)
            {
                rx0 = w - 0.001;
                ry0 = h;
                rx1 = w;
                ry1 = h-0.001;
                if (!ischamfer)
                    axEWdraw3.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw3.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            if (isv3)
            {
                rx0 = 0.001;
                ry0 = h;
                rx1 = 0;
                ry1 = h - 0.001;
                if (!ischamfer)
                    axEWdraw3.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw3.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            axEWdraw3.NoDisplayWhenCreate3DSolid(true);
            int entface = axEWdraw3.EntToFace(rect, true);
            int orgplank = axEWdraw3.Prism(entface, thickness, new object[] { 0, 0, 1 });
            axEWdraw3.Delete(entface);
            axEWdraw3.NoDisplayWhenCreate3DSolid(false);
            if (eid > 0)
            {
                double minx,miny,minz,maxx,maxy,maxz;
                minx = miny = minz = maxx = maxy = maxz = 0.0;
                axEWdraw3.GetEntBoundingBox(eid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                
                int ptype = axEWdraw3.GetPlankType(eid);
                switch (ptype)
                {
                    case 0:
                        {
                            axEWdraw3.MoveTo(orgplank, new object[] { w, h, 0 }, new object[] { maxx, maxy, minz });
                        }
                        break;
                    case 1://侧板
                        {
                            if (isv1 || isv2)
                            {
                                axEWdraw3.MoveTo(orgplank, new object[] { 0, 0, 0 }, new object[] { maxx, miny, minz });
                                axEWdraw3.Rotate(orgplank, new object[] { maxx, miny, minz }, new object[] { 0, -1, 0 }, 90);
                            }
                            else if (isv0 || isv3)
                            {
                                axEWdraw3.MoveTo(orgplank, new object[] { w, 0, 0 }, new object[] { minx, miny, minz });
                                axEWdraw3.Rotate(orgplank, new object[] { minx, miny, minz }, new object[] { 0, -1, 0 }, -90);
                            }
                            
                        }
                        break;
                    case 2://面板
                        {
                            axEWdraw3.MoveTo(orgplank, new object[] { 0, h, 0 }, new object[] { minx, miny, minz });//miny+thickness
                            axEWdraw3.Rotate(orgplank, new object[] { minx, miny, minz }, new object[] { 1, 0, 0 }, -90);
                        }
                        break;
                }
                axEWdraw3.Replace3DSolid(orgplank, eid);
                int wjsize = axEWdraw3.GetPlankWJSize(eid);
                int wjtype,wz,side,sl,yl;
                wjtype = wz = side = sl = yl = 0;
                int wjtype1, wz1, side1, sl1, yl1;
                wjtype1 = wz1 = side1 = sl1 = yl1 = 0;

                double qk, hk;
                qk = hk = 0;
                double qk1, hk1;
                qk1 = hk1 = 0;
                //取得已有三合一参数
                for(int i = 0;i<wjsize;i++)
                {
                    axEWdraw3.GetPlankWJ(eid,i, ref wjtype, ref wz, ref side, ref qk, ref hk, ref sl, ref yl);
                    if(wjtype == 5016)
                    {
                        break;
                    }
                }
                //取得已有木楔参数
                bool ishavems = false;
                for (int i = 0; i < wjsize; i++)
                {
                    axEWdraw3.GetPlankWJ(eid, i, ref wjtype1, ref wz1, ref side1, ref qk1, ref hk1, ref sl1, ref yl1);
                    if (wjtype1 == 5015)
                    {
                        ishavems = true;
                        break;
                    }
                }

                //判断五金
                axEWdraw3.DeletePlankWJ(eid);
                if (isv1)
                {
                    switch (ptype)
                    {
                        case 0://层板
                            {
                                //后边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 5, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 5, qk1, hk1, sl1, yl1);
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk, hk, sl, yl);// 
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1, hk1, sl1, yl1);

                            }
                            break;
                        case 1://侧板
                            {
                                //后边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 5, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 5, qk1, hk1, sl1, yl1);
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1, hk1, sl1, yl1);
                            }
                            break;
                        case 2://面板
                            {
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1, hk1, sl1, yl1);
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1, hk1, sl1, yl1);
                            }
                            break;
                    }
                }else if (isv0)
                {
                    switch (ptype)
                    {
                        case 0://层板
                            {
                                //后边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 5, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 5, qk1, hk1, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1, hk1, sl1, yl1);

                            }
                            break;
                        case 1://侧板
                            {
                                //后边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 5, qk, hk,sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 5, qk1, hk1, sl1, yl1);
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1, hk1, sl1, yl1);
                            }
                            break;
                        case 2://面板
                            {
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1, hk1, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1, hk1, sl1, yl1);
                            }
                            break;
                    }
                }
                else if ((isv0 && isv1))//前部顶点
                {
                    switch (ptype)
                    {
                        case 0://层板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk + fillet, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1 + fillet, hk1 , sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk + fillet, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1 + fillet, hk1, sl1, yl1);
                            }
                            break;
                        case 1://侧板
                            {
                                //上边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 2, qk + fillet, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 2, qk1 + fillet, hk1, sl1, yl1);
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk + fillet, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1 + fillet, hk1, sl1, yl1);
                            }
                            break;
                        case 2://面板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk + fillet, hk , sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1 + fillet, hk1, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk + fillet, hk, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1 + fillet, hk1, sl1, yl1);
                            }
                            break;
                    }
                }
                else if (isv2 && isv3)//后部两顶点
                {
                    switch (ptype)
                    {
                        case 0://层板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1, hk1 + fillet, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1, hk1 + fillet, sl1, yl1);
                            }
                            break;
                        case 1://侧板
                            {
                                //上边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 2, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 2, qk1, hk1 + fillet, sl1, yl1);
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1, hk1 + fillet, sl1, yl1);
                            }
                            break;
                        case 2://面板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1, hk1 + fillet, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1, hk1 + fillet, sl1, yl1);
                            }
                            break;
                    }
                }
                else if (isv0 && isv1 && isv2 && isv3)//四边导角
                {
                    switch (ptype)
                    {
                        case 0://层板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1 + fillet, hk1 + fillet, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1 + fillet, hk1 + fillet, sl1, yl1);
                            }
                            break;
                        case 1://侧板
                            {
                                //上边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 2, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 2, qk1 + fillet, hk1 + fillet, sl1, yl1);
                                //下边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 3, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 3, qk1 + fillet, hk1 + fillet, sl1, yl1);
                            }
                            break;
                        case 2://面板
                            {
                                //左边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 0, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 0, qk1 + fillet, hk1 + fillet, sl1, yl1);
                                //右边
                                axEWdraw3.AddPlankWJ(eid, 5016, wz, 1, qk + fillet, hk + fillet, sl, yl);
                                if (ishavems)
                                    axEWdraw3.AddPlankWJ(eid, 5015, wz1, 1, qk1 + fillet, hk1 + fillet, sl1, yl1);
                            }
                            break;
                    }
                }
            }
            axEWdraw3.EndUndo();
            return 0;
        }

        //初始化异型板
        private void InitYxbs()
        {
            if (g_yxbs == null)
                g_yxbs = new ArrayList();
            if (g_yxbs.Count > 0)
                g_yxbs.Clear();
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "右弧板";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 600.0));
                ayxb.m_paras.Add(new MyYXBPara("深度", 300.0));
                ayxb.m_paras.Add(new MyYXBPara("半径", 260));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "左弧板";
                ayxb.m_paras.Add(new MyYXBPara("宽度",600));
                ayxb.m_paras.Add(new MyYXBPara("深度", 300));
                ayxb.m_paras.Add(new MyYXBPara("半径", 260));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "前双弧台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "后双弧台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "圆弧台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "右导角板";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 600.0));
                ayxb.m_paras.Add(new MyYXBPara("深度", 300.0));
                ayxb.m_paras.Add(new MyYXBPara("半径", 260));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "左导角板";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 600));
                ayxb.m_paras.Add(new MyYXBPara("深度", 300));
                ayxb.m_paras.Add(new MyYXBPara("半径", 260));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "前双导角台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "后双导角台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }
            {//
                MyYXB ayxb = new MyYXB();
                ayxb.m_paras = new ArrayList();
                ayxb.m_name = "导角台面";
                ayxb.m_paras.Add(new MyYXBPara("宽度", 500));
                ayxb.m_paras.Add(new MyYXBPara("深度", 250));
                ayxb.m_paras.Add(new MyYXBPara("半径", 20));
                g_yxbs.Add(ayxb);
            }

            //...待续
        }

        //根据索引与名称取得参数 2020-02-10
        private bool GetYXBPara(int inx,string name,ref double val)
        {
            if (g_yxbs.Count > 0)
            {
                for (int i = 0; i < g_yxbs.Count; i++)
                {
                    if(i == inx)
                    {
                        MyYXB ayxb = (MyYXB)g_yxbs[i];
                        for(int j = 0; j < ayxb.m_paras.Count;j++)
                        {
                            if (((MyYXBPara)ayxb.m_paras[j]).m_name == name)
                            {
                                val = ((MyYXBPara)ayxb.m_paras[j]).m_val;
                                return true;
                            }
                        }
                    }
                }
                
            }
            return false;
        }

        //创建异型板 2020-02-10
        private bool CreateYXB(int inx,int eid)
        {
            if (g_yxbs.Count > 0)
            {
                if (inx >= 0 && inx < g_yxbs.Count)
                {
                    if (eid > 0)
                    {
                        double rad = 0.0;
                        switch (inx)
                        {
                            case 0:
                                {//右弧板
                                    GetYXBPara(inx,"半径",ref rad);
                                    GetArcYXB(eid, rad, false, true, false, false, false);//
                                }
                                break;
                            case 1:
                                {//左弧板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, false, false, false, false);//
                                }
                                break;
                            case 2:
                                {//前双弧板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, true, false, false, false);//
                                }
                                break;
                            case 3:
                                {//后双弧板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, false, false, true, true, false);//
                                }
                                break;
                            case 4:
                                {//弧板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, true, true, true, false);//
                                }
                                break;
                            case 5:
                                {//右导角板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, false, true, false, false, true);//
                                }
                                break;
                            case 6:
                                {//左导角板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, false, false, false, true);//
                                }
                                break;
                            case 7:
                                {//前导角板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, true, false, false, true);//
                                }
                                break;
                            case 8:
                                {//后导角板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, false, false, true, true, true);//
                                }
                                break;
                            case 9:
                                {//导角板
                                    GetYXBPara(inx, "半径", ref rad);
                                    GetArcYXB(eid, rad, true, true, true, true, true);//
                                }
                                break;

                        }
                        return true;
                    }
                }
            }
            return false;
        }


        //创建异型板 2020-02-10
        private bool MakePreYXB(int inx,double nrad = -1.0)
        {
            if (g_yxbs.Count > 0)
            {
                if (inx >= 0 && inx < g_yxbs.Count)
                {
                    //if (eid > 0)
                    {
                        double rad = 0.0;
                        double w, h;
                        w = h = 0;
                        switch (inx)
                        {
                            case 0:
                                {//右弧板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);
                                    PreArcYXB(rad, w, h, false, true, false, false, false);//
                                }
                                break;
                            case 1:
                                {//左弧板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);
                                    PreArcYXB(rad, w, h, true, false, false, false, false);//
                                }
                                break;
                            case 2:
                                {//前双弧板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, true, true, false, false, false);//
                                }
                                break;
                            case 3:
                                {//后双弧板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, false, false, true, true, false);//
                                }
                                break;
                            case 4:
                                {//弧板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, true, true, true, true, false);//
                                }
                                break;
                            case 5:
                                {//右导角板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, false, true, false, false, true);//
                                }
                                break;
                            case 6:
                                {//左导角板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, true, false, false, false, true);//
                                }
                                break;
                            case 7:
                                {//前导角板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, true, true, false, false, true);//
                                }
                                break;
                            case 8:
                                {//后导角板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, false, false, true, true, true);//
                                }
                                break;
                            case 9:
                                {//导角板
                                    if (nrad < 0.0)
                                        GetYXBPara(inx, "半径", ref rad);
                                    else
                                        rad = nrad;
                                    GetYXBPara(inx, "宽度", ref w);
                                    GetYXBPara(inx, "深度", ref h);

                                    PreArcYXB(rad, w, h, true, true, true, true, true);//
                                }
                                break;

                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private int PreArcYXB(double fillet,double w,double h, bool isv0, bool isv1, bool isv2, bool isv3, bool ischamfer = false)
        {
            double thickness = 18.0;
            double width, height, area;
            string cname = "";
            axEWdraw2.DeleteAll();

            double minval = Math.Min(w, h);
            if (fillet > minval)
                fillet = minval * 0.75;
            //
            if (fillet > minval) return 0;
            if ((isv0 && isv3) && (fillet * 2.0 > h))
                return 0;
            if ((isv1 && isv2) && (fillet * 2.0 > h))
                return 0;

            if ((isv0 && isv1) && (fillet * 2.0 > w))
                return 0;
            if ((isv2 && isv3) && (fillet * 2.0 > w))
                return 0;

            int rect = axEWdraw2.Rectangle(0, 0, w, h, 0);
            axEWdraw2.SetEntityInvisible(rect, true);
            //逆时针顶点计算
            //顶点一
            SPoint v0 = new SPoint();
            v0.x = 0;
            v0.y = 0;
            v0.z = 0;
            //顶点二
            SPoint v1 = new SPoint();
            v0.x = w;
            v0.y = 0;
            v0.z = 0;
            //顶点三
            SPoint v2 = new SPoint();
            v0.x = w;
            v0.y = h;
            v0.z = 0;
            //顶点四
            SPoint v3 = new SPoint();
            v0.x = 0;
            v0.y = h;
            v0.z = 0;
            //
            double rx0, ry0, rz0, rx1, ry1, rz1;
            rx0 = ry0 = rz0 = rx1 = ry1 = rz1 = 0.0;
            if (isv0)
            {
                rx0 = 0;
                ry0 = 0.001;
                rx1 = 0.001;
                ry1 = 0.0;
                if (!ischamfer)
                    axEWdraw2.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw2.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            if (isv1)
            {
                rx0 = w - 0.001;
                ry0 = 0;
                rx1 = w;
                ry1 = 0.001;
                if (!ischamfer)
                    axEWdraw2.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw2.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            if (isv2)
            {
                rx0 = w - 0.001;
                ry0 = h;
                rx1 = w;
                ry1 = h - 0.001;
                if (!ischamfer)
                    axEWdraw2.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw2.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            if (isv3)
            {
                rx0 = 0.001;
                ry0 = h;
                rx1 = 0;
                ry1 = h - 0.001;
                if (!ischamfer)
                    axEWdraw2.fillet(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet);
                else
                    axEWdraw2.Chamfer(rect, rect, new object[] { rx0, ry0, rz0 }, new object[] { rx1, ry1, rz1 }, fillet, fillet);
            }
            axEWdraw2.NoDisplayWhenCreate3DSolid(true);
            int entface = axEWdraw2.EntToFace(rect, true);
            axEWdraw2.NoDisplayWhenCreate3DSolid(false);
            int orgplank = axEWdraw2.Prism(entface, thickness, new object[] { 0, 0, 1 });
            axEWdraw2.SetEntTexture(orgplank, "wood02.jpg", 1, 1, 1, 1, 0, 0);
            axEWdraw2.Delete(entface);
            axEWdraw2.ZoomALL();
            return orgplank;
        }

        //创建异型板 2020-02-10
        private bool MakeYXB(int inx, int ent,double nrad = -1.0)
        {
            if (g_yxbs.Count > 0)
            {
                if (inx >= 0 && inx < g_yxbs.Count)
                {
                    double rad = 0.0;
                    double w, h;
                    w = h = 0;
                    switch (inx)
                    {
                        case 0:
                            {//右弧板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);
                                GetArcYXB(ent, rad, false, true, false, false, false);
                            }
                            break;
                        case 1:
                            {//左弧板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);
                                GetArcYXB(ent, rad, true, false, false, false, false);//
                            }
                            break;
                        case 2:
                            {//前双弧板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, true, true, false, false, false);//
                            }
                            break;
                        case 3:
                            {//后双弧板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, false, false, true, true, false);//
                            }
                            break;
                        case 4:
                            {//弧板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, true, true, true, true, false);//
                            }
                            break;
                        case 5:
                            {//右导角板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, false, true, false, false, true);//
                            }
                            break;
                        case 6:
                            {//左导角板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, true, false, false, false, true);//
                            }
                            break;
                        case 7:
                            {//前导角板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, true, true, false, false, true);//
                            }
                            break;
                        case 8:
                            {//后导角板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, false, false, true, true, true);//
                            }
                            break;
                        case 9:
                            {//导角板
                                if (nrad < 0.0)
                                    GetYXBPara(inx, "半径", ref rad);
                                else
                                    rad = nrad;
                                GetYXBPara(inx, "宽度", ref w);
                                GetYXBPara(inx, "深度", ref h);

                                GetArcYXB(ent, rad, true, true, true, true, true);//
                            }
                            break;

                    }
                    return true;
                }
            }
            return false;
        }

        //创建衣通 2020-02-13
        private int MakeYitong(double left, double right,double top,double front,double back,
            double topspace, double depth, double rad,bool isautodepath = true)
        {
            g_yitongmode = 0;//插入衣通
            axEWdraw3.ClearSelected();
            double x,y,z;
            x = y = z = 0.0;
            double cx, cy, cz;
            cx = cy = cz = 0.0;
            if (isautodepath)
            {
                cx = left;
                cy = (front + back) / 2.0;
                cz = top - topspace-rad;
                depth = (back-front)/2.0;
            }
            else
            {
                cx = left;
                cy = front-depth;
                cz = top - topspace - rad;
            }
            int ent = axEWdraw3.Cylinder(new object[] { cx, cy, cz }, rad, right - left, new object[] { 1, 0, 0 });
            string str;
            int ngrp = 0;
            str = "yt:1;" + "autod:" + Convert.ToInt32(isautodepath).ToString() + ";" + "top:" + topspace.ToString("0.00") + ";" + "depth:" + depth.ToString("0.00") + ";"
                + "rad:" + rad.ToString("0.00") + ";" + "length:" + (right-left).ToString("0.00") + ";";
            if (g_yitongorg > 0)
            {
                axEWdraw3.SetEntityUserData(ent, str);
                axEWdraw3.ClearIDBuffer();
                axEWdraw3.AddIDToBuffer(ent);

                ngrp = axEWdraw3.MakeGroup("grp_yt_" + ent.ToString(), new object[] { cx, cy, cz });
                axEWdraw3.GetGroupBndPt(ngrp, 3, ref x, ref y, ref z);
                axEWdraw3.SetGroupInsPt(ngrp, new object[] { x, y, z });
                axEWdraw3.SetGroupPlaneByBoxFace(ngrp, 2);
                axEWdraw3.ClearIDBuffer();
                axEWdraw3.Delete(g_yitongorg);
                g_yitongorg = 0;
            }
            else
            {
                axEWdraw3.SetEntityUserData(ent, str);
                axEWdraw3.ClearIDBuffer();
                axEWdraw3.AddIDToBuffer(ent);
                ngrp = axEWdraw3.MakeGroup("grp_yt_" + ent.ToString(), new object[] { cx, cy, cz });
                axEWdraw3.GetGroupBndPt(ngrp,3,ref x,ref y,ref z);
                axEWdraw3.SetGroupInsPt(ngrp, new object[] { x, y, z });
                axEWdraw3.SetGroupPlaneByBoxFace(ngrp, 2);
                axEWdraw3.ClearIDBuffer();
            }
            return ngrp;
        }

        //更新衣通 2020-02-14
        private int UpdateYitong(int ent,double minx,double miny,double minz,double maxx,double maxy,double maxz)
        {
            double topspace = 0;
            double depth = 0;
            double rad = 0;
            bool isautodepth = true;

            topspace = GetDblfromProStr(GetProStrFromEnt_N3(ent, "top"));
            depth = GetDblfromProStr(GetProStrFromEnt_N3(ent, "depth"));
            rad = GetDblfromProStr(GetProStrFromEnt_N3(ent, "rad"));
            if (rad < 0.001)
            {
                int tmptype = axEWdraw3.GetEntType(ent);
                return 0;
            }
            int tmpint = GetIntfromProStr(GetProStrFromEnt_N3(ent, "autod"));
            if (tmpint == 1)
                isautodepth = true;
            else
                isautodepth = false;
            g_yitongorg = ent;
            int rent = MakeYitong(minx, maxx, maxz, miny, maxy,
                        topspace, depth, rad, isautodepth);

            return rent;
        }

        private string GetProStrFromEnt_N3(int ent, string field)
        {
            string prostr = "";
            if (ent > 0)
            {
                string str = axEWdraw3.GetEntityUserData(ent);
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

        private void button32_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            int selsize = axEWdraw3.GetSelectEntSize();
            int ent = 0;
            bool isyt = false;
            int enttype = -1;
            g_yitongorg = 0;
            if (selsize > 0)
            {
                ent = axEWdraw3.GetSelectEnt(0);
                enttype = axEWdraw3.GetEntType(ent);
                if (enttype == 55)
                {
                    isyt = true;
                    g_yitongorg = ent;
                }
            }
            if (isyt)
            {
                label1.Text = "重新设置衣通";
                g_yitongmode = 1;//修改现存的衣通
            }
            else
            {
                label1.Text = "选择插入衣通的空间";
                g_yitongmode = 2;//插入衣通
            }
        }

        private void ReSetJiaJuKu()
        {
            axEWdraw3.ClearSelected();
            treeView1.Nodes.Clear();
            TreeNode root = new TreeNode();
            root.Text = "家具模型库";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //
            ArrayList tmpstrarr = new ArrayList();
            tmpstrarr.Add("抽屉");
            CreateTwo(root, 1245, tmpstrarr, true);
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //MiddleLeft
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "基础柜");
            treeView1.SelectedNode = node;
            treeView1.Focus();//2019-12-27
            axEWdraw2.SetPerspectiveMode(true);
            //
            axEWdraw3.SetWireOfShape(axEWdraw3.RGBToIndex(0, 0, 0), 2);
            axEWdraw3.SetGridValue(200, 200, 3000, 3000, 0);
            axEWdraw3.SetGridColor(128, 128, 128, 128, 128, 128);
            axEWdraw3.SetGridOrgColor(true, 255, 0, 0, 0, 255, 0);
            axEWdraw3.SetGridOn(true);
            axEWdraw3.SetViewCondition(8);
        }

        //取得抽屉的参数 2019-02-21
        private void GetCTParamaters_N(int ent,ref ArrayList paralist)
        {
            string memo = axEWdraw3.GetEntityUserData(ent);
            if (memo.Length > 0)
            {
                string[] strarr = memo.Split(';');
                if (strarr.Length > 0)
                {
                    int tcode = 0;
                    for (int i = 0; i < strarr.Length - 1; i++)
                    {
                        if (strarr[i].Length > 0)
                        {
                            string[] strarr1 = strarr[i].Split(':');
                            if (strarr1[0] == "名称")
                            {
                                TreeNode node = FindNode(treeView1.Nodes[0], strarr1[1]);
                                if (node != null)
                                {
                                    treeView1.SelectedNode = node;
                                    treeView1.Focus();//2019-12-27
                                    treeView1.Refresh();
                                    //
                                    tcode = ((SNode)node.Tag).code;
                                    g_tcode = tcode;
                                    int type = ((SNode)node.Tag).type;
                                    g_tname = ((SNode)node.Tag).name;
                                    int size = axEWdraw2.GetPlugInParameterSize(tcode);
                                    double val = 0;
                                    dataGridView1.Rows.Clear();
                                    for (int j = 0; j < size; j++)
                                    {
                                        string name = axEWdraw2.GetPlugInParameter(j, ref val);
                                        dataGridView1.Rows.Add();
                                        int count = dataGridView1.Rows.Count;
                                        dataGridView1[0, count - 1].Value = name;
                                        dataGridView1[1, count - 1].Value = val.ToString("0.00");
                                    }
                                    if (dataGridView1.ColumnCount > 0)
                                    {
                                        dataGridView1.Columns[0].Width = 120;
                                        dataGridView1.Columns[1].Width = 73;
                                    }
                                }
                            }
                            else
                            {
                                string name = strarr1[0];
                                string value = strarr1[1];
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                        if (dataGridView1.Rows[j].Cells[0].Value.ToString() == name)
                                        {
                                            if (paralist != null)
                                            {
                                                for (int k = 0; k < paralist.Count; k++)//2020-02-21
                                                {
                                                    if (dataGridView1.Rows[j].Cells[0].Value.ToString() == ((MyYXBPara)paralist[k]).m_name)
                                                    {
                                                        value = ((MyYXBPara)paralist[k]).m_val.ToString("0.00");
                                                        break;
                                                    }
                                                }
                                            }
                                            dataGridView1.Rows[j].Cells[1].Value = value;
                                            axEWdraw2.SetPlugInParameter(j, zPubFun.zPubFunLib.CStr2Double(value));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //调用模型
                    axEWdraw2.DeleteAll();
                    axEWdraw2.CreatePlugIn(tcode);
                    //纹理
                    int entsize = axEWdraw2.GetEntSize();
                    for (int j = 1; j <= entsize; j++)
                    {
                        ent = axEWdraw2.GetEntID(j);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            axEWdraw2.AddEntToShare(ent);//2020-02-21
                        }
                    }
                    axEWdraw2.SetViewCondition(8);
                    axEWdraw3.Focus();
                }
            }
        }

        //更新f架宽度
        private void UpdateFjiaWidth(int id,double minx,double width,double depth)
        {
            if (GDesignMode != 3)
                button33.PerformClick();
            if (id > 0)
            {

                ArrayList paralist = new ArrayList();
                if(checkBox2.Checked)
                {
                    MyYXBPara apara = new MyYXBPara();
                    apara.m_name = "宽度";
                    apara.m_val = width;
                    paralist.Add(apara);
                }
                if (checkBox4.Checked)
                {
                    MyYXBPara apara = new MyYXBPara();
                    apara.m_name = "深度";
                    apara.m_val = depth;
                    paralist.Add(apara);
                }

                GetCTParamaters_N(id, ref paralist);
            }
            else
            {
                axEWdraw2.ClearShare();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (checkBox2.Checked)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "宽度")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = width.ToString("0.00");
                            axEWdraw2.SetPlugInParameter(i, width);
                        }
                    }
                    if (checkBox4.Checked)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "深度")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = depth.ToString("0.00");
                            axEWdraw2.SetPlugInParameter(i, depth);
                        }
                    }
                }
                DataGridViewCell cell = dataGridView1.CurrentCell;
                //调用模型
                axEWdraw2.DeleteAll();
                axEWdraw2.ClearShare();
                if (g_tcode > 0)
                {
                    axEWdraw2.CreatePlugIn(g_tcode);
                    //纹理
                    int entsize = axEWdraw2.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        int ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            axEWdraw2.AddEntToShare(ent);
                        }
                    }
                    axEWdraw2.ZoomALL();
                }
            }
            CalFJIABasePt(ref g_fjiabx, ref g_fjiaby, ref g_fjiabz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
            string grpname = "grp_fjia_" + g_tcode.ToString();
            int grpid = 0;
            axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
            if (grpid > 0)
            {
                if (id > 0)
                    axEWdraw3.Delete(id);
                double tmpdist = 0.0;
                //
                axEWdraw3.SetGroupInsPt(grpid, new object[] { g_fjiabx, g_fjiaby, g_fjiabz });
                axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);

                axEWdraw3.MoveTo(grpid, new object[] { g_fjiabx, g_fjiaby, g_fjiabz }, new object[] { minx, g_sminy, g_sminz });//g_sminx 2020-04-13
                TreeNode node = treeView1.SelectedNode;
                SaveCTParamaters(grpid, node.Text);
            }
            axEWdraw2.ClearShare();
        }

        //更新博古架宽度
        private void UpdateBOGUWidth(int id, double minx,double width, double depth,double height)
        {
            if (GDesignMode != 4)
                button34.PerformClick();
            if (id > 0)
            {

                ArrayList paralist = new ArrayList();
                if (checkBox2.Checked)
                {
                    MyYXBPara apara = new MyYXBPara();
                    apara.m_name = "宽度";
                    apara.m_val = width;
                    paralist.Add(apara);
                }
                if (checkBox4.Checked)
                {
                    MyYXBPara apara = new MyYXBPara();
                    apara.m_name = "深度";
                    apara.m_val = depth;
                    paralist.Add(apara);
                }
                if (checkBox3.Checked)
                {
                    MyYXBPara apara = new MyYXBPara();
                    apara.m_name = "高度";
                    apara.m_val = height;
                    paralist.Add(apara);
                }
                GetCTParamaters_N(id, ref paralist);
            }
            else
            {
                axEWdraw2.ClearShare();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (checkBox2.Checked)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "宽度")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = width.ToString("0.00");
                            axEWdraw2.SetPlugInParameter(i, width);
                        }
                    }
                    if (checkBox4.Checked)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "深度")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = depth.ToString("0.00");
                            axEWdraw2.SetPlugInParameter(i, depth);
                        }
                    }
                    if (checkBox3.Checked)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == "高度")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = height.ToString("0.00");
                            axEWdraw2.SetPlugInParameter(i, height);
                        }
                    }
                }
                DataGridViewCell cell = dataGridView1.CurrentCell;
                //调用模型
                axEWdraw2.DeleteAll();
                axEWdraw2.ClearShare();
                if (g_tcode > 0)
                {
                    axEWdraw2.CreatePlugIn(g_tcode);
                    //纹理
                    int entsize = axEWdraw2.GetEntSize();
                    for (int i = 1; i <= entsize; i++)
                    {
                        int ent = axEWdraw2.GetEntID(i);
                        int enttype = axEWdraw2.GetEntType(ent);
                        if (enttype != 501/*三合一*/ && enttype != 502/*木楔*/ && enttype != 503/*二合一*/)
                        {
                            axEWdraw2.SetEntTexture(ent, "wood02.jpg", 1, 1, 1, 1, 0, 0);
                            axEWdraw2.AddEntToShare(ent);
                        }
                    }
                    axEWdraw2.ZoomALL();
                }
            }
            CalFJIABasePt(ref g_bogubx, ref g_boguby, ref g_bogubz, ref g_ctminx, ref g_ctminy, ref g_ctminz, ref g_ctmaxx, ref g_ctmaxy, ref g_ctmaxz);
            string grpname = "grp_bogu_" + g_tcode.ToString();
            int grpid = 0;
            axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
            if (grpid > 0)
            {
                if (id > 0)
                    axEWdraw3.Delete(id);
                double tmpdist = 0.0;
                //
                axEWdraw3.SetGroupInsPt(grpid, new object[] { g_bogubx, g_boguby, g_bogubz });
                axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);

                axEWdraw3.MoveTo(grpid, new object[] { g_bogubx, g_boguby, g_bogubz }, new object[] { minx, g_sminy, g_sminz });//g_sminx, 2020-04-13
                TreeNode node = treeView1.SelectedNode;
                SaveCTParamaters(grpid, node.Text);
            }
            axEWdraw2.ClearShare();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            label1.Text = "插入对应的F架";
            HideWJ();//消隐五金 2020-03-20
            GDesignMode = 3;
            listView6.Visible = false;
            treeView1.Visible = true;
            checkBox3.Visible = false;
            dataGridView1.Visible = true;
            button38.Visible = false;//2020-03-20
            button40.Visible = false;//2020-03-20
            //
            axEWdraw2.Top = 42;
            axEWdraw2.Height = 182;
            axEWdraw2.Width = 233;
            //
            treeView1.Nodes.Clear();
            dataGridView1.Rows.Clear();

            TreeNode root = new TreeNode();
            root.Text = "F架";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //2020-01-23
            ArrayList tmpstrarr = new ArrayList();
            tmpstrarr.Add("左F架");
            tmpstrarr.Add("右F架");
            CreateTwo(root, 2685, tmpstrarr, false);
            //
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //MiddleLeft
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "右F架");
            treeView1.SelectedNode = node;
            treeView1.Focus();

        }

        private void button34_Click(object sender, EventArgs e)
        {
            label1.Text = "插入对应的博古架";
            HideWJ();//消隐五金 2020-03-20
            GDesignMode = 4;
            listView6.Visible = false;
            treeView1.Visible = true;
            checkBox3.Visible = true;
            checkBox3.Checked = true;//2020-03-04
            button38.Visible = false;//2020-03-20
            button40.Visible = false;//2020-03-20
            dataGridView1.Visible = true;
            //
            axEWdraw2.Top = 42;
            axEWdraw2.Height = 182;
            axEWdraw2.Width = 233;
            //
            treeView1.Nodes.Clear();
            dataGridView1.Rows.Clear();

            TreeNode root = new TreeNode();
            root.Text = "博古架";
            SNode asubnode = new SNode();
            asubnode.type = 0;
            asubnode.code = 1245;
            root.Tag = asubnode;
            //2020-01-23
            ArrayList tmpstrarr = new ArrayList();
            tmpstrarr.Add("一");
            tmpstrarr.Add("二");
            tmpstrarr.Add("四");
            tmpstrarr.Add("五");
            tmpstrarr.Add("六");
            CreateTwo(root, 2638, tmpstrarr, false);
            //
            treeView1.Nodes.Add(root);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            if (dataGridView1.Columns.Count >= 2)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //MiddleLeft
            }
            TreeNode node = FindNode(treeView1.Nodes[0], "一");
            treeView1.SelectedNode = node;
            treeView1.Focus();

        }

        private void button35_Click(object sender, EventArgs e)
        {
        }

        //根据一个板材得到空间里的组件 2020-03-07
        private void GetSpaceEnts(int orgent)
        {
            g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            int tmpent = 0;
            g_axismoveid = orgent;//设置板材ID号
            axEWdraw3.GetEntBoundingBox(g_axismoveid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            if (g_axismoveplank == null)
                g_axismoveplank = new CPlank();
            g_axismoveplank.minx = minx;
            g_axismoveplank.miny = miny;
            g_axismoveplank.minz = minz;
            g_axismoveplank.maxx = maxx;
            g_axismoveplank.maxy = maxy;
            g_axismoveplank.maxz = maxz;
            g_axismoveplank.id = g_axismoveid;
            //
            int tmptype = axEWdraw3.GetPlankType(g_axismoveid);
            for (int i = 0; i < g_axismovespacesize; i++)
            {
                axEWdraw3.GetCabinetSpace(i, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                //判断左,右侧
                double midx = (minx + maxx) / 2.0;
                double midy = (miny + maxy) / 2.0;
                double midz = (minz + maxz) / 2.0;
                if (tmptype == 1)
                {
                    if (midx < g_axismoveplank.minx)
                    {//左侧
                        if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                   g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                   g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                        {
                            CPlank abnd = new CPlank();
                            abnd.minx = minx;
                            abnd.miny = miny;
                            abnd.minz = minz;
                            //
                            abnd.maxx = maxx;
                            abnd.maxy = maxy;
                            abnd.maxz = maxz;
                            g_axismoveleftup.Add(abnd);
                        }
                    }
                    else
                    {//右侧
                        CPlank abnd = new CPlank();
                        abnd.minx = minx;
                        abnd.miny = miny;
                        abnd.minz = minz;
                        //
                        abnd.maxx = maxx;
                        abnd.maxy = maxy;
                        abnd.maxz = maxz;
                        g_axismoverightdown.Add(abnd);
                    }
                }
                else if (tmptype == 0)
                {
                    if (midz > g_axismoveplank.maxz)
                    {//左侧
                        if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                   g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                   g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                        {
                            CPlank abnd = new CPlank();
                            abnd.minx = minx;
                            abnd.miny = miny;
                            abnd.minz = minz;
                            //
                            abnd.maxx = maxx;
                            abnd.maxy = maxy;
                            abnd.maxz = maxz;
                            g_axismoveleftup.Add(abnd);
                        }
                    }
                    else
                    {//右侧
                        CPlank abnd = new CPlank();
                        abnd.minx = minx;
                        abnd.miny = miny;
                        abnd.minz = minz;
                        //
                        abnd.maxx = maxx;
                        abnd.maxy = maxy;
                        abnd.maxz = maxz;
                        g_axismoverightdown.Add(abnd);
                    }
                }
            }
            int entsize = axEWdraw3.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                tmpent = axEWdraw3.GetEntID(i);
                if (tmpent > 0)
                {
                    if (axEWdraw3.IsGroup(tmpent))
                    {
                        string name = axEWdraw3.GetGroupName(tmpent);
                        if (name.IndexOf("_ct_") >= 0 || name.IndexOf("_yt_") >= 0 || name.IndexOf("_fjia_") >= 0 || name.IndexOf("_bogu_") >= 0)
                        {
                            double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                            tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                            axEWdraw3.GetGroupBndPt(tmpent, 0, ref tmpx1, ref tmpy1, ref tmpz1);
                            axEWdraw3.GetGroupBndPt(tmpent, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                            double midx = (tmpx1 + tmpx2) / 2.0;
                            double midy = (tmpy1 + tmpy2) / 2.0;
                            double midz = (tmpz1 + tmpz2) / 2.0;
                            //左侧
                            for (int j = 0; j < g_axismoveleftup.Count; j++)
                            {
                                if (midx > ((CPlank)g_axismoveleftup[j]).minx && midy > ((CPlank)g_axismoveleftup[j]).miny && midz > ((CPlank)g_axismoveleftup[j]).minz &&
                                    midx < ((CPlank)g_axismoveleftup[j]).maxx && midy < ((CPlank)g_axismoveleftup[j]).maxy && midz < ((CPlank)g_axismoveleftup[j]).maxz
                                    )
                                {
                                    SNode node = new SNode();//2020-02-14
                                    node.code = tmpent;
                                    if (name.IndexOf("_ct_") >= 0)
                                        node.type = 0;//抽屉类型
                                    else if (name.IndexOf("_yt_") >= 0)
                                        node.type = 1;//衣通类型
                                    else if (name.IndexOf("_fjia_") >= 0)//F架
                                        node.type = 2;
                                    else if (name.IndexOf("_bogu_") >= 0)//博古
                                        node.type = 3;
                                    g_axismoveidsleftup.Add(node);//tmpent
                                }
                            }
                            //右侧
                            for (int j = 0; j < g_axismoverightdown.Count; j++)
                            {
                                if (midx > ((CPlank)g_axismoverightdown[j]).minx && midy > ((CPlank)g_axismoverightdown[j]).miny && midz > ((CPlank)g_axismoverightdown[j]).minz &&
                                    midx < ((CPlank)g_axismoverightdown[j]).maxx && midy < ((CPlank)g_axismoverightdown[j]).maxy && midz < ((CPlank)g_axismoverightdown[j]).maxz
                                    )
                                {
                                    SNode node = new SNode();//2020-02-14
                                    node.code = tmpent;
                                    if (name.IndexOf("_ct_") >= 0)
                                        node.type = 0;//抽屉类型
                                    else if (name.IndexOf("_yt_") >= 0)
                                        node.type = 1;//抽屉类型
                                    else if (name.IndexOf("_fjia_") >= 0)//F架
                                        node.type = 2;
                                    else if (name.IndexOf("_bogu_") >= 0)//F架
                                        node.type = 3;
                                    g_axismoveidsrightdown.Add(node);//tmpent
                                }
                            }
                        }
                    }
                }
            }
            g_axismoveleftup.Clear();
            g_axismoverightdown.Clear();
        }

        //根据设置的板材改变所涉及的组件 2020-03-07
        private void SetCompByPlank(int orgent)
        {
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            int tmpent = 0;
            g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();
            g_axismoveid = orgent;//设置板材ID号
            int tmptype = axEWdraw3.GetPlankType(g_axismoveid);
            for (int i = 0; i < g_axismovespacesize; i++)
            {
                axEWdraw3.GetCabinetSpace(i, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                //判断左,右侧
                double midx = (minx + maxx) / 2.0;
                double midy = (miny + maxy) / 2.0;
                double midz = (minz + maxz) / 2.0;

                if (tmptype == 1)
                {
                    if (midx < g_axismoveplank.minx)
                    {//左侧
                        if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                   g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                   g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                        {
                            CPlank abnd = new CPlank();
                            abnd.minx = minx;
                            abnd.miny = miny;
                            abnd.minz = minz;
                            //
                            abnd.maxx = maxx;
                            abnd.maxy = maxy;
                            abnd.maxz = maxz;
                            g_axismoveleftup.Add(abnd);
                        }
                    }
                    else
                    {//右侧
                        CPlank abnd = new CPlank();
                        abnd.minx = minx;
                        abnd.miny = miny;
                        abnd.minz = minz;
                        //
                        abnd.maxx = maxx;
                        abnd.maxy = maxy;
                        abnd.maxz = maxz;
                        g_axismoverightdown.Add(abnd);
                    }
                }
                else if (tmptype == 0)
                {
                    if (midz > g_axismoveplank.maxz)
                    {//左侧
                        if (axEWdraw3.IsIntersectAABBs(minx - 0.001, miny - 0.001, minz - 0.001, maxx + 0.001, maxy + 0.001, maxz + 0.001,
                                                   g_axismoveplank.minx, g_axismoveplank.miny, g_axismoveplank.minz,
                                                   g_axismoveplank.maxx, g_axismoveplank.maxy, g_axismoveplank.maxz))
                        {
                            CPlank abnd = new CPlank();
                            abnd.minx = minx;
                            abnd.miny = miny;
                            abnd.minz = minz;
                            //
                            abnd.maxx = maxx;
                            abnd.maxy = maxy;
                            abnd.maxz = maxz;
                            g_axismoveleftup.Add(abnd);
                        }
                    }
                    else
                    {//右侧
                        CPlank abnd = new CPlank();
                        abnd.minx = minx;
                        abnd.miny = miny;
                        abnd.minz = minz;
                        //
                        abnd.maxx = maxx;
                        abnd.maxy = maxy;
                        abnd.maxz = maxz;
                        g_axismoverightdown.Add(abnd);
                    }
                }
            }
            if (g_axismoveidsleftup.Count > 0)
            {
                //左侧
                for (int i = 0; i < g_axismoveidsleftup.Count; i++)
                {
                    double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                    tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                    axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsleftup[i]).code, 0, ref tmpx1, ref tmpy1, ref tmpz1);
                    axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsleftup[i]).code, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                    for (int j = 0; j < g_axismoveleftup.Count; j++)
                    {
                        if (axEWdraw3.IsIntersectAABBs(tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2,
                                                    ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                                    ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz))
                        {
                            if (((SNode)g_axismoveidsleftup[i]).type == 0)
                            {
                                UpdateCTWidth(((SNode)g_axismoveidsleftup[i]).code,
                                    ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                    ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz);
                            }
                            else if (((SNode)g_axismoveidsleftup[i]).type == 1)//衣通
                            {
                                UpdateYitong(((SNode)g_axismoveidsleftup[i]).code, ((CPlank)g_axismoveleftup[j]).minx, ((CPlank)g_axismoveleftup[j]).miny, ((CPlank)g_axismoveleftup[j]).minz,
                                    ((CPlank)g_axismoveleftup[j]).maxx, ((CPlank)g_axismoveleftup[j]).maxy, ((CPlank)g_axismoveleftup[j]).maxz);
                            }
                            else if (((SNode)g_axismoveidsleftup[i]).type == 2)//F架
                            {
                                UpdateFjiaWidth(((SNode)g_axismoveidsleftup[i]).code,
                                    ((CPlank)g_axismoveleftup[j]).minx,//2020-04-13
                                    ((CPlank)g_axismoveleftup[j]).maxx - ((CPlank)g_axismoveleftup[j]).minx,
                                    ((CPlank)g_axismoveleftup[j]).maxy - ((CPlank)g_axismoveleftup[j]).miny
                                    );
                            }
                            else if (((SNode)g_axismoveidsleftup[i]).type == 3)//博古
                            {
                                UpdateBOGUWidth(((SNode)g_axismoveidsleftup[i]).code,
                                    ((CPlank)g_axismoveleftup[j]).minx,
                                    ((CPlank)g_axismoveleftup[j]).maxx - ((CPlank)g_axismoveleftup[j]).minx,
                                    ((CPlank)g_axismoveleftup[j]).maxy - ((CPlank)g_axismoveleftup[j]).miny,
                                    ((CPlank)g_axismoveleftup[j]).maxz - ((CPlank)g_axismoveleftup[j]).minz
                                    );
                            }
                        }
                    }

                }
            }
            if (g_axismoveidsrightdown.Count > 0)
            {
                //右侧
                for (int i = 0; i < g_axismoveidsrightdown.Count; i++)
                {
                    double tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2;
                    tmpx1 = tmpy1 = tmpz1 = tmpx2 = tmpy2 = tmpz2 = 0.0;
                    axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsrightdown[i]).code, 0, ref tmpx1, ref tmpy1, ref tmpz1);
                    axEWdraw3.GetGroupBndPt(((SNode)g_axismoveidsrightdown[i]).code, 6, ref tmpx2, ref tmpy2, ref tmpz2);
                    for (int j = 0; j < g_axismoverightdown.Count; j++)
                    {
                        if (axEWdraw3.IsIntersectAABBs(tmpx1, tmpy1, tmpz1, tmpx2, tmpy2, tmpz2,
                                                    ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                                    ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz))
                        {
                            if (((SNode)g_axismoveidsrightdown[i]).type == 0)
                            {
                                UpdateCTWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                    ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                    ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz);
                            }
                            else if (((SNode)g_axismoveidsrightdown[i]).type == 1)//衣通
                            {//衣通
                                UpdateYitong(((SNode)g_axismoveidsrightdown[i]).code, ((CPlank)g_axismoverightdown[j]).minx, ((CPlank)g_axismoverightdown[j]).miny, ((CPlank)g_axismoverightdown[j]).minz,
                                    ((CPlank)g_axismoverightdown[j]).maxx, ((CPlank)g_axismoverightdown[j]).maxy, ((CPlank)g_axismoverightdown[j]).maxz);
                            }

                            else if (((SNode)g_axismoveidsrightdown[i]).type == 2)//F架
                            {
                                UpdateFjiaWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                    ((CPlank)g_axismoverightdown[j]).minx,//2020-04-13
                                    ((CPlank)g_axismoverightdown[j]).maxx - ((CPlank)g_axismoverightdown[j]).minx,
                                    ((CPlank)g_axismoverightdown[j]).maxy - ((CPlank)g_axismoverightdown[j]).miny
                                    );
                            }
                            else if (((SNode)g_axismoveidsrightdown[i]).type == 3)//博古
                            {
                                UpdateBOGUWidth(((SNode)g_axismoveidsrightdown[i]).code,
                                    ((CPlank)g_axismoverightdown[j]).minx,//2020-04-13
                                    ((CPlank)g_axismoverightdown[j]).maxx - ((CPlank)g_axismoverightdown[j]).minx,
                                    ((CPlank)g_axismoverightdown[j]).maxy - ((CPlank)g_axismoverightdown[j]).miny,
                                    ((CPlank)g_axismoverightdown[j]).maxz - ((CPlank)g_axismoverightdown[j]).minz
                                    );
                            }
                        }
                    }
                }
            }
            g_axismoveleftup.Clear();
            g_axismoverightdown.Clear();
            g_axismoveid = 0;
        }

        private void button35_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void button35_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HideWJ();//消隐五金 2020-03-20
                if (g_editpent > 0)
                {
                    if (textBox3.Enabled)
                    {
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        //textBox9.Enabled = false;
                        button35.Text = "修改尺寸";
                        g_editw = zPubFun.zPubFunLib.CStr2Double(textBox3.Text);
                        g_edith = zPubFun.zPubFunLib.CStr2Double(textBox4.Text);
                        g_editt = zPubFun.zPubFunLib.CStr2Double(textBox5.Text);
                        //
                        double xbl, ybl, zbl;
                        xbl = 1.0;
                        ybl = 1.0;
                        zbl = 1.0;
                        axEWdraw3.BeginUndo(true);
                        switch (g_editptype)
                        {
                            case 0:
                                {//横板
                                    
                                    if (Math.Abs(g_editw - g_oldeditw) > 0.001)
                                    {
                                        xbl = g_editw / g_oldeditw;
                                        if (Math.Abs(xbl - 1.0) < 0.001)
                                            return;
                                        axEWdraw3.PlankScale(g_editpent, xbl, 1.0, 1.0);
                                    }
                                    if (Math.Abs(g_edith - g_oldedith) > 0.001)
                                    {
                                        ybl = g_edith / g_oldedith;
                                        if (Math.Abs(ybl - 1.0) < 0.001)
                                            return;
                                        axEWdraw3.PlankScale(g_editpent, 1.0, ybl, 1.0);
                                    }
                                    if (Math.Abs(g_editt - g_oldeditt) > 0.001)
                                    {
                                        GetSpaceEnts(g_editpent);//2020-03-07
                                        axEWdraw3.CabinetDesignPlankThickness(g_editpent, g_editt);
                                        SetCompByPlank(g_editpent);
                                    }
                                    
                                }
                                break;
                            case 1:
                                {//竖板
                                    if (Math.Abs(g_editw - g_oldeditw) > 0.001)
                                    {
                                        zbl = g_editw / g_oldeditw;
                                        if (Math.Abs(zbl - 1.0) < 0.001)
                                            return;

                                        axEWdraw3.PlankScale(g_editpent, 1.0, 1.0, zbl);
                                    }
                                    if (Math.Abs(g_edith - g_oldedith) > 0.001)
                                    {
                                        ybl = g_edith / g_oldedith;
                                        if (Math.Abs(ybl - 1.0) < 0.001)
                                            return;
                                        axEWdraw3.PlankScale(g_editpent, 1.0, ybl, 1.0);
                                    }
                                    if (Math.Abs(g_editt - g_oldeditt) > 0.001)
                                    {
                                        GetSpaceEnts(g_editpent);//2020-03-07
                                        axEWdraw3.CabinetDesignPlankThickness(g_editpent, g_editt);
                                        SetCompByPlank(g_editpent);
                                    }
                                    axEWdraw3.UpdateView();

                                }
                                break;
                            case 2:
                                {//面板
                                    if (Math.Abs(g_editw - g_oldeditw) > 0.001)
                                    {
                                        xbl = g_editw / g_oldeditw;
                                        if (Math.Abs(xbl - 1.0) < 0.001)
                                            return;

                                        axEWdraw3.PlankScale(g_editpent, xbl, 1.0, 1.0);
                                    }
                                    if (Math.Abs(g_edith - g_oldedith) > 0.001)
                                    {
                                        zbl = g_edith / g_oldedith;
                                        if (Math.Abs(zbl - 1.0) < 0.001)
                                            return;
                                        axEWdraw3.PlankScale(g_editpent, 1.0, 1.0, zbl);
                                    }
                                    if (Math.Abs(g_editt - g_oldeditt) > 0.001)
                                    {
                                        GetSpaceEnts(g_editpent);//2020-03-07
                                        axEWdraw3.CabinetDesignPlankThickness(g_editpent, g_editt);
                                        SetCompByPlank(g_editpent);
                                    }
                                    axEWdraw3.UpdateView();
                                }
                                break;
                        }
                        axEWdraw3.EndUndo();
                        axEWdraw3.UpdateView();

                    }
                    else
                    {
                        textBox3.Enabled = true;
                        textBox4.Enabled = true;
                        textBox5.Enabled = true;
                        g_oldeditw = zPubFun.zPubFunLib.CStr2Double(textBox3.Text);
                        g_oldedith = zPubFun.zPubFunLib.CStr2Double(textBox4.Text);
                        g_oldeditt = zPubFun.zPubFunLib.CStr2Double(textBox5.Text);
                        button35.Text = "确认修改";
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                button35.Text = "修改尺寸";
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
           
        }

        private void button36_MouseUp(object sender, MouseEventArgs e)
        {
            if(g_editpent>0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    HideWJ();//消隐五金 2020-03-20
                    if (button36.Text == "修改偏移量")
                    {
                        textBox6.Enabled = true;
                        textBox7.Enabled = true;
                        textBox8.Enabled = true;

                        button36.Text = "确认修改";
                    }
                    else
                    {
                        textBox6.Enabled = false;
                        textBox7.Enabled = false;
                        textBox8.Enabled = false;
                        axEWdraw3.BeginUndo(true);//2020-03-17
                        axEWdraw3.MoveTo(g_editpent,new object[]{0,0,0},new object[]{zPubFun.zPubFunLib.CStr2Double(textBox6.Text),zPubFun.zPubFunLib.CStr2Double(textBox7.Text),zPubFun.zPubFunLib.CStr2Double(textBox8.Text)});
                        if (axEWdraw3.IsBeginUndo())//2020-03-17
                            axEWdraw3.EndUndo();
                        button36.Text = "修改偏移量";
                        axEWdraw3.CabinetSpaceDesignMode();
                    }
                }
                else
                {
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;

                    button36.Text = "修改偏移量";
                }
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (isCabinetDesign)
            {
                double x1, y1, z1, x2, y2, z2;
                x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
                int movent = axEWdraw3.GetOneEntSel(ref x1, ref y1, ref z1);

                if (movent > 0)
                {
                    axEWdraw3.SetOSNAPLimit(8);
                    axEWdraw3.SetOSNAPMode(0);
                    axEWdraw3.EnableGroupOsnapOnlyBnd(true);
                    if (g_isquit)
                        return;
                    label1.Text = "选择起始点";
                    if (axEWdraw3.GetPoint(ref x1, ref y1, ref z1))
                    {
                        if (!g_isquit)
                        {
                            label1.Text = "选择结束点";
                            if (axEWdraw3.GetPoint(ref x2, ref y2, ref z2))
                            {
                                if (!g_isquit)
                                {
                                    axEWdraw3.BeginUndo(true);
                                    axEWdraw3.MoveTo(movent, new object[] { x1, y1, z1 }, new object[] { x2, y2, z2 });
                                    if (axEWdraw3.IsBeginUndo())
                                        axEWdraw3.EndUndo();
                                    axEWdraw3.ClearSelected();
                                    axEWdraw3.CabinetSpaceDesignMode();
                                }
                            }
                        }
                    }
                }
                else label1.Text = "选择要移动的板材";
            }
            else
            {
                button23.PerformClick();
            }
        }

        //板材移动
        private void button37_Click(object sender, EventArgs e)
        {
            HideWJ();//消隐五金 2020-03-20
            if (isCabinetDesign)
            {
                int selsize = axEWdraw3.GetSelectEntSize();
                if (selsize > 0)
                {
                    g_axismoveid = axEWdraw3.GetSelectEnt(0);
                    if (g_axismoveid > 0)
                    {
                        double minx, miny, minz, maxx, maxy, maxz;
                        minx = miny = minz = maxx = maxy = maxz = 0.0;
                        axEWdraw3.GetEntBoundingBox(g_axismoveid, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                        if (g_axismoveplank == null)
                            g_axismoveplank = new CPlank();
                        g_axismoveplank.minx = minx;
                        g_axismoveplank.miny = miny;
                        g_axismoveplank.minz = minz;
                        g_axismoveplank.maxx = maxx;
                        g_axismoveplank.maxy = maxy;
                        g_axismoveplank.maxz = maxz;
                        g_axismoveplank.id = g_axismoveid;
                        g_timerproc = 2;
                        timer9.Enabled = true;
                    }
                }

                label1.Text = "选择要移动的板材.";
                axEWdraw3.EnableMoveSinglePlank(true);
                axEWdraw3.EnableSinglePlankAxisMove(true);
                issingplankmove = true;
                axEWdraw3.EnableSelWhenCommand(true);
                axEWdraw3.EnableSelAxisWhenAxisMove(false);
                axEWdraw3.BeginUndo(true);
                axEWdraw3.ToDrawAxisMove();
            }
            else
            {
                MessageBox.Show("请在柜体设计状态下使用");
            }

        }
        //异型变矩形
        private void button38_Click(object sender, EventArgs e)
        {
            double minx,miny,minz,maxx,maxy,maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            if (g_editpent > 0)
            {
                axEWdraw3.BeginUndo(true);
                axEWdraw3.GetEntBoundingBox(g_editpent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                int ent = axEWdraw3.Box(new object[] { minx, miny, minz }, maxx - minx, maxy - miny, maxz - minz);
                axEWdraw3.Replace3DSolid(ent, g_editpent);
                int enttype = axEWdraw3.GetPlankType(g_editpent);
                
                int wjsize = axEWdraw3.GetPlankWJSize(g_editpent);
                bool ishaveshy = false;
                bool ishavems = false;
                int wjtype, wz, side, sl, yl;
                wjtype = wz = side = sl = yl = 0;
                int wjtype1, wz1, side1, sl1, yl1;
                wjtype1 = wz1 = side1 = sl1 = yl1 = 0;

                double qk, hk;
                qk = hk = 0;
                double qk1, hk1;
                qk1 = hk1 = 0;
                
                for (int i = 0; i < wjsize; i++)
                {
                    axEWdraw3.GetPlankWJ(g_editpent, i, ref wjtype, ref wz, ref side, ref qk, ref hk, ref sl, ref yl);
                    if (wjtype == 5016)
                    {
                        ishaveshy = true;
                        break;
                    }
                }

                for (int i = 0; i < wjsize; i++)
                {
                    axEWdraw3.GetPlankWJ(g_editpent, i, ref wjtype1, ref wz1, ref side1, ref qk1, ref hk1, ref sl1, ref yl1);
                    if (wjtype1 == 5015)
                    {
                        ishavems = true;
                        break;
                    }
                }
                axEWdraw3.DeletePlankWJ(g_editpent);
                switch (enttype)
                {
                    case 0:
                        {
                            //左边
                            if(ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 0, qk, hk, sl, yl);
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 0, qk1, hk1, sl1, yl1);
                            //左边
                            if (ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 1, qk, hk, sl, yl);// 
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 1, qk1, hk1, sl1, yl1);
                        }
                        break;
                    case 1:
                        {
                            //上
                            if (ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 2, qk, hk, sl, yl);
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 2, qk1, hk1, sl1, yl1);
                            //下
                            if (ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 3, qk, hk, sl, yl);// 
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 3, qk1, hk1, sl1, yl1);
                        }
                        break;
                    case 2:
                        {
                            //左
                            if (ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 0, qk, hk, sl, yl);
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 0, qk1, hk1, sl1, yl1);
                            //右
                            if (ishaveshy)
                                axEWdraw3.AddPlankWJ(g_editpent, 5016, wz, 1, qk, hk, sl, yl);// 
                            if (ishavems)
                                axEWdraw3.AddPlankWJ(g_editpent, 5015, wz1, 1, qk1, hk1, sl1, yl1);
                        }
                        break;
                }
                axEWdraw3.ReCalWj();//2020-03-20
                axEWdraw3.EndUndo();
                axEWdraw3.ClearSelected();
            }
        }

        private void axEWdraw3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int selsize = axEWdraw3.GetSelectEntSize();
                if (selsize > 0)
                {
                    int ent = 0;
                    axEWdraw3.BeginUndo(true);
                    ArrayList insideids = new ArrayList();
                    for (int i = 0; i < selsize; i++)
                    {
                        ent = axEWdraw3.GetSelectEnt(i);
                        GetCabinectInside(ent,ref insideids);
                        axEWdraw3.Delete(ent);
                        if (insideids.Count > 0)//2020-04-13
                        {
                            for (int j = 0; j < selsize; j++)
                            {
                                axEWdraw3.Delete((int)insideids[j]);
                            }
                        }
                        insideids.Clear();
                    }
                    if(axEWdraw3.IsBeginUndo())
                        axEWdraw3.EndUndo();
                }
                g_axismovespacesize = axEWdraw3.CabinetSpaceDesignMode();
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                axEWdraw3.EnableUndoRedo(1);
                axEWdraw3.Undo();
                axEWdraw3.EnableUndoRedo(0);
                axEWdraw3.CabinetSpaceDesignMode();
            }
        }

        private void axEWdraw3_GetProcInfo(object sender, AxEWDRAWLib._DAdrawEvents_GetProcInfoEvent e)
        {
            if (e.info == "")
            {
                if (axEWdraw3.IsEndCommand())
                {
                    axEWdraw3.EnableSelWhenCommand(false);
                }
            }
        }

        //插入柜体到工作空间
        private void button40_Click(object sender, EventArgs e)
        {
            if (g_orderid == null)
            {
                Form8 input = new Form8();
                if (input.ShowDialog() == DialogResult.OK)
                {
                    g_orderid = input.label2.Text;
                    g_orderdate = input.dateTimePicker1.Text;
                    g_orderoutdtate = input.dateTimePicker2.Text;
                    g_orderconnecter = input.textBox3.Text;
                    g_orderphone = input.textBox4.Text;
                    g_orderuser = input.textBox1.Text;//2020-02-28
                    axEWdraw3.AddFurnitureOrder(input.label2.Text);
                    axEWdraw3.DeleteAll();
                    Text = "家具拆单---" + "订单号:" + g_orderid;
                    //

                }
            }
            int entsize = axEWdraw2.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw2.GetEntID(i);
                int enttype = axEWdraw2.GetEntType(ent);
                if (enttype != 501 && enttype != 502 && enttype != 503)
                    axEWdraw2.AddEntToShare(ent);
            }

            string grpname = "grp_" + g_tcode.ToString();
            bool isperspect = false;
            if (axEWdraw3.GetEntSize() == 0)
                isperspect = true;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw3.GetAllBoundingBox(ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
            if (axEWdraw3.GetEntSize() == 0)
            {
                maxx = maxy = maxz = 0;
            }
            int grpid = 0;
            axEWdraw3.GetAllEntFromShare(true, true, new object[] { 0, 0, 0 }, grpname, ref grpid);
            if (grpid > 0)
            {
                axEWdraw3.SetGroupInsPt(grpid, new object[] { 0, 0, 0 });
                axEWdraw3.SetGroupPlaneByBoxFace(grpid, 2);
                axEWdraw3.MoveTo(grpid, new object[] { 0, 0, 0 }, new object[] { maxx, maxy, 0 });
            }

            if (axEWdraw3.GetEntSize() > 0)
                axEWdraw3.SetPerspectiveMode(true);
            //
            axEWdraw2.ClearShare();
            axEWdraw3.ZoomALL();//2020-03-24
        }

        //消隐五金
        private void HideWJ(bool isrexplode = true)
        {
            if(isrexplode)//2020-04-24
                RestoreExplodeCabinet();//2020-04-20
            if (button41.Text == "消隐五金")
            {
                button41.Text = "显示五金";
                int entsize = axEWdraw3.GetEntSize();
                int ent = 0;
                for (int i = 1; i <= entsize; i++)
                {
                    ent = axEWdraw3.GetEntID(i);
                    if (axEWdraw3.IsGroup(ent))
                    {
                        axEWdraw3.ClearIDBuffer();
                        axEWdraw3.GetGroupAllIDs(ent);
                        int grpentsize = axEWdraw3.GetIDBufferSize();
                        for (int j = 0; j < grpentsize; j++)
                        {
                            ent = axEWdraw3.GetImpEntID(j);
                            axEWdraw3.SetTransparency(ent, 0);
                        }
                    }
                    else
                    {
                        int enttype = axEWdraw3.GetEntType(ent);
                        if (enttype == 501/*三合一*/ || enttype == 502/*木楔*/ || enttype == 503/*二合一*/)
                        {
                            axEWdraw3.Delete(ent);
                        }
                        else
                            axEWdraw3.SetTransparency(ent, 0);
                    }
                }
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            if (axEWdraw3.GetEntSize() == 0)
            {
                MessageBox.Show("请先插入柜体");
                return;
            }
            if (button41.Text == "显示五金")
            {
                axEWdraw3.ReCalWj();
                button41.Text = "消隐五金";
                int entsize = axEWdraw3.GetEntSize();
                int ent = 0;
                for (int i = 1; i <= entsize; i++)
                {
                    ent = axEWdraw3.GetEntID(i);
                    if (axEWdraw3.IsGroup(ent))
                    {
                        axEWdraw3.ClearIDBuffer();
                        axEWdraw3.GetGroupAllIDs(ent);
                        int grpentsize = axEWdraw3.GetIDBufferSize();
                        for (int j = 0; j < grpentsize; j++)
                        {
                            ent = axEWdraw3.GetIDBuffer(j);
                            axEWdraw3.SetTransparency(ent, 0.7);
                        }
                    }
                    else
                    {
                        int enttype = axEWdraw3.GetEntType(ent);
                        if (enttype == 501/*三合一*/ || enttype == 502/*木楔*/ || enttype == 503/*二合一*/)
                        {
                            axEWdraw3.SetEntColor(ent, axEWdraw3.RGBToIndex(255, 255, 0));
                        }
                        else
                            axEWdraw3.SetTransparency(ent, 0.7);
                    }
                }
            }
            else
            {
                button41.Text = "显示五金";
                int entsize = axEWdraw3.GetEntSize();
                int ent = 0;
                for (int i = 1; i <= entsize; i++)
                {
                    ent = axEWdraw3.GetEntID(i);
                    if (axEWdraw3.IsGroup(ent))
                    {
                        axEWdraw3.ClearIDBuffer();
                        axEWdraw3.GetGroupAllIDs(ent);
                        int grpentsize = axEWdraw3.GetIDBufferSize();
                        for (int j = 0; j < grpentsize; j++)
                        {
                            ent = axEWdraw3.GetIDBuffer(j);
                            axEWdraw3.SetTransparency(ent, 0);
                        }
                    }
                    else
                    {
                        int enttype = axEWdraw3.GetEntType(ent);
                        if (enttype == 501/*三合一*/ || enttype == 502/*木楔*/ || enttype == 503/*二合一*/)
                        {
                            axEWdraw3.Delete(ent);
                        }
                        else
                            axEWdraw3.SetTransparency(ent, 0);
                    }
                }

            }
        }
        //得到修改前柜体横向或竖向相关的柜体 2020-03-23
        private void GetCabinetBndBeforeModify(int id, ref CBoundingBox before, ref ArrayList bounds, ref ArrayList bounds1)
        {
            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
            int type = 0;
            int entsize = 0;
            double minx, miny, minz, maxx, maxy, maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            axEWdraw3.GetGroupAxis(id, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
            if (bounds == null)
                bounds = new ArrayList();
            else
                bounds.Clear();
            if (bounds1 == null)
                bounds1 = new ArrayList();
            else
                bounds1.Clear();

            if (Math.Abs(xdx) > 0.001 && Math.Abs(xdy) < 0.001 && Math.Abs(xdz) < 0.001)
            {//横向,X轴
                if(xdx<0.0)
                    type = 2;
                else
                    type = 1;
            }
            else if (Math.Abs(xdx) < 0.001 && Math.Abs(xdy) > 0.001 && Math.Abs(xdz) < 0.001)
            {//深度方向,Y轴
                if(xdy<0.0)
                    type = 4;
                else
                    type = 3;
            }
            before.id = id;
            axEWdraw3.GetEntBoundingBox(id, ref before.minx, ref before.miny, ref before.minz, ref before.maxx, ref before.maxy, ref before.maxz);
            entsize = axEWdraw3.GetEntSize();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw3.GetEntID(i);
                if (ent != id)
                {
                    axEWdraw3.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    switch(type)
                    {
                        case 1:
                        case 2:
                            {
                                CBoundingBox abox = new CBoundingBox();
                                abox.id = ent;
                                abox.minx = minx;
                                abox.miny = miny;
                                abox.minz = minz;
                                abox.maxx = maxx;
                                abox.maxy = maxy;
                                abox.maxz = maxz;
                                abox.type = type;
                                if (maxx > before.maxx)
                                {
                                    bounds.Add(abox);
                                }
                                else
                                {
                                    bounds1.Add(abox);
                                }
                            }
                            break;
                        case 3:
                        case 4:
                            {
                                CBoundingBox abox = new CBoundingBox();
                                abox.id = ent;
                                abox.minx = minx;
                                abox.miny = miny;
                                abox.minz = minz;
                                abox.maxx = maxx;
                                abox.maxy = maxy;
                                abox.maxz = maxz;
                                abox.type = type;
                                if (miny > before.miny)
                                {
                                    bounds.Add(abox);
                                }
                                else if (maxy < before.maxy)
                                {
                                    bounds1.Add(abox);
                                }
                            }
                            break;
                    }
                }
            }
        }
        //在柜体被修改后,移动相关的柜体
        private void MoveCabinetAfterModify(int id, 
            ref CBoundingBox before,
            ref CBoundingBox after,
            ref ArrayList beforelist,
            ref ArrayList beforelist1
            )
        {
            double orgx, orgy, orgz, dx, dy, dz, xdx, xdy, xdz;
            orgx = orgy = orgz = dx = dy = dz = xdx = xdy = xdz = 0.0;
            double xsub = 0;
            double xsub1 = 0;
            double ysub = 0;
            double ysub1 = 0;
            double minx,miny,minz,maxx,maxy,maxz;
            minx = miny = minz = maxx = maxy = maxz = 0.0;
            CBoundingBox orgbox = new CBoundingBox();
            if (id > 0)
            {
                bool isright = false;
                bool isleft = false;
                axEWdraw3.GetGroupAxis(id, ref orgx, ref orgy, ref orgz, ref dx, ref dy, ref dz, ref xdx, ref xdy, ref xdz);
                if (Math.Abs(xdx) > 0.001 && Math.Abs(xdy) < 0.001 && Math.Abs(xdz) < 0.001)
                {//横向,X轴
                    xsub = after.maxx - before.maxx;
                    xsub1 = after.minx - before.minx;
                    if(Math.Abs(after.minx-before.minx)>0.001)
                        isleft = true;
                    if (Math.Abs(after.maxx - before.maxx) > 0.001)
                        isright = true;
                }
                else if (Math.Abs(xdx) < 0.001 && Math.Abs(xdy) > 0.001 && Math.Abs(xdz) < 0.001)
                {//深度方向,Y轴
                    ysub = after.maxy - before.maxy;
                    ysub1 = after.miny - before.miny;
                    if (Math.Abs(after.maxy - before.maxy) > 0.001)
                       isright = true;
                    if (Math.Abs(after.miny - before.miny) > 0.001)
                        isleft = true;


                }
                for (int i = 0; i < beforelist.Count; i++)
                {
                    switch (((CBoundingBox)beforelist[i]).type)
                    {
                        case 1:
                        case 2:
                            {
                                if(isright)
                                axEWdraw3.MoveTo(((CBoundingBox)beforelist[i]).id,
                                    new object[] { 0, 0, 0 },
                                    new object[] { xsub, 0, 0 });
                            }
                            break;
                        case 3:
                        case 4:
                            {
                                if (isright)
                                axEWdraw3.MoveTo(((CBoundingBox)beforelist[i]).id,
                                    new object[] { 0, 0, 0 },
                                    new object[] { 0, ysub, 0 });
                            }
                            break;
                    }
                }

                for (int i = 0; i < beforelist1.Count; i++)
                {
                    switch (((CBoundingBox)beforelist1[i]).type)
                    {
                        case 1:
                        case 2:
                            {
                                if (isleft)
                                    axEWdraw3.MoveTo(((CBoundingBox)beforelist1[i]).id,
                                        new object[] { 0, 0, 0 },
                                        new object[] {xsub1, 0, 0 });
                            }
                            break;
                        case 3:
                        case 4:
                            {
                                if (isleft)
                                axEWdraw3.MoveTo(((CBoundingBox)beforelist1[i]).id,
                                    new object[] { 0, 0, 0 },
                                    new object[] { 0, ysub1, 0 });
                            }
                            break;
                    }
                }
            }
            
        }

        private void SaveWork()
        {
            HideWJ();//消隐五金 2020-03-20
            RestoreExplodeCabinet();//2020-04-20
            if (g_orderid != null)
            {
                string filename = ConvertPrjIDToEWDPath("", false);
                if (filename.Length > 0)
                {
                    if (isCabinetDesign)
                    {//如果已经是设计状态,则关闭该状态
                        if (g_cabinetdesignid > 0)
                            axEWdraw3.SetCabinetDesignMode(g_cabinetdesignid, false);
                        axEWdraw3.SetViewCondition(8);
                        //
                        button12.Visible = false;
                        button13.Visible = false;
                        button14.Visible = false;
                        button15.Visible = false;
                        button16.Visible = true;
                        button17.Visible = false;
                        button18.Visible = false;
                        button23.Visible = true;//2020-01-25
                        button24.Visible = true;
                        button25.Visible = true;
                        button26.Visible = true;
                        button32.Visible = false;//2020-02-17
                        //
                        button27.Left += 100;
                        button28.Left += 100;
                        button29.Left += 100;
                        button30.Left += 100;
                        button31.Left += 100;
                        button42.Left += 100;
                        button43.Left += 100;
                        button44.Left += 100;
                        //
                        isCabinetDesign = false;
                        InitOrgPlanks(false);
                        listView7.Items[3].ImageIndex = 2;
                        listView7.Refresh();
                        //
                        axEWdraw3.ClearAllUndoRedo();
                        if (GDesignMode == 2 || GDesignMode == 1 || GDesignMode == 3)//2020-02-17
                        {
                            GDesignMode = -1;
                        }
                        ResetInitJiaJuKu();
                    }
                    axEWdraw3.SaveEwd(filename);
                    axEWdraw3.SetPerspectiveMode(true);
                }
            }
            else MessageBox.Show("请先创建或打开文件!");
        }
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button42_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();//2020-03-24
            axEWdraw3.ZoomALL();
        }

        private void button43_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.ToDrawOrbit();
        }

        private void button44_Click(object sender, EventArgs e)
        {
            axEWdraw3.CancelCommand();
            axEWdraw3.ToDrawPan();

        }
        //获取画墙的显示域区,当前的中心点
        private void GetDrawWallScreenCenter(ref double x,ref double y)
        {
            double x1,y1,z1,x2,y2,z2;
            x1 = y1 = z1 = x2 = y2 = z2 = 0.0;
            axEWdraw1.Screen2Coordinate(0, 0, ref x1, ref y1, ref z1);
            axEWdraw1.Screen2Coordinate(axEWdraw1.Width, axEWdraw1.Height, ref x2, ref y2, ref z2);
            x = (x1 + x2) / 2.0;
            y = (y1 + y2) / 2.0;
        }
        //将柜体转入户型
        private void button45_Click(object sender, EventArgs e)
        {
            
            if (axEWdraw1.GetSingleWallSize() >= 3)
            {
                if (axEWdraw3.GetEntSize() > 0)
                {
                    tabControl1.SelectedIndex = 0;
                    InitDrawWall();
                    int size = axEWdraw3.GetEntSize();
                    int ent = 0;
                    for (int i = 1; i <= size; i++)
                    {
                        ent = axEWdraw3.GetEntID(i);
                        if (ent > 0)
                        {
                            if (axEWdraw3.IsGroup(ent))
                            {
                                axEWdraw3.ClearIDBuffer();
                                if (axEWdraw3.GetGroupAllIDs(ent))
                                {
                                    int idsize = axEWdraw3.GetIDBufferSize();
                                    for (int j = 0; j < idsize; j++)
                                    {
                                        axEWdraw3.AddEntToShare(axEWdraw3.GetIDBuffer(j));
                                    }
                                }
                                axEWdraw3.ClearIDBuffer();
                            }
                            else
                            {
                                axEWdraw3.AddEntToShare(axEWdraw3.GetIDBuffer(ent));
                            }
                        }
                    }
                    int grpid = 0;
                    double midx,midy;
                    midx = midy = 0;
                    GetDrawWallScreenCenter(ref midx, ref midy);
                    axEWdraw1.GetAllEntFromShare(true, true, new object[] {0,0,0 }, "insert_group" + insertgroupadd.ToString(), ref grpid);
                    axEWdraw1.SetGroupPlaneByBoxFace(grpid, 2);
                    axEWdraw1.MoveTo(grpid, new object[] { 0, 0, 0 }, new object[] { midx, midy, 0 });
                    axEWdraw3.ClearShare();
                }
                else
                    MessageBox.Show("请先生成柜体");
            }else
                MessageBox.Show("请先绘制墙体");
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (g_isactiveupdate)//2021-05-29
            {
                g_isactiveupdate = false;
                if (axEWdraw2.Visible)
                    axEWdraw2.UpdateView();
                if (axEWdraw3.Visible)
                    axEWdraw3.UpdateView();
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            g_isactiveupdate = true;
        }

        private void axEWdraw3_EnterNumInfo(object sender, AxEWDRAWLib._DAdrawEvents_EnterNumInfoEvent e)
        {
            textBox2.Top = e.yWin;
            textBox2.Left = e.xWin;
            textBox2.Visible = true;
            textBox2.Text = e.info;
            textBox2.Focus();
            textBox2.Select(textBox2.TextLength, 0);

        }



    }
}