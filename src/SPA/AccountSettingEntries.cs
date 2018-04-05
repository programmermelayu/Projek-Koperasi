using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Control;
using SPA.Enums;
using SPA.Entity;
using SPA.Core;
using SPA.Cache;

namespace SPA
{
    public partial class AccountSettingEntries : SPAForm
    {

        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public int ID { get; set; }
            public int AccountID { get; set; }
            public string AccountCode { get; set; }
            public string AccountDescription { get; set; }
            public string Amount { get; set; }
            public string Status { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        public AccountSettingEntries()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            AccountSettingEntry entryForm = new AccountSettingEntry(this, FormEnum.EntryMode.New);
            entryForm.Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            AccountSettingEntry entryForm = new AccountSettingEntry(this, FormEnum.EntryMode.Edit);
            entryForm.AccountSettingID = dgSettings[dgSettings.Columns["ID"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToInt();
            entryForm.AccountID = dgSettings[dgSettings.Columns["AccountID"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToInt();
            entryForm.Amount = dgSettings[dgSettings.Columns["Amount"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToDouble();
            entryForm.StartDate = dgSettings[dgSettings.Columns["StartDate"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToDateFormatted();
            entryForm.EndDate = dgSettings[dgSettings.Columns["EndDate"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToDateFormatted();
            entryForm.Show();
        }

        private void AccountSettingEntries_Load(object sender, EventArgs e)
        {
            RefillDatagridView();
        }

        public void RefillDatagridView()
        {
            LoadRecords();
            if (DisplayRecords.Count > 0)
            {
                ResetDatagridViewColumn();
                SetRowNumber();
                dgSettings.ReadOnly = true;
                dgSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void LoadRecords()
        {
            AccountSettingReader reader = new AccountSettingReader();
            DisplayRecords = new List<DisplayRecord>();
            if (reader.ReadMultiple())
            {
                foreach (var setting in reader.MultipleRecords)
                {
                    DisplayRecord displayRecord = new DisplayRecord();
                    displayRecord.ID = setting.ID;
                    displayRecord.AccountID = setting.AccountID;
                    displayRecord.AccountCode = setting.AccountCode;
                    displayRecord.AccountDescription = AccountCache.GetAccountDescription(setting.AccountID);
                    displayRecord.Amount = setting.Amount.ToString().ShowTo2Decimal();
                    displayRecord.StartDate = setting.StartDate.ToString().ToDateFormatted();
                    displayRecord.EndDate = setting.EndDate.ToString().ToDateFormatted();
                    displayRecord.Status = (setting.IsActive == true) ? "Aktif" : "-";
                    this.DisplayRecords.Add(displayRecord);
                }
                dgSettings.DataSource = DisplayRecords;
            }
            else
            {
                dgSettings.DataSource = null;
                MessageBox.Show("Tiada rekod dikesan!", "Makluman", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetRowNumber()
        {
            foreach (DataGridViewRow row in this.dgSettings.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

        private void ResetDatagridViewColumn()
        {
            DataGridViewColumn coID = dgSettings.Columns["ID"];
            coID.HeaderText = "ID";
            coID.Width = 0;
            coID.Visible = false;

            DataGridViewColumn colAccountID = dgSettings.Columns["AccountID"];
            colAccountID.HeaderText = "ID";
            colAccountID.Width = 0;
            colAccountID.Visible = false;

            DataGridViewColumn colAccountCode = dgSettings.Columns["AccountCode"];
            colAccountCode.HeaderText = "Kod Akaun";
            colAccountCode.Width = 100;

            DataGridViewColumn colAccountDesc = dgSettings.Columns["AccountDescription"];
            colAccountDesc.HeaderText = "Nama Akaun";
            colAccountDesc.Width = 250;

            DataGridViewColumn colAmount = dgSettings.Columns["Amount"];
            colAmount.HeaderText = "Amaun";
            colAmount.Width = 100;

            DataGridViewColumn colStart = dgSettings.Columns["StartDate"];
            colStart.HeaderText = "Efektif bermula";
            colStart.Width = 150;

            DataGridViewColumn colEnd = dgSettings.Columns["EndDate"];
            colEnd.HeaderText = "Efektif berakhir";
            colEnd.Width = 150;

            dgSettings.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgSettings.RowHeadersWidth = 50;
        }

        void dgSettings_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            AccountSettingEntry editForm = new AccountSettingEntry(this, FormEnum.EntryMode.Edit);

            editForm.AccountSettingID = dgSettings[dgSettings.Columns["ID"].Index, e.RowIndex].Value.ToString().ToInt();
            editForm.AccountID = dgSettings[dgSettings.Columns["AccountID"].Index, e.RowIndex].Value.ToString().ToInt();
            editForm.Amount = dgSettings[dgSettings.Columns["Amount"].Index, e.RowIndex].Value.ToString().ToDouble();
            editForm.StartDate = dgSettings[dgSettings.Columns["StartDate"].Index, e.RowIndex].Value.ToString().ToDateFormatted();
            editForm.EndDate = dgSettings[dgSettings.Columns["EndDate"].Index, e.RowIndex].Value.ToString().ToDateFormatted();

            editForm.Show();
        }

    }
}
