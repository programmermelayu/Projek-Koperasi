using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SPA.Data
{
    public class DbUpdater
    {
        public  string ErrorMessage;
        public  bool Execute(String sql)
        {
            var spaConnection = new SPA.Data.SPAConnection().Connection;
            try
            {
                if (spaConnection.State != System.Data.ConnectionState.Open)
                {
                    spaConnection.Open();  
                }

                var updateCommand = new SqlCommand(sql, spaConnection);
                updateCommand.ExecuteNonQuery();

                if (spaConnection.State != System.Data.ConnectionState.Closed)
                {
                    spaConnection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (spaConnection.State != System.Data.ConnectionState.Closed)
                {
                    spaConnection.Close();
                }
            }

            return true;
        }
    }
}
