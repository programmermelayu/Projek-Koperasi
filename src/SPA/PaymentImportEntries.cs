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
using SPA.Cache;
using SPA.Core;
using SPA.Entity.Client;
using SPA.Control;

namespace SPA
{
    public partial class PaymentImportEntries : SPAForm 
    {
        public PaymentImportEntries()
        {
            InitializeComponent();
        }

        private class PaymentRecord
        {
            public string Nama_Ahli { get; set; }
            public string No_KP_Baru { get; set; }
            public string Kod_Akaun  { get; set; }
            public string Nama_Akaun { get; set; }
            public string Bayaran { get; set; }
            public string Faedah { get; set; }
            public int Bulan_Bayaran { get; set; }
            public int Tahun_Bayaran { get; set; }
            public string Kod_Koperasi { get; set; }
            public string Kod_Majikan { get; set; }
            public string Nombor_Larian { get; set; }
            public string Nombor_Baucer { get; set; }
        }

        private List<PaymentRecord> _importedPayments;
        private List<PaymentRecord>  ImportedPayments { 
            get
            {
                if (_importedPayments == null)
                {
                    _importedPayments = new List<PaymentRecord>();
                    
                }
                return _importedPayments;
            }
            set
            {
                _importedPayments = value;
            }
        }

        private List<PaymentRecord> _existingPayments;
        private List<PaymentRecord> ExistingPayments
        {
            get
            {
                if (_existingPayments == null)
                {
                    _existingPayments = new List<PaymentRecord>();

                }
                return _existingPayments;
            }
            set
            {
                _existingPayments = value;
            }
        }

