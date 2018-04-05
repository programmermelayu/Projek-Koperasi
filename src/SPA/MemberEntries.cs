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
using SPA.Cache;
using SPA.Core;
using SPA.Control;

namespace SPA
{
    public partial class MemberEntries : SPAForm 
    {
        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string NoKP { get; set; }
            public int Age { get; set; }
            public string Category { get; set; }
            public string Status { get; set; }

            public string PermanentAddress { get;set;}
            public string PermanentDistrict { get; set; }
            public string PermanentPostcode { get; set; }
            public string PermanentState { get; set; }

            public string CurrentAddress { get; set; }
            public string CurrentDistrict { get; set; }
            public string CurrentPostcode { get; set; }
            public string CurrentState { get; set; }

            public string OfficePositionTitle { get; set; }
            public string OfficeAddress { get; set; }
            public string OfficeDistrict { get; set; }
            public string OfficePostcode { get; set; }
            public string OfficeState { get; set; }
            public string OfficeRetiredDate { get; set; }

            public string Email { get; set; }
            public string HomePhone { get; set; }
            public string MobilePhone { get; set; }
        }

        private MemberReader MemberReader { get; set; }
        public MemberEntries()
        {
            InitializeComponent();
        }

        private MemberEntry entryForm;
        private void btnNew_Click(object sender, EventArgs e)
        {
            entryForm = new MemberEntry(this, FormEnum.EntryMode.New);
            entryForm.ShowDialog();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefillDatagridView();
        }

        private void MemberEntries_Load(object sender, EventArgs e)
        {
            FillDropdownList();
            RefillDatagridView();
            this.btnUpdate.Click += new EventHandler(GridNotSelected_ButtonClick);
            this.btnPrint.Click  += new EventHandler(GridNotSelected_ButtonClick);
        }

        private void FillDropdownList()
        {
            foreach (var category in Cache.SettingsCache.Categories)
            {
                cmbCategory.Items.Add(category.Description);
            }
        }

        private void LoadRecords()
        {
            dgMembers.DataSource = null;
            DisplayRecords = new List<DisplayRecord>();
            MemberReader = new MemberReader();

            if (MemberReader.ReadMultiple(GetFilters()))
            {
                foreach (var member in MemberReader.MultipleRecords)
                {
                    DisplayRecord displayRecord = new DisplayRecord();
                    displayRecord.ID = member.ID;
                    displayRecord.Code  = member.Code;
                    displayRecord.Name = member.Name;
                    displayRecord.NoKP = member.NewIC;
                    displayRecord.Age = Helper.CalculateAgeInYears(DateTime.Parse(member.Birthdate.ToDateFormatted()));
                    displayRecord.Category = Cache.SettingsCache.GetDescription(SettingEnum.Setting.Category, member.CategoryID);
                   
                    displayRecord.PermanentAddress = member.PermanentAddress;
                    displayRecord.PermanentPostcode = member.PermanentPostcode;
                    displayRecord.PermanentDistrict = member.PermanentDistrict;
                    displayRecord.PermanentState = Cache.SettingsCache.GetDescription(SettingEnum.Setting.State, member.PermanentStateID);

                    displayRecord.CurrentAddress = member.PermanentAddress;
                    displayRecord.CurrentPostcode = member.PermanentPostcode;
                    displayRecord.CurrentDistrict = member.PermanentDistrict;
                    displayRecord.CurrentState = Cache.SettingsCache.GetDescription(SettingEnum.Setting.State, member.CurrentStateID);

                    displayRecord.OfficePositionTitle = member.OfficePositionTitle;

                    TimeSpan daysDiff = (DateTime.Today - DateTime.Parse(member.OfficeRetiredDate));
                    displayRecord.OfficeRetiredDate =  (daysDiff.Days < 1) ? string.Empty : member.OfficeRetiredDate;
                   
                    displayRecord.OfficeAddress = member.OfficeAddress;
                    displayRecord.OfficePostcode = member.OfficePostcode;
                    displayRecord.OfficeDistrict = member.OfficeDistrict;
                    displayRecord.OfficeState = Cache.SettingsCache.GetDescription(SettingEnum.Setting.State, member.OfficeStateID);

                    displayRecord.HomePhone = member.PersonalHomePhone;
                    displayRecord.MobilePhone = member.PersonalMobilePhone;
                    displayRecord.Email = member.PersonalEmail;
                    displayRecord.Status = (member.StatusID == (int)Enums.MemberEnum.Status.Active) ? "Aktif" : "Tidak Aktif"; 
                    this.DisplayRecords.Add(displayRecord);

                }
                dgMembers.DataSource = DisplayRecords;
            }
        }

