using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Entity;
using SPA.Core;
using SPA.Cache;

namespace SPA
{
    public partial class Input_ReportViewer_LejerPembayaran : Form 
    {
        public int MemberID { get; set; }

        private int paymentTotalId = 0; 
        ReportViewer_LejerPembayaran baseViewer;
        public Input_ReportViewer_LejerPembayaran(ReportViewer_LejerPembayaran viewer)
        {
            InitializeComponent();
            baseViewer = viewer;
        }

        private void LedgerByMemberBalanceLastYear_Load(object sender, EventArgs e)
        {
            LoadPreviousYearTotal();

            Assign2DecimalToAmount(txtYuranBulanan);
            Assign2DecimalToAmount(txtTabung);
            Assign2DecimalToAmount(txtSaham);
            Assign2DecimalToAmount(txtFiMasuk);
            Assign2DecimalToAmount(txtSimpananKhas);
        }

        private void Assign2DecimalToAmount(TextBox amountTextbox)
        {
            if (amountTextbox.Text.IndexOf(".") < 0)
            {
                amountTextbox.Text += ".00";
            }
            else if (amountTextbox.Text.ToDouble() == 0.0)
            {
                amountTextbox.Text = "0.00";
            }
        }
        private void NumericalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf(".") > -1))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var total = new PaymentTotal();
            total.ID = this.paymentTotalId;
            total.MemberID = this.MemberID;
            total.FiMasuk = txtFiMasuk.Text.ToDouble();
            total.YuranBulanan = txtYuranBulanan.Text.ToDouble();
            total.TabungDerma = txtTabung.Text.ToDouble();
            total.Saham = txtSaham.Text.ToDouble();
            total.SimpananKhas = txtSimpananKhas.Text.ToDouble();
            if (total.Create())
            {
                baseViewer.LoadPreviousYearTotal();
                Close();
            }
        }

        public void LoadPreviousYearTotal()
        {
            PaymentTotalReader totalReader = new Entity.PaymentTotalReader();
            FilterElement filter = new Entity.FilterElement();
            filter.Key = Enums.Data.KeyElements.MemberID;
            filter.Value = MemberID;
            if (totalReader.ReadSingle(filter))
            {
                paymentTotalId = totalReader.SingleRecord.ID;
                txtFiMasuk.Text = totalReader.SingleRecord.FiMasuk.ToString();
                txtYuranBulanan.Text  = totalReader.SingleRecord.YuranBulanan.ToString();
                txtTabung.Text  = totalReader.SingleRecord.TabungDerma.ToString();
                txtSaham.Text = totalReader.SingleRecord.Saham.ToString();
                txtSimpananKhas.Text = totalReader.SingleRecord.SimpananKhas.ToString();
            }

        }
    }
}

