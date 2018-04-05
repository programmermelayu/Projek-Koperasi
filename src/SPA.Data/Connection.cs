using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using SPA.Config;

namespace SPA.Data
{
  

    public class SPAConnection  
    {
        public SqlConnection Connection
        {
            get
            {
                DbConnection dbConn = new DbConnection();
                return new SqlConnection(dbConn.ConnectionString);
            }
        }


    }
}
