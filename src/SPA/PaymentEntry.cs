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
using SPA.Cache;
using SPA.Core;
using SPA.Control;

namespace SPA
{
    public partial class PaymentEntry : SPAForm 
    {
        private class CalculatedPayment
        {
            public int AccountID;
            public double AmountToPay;
            //public double BaseAmount;
            public double Interest;
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
        private int periodInMonth;
        public string NoLarian { get; set; }
        public string MemberKP { get; set; }

        private string paymentMessage = "Jumlah penerimaan untuk {0} bulan:";

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
            if ((ValidateEntries())== false) return;
            this.AddCalculatedPayment();
            var payment = new Payment();
            payment.MemberID = Cache.MemberCache.GetMemberID(this.MemberKP);
            payment.NoLarian = txtNoResit.Text.Trim();
            payment.NoBaucer = txtNoBaucer.Text.Trim();
            payment.NoResit = txtNoResit.Text.Trim();
            
            for (int i = 0; i < periodInMonth; i++)
            {
                CreateMonthlyPayment(payment, i);
            }

            if (payment.PaymentDetails.Count < 1)
            {
                MessageBox.Show("Maklumat tidak disimpan. ", "Amaran", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (payment.Create())
            {
                NotifyExistingPayment(payment.ExistingPayments);
                if (payment.ExistingPayments.Count < 1) MessageBox.Show(payment.SuccessMessage);
                this.formPaymentEntries.RefillDatagridView();

                var member = new Member() { ID = payment.MemberID };
                member.UpdateStatus();
               
                this.Close();
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

        private void NotifyExistingPayment(List<Payment.PaymentRecord> existingPayments)
        {
            if (existingPayments.Count > 0)
            {
                foreach (var existingPayment in existingPayments)
                {
                    var accountName = AccountCache.GetAccountDescription(existingPayment.AccountID);
                    var paymentMonth = existingPayment.PaymentMonth;
                    var paymentYear = existingPayment.PaymentYear;
                    ShowMessage("Rekod bayaran untuk " + accountName + " bagi bulan " + paymentMonth + " tahun " + paymentYear +
                       " telah pun wujud. Sila kemaskini rekod ini jika perlu.", MessageFor.Instruction);
                }

            }
        }

        private void AddCalculatedPayment()
        {
            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.Saham))
            {
                CalculatedPayments.Add(new CalculatedPayment()
                {
                    AccountID = (int)Enums.PaymentEnum.AccountID.Saham, AmountToPay = txtSaham.Text.ToDouble()});
            }

            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.SimpananKhas))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = (int)Enums.PaymentEnum.AccountID.SimpananKhas, 
                    AmountToPay = txtSimpananKhas.Text.ToDouble()});  
            }

            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.PinjamanBiasa))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = (int)Enums.PaymentEnum.AccountID.PinjamanBiasa, 
                    AmountToPay = txtPinjamanBiasa.Text.ToDouble(), Interest = txtFaedahPinjamanBiasa.Text.ToDouble()});     
            }
            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.PinjamanKhas))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = (int)Enums.PaymentEnum.AccountID.PinjamanKhas, 
                    AmountToPay = txtPinjamanKhas.Text.ToDouble(), Interest = txtFaedahPinjamanKhas.Text.ToDouble()});
            }
            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.PinjamanMedisihat))
            {
                CalculatedPayments.Add(new CalculatedPayment() { AccountID = (int)Enums.PaymentEnum.AccountID.PinjamanMedisihat, 
                    AmountToPay = txtPinjamanMedi.Text.ToDouble(),Interest = txtFaedahPinjamanMedi.Text.ToDouble() });
            }
            if (IsSelectedAccount((int)Enums.PaymentEnum.AccountID.PinjamanKecemasan))
            {
                CalculatedPayments.Add(new CalculatedPayment()
                {
                    AccountID = (int)Enums.PaymentEnum.AccountID.PinjamanKecemasan,
                    AmountToPay = txtPinjamanKecemasan.Text.ToDouble(),
                    Interest = txtFaedahPinjamanKecemasan.Text.ToDouble()
                });
            }
        }

        private void LoadCalculatedPayments()
        {
            CalculatedPayments = new List<CalculatedPayment>();

            var yuranBulanan = new CalculatedPayment();
            yuranBulanan.AccountID = (int)PaymentEnum.AccountID.YuranBulanan;
            yuranBulanan.AmountToPay = txtYuran.Text.ToDouble();

            var tabung = new CalculatedPayment();
            tabung.AccountID = (int)PaymentEnum.AccountID.KebajikanDermasiswa;
            tabung.AmountToPay = txtTabung.Text.ToDouble();

            var fiMasuk = new CalculatedPayment();
            fiMasuk.AccountID = (int)PaymentEnum.AccountID.FiMasuk;
            fiMasuk.AmountToPay = txtFiMasuk.Text.ToDouble();

            CalculatedPayments.Add(yuranBulanan);
            CalculatedPayments.Add(tabung);
            CalculatedPayments.Add(fiMasuk);

        }

        private void CreateMonthlyPayment(Payment payment, int index)
        {
            foreach (var calculatedPayment in CalculatedPayments)
            {
                if (IsSelectedAccount(calculatedPayment.AccountID) == false) continue;
              
                var paymentDetail = new PaymentDetail();
                paymentDetail.AccountID = calculatedPayment.AccountID;
                paymentDetail.Amount = GetCalculatedAmount(calculatedPayment, index);
                paymentDetail.Interest = calculatedPayment.Interest / periodInMonth;
                if (paymentDetail.Amount == 0.0) continue;                 
                DateTime paymentDate = this.radDateTimePicker1.Value.AddMonths(index);
                paymentDetail.PaymentMonth = paymentDate.Month;
                paymentDetail.PaymentYear = paymentDate.Year;
                payment.PaymentDetails.Add(paymentDetail);
            }
        }
  
        private double GetCalculatedAmount(CalculatedPayment calculatedPayment, int index)
        {
            double amount = 0.0;
            switch (calculatedPayment.AccountID)
            {
                case (int)PaymentEnum.AccountID.FiMasuk:
                    if (index == 0) amount = calculatedPayment.AmountToPay;
                    break;
                case (int)PaymentEnum.AccountID.Saham:
                    if (index == 0) amount = txtSaham.Text.ToDouble();
                    break;
                case (int)PaymentEnum.AccountID.KebajikanDermasiswa:
                case (int)PaymentEnum.AccountID.YuranBulanan:
                    amount = AccountCache.GetBaseAmount(calculatedPayment.AccountID);
                    break;
                case (int)PaymentEnum.AccountID.PinjamanBiasa:
                case (int)PaymentEnum.AccountID.PinjamanKhas:
                case (int)PaymentEnum.AccountID.PinjamanMedisihat:
                case (int)PaymentEnum.AccountID.PinjamanKecemasan:
                    amount = calculatedPayment.AmountToPay / this.periodInMonth;
                    break;     
                case (int)PaymentEnum.AccountID.SimpananKhas:
                    amount = calculatedPayment.AmountToPay / this.periodInMonth;
                    break;
                default:
                    amount = 0.0;
                    break;
                }
            return amount;
        }

        private bool IsSelectedAccount(int accountID)
        {
            bool isSelected = false;
            switch (accountID)
            {
                case (int)PaymentEnum.AccountID.FiMasuk:
                    isSelected = chkFiMasuk.Checked;
                    break;
                case (int)PaymentEnum.AccountID.KebajikanDermasiswa:
                    isSelected = chkTabung.Checked;
                    break;
                case (int)PaymentEnum.AccountID.PinjamanBiasa:
                    isSelected = chkPinjamanBiasa.Checked;
                    break;
                case (int)PaymentEnum.AccountID.PinjamanKhas:
                    isSelected = chkPinjamanKhas.Checked;
                    break;
                case (int)PaymentEnum.AccountID.PinjamanMedisihat:
                    isSelected = chkPinjamanMedi.Checked;
                    break;
                case (int)PaymentEnum.AccountID.PinjamanKecemasan:
                    isSelected = chkPinjamanKecemasan.Checked;
                    break;
                case (int)PaymentEnum.AccountID.Saham:
                    isSelected = chkSaham.Checked;
                    break;
                case (int)PaymentEnum.AccountID.YuranBulanan:
                    isSelected = chkYuran.Checked;
                    break;
                case (int)PaymentEnum.AccountID.SimpananKhas:
                    isSelected = chkSimpananKhas.Checked;
                    break;
                default:
                    isSelected = false;
                    break;
            }
            return isSelected;
        }

        private void InitInputProperty()
        {
            chkPinjamanBiasa.Checked = false;
            chkPinjamanKhas.Checked = false;
            chkPinjamanMedi.Checked = false;
            chkPinjamanKecemasan.Checked = false;

            chkFiMasuk.Checked = false;
            chkYuran.Checked = false;
            chkTabung.Checked = false;
            chkSaham.Checked = false;
            chkSimpananKhas.Checked = false;

            txtFiMasuk.Enabled = false;
            txtYuran.Enabled = false;
            txtTabung.Enabled = false;
            txtSaham.Enabled = false;
            txtSimpananKhas.Enabled = false;

            txtPinjamanBiasa.Enabled = false;
            txtPinjamanKhas.Enabled = false;
            txtPinjamanMedi.Enabled = false;
            txtPinjamanKecemasan.Enabled = false;

            this.radDateTimePicker1.Value = DateTime.Today;
            this.radDateTimePicker2.Value = DateTime.Today;
        }

        private void RefillMemberInfo()
        {
            Member member = Cache.MemberCache.GetMember(this.MemberKP);
            if (member != null)
            {
                lblName.Text = member.Name;
                lblMemberCode.Text = member.Code;
                lblMyKad.Text = member.NewIC;
                ChangeLabelByMemberStatus(member.StatusID);
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

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            this.LoadCalculatedPayments();
            txtYuran.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int) Enums.PaymentEnum.AccountID.YuranBulanan).AmountToPay.ToString();
            txtTabung.Text = this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa).AmountToPay.ToString();
            txtFiMasuk.Text = AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.FiMasuk).ToString(); //this.CalculatedPayments.FirstOrDefault(x => x.AccountID == (int)Enums.PaymentEnum.AccountID.FiMasuk).AmountToPay.ToString();

            Assign2DecimalToAmount(txtYuran);
            Assign2DecimalToAmount(txtTabung);
            Assign2DecimalToAmount(txtSaham);
            Assign2DecimalToAmount(txtFiMasuk);
            Assign2DecimalToAmount(txtSimpananKhas);
            lblPaymentMessage.Text = string.Format(paymentMessage, periodInMonth.ToString());
        }

        private DateTime GetPaymentDate(DateTime startDate, int i)
        {
            return startDate.AddMonths(i);
        }

        private int GetPeriod()
        {   
            DateTime dateFrom = radDateTimePicker1.Value;
            DateTime dateTo = radDateTimePicker2.Value;
            int period =  ((dateTo.Year - dateFrom.Year) * 12) + dateTo.Month - dateFrom.Month + 1;
            return (period < 0) ? 0 : period;
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
            RecalculateTotal_OnAmountChanged();
        }

        private void chkTabung_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkTabung, txtTabung);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkSaham_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkSaham, txtSaham);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkSimpananKhas_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkSimpananKhas, txtSimpananKhas);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkFiMasuk_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkFiMasuk, txtFiMasuk);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkPinjamanBiasa_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanBiasa, txtPinjamanBiasa, txtFaedahPinjamanBiasa);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkPinjamanKhas_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanKhas, txtPinjamanKhas, txtFaedahPinjamanKhas);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkPinjamanMedi_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanMedi, txtPinjamanMedi, txtFaedahPinjamanMedi);
            RecalculateTotal_OnAmountChanged();
        }

        private void chkPinjamanKecemasan_CheckedChanged(object sender, EventArgs e)
        {
            CheckboxTextboxChanged(chkPinjamanKecemasan, txtPinjamanKecemasan, txtFaedahPinjamanKecemasan);
            RecalculateTotal_OnAmountChanged();
        }

        private void InitializeAmountTextBox(TextBox amountTextBox, CheckBox amountCheckbox)
        {
            if (string.IsNullOrWhiteSpace(amountTextBox.Text))
            {
                amountTextBox.Text = "0.00";
            }

            this.periodInMonth = GetPeriod();
            switch (amountTextBox.Name)
            {
                case "txtYuran":
                    if (amountCheckbox.Checked)
                    {
                        amountTextBox.Text = (AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.YuranBulanan) * periodInMonth).ToString();                        
                    }
                    else
                    {
                        amountTextBox.Text = "0.00";
                    }

                    break;
                case "txtFiMasuk":
                    if (amountCheckbox.Checked)
                    {
                        amountTextBox.Text = AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.FiMasuk).ToString();
                    }
                    else
                    {
                        amountTextBox.Text = "0.00";
                    }

                    break;
                case "txtTabung":
                    if (amountCheckbox.Checked)
                    {
                        txtTabung.Text = (AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa) * periodInMonth).ToString();
                    }
                    else
                    {
                        amountTextBox.Text = "0.00";
                    }
                    break;
                default:
                    if (amountCheckbox.Checked == false)
                    {
                        amountTextBox.Text = "0.00";
                    }
                    break;
            }
            ChangeLabelPayment();
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
            InitializeAmountTextBox(amountTextbox, amountCheckbox);
        }

        private void CheckboxTextboxChanged(CheckBox amountCheckbox, TextBox amountTextbox, TextBox interestTextbox)
        {
            if (amountCheckbox.Checked == false)
            {
                amountTextbox.Enabled = false;
                interestTextbox.Enabled = false;
            }
            else
            {
                amountTextbox.Enabled = true;
                interestTextbox.Enabled = true;
            }
            InitializeAmountTextBox(amountTextbox, amountCheckbox);
            InitializeAmountTextBox(interestTextbox, amountCheckbox);
        }

        private void RecalculateTotal_OnAmountChanged(object sender, EventArgs e)
        {
            paymentTotal = 0.0;
            CalculateTotal(chkYuran, txtYuran);
            CalculateTotal(chkTabung, txtTabung);
            CalculateTotal(chkSaham, txtSaham);
            CalculateTotal(chkFiMasuk, txtFiMasuk);
            CalculateTotal(chkSimpananKhas, txtSimpananKhas);
            CalculateTotal(chkPinjamanBiasa, txtPinjamanBiasa, txtFaedahPinjamanBiasa);
            CalculateTotal(chkPinjamanKhas, txtPinjamanKhas, txtFaedahPinjamanKhas);
            CalculateTotal(chkPinjamanMedi, txtPinjamanMedi, txtFaedahPinjamanMedi);
            CalculateTotal(chkPinjamanKecemasan, txtPinjamanKecemasan, txtFaedahPinjamanKecemasan);
        }

        private void RecalculateTotal_OnAmountChanged()
        {
            paymentTotal = 0.0;
            CalculateTotal(chkYuran, txtYuran);
            CalculateTotal(chkTabung, txtTabung);
            CalculateTotal(chkSaham, txtSaham);
            CalculateTotal(chkFiMasuk, txtFiMasuk);
            CalculateTotal(chkSimpananKhas, txtSimpananKhas);
            CalculateTotal(chkPinjamanBiasa, txtPinjamanBiasa, txtFaedahPinjamanBiasa);
            CalculateTotal(chkPinjamanKhas, txtPinjamanKhas, txtFaedahPinjamanKhas);
            CalculateTotal(chkPinjamanMedi, txtPinjamanMedi, txtFaedahPinjamanMedi);
            CalculateTotal(chkPinjamanKecemasan, txtPinjamanKecemasan, txtFaedahPinjamanKecemasan);
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

        private void CalculateTotal(CheckBox amountCheckbox, TextBox amountTextBox, TextBox interestTextBox)
        {
            if (amountCheckbox.Checked)
            {
                paymentTotal += amountTextBox.Text.ToDouble();
                paymentTotal += interestTextBox.Text.ToDouble();
            }
            txtTotal.Text = paymentTotal.ToString();
            this.Assign2DecimalToAmount(txtTotal);
            this.Assign2DecimalToAmount(amountTextBox);
            this.Assign2DecimalToAmount(interestTextBox);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.InitInputProperty();
        }

        private void Period_OnChanged(object sender, EventArgs e)
        {
            this.periodInMonth = GetPeriod();
            if(chkYuran.Checked) txtYuran.Text = (AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.YuranBulanan) * periodInMonth).ToString();
            if(chkTabung.Checked) txtTabung.Text = (AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa) * periodInMonth).ToString();
            RecalculateTotal_OnAmountChanged();
            ChangeLabelPayment();
        }

        private bool ValidateEntries()
        {
            if (CalculatedPayments.Count < 1) LoadCalculatedPayments();   
            if (this.txtTotal.Text.ToDouble() == 0.0)
            {
                ShowMessage("Tiada nilai penerimaan dikesan.", MessageFor.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(this.txtNoResit.Text))
            {
                ShowMessage("Sila masukkan [No Resit].", MessageFor.Warning);
                return false;
            }

            return true;
        }

        private void ChangeLabelPayment()
        {
            lblPaymentMessage.Text = string.Format(paymentMessage, periodInMonth.ToString());
        }

        private void txtNoResit_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
