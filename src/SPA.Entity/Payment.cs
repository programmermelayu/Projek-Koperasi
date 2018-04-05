using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Data;
using SPA.Core;

namespace SPA.Entity
{
    public class Payment : EntityBase, IEntity 
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int MemberID { get; set; }
        public string NoKPBaru { get; set; }
        public string NoKPLama { get; set; }
        public string KodKoperasi { get; set; }
        public string KodMajikan { get; set; }
        public string NoLarian { get; set; }
        public string NoBaucer { get; set; }
        public string NoResit { get; set; }
        public bool IsExist = false;

        private List<PaymentDetail> _paymentDetails;
        public List<PaymentDetail> PaymentDetails
        {
            get
            {
                if (_paymentDetails ==null)
                {
                    _paymentDetails = new List<PaymentDetail>();
                }
                return _paymentDetails;
            }
            set
            {
                _paymentDetails = value;
            }
        }

        private List<PaymentRecord> _existingPayments;
        public List<PaymentRecord> ExistingPayments
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

        public class PaymentRecord
        {
            public int AccountID { get; set; }
            public int MemberID { get; set; }
            public string MemberCode { get; set; }
            public string MemberName { get; set; }
            public string NoKPBaru { get; set; }
            public string NoKPLama { get; set; }
            public string KodKoperasi { get; set; }
            public string KodMajikan { get; set; }
            public string NoLarian { get; set; }
            public string NoBaucer { get; set; }
            public string NoResit { get; set; }
            public string AccountCode { get; set; }
            public string AccountDescription { get; set; }
            public double Amount { get; set; }
            public double Interest { get; set; }
            public int PaymentMonth { get; set; }
            public int PaymentYear { get; set; }
        }

        public string ErrorMessage { get; set; }

        private string successMessage = "Maklumat berjaya disimpan!";
        
        public string SuccessMessage
        {
            get
            {
                return successMessage;
            }
            set
            {
                successMessage = value;
            }
        }

        public Payment():base("Payments")
        {

        }

