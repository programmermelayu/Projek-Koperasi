using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Core;

namespace SPA.Data
{
    public class DbInserter 
    {
        public string SystemErrorMessage { get; set; }
        public bool Execute(String sql)
        {
            var spaConnection = new SPA.Data.SPAConnection().Connection;
            try
            {
                if (spaConnection.State != System.Data.ConnectionState.Open)
                {
                    spaConnection.Open();  
                }
                
                var insertCommand = new SqlCommand(sql, spaConnection);
                insertCommand.ExecuteNonQuery();

                if (spaConnection.State != System.Data.ConnectionState.Closed)
                {
                    spaConnection.Close();
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteError(ex);
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


        public bool  Execute(List<string> sqlCollection)
        {
            using (SqlConnection spaConnection = new SPA.Data.SPAConnection().Connection) 
            {
                if (spaConnection.State != System.Data.ConnectionState.Open)
                {
                    spaConnection.Open();
                }

                SqlCommand command = spaConnection.CreateCommand();
                SqlTransaction spaTransaction;

                // Start a local transaction.
                spaTransaction = spaConnection.BeginTransaction("SpaTransaction");

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = spaConnection;
                command.Transaction = spaTransaction;
                try
                {
                    foreach (var sql in sqlCollection)
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        
                    }
                    spaTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHandler.WriteError(ex);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        spaTransaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                       SystemErrorMessage = string.Format("Rollback Exception Type: {0}", ex2.GetType());
                       SystemErrorMessage += string.Format(" Message: {0}", ex2.Message);
                       LogHandler.WriteError(ex);
                    }
                    return false;
                }
            }
        }
    
    
    }
}
