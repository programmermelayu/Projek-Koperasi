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
using SPA.Cache;
using SPA.Entity;

namespace SPA
{
    public partial class UserChangePassword : SPAForm 
    {
        public UserChangePassword()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (VerifyOldPassword()==false)
            {
                ShowMessage("Kata laluan lama tidak tepat, sila masukkan sekali lagi.", MessageFor.Restriction);
                txtOldPassword.Focus();
                return;
            }

            if (VerifyConfirmPassword()==false)
            {
                ShowMessage("Ulangan kata laluan baru tidak tepat.", MessageFor.Restriction);
                txtNewPassword2.Focus();
                return;
            }

            if (VerifyChangePassword()==false)
            {
                ShowMessage("Sila masukkan kata laluan baru yang berbeza dari kata laluan lama.", MessageFor.Restriction);
                txtNewPassword1.Focus();
                return;
            }

            User user = new User();
            user.ID = UserCache.CurrentUserID;
            user.Password = txtNewPassword1.Text.Trim();
            if (user.Create())
            {
                ShowMessage("Kata laluan telah berjaya ditukar.", MessageFor.Information);
                Close();
            }
        }

        private bool VerifyOldPassword()
        {
            return (txtOldPassword.Text.Trim() == UserCache.CurrentUserPassword); 
        }

        private bool VerifyConfirmPassword()
        {
            return (txtNewPassword1.Text.Trim() == txtNewPassword2.Text.Trim()); 
        }

        private bool VerifyChangePassword()
        {
            return (txtOldPassword.Text.Trim() != txtNewPassword1.Text.Trim());
        }

        private void UserChangePassword_Load(object sender, EventArgs e)
        {

        }
    }
}
