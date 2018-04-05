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
using SPA.Enums;
using SPA.Data;
using SPA.Core;

namespace SPA
{
    public partial class PaymentEntry : Form
    {
        private class CalculatedPayment
        {
            public int AccountID;
            public double AmountToPay;
            public double BaseAmount;
        }
        
        private List<CalculatedPayment> _calculatedPayment;
        private List<CalculatedPayment> CalculatedPayments
        {
            get
            {
                if (_calculatedPayment == null)
                {

                    _calculatedPayment = new List<CalculatedPayment>();
                }
                return _calculatedPayment;
            }
            set
            {
                _calculatedPayment = value;
            }
        }
        private int _periodInMonth;



        public string NoLarian { get; set; }
        public int MemberID { get; set; }
        public string MemberKP { get; set; }
        private FormEnum.EntryMode entryMode;
        private PaymentEntries formPaymentEntries;

        public PaymentEntry(PaymentEntries entriesForm, FormEnum.EntryMode mode)
        {
            this.formPaymentEntries = entriesForm;
            this.entryMode = mode;
            InitializeComponent();
        }
       

        private void btnSave_Click(object sender, EventArgs e)
        {
            double amount21, amount22, amount23;
            if (double.TryParse(txtPinjamanBiasa.Text, out amount21))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = 21, AmountToPay = amount21 });          
            }
            if (double.TryParse(txtPinjamanKhas.Text, out amount22))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = 22, AmountToPay = amount22 });
            }
            if (double.TryParse(txtPinjamanMedi.Text, out amount23))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = 23, AmountToPay = amount23 });
            }

            DateTime startDate = new DateTime(2014,1,1); //new DateTime(int.Parse(this.cmbStartYear.Text), this.cmbStartMonth.SelectedIndex + 1, 1);
         
            //logic here, should be moved to SPA.Entity library
            var payment = new Payment();
            payment.MemberID =  this.MemberID;
            for (int i = 0; i < _periodInMonth; i++)
            {
                foreach (var calculatedPayment in CalculatedPayments)
                {
                    var paymentDetail = new PaymentDetail();
                    paymentDetail.AccountID = calculatedPayment.AccountID;
                    if (paymentDetail.AccountID > 20)
                    {
                        paymentDetail.Amount = calculatedPayment.AmountToPay;
                    }
                    else
                    {
                        paymentDetail.Amount = calculatedPayment.BaseAmount;
                    }
                    DateTime paymentDate = this.GetPaymentDate(startDate, i);
                    paymentDetail.PaymentMonth = paymentDate.Month; 
                    paymentDetail.PaymentYear = paymentDate.Year; 
                    payment.PaymentDetails.Add(paymentDetail);
                }

            }
            if (payment.Create())
            {
                //payment created
                MessageBox.Show(payment.SuccessMessage);                
            }
            else
            {
                MessageBox.Show(payment.SystemErrorMessage);
            }
        }

        private void PaymentEntry_Load(object sender, EventArgs e)
        {
            InitInputProperty();
            RefillMemberInfo();          
        }

        private void InitInputProperty()
        {
            chkPinjamanBiasa.Checked = false;
            chkPinjamanKhas.Checked = false;
            chkPinjamanMedi.Checked = false;

            chkFiMasuk.Checked = false;
            chkYuran.Checked = false;
            chkTabung.Checked = false;
            chkSaham.Checked = false;

            txtFiMasuk.Enabled = false;
            txtYuran.Enabled = false;
            txtTabung.Enabled = false;
            txtSaham.Enabled = false;

            txtPinjamanBiasa.Enabled = false;
            txtPinjamanKhas.Enabled = false;
            txtPinjamanMedi.Enabled = false;
        }

        private void RefillMemberInfo()
        {
            Member member = Cache.MemberCache.GetMember(this.MemberKP);
            if (member != null)
            {
                lblName.Text = member.Name;
                lblMemberCode.Text = member.Code;
                lblMyKad.Text = member.NewIC;
                ChangeLabelByMemberStatus(member.Status);
            }
            else
            {
                lblName.Text = string.Empty;
                lblMemberCode.Text = string.Empty;
                lblMyKad.Text = string.Empty;
                ChangeLabelByMemberStatus(-1);
            }
        }

        private void ChangeLabelByMemberStatus(int status)
        {
            if (status == 1)
            {
                lblStatus.Text = Common.GetMemberStatusDescription(MemberEnum.Status.Active);
                lblStatus.BackColor = Color.Yellow;
            }
            else if (status == 2)
            {
                lblStatus.Text = Common.GetMemberStatusDescription(MemberEnum.Status.NotActive);
                lblStatus.BackColor = Color.Gray;
            }
            else
            {
                lblStatus.Text = string.Empty;
                lblStatus.BackColor = System.Drawing.SystemColors.Control;
            }
        }


        private int GetToMonth()
        {
            return DateTime.Today.Month - 1;
        }

        private int GetFromMonth()
        {
            int fromMonth = DateTime.Today.Month;
            switch (DateTime.Today.Month)
            {
                case 1:
                    fromMonth = 10;
                    break;
                case 2:
                    fromMonth = 11;
                    break;
                case 3:
                    fromMonth = 12;
                    break;
                default:
                    fromMonth = fromMonth - 3;
                    break;
            }
            return fromMonth - 1;
        }

        private int GetToYear()
        {
            return DateTime.Today.Year;

        }

        private int GetFromYear()
        {
            int fromYear;
            switch (DateTime.Today.Month)
            {
                case 1:
                case 2:
                case 3:
                    fromYear = DateTime.Today.Year - 1;
                    break;
                default:
                    fromYear = DateTime.Today.Year;
                    break;
            }
            return fromYear;

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            this.LoadCalculatedPayments();
            txtYuran.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int) Enums.PaymentEnum.AccountID.YuranBulanan).AmountToPay.ToString();
            txtTabung.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa).AmountToPay.ToString();
            txtSaham.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int)Enums.PaymentEnum.AccountID.Saham).AmountToPay.ToString();
            txtFiMasuk.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int)Enums.PaymentEnum.AccountID.FiMasuk).AmountToPay.ToString();
            Assign2DecimalToAmount(txtYuran);
            Assign2DecimalToAmount(txtTabung);
            Assign2DecimalToAmount(txtSaham);
            Assign2DecimalToAmount(txtFiMasuk);
            lblPaymentMessage.Text = "Jumlah pembayaran untuk " + _periodInMonth.ToString() + " bulan:";
        }

        private DateTime GetPaymentDate(DateTime startDate, int i)
        {
            return startDate.AddMonths(i);
        }

        private void LoadCalculatedPayments()
        {
            this._periodInMonth = 0; // get month period
            var acReader = new AccountSettingReader();
            if (acReader.ReadMultiple(new FilterElement() { ColumnName = "IsActive", Value = "1" }))
            {
                List<AccountSetting> accountSettings = acReader.MultipleRecords;
                foreach (var accountSetting in acReader.MultipleRecords)
                {
                    var paymentAmount = new CalculatedPayment();
                    paymentAmount.AccountID = accountSetting.AccountID;
                    paymentAmount.BaseAmount = accountSetting.Amount;
                    if (accountSetting.AccountID == (int)Enums.PaymentEnum.AccountID.Saham)
                    {
                        paymentAmount.AmountToPay = 0.00;
                    }
                    else if (accountSetting.AccountID == (int)Enums.PaymentEnum.AccountID.FiMasuk)
                    {
                        paymentAmount.AmountToPay = paymentAmount.BaseAmount;
                    }
                    else
                    {
                        paymentAmount.AmountToPay = paymentAmount.BaseAmount * _periodInMonth;
                    }

                    CalculatedPayments.Add(paymentAmount);
                }
            }
            else
            {
                MessageBox.Show(acReader.ErrorMessage);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void chkYuran_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkYuran, txtYuran);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkTabung_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkTabung, txtTabung);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkSaham_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkSaham, txtSaham);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkFiMasuk_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkFiMasuk, txtFiMasuk);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkPinjamanBiasa_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanBiasa, txtPinjamanBiasa);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkPinjamanKhas_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanKhas, txtPinjamanKhas);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void chkPinjamanMedi_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanMedi, txtPinjamanMedi);
            RecalculateTotal_OnAmountChanged(sender, e);
        }

        private void InitializeAmountTextBox(TextBox amountTextBox)
        {
            if (string.IsNullOrWhiteSpace(amountTextBox.Text))
            {
                amountTextBox.Text = "0.00";
            }
        }

        private void CheckboxTextboxChanged(CheckBox amountCheckbox, TextBox amountTextbox)
        {
            if (amountCheckbox.Checked == false)
            {
                amountTextbox.Enabled = false;
            }
            else
            {
                amountTextbox.Enabled = true;
            }
            InitializeAmountTextBox(amountTextbox);
        }

        private void RecalculateTotal_OnAmountChanged(object sender, EventArgs e)
        {
            paymentTotal = 0.0;
            CalculateTotal(chkYuran, txtYuran);
            CalculateTotal(chkTabung, txtTabung);
            CalculateTotal(chkSaham, txtSaham);
            CalculateTotal(chkFiMasuk, txtFiMasuk);
            CalculateTotal(chkPinjamanBiasa, txtPinjamanBiasa); 
            CalculateTotal(chkPinjamanKhas, txtPinjamanKhas);
            CalculateTotal(chkPinjamanMedi, txtPinjamanMedi);
        }

        private void InitTotal()
        {
            paymentTotal = 0.0;
        }
      
        private static double paymentTotal = 0.0;
        private void CalculateTotal(CheckBox amountCheckbox, TextBox amountTextBox)
        {
            if (amountCheckbox.Checked)
            {
                paymentTotal += amountTextBox.Text.ToDouble();
            }
            txtTotal.Text = paymentTotal.ToString();
            this.Assign2DecimalToAmount(txtTotal);
            this.Assign2DecimalToAmount(amountTextBox);
        }
        

    }
}
