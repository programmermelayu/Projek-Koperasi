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
    public partial class ReportViewer_LejerPembayaran : SPAForm
    {
        public ReportViewer_LejerPembayaran()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            dtFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtTo.Value = new DateTime(DateTime.Today.Year, 12, 31);
            dtSelectedYear.Value = new DateTime(DateTime.Today.Year, 1, 1);

            if (EnvironmentCache.IsDebug())
            {
                txtSearchCode.Text = "00547";
                dtSelectedYear.Value = new DateTime(DateTime.Today.Year + 1, 1, 1);
            }
        }

        private double FiMasuk, TabungKebajikan, YuranBulanan, Saham, SimpananKhas;
        private IReportFilter GetFilter()
        {
            LedgerByMemberData.ReportFilter reportFilter = new LedgerByMemberData.ReportFilter();
            reportFilter.MemberID = MemberCache.GetMemberID(txtSearchCode.Text.Trim());

            if (reportFilter.MemberID == -1)
            {
                MessageBox.Show("No Anggota atau No MyKad tidak dijumpai", "Ralat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
                  
            reportFilter.SelectedYear = dtSelectedYear.Value.Year;
            reportFilter.PreviousTotalFiMasuk = this.FiMasuk;
            reportFilter.PreviousTotalSaham = this.Saham;
            reportFilter.PreviousTotalSimpananKhas = this.SimpananKhas;
            reportFilter.PreviousTotalTabungDerma = this.TabungKebajikan;
            reportFilter.PreviousTotalYuranBulanan = this.YuranBulanan;
            //reportFilter.PeriodFrom = string.Format("{0:yyyyMM}", dtFrom.Value).ToInt();
            //reportFilter.PeriodTo = string.Format("{0:yyyyMM}", dtTo.Value).ToInt();

            //if (reportFilter.PeriodFrom > reportFilter.PeriodTo)
            //{ 
            //    MessageBox.Show("Carian tarikh dalam tempoh ini dibenarkan.", "Ralat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}

            //reportFilter.StartMonth = dtFrom.Value;
            //reportFilter.EndMonth = dtTo.Value;

      

            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ClearPreviousYearTotal();
            if (dtSelectedYear.Value.Year < 2016)
            {
                LoadPreviousYearTotal();
            }

            ReportManager manager = new ReportManager(this.reportViewer1);
            if (GetFilter()!= null)
            {
                manager.CustomFilter = GetFilter();
                manager.ProcessReport(new LedgerByMemberReport2());                
            }
        }

        private void btnAddBalance_Click(object sender, EventArgs e)
        {
            Input_ReportViewer_LejerPembayaran entry = new Input_ReportViewer_LejerPembayaran(this);
            entry.MemberID = MemberCache.GetMemberID(txtSearchCode.Text.Trim());
            if (entry.MemberID ==-1)
            {
                ShowMessage("No Anggota atau No MyKad tidak wujud.", MessageFor.Error);
                return;                
            }
            entry.ShowDialog();
        }

        public void LoadPreviousYearTotal()
        {
            PaymentTotalReader totalReader = new Entity.PaymentTotalReader();
            FilterElement filter = new Entity.FilterElement();
            filter.Key= Enums.Data.KeyElements.MemberID;
            filter.Value= MemberCache.GetMemberID(txtSearchCode.Text.Trim());
            if (totalReader.ReadSingle(filter))
            {
                this.FiMasuk = totalReader.SingleRecord.FiMasuk;
                this.YuranBulanan  = totalReader.SingleRecord.YuranBulanan;
                this.TabungKebajikan= totalReader.SingleRecord.TabungDerma;
                this.Saham = totalReader.SingleRecord.Saham;
                this.SimpananKhas = totalReader.SingleRecord.SimpananKhas;                
            }
            
        }

        private void ClearPreviousYearTotal()
        {
            FiMasuk = 0;
            YuranBulanan = 0;
            TabungKebajikan = 0;
            Saham = 0;
            SimpananKhas = 0;
        }

    }
}
