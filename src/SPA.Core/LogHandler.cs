using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SPA.Config;

namespace SPA.Core
{
    public class LogHandler
    {

        private static string ErrorFileName { 
            get
            {
                return "error_" + String.Format("{0:yyyyMMdd}", DateTime.Today) + ".txt";
            }
        }

        public static void WriteError(string error)
        {
            System.IO.File.AppendAllText(DirectoryHandler.GetDefaultDirectoryLog() + ErrorFileName, Environment.NewLine + GetErrorLogLine(error));
        }

        public static void WriteError(Exception error)
        {
            System.IO.File.AppendAllText(DirectoryHandler.GetDefaultDirectoryLog() + ErrorFileName, Environment.NewLine + GetErrorLogLine(error));
        }

        private static string GetErrorLogLine(string error)
        {
            return "[" + String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now) + "] " + error;
        }

        private static string GetErrorLogLine(Exception error)
        {
            return "[" + String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now) + "] " + error.Message + Environment.NewLine  + "*** Trace: " + Environment.NewLine + error.StackTrace;
        }

    }
}
