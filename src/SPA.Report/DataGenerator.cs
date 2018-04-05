using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Data;
using SPA.Core;

namespace SPA.Reporting
{
    public class DataGenerator 
    {
        public IDataGenerator ReportDataGenerator { get; set; }

        private string _query;
        public string Query
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    _query = ReportDataGenerator.GetQuery();
                }
                return _query;
            }
            set
            {
                _query = value;
            }
        }

        private string _queryHeader;
        public string QueryHeader
        {
            get
            {
                if (string.IsNullOrEmpty(_queryHeader))
                {
                    _queryHeader = ReportDataGenerator.GetQueryHeader();
                }
                return _queryHeader;
            }
            set
            {
                _queryHeader = value;
            }
        }

        public DataGenerator(IDataGenerator reporter)
        {
            ReportDataGenerator = reporter;
        }

        public IReportFilter ReportFilter { get; set; }

        public void GenerateData()
        {
            ReportDataGenerator.Filter = this.ReportFilter;
            
            SqlDataReader dr = null;

            if (!string.IsNullOrEmpty(this.Query))
            {
                dr = DbReader.Execute(this.Query);
                if (dr.HasRecord())
                {
                    ReportDataGenerator.GenerateReportDetailData(dr);
                }
            }
            else
            {
                ReportDataGenerator.GenerateReportDetailData();
            }

            if (!string.IsNullOrEmpty(this.QueryHeader))
            {
                dr = DbReader.Execute(this.QueryHeader);
                if (dr.HasRecord())
                {
                    ReportDataGenerator.GenerateReportHeaderData(dr);
                } 
            }
            else
            {
                ReportDataGenerator.GenerateReportHeaderData();
            }
            
        }

    }
}
