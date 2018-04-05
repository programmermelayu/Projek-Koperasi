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
using SPA.Control;
using SPA.Core;
using SPA.Enums;
using SPA.Entity;

namespace SPA
{
    public partial class AccountSettingEntry : SPAForm
    {
        private FormEnum.EntryMode eMode;
        private AccountSettingEntries parentForm;
        public int AccountSettingID { get; set; }
        public int AccountID { get; set; }
        public double Amount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public AccountSettingEntry(AccountSettingEntries parent, FormEnum.EntryMode entryMode)
        {
            parentForm = parent;
            eMode = entryMode;
            InitializeComponent();
        }

        private void AccountSettingEntry_Load(object sender, EventArgs e)
        {
            string operationName = string.Empty;
            switch (eMode)
            {
                case FormEnum.EntryMode.Edit:
                    btnSave.Text = "Kemaskini";
                    operationName = btnSave.Text;
                    this.txtAmount.Text = Amount.ToString().ShowTo2Decimal();
                    dtStart.Value = DateTime.Parse(StartDate);
                    dtEnd.Value = DateTime.Parse(EndDate);
                    cmbAccount.SelectedIndex = GetComboAccountIndex();
                    break;
                case FormEnum.EntryMode.New:
                    btnSave.Text = "Simpan";
                    operationName = "Tambah";
                    break;
                default:
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (eMode)
            {
                case FormEnum.EntryMode.Edit:
                    UpdateRecord();
                    break;
                case FormEnum.EntryMode.New:
                    CreateRecord();
                    break;
                default:
                    break;
            }
            parentForm.RefillDatagridView();
            Close();
        }

        private void CreateRecord()
        {
            AccountSetting setting = new AccountSetting();
            setting.AccountID = AccountCache.GetAccountID(cmbAccount.Text.GetCode());
            setting.Amount = txtAmount.Text.ToDouble();
            setting.StartDate = dtStart.Value;
            setting.EndDate = dtEnd.Value;

            if (setting.CheckExist())
            {
                ShowMessage("Anda tidak dibenarkan menambah tetapan akaun yang masih aktif.", MessageFor.Warning);
            }

            if (setting.Create())
            {
                ShowSaveSuccess();
            }
        }

        private void UpdateRecord()
        {
            AccountSetting setting = new AccountSetting();
            setting.ID = AccountSettingID;
            setting.Amount = txtAmount.Text.ToDouble();
            setting.StartDate = dtStart.Value;
            setting.EndDate = dtEnd.Value;
            if (setting.Create())
            {
                ShowUpdateSuccess();
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

        private int GetComboAccountIndex()
        {
            int comboIndex = 0;
            switch (AccountID)
            {
                case 1:
                    comboIndex = 0;
                    //txtFrequency.Text = "Bulanan";
                    break;
                case 2:
                    comboIndex = 1;
                    //txtFrequency.Text = "Bulanan";
                    break;
                case 4:
                    comboIndex = 2;
                    //txtFrequency.Text = "Sekali seumur hidup";
                    break;
                default:
                    break;
            }

            return comboIndex;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbAccount.SelectedIndex)
            {
                case 0:
                case 1:
                    txtFrequency.Text = "Bulanan";
                    break;
                case 2:
                    txtFrequency.Text = "Sekali seumur hidup";
                    break;
                default:
                    break;
            }
        }

    }
}