        private List<FilterElement> GetFilters()
        {
            List<FilterElement> filterCollection = new List<FilterElement>();
            
            if (!string.IsNullOrEmpty(txtSearchCode.Text))
            {
                FilterElement filterCode = new FilterElement();
                filterCode.Key = Enums.Data.KeyElements.MemberID;
                filterCode.Value = MemberCache.GetMemberID(txtSearchCode.Text.Trim());
                filterCollection.Add(filterCode);
            }

            if (!string.IsNullOrEmpty(this.cmbStatus.Text))
            {
                FilterElement filterStatus = new FilterElement();
                filterStatus.Key = Enums.Data.KeyElements.UseColumnName;
                filterStatus.ColumnName = "StatusID";
                filterStatus.Value = (this.cmbStatus.SelectedIndex == 1) ? 1 : 2;
                filterCollection.Add(filterStatus);
            }

            if (!string.IsNullOrEmpty(this.cmbCategory.Text))
            {
                FilterElement filterCategory = new FilterElement();
                filterCategory.Key = Enums.Data.KeyElements.UseColumnName;
                filterCategory.ColumnName = "CategoryID";
                filterCategory.Value = SettingsCache.GetID(SettingEnum.Setting.Category, cmbCategory.Text);
                filterCollection.Add(filterCategory);
            }

            return (filterCollection.Count > 0) ? filterCollection : null;
        }


        public void RefillDatagridView()
        {
            LoadRecords();
            if (DisplayRecords.Count > 0)
            {
                ResizeDatagridViewColumn();
                SetRowNumber();
                dgMembers.ReadOnly = true;
                dgMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }           
        }

       
        private void ResizeDatagridViewColumn()
        {
            DataGridViewColumn columnID = dgMembers.Columns["ID"];
            columnID.HeaderText = "ID";
            columnID.Width = 0;
            columnID.Visible = false;

            DataGridViewColumn columnCode = dgMembers.Columns["Code"];
            columnCode.HeaderText = "No Anggota";
            columnCode.Width = 200;

            DataGridViewColumn columnName = dgMembers.Columns["Name"];
            columnName.HeaderText = "Nama";
            columnName.Width = 300;

            DataGridViewColumn columnNewIC = dgMembers.Columns["NoKP"];
            columnNewIC.HeaderText = "No MyKad";
            columnNewIC.Width = 200;

            DataGridViewColumn Category = dgMembers.Columns["Category"];
            Category.HeaderText = "Kategori";
            Category.Width = 100;

            DataGridViewColumn colStatus = dgMembers.Columns["Status"];
            colStatus.HeaderText = "Status";
            colStatus.Width = 100;

            DataGridViewColumn Age = dgMembers.Columns["Age"];
            Age.HeaderText = "Umur";
            Age.Width = 300;
            columnCode.SortMode = DataGridViewColumnSortMode.Automatic;

            DataGridViewColumn columnPerAddress = dgMembers.Columns["PermanentAddress"];
            columnPerAddress.HeaderText = "Alamat Tetap";
            columnPerAddress.Width = 1000;

            DataGridViewColumn colPerPostcode = dgMembers.Columns["PermanentPostcode"];
            colPerPostcode.HeaderText = "Poskod";
            colPerPostcode.Width = 100;

            DataGridViewColumn colPerDistrict = dgMembers.Columns["PermanentDistrict"];
            colPerDistrict.HeaderText = "Daerah";
            colPerDistrict.Width = 100;

            DataGridViewColumn colPermanentState = dgMembers.Columns["PermanentState"];
            colPermanentState.HeaderText = "Nageri";
            colPermanentState.Width = 100;

            DataGridViewColumn CurrentAddress = dgMembers.Columns["CurrentAddress"];
            CurrentAddress.HeaderText = "Alamat Tetap";
            CurrentAddress.Width = 1000;

            DataGridViewColumn CurrentPostcode = dgMembers.Columns["CurrentPostcode"];
            CurrentPostcode.HeaderText = "Poskod";
            CurrentPostcode.Width = 100;

            DataGridViewColumn CurrentDistrict = dgMembers.Columns["CurrentDistrict"];
            CurrentDistrict.HeaderText = "Daerah";
            CurrentDistrict.Width = 100;

            DataGridViewColumn CurrentState = dgMembers.Columns["CurrentState"];
            CurrentState.HeaderText = "Nageri";
            CurrentState.Width = 100;

            DataGridViewColumn OfficePositionTitle = dgMembers.Columns["OfficePositionTitle"];
            OfficePositionTitle.HeaderText = "Pekerjaan";
            OfficePositionTitle.Width = 100;

            DataGridViewColumn OfficeRetiredDate = dgMembers.Columns["OfficeRetiredDate"];
            OfficeRetiredDate.HeaderText = "Tarikh Bersara";
            OfficeRetiredDate.Width = 100;

            DataGridViewColumn OfficeAddress = dgMembers.Columns["OfficeAddress"];
            OfficeAddress.HeaderText = "Alamat Tempat Bekerja";
            OfficeAddress.Width = 200;

            DataGridViewColumn OfficePostcode = dgMembers.Columns["OfficePostcode"];
            OfficePostcode.HeaderText = "Poskod";
            OfficePostcode.Width = 100;

            DataGridViewColumn OfficeDistrict = dgMembers.Columns["OfficeDistrict"];
            OfficeDistrict.HeaderText = "Daerah";
            OfficeDistrict.Width = 100;

            DataGridViewColumn OfficeState = dgMembers.Columns["OfficeState"];
            OfficeState.HeaderText = "Nageri";
            OfficeState.Width = 100;

            DataGridViewColumn colHomePhone = dgMembers.Columns["HomePhone"];
            colHomePhone.HeaderText = "Telefon Rumah";
            colHomePhone.Width = 200;

            DataGridViewColumn colMobilePhone = dgMembers.Columns["MobilePhone"];
            colMobilePhone.HeaderText = "Telefon Bimbit";
            colMobilePhone.Width = 200;

            DataGridViewColumn colEmail = dgMembers.Columns["Email"];
            colEmail.HeaderText = "Emel";
            colEmail.Width = 500;


            dgMembers.RowHeadersWidth = 50;

        }