        #region IEntity Members
        public bool Delete()
        {
            base.BaseDeleteBuilder.AddWhereField("ID", ID);
            return base.ExecuteDelete();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public bool Create()
        {
            if (!Validate())
            {
                return false;
            }
            else
            {
                if (ID > 0)
                {
                    return Update();
                }
                else
                {
                    return Insert();
                }
            }
           
        }

        /// <summary>
        /// Always delete before insert new record to avoid duplicated records. Best to warn user upfront.
        /// </summary>
        /// <returns>status of fhe execution</returns>
        public bool Insert()
        {
            List<string> sqlCollection = new List<string>();
            ExistingPayments = new List<PaymentRecord>();
            foreach (PaymentDetail paymentDetail in PaymentDetails)
            {
                if (this.MemberID < 1) if (!AssignMemberID()) return false;    

                if (CheckExist(paymentDetail))
                {
                    AddExistingPayment(paymentDetail);
                }
                else
                {
                    BaseInsertBuilder = new InsertBuilder();
                    BaseInsertBuilder.Table = base.TableName;
                    BaseInsertBuilder.AddField("MemberID", this.MemberID, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("AccountID", paymentDetail.AccountID, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("amount", paymentDetail.Amount, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("Interest", paymentDetail.Interest, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("PaymentMonth", paymentDetail.PaymentMonth, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("PaymentYear", paymentDetail.PaymentYear, Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("YearMonth", GetYearMonth(paymentDetail.PaymentMonth, paymentDetail.PaymentYear), Enums.Data.DataType.Number);
                    BaseInsertBuilder.AddField("NomborLarian", this.NoLarian);
                    BaseInsertBuilder.AddField("NomborBaucer", this.NoBaucer);
                    BaseInsertBuilder.AddField("NomborResit", this.NoResit);
                    BaseInsertBuilder.AddField("KodKoperasi", this.KodKoperasi);
                    BaseInsertBuilder.AddField("KodMajikan", this.KodMajikan);
                    sqlCollection.Add(BaseInsertBuilder.Sql);              
                }
            }
            if (sqlCollection.Count > 0)
            {
                return base.ExecuteQueries(sqlCollection);
            }
            else
            {
                return true;
            }           
        }

        public bool Update()
        {
            PaymentDetail paymentDetail = PaymentDetails[0];
            BaseUpdateBuilder = new UpdateBuilder();
            BaseUpdateBuilder.Table = base.TableName;
            BaseUpdateBuilder.AddField("Amount", paymentDetail.Amount, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("Interest", paymentDetail.Interest, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number);
            return base.ExecuteUpdate();
        }

        public string Sql
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ID { get; set; }

        private bool Validate()
        {
        
            return true;
        }

        #endregion

        public static int GetYearMonth(int month, int year)
        {
            string yearMonth;
            yearMonth = year.ToString();
            if (month < 10)
            {
                yearMonth += "0" + month.ToString();
            }
            else
            {
                yearMonth += month.ToString();
            }
            return int.Parse(yearMonth);
        }

        private bool AssignMemberID()
        {
            var member = new Member();
            member.Name = this.MemberName;
            member.NewIC = this.NoKPBaru;
            if (member.CheckExist())
            {
                this.MemberID = member.GetID();
            }
            else
            {
                if (member.Create())
                {
                    this.MemberID = member.GetID();
                }
                else
                {
                    ErrorMessage = string.Format("Maaf, ahli dengan Nombor Kad Pengenalan {0} tidak dapat dicipta.", this.NoKPBaru);
                    return false;
                }
            }
            if (this.MemberID == -1)
            {
                ErrorMessage = string.Format("Transaksi gagal.", this.NoKPBaru);
                return false;                
            }
            return true;
        }

        public override int GetID()
        {
            throw new NotImplementedException();
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            throw new NotImplementedException();
        }

        public bool CheckExist(PaymentDetail paymentDetail)
        {
            if (this.MemberID < 1) return false;

            List<FilterElement> filterCollection = new List<FilterElement>();

            FilterElement filterYearMonth = new FilterElement();
            filterYearMonth.Key = Enums.Data.KeyElements.UseColumnName;
            filterYearMonth.ColumnName = "YearMonth";
            filterYearMonth.Value = GetYearMonth(paymentDetail.PaymentMonth, paymentDetail.PaymentYear);

            FilterElement filterAccountID = new FilterElement();
            filterAccountID.Key = Enums.Data.KeyElements.AccountID;
            filterAccountID.Value = paymentDetail.AccountID;

            FilterElement filterMemberID = new FilterElement();
            filterMemberID.Key = Enums.Data.KeyElements.MemberID;
            filterMemberID.Value = this.MemberID;

            filterCollection.Add(filterYearMonth);
            filterCollection.Add(filterAccountID);
            filterCollection.Add(filterMemberID);
            return new PaymentReader().ReadSingle(filterCollection);
        }

        private void AddExistingPayment(PaymentDetail paymentDetail)
        {
            PaymentRecord paymentRecord = new PaymentRecord();
            paymentRecord.AccountID = paymentDetail.AccountID;
            paymentRecord.MemberID = this.MemberID;
            paymentRecord.MemberName = this.MemberName;
            paymentRecord.NoKPBaru = this.NoKPBaru;
            paymentRecord.KodKoperasi = this.KodKoperasi;
            paymentRecord.KodMajikan = this.KodMajikan;
            paymentRecord.NoLarian = this.NoLarian;
            paymentRecord.NoBaucer = this.NoBaucer;
            paymentRecord.NoResit = this.NoResit;
            paymentRecord.AccountCode = paymentDetail.AccountCode;
            paymentRecord.AccountDescription = paymentDetail.AccountDescription;
            paymentRecord.PaymentMonth = paymentDetail.PaymentMonth;
            paymentRecord.PaymentYear = paymentDetail.PaymentYear;
            paymentRecord.Amount = paymentDetail.Amount;
            paymentRecord.Interest = paymentDetail.Interest;
            ExistingPayments.Add(paymentRecord);
        }

    }



}

