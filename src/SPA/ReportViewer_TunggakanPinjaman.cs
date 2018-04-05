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
    public partial class ReportViewer_TunggakanPinjaman : SPAForm
    {
        public ReportViewer_TunggakanPinjaman()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            dtSelectedMonth.Value = new DateTime(DateTime.Today.Year, 1, 1);
            cmbLoanType.SelectedIndex = 0;
        }

        private IReportFilter GetFilter()
        {
            OutstandingLoanData.ReportFilter reportFilter = new OutstandingLoanData.ReportFilter();
            reportFilter.MemberID = string.IsNullOrEmpty(txtSearchCode.Text.Trim()) ? -1 : MemberCache.GetMemberID(txtSearchCode.Text.Trim());
            reportFilter.StartMonth = dtSelectedMonth.Value;
            reportFilter.AccountId = (int)GetLoanType();

            if (reportFilter.MemberID == -1)
            {
                //DialogResult result = MessageBox.Show("Menjana laporan tunggakan pinjaman untuk semua anggota akan mengambil masa beberapa minit. Klik OK untuk teruskan?",
                //               "Laporan Tunggakan Semua Anggota", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //if (result == DialogResult.Cancel) return null;
            }


            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ReportManager manager = new ReportManager(this.reportViewer1);
            IReportFilter filter = GetFilter();
            if (filter != null)
            {
                manager.CustomFilter = filter;
                manager.ProcessReport(new OutstandingLoanReport());
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
