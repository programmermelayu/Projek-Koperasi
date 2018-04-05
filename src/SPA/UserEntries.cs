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
using SPA.Entity;

namespace SPA
{
    public partial class UserEntries : Form
    {
        private List<DisplayRecord> DisplayRecords { get; set; }
        private class DisplayRecord
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string UserType { get; set; } 
        }

        public UserEntries()
        {
            InitializeComponent();
        }

        private void UserEntries_Load(object sender, EventArgs e)
        {
            RefillDatagridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            UserEntry entryForm = new UserEntry(this, Enums.FormEnum.EntryMode.New);
            entryForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UserEntry entryForm = new UserEntry(this, Enums.FormEnum.EntryMode.Edit);
            entryForm.UserID = dgUsers[dgUsers.Columns["ID"].Index, dgUsers.CurrentRow.Index].Value.ToString().ToInt();
            entryForm.ShowDialog();
        }

        void dgMembers_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            UserEntry entryForm = new UserEntry(this, Enums.FormEnum.EntryMode.Edit);
            entryForm.UserID = dgUsers[dgUsers.Columns["ID"].Index, e.RowIndex].Value.ToString().ToInt();
            entryForm.ShowDialog();
        }

        public void RefillDatagridView()
        {
            LoadRecords();
            if (DisplayRecords.Count > 0)
            {
                ResetDatagridViewColumn();
                SetRowNumber();
                dgUsers.ReadOnly = true;
                dgUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void LoadRecords()
        {
            UserReader reader = new UserReader();
            DisplayRecords = new List<DisplayRecord>();
            if (reader.ReadMultiple())
            {
                foreach (var user in reader.MultipleRecords)
                {
                    DisplayRecord displayRecord = new DisplayRecord();
                    displayRecord.ID = user.ID;
                    displayRecord.Name = user.Name;
                    displayRecord.Username = user.Username;
                    displayRecord.UserType = (user.UserTypeID == 1) ? "Administrator" : "Pengguna Biasa";

                    this.DisplayRecords.Add(displayRecord);
                }
                dgUsers.DataSource = DisplayRecords;
            }
            else
            {
                dgUsers.DataSource = null;
                MessageBox.Show("Tiada rekod dikesan!", "Makluman", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetRowNumber()
        {
            foreach (DataGridViewRow row in this.dgUsers.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

        private void ResetDatagridViewColumn()
        {
            DataGridViewColumn coID = dgUsers.Columns["ID"];
            coID.HeaderText = "ID";
            coID.Width = 0;
            coID.Visible = false;

            DataGridViewColumn colName = dgUsers.Columns["Name"];
            colName.HeaderText = "Nama Penuh";
            colName.Width = 250;

            DataGridViewColumn colUName = dgUsers.Columns["Username"];
            colUName.HeaderText = "Nama Pengguna";
            colUName.Width = 150;


            DataGridViewColumn colType = dgUsers.Columns["UserType"];
            colType.HeaderText = "Kategori";
            colType.Width = 150;

            dgUsers.RowHeadersWidth = 50;
        }

    }
}
