namespace SPA
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.LoginMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.KeanggotaanMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PembayaranMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PembayaranSenaraiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImpotPembayaranMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.padamRekodPenerimaanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SistemMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TukarKatalaluanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PengurusanPenggunaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TetapanMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AkaunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeanggotaanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AgamaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BangsaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HubunganWasiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kewarganegaraanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NegeriMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusPerkahwinanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kategoriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LaporanMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LejerPembayaranMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LejarPembayaranByAnggotaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lejerPinjamanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.berdasarkanAnggotaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laporanBerdasarkanNegeriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laporanUntukSKMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tunggakanPembayaranToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tunggakanPinjamanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laporanBulananTahunanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SambunganDbMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserType = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginMainMenu,
            this.KeanggotaanMainMenu,
            this.PembayaranMainMenu,
            this.SistemMainMenu,
            this.TetapanMainMenu,
            this.LaporanMainMenu,
            this.LogMainMenu,
            this.SambunganDbMenu,
            this.AboutMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.MdiWindowListItem = this.LaporanMainMenu;
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MainMenu.Size = new System.Drawing.Size(1249, 28);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "MenuStrip";
            // 
            // LoginMainMenu
            // 
            this.LoginMainMenu.Image = global::SPA.Properties.Resources.sign_up;
            this.LoginMainMenu.Name = "LoginMainMenu";
            this.LoginMainMenu.Size = new System.Drawing.Size(74, 24);
            this.LoginMainMenu.Text = "Login";
            this.LoginMainMenu.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // KeanggotaanMainMenu
            // 
            this.KeanggotaanMainMenu.Image = global::SPA.Properties.Resources.Search_Male_User;
            this.KeanggotaanMainMenu.Name = "KeanggotaanMainMenu";
            this.KeanggotaanMainMenu.Size = new System.Drawing.Size(126, 24);
            this.KeanggotaanMainMenu.Text = "Keanggotaan";
            this.KeanggotaanMainMenu.Click += new System.EventHandler(this.keanggotaanToolStripMenuItem_Click);
            // 
            // PembayaranMainMenu
            // 
            this.PembayaranMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PembayaranSenaraiMenuItem,
            this.ImpotPembayaranMenuItem,
            this.padamRekodPenerimaanToolStripMenuItem});
            this.PembayaranMainMenu.Image = global::SPA.Properties.Resources.payment_card;
            this.PembayaranMainMenu.Name = "PembayaranMainMenu";
            this.PembayaranMainMenu.Size = new System.Drawing.Size(115, 24);
            this.PembayaranMainMenu.Text = "Penerimaan";
            this.PembayaranMainMenu.Click += new System.EventHandler(this.PembayaranMainMenu_Click);
            // 
            // PembayaranSenaraiMenuItem
            // 
            this.PembayaranSenaraiMenuItem.Name = "PembayaranSenaraiMenuItem";
            this.PembayaranSenaraiMenuItem.Size = new System.Drawing.Size(252, 24);
            this.PembayaranSenaraiMenuItem.Text = "Senarai Penerimaan";
            this.PembayaranSenaraiMenuItem.Click += new System.EventHandler(this.pambayaranManualToolStripMenuItem_Click);
            // 
            // ImpotPembayaranMenuItem
            // 
            this.ImpotPembayaranMenuItem.Name = "ImpotPembayaranMenuItem";
            this.ImpotPembayaranMenuItem.Size = new System.Drawing.Size(252, 24);
            this.ImpotPembayaranMenuItem.Text = "Impot Penerimaan";
            this.ImpotPembayaranMenuItem.Click += new System.EventHandler(this.impotPembayaranToolStripMenuItem_Click);
            // 
            // padamRekodPenerimaanToolStripMenuItem
            // 
            this.padamRekodPenerimaanToolStripMenuItem.Name = "padamRekodPenerimaanToolStripMenuItem";
            this.padamRekodPenerimaanToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.padamRekodPenerimaanToolStripMenuItem.Text = "Padam Rekod Penerimaan";
            this.padamRekodPenerimaanToolStripMenuItem.Click += new System.EventHandler(this.padamRekodPenerimaanToolStripMenuItem_Click);
            // 
            // SistemMainMenu
            // 
            this.SistemMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TukarKatalaluanMenuItem,
            this.PengurusanPenggunaMenuItem});
            this.SistemMainMenu.Image = global::SPA.Properties.Resources.item_configuration;
            this.SistemMainMenu.Name = "SistemMainMenu";
            this.SistemMainMenu.Size = new System.Drawing.Size(81, 24);
            this.SistemMainMenu.Text = "Sistem";
            // 
            // TukarKatalaluanMenuItem
            // 
            this.TukarKatalaluanMenuItem.Name = "TukarKatalaluanMenuItem";
            this.TukarKatalaluanMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TukarKatalaluanMenuItem.Text = "Tukar Kata Laluan";
            this.TukarKatalaluanMenuItem.Click += new System.EventHandler(this.tukarPasswordToolStripMenuItem_Click);
            // 
            // PengurusanPenggunaMenuItem
            // 
            this.PengurusanPenggunaMenuItem.Name = "PengurusanPenggunaMenuItem";
            this.PengurusanPenggunaMenuItem.Size = new System.Drawing.Size(224, 24);
            this.PengurusanPenggunaMenuItem.Text = "Pengurusan Pengguna";
            this.PengurusanPenggunaMenuItem.Click += new System.EventHandler(this.penggunaToolStripMenuItem_Click);
            // 
            // TetapanMainMenu
            // 
            this.TetapanMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AkaunMenuItem,
            this.KeanggotaanMenuItem});
            this.TetapanMainMenu.Image = global::SPA.Properties.Resources.wheel;
            this.TetapanMainMenu.Name = "TetapanMainMenu";
            this.TetapanMainMenu.Size = new System.Drawing.Size(91, 24);
            this.TetapanMainMenu.Text = "&Tetapan";
            // 
            // AkaunMenuItem
            // 
            this.AkaunMenuItem.Name = "AkaunMenuItem";
            this.AkaunMenuItem.Size = new System.Drawing.Size(167, 24);
            this.AkaunMenuItem.Text = "Penerimaan";
            this.AkaunMenuItem.Click += new System.EventHandler(this.akaunToolStripMenuItem_Click);
            // 
            // KeanggotaanMenuItem
            // 
            this.KeanggotaanMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AgamaMenuItem,
            this.BangsaMenuItem,
            this.HubunganWasiMenuItem,
            this.kewarganegaraanToolStripMenuItem,
            this.NegeriMenuItem,
            this.statusPerkahwinanToolStripMenuItem,
            this.kategoriToolStripMenuItem});
            this.KeanggotaanMenuItem.Name = "KeanggotaanMenuItem";
            this.KeanggotaanMenuItem.Size = new System.Drawing.Size(167, 24);
            this.KeanggotaanMenuItem.Text = "Keanggotaan";
            // 
            // AgamaMenuItem
            // 
            this.AgamaMenuItem.Name = "AgamaMenuItem";
            this.AgamaMenuItem.Size = new System.Drawing.Size(205, 24);
            this.AgamaMenuItem.Text = "Agama";
            this.AgamaMenuItem.Click += new System.EventHandler(this.agamaToolStripMenuItem_Click);
            // 
            // BangsaMenuItem
            // 
            this.BangsaMenuItem.Name = "BangsaMenuItem";
            this.BangsaMenuItem.Size = new System.Drawing.Size(205, 24);
            this.BangsaMenuItem.Text = "Bangsa";
            this.BangsaMenuItem.Click += new System.EventHandler(this.bangsaToolStripMenuItem_Click);
            // 
            // HubunganWasiMenuItem
            // 
            this.HubunganWasiMenuItem.Name = "HubunganWasiMenuItem";
            this.HubunganWasiMenuItem.Size = new System.Drawing.Size(205, 24);
            this.HubunganWasiMenuItem.Text = "Hubungan Wasi";
            this.HubunganWasiMenuItem.Click += new System.EventHandler(this.hubunganWasiToolStripMenuItem_Click);
            // 
            // kewarganegaraanToolStripMenuItem
            // 
            this.kewarganegaraanToolStripMenuItem.Name = "kewarganegaraanToolStripMenuItem";
            this.kewarganegaraanToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.kewarganegaraanToolStripMenuItem.Text = "Kewarganegaraan";
            this.kewarganegaraanToolStripMenuItem.Click += new System.EventHandler(this.kewarganegaraanToolStripMenuItem_Click);
            // 
            // NegeriMenuItem
            // 
            this.NegeriMenuItem.Name = "NegeriMenuItem";
            this.NegeriMenuItem.Size = new System.Drawing.Size(205, 24);
            this.NegeriMenuItem.Text = "Negeri";
            this.NegeriMenuItem.Click += new System.EventHandler(this.negeriToolStripMenuItem_Click);
            // 
            // statusPerkahwinanToolStripMenuItem
            // 
            this.statusPerkahwinanToolStripMenuItem.Name = "statusPerkahwinanToolStripMenuItem";
            this.statusPerkahwinanToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.statusPerkahwinanToolStripMenuItem.Text = "Status Perkahwinan";
            this.statusPerkahwinanToolStripMenuItem.Click += new System.EventHandler(this.statusPerkahwinanToolStripMenuItem_Click);
            // 
            // kategoriToolStripMenuItem
            // 
            this.kategoriToolStripMenuItem.Name = "kategoriToolStripMenuItem";
            this.kategoriToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.kategoriToolStripMenuItem.Text = "Kategori";
            this.kategoriToolStripMenuItem.Click += new System.EventHandler(this.kategoriToolStripMenuItem_Click);
            // 
            // LaporanMainMenu
            // 
            this.LaporanMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LejerPembayaranMenuItem,
            this.lejerPinjamanToolStripMenuItem,
            this.laporanBerdasarkanNegeriToolStripMenuItem,
            this.laporanUntukSKMToolStripMenuItem,
            this.tunggakanPembayaranToolStripMenuItem,
            this.tunggakanPinjamanToolStripMenuItem,
            this.laporanBulananTahunanToolStripMenuItem,
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem});
            this.LaporanMainMenu.Image = global::SPA.Properties.Resources.sales_report;
            this.LaporanMainMenu.Name = "LaporanMainMenu";
            this.LaporanMainMenu.Size = new System.Drawing.Size(91, 24);
            this.LaporanMainMenu.Text = "&Laporan";
            this.LaporanMainMenu.Click += new System.EventHandler(this.windowsMenu_Click);
            // 
            // LejerPembayaranMenuItem
            // 
            this.LejerPembayaranMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LejarPembayaranByAnggotaMenuItem});
            this.LejerPembayaranMenuItem.Name = "LejerPembayaranMenuItem";
            this.LejerPembayaranMenuItem.Size = new System.Drawing.Size(353, 24);
            this.LejerPembayaranMenuItem.Text = "Lejer Pembayaran";
            this.LejerPembayaranMenuItem.Click += new System.EventHandler(this.ArrangeIconsToolStripMenuItem_Click);
            // 
            // LejarPembayaranByAnggotaMenuItem
            // 
            this.LejarPembayaranByAnggotaMenuItem.Name = "LejarPembayaranByAnggotaMenuItem";
            this.LejarPembayaranByAnggotaMenuItem.Size = new System.Drawing.Size(221, 24);
            this.LejarPembayaranByAnggotaMenuItem.Text = "Berdasarkan Anggota";
            this.LejarPembayaranByAnggotaMenuItem.Click += new System.EventHandler(this.berdasarkanAnggotaToolStripMenuItem_Click);
            // 
            // lejerPinjamanToolStripMenuItem
            // 
            this.lejerPinjamanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.berdasarkanAnggotaToolStripMenuItem});
            this.lejerPinjamanToolStripMenuItem.Name = "lejerPinjamanToolStripMenuItem";
            this.lejerPinjamanToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.lejerPinjamanToolStripMenuItem.Text = "Lejer Pinjaman";
            // 
            // berdasarkanAnggotaToolStripMenuItem
            // 
            this.berdasarkanAnggotaToolStripMenuItem.Name = "berdasarkanAnggotaToolStripMenuItem";
            this.berdasarkanAnggotaToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.berdasarkanAnggotaToolStripMenuItem.Text = "Berdasarkan Anggota";
            this.berdasarkanAnggotaToolStripMenuItem.Click += new System.EventHandler(this.berdasarkanAnggotaToolStripMenuItem_Click_1);
            // 
            // laporanBerdasarkanNegeriToolStripMenuItem
            // 
            this.laporanBerdasarkanNegeriToolStripMenuItem.Name = "laporanBerdasarkanNegeriToolStripMenuItem";
            this.laporanBerdasarkanNegeriToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.laporanBerdasarkanNegeriToolStripMenuItem.Text = "Pembayaran Mengikut Negeri";
            this.laporanBerdasarkanNegeriToolStripMenuItem.Click += new System.EventHandler(this.laporanBerdasarkanNegeriToolStripMenuItem_Click);
            // 
            // laporanUntukSKMToolStripMenuItem
            // 
            this.laporanUntukSKMToolStripMenuItem.Name = "laporanUntukSKMToolStripMenuItem";
            this.laporanUntukSKMToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.laporanUntukSKMToolStripMenuItem.Text = "Laporan Pembayaran Untuk SKM";
            this.laporanUntukSKMToolStripMenuItem.Click += new System.EventHandler(this.laporanUntukSKMToolStripMenuItem_Click);
            // 
            // tunggakanPembayaranToolStripMenuItem
            // 
            this.tunggakanPembayaranToolStripMenuItem.Name = "tunggakanPembayaranToolStripMenuItem";
            this.tunggakanPembayaranToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.tunggakanPembayaranToolStripMenuItem.Text = "Tunggakan Pembayaran";
            this.tunggakanPembayaranToolStripMenuItem.Click += new System.EventHandler(this.tunggakanPembayaranToolStripMenuItem_Click);
            // 
            // tunggakanPinjamanToolStripMenuItem
            // 
            this.tunggakanPinjamanToolStripMenuItem.Name = "tunggakanPinjamanToolStripMenuItem";
            this.tunggakanPinjamanToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.tunggakanPinjamanToolStripMenuItem.Text = "Tunggakan Pinjaman";
            this.tunggakanPinjamanToolStripMenuItem.Click += new System.EventHandler(this.tunggakanPinjamanToolStripMenuItem_Click);
            // 
            // laporanBulananTahunanToolStripMenuItem
            // 
            this.laporanBulananTahunanToolStripMenuItem.Name = "laporanBulananTahunanToolStripMenuItem";
            this.laporanBulananTahunanToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.laporanBulananTahunanToolStripMenuItem.Text = "Pembayaran Bulanan / Tahunan";
            this.laporanBulananTahunanToolStripMenuItem.Click += new System.EventHandler(this.laporanBulananTahunanToolStripMenuItem_Click);
            // 
            // LogMainMenu
            // 
            this.LogMainMenu.Image = global::SPA.Properties.Resources.business_info;
            this.LogMainMenu.Name = "LogMainMenu";
            this.LogMainMenu.Size = new System.Drawing.Size(62, 24);
            this.LogMainMenu.Text = "Log";
            this.LogMainMenu.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // SambunganDbMenu
            // 
            this.SambunganDbMenu.Image = global::SPA.Properties.Resources.Database_Upload2;
            this.SambunganDbMenu.Name = "SambunganDbMenu";
            this.SambunganDbMenu.Size = new System.Drawing.Size(224, 24);
            this.SambunganDbMenu.Text = "Sambungan Pengkalan Data";
            this.SambunganDbMenu.Click += new System.EventHandler(this.SambunganDbMenu_Click);
            // 
            // AboutMenu
            // 
            this.AboutMenu.Image = global::SPA.Properties.Resources.distributor_report;
            this.AboutMenu.Name = "AboutMenu";
            this.AboutMenu.Size = new System.Drawing.Size(129, 24);
            this.AboutMenu.Text = "Tentang Kami";
            this.AboutMenu.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblCurrentUser,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel2,
            this.lblUserType});
            this.statusStrip.Location = new System.Drawing.Point(0, 512);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1249, 23);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Navy;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 18);
            this.toolStripStatusLabel1.Text = "Maklumat login:";
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.ActiveLinkColor = System.Drawing.Color.Yellow;
            this.lblCurrentUser.BackColor = System.Drawing.SystemColors.Control;
            this.lblCurrentUser.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblCurrentUser.Image = global::SPA.Properties.Resources.Add_Male_User;
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(29, 18);
            this.lblCurrentUser.Text = "-";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 18);
            // 
            // lblUserType
            // 
            this.lblUserType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(13, 18);
            this.lblUserType.Text = "-";
            // 
            // laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem
            // 
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem.Name = "laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem";
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem.Size = new System.Drawing.Size(353, 24);
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem.Text = "Baki Pinjaman Berdasarkan Tarikh Bersara";
            this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem.Click += new System.EventHandler(this.laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 535);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Sistem Pendaftaran Anggota (SPA)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem TetapanMainMenu;
        private System.Windows.Forms.ToolStripMenuItem LaporanMainMenu;
        private System.Windows.Forms.ToolStripMenuItem LejerPembayaranMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem KeanggotaanMainMenu;
        private System.Windows.Forms.ToolStripMenuItem PembayaranMainMenu;
        private System.Windows.Forms.ToolStripMenuItem PembayaranSenaraiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImpotPembayaranMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AkaunMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KeanggotaanMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AgamaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BangsaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SistemMainMenu;
        private System.Windows.Forms.ToolStripMenuItem PengurusanPenggunaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NegeriMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HubunganWasiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LejarPembayaranByAnggotaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogMainMenu;
        private System.Windows.Forms.ToolStripMenuItem TukarKatalaluanMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoginMainMenu;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentUser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblUserType;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripMenuItem kewarganegaraanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusPerkahwinanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SambunganDbMenu;
        private System.Windows.Forms.ToolStripMenuItem AboutMenu;
        private System.Windows.Forms.ToolStripMenuItem kategoriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lejerPinjamanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem berdasarkanAnggotaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem padamRekodPenerimaanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laporanBerdasarkanNegeriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laporanUntukSKMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tunggakanPembayaranToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tunggakanPinjamanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laporanBulananTahunanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laporanTarikhBersaraDanBakiPinjamanToolStripMenuItem;
    }
}



