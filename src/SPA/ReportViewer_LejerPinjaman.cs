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
    public partial class ReportViewer_LejerPinjaman : SPAForm
    {
        private double Base { get; set; }
        private double Interest { get; set; }
        private double BaseBalance { get; set; }
        private double InterestBalance { get; set; }
        private DateTime LoanDate { get; set; }
        private DateTime LoanStartDate { get; set; }
        private DateTime LoanEndDate { get; set; }
        private int LoanDuration { get; set; }

        public ReportViewer_LejerPinjaman()
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
                txtSearchCode.Text = "00125";
                cmbLoanType.SelectedIndex = 1;
            }
        }

        private IReportFilter GetFilter()
        {
            LoanLedgerByMemberData.ReportFilter reportFilter = new LoanLedgerByMemberData.ReportFilter();
            reportFilter.MemberID = MemberCache.GetMemberID(txtSearchCode.Text.Trim());

            if (reportFilter.MemberID == -1)
            {
                ShowMessage("No Anggota atau No MyKad tidak wujud.", MessageFor.Error);
                return null;
            }
            else if (GetLoanType() == Enums.PaymentEnum.AccountID.Saham)
            {
                ShowMessage("Sila pilih lejer pinjaman yang betul.", MessageFor.Error);
                return null;
            }
                  
            reportFilter.SelectedYear = dtSelectedYear.Value.Year;
            reportFilter.Base = this.Base;
            reportFilter.Interest = this.Interest;
            reportFilter.BaseBalance = this.BaseBalance;
            reportFilter.InterestBalance = this.InterestBalance;
            reportFilter.LoanType = GetLoanType();
       
            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ClearLoadLoanSetting();
            if (dtSelectedYear.Value.Year < 2016)
            {
                this.LoadLoanSetting();
            }

            ReportManager manager = new ReportManager(this.reportViewer1);
            IReportFilter reportFilter = GetFilter();
            if (reportFilter != null)
            {
                manager.CustomFilter = reportFilter;
                manager.ProcessReport(new LoanLedgerByMemberReport2());                
            }
        }

        private void btnUpdateLoan_Click(object sender, EventArgs e)
        {
            Input_ReportViewer_LejerPinjaman entry = new Input_ReportViewer_LejerPinjaman(this);
            entry.MemberID = MemberCache.GetMemberID(txtSearchCode.Text.Trim());
            entry.LoanType = this.GetLoanType();
            if (entry.MemberID == -1)
            {
                ShowMessage("No Anggota atau No MyKad tidak wujud.", MessageFor.Error);
                return;
            }
            else if(entry.LoanType == Enums.PaymentEnum.AccountID.Saham)
            {
                ShowMessage("Sila pilih lejer pinjaman yang betul.", MessageFor.Error);
                return;
            }
            entry.ShowDialog();
        }
        
        public void LoadLoanSetting()
        {

            MemberLoanReader reader = new Entity.MemberLoanReader();
            List<FilterElement> filterCollection = new List<FilterElement>();
            FilterElement filterMemberID = new Entity.FilterElement();
            filterMemberID.Key = Enums.Data.KeyElements.MemberID;
            filterMemberID.Value = MemberCache.GetMemberID(txtSearchCode.Text.Trim());

            FilterElement filterLoanType = new Entity.FilterElement();
            filterLoanType.ColumnName = "TypeID";
            filterLoanType.Value = (int)GetLoanType();

            filterCollection.Add(filterMemberID);
            filterCollection.Add(filterLoanType);

            if (reader.ReadSingle(filterCollection))
            {
                this.Base = reader.SingleRecord.Base;
                this.Interest = reader.SingleRecord.Interest;
                this.BaseBalance = reader.SingleRecord.BaseBalance;
                this.InterestBalance = reader.SingleRecord.InterestBalance;
                this.LoanDuration = reader.SingleRecord.LoanDuration;
                this.LoanDate =  DateTime.Parse(reader.SingleRecord.LoanDate.ToDateMalayFormatted(false));
                this.LoanStartDate = DateTime.Parse(reader.SingleRecord.LoanStartDate.ToDateMalayFormatted(false));
                this.LoanEndDate = DateTime.Parse(reader.SingleRecord.LoanEndDate.ToDateMalayFormatted(false));            
            }
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

        private void ClearLoadLoanSetting()
        {
            this.Base = 0;
            this.Interest = 0;
            this.BaseBalance = 0;
            this.InterestBalance = 0;
        }

    }
}
