using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using SPA.Config;

namespace SPA.Cache
{
    public class ConfigCache
    {
        public static string DBServer  { get; set; }
        public static string DBName { get; set; }

        public static void LoadDbConfig()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                FileStream fs = new FileStream(DirectoryHandler.GetDefaultConfigPath() + "\\DbConfig.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("Database");
                DBServer = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
                DBName = xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
            }
            catch
            {

            }
        }
    }
}
