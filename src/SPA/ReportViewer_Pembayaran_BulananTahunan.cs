using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Reporting;
using SPA.Entity;
using Telerik.Reporting;
using SPA.Cache;
using SPA.Core;
using SPA.Control;
using SPA.Enums;

namespace SPA
{
    public partial class ReportViewer_Pembayaran_BulananTahunan: SPAForm
    {
        public ReportViewer_Pembayaran_BulananTahunan()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            this.rdoMonthly.Checked = true;
            this.dtSelected.Value = DateTime.Now.AddMonths(-1);     
        }

        private IReportFilter GetFilter()
        {
            PaidMemberMonthYearData.ReportFilter reportFilter = new PaidMemberMonthYearData.ReportFilter();
            reportFilter.IsYearly = rdoYearly.Checked;
            reportFilter.SelectedMonth = dtSelected.Value;
   
            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ReportManager manager = new ReportManager(this.reportViewer1);
            if (GetFilter() != null)
            {
                manager.CustomFilter = GetFilter();
                manager.ProcessReport(new PaidMemberMonthYearReport());
            }
            this.Cursor = Cursors.Default;
        }

        private void rdoMonthly_CheckedChanged(object sender, EventArgs e)
        {
            dtSelected.CustomFormat = "MMM - yyyy";
        }

        private void rdoYearly_CheckedChanged(object sender, EventArgs e)
        {
            dtSelected.CustomFormat = "yyyy";
        }

     
        
    
    }
}