        private List<PaymentRecord> _failedPayments;
        private List<PaymentRecord> FailedPayments
        {
            get
            {
                if (_failedPayments == null)
                {
                    _failedPayments = new List<PaymentRecord>();

                }
                return _failedPayments;
            }
            set
            {
                _failedPayments = value;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
          
        }
        public List<Payment> Payments { get; set; }
        private int recordCount = 0;
        private void InitGrid()
        {
            DataGridBoolColumn colSelec = new DataGridBoolColumn();
            DataGridViewColumn colName = new DataGridViewColumn();
            DataGridViewColumn colNoKPBaru = new DataGridViewColumn();
            DataGridViewColumn colAccountCode = new DataGridViewColumn();
            DataGridViewColumn colAmount = new DataGridViewColumn();
        }

        private void PaymentImportEntries_Load(object sender, EventArgs e)
        {
            txtFile.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int failureCount = 0;
            ExistingPayments = new List<PaymentRecord>();
            foreach (var payment in this.Payments)
            {                
                if (payment.Create())
                {
                    successCount++;               
                }
                else
                {
                    failureCount++;
                }

                if (payment.ExistingPayments.Count > 0) AddExistingRecords(payment);

                UpdateMemberStatus(payment.MemberID);
            }
           
            dgPaymentsExist.DataSource = ExistingPayments;
            ResetDatagridViewColumn(dgPaymentsExist);

            if (dgPaymentsExist.RowCount > 0)
            {
                ShowMessage(dgPaymentsExist.RowCount.ToString() + " rekod telah wujud dikesan. Sila kemaskini rekod ini secara manual.", MessageFor.Instruction);
                tabControl1.SelectedIndex = 1;
            }
            else if (failureCount < 1)
            {
                ShowMessage("Semua rekod telah berjaya disimpan.", MessageFor.Information);
            }
            else if (successCount == 0)
            {
                ShowMessage("Semua rekod gagal disimpan.", MessageFor.Error);
            }
            
            if(failureCount > 0)
            {
                ShowMessage("Sebahagian rekod gagal disimpan. Sila lihat log fail.", MessageFor.Warning);
            }
        }

        private void btnImportFile_Click(object sender, EventArgs e)
        {
            this.Payments = new List<Payment>();
            this.ImportedPayments = new List<PaymentRecord>();
            FileBrowser.InitialDirectory = new PaymentExtractor().DefaultDirectory;
            FileBrowser.DefaultExt = "*.txt";
            if (FileBrowser.ShowDialog() == DialogResult.OK)
            {
                recordCount = 0;
                txtFile.Text = FileBrowser.FileName;
                this.ProcessSelectedFile(FileBrowser.FileName);
                dgPayments.DataSource = this.ImportedPayments;
               
                lblCount.Text = recordCount.ToString() + " rekod bayaran berjaya diimpot!";
                lblCount.Visible = true;
            }

            ResetDatagridViewColumn(dgPayments);
        }
        private bool ProcessSelectedFile(string fileName)
        {
            this.Cursor = Cursors.WaitCursor;
            PaymentExtractor extractor = new PaymentExtractor(fileName);
            if (extractor.Extract())
            {
                this.Payments.AddRange(extractor.Payments);
                foreach (var payment in extractor.Payments)
                {
                    foreach (var paymentDetail in payment.PaymentDetails)
                    {
                        ImportedPayments.Add(CreateImportedPaymentRecord(payment, paymentDetail));
                        this.recordCount += 1;
                    }
                }
            }
            else
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(extractor.ErrorMessage, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            this.Cursor = Cursors.Default;
            return true;
        }
        
        private void btnImportDir_Click(object sender, EventArgs e)
        {
            //todo: wait cursor should change to progress bar with percentage
            this.Payments = new List<Payment>();
            this.ImportedPayments = new List<PaymentRecord>();
            try
            {
                string[] files = System.IO.Directory.GetFiles(new PaymentExtractor().DefaultDirectory);
                this.recordCount = 0;
                foreach (var file in files)
                {
                    if (!file.EndsWith("pdf"))
                    {
                        if (!this.ProcessSelectedFile(file))
                        {
                            this.Cursor = Cursors.Default;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHandler.WriteError(ex);
            }
            finally
            {
                dgPayments.DataSource = this.ImportedPayments;
                lblCount.Text = recordCount.ToString() + " rekod bayaran berjaya diimpot!"; 
            }
                 
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void SetRowNumber(DataGridView dgView)
        {
            foreach (DataGridViewRow row in dgView.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }
        
        private PaymentRecord CreateExistingPaymentRecord(Payment.PaymentRecord record)
        {
            PaymentRecord ps = new PaymentRecord();
            ps.Nama_Ahli = record.MemberName;
            ps.No_KP_Baru = record.NoKPBaru;
            ps.Nombor_Larian = record.NoLarian;
            ps.Kod_Majikan = record.KodMajikan;
            ps.Kod_Koperasi = record.KodKoperasi;
            ps.Nombor_Baucer = record.NoBaucer;
            ps.Kod_Akaun = record.AccountCode;
            ps.Nama_Akaun = AccountCache.GetAccountDescription(record.AccountID);
            ps.Bayaran = record.Amount.ToString().ShowTo2Decimal();
            ps.Bulan_Bayaran = record.PaymentMonth;
            ps.Tahun_Bayaran = record.PaymentYear;
            ps.Faedah = record.Interest.ToString().ShowTo2Decimal();
            return ps;
        }

        private PaymentRecord CreateImportedPaymentRecord(Payment payment,  PaymentDetail paymentDetail)
        {
            PaymentRecord ps = new PaymentRecord();
            ps.Nama_Ahli = payment.MemberName;
            ps.No_KP_Baru = payment.NoKPBaru;
            ps.Nombor_Larian = payment.NoLarian;
            ps.Kod_Majikan = payment.KodMajikan;
            ps.Kod_Koperasi = payment.KodKoperasi;
            ps.Nombor_Baucer = payment.NoBaucer;
            ps.Kod_Akaun = paymentDetail.AccountCode;
            ps.Nama_Akaun = AccountCache.GetAccountDescription(paymentDetail.AccountID);
            ps.Bayaran = paymentDetail.Amount.ToString().ShowTo2Decimal();
            ps.Bulan_Bayaran = paymentDetail.PaymentMonth;
            ps.Tahun_Bayaran = paymentDetail.PaymentYear;
            ps.Faedah = paymentDetail.Interest.ToString().ShowTo2Decimal();
            return ps;
        }

        private void AddExistingRecords(Payment payment)
        {
            foreach (var record in payment.ExistingPayments)
            {
                this.ExistingPayments.Add(this.CreateExistingPaymentRecord(record));
            }  
        }

        private void ResetDatagridViewColumn(DataGridView dgView)
        {
            if (dgView.RowCount > 0)
            {
                DataGridViewColumn colNamaAhli = dgView.Columns["Nama_Ahli"];
                colNamaAhli.HeaderText = "Nama Ahli";
                colNamaAhli.Width = 300;

                DataGridViewColumn colNamaAkaun = dgView.Columns["Nama_Akaun"];
                colNamaAkaun.HeaderText = "Nama Akaun";
                colNamaAkaun.Width = 250;

                DataGridViewColumn colKP = dgView.Columns["No_KP_Baru"];
                colKP.HeaderText = "No MyKad";
                colKP.Width = 100;

                dgView.Columns["Bayaran"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgView.Columns["Faedah"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgView.Columns["Bulan_Bayaran"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgView.Columns["Tahun_Bayaran"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgView.RowHeadersWidth = 50;
                SetRowNumber(dgView);
            }
         

        }

        private void UpdateMemberStatus(int memberId)
        {
            var member = new Member();
            member.ID = memberId;
            member.UpdateStatus();
        }
    }
}
