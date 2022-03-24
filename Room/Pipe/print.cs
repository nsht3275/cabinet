
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;

namespace RDLCPrinter
{
    /// <summary>
    /// ͨ��RDLC��Ĭ�ϴ�ӡ�������ӡ����
    /// </summary>
    public class BillPrint:IDisposable
    {
        string m_reportfilename;
        /// <summary>
        /// ��ǰ��ӡҳ��
        /// </summary>
        static int m_currentPageIndex;

        /// <summary>
        /// RDCLת��streamһҳ��Ӧһ��stream
        /// </summary>
        static List<Stream> m_streams;

        /// <summary>
        /// ��report�����stream
        /// </summary>
        /// <param name="report">������ҪExport��report</param>
        private void Export(LocalReport report, double top, double bottom, double left, double right)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
                "  <MarginTop>"+top.ToString("0.0")+"cm</MarginTop>" +
                "  <MarginLeft>" + left.ToString("0.0") + "cm</MarginLeft>" +
                "  <MarginRight>" + right.ToString("0.0") + "cm</MarginRight>" +
                "  <MarginBottom>" + bottom.ToString("0.0") + "cm</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        /// <summary>
        /// ��������ָ�������ƺ͸�ʽ������
        /// </summary>
        private Stream CreateStream(string name, string fileNameExtension,
      Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\"+name + "." + fileNameExtension,
              FileMode.Create);
            m_reportfilename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\"+name + "." + fileNameExtension;
            m_streams.Add(stream);
            return stream;
        }

        /// <summary>
        /// ��ӡ���
        /// </summary>
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage =
              new Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        /// <summary>
        /// ��ӡԤ����
        /// </summary>
        private void Print()
        {
            PrintDocument printDoc = new PrintDocument();
            string printerName = printDoc.PrinterSettings.PrinterName;
            if (m_streams == null || m_streams.Count == 0)
                return;
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".", printerName);
                throw new Exception(msg);
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            StandardPrintController spc = new StandardPrintController();
            printDoc.PrintController = spc;
            printDoc.Print();
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        /// <summary>
        /// ����ӿ�,������ӡ
        /// </summary>
        /// <param name="dtSource">��ӡ�����Ӧ������Դ</param>
        /// <param name="sReport">��ӡ��������</param>
        public static void Run(LocalReport report,double top,double bottom,double left,double right)
        {
            m_currentPageIndex = 0;
            BillPrint billPrint = new BillPrint();
            billPrint.Export(report,top,bottom,left,right);
            billPrint.Print();
            billPrint.Dispose();
            if (!string.IsNullOrEmpty(billPrint.m_reportfilename))//2020-04-21
            {
                if (System.IO.File.Exists(billPrint.m_reportfilename))
                {
                    string mydoc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    if (billPrint.m_reportfilename.IndexOf(mydoc) >= 0 && billPrint.m_reportfilename.Length > mydoc.Length)
                    {
                        File.Delete(billPrint.m_reportfilename);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ��ӡ��״̬
        /// </summary>
        /// <param name="printerName">��ӡ������</param>
        /// <param name="status">�����ӡ��״̬</param>
        private static void GetPrinterStatus2(string printerName, ref uint status)
        {
            try
            {

                string lcPrinterName = printerName;
                IntPtr liHandle = IntPtr.Zero;
                if (!Win32.OpenPrinter(lcPrinterName, out liHandle, IntPtr.Zero))
                {
                    Console.WriteLine("print  is close");
                    return;
                }
                UInt32 level = 2;
                UInt32 sizeNeeded = 0;
                IntPtr buffer = IntPtr.Zero;
                Win32.GetPrinter(liHandle, level, buffer, 0, out sizeNeeded);
                buffer = Marshal.AllocHGlobal((int)sizeNeeded);
                if (!Win32.GetPrinter(liHandle, level, buffer, sizeNeeded, out sizeNeeded))
                {
                    Console.WriteLine(Environment.NewLine + "Fail GetPrinter:" + Marshal.GetLastWin32Error());
                    return;
                }

                Win32.PRINTER_INFO_2 info = (Win32.PRINTER_INFO_2)Marshal.PtrToStructure(buffer, typeof(Win32.PRINTER_INFO_2));
                status = info.Status;
                Marshal.FreeHGlobal(buffer);
                Win32.ClosePrinter(liHandle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ����ӿ�,��ȥ��ӡ����Ϣ
        /// </summary>
        /// <param name="printerName">��ӡ������</param>
        /// <returns>���ش�ӡ����ǰ״̬</returns>
        public static string GetPrinterStatus(string printerName)
        {
            uint intValue = 0;
            PrintDocument pd = new PrintDocument();
            printerName = printerName == "" ? pd.PrinterSettings.PrinterName : printerName;
            GetPrinterStatus2(printerName, ref intValue);
            string strRet = string.Empty;
            switch (intValue)
            {
                case 0:
                    strRet = "׼��������Ready��";
                    break;
                case 4194432:
                    strRet = "���򿪣�Lid Open��";
                    break;
                case 144:
                    strRet = "��ӡֽ���꣨Out of Paper��";
                    break;
                case 4194448:
                    strRet = "���򿪲��Ҵ�ӡֽ���꣨Out of Paper && Lid Open��";
                    break;
                case 1024:
                    strRet = "��ӡ�У�Printing��";
                    break;
                case 32768:
                    strRet = "��ʼ����Initializing��";
                    break;
                case 160:
                    strRet = "�ֹ���ֽ(Manual Feed in Progress)";
                    break;
                case 4096:
                    strRet = "�ѻ�(Offline)";
                    break;
                default:
                    strRet = "δ֪״̬��unknown state��";
                    break;
            }
            return strRet;
        }
    }
    public class Win32
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool OpenPrinter(string printer, out IntPtr handle, IntPtr printerDefaults);
        [DllImport("winspool.drv")]
        public static extern bool ClosePrinter(IntPtr handle);
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetPrinter(IntPtr handle, UInt32 level, IntPtr buffer, UInt32 size, out UInt32 sizeNeeded);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PRINTER_INFO_2
        {
            public string pServerName;
            public string pPrinterName;
            public string pShareName;
            public string pPortName;
            public string pDriverName;
            public string pComment;
            public string pLocation;
            public IntPtr pDevMode;
            public string pSepFile;
            public string pPrintProcessor;
            public string pDatatype;
            public string pParameters;
            public IntPtr pSecurityDescriptor;
            public UInt32 Attributes;
            public UInt32 Priority;
            public UInt32 DefaultPriority;
            public UInt32 StartTime;
            public UInt32 UntilTime;
            public UInt32 Status;
            public UInt32 cJobs;
            public UInt32 AveragePPM;
        }
    }
}

