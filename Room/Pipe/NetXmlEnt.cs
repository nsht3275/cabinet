using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
//范围盒 2020-03-20
public class CBoundingBox
{
    public CBoundingBox()
    {
        id = 0;
        type = -1;
        minx = miny = minz = maxx = maxy = maxz = 0.0;
    }
    public int id;
    public int type;
    public double minx;
    public double miny;
    public double minz;
    //
    public double maxx;
    public double maxy;
    public double maxz;

}
//五金类 //2019-12-19
public class CWJTClass
{
    public CWJTClass()
    {
        id = 0;
        name = "";
        color = "";
        num = 0;
        length = 0;
        total = 0.0;
    }
    public int id;//id号
    public string name;//名称
    public string color;//规格
    public int num;//数量
    public double length;//长度,衣通用
    public double total;//分段计价的汇总,用于衣通
}

//板材厚度类 //2019-12-19
public class CPlankTClass
{
    public CPlankTClass()
    {
        thickness = 18.0;
        classid = "";
        plankid = 0;
    }
    public double thickness;//板的厚度
    public string classid;
    public int    plankid;
}
public class CReportPlank
{
    public string tcode;//编号
    public string gtmc;//柜体名称
    public string mokuai;//模块名称
    public string mingcheng;//名称
    public string yanse;//颜色
    public string chang;//长
    public string kuan;//宽
    public string hou;//厚
    public string shuliang;//数量
    public string danwei;//单位
    public string danjia;//单价
    public string xiaoji;//小计
    public string imagefile = "";//2020-04-14
}
public class CPlank
{
    public int id;
    public double minx, miny, minz, maxx, maxy, maxz;
    public double width, depth, height;

}

class CMyGrps
{
    public string m_name = "";
    public string m_csname = "";//中文别名
    public ArrayList m_ids = new ArrayList();
    public string m_imagefile = "";//2020-04-14
};
//材料表 //2019-12-06
namespace zPubFun
{
    public class zPubFunLib
    {
        public static bool g_istriallimit = true;//试用版限制
        public static double CStr2Double(string str)
        {
            if (str.Trim().Length <= 0)
            {
                return 0.0;
            }
            double r = 0.0;
            bool isfinish = double.TryParse(str,out r);
            if (isfinish)
                return r;
            return 0.0;
        }
        public static double CStr2Double(double val)
        {
            return val;
        }
        public static double CStr2Double(object val)
        {
            string str = val.ToString();
            if (str.Trim().Length <= 0)
            {
                return 0.0;
            }
            double r = 0.0;
            bool isfinish = double.TryParse(str, out r);
            if (isfinish)
                return r;
            return 0.0;
        }
        //
        public static int CStr2Int(string str)
        {
            if (str.Trim().Length <= 0)
            {
                return 0;
            }
            int r = 0;
            bool isfinish = int.TryParse(str, out r);
            if (isfinish)
                return r;
            return 0;
        }
        public static int CStr2Int(int val)
        {
            return val;
        }
        public static int CStr2Int(object val)
        {
            string str = val.ToString();
            if (str.Trim().Length <= 0)
            {
                return 0;
            }
            int r = 0;
            bool isfinish = int.TryParse(str, out r);
            if (isfinish)
                return r;
            return 0;
        }

    }
}
//刀具管理 2020-04-27
namespace zDaoJu
{
    public class DaoJuEntity
    {
        public DaoJuEntity()
        {
            Id = "";
            Dia = "";
            Begin = "";
            End = "";
            S = "1600";
            F = "4000";//钻洞的进给率
            F1 = "12000";//开料进给率
            F2 = "15000";//划槽进给率
            Up = "15";
            State = "";
            isCustom = "0";
        }
        public string Id { get; set; }//id号
        public string Dia { get; set; }//直经
        public string Begin { get; set; }//起始代码
        public string End { get; set; }//终止代码
        public string S { get; set; }//转速
        public string F { get; set; }//进给
        public string F1 { get; set; }//进给
        public string F2 { get; set; }//进给
        public string Up { get; set; }//状态
        public string State { get; set; }//状态
        public string isCustom { get; set; }//状态
    }
}
namespace zDaoJu
{
    public class DaoJuEnter : Netxmldb.xml.Base<DaoJuEntity>
    {
        private static DaoJuEnter inst = new DaoJuEnter();
        private DaoJuEnter() { }
        public static DaoJuEnter getInst()
        {
            return inst;
        }
    }
}

