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
    public partial class PaymentEntries : Form
    {
        private PaymentReader reader;
        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public int ID { get; set; }
            public string NoLarian { get; set; }
            public int MemberID { get; set; }
            public string Amount { get; set; }
            public string Interest { get; set; }
            public int AccountID { get; set; }
            public string AccountDescription { get; set; }
            public string PaymentMonth { get; set; }
            public string Syscreated { get; set; }
        }

        public PaymentEntries()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblName.Text))
            {
                return;                
            }

            PaymentEntry entry = new PaymentEntry(this, FormEnum.EntryMode.New);
            entry.MemberKP = lblMyKad.Text.Trim();
            entry.Show();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefillMemberInfo();
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
                    displayRecord.ID = payment.ID;
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
                dgPayments.ReadOnly = true;
                dgPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void RefillMemberInfo()
        {
            if (string.IsNullOrEmpty(txtSearchCode.Text)) return;

            Member member = Cache.MemberCache.GetMember(txtSearchCode.Text.Trim());
            if (member != null && !string.IsNullOrEmpty(txtSearchCode.Text))
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
                lblStatus.Text = Common.GetMemberStatusDescription(MemberEnum.Status.NotActive);
                lblStatus.BackColor = Color.Gray; 
            }
        }

        private List<FilterElement> GetFilters()
        {
            List<FilterElement> filterCollection = new List<FilterElement>();
            FilterElement filterCode = new FilterElement();
            filterCode.Key = Enums.Data.KeyElements.MemberID;
            filterCode.Value = MemberCache.GetMemberID(this.txtSearchCode.Text.Trim());

            FilterElement filterPeriod = new FilterElement();
            filterPeriod.Key = Enums.Data.KeyElements.PeriodBetweenNumber;
            filterPeriod.Value = string.Format("{0:yyyyMM}", radDateStart.Value);
            filterPeriod.Value2  = string.Format("{0:yyyyMM}", radDateEnd.Value);

            filterCollection.Add(filterCode);
            filterCollection.Add(filterPeriod);
            return filterCollection;
        }

        private void ResetDatagridViewColumn()
        {
            DataGridViewColumn coID = dgPayments.Columns["ID"];
            coID.HeaderText = "ID";
            coID.Width = 0;
            coID.Visible = false;

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

        void dgMembers_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;                
            }
            PaymentEntryEdit editForm = new PaymentEntryEdit(this);
            editForm.PaymentID = dgPayments[dgPayments.Columns["ID"].Index, e.RowIndex].Value.ToString().ToInt();
            editForm.AccountID = dgPayments[dgPayments.Columns["AccountID"].Index, e.RowIndex].Value.ToString().ToInt();
            editForm.AccountDescription = dgPayments[dgPayments.Columns["AccountDescription"].Index, e.RowIndex].Value.ToString();
            editForm.MemberID = Cache.MemberCache.GetMemberID(txtSearchCode.Text);
            editForm.Amount = dgPayments[dgPayments.Columns["Amount"].Index, e.RowIndex].Value.ToString().ToDouble();
            editForm.Interest = dgPayments[dgPayments.Columns["Interest"].Index, e.RowIndex].Value.ToString().ToDouble();
            editForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaymentEntries_Load(object sender, EventArgs e)
        {
            radDateStart.Value = DateTime.Today.AddMonths(-6);
            radDateEnd.Value = DateTime.Today.AddMonths(6);

            this.btnNew.Click += new EventHandler(EmptyRecords_ButtonClick);
            this.btnDelete.Click += new EventHandler(EmptyRecords_ButtonClick);
            this.btnUpdate.Click += new EventHandler(EmptyRecords_ButtonClick);
            this.btnDelete.Click += new EventHandler(GridNotSelected_ButtonClick);
            this.btnUpdate.Click += new EventHandler(GridNotSelected_ButtonClick);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgPayments.CurrentRow == null) return;
            PaymentEntryEdit editForm = new PaymentEntryEdit(this);
            editForm.PaymentID = dgPayments[dgPayments.Columns["ID"].Index, dgPayments.CurrentRow.Index].Value.ToString().ToInt();
            editForm.AccountID = dgPayments[dgPayments.Columns["AccountID"].Index, dgPayments.CurrentRow.Index].Value.ToString().ToInt();
            editForm.AccountDescription = dgPayments[dgPayments.Columns["AccountDescription"].Index, dgPayments.CurrentRow.Index].Value.ToString();
            editForm.MemberID = Cache.MemberCache.GetMemberID(txtSearchCode.Text);
            editForm.Amount = dgPayments[dgPayments.Columns["Amount"].Index, dgPayments.CurrentRow.Index].Value.ToString().ToDouble();
            editForm.Interest = dgPayments[dgPayments.Columns["Interest"].Index, dgPayments.CurrentRow.Index].Value.ToString().ToDouble();
            editForm.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgPayments.CurrentRow == null) return;
            DialogResult result = MessageBox.Show("Adakah anda ingin memadam rekod ini?", 
                                 "Padam Rekod?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            Payment payment = new Payment();
            payment.ID = dgPayments[dgPayments.Columns["ID"].Index, dgPayments.CurrentRow.Index].Value.ToString().ToInt();
            if (payment.Delete())
            {
                RefillDatagridView();
            }
            else
            {
                MessageBox.Show(payment.ErrorMessage, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EmptyRecords_ButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblName.Text))
            {
                MessageBox.Show("Sila paparkan maklumat anggota dengan memasukkan [No MyKad] atau [No Anggota] sebelum melalukan transaksi ini.", 
               "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
        }

        private void GridNotSelected_ButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblName.Text) && dgPayments.CurrentRow == null && dgPayments.RowCount > 0)
            {
              MessageBox.Show("Sila klik pada rekod yang hendak dikemaskini atau dipadam.",
              "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }

        }
      

    }
}
