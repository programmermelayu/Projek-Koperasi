using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Core;

namespace SPA.Data
{
    public class DbReader 
    {
        public static string ErrorMessage;
        public static SqlDataReader Execute(String sql)
        {
            SqlDataReader dr;

            try
            {
                var sisproConn = new SPA.Data.SPAConnection().Connection;
                if (sisproConn.State != System.Data.ConnectionState.Open)
                {
                    sisproConn.Open();  
                }
                
                var reader = new SqlCommand(sql, sisproConn);
                dr = reader.ExecuteReader();

                //if (sisproConn.State != System.Data.ConnectionState.Closed)
                //{
                //    sisproConn.Close();
                //}
            }
            catch (Exception ex)
            {
                LogHandler.WriteError(ex);
                return null;
            }

            return dr;
        }
    }
}
