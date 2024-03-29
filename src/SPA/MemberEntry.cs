﻿using System;
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
using SPA.Core;
using SPA.Cache;
using SPA.Control;


namespace SPA
{
    public partial class MemberEntry : SPAForm
    {
        private MemberEntries formMemberEntries { get; set; }
        private FormEnum.EntryMode entryMode;
        private int memberWasiID1 = 0;
        private int memberWasiID2 = 0;
        private int statusID;

        public int MemberID { get; set; }

        private System.Drawing.Color m_tbcolorenter = System.Drawing.Color.LightSteelBlue;
        private System.Drawing.Color m_tbcolorleave = System.Drawing.Color.White;
        private System.Drawing.Color m_tbcolorerror = System.Drawing.Color.Pink;
        protected Boolean m_tbcolorerrorflag = false;

        public MemberEntry(MemberEntries entriesForm, FormEnum.EntryMode mode)
        {
            this.formMemberEntries = entriesForm;
            this.entryMode = mode;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (VerifyInputs(e) == false) return;
            
            Member member = new Member();
            member.ID = MemberID;
            member.NewIC = txtNoKP.Text.Trim();
            member.Name = txtName.Text.Trim();
            member.Code = txtCode.Text.Trim();
            member.Age = numAge.Value.ToString().ToInt();
            member.Birthdate = dtBirthdate.Value.ToString();
            member.CategoryID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.Category, cmbCategory.Text.Trim());

