using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Config
{
    public class DirectoryHandler
    {
        public static string GetDefaultDirectoryImport()
        {
            //todo: read from database, if not exist, read from default directory
            return GetDefaultPath("Import Files");
        }

        public static string GetDefaultDirectoryLog()
        {
            return GetDefaultPath("Log"); 
        }

        public static string GetDefaultConfigPath()
        {
            return GetDefaultPath("Config");
        }
        
        private static string GetDefaultPath(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string defaultImportDir = baseDir.Replace("\\bin\\", "\\" + fileName + "\\");
            return defaultImportDir;
        }

    }
}
