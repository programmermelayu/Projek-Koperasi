using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Data;
using SPA.Enums;

namespace SPA.Entity
{
    public class PaymentHistoryReader : IEntityReader<PaymentHistory> 
    {
      
        #region IEntityReader Members

        public string ErrorMessage { get; set; }
        public string SystemErrorMessage { get; set; }
        private PaymentHistory _singleRecord;
        public PaymentHistory SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new PaymentHistory();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        private List<PaymentHistory> _multipleRecord;
        public List<PaymentHistory> MultipleRecords
        {
            get
            {
                if (_multipleRecord == null)
                {
                    _multipleRecord = new List<PaymentHistory>();
                }
                return _multipleRecord;
            }
            set
            {
                _multipleRecord = value;
            }
        }

        public bool ReadSingle(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            if (dr == null)
            {
                ErrorMessage = DbReader.ErrorMessage;
                return false;
            }
            else
            {
                while (dr.Read())
                {
                    SingleRecord.MemberID = dr.GetInt32(dr.GetOrdinal("MemberID"));
                    SingleRecord.MemberName = dr.GetString(dr.GetOrdinal("MemberName"));

                    var yb = new PaymentDetailHistory();
                    yb.AccountCode = "0101";
                    yb.AccountDescription = "Yuran Bulanan";
                    yb.LastPaymentMonth = GetMonth(dr.GetValue(dr.GetOrdinal("YuranBulanan")).ToString());
                    yb.LastPaymentYear = GetYear(dr.GetValue(dr.GetOrdinal("YuranBulanan")).ToString());
                    yb.Outstanding = CalculateOutstanding(yb.LastPaymentMonth, yb.LastPaymentYear);

                    SingleRecord.PaymentsDetailHistory.Add(yb);

                    var kds = new PaymentDetailHistory();
                    kds.AccountCode = "0602";
                    kds.AccountDescription = "Kebajikan dan Dermasiswa";
                    kds.LastPaymentMonth = GetMonth(dr.GetValue(dr.GetOrdinal("KebajikanDermasiswa")).ToString());
                    kds.LastPaymentYear = GetYear(dr.GetValue(dr.GetOrdinal("KebajikanDermasiswa")).ToString());
                    kds.Outstanding = CalculateOutstanding(kds.LastPaymentMonth, kds.LastPaymentYear);

                    SingleRecord.PaymentsDetailHistory.Add(kds);

                    //TODO: add all payment history


                }
                return true;
            }
        }
        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("m.ID as MemberID");
            sb.Fields.Add("m.Name as MemberName");
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS YuranBulanan", 
                           (int)Enums.PaymentEnum.AccountID.YuranBulanan));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS KebajikanDermasiswa",
                           (int)Enums.PaymentEnum.AccountID.YuranBulanan));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS Saham",
                           (int)Enums.PaymentEnum.AccountID.Saham));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS FiMasuk",
                           (int)Enums.PaymentEnum.AccountID.FiMasuk));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS PinjamanBiasa",
                           (int)Enums.PaymentEnum.AccountID.PinjamanBiasa));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS PinjamanKhas",
                           (int)Enums.PaymentEnum.AccountID.PinjamanKhas));
            sb.Fields.Add(String.Format("MAX(CASE WHEN  p.AccountID = {0} THEN p.YearMonth ELSE NULL END) AS PinjamanMedisihat",
                           (int)Enums.PaymentEnum.AccountID.PinjamanMedisihat));

            sb.Table = "Members m";
            sb.Table += " LEFT OUTER JOIN Payments p ON m.ID = p.MemberID";
            sb.GroupBy = "m.ID, m.Name";
            if (filter.Key == Enums.Data.KeyElements.MemberKP)
            {
                sb.AddWhereField("m.NewIC", filter.Value.ToString());
            }
           
            return sb.Sql;
        }

        private string GetQuery(List<FilterElement> filterCollection)
        {
            return null;
        }
        #endregion

        private int GetMonth(string yearMonth)
        {
            return (yearMonth.Length >= 6) ? int.Parse(yearMonth.Remove(0, 4)) : -1;           
        }

        private int GetYear(string yearMonth)
        {
            return (yearMonth.Length >= 6) ? int.Parse(yearMonth.Remove(4, 2)) : -1;
        }

        private double CalculateOutstanding(int lastPaymentMonth, int lastPaymentYear)
        {
            //get the value from AccountValue * number of month not paid (only applied to non-Pinjaman)
            return 0;

        }

        bool IEntityReader<PaymentHistory>.ReadSingle(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        bool IEntityReader<PaymentHistory>.ReadMultiple()
        {
            throw new NotImplementedException();
        }

        bool IEntityReader<PaymentHistory>.ReadMultiple(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        bool IEntityReader<PaymentHistory>.ReadMultiple(FilterElement filter)
        {
            throw new NotImplementedException();
        }


    }
}
