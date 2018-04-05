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
    public partial class ReportViewer_Pembayaran_SKM : SPAForm
    {
        public ReportViewer_Pembayaran_SKM()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            this.btnShow_Click(sender, e);        
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ReportManager manager = new ReportManager(this.reportViewer1);
            manager.ProcessReport(new PaidMemberSKMReport());
            this.Cursor = Cursors.Default;
        }

     
        
    
    }
}
