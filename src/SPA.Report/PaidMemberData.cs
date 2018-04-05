using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Data;
using System.Data.SqlClient;
using SPA.Entity;
using SPA.Core;
using SPA.Enums;
using SPA.Cache;

namespace SPA.Reporting
{
    public class PaidMemberData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<PaidMemberDataField> Records { get; set; }
        public static List<PaidMemberLoanDataField> RecordsLoan { get; set; }

        public static ReportHeaderElement ReportHeader { get; set; }

        public enum MemberPaymentCategory
        {
            Paid = 0,
            Unpaid,
            PaidLoan,
            UnpaidLoan
        }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
             ReportHeader = new ReportHeaderElement();
             switch (reportFilter.Category)
             {
                 case MemberPaymentCategory.Paid:
                     GeneratePaidMemberDetailData(dr);
                     break;
                 case MemberPaymentCategory.Unpaid:
                     break;
                 case MemberPaymentCategory.PaidLoan:
                     GeneratePaidLoanMemberDetailData(dr);
                     break;
                 case MemberPaymentCategory.UnpaidLoan:
                     break;
                 default:
                     break;
             }
        }
 
        public void GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

         public void GenerateReportHeaderData()
         {
             ReportFilter reportFilter = (ReportFilter)this.Filter;
             ReportHeader = new ReportHeaderElement();
             switch (reportFilter.Category)
             {
                 case MemberPaymentCategory.Paid:
                     ReportHeader.ReportTitle = "SENARAI ANGGOTA YANG MEMBUAT BAYARAN";
                     break;
                 case MemberPaymentCategory.Unpaid:
                     ReportHeader.ReportTitle = "SENARAI ANGGOTA YANG TIDAK MEMBUAT BAYARAN";
                     break;
                 case MemberPaymentCategory.PaidLoan:
                     ReportHeader.ReportTitle = "SENARAI ANGGOTA YANG MEMBUAT BAYARAN PINJAMAN";
                     break;
                 case MemberPaymentCategory.UnpaidLoan:
                     ReportHeader.ReportTitle = "SENARAI ANGGOTA YANG TIDAK MEMBUAT BAYARAN PINJAMAN";
                     break;
                 default:
                     break;
             }

             ReportHeader.Period = string.Format("{0:MMM-yyyy}", reportFilter.StartMonth) + " - " + string.Format("{0:MMM-yyyy}", reportFilter.EndMonth);
         }

        public string GetQuery()
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            if (reportFilter.Category == MemberPaymentCategory.Paid)
            {
                return GeneratePaidMemberQuery(reportFilter);
            }
            else
            {
              return string.Empty;
            }            
        }

        public string GetQueryHeader()
        {
            return string.Empty;      
        }
        public class ReportFilter : IReportFilter 
        {
            public int MemberID { get; set; }
            public int SelectedYear { get; set; }
            public int PeriodFrom { get; set; }
            public int PeriodTo { get; set; }
            public DateTime StartMonth { get; set; }
            public DateTime EndMonth { get; set; }
            public MemberPaymentCategory Category { get; set; }
        }

        public class ReportHeaderElement : IReportHeader 
        {
            public string ReportTitle { get; set; }
            public string Period { get; set; }
        }

        public void GenerateReportDetailData()
        {
            throw new NotImplementedException();
        }

        private string GeneratePaidMemberQuery(ReportFilter reportFilter)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("m.Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.MembershipDate");
            sb.Fields.Add("SUM(p1.Amount) AS YuranBulanan");
            sb.Fields.Add("SUM(p2.Amount) AS TabungDerma");
            sb.Fields.Add("SUM(p3.Amount) AS Saham");
            sb.Fields.Add("SUM(p4.Amount) AS FiMasuk");
            sb.Fields.Add("SUM(p5.Amount) AS SimpananKhas");
            sb.Table += "Members m";
            sb.Table += string.Format(" LEFT JOIN Payments p1 ON p1.AccountID={0} and p1.YearMonth between {1} and {2} and p1.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.YuranBulanan, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p2 ON p2.AccountID={0} and p2.YearMonth between {1} and {2} and p2.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.KebajikanDermasiswa, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p3 ON p3.AccountID={0} and p3.YearMonth between {1} and {2} and p3.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.Saham, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p4 ON p4.AccountID={0} and p4.YearMonth between {1} and {2} and p4.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.FiMasuk, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p5 ON p5.AccountID={0} and p5.YearMonth between {1} and {2} and p5.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.SimpananKhas, reportFilter.PeriodFrom, reportFilter.PeriodTo);

            sb.GroupBy = "m.Code, m.Name, m.NewIC, m.MembershipDate, p1.Amount, p2.Amount, p3.Amount, p4.Amount, p5.Amount ";
            sb.OrderBy = "m.Code";

            return sb.Sql;

        }

        private string GeneratePaidLoanMemberQuery(ReportFilter reportFilter)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("m.Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.MembershipDate");
            sb.Fields.Add("SUM(p1.Amount) AS Base");
            sb.Fields.Add("SUM(p1.Interest) AS Interest");
            sb.Table += "Members m";
            sb.Table += string.Format(" LEFT JOIN Payments p1 ON p1.AccountID={0} and p1.YearMonth between {1} and {2} and p1.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.PinjamanBiasa, reportFilter.PeriodFrom, reportFilter.PeriodTo);
      
            sb.GroupBy = "m.Code, m.Name, m.NewIC, m.MembershipDate, p1.Amount, p2.Amount, p3.Amount, p4.Amount, p5.Amount ";
            sb.OrderBy = "m.Code";

            return sb.Sql;

        }
      
        private void GeneratePaidMemberDetailData(SqlDataReader dr)
        {
            int count = 0;
            Records = new List<PaidMemberDataField>();
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            while (dr.Read())
            {
                PaidMemberDataField record = new PaidMemberDataField();
                count += 1;
                record.Index = count;
                record.MemberName = dr.GetValueString("Name");
                record.MembershipNo = dr.GetValueString("Code");
                record.MemberKP = dr.GetValueString("NewIC");
                record.MembershipDate = string.Empty;
                if (dr.GetValueDate("MembershipDate").Year != 2999)
                {
                    record.MembershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateFormatted().ToDateMalayFormatted(true).ToUpper();
                }
                record.PaidFiMasuk = dr.GetValueDouble("FiMasuk");
                //record.PaidSaham = dr.GetValueDouble("Saham");
                record.PaidYuranBulanan = dr.GetValueDouble("YuranBulanan");
                record.PaidTabungDerma = dr.GetValueDouble("TabungDerma");
                //record.PaidSimpananKhas = dr.GetValueDouble("SimpananKhas");
                record.PaidJumlah = record.PaidFiMasuk + record.PaidSaham + record.PaidYuranBulanan + record.PaidTabungDerma + record.PaidSimpananKhas;

                var calculator = new PaymentCalculator();
                calculator.MembershipDate = DateTime.Parse(record.MembershipDate);
                calculator.LatestPaymentDate =  reportFilter.EndMonth; //dummy only, will take it from filter

                var expected = calculator.GetExpectedPayment();
                record.UnPaidFiMasuk = expected.ExpectedFiMasuk - record.PaidFiMasuk;
                record.UnPaidYuranBulanan  = expected.ExpectedYuranBulanan - record.PaidYuranBulanan;
                record.UnPaidTabungDerma = expected.ExpectedTabungDerma - record.PaidTabungDerma;


                Records.Add(record);
            }
        }

        private void GeneratePaidLoanMemberDetailData(SqlDataReader dr)
        {

        }
    
    }

    public class PaidMemberDataField
    {
        public int Index { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string MembershipNo { get; set; }
        public double PaidFiMasuk { get; set; }
        public double PaidSaham { get; set; }
        public double PaidYuranBulanan { get; set; }
        public double PaidTabungDerma { get; set; }
        public double PaidSimpananKhas { get; set; }
        public double PaidJumlah { get; set; }
        public double UnPaidFiMasuk { get; set; }
        public double UnPaidSaham { get; set; }
        public double UnPaidYuranBulanan { get; set; }
        public double UnPaidTabungDerma { get; set; }
        public double UnPaidSimpananKhas { get; set; }
        public double UnPaidJumlah { get; set; }
    }

    public class PaidMemberLoanDataField
    {
        public int Index { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string MembershipNo { get; set; }
        public double Base { get; set; }
        public double Interest { get; set; }
        public double Jumlah { get; set; }
    }

}

