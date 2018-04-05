using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SPA.Reporting
{
    public interface IDataGenerator
    {
         //List<object> Records { get; set; }
         string GetQuery();
         string GetQueryHeader();
         IReportFilter Filter { get; set; }
         void GenerateReportDetailData(SqlDataReader dr);
         void GenerateReportDetailData();
         void GenerateReportHeaderData(SqlDataReader dr);
         void GenerateReportHeaderData();
    }
    public interface IReportFilter{}
    public interface IReportHeader{}
}
