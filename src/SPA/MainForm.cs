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
using SPA.Reporting;

namespace SPA
{
    public partial class MainForm : SPAForm 
    {


        public MainForm()
        {
            InitializeComponent();
        }

        private Form childForm;

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
       }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void windowsMenu_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void agamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.Religion);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void keanggotaanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new MemberEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            childForm = new MemberEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void impotPembayaranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new PaymentImportEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;

        }

        private void pambayaranManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new PaymentEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void berdasarkanAnggotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_LejerPembayaran();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new FormErrorLog();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void bangsaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.Race);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void negeriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.State);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void hubunganWasiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.Wasi);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void akaunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new AccountSettingEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void penggunaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new UserEntries();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void tukarPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new UserChangePassword();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new Login();
            childForm.ShowDialog();
            RecreateMenuOnLoginSuccess();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitForm();
            if (UserCache.CurrentUserID < 1)
            {
                childForm = new Login();
                childForm.ShowDialog();
            }
            RecreateMenuOnLoginSuccess();
        }

        private void RecreateMenuOnLoginSuccess()
        {
            if (UserCache.CurrentUserID > 0)
            {
                MainMenu.Items.Remove(this.LoginMainMenu);
                MainMenu.Items.Remove(this.SambunganDbMenu);
                lblCurrentUser.Text = UserCache.CurrentName;
                switch (UserCache.CurrentUserType)
                {
                    case SPA.Enums.UserEnum.UserType.Administrator:
                        ShowAdminMenu();
                        break;
                    case SPA.Enums.UserEnum.UserType.RegularUser:
                        ShowRegularUserMenu();
                        break;
                    default:
                        break;
                }
            }
        }

        private void InitForm()
        {
            MainMenu.Items.Remove(this.KeanggotaanMainMenu);
            MainMenu.Items.Remove(this.PembayaranMainMenu);
            MainMenu.Items.Remove(this.LaporanMainMenu);
            MainMenu.Items.Remove(this.TetapanMainMenu);
            MainMenu.Items.Remove(this.SistemMainMenu);
            MainMenu.Items.Remove(this.LogMainMenu);
        }

        private void ShowAdminMenu()
        {
            lblUserType.Text = "(Administrator)";
            MainMenu.Items.Add(this.KeanggotaanMainMenu);
            MainMenu.Items.Add(this.PembayaranMainMenu);
            MainMenu.Items.Add(this.LaporanMainMenu);
            MainMenu.Items.Add(this.TetapanMainMenu);
            MainMenu.Items.Add(this.SistemMainMenu);
            MainMenu.Items.Add(this.LogMainMenu);
            MainMenu.Items.Add(this.AboutMenu);
        }

        private void ShowRegularUserMenu()
        {
            lblUserType.Text = "(Pengguna Biasa)";
            MainMenu.Items.Add(this.KeanggotaanMainMenu);
            MainMenu.Items.Add(this.PembayaranMainMenu);
            MainMenu.Items.Add(this.LaporanMainMenu);
            MainMenu.Items.Add(this.SistemMainMenu);
            MainMenu.Items.Add(this.AboutMenu);
            SistemMainMenu.DropDownItems.Remove(this.PengurusanPenggunaMenuItem);
        }

        private void kewarganegaraanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.Citizenship);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void statusPerkahwinanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.MaritalStatus);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void SambunganDbMenu_Click(object sender, EventArgs e)
        {
            childForm = new DatabaseConfig();
            childForm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new About();
            childForm.ShowDialog();
        }

        private void PembayaranMainMenu_Click(object sender, EventArgs e)
        {

        }

        private void kategoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new SettingEntries(Enums.SettingEnum.Setting.Category);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void berdasarkanAnggotaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            childForm = new ReportViewer_LejerPinjaman();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;

        }

        private void anggotaYangMembuatPembayaranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_Pembayaran(PaidMemberData.MemberPaymentCategory.Paid);
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;

        }

        private void padamRekodPenerimaanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new PaymentEntriesAllMembers();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void laporanBerdasarkanNegeriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_Pembayaran_ByState();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void laporanUntukSKMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_Pembayaran_SKM();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void tunggakanPembayaranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_TunggakanPembayaran();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;

        }

        private void tunggakanPinjamanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_TunggakanPinjaman();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void laporanBulananTahunanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_Pembayaran_BulananTahunan();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childForm = new ReportViewer_BakiPinjamanBersara();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Minimized;
            childForm.WindowState = FormWindowState.Maximized;

        }


    }
}
