using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPA.Data;
using SPA.Control;
using SPA.Enums;
using SPA.Entity;

namespace SPA
{
    public partial class UserEntry : SPAForm 
    {
        private FormEnum.EntryMode eMode;
        private UserEntries parentForm;
        public int UserID { get; set; }
        private System.Drawing.Color m_tbcolorenter = System.Drawing.Color.LightSteelBlue;
        private System.Drawing.Color m_tbcolorleave = System.Drawing.Color.White;
        private System.Drawing.Color m_tbcolorerror = System.Drawing.Color.Pink;
        private Boolean m_tbcolorerrorflag = false;

        public UserEntry(UserEntries parent, FormEnum.EntryMode entryMode)
        {
            parentForm = parent;
            eMode = entryMode;
            InitializeComponent();
        }

        private void UserEntry_Load(object sender, EventArgs e)
        {
            string operationName = string.Empty;
            switch (eMode)
            {
                case FormEnum.EntryMode.Edit:
                    btnSave.Text = "Kemaskini";
                    operationName = btnSave.Text;
                    txtUsername.Enabled = false;
                    txtPassword.Enabled = false;
                    txtPassword2.Enabled = false;
                    btnReset.Enabled = false;
                    LoadUser();
                    break;
                case FormEnum.EntryMode.New:
                    btnSave.Text = "Simpan";
                    operationName = "Tambah";
                    break;
                default:
                    break;
            }

            this.Text = operationName + " - Pengguna";
        }

        private void LoadUser()
        {
            UserReader user = new UserReader();
            var filterUser = new FilterElement() { Key = Enums.Data.KeyElements.UseColumnName, ColumnName = "ID", Value = UserID };
            if (user.ReadSingle(filterUser))
            {
                txtUsername.Text = user.SingleRecord.Username;
                txtName.Text = user.SingleRecord.Name;
                txtPassword.Text = user.SingleRecord.Password;
                if (user.SingleRecord.UserTypeID == 1)
                {
                    rdoAdminUser.Checked = true;
                }
                else
                {
                    rdoRegularUser.Checked = true;
                }

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void ResetInput()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtPassword2.Clear();
            txtUsername.Clear();
            rdoRegularUser.Checked= true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (VerifyInput(e) == false) return;
            
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
            User user = new User();
            user.Username = txtUsername.Text.Trim();
            user.Password = txtPassword.Text.Trim();
            user.Name = txtName.Text.Trim();
            user.UserTypeID = (this.rdoAdminUser.Checked) ? 1 : 2;

            if (user.CheckExist())
            {
                ShowMessage("Rekod ini telah wujud.", MessageFor.Warning);
                return;
            }

            if (user.Create())
            {
                ShowSaveSuccess();
            }
        }

        private void UpdateRecord()
        {
            User user = new User();
            user.ID = UserID;
            user.Password = txtPassword.Text.Trim();
            user.Name = txtName.Text.Trim();
            user.UserTypeID = (this.rdoAdminUser.Checked) ? 1 : 2;

            if (user.Create())
            {
                ShowUpdateSuccess();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool VerifyConfirmPassword()
        {
            return (txtPassword.Text.Trim() == txtPassword2.Text.Trim());
        }

        private bool VerifyInput(EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                 ShowMessage("Sila masukkan nama penuh pengguna.", MessageFor.Restriction);
                 m_tbcolorerrorflag = false;
                 TextBox_Error(txtName, e);
                 return false;
            }

            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                ShowMessage("Sila masukkan nama pengguna.", MessageFor.Restriction);
                m_tbcolorerrorflag = false;
                TextBox_Error(txtUsername, e);
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                ShowMessage("Sila masukkan nama pengguna.", MessageFor.Restriction);
                m_tbcolorerrorflag = false;
                TextBox_Error(txtPassword, e);
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                ShowMessage("Sila masukkan kata laluan.", MessageFor.Restriction);
                m_tbcolorerrorflag = false;
                TextBox_Error(txtPassword, e);
                return false;
            }

            if ((string.IsNullOrEmpty(txtPassword2.Text)) && (eMode == FormEnum.EntryMode.New))
            {
                ShowMessage("Sila masukkan ulangan kata laluan.", MessageFor.Restriction);
                m_tbcolorerrorflag = false;
                TextBox_Error(txtPassword2, e);
                return false;
            }

            if ((VerifyConfirmPassword() == false) && (eMode == FormEnum.EntryMode.New))
            {
                ShowMessage("Ulangan kata laluan tidak tepat.", MessageFor.Restriction);
                m_tbcolorerrorflag = false;
                TextBox_Error(txtPassword2, e);
                return false;
            }

            return true;
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


    }
}
