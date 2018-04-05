namespace SPA.Reporting
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ReportLedger1.
    /// </summary>
    public partial class LoanLedgerByMemberReport2 : Telerik.Reporting.Report, IReportDocument
    {

        public IReportFilter CustomFilter { get; set; }
        public LoanLedgerByMemberReport2()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        public void LoadData()
        {
            DataGenerator generator = new DataGenerator(new LoanLedgerByMemberData());
            generator.ReportFilter = this.CustomFilter;
            generator.GenerateData();
        }


    }


}