using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Cache;


namespace SPA
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private Form _activeForm;
        private void btnReg_Click(object sender, EventArgs e)
        {
            _activeForm = new MemberEntries();
            _activeForm.Show();

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            _activeForm = new PaymentEntries();
            _activeForm.Show();

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            _activeForm = new PaymentImportEntries();
            _activeForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _activeForm = new FormErrorLog();
            _activeForm.Show();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            Cache.AccountCache.LoadAccounts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _activeForm = new LedgerByMemberViewer();
            _activeForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _activeForm = new RadForm1();
            _activeForm.Show();
        }
    }
}
