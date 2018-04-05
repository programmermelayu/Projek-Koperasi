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

namespace SPA
{
    public partial class PaymentEntryEdit : Form
    {
        public int PaymentID { get; set; }
        public int MemberID { get; set; }
        public int AccountID { get; set; }
        public string AccountDescription { get; set; }
        public double Amount { get; set; }
        public double Interest { get; set; }

        private PaymentEntries formEntries; 
        public PaymentEntryEdit(PaymentEntries entriesForm)
        {
            formEntries = entriesForm;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.ID = PaymentID;
            payment.MemberID = MemberID;

            PaymentDetail detail = new PaymentDetail();
            detail.AccountID = AccountID;
            detail.Amount = txtAmount.Text.ToDouble();
            detail.Interest = txtInterest.Text.ToDouble();
            payment.PaymentDetails.Add(detail);
            if (payment.Create())
            {
                MessageBox.Show(payment.SuccessMessage, "Berjaya", MessageBoxButtons.OK, MessageBoxIcon.Information);
                formEntries.RefillDatagridView();
            }
            else
            {
                MessageBox.Show(payment.ErrorMessage, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
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

        private void PaymentEntryEdit_Load(object sender, EventArgs e)
        {
            radGroupBox1.HeaderText = AccountDescription;
            txtAmount.Text = Assign2DecimalToAmount(Amount.ToString());
            txtInterest.Text = Assign2DecimalToAmount(Interest.ToString());

            if (AccountID > (int) Enums.PaymentEnum.AccountID.Pinjaman)
            {
                txtInterest.Visible = true;
                lblInterest.Visible = true;
            }
        }

        private string Assign2DecimalToAmount(string amount)
        {
            if (amount.IndexOf(".") < 0)
            {
                amount += ".00";
            }
            return amount;
        }
      
    }
}