            member.CitizenshipID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.Citizenship, cmbCitizen.Text.Trim());
            member.RaceID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.Race, cmbRace.Text.Trim());
            member.ReligionID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.Religion, cmbReligion.Text.Trim());
            member.SexID = (this.rdoSexMale.Checked) ? 1 : 2;
            member.MaritalStatusID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.MaritalStatus, cmbMaritalStatus.Text.Trim());

            member.PermanentAddress = txtPermanentAddress.Text.Trim();
            member.PermanentDistrict = txtPermanentDistrict.Text.Trim();
            member.PermanentPostcode = txtPermanentPostcode.Text.Trim();
            member.PermanentStateID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.State, cmbPermanentState.Text.Trim());

            member.CurrentAddress = txtCurrentAddress.Text.Trim();
            member.CurrentDistrict = txtCurrentDistrict.Text.Trim();
            member.CurrentPostcode = txtCurrentPostcode.Text.Trim();
            member.CurrentStateID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.State, cmbCurrentState.Text.Trim());

            member.OfficePositionTitle = txtOfficePositionTitle.Text.Trim();
            member.OfficeAddress = txtOfficeAddress.Text.Trim();
            member.OfficeDistrict = txtOfficeDistrict.Text.Trim();
            member.OfficePostcode = txtOfficePostcode.Text.Trim();
            member.OfficeStateID = Cache.SettingsCache.GetID(Enums.SettingEnum.Setting.State, cmbOfficeState.Text.Trim());
            member.OfficePhone = txtOfficePhone.Text.Trim();
            member.OfficeFax = txtOfficeFax.Text.Trim();
            member.OfficeEmail = txtOfficeEmail.Text.Trim();
            if (!string.IsNullOrEmpty(dtOfficeRetiredDate.CustomFormat.Trim()))
            {
                member.OfficeRetiredDate = dtOfficeRetiredDate.Value.ToString();
            }
            member.PersonalEmail = txtPersonalEmail.Text.Trim();
            member.PersonalHomePhone = txtPersonalHomePhone.Text.Trim();
            member.PersonalMobilePhone = txtPersonalMobilePhone.Text.Trim();

            if (chkMemberStart.Checked)
            {
                member.MemberStart = true;
                member.MemberStartDate = dtMemberStart.Value.ToString();                
            }
            else
            {
                member.MemberStart = false;
            }

            if (chkMemberEnd.Checked)
            {
                member.MemberEnd = true;
                member.MemberEndDate = dtMemberEnd.Value.ToString();   
            }
            else
            {
                member.MemberEnd = false;
            }

            if (!string.IsNullOrEmpty(txtWasiName1.Text.Trim()))
            {
                MemberWasi wasi1 = new MemberWasi();
                wasi1.ID = this.memberWasiID1;
                wasi1.MemberID = MemberID;
                wasi1.Name = txtWasiName1.Text.Trim();
                wasi1.MyKad = txtWasiMyKad1.Text.Trim();
                wasi1.Phone = txtWasiPhone1.Text.Trim();
                wasi1.Birthdate = dtWasiBirthdate1.Value.ToString();
                wasi1.WasiID = Cache.SettingsCache.GetID(SettingEnum.Setting.Wasi, cmbWasiRel1.Text.Trim());
                member.MemberWasis.Add(wasi1);
            }

            if (!string.IsNullOrEmpty(txtWasiName2.Text.Trim()))
            {
                MemberWasi wasi2 = new MemberWasi();
                wasi2.ID = this.memberWasiID2;
                wasi2.MemberID = MemberID;
                wasi2.Name = txtWasiName2.Text.Trim();
                wasi2.MyKad = txtWasiMyKad2.Text.Trim();
                wasi2.Phone = txtWasiPhone2.Text.Trim();
                wasi2.Birthdate = dtWasiBirthdate2.Value.ToString();
                wasi2.WasiID = Cache.SettingsCache.GetID(SettingEnum.Setting.Wasi, cmbWasiRel2.Text.Trim());
                member.MemberWasis.Add(wasi2);
            }
           
            if (entryMode == FormEnum.EntryMode.New && member.CheckExist())
            {
                ShowMessage("Anggota ini telah wujud. Hanya kemaskini dibenarkan.", MessageFor.Restriction);
                return;
            }
    
           if (member.Create())
           {
               ShowSaveSuccess();
               MemberCache.LoadMembers();
           }
           else{
               MessageBox.Show(member.ErrorMessage);
               this.Close();
           }

           this.formMemberEntries.RefillDatagridView();
           this.Close();
        }

        private void MemberEntry_Load(object sender, EventArgs e)
        {
            LoadDropdownListSettings();
            this.dtOfficeRetiredDate.CustomFormat = " ";
            dtOfficeRetiredDate.Format = DateTimePickerFormat.Custom;
            if (entryMode == FormEnum.EntryMode.New)
            {
                btnSave.Text = "Simpan";
                dtMemberStart.Enabled = false;
                dtMemberEnd.Enabled = false;
            }
            else
            {
                LoadEntry();
                btnSave.Text = "Kemaskini";
            }
        }

        private void LoadDropdownListSettings()
        {
            foreach (var state in Cache.SettingsCache.States)
            {
                cmbPermanentState.Items.Add(state.Description);
                cmbCurrentState.Items.Add(state.Description);
                cmbOfficeState.Items.Add(state.Description);
            }

            foreach (var citizenship in Cache.SettingsCache.Citizenships)
            {
                cmbCitizen.Items.Add(citizenship.Description);
            }

            foreach (var religion in Cache.SettingsCache.Religions)
            {
                cmbReligion.Items.Add(religion.Description);
            }

            foreach (var race in Cache.SettingsCache.Races)
            {
                cmbRace.Items.Add(race.Description);
            }

            foreach (var wasi in Cache.SettingsCache.Wasis)
            {
                cmbWasiRel1.Items.Add(wasi.Description);
                cmbWasiRel2.Items.Add(wasi.Description);
            }

            foreach (var maritalStatus in Cache.SettingsCache.MaritalStatuses)
            {
                cmbMaritalStatus.Items.Add(maritalStatus.Description);
            }

            foreach (var category in Cache.SettingsCache.Categories)
            {
                cmbCategory.Items.Add(category.Description);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.MemberID < 1) return;
            DialogResult result = MessageBox.Show("Adakah anda ingin memadam anggota ini?",
                                "Padam Anggota?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            Member member = new Member();
            member.ID = this.MemberID;
            if (member.Delete())
            {
                ShowMessage("Anggota ini telah berjaya dipadam.", MessageFor.Information);
                this.formMemberEntries.RefillDatagridView();
                Close();
            }
            else
            {
                ShowMessage("Anggota ini tidak berjaya dipadam.", MessageFor.Error);
            }

        }

        private void LoadEntry()
        {
            MemberReader reader = new MemberReader();
            if (reader.ReadSingle(new FilterElement(){Key = Enums.Data.KeyElements.MemberID, Value = MemberID}))
            {
                txtNoKP.Text = reader.SingleRecord.NewIC;
                txtName.Text =  reader.SingleRecord.Name;
                txtCode.Text =  reader.SingleRecord.Code;
                statusID = reader.SingleRecord.StatusID;
                dtBirthdate.Value = DateTime.Parse(reader.SingleRecord.Birthdate.ToDateMalayFormatted(false));
                numAge.Value = Helper.CalculateAgeInYears(dtBirthdate.Value);

                cmbCategory.Text = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.Category, reader.SingleRecord.CategoryID);
                cmbCitizen.Text  = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.Citizenship, reader.SingleRecord.CitizenshipID);
                cmbRace.Text  = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.Race, reader.SingleRecord.RaceID);
                cmbReligion.Text = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.Religion,  reader.SingleRecord.ReligionID) ;
                if(reader.SingleRecord.SexID == 1) 
                {
                    rdoSexMale.Checked = true;
                }
                else
                {
                    rdoSexFemale.Checked= true;
                }

                cmbMaritalStatus.Text   = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.MaritalStatus,reader.SingleRecord.MaritalStatusID);
 
                txtPermanentAddress.Text = reader.SingleRecord.PermanentAddress;
                txtPermanentDistrict.Text =  reader.SingleRecord.PermanentDistrict;                   
                txtPermanentPostcode.Text = reader.SingleRecord.PermanentPostcode ;
                cmbPermanentState.Text = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, reader.SingleRecord.PermanentStateID);

                txtCurrentAddress.Text = reader.SingleRecord.CurrentAddress;
                txtCurrentDistrict.Text =  reader.SingleRecord.CurrentDistrict;
                txtCurrentPostcode.Text = reader.SingleRecord.CurrentPostcode;   
                cmbCurrentState.Text = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, reader.SingleRecord.CurrentStateID);

                txtOfficePositionTitle.Text = reader.SingleRecord.OfficePositionTitle;
                txtOfficeAddress.Text = reader.SingleRecord.OfficeAddress;
                txtOfficeDistrict.Text = reader.SingleRecord.OfficeDistrict;
                txtOfficePostcode.Text = reader.SingleRecord.OfficePostcode;
                cmbOfficeState.Text = Cache.SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, reader.SingleRecord.OfficeStateID);
                txtOfficePhone.Text = reader.SingleRecord.OfficePhone;
                txtOfficeFax.Text =  reader.SingleRecord.OfficeFax;
                txtOfficeEmail.Text = reader.SingleRecord.OfficeEmail;

                var retiredDate = DateTime.Parse(reader.SingleRecord.OfficeRetiredDate.ToDateMalayFormatted(false));
                if (retiredDate.Year != 2999)
                {
                    dtOfficeRetiredDate.CustomFormat = "dd-MMM-yyyy";
                    dtOfficeRetiredDate.Value = DateTime.Parse(reader.SingleRecord.OfficeRetiredDate.ToDateMalayFormatted(false));
                }
               
                txtPersonalEmail.Text = reader.SingleRecord.PersonalEmail;
                txtPersonalHomePhone.Text = reader.SingleRecord.PersonalHomePhone;
                txtPersonalMobilePhone.Text = reader.SingleRecord.PersonalMobilePhone;

                if (reader.SingleRecord.MemberStart)
                {
                    chkMemberStart.Checked = true;
                    dtMemberStart.Value = DateTime.Parse(reader.SingleRecord.MemberStartDate.ToDateMalayFormatted(false));
                    dtMemberStart.Enabled = true;
                }
                else
                {
                    chkMemberStart.Checked = false;
                    dtMemberStart.Enabled = false;
                }

                if (reader.SingleRecord.MemberEnd)
                {
                    chkMemberEnd.Checked = true;
                    dtMemberEnd.Value = DateTime.Parse(reader.SingleRecord.MemberEndDate.ToDateMalayFormatted(false));
                }
                else
                {
                    chkMemberEnd.Checked = false;
                    dtMemberEnd.Enabled = false;
                }

                if (reader.SingleRecord.StatusID == (int)MemberEnum.Status.Active)
                {
                    ShowMemberStatus(true);
                }
                else
                {
                    ShowMemberStatus(false);
                }
                
                MemberWasiReader wasiReader = new MemberWasiReader();
                if (wasiReader.ReadMultiple(new FilterElement() { Key = Enums.Data.KeyElements.MemberID, Value = MemberID }))
                {
                    for (int i = 0; i < wasiReader.MultipleRecords.Count; i++)
                    {
                        if (i==0)
                        {
                            memberWasiID1 = wasiReader.MultipleRecords[i].ID;
                            txtWasiName1.Text = wasiReader.MultipleRecords[i].Name;
                            txtWasiPhone1.Text = wasiReader.MultipleRecords[i].Phone;
                            txtWasiMyKad1.Text = wasiReader.MultipleRecords[i].MyKad;
                            dtWasiBirthdate1.Value = DateTime.Parse(wasiReader.MultipleRecords[i].Birthdate.ToDateMalayFormatted(false));
                            cmbWasiRel1.Text = Cache.SettingsCache.GetDescription(SettingEnum.Setting.Wasi, wasiReader.MultipleRecords[i].WasiID);
                        }
                        else if (i==1)
                        {
                            memberWasiID2 = wasiReader.MultipleRecords[i].ID;
                            txtWasiName2.Text = wasiReader.MultipleRecords[i].Name;
                            txtWasiPhone2.Text = wasiReader.MultipleRecords[i].Phone;
                            txtWasiMyKad2.Text = wasiReader.MultipleRecords[i].MyKad;
                            dtWasiBirthdate2.Value = DateTime.Parse(wasiReader.MultipleRecords[i].Birthdate.ToDateMalayFormatted(false));
                            cmbWasiRel2.Text = Cache.SettingsCache.GetDescription(SettingEnum.Setting.Wasi, wasiReader.MultipleRecords[i].WasiID);
                        }
                    }
                }

            }
        }

      
        private void ShowMemberStatus(bool active)
        {
            if (active)
            {
                lblStatus.Text  = "Aktif";
                lblStatus.BackColor = Color.DarkBlue;
                lblStatus.ForeColor = Color.White;
            }
            else
            {
                lblStatus.Text  = "Tidak Aktif";
                lblStatus.BackColor = Color.DarkRed;
                lblStatus.ForeColor = Color.White;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MembershipLetterViewer viewer = new MembershipLetterViewer();
            viewer.MemberID = this.MemberID;
            if (statusID != (int)Enums.MemberEnum.Status.Active)
            {
                Member member = new Member();
                member.ID = MemberID;
                if (member.IsActive())
                {
                    viewer.Show();
                }
                else
                {
                    ShowMessage("Anggota ini belum aktif. Surat tidak boleh dicetak.", MessageFor.Restriction);
                }
            }
            else
            {
                viewer.Show();
            }
        }

        private bool VerifyInputs(EventArgs e)
        {
            string notify = "Sila masukkan ";
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ShowMessage(notify + "[Nama] anggota", MessageFor.Warning);
                m_tbcolorerrorflag = false;
                tabControl1.SelectedIndex = 0;
                TextBox_Error(txtName, e);
                return false;
            }

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                ShowMessage(notify + "[No Anggota].", MessageFor.Warning);
                m_tbcolorerrorflag = false;
                tabControl1.SelectedIndex = 0;
                TextBox_Error(txtCode, e);
                return false;
            }

            if (string.IsNullOrEmpty(txtNoKP.Text.Trim()))
            {
                ShowMessage(notify + "[No MyKad] anggota.", MessageFor.Warning);
                m_tbcolorerrorflag = false;
                tabControl1.SelectedIndex = 0;
                TextBox_Error(txtNoKP, e);
                return false;
            }

            if (dtBirthdate.Value > DateTime.Today)
            {
                ShowMessage(notify + "tarikh lahir yang betul.", MessageFor.Warning);
                tabControl1.SelectedIndex = 0;
                dtBirthdate.Focus();
                return false;                
            }

            if (this.dtWasiBirthdate1.Value > DateTime.Today.AddDays(1))
            {
                ShowMessage(notify + "tarikh lahir wasi yang betul.", MessageFor.Warning);
                tabControl1.SelectedIndex = 2;
                dtWasiBirthdate1.Focus();
                return false;
            }

            if (this.dtWasiBirthdate2.Value > DateTime.Today.AddDays(1))
            {
                ShowMessage(notify + "tarikh lahir wasi yang betul.", MessageFor.Warning);
                tabControl1.SelectedIndex = 2;
                dtWasiBirthdate2.Focus();
                return false;
            }

            return true;
        }

        private void chkCopyAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopyAddress.Checked)
            {
                txtCurrentAddress.Text = txtPermanentAddress.Text.Trim();
                txtCurrentDistrict.Text = txtPermanentDistrict.Text.Trim();
                txtCurrentPostcode.Text = txtPermanentPostcode.Text.Trim();
                cmbCurrentState.SelectedIndex = cmbPermanentState.SelectedIndex;
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox tb = null;
            if (sender is TextBox)
            {
                tb = (TextBox)sender;
                if (m_tbcolorerrorflag)
                    tb.BackColor = m_tbcolorerror;
                else
                    tb.BackColor = m_tbcolorenter;
            }

            return;

        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = null;
            if (sender is TextBox)
            {
                tb = (TextBox)sender;
                tb.BackColor = m_tbcolorleave;
            }

            return;
        }

        private void ComboBox_Enter(object sender, EventArgs e)
        {
            ComboBox tb = null;
            if (sender is ComboBox)
            {
                tb = (ComboBox)sender;
                if (m_tbcolorerrorflag)
                    tb.BackColor = m_tbcolorerror;
                else
                    tb.BackColor = m_tbcolorenter;
            }

            return;

        }

        private void ComboBox_Leave(object sender, EventArgs e)
        {
            ComboBox tb = null;
            if (sender is ComboBox)
            {
                tb = (ComboBox)sender;
                tb.BackColor = m_tbcolorleave;
            }

            return;
        }


        private void TextBox_Error(object sender, EventArgs e)
        {
            TextBox tb = null;
            if (sender is TextBox)
            {
                tb = (TextBox)sender;
                tb.BackColor = m_tbcolorerror;
                tb.Focus();
                m_tbcolorerrorflag = false;
            }
            return;
        }


        private void ComboBox_Error(object sender, EventArgs e)
        {
            ComboBox tb = null;
            if (sender is ComboBox)
            {
                tb = (ComboBox)sender;
                tb.BackColor = m_tbcolorerror;
                tb.Focus();
                m_tbcolorerrorflag = false;
            }
            return;
        }

        private void DateTimePicker_Enter(object sender, EventArgs e)
        {
            DateTimePicker tb = null;
            if (sender is DateTimePicker)
            {
                tb = (DateTimePicker)sender;
                if (m_tbcolorerrorflag)
                    tb.BackColor = m_tbcolorerror;
                else
                    tb.BackColor = m_tbcolorenter;
            }

            return;

        }

        private void DateTimePicker_Leave(object sender, EventArgs e)
        {
            DateTimePicker tb = null;
            if (sender is DateTimePicker)
            {
                tb = (DateTimePicker)sender;
                tb.BackColor = m_tbcolorleave;
            }

            return;
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

        private void chkMemberEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMemberEnd.Checked)
            {
                dtMemberEnd.Enabled = true;
            }
            else
            {
                dtMemberEnd.Enabled = false;
            }
        }

        private void chkMemberStart_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMemberStart.Checked)
            {
                dtMemberStart.Enabled = true;
            }
            else
            {
                dtMemberEnd.Enabled = false;
            }
        }

        private void dtOfficeRetiredDate_ValueChanged(object sender, EventArgs e)
        {
            dtOfficeRetiredDate.CustomFormat = "dd-MMM-yyyy";
        }

    }
}
