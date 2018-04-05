using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Core
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
        
        private static string GetDefaultPath(string folderName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string defaultImportDir = baseDir.Replace("\\bin\\", "\\" + folderName + "\\");
            return defaultImportDir;
        }

    }
}
