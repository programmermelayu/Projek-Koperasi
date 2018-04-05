using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Data;
using SPA.Core;

namespace SPA.Entity
{
    public class PaymentReader : IEntityReader<Payment>
    {
 
        public string ErrorMessage { get; set; }

        public string SystemErrorMessage { get; set; }

        private Payment _singleRecord;
        public Payment SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new Payment();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        private List<Payment> _multipleRecords;
        public List<Payment> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<Payment>();
                }
                return _multipleRecords;
            }

            set
            {
                _multipleRecords = value;
            }
        }

        public bool ReadSingle(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            return CreateSingleRecordDataReader(dr);   
        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filterCollection));
            return CreateSingleRecordDataReader(dr);   
        }

        public bool ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple(List<FilterElement> filterCollection)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filterCollection));
            return CreateMultipleRecordsDataReader(dr);   
        }

        public bool ReadMultiple(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            return CreateMultipleRecordsDataReader(dr);     
        }

        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            if (filter != null)
            {
                sb.AddWhereField("p.MemberID", new Member().GetID(filter.Key, filter.Value), SPA.Enums.Data.DataType.Number);
            }
            return sb.Sql;
        }

        private string GetQuery(IEnumerable<FilterElement> filterCollection)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            if (filterCollection != null)
            {
                foreach (var filter in filterCollection)
                {
                   switch(filter.Key)
                   {
                       case Enums.Data.KeyElements.MemberCode:
                       case Enums.Data.KeyElements.MemberKP:
                            sb.AddWhereField("p.MemberID", new Member().GetID(filter.Key, filter.Value), SPA.Enums.Data.DataType.Number);
                           break;
                       case Enums.Data.KeyElements.MemberID:
                           sb.AddWhereField("p.MemberID", filter.Value, SPA.Enums.Data.DataType.Number);
                           break;
                       case Enums.Data.KeyElements.PeriodBetweenNumber:
                           sb.AddWhereField("(p.YearMonth", filter.Value, filter.Value2);
                           break;
                       case Enums.Data.KeyElements.AccountID:
                           sb.AddWhereField("p.AccountID", filter.Value);  
                           break;
                       case Enums.Data.KeyElements.UseColumnName:
                           sb.AddWhereField(filter.ColumnName, filter.Value);  
                           break;
                   }
	
                }
            }

            return sb.Sql;
        }

        private bool CreateMultipleRecordsDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    Payment payment = new Payment();
                    payment.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    payment.MemberID = dr.GetString(dr.GetOrdinal("MemberID")).ToInt();

                    payment.MemberCode = dr.GetValueString("MemberCode");
                    payment.MemberName = dr.GetValueString("MemberName");
                  
                    payment.NoLarian = dr.GetString(dr.GetOrdinal("NomborLarian"));

                    PaymentDetail detail = new PaymentDetail();
                    detail.Amount = dr.GetValue(dr.GetOrdinal("Amount")).ToString().ToDouble();
                    detail.PaymentMonth = dr.GetValue(dr.GetOrdinal("PaymentMonth")).ToString().ToInt();
                    detail.PaymentYear = dr.GetValue(dr.GetOrdinal("PaymentYear")).ToString().ToInt();
                    detail.AccountID = dr.GetValue(dr.GetOrdinal("AccountID")).ToString().ToInt();
                    detail.AccountDescription = dr.GetValue(dr.GetOrdinal("AccountDescription")).ToString();
                    detail.Interest = dr.GetValue(dr.GetOrdinal("Interest")).ToString().ToDouble();
                    detail.Syscreated = dr.GetValue(dr.GetOrdinal("Syscreated")).ToString();
                    payment.PaymentDetails.Add(detail);
                    MultipleRecords.Add(payment);
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool CreateSingleRecordDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    SingleRecord.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    SingleRecord.MemberID = dr.GetString(dr.GetOrdinal("MemberID")).ToInt();
                    SingleRecord.NoLarian = dr.GetString(dr.GetOrdinal("NomborLarian"));

                    PaymentDetail detail = new PaymentDetail();
                    detail.Amount = dr.GetValue(dr.GetOrdinal("Amount")).ToString().ToDouble();
                    detail.PaymentMonth = dr.GetValue(dr.GetOrdinal("PaymentMonth")).ToString().ToInt();
                    detail.PaymentYear = dr.GetValue(dr.GetOrdinal("PaymentYear")).ToString().ToInt();
                    detail.AccountID = dr.GetValue(dr.GetOrdinal("AccountID")).ToString().ToInt();
                    detail.AccountDescription = dr.GetValue(dr.GetOrdinal("AccountDescription")).ToString();
                    detail.Interest = dr.GetValue(dr.GetOrdinal("Interest")).ToString().ToDouble();
                    detail.Syscreated = dr.GetValue(dr.GetOrdinal("Syscreated")).ToString();
                    SingleRecord.PaymentDetails.Add(detail);
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        private void CreateQueryHeader(SelectBuilder sb)
        {
            sb.Fields.Add("p.ID");
            sb.Fields.Add("p.AccountID");
            sb.Fields.Add("p.MemberID");
            sb.Fields.Add("p.NomborLarian");
            sb.Fields.Add("p.Amount");
            sb.Fields.Add("a.Description AS AccountDescription");
            sb.Fields.Add("p.PaymentMonth");
            sb.Fields.Add("p.PaymentYear");
            sb.Fields.Add("p.Interest");
            sb.Fields.Add("p.syscreated");
            sb.Fields.Add("m.Code as MemberCode");
            sb.Fields.Add("m.Name as MemberName");
            sb.Table = "Payments p";
            sb.Table += " LEFT JOIN Accounts a ON a.ID = p.AccountID";
            sb.Table += " LEFT JOIN Members m ON m.ID = p.MemberID";
            //sb.AddWhereField("p.AccountID", "a.ID", Enums.Data.DataType.Number);
        }
    }
}
