using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SPA.Core
{
    public static class DataHandler
    {
        public static Boolean HasRecord(this SqlDataReader dr)
        {
            return (dr != null && dr.HasRows);
        }

        public static string GetValueString(this SqlDataReader dr, string columnName)
        {
            try
            {
                if (string.IsNullOrEmpty(dr.GetValue(dr.GetOrdinal(columnName)).ToString()))
                {
                    return string.Empty;
                }
                else
                {
                    return dr.GetValue(dr.GetOrdinal(columnName)).ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static DateTime GetValueDate(this SqlDataReader dr, string columnName)
        {
            if (string.IsNullOrEmpty(dr.GetValue(dr.GetOrdinal(columnName)).ToString()))
            {
                return new DateTime(2999, 1, 1);
            }
            else
            {
                DateTime expectedDate;
                if (DateTime.TryParse(dr.GetValue(dr.GetOrdinal(columnName)).ToString(), out expectedDate) == false)
                {
                    return new DateTime(2999, 1, 1);
                }
                return DateTime.Parse(expectedDate.ToString().ToDateMalayFormatted(false));
            }
        }
        
        public static int GetValueInt(this SqlDataReader dr, string columnName)
        {
            if (string.IsNullOrEmpty(dr.GetValue(dr.GetOrdinal(columnName)).ToString()))
            {
                return 0;
            }
            else
            {
                return dr.GetValue(dr.GetOrdinal(columnName)).ToString().ToInt();
            }
        }

        public static double GetValueDouble(this SqlDataReader dr, string columnName)
        {
            if (string.IsNullOrEmpty(dr.GetValue(dr.GetOrdinal(columnName)).ToString()))
            {
                return 0.0;
            }else
            {
                return dr.GetValue(dr.GetOrdinal(columnName)).ToString().ToDouble();
            }
        }

        public static Boolean GetValueBoolean(this SqlDataReader dr, string columnName)
        {
            string value = dr.GetValue(dr.GetOrdinal(columnName)).ToString();
            if (value=="0" || value=="1" || value.ToLower() == "true" || value.ToLower() == "false")
            {
                return dr.GetBoolean(dr.GetOrdinal(columnName));                
            }
            else
            {
                return false;
            }
        }

    }
}
