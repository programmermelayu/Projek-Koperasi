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
    public class LedgerByMemberData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<LedgerByMemberDataField> Records { get; set;}
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            while (dr.Read())  
            {
                    LedgerByMemberDataField record = new LedgerByMemberDataField();
                    record.NomborLarian = dr.GetString(dr.GetOrdinal("NomborLarian"));
                    int month = dr.GetValueInt("PaymentMonth");
                    int year = dr.GetValueInt("PaymentYear");
                    record.BulanPotongan = string.Format("{0:MMM-yyyy}", new DateTime(year, month, 1));
                    record.FiMasuk = dr.GetValueDouble("FiMasuk");
                    record.Saham = dr.GetValueDouble("Saham");
                    record.YuranBulanan = dr.GetValueDouble("YuranBulanan");
                    record.TabungDerma = dr.GetValueDouble("TabungDerma");
                    record.SimpananKhas = dr.GetValueDouble("SimpananKhas");
                    record.Jumlah = record.FiMasuk + record.Saham + record.YuranBulanan + record.TabungDerma;
                    record.GeneratedOn = String.Format("{0:yyyy/MM/dd HH:mm:ss} ", DateTime.Now);
                    Records.Add(record);
                }
         
        }
 
        public void GenerateReportDetailData()
        {
            Records = new List<LedgerByMemberDataField>();
            var record = new LedgerByMemberDataField();
            var reportFilter = (ReportFilter)Filter;
            List<double> balanceIn2014 = null;

            //values store in table [PaymentsTotal] is for 2014, 
            //balance for the year 2015 onward will be calculated from table [Payments]
            balanceIn2014 = GetBalanceYear2014(reportFilter.MemberID);

            record.BulanPotongan = "Baki tahun " + (reportFilter.SelectedYear - 1).ToString();
            record.YuranBulanan = (reportFilter.SelectedYear == 2015) ? balanceIn2014[0] : GetPreviousYearAmount(PaymentEnum.AccountID.YuranBulanan, reportFilter) + balanceIn2014[0];
            record.TabungDerma = (reportFilter.SelectedYear == 2015) ? balanceIn2014[1] : GetPreviousYearAmount(PaymentEnum.AccountID.KebajikanDermasiswa, reportFilter) + balanceIn2014[1];
            record.Saham  = (reportFilter.SelectedYear == 2015) ? balanceIn2014[2]  : GetPreviousYearAmount(PaymentEnum.AccountID.Saham, reportFilter) + balanceIn2014[2];
            record.FiMasuk = (reportFilter.SelectedYear == 2015) ? balanceIn2014[3] : GetPreviousYearAmount(PaymentEnum.AccountID.FiMasuk, reportFilter) + balanceIn2014[3];
            record.SimpananKhas = (reportFilter.SelectedYear == 2015) ? balanceIn2014[4] : GetPreviousYearAmount(PaymentEnum.AccountID.SimpananKhas, reportFilter) + balanceIn2014[4];
          
            record.Jumlah = record.FiMasuk + record.YuranBulanan + record.TabungDerma + record.Saham + record.SimpananKhas;
            Records.Add(record);
            for (int i = 1; i < 13; i++)
            {
                SqlDataReader dr = DbReader.Execute(GetQuery(i, reportFilter.SelectedYear));
                if (dr.HasRecord())
                {
                    GenerateReportDetailData(dr);    
                }
                else
                {
                    record = new LedgerByMemberDataField();
                    record.NomborLarian = string.Empty;
                    record.BulanPotongan = string.Format("{0:MMM-yyyy}", new DateTime(reportFilter.SelectedYear, i, 1));
                    record.FiMasuk = 0.0;
                    record.Saham = 0.0;
                    record.YuranBulanan = 0.0;
                    record.TabungDerma = 0.0;
                    record.SimpananKhas = 0.0;
                    record.Jumlah = 0.00;
                    Records.Add(record);
                }           
            }            
        }

        private double GetPreviousYearAmount(PaymentEnum.AccountID accountID, ReportFilter reportFilter)
         {
             double amount = 0;
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("SUM(Amount) AS Amount");
            sb.Table = "Payments";
            sb.AddWhereField("AccountID", (int)accountID, Enums.Data.DataType.Number);
            sb.AddWhereField("(YearMonth", 201501, GetPreviousYearDecember(reportFilter)); //always get the balance from Jan 2015.
            sb.AddWhereField("memberID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);

            SqlDataReader dr = DbReader.Execute(sb.Sql); 
                
            if (dr.HasRecord())  
            {     
                while (dr.Read()) 
                { 
                    amount = dr.GetValueDouble("Amount");                         
                }  
            }
            
            return amount;
         }

        private List<double> GetBalanceYear2014(int memberId)
        {
            List<double> amounts = new List<double>();
            SelectBuilder sb = new SelectBuilder();
            sb.Top = 1;
            sb.Fields.Add("YuranBulanan, TabungKebajikan, Saham, FiMasuk, SimpananKhas");
            sb.Table = "PaymentsTotal";
            sb.AddWhereField("memberID", memberId, SPA.Enums.Data.DataType.Number);
            sb.OrderBy = "syscreated DESC";

            SqlDataReader dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    amounts.Add(dr.GetValueDouble("YuranBulanan"));
                    amounts.Add(dr.GetValueDouble("TabungKebajikan"));
                    amounts.Add(dr.GetValueDouble("Saham"));
                    amounts.Add(dr.GetValueDouble("FiMasuk"));
                    amounts.Add(dr.GetValueDouble("SimpananKhas"));
                }
            }
            else
            {
                amounts.Add(0.0);
                amounts.Add(0.0);
                amounts.Add(0.0);
                amounts.Add(0.0);
                amounts.Add(0.0);
            }
            return amounts;
        }

        private int GetPreviousYearJanuary(ReportFilter reportFilter)
        {
            string startPeriod = GetPreviousYear(reportFilter).ToString() + "01";
            return startPeriod.ToInt();
        }
        private int GetPreviousYearDecember(ReportFilter reportFilter)
        {
            string endPeriod = GetPreviousYear(reportFilter).ToString() + "12";
            return endPeriod.ToInt();
        }
        private int GetPreviousYear(ReportFilter reportFilter)
        {
            //DateTime currentDate = reportFilter.SelectedMonth;
            //DateTime lastyearDate = currentDate.AddYears(-1);
            return reportFilter.SelectedYear -1;
        }

         public void GenerateReportHeaderData(SqlDataReader dr)
        {
            ReportHeader = new ReportHeaderElement();
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    ReportHeader.Name = dr.GetString(dr.GetOrdinal("Name"));
                    ReportHeader.NomborKP = dr.GetString(dr.GetOrdinal("NewIC"));
                    ReportHeader.Address = dr.GetValueString("PermanentAddress") + "," +
                                 dr.GetValueString("PermanentPostcode") + "," +
                                 dr.GetValueString("PermanentDistrict") + "," +
                                 SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, dr.GetValueInt("PermanentStateID"));
                    ReportHeader.MembershipNo = dr.GetString(dr.GetOrdinal("Code"));
                    ReportHeader.MembershipDate = string.Empty;
                    if (dr.GetValueDate("MembershipDate").Year != 2999)
                    {
                        ReportHeader.MembershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateFormatted().ToDateMalayFormatted(true).ToUpper();
                    }
                }
            }
        }

        public string GetQuery()
        {
             return null; 
        }
        private string GetQuery(int paymentMonth, int paymentYear)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("p.NomborLarian");
            sb.Fields.Add("p.PaymentMonth");
            sb.Fields.Add("p.PaymentYear");
            sb.Fields.Add("p1.Amount AS YuranBulanan");
            sb.Fields.Add("p2.Amount AS TabungDerma");
            sb.Fields.Add("p3.Amount AS Saham");
            sb.Fields.Add("p4.Amount AS FiMasuk");
            sb.Fields.Add("p5.Amount AS SimpananKhas");

            sb.Table = "Payments p";
            sb.Table += string.Format(" LEFT JOIN Payments p1 ON p.NomborLarian = p1.NomborLarian and p1.AccountID={0} and p1.PaymentMonth={1} and p1.PaymentYear={2} and p1.MemberID = p.MemberID", (int)SPA.Enums.PaymentEnum.AccountID.YuranBulanan, paymentMonth, paymentYear);
            sb.Table += string.Format(" LEFT JOIN Payments p2 ON p.NomborLarian = p2.NomborLarian and p2.AccountID={0} and p2.PaymentMonth={1} and p2.PaymentYear={2} and p2.MemberID = p.MemberID", (int)SPA.Enums.PaymentEnum.AccountID.KebajikanDermasiswa, paymentMonth, paymentYear);
            sb.Table += string.Format(" LEFT JOIN Payments p3 ON p.NomborLarian = p3.NomborLarian and p3.AccountID={0} and p3.PaymentMonth={1} and p3.PaymentYear={2} and p3.MemberID = p.MemberID", (int)SPA.Enums.PaymentEnum.AccountID.Saham, paymentMonth, paymentYear);
            sb.Table += string.Format(" LEFT JOIN Payments p4 ON p.NomborLarian = p4.NomborLarian and p4.AccountID={0} and p4.PaymentMonth={1} and p4.PaymentYear={2} and p4.MemberID = p.MemberID", (int)SPA.Enums.PaymentEnum.AccountID.FiMasuk, paymentMonth, paymentYear);
            sb.Table += string.Format(" LEFT JOIN Payments p5 ON p.NomborLarian = p5.NomborLarian and p5.AccountID={0} and p5.PaymentMonth={1} and p5.PaymentYear={2} and p5.MemberID = p.MemberID", (int)SPA.Enums.PaymentEnum.AccountID.SimpananKhas, paymentMonth, paymentYear);

            ReportFilter reportFilter = (ReportFilter)this.Filter;
            if (reportFilter != null)
            {
                if (reportFilter.MemberID > 0)
                {
                    sb.AddWhereField("p.memberID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
                }
                //if (reportFilter.PeriodFrom > 0 && reportFilter.PeriodTo > 0)
                //{
                //    sb.AddWhereField("(p.YearMonth", reportFilter.PeriodFrom, reportFilter.PeriodTo);
                //}
            }

            sb.AddWhereField("p.PaymentMonth", paymentMonth, SPA.Enums.Data.DataType.Number);
            sb.AddWhereField("p.PaymentYear", paymentYear, SPA.Enums.Data.DataType.Number);

            sb.GroupBy = "p.NomborLarian, p.PaymentMonth, p.PaymentYear, p1.amount, p2.Amount, p3.Amount, p4.amount, p5.amount, p.YearMonth";
            sb.OrderBy = "p.PaymentYear, p.PaymentMonth";

            return sb.Sql;
        }

        public string GetQueryHeader()
        {
            var sb = new SelectBuilder();
            sb.Table = "Members";
            sb.Fields.Add("Name");
            sb.Fields.Add("NewIC");
            sb.Fields.Add("PermanentAddress");
            sb.Fields.Add("PermanentPostcode");
            sb.Fields.Add("PermanentDistrict");
            sb.Fields.Add("PermanentStateID");
            sb.Fields.Add("Code");
            sb.Fields.Add("MembershipDate");
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            sb.AddWhereField("ID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
            return sb.Sql;                 
        }
        public class ReportFilter : IReportFilter 
        {
            public int MemberID { get; set; }
            public int SelectedYear { get; set; }
            public int PeriodFrom { get; set; }
            public int PeriodTo { get; set; }
            public DateTime StartMonth { get; set; }
            public DateTime EndMonth { get; set; }
            public double PreviousTotalFiMasuk { get; set; }
            public double PreviousTotalYuranBulanan { get; set; }
            public double PreviousTotalTabungDerma { get; set; }
            public double PreviousTotalSimpananKhas { get; set; }
            public double PreviousTotalSaham { get; set; }
        }

        public class ReportHeaderElement : IReportHeader 
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string NomborKP { get; set; }
            public string MembershipDate { get; set; }
            public string MembershipNo { get; set; }
            public string SubTotalCaption { get; set; }
            public string GrandTotalCaption { get; set; }

        }



        public void GenerateReportHeaderData()
        {
            throw new NotImplementedException();
        }
    }

    public class LedgerByMemberDataField
    {
       public string NomborLarian { get; set; }
        public string BulanPotongan { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public double  FiMasuk { get; set; }
        public double  Saham { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double SimpananKhas { get; set; }
        public double Jumlah { get; set; }
        public string GeneratedOn { get; set; }
    }
}

