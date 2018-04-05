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

namespace SPA
{
    public partial class SettingEntry : SPAForm
    {
        private FormEnum.EntryMode eMode;
        private SettingEntries parentForm;
        private SettingEnum.Setting settingName;
        public int SettingID { get; set; }
        public string SettingDescription { get; set; }
        public SettingEntry(SettingEntries parent, FormEnum.EntryMode entryMode, SettingEnum.Setting name)
        {
            eMode = entryMode;
            parentForm = parent;
            settingName = name;
            InitializeComponent();
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
            Setting setting = new Setting(settingName);
            setting.Description = txtDescription.Text.Trim();

            if (setting.CheckExist())
            {
                ShowMessage("Rekod ini telah wujud.", MessageFor.Warning);
                return;
            }

            if (setting.Create())
            {
                ShowSaveSuccess();
                Cache.SettingsCache.LoadSettings();
            }
        }

        private void UpdateRecord()
        {
            Setting setting = new Setting(settingName);
            setting.ID = SettingID;
            setting.Description = txtDescription.Text;
            if (setting.Create())
            {
                ShowUpdateSuccess();
                Cache.SettingsCache.LoadSettings();
            }
        }

        private void SettingEntry_Load(object sender, EventArgs e)
        {
            string operationName = string.Empty;
            switch (eMode)
            {
                case FormEnum.EntryMode.Edit:
                    btnSave.Text = "Kemaskini";
                    operationName = btnSave.Text;
                    txtDescription.Text = SettingDescription;
                    break;
                case FormEnum.EntryMode.New:
                    btnSave.Text = "Simpan";
                    operationName = "Tambah";
                    break;
                default:
                    break;
            }

            this.Text = operationName + " - " + SettingEntries.GetSettingDescription(settingName);
            this.lblDescription.Text = SettingEntries.GetSettingDescription(settingName) + ":";
        }

        //private string GetSettingDescription()
        //{
        //    string settingDescripton = string.Empty;
        //    switch (settingName)
        //    {
        //        case SettingEnum.Setting.Account:
        //            break;
        //        case SettingEnum.Setting.Citizenship:
        //            settingDescripton = "Kewarganegaraan";
        //            break;
        //        case SettingEnum.Setting.Race:
        //            settingDescripton = "Bangsa";
        //            break;
        //        case SettingEnum.Setting.Religion:
        //            settingDescripton = "Agama";
        //            break;
        //        case SettingEnum.Setting.State:
        //            settingDescripton = "Negeri";
        //            break;
        //        case SettingEnum.Setting.Wasi:
        //            settingDescripton = "Hubungan Wasi";
        //            break;
        //        case SettingEnum.Setting.MaritalStatus:
        //            settingDescripton = "Status Perkahwinan";
        //            break;
        //        case SettingEnum.Setting.Category:
        //            settingDescripton = "Kategori";
        //            break;
        //        default:
        //            break;
        //    }
        //    return settingDescripton;
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
