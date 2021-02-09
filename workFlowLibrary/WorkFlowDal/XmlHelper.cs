using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WorkFlowLibrary.WorkFlowModel;
using Models;

namespace WorkFlowLibrary.WorkFlowDal
{
    public class XmlHelper
    {
        public static readonly string paths = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "WorkFlowConfig.xml";//"D:/WorflowConfig.xml";
        public static string GetConnectString(string key)
        {
            XElement el = XElement.Load(paths);
            IEnumerable<XAttribute> childEl = from connect in el.Descendants("connectionstring")
                                              from add in connect.Descendants("add")
                                              where (string)add.Attribute("key") == key
                                              select add.Attribute("value");
            foreach (XAttribute xa in childEl)
            {
                return (string)xa;
            }
            return null;
        }

        public static IDictionary<string, string> GetWFTypeConfig(string key)
        {

            XElement rootXml = XElement.Load(paths);

            XElement config = (from tablename in rootXml.Descendants("tablename")
                               where (string)tablename.Attribute("type") == key
                               select tablename).First();
            IDictionary<string, string> retDic = new Dictionary<string, string>();

            retDic.Add("name", (string)config.Element("name"));
            retDic.Add("filter", (string)config.Element("filter"));
            retDic.Add("codecolum", (string)config.Element("codecolum"));
            retDic.Add("usercolum", (string)config.Element("usercolum"));
            retDic.Add("typename", (string)config.Element("typename"));
            retDic.Add("codemaintable", (string)config.Element("codemaintable"));
            retDic.Add("codemainvalue", (string)config.Element("codemainvalue"));
            retDic.Add("typecode", key);

            return retDic;
        }

        public static IList<ConfigModel> GetConfigAll()
        {
            XElement rootXml = XElement.Load(paths);
            IList<ConfigModel> list = new List<ConfigModel>();
            IEnumerable<XElement> configs = from tablename in rootXml.Descendants("tablename")
                                            select tablename;

            foreach (XElement elt in configs)
            {
                ConfigModel config = new ConfigModel();
                config.Codecolum = (string)elt.Element("codecolum");
                config.Filter = (string)elt.Element("filter");
                config.Tabname = (string)elt.Element("name");
                config.Typecode = (string)elt.Attribute("type");
                config.Typename = (string)elt.Element("typename");
                config.Usercolum = (string)elt.Element("usercolum");
                list.Add(config);
            }
            return list;
        }
    }

}
