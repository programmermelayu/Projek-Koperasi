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
    public partial class ReportViewer_Pembayaran_ByState : SPAForm
    {
        public ReportViewer_Pembayaran_ByState()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            foreach (var state in Cache.SettingsCache.States)
            {
                this.cmbState.Items.Add(state.Description);
            }
        }

        private IReportFilter GetFilter()
        {
       
            PaidMemberByStateData.ReportFilter reportFilter = new PaidMemberByStateData.ReportFilter();

            reportFilter.StateID = SPA.Cache.SettingsCache.GetID(SPA.Enums.SettingEnum.Setting.State, cmbState.SelectedItem.ToString());

            if (cmbStatus.SelectedIndex > 0)
            {
                reportFilter.StatusID = cmbStatus.SelectedIndex == 1 ? 1 : cmbStatus.SelectedIndex;
            }
            return reportFilter;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ReportManager manager = new ReportManager(this.reportViewer1);
            IReportFilter reportFilter = GetFilter();
            if (reportFilter != null)
            {
                manager.CustomFilter = reportFilter;
                manager.ProcessReport(new PaidMemberByStateReport());                
            }
            this.Cursor = Cursors.Default;
        }

     
        
    
    }
}
