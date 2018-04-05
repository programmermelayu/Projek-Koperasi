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

namespace SPA
{
    public partial class ReportViewer_Pembayaran : SPAForm
    {
        private PaidMemberData.MemberPaymentCategory reportCategory;
        public ReportViewer_Pembayaran(PaidMemberData.MemberPaymentCategory category)
        {
            InitializeComponent();
            reportCategory = category;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            dtFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtTo.Value = new DateTime(DateTime.Today.Year, 12, 31);
        }

        private IReportFilter GetFilter()
        {
            PaidMemberData.ReportFilter reportFilter = new PaidMemberData.ReportFilter();
       
            reportFilter.PeriodFrom = string.Format("{0:yyyyMM}", dtFrom.Value).ToInt();
            reportFilter.PeriodTo = string.Format("{0:yyyyMM}", dtTo.Value).ToInt();

            if (reportFilter.PeriodFrom > reportFilter.PeriodTo)
            { 
                MessageBox.Show("Carian tarikh dalam tempoh ini tidak dibenarkan.", "Ralat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            reportFilter.StartMonth = dtFrom.Value;
            reportFilter.EndMonth = dtTo.Value;
            reportFilter.Category = reportCategory;
       
            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ReportManager manager = new ReportManager(this.reportViewer1);
            IReportFilter reportFilter = GetFilter();
            if (reportFilter != null)
            {
                manager.CustomFilter = reportFilter;
                manager.ProcessReport(new PaidMemberReport());                
            }
        }

     
        
    
    }
}
