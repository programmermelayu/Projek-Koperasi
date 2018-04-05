using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.ReportViewer.WinForms;
using Telerik.Reporting;

namespace SPA.Reporting
{
    public class ReportManager
    {
        public ReportViewer reportViewer { get; set; }
        public ReportManager(Telerik.ReportViewer.WinForms.ReportViewer viewer)
        {
            reportViewer = viewer;
        }

        public IReportFilter CustomFilter { get; set; }

        public void ProcessReport(IReportDocument report)
        {
            report.CustomFilter = CustomFilter;
            report.LoadData();
   
            InstanceReportSource reportSource = new InstanceReportSource();
            reportSource.ReportDocument =  (Report)report;

            reportViewer.ReportSource = reportSource;
            reportViewer.RefreshReport();
            
        }


    }
}
