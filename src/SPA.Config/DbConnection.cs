using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SPA.Cache;

namespace SPA.Config
{
    public class DbConnection
    {
        //TODO: this connectionstring should be read from xml cofig file.
        //private string connString = "Data Source=KYGMB-SERVER\\SPA;Initial Catalog=SPADB;User ID=sa;Password=#123456#;Max Pool Size=30000";
        private string connString = "Data Source="+ ConfigCache.DBServer + ";Initial Catalog="+ ConfigCache.DBName +";User ID=sa;Password=#123456#;Max Pool Size=30000";
        public  string ConnectionString
        {
            get
            {
                return connString;
            }
            set
            {
                connString = value;
            }
        }

        public DbConnection()
        {
           
        }
    }
}
