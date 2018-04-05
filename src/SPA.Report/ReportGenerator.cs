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
    public class ReportGenerator<T>
    {
        public IReporter<T> Reporter { get; set; }
        public ReportGenerator(IReporter<T> reporter)
        {
            Reporter = reporter;
        }

        public List<T> Records { get; set; }

        private string _query;
        public string Query
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    _query = Reporter.GetQuery();
                }
                return _query;
            }
            set
            {
                _query = value;
            }
        }
        public void CreateReport()
        {
            SqlDataReader dr = DbReader.Execute(this.Query);
            Reporter.LoadReportData(dr, Records);
        }

    }
}
