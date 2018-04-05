using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Core;

namespace SPA.Data
{
    public class DbDeleter 
    {
        public string ErrorMessage;
        public bool Execute(String sql)
        {
            try
            {
                var sisproConn = new SPA.Data.SPAConnection().Connection;
                if (sisproConn.State != System.Data.ConnectionState.Open)
                {
                    sisproConn.Open();  
                }
                
                var updateCommand = new SqlCommand(sql, sisproConn);
                updateCommand.ExecuteNonQuery();

                if (sisproConn.State != System.Data.ConnectionState.Closed)
                {
                    sisproConn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                LogHandler.WriteError(ex);
                return false;
                throw;
            }

            return true;
        }
    }
}