//材料表 //2019-12-06
namespace zCaiLiao
{
    public class CaiLiaoEntity:ICloneable
    {
        public CaiLiaoEntity()
        {
            IsCheck = false;
            Id = "";
            Name = "";
            Color = "";
            Width = "";
            Height = "";
            Texture = "";
            Class = "";
            Thickness = "";
            Unit = "";
            Memo = "";
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
        public bool   IsCheck { get; set; }//备注
        public string Id { get; set; }//id号由日期时间+4位随机数组成
        public string Name { get; set; }//名称
        public string Color { get; set; }//颜色
        public string Width { get; set; }//宽
        public string Height { get; set; }//高
        public string Texture { get; set; }//纹理文件
        public string Class { get; set; }//规格
        public string Thickness { get; set; }//厚度
        public string Unit { get; set; }//单价
        public string Memo { get; set; }//备注
    }
}

namespace zCaiLiao
{
    public class CaiLiaoEnter : Netxmldb.xml.Base<CaiLiaoEntity>
    {
        private static CaiLiaoEnter inst = new CaiLiaoEnter();
        private CaiLiaoEnter() { }
        public static CaiLiaoEnter getInst()
        {
            return inst;
        }
    }
}
//订单类
namespace NetXmlEnt
{
    public class CodeEntity
    {
        public string Id { get; set; }//id号由日期时间+4位随机数组成
        public string ClientClass { get; set; }//经销商或客户类别
        public string Client { get; set; }//经销商或客户名称
        public string OrderClass { get; set; }//订单类别
        public string Date{ get; set; }//日期
        public string Project { get; set; }//工程名称
        public string Address { get; set; }//地址
        public string Contacts { get; set; }//联系人
        public string Phone { get; set; }//电话
        public string Logistics { get; set; }//物流
        public string DDate { get; set; }//送货日期
        public string Memo { get; set; }//备注
    }
}

namespace NetXmlEnt
{
    public class CodeEnter : Netxmldb.xml.Base<CodeEntity>
    {
        private static CodeEnter inst = new CodeEnter();
        private CodeEnter() { }
        public static CodeEnter getInst()
        {
            return inst;
        }
    }
}

namespace Netxmldb.xml
{
    public class XmlSerializerEnter<T>
    {
        private string dbFile;
        private static XmlSerializerEnter<T> instance;
        public string Dbfile
        {
            get { return dbFile; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(dbFile))
                {
                    this.entityList.Clear();
                }
                dbFile = value;
                this.ReadDb();
            }
        }
        private List<T> entityList = new List<T>();
        private XmlSerializerEnter()
        {
            this.SetDbFile();
            this.ReadDb();
        }
        private void SetDbFile()
        {
            //2020-04-21
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EWDraw3DSet\\" + "EWDProject\\";
            try
            {
                if (Directory.Exists(folder) == false)
                {
                    Directory.CreateDirectory(folder);
                }
                Type type = typeof(T);
                if (string.IsNullOrEmpty(this.Dbfile))
                {
                    
                    this.Dbfile = Path.Combine(folder, type.Name + ".xml");
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public static XmlSerializerEnter<T> GetInstance()
        {
            if (instance == null)
            {
                instance = new XmlSerializerEnter<T>();
            }
            return instance;
        }
        public void Insert(T entity)
        {
            this.entityList.Add(entity);
            this.WriteDb();
        }
        public void Insert(T entity,int index)//2020-04-28
        {
            this.entityList.Insert(index,entity);
            this.WriteDb();
        }
        public void InsertBeforeWrite(T entity)//2019-12-18
        {
            this.entityList.Add(entity);
        }
        public void InsertRange(IList<T> list)
        {
            this.entityList.AddRange(list);
            this.WriteDb();
        }
        public System.Collections.Generic.List<T> SelectBy(string name, Object value)
        {
            System.Collections.Generic.List<T> list = new List<T>();
            if (value == null)
            {
                return list;
            }
            Type t = typeof(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == name.ToLower())
                    {
                        if (value.ToString() == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                        {
                            list.Add(inst);
                        }
                    }
                }
            }
            return list;
        }
        public T SelectById(string id)
        {
            Type t = typeof(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == "id")
                    {
                        if (id == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                        {
                            return inst;
                        }
                    }
                }
            }
            return default(T);
        }
        public void UpdateById(T entity)
        {
            Type t = typeof(T);
            string id = string.Empty;
            foreach (PropertyInfo pro in t.GetProperties())
            {
                if (pro.Name.ToLower() == "id")
                {
                    id = (pro.GetValue(entity, null) ?? string.Empty).ToString();
                    break;
                }
            }
            this.DeleteById(id);
            this.Insert(entity);
        }
        public void UpdateById(T entity,int index)//2020-04-28
        {
            Type t = typeof(T);
            string id = string.Empty;
            foreach (PropertyInfo pro in t.GetProperties())
            {
                if (pro.Name.ToLower() == "id")
                {
                    id = (pro.GetValue(entity, null) ?? string.Empty).ToString();
                    break;
                }
            }
            this.DeleteById(id);
            this.Insert(entity,index);
        }

        public void DeleteById(string id)
        {
            Type t = typeof(T);
            T entity = default(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == "id")
                    {
                        if ((pro.GetValue(inst, null) ?? string.Empty).ToString() == id)
                        {
                            entity = inst;
                            goto FinishLoop;
                        }
                    }
                }
            }
            FinishLoop:
            this.entityList.Remove(entity);
            this.WriteDb();
        }
        public List<T> SelectAll()
        {
            return this.entityList;
        }
        public void DeleteAll()
        {
            this.entityList.Clear();
            this.WriteDb();
        }
        public void DeleteAllBeforeWrite()//2019-12-18
        {
            this.entityList.Clear();
        }
        public void WriteDb()
        {
            if (!string.IsNullOrEmpty(this.Dbfile))//2020-04-21
            {
                XmlSerializer ks = new XmlSerializer(typeof(List<T>));
                FileInfo fi = new FileInfo(this.Dbfile);
                var dir = fi.Directory;
                if (!dir.Exists)
                {
                    dir.Create();
                }
                Stream writer = new FileStream(this.Dbfile, FileMode.Create, FileAccess.ReadWrite);
                ks.Serialize(writer, this.entityList);
                writer.Close();
            }
        }
        private void ReadDb()
        {
            if (File.Exists(this.Dbfile))
            {
                XmlSerializer ks = new XmlSerializer(typeof(List<T>));
                Stream reader = new FileStream(this.Dbfile, FileMode.Open, FileAccess.ReadWrite);
                this.entityList = ks.Deserialize(reader) as List<T>;
                reader.Close();
            }
            else
            {
                this.entityList = new List<T>();
            }
        }
    }
    public class Base<T> where T : new()
    {
        public string DbFile
        {
            get { return this.enter.Dbfile; }
            set { enter.Dbfile = value; }
        }
        private XmlSerializerEnter<T> enter = XmlSerializerEnter<T>.GetInstance();
        public void Delete(string id)
        {
            var entity = this.Select(id);
            enter.DeleteById(id);
        }
        public void Insert(T entity)
        {
            enter.Insert(entity);
        }
        public void InsertBeforeWrite(T entity)//2019-12-18
        {
            enter.InsertBeforeWrite(entity);
        }
        public void Insert(List<T> list)
        {
            enter.InsertRange(list);
        }
        public System.Collections.Generic.List<T> SelectAll()
        {
            return enter.SelectAll();
        }
        public void Update(T entity)//string oldidstr
        {
            enter.UpdateById(entity);
        }
        public void Update(T entity,int index)//2020-04-28
        {
            enter.UpdateById(entity,index);
        }
        public T Select(string id)
        {
            return enter.SelectById(id);
        }
        public System.Collections.Generic.List<T> SelectBy(string name, object value)
        {
            return enter.SelectBy(name, value);
        }
        public void DeleteAll()
        {
            enter.DeleteAll();
        }
        public void DeleteAllBeforeWrite()
        {
            enter.DeleteAllBeforeWrite();
        }
        public void WriteDb()
        {
            enter.WriteDb();
        }
    }
}
