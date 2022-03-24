using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Room
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            axEWdraw1.DisplayTrihedron(true);
            axEWdraw1.AddOne2DVertex(-1000,-1000 , 0);
            axEWdraw1.AddOne2DVertex(-1000, 1000, 0);
            axEWdraw1.AddOne2DVertex(1000, 1000, 0);
            axEWdraw1.AddOne2DVertex(-1000, -2000, 0);
            int tmpent = axEWdraw1.PolyLine2D(true);
            axEWdraw1.Clear2DPtBuf();
            axEWdraw1.SetViewCondition(8);
            axEWdraw1.Delete(tmpent);
            axEWdraw1.SetBackGroundColor(axEWdraw1.RGBToIndex(150, 150, 200));
            axEWdraw1.SetLayerColor("0", axEWdraw1.RGBToIndex(220, 220, 220));
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawCylinder();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                axEWdraw1.SendCommandStr(textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void axEWdraw1_GetProcInfo(object sender, AxEWDRAWLib._DAdrawEvents_GetProcInfoEvent e)
        {
            label1.Text = e.info;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int entsize = axEWdraw1.GetEntSize();
            double tminx,tminy,tminz,tmaxx,tmaxy,tmaxz;
            double minx,miny,minz,maxx,maxy,maxz;
            tminx=tminy=tminz=tmaxx=tmaxy=tmaxz=minx=miny=minz=maxx=maxy=maxz=0.0;
            int tmpjs = 0;
            axEWdraw1.ClearIDBuffer();
            for (int i = 1; i <= entsize; i++)
            {
                int ent = axEWdraw1.GetEntID(i);
                if (axEWdraw1.Is3DSolid(ent))
                {
                    axEWdraw1.GetEntBoundingBox(ent, ref minx, ref miny, ref minz, ref maxx, ref maxy, ref maxz);
                    if (tmpjs == 0)
                    {
                        tminx = minx;
                        tminy = miny;
                        tminz = minz;
                        tmaxx = maxx;
                        tmaxy = maxy;
                        tmaxz = maxz;
                    }
                    else
                    {
                        tminx = Math.Min(tminx,minx);
                        tminy = Math.Min(tminy, miny);
                        tminz = Math.Min(tminz, minz);
                        tmaxx = Math.Max(tmaxx, maxx);
                        tmaxy = Math.Max(tmaxy, maxy);
                        tmaxz = Math.Max(tmaxz, maxz);
                    }
                    axEWdraw1.AddIDToBuffer(ent);
                    tmpjs++;
                }
            }
            int grp = axEWdraw1.MakeGroup("selfdraw", new object[] { 0, 0, 0 });
            string str = "xlength:" + String.Format("{0:f3}", tmaxx - tminx) +";"+ "ylength:" + String.Format("{0:f3}", tmaxy - tminy) +";"+ "zlength:" + String.Format("{0:f3}", tmaxz - tminz)+";";
            axEWdraw1.SetEntityUserData(grp, str);
            axEWdraw1.ClearIDBuffer();
            axEWdraw1.SetGroupInsPt(grp, new object[] { tminx, tmaxy, tminz });
            axEWdraw1.SetGroupPlaneByBoxFace(grp, 2);
            if (zPubFun.zPubFunLib.g_istriallimit)//2020-04-20
            {
                MessageBox.Show("试用版不支持该功能");
            }
            else
            {
                if (File.Exists("selfdraw.ewd"))
                    File.Delete("selfdraw.ewd");
                axEWdraw1.WriteGroup(grp, "selfdraw.ewd");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawSphere();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawCone();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawTorus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawMove();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            axEWdraw1.CancelCommand();
            axEWdraw1.ToDrawRotate();
        }
    }
}
