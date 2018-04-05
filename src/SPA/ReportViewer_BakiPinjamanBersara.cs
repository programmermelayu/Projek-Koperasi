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
    public partial class ReportViewer_BakiPinjamanBersara : SPAForm
    {
        public ReportViewer_BakiPinjamanBersara()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            dtSelected.Value = DateTime.Today;
        }

        private IReportFilter GetFilter()
        {
            var reportFilter = new MemberRetiredLoanBalanceData.ReportFilter();
            reportFilter.LoanType  = GetLoanType();
            reportFilter.RetiredAfter = dtSelected.Value;

            return reportFilter;
        }


        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ReportManager manager = new ReportManager(this.reportViewer1);
  
           
            if (GetFilter() != null)
            {
                manager.CustomFilter = GetFilter();
                manager.ProcessReport(new MemberRetiredLoanBalanceReport());
            }
            this.Cursor = Cursors.Default;
        }

        private Enums.PaymentEnum.AccountID GetLoanType()
        {
            Enums.PaymentEnum.AccountID loanType = Enums.PaymentEnum.AccountID.Saham;
            switch (cmbLoanType.SelectedIndex)
            {
                case 0:
                    loanType = Enums.PaymentEnum.AccountID.PinjamanBiasa;
                    break;
                case 1:
                    loanType = Enums.PaymentEnum.AccountID.PinjamanKhas;
                    break;
                case 2:
                    loanType = Enums.PaymentEnum.AccountID.PinjamanMedisihat;
                    break;
                case 3:
                    loanType = Enums.PaymentEnum.AccountID.PinjamanKecemasan;
                    break;
            }
            return loanType;
        }

     
        
    
    }
}
