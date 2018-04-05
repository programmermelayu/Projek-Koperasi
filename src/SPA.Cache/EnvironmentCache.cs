using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

namespace SPA.Cache
{
    public class EnvironmentCache
    {
        public static bool IsDebug()
        {
            var myComputer = new Computer();
            if (myComputer.Name == "LENOVO-PC")
            {
                return true;
            }
            return false;
        }
    }
}
