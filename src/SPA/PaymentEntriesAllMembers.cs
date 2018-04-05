using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Core;
using SPA.Data;
using SPA.Entity;
using SPA.Enums;
using SPA.Cache;

namespace SPA
{
    public partial class PaymentEntriesAllMembers : Form
    {
        private PaymentReader reader;
        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public bool IsSelected { get; set; }
            public int ID { get; set; }
            public string MemberCode { get; set; }
            public string MemberName { get; set; }
            public string NoLarian { get; set; }
            public int MemberID { get; set; }
            public string Amount { get; set; }
            public string Interest { get; set; }
            public int AccountID { get; set; }
            public string AccountDescription { get; set; }
            public string PaymentMonth { get; set; }
            public string Syscreated { get; set; }
        }

        public PaymentEntriesAllMembers()
        {
            InitializeComponent();
        }

      

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefillDatagridView();
        }

        private void LoadRecords()
        {
            reader = new PaymentReader();
            DisplayRecords = new List<DisplayRecord>();
            if (reader.ReadMultiple(this.GetFilters()))
            {
                foreach (var payment in reader.MultipleRecords)
                {
                    DisplayRecord displayRecord = new DisplayRecord();
                    displayRecord.IsSelected = false;
                    displayRecord.ID = payment.ID;
                    displayRecord.MemberCode = payment.MemberCode;
                    displayRecord.MemberName = payment.MemberName;              
                    displayRecord.NoLarian = payment.NoLarian;
                    displayRecord.AccountID = payment.PaymentDetails[0].AccountID;
                    displayRecord.AccountDescription = payment.PaymentDetails[0].AccountDescription;
                    displayRecord.Amount = payment.PaymentDetails[0].Amount.ToString().ShowTo2Decimal();
                    displayRecord.Interest  = payment.PaymentDetails[0].Interest.ToString().ShowTo2Decimal();
                    displayRecord.PaymentMonth = string.Format("{0:MMM - yyyy}", 
                                                 new DateTime(payment.PaymentDetails[0].PaymentYear, payment.PaymentDetails[0].PaymentMonth, 1));
                    displayRecord.Syscreated = payment.PaymentDetails[0].Syscreated.ToDateFormatted();
                    this.DisplayRecords.Add(displayRecord);
                }
                dgPayments.DataSource = DisplayRecords;
            }
            else
            {
                dgPayments.DataSource = null;
                MessageBox.Show("Tiada rekod dikesan!", "Makluman", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void RefillDatagridView()
        {
            LoadRecords();
            if (DisplayRecords.Count > 0)
            {
                ResetDatagridViewColumn();
                SetRowNumber();
                dgPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private List<FilterElement> GetFilters()
        {
            List<FilterElement> filterCollection = new List<FilterElement>();
            if (txtNoLarian.Text != string.Empty)
            {
                FilterElement filterNoLarian = new FilterElement();
                filterNoLarian.Key = Enums.Data.KeyElements.UseColumnName;
                filterNoLarian.ColumnName = "NomborLarian";
                filterNoLarian.Value = txtNoLarian.Text.Trim();
                filterCollection.Add(filterNoLarian);
            }
        
            FilterElement filterPeriod = new FilterElement();
            filterPeriod.Key = Enums.Data.KeyElements.PeriodBetweenNumber;
            filterPeriod.Value = string.Format("{0:yyyyMM}", radDateStart.Value);
            filterPeriod.Value2  = string.Format("{0:yyyyMM}", radDateEnd.Value);
            filterCollection.Add(filterPeriod);

            return filterCollection;
        }

        private void ResetDatagridViewColumn()
        {
            DataGridViewCheckBoxColumn colSelected = (DataGridViewCheckBoxColumn)dgPayments.Columns["IsSelected"];
            colSelected.HeaderText = "";
            colSelected.Width = 50;
            colSelected.ReadOnly = false;

            DataGridViewColumn colID = dgPayments.Columns["ID"];
            colID.HeaderText = "ID";
            colID.Width = 0;
            colID.Visible = false;

            DataGridViewColumn colMemberCode = dgPayments.Columns["MemberCode"];
            colMemberCode.HeaderText = "Kod Anggota";
            colMemberCode.Width = 100;

            DataGridViewColumn colMemberName = dgPayments.Columns["MemberName"];
            colMemberName.HeaderText = "Nama Anggota";
            colMemberName.Width = 300;

            DataGridViewColumn colAccountID = dgPayments.Columns["AccountID"];
            colAccountID.HeaderText = "Tarikh Transaksi";
            colAccountID.Width = 0;
            colAccountID.Visible = false;

            DataGridViewColumn col1 = dgPayments.Columns["NoLarian"];
            col1.HeaderText = "No Larian";
            col1.Width = 100;

            DataGridViewColumn colAccountDescription = dgPayments.Columns["AccountDescription"];
            colAccountDescription.HeaderText = "Akaun Pembayaran";
            colAccountDescription.Width = 400;

            DataGridViewColumn colAmount = dgPayments.Columns["Amount"];
            colAmount.HeaderText = "Amaun";
            colAmount.Width = 100;

            DataGridViewColumn colInterest = dgPayments.Columns["Interest"];
            colInterest.HeaderText = "Faedah";
            colInterest.Width = 100;

            DataGridViewColumn colPaymentMonth = dgPayments.Columns["PaymentMonth"];
            colPaymentMonth.HeaderText = "Bulan Bayaran";
            colPaymentMonth.Width = 200;


            DataGridViewColumn col5 = dgPayments.Columns["Syscreated"];
            col5.HeaderText = "Tarikh Transaksi";
            col5.Width = 200;

            dgPayments.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgPayments.Columns["Interest"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgPayments.RowHeadersWidth = 50;

        }

        private void SetRowNumber()
        {
            foreach (DataGridViewRow row in this.dgPayments.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

     

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaymentEntries_Load(object sender, EventArgs e)
        {
            radDateStart.Value = DateTime.Today.AddMonths(-6);
            radDateEnd.Value = DateTime.Today.AddMonths(6);

            this.btnDelete.Click += new EventHandler(EmptyRecords_ButtonClick);
            this.btnDelete.Click += new EventHandler(GridNotSelected_ButtonClick);
       
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

        private void EmptyRecords_ButtonClick(object sender, EventArgs e)
        {
            if (dgPayments.RowCount < 1)
            {
                MessageBox.Show("Tiada rekod", 
               "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
        }

        private void GridNotSelected_ButtonClick(object sender, EventArgs e)
        {
            if (dgPayments.RowCount < 1 && dgPayments.CurrentRow == null && dgPayments.RowCount > 0)
            {
              MessageBox.Show("Sila klik pada rekod yang hendak dipadam.",
              "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgPayments.Rows.Count; i++)
            {
                DataGridViewRow row = dgPayments.Rows[i];
                if ((bool)dgPayments[dgPayments.Columns["IsSelected"].Index, i].Value == true)
                {
                    var paymentID = dgPayments[dgPayments.Columns["ID"].Index, i].Value.ToString().ToInt();
                    DoDelete(paymentID);
                }
            }

            RefillDatagridView();
        }

        private void DoDelete(int paymentID)
        {
            Payment payment = new Payment();
            payment.ID = paymentID;
            if (!payment.Delete())
            {
                MessageBox.Show(payment.ErrorMessage, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                foreach (DataGridViewRow row in dgPayments.Rows)
                {
                    row.Cells["IsSelected"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgPayments.Rows)
                {
                    row.Cells["IsSelected"].Value = false;
                }
            }
           
        }

    }
}
