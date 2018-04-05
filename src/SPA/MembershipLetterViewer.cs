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
using SPA.Cache;
using Telerik.Reporting;

namespace SPA
{
    public partial class MembershipLetterViewer : Form
    {
        public int MemberID { get; set; }
        public MembershipLetterViewer()
        {
            InitializeComponent();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void MembershipLetterViewer_Load(object sender, EventArgs e)
        {
            ReportManager manager = new ReportManager(this.reportViewer1);
            manager.CustomFilter = GetFilter();
            manager.ProcessReport(new MembershipLetterReport());
        }

        private IReportFilter GetFilter()
        {
            MembershipLetterData.ReportFilter reportFilter = new MembershipLetterData.ReportFilter();
            reportFilter.MemberID = this.MemberID;
            return reportFilter;
        }
    }
}