        private void SetRowNumber()
        {
            foreach (DataGridViewRow row in this.dgMembers.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);    
            }
        }

        void dgMembers_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            entryForm = new MemberEntry(this, FormEnum.EntryMode.Edit);
            entryForm.MemberID = dgMembers[dgMembers.Columns["ID"].Index, e.RowIndex].Value.ToString().ToInt();
            entryForm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radGroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgMembers.CurrentRow != null)
            {
                entryForm = new MemberEntry(this, FormEnum.EntryMode.Edit);
                entryForm.MemberID = dgMembers[dgMembers.Columns["ID"].Index, dgMembers.CurrentRow.Index].Value.ToString().ToInt();
                entryForm.Show();
            }  
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgMembers.CurrentRow == null)
            {
                return;
            }

            int memberID = dgMembers[dgMembers.Columns["ID"].Index, dgMembers.CurrentRow.Index].Value.ToString().ToInt();
            int statusID = dgMembers[dgMembers.Columns["Status"].Index, dgMembers.CurrentRow.Index].Value.ToString().ToInt();

            MembershipLetterViewer viewer = new MembershipLetterViewer();
            viewer.MemberID = memberID;
            if (statusID != (int)Enums.MemberEnum.Status.Active)
            {
                Member member = new Member();
                member.ID = memberID;
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

        private void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //List<Member> InactiveMembers = MemberReader.MultipleRecords.Where(x => x.StatusID == (int)Enums.MemberEnum.Status.NotActive).ToList();
            //foreach (var member in InactiveMembers)
            foreach (var member in MemberReader.MultipleRecords)
            {

               member.UpdateStatus(); 
        
            }
            this.RefillDatagridView();
            this.Cursor = Cursors.Default;
        }

        private void GridNotSelected_ButtonClick(object sender, EventArgs e)
        {
            if (dgMembers.CurrentRow == null)
            {
                MessageBox.Show("Sila klik pada salah satu rekod sebelum melakukan tindakan ini.",
                "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgMembers.CurrentRow == null)
            {
                return;
            }

            DialogResult result = MessageBox.Show("Adakah anda ingin memadam anggota ini?",
                                "Padam Anggota?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            Member member = new Member();
            member.ID = dgMembers[dgMembers.Columns["ID"].Index, dgMembers.CurrentRow.Index].Value.ToString().ToInt();
            if (member.Delete())
            {
                ShowMessage("Anggota ini telah berjaya dipadam.", MessageFor.Information);
                RefillDatagridView();
            }
            else
            {
                ShowMessage("Anggota ini tidak berjaya dipadam.", MessageFor.Error);
            }

        }

     
    }

}
