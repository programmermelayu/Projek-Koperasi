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
using SPA.Control;
using SPA.Enums;
using SPA.Entity;

namespace SPA
{
    public partial class SettingEntries : SPAForm 
    {

        private SettingEnum.Setting settingName;

        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public int ID { get; set; }
            public string Description { get; set; }
        }

        public SettingEntries(SettingEnum.Setting name)
        {
            settingName = name;
            InitializeComponent();
        }

        private void SettingEntries_Load(object sender, EventArgs e)
        {
            this.Text = GetSettingDescription(settingName);
            radGroupBox1.Text = GetSettingDescription(settingName);
            RefillDatagridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SettingEntry entryForm = new SettingEntry(this, Enums.FormEnum.EntryMode.New, settingName);
            entryForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SettingEntry entryForm = new SettingEntry(this, Enums.FormEnum.EntryMode.Edit, settingName);
            entryForm.SettingID = dgSettings[dgSettings.Columns["ID"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToInt();
            entryForm.SettingDescription = dgSettings[dgSettings.Columns["Description"].Index, dgSettings.CurrentRow.Index].Value.ToString();
            entryForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgSettings.CurrentRow == null) return;
            DialogResult result = MessageBox.Show("Adakah anda ingin memadam rekod ini?",
                                 "Padam Rekod?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            Setting setting = new Setting(settingName);
            setting.ID = dgSettings[dgSettings.Columns["ID"].Index, dgSettings.CurrentRow.Index].Value.ToString().ToInt();

            if (setting.IsBeingUsed())
            {
                ShowMessage("Tetapan ini telah digunakan. Ia tidak boleh dipadam.", MessageFor.Restriction);
                return;
            }

            if (setting.Delete())
            {
                ShowDeleteSuccess();
                RefillDatagridView();
            }
            else
            {
                ShowMessage("Rekod ini gagal dipadam", MessageFor.Error);
            }
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
            SettingReader reader = new SettingReader(settingName);
            DisplayRecords = new List<DisplayRecord>();
            if (reader.ReadMultiple())
            {
                foreach (var setting in reader.MultipleRecords)
                {
                    DisplayRecord displayRecord = new DisplayRecord();
                    displayRecord.ID = setting.ID;
                    displayRecord.Description = setting.Description;
        
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

            DataGridViewColumn colDescription = dgSettings.Columns["Description"];
            colDescription.HeaderText = GetSettingDescription(settingName);
            colDescription.Width = 370;

            dgSettings.RowHeadersWidth = 50;
        }

        public static string GetSettingDescription(SettingEnum.Setting settingName)
        {
            string settingDescripton = string.Empty;
            switch (settingName)
            {
                case SettingEnum.Setting.Account:
                    break;
                case SettingEnum.Setting.Citizenship:
                    settingDescripton = "Kewarganegaraan";
                    break;
                case SettingEnum.Setting.Race:
                    settingDescripton = "Bangsa";
                    break;
                case SettingEnum.Setting.Religion:
                    settingDescripton = "Agama";
                    break;
                case SettingEnum.Setting.State:
                    settingDescripton = "Negeri";
                    break;
                case SettingEnum.Setting.Wasi:
                    settingDescripton = "Hubungan Wasi";
                    break;
                case SettingEnum.Setting.MaritalStatus:
                    settingDescripton = "Status Perkahwinan";
                    break;
                case SettingEnum.Setting.Category:
                    settingDescripton = "Kategori";
                    break;
                default:
                    break;
            }
            return settingDescripton;
        }

    }
}
