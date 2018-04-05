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
    public partial class Input_ReportViewer_LejerPinjaman : Form 
    {
        public int MemberID { get; set; }
        public int MemberLoanID { get; set; }
        public Enums.PaymentEnum.AccountID LoanType;
        ReportViewer_LejerPinjaman baseViewer;
        public Input_ReportViewer_LejerPinjaman(ReportViewer_LejerPinjaman viewer)
        {
            InitializeComponent();
            baseViewer = viewer;
        }

        private void LedgerByMemberBalanceLastYear_Load(object sender, EventArgs e)
        {
            WriteFormHeader();
            LoadLoanSetting();

            Assign2DecimalToAmount(txtInterest);
            Assign2DecimalToAmount(txtBase);
            Assign2DecimalToAmount(txtInterestBalance);
            Assign2DecimalToAmount(txtBaseBalance);
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

        private void NonDecimalNumericalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var loanSetting = new MemberLoan();
            loanSetting.ID = this.MemberLoanID;
            loanSetting.MemberID = this.MemberID;
            loanSetting.Base = txtBase.Text.ToDouble();
            loanSetting.Interest = txtInterest.Text.ToDouble();
            loanSetting.BaseBalance = txtBaseBalance.Text.ToDouble();
            loanSetting.InterestBalance = txtInterestBalance.Text.ToDouble();
            loanSetting.LoanDate = dtLoan.Value.ToString();
            loanSetting.LoanStartDate = dtLoanStart.Value.ToString();
            loanSetting.LoanEndDate = dtLoanEnd.Value.ToString();
            loanSetting.LoanDuration = txtLoanDuration.Text.ToInt();
            loanSetting.Type = LoanType;
         
            if (loanSetting.Create())
            {
                baseViewer.LoadLoanSetting();
                Close();
            }
        }

        public void LoadLoanSetting()
        {
            MemberLoanReader reader = new Entity.MemberLoanReader();
            List<FilterElement> filterCollection = new List<FilterElement>();
            FilterElement filterMemberID = new Entity.FilterElement();
            filterMemberID.Key = Enums.Data.KeyElements.MemberID;
            filterMemberID.Value = MemberID;

            FilterElement filterLoanType = new Entity.FilterElement();
            filterLoanType.Key = Enums.Data.KeyElements.UseColumnName;
            filterLoanType.ColumnName = "TypeID";
            filterLoanType.Value = (int)LoanType;

            filterCollection.Add(filterMemberID);
            filterCollection.Add(filterLoanType);

            if (reader.ReadSingle(filterCollection))
            {
                this.MemberLoanID = reader.SingleRecord.ID;
                txtBase.Text = reader.SingleRecord.Base.ToString();
                txtInterest.Text = reader.SingleRecord.Interest.ToString();
                txtBaseBalance.Text = reader.SingleRecord.BaseBalance.ToString();
                txtInterestBalance.Text = reader.SingleRecord.InterestBalance.ToString();
                txtLoanDuration.Text = reader.SingleRecord.LoanDuration.ToString();
                dtLoan.Value = DateTime.Parse(reader.SingleRecord.LoanDate.ToDateMalayFormatted(false));
                dtLoanStart.Value = DateTime.Parse(reader.SingleRecord.LoanStartDate.ToDateMalayFormatted(false));
                dtLoanEnd.Value = DateTime.Parse(reader.SingleRecord.LoanEndDate.ToDateMalayFormatted(false));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WriteFormHeader()
        {
            switch (LoanType)
            {
                case SPA.Enums.PaymentEnum.AccountID.PinjamanBiasa:
                    this.Text += "- Pinjaman Biasa";
                    break;
                case SPA.Enums.PaymentEnum.AccountID.PinjamanKecemasan:
                    this.Text += "- Pinjaman Kecemasan";
                    break;
                case SPA.Enums.PaymentEnum.AccountID.PinjamanKhas:
                    this.Text += "- Pinjaman Khas";
                    break;
                case SPA.Enums.PaymentEnum.AccountID.PinjamanMedisihat:
                    this.Text += "- Pinjaman Medisihat";
                    break;
                default:
                    break;
            }
        }

      
    }
}

