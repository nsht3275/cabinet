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
using System.Drawing.Imaging;
using Microsoft.Reporting.WinForms;
using RDLCPrinter;
namespace Room
{
    public partial class Form18 : Form
    {
        public DataTable dt = null;//2020-01-22
        public string orderidstr = "";//订单号
        public string username = "";//柜体名称
        public ArrayList imagefiles;//排版图文件列表
        public ArrayList codefiles;//条形码文件列表
        public ArrayList cabinetnames;//柜体名称列表
        public ArrayList matnames;//材质名称列表
        public ArrayList matcolors;//材质颜色
        public ArrayList planksizes;//板材的大小列表
        public Form18()
        {
            InitializeComponent();
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
        public void MakeReport()
        {
            Form10.g_static_fm10.MakeBQData();
            username = Form10.g_static_fm10.m_username;//2020-02-28
            orderidstr = Form10.g_static_fm10.m_orderid;
            imagefiles = Form10.g_static_fm10.imagefiles;//排版图文件列表
            codefiles = Form10.g_static_fm10.codefiles;//条形码文件列表
            cabinetnames = Form10.g_static_fm10.cabinetnames;//柜体名称列表
            matnames = Form10.g_static_fm10.matnames;//材质名称列表
            matcolors = Form10.g_static_fm10.matcolors;//材质颜色
            planksizes = Form10.g_static_fm10.planksizes;//板材的大小列表

            this.rptView.Clear();
            this.rptView.LocalReport.DataSources.Clear();
            //写入报表内容
            //第二步：指定报表路径
            this.rptView.LocalReport.ReportPath = "Report2.rdlc";
            //第三步：构造新的DataTable
            dt = new DataTable("DataTable2");
            dt.Columns.Add("DDH1");
            dt.Columns.Add("USER1");
            dt.Columns.Add("SIZE1");
            dt.Columns.Add("GUI1");
            dt.Columns.Add("MAT1");
            dt.Columns.Add("COLOR1");
            dt.Columns.Add("PIC1");
            dt.Columns.Add("CODE1");
            dt.Columns.Add("DDH2");
            dt.Columns.Add("USER2");
            dt.Columns.Add("SIZE2");
            dt.Columns.Add("GUI2");
            dt.Columns.Add("MAT2");
            dt.Columns.Add("COLOR2");
            dt.Columns.Add("PIC2");
            dt.Columns.Add("CODE2");
            //
            int tmprow = imagefiles.Count % 2;
            int tmpsize = imagefiles.Count / 2;
            for (int i = 0; i < tmpsize; i++)
            {
                Bitmap img = new Bitmap((string)imagefiles[i * 2]);
                byte[] buffer = BitmapToBytes(img);

                Bitmap img1 = new Bitmap((string)codefiles[i * 2]);
                byte[] buffer1 = BitmapToBytes(img1);

                Bitmap img2 = new Bitmap((string)imagefiles[i * 2 + 1]);
                byte[] buffer2 = BitmapToBytes(img2);

                Bitmap img3 = new Bitmap((string)codefiles[i * 2 + 1]);
                byte[] buffer3 = BitmapToBytes(img3);

                dt.Rows.Add(new object[] {"订单号:"+orderidstr,
                                    "用户名:"+username,
                                    (string)planksizes[i*2],
                                    (string)cabinetnames[i*2],
                                    (string)matnames[i*2],
                                    (string)matcolors[i*2],
                                    Convert.ToBase64String(buffer),
                                    Convert.ToBase64String(buffer1),
                                    "订单号:"+orderidstr,
                                    "用户名:"+username,
                                    (string)planksizes[i*2+1],
                                    (string)cabinetnames[i*2+1],
                                    (string)matnames[i*2+1],
                                    (string)matcolors[i*2+1],
                                    Convert.ToBase64String(buffer2),
                                    Convert.ToBase64String(buffer3)});
                img.Dispose();
                img1.Dispose();
                img2.Dispose();
                img3.Dispose();


            }
            if (tmprow == 1)
            {
                Bitmap img = new Bitmap((string)imagefiles[tmpsize * 2]);
                byte[] buffer = BitmapToBytes(img);

                Bitmap img1 = new Bitmap((string)codefiles[tmpsize * 2]);
                byte[] buffer1 = BitmapToBytes(img1);

                dt.Rows.Add(new object[] {"订单号:"+orderidstr,
                                      "用户名:"+username,
                                      (string)planksizes[tmpsize*2],
                                      (string)cabinetnames[tmpsize*2],
                                      (string)matnames[tmpsize*2],
                                      (string)matcolors[tmpsize*2],
                                      Convert.ToBase64String(buffer),
                                      Convert.ToBase64String(buffer1),
                                      "",
                                      "",
                                      "",
                                      "",
                                      "",
                                      "",
                                      "",
                                      ""});
                img.Dispose();
                img1.Dispose();

            }


            //名称不能写错，和报表中的数据集名称一致
            ReportDataSource rdsItem = new ReportDataSource("DataSet2", dt);
            //此处可以有多个数据源
            this.rptView.LocalReport.DataSources.Add(rdsItem);


            this.rptView.ZoomMode = ZoomMode.Percent;
            this.rptView.ZoomPercent = 100;
            //第五步：刷新报表
            this.rptView.LocalReport.EnableExternalImages = true;
            this.rptView.ZoomPercent = 133;

            this.rptView.RefreshReport();
        }
        private void Form18_Load(object sender, EventArgs e)
        {
            Text = "标签打印 --- 正在生成......";
            button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MakeReport();
            Text = "标签打印 --- 报表完成";
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LocalReport rept = this.rptView.LocalReport;
            double top = rept.GetDefaultPageSettings().Margins.Top / 100.0 * 2.54;
            double bottom = rept.GetDefaultPageSettings().Margins.Bottom / 100.0 * 2.54;
            double left = rept.GetDefaultPageSettings().Margins.Left / 100.0 * 2.54;
            double right = rept.GetDefaultPageSettings().Margins.Right / 100.0 * 2.54;

            BillPrint.Run(rept, top, bottom, left, right);

        }
    }
}
