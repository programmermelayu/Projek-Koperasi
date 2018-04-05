using SPA.Cache;
using SPA.Core;
using SPA.Data;
using SPA.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SPA.Reporting
{
    public class LoanLedgerByMemberData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<LoanLedgerByMemberDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }
        private double HeaderBaseBalance { get; set; }
        private double HeaderInterestBalance { get; set; }
        private double YearlyBaseBalance { get; set; }
        private double YearlyInteresetBalance { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            while (dr.Read())  
            {
                LoanLedgerByMemberDataField record = new LoanLedgerByMemberDataField();
                record.NomborLarian = dr.GetString(dr.GetOrdinal("NomborLarian"));
                int month = dr.GetValueInt("PaymentMonth");
                int year = dr.GetValueInt("PaymentYear");
                record.CreationDate = dr.GetValueDate("syscreated").ToShortDateString().ToDateMalayFormatted(false);
                record.BulanPotongan = string.Format("{0:MMM-yyyy}", new DateTime(year, month, 1));
                record.Base  = dr.GetValueDouble("Base");
                record.Interest = dr.GetValueDouble("Interest");
                record.MonthlyTotal = record.Base + record.Interest;
                
                HeaderBaseBalance -= record.Base;
                HeaderInterestBalance -= record.Interest;
                record.BaseBalance = HeaderBaseBalance;
                record.InterestBalance = HeaderInterestBalance;
                record.MonthlyTotalBalance = record.BaseBalance + record.InterestBalance;

                record.YearlyBaseBalance = record.BaseBalance;
                record.YearlyInterestBalance = record.InterestBalance;
                YearlyBaseBalance = record.YearlyBaseBalance;
                YearlyInteresetBalance = record.YearlyInterestBalance;

                Records.Add(record);    
            }
         
        }
 
        public void GenerateReportDetailData()
        {
            Records = new List<LoanLedgerByMemberDataField>();
            LoanLedgerByMemberDataField record = new LoanLedgerByMemberDataField();
            ReportFilter reportFilter = (ReportFilter)Filter;            
             
            record.Base = 0.0;  
            record.Interest = 0.0;

            List<double> previousYearBalance = GetPreviousYearBalance(reportFilter);
            record.BulanPotongan = "Baki tahun " + (reportFilter.SelectedYear - 1).ToString();
            record.BaseBalance = HeaderBaseBalance = previousYearBalance[00];
            record.InterestBalance = HeaderInterestBalance = previousYearBalance[1];
            record.MonthlyTotalBalance = record.BaseBalance + record.InterestBalance;
     
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
                    record = new LoanLedgerByMemberDataField();
                    record.NomborLarian = string.Empty;
                    record.BulanPotongan = string.Format("{0:MMM-yyyy}", new DateTime(reportFilter.SelectedYear, i, 1));
                    record.Base = 0.0;
                    record.Interest = 0.0;
                    record.BaseBalance = YearlyBaseBalance;
                    record.InterestBalance = YearlyInteresetBalance;
                    record.MonthlyTotalBalance = YearlyBaseBalance + YearlyInteresetBalance;
                    record.YearlyBaseBalance = YearlyBaseBalance;
                    record.YearlyInterestBalance = YearlyInteresetBalance;

                    //CR on March 2018, do not show monthly record if there's no transaction (record.Base and record.Interest) in that month
                    //Records.Add(record);
                }           
            }            
        }

        private List<double>  GetPreviousYearBalance(ReportFilter reportFilter)
         {
             List<double> balances = new List<double>();
             double baseBalance = 0;
             double interestBalance = 0;
             List<double> balanceIn2014 = GetBalanceYear2014(reportFilter.MemberID, (int)reportFilter.LoanType);
             if (balanceIn2014.Count > 1)
             {
                 baseBalance = balanceIn2014[0];
                 interestBalance = balanceIn2014[1];
             }
             else
             {
                 balances.Add(baseBalance);
                 balances.Add(interestBalance);                 
                 return balances;
             }

            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("SUM(Amount) AS TotalBase, SUM(Interest) AS TotalInterest");
            sb.Table = "Payments";
            sb.AddWhereField("AccountID", (int)reportFilter.LoanType, Enums.Data.DataType.Number);
            sb.AddWhereField("(YearMonth", 201501, GetPreviousYearDecember(reportFilter, 1));
            sb.AddWhereField("memberID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);

            SqlDataReader dr = DbReader.Execute(sb.Sql);
            {
                if (dr.HasRecord())
                {
                    while (dr.Read())
                    {
                        balances.Add(baseBalance - dr.GetValueDouble("TotalBase"));
                        balances.Add(interestBalance - dr.GetValueDouble("TotalInterest"));
                    }
                }
                else
                {
                    balances.Add(baseBalance);
                    balances.Add(interestBalance);
                }
            }
            return balances;
         }

        private List<double> GetBalanceYear2014(int memberId, int loanType)
        {
            List<double> amounts = new List<double>();
            SelectBuilder sb = new SelectBuilder();
            sb.Top = 1;
            sb.Fields.Add("BaseBalance, InterestBalance");
            sb.Table = "MembersLoan";
            sb.AddWhereField("memberID", memberId, SPA.Enums.Data.DataType.Number);
            sb.AddWhereField("TypeID", loanType, Enums.Data.DataType.Number);
            sb.OrderBy = "syscreated DESC";

            SqlDataReader dr = DbReader.Execute(sb.Sql);

            {
                if (dr.HasRecord())
                {
                    while (dr.Read())
                    {
                        amounts.Add(dr.GetValueDouble("BaseBalance"));
                        amounts.Add(dr.GetValueDouble("InterestBalance"));
                    }
                }
            }
            return amounts;
        }


        private int GetPreviousYearJanuary(ReportFilter reportFilter, int yearsBackward)
        {
            string startPeriod = GetPreviousYear(reportFilter, yearsBackward).ToString() + "01";
            return startPeriod.ToInt();
        }
        private int GetPreviousYearDecember(ReportFilter reportFilter, int yearsBackward)
        {
            string endPeriod = GetPreviousYear(reportFilter, yearsBackward).ToString() + "12";
            return endPeriod.ToInt();
        }
        private int GetPreviousYear(ReportFilter reportFilter, int yearsBackward)
        {
            return reportFilter.SelectedYear - yearsBackward;
        }

         public void GenerateReportHeaderData(SqlDataReader dr)
        {
            ReportHeader = new ReportHeaderElement();
            ReportFilter reportFilter = (ReportFilter)Filter;  
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    ReportHeader.MembershipNo = dr.GetValueString("Code");
                    ReportHeader.Name = dr.GetValueString("Name");
                    ReportHeader.NomborKP = dr.GetValueString("NewIC");
                    ReportHeader.MemberStatus = (dr.GetValueInt("StatusID") == 0 ? "Tidak Aktif" : "Aktif");
                    ReportHeader.Address = dr.GetValueString("PermanentAddress") + "," +
                                           dr.GetValueString("PermanentPostcode") + "," +
                                           dr.GetValueString("PermanentDistrict") + "," +
                                           SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, dr.GetValueInt("PermanentStateID"));

                   ReportHeader.RetiredDate = string.Format("{0:MM-yy}", dr.GetValueDate("OfficeRetiredDate"));
                   ReportHeader.Category = SettingsCache.GetDescription(Enums.SettingEnum.Setting.Category, dr.GetValueInt("CategoryID"));
                   ReportHeader.Base = dr.GetValueDouble("Base");
                   ReportHeader.Interest = dr.GetValueDouble("Interest");
                   ReportHeader.MonthlyTotal = ReportHeader.Base + ReportHeader.Interest;
                   ReportHeader.LoanDuration = dr.GetValueInt("LoanDuration");
                   ReportHeader.LoanDate = string.Format("{0:dd-MM-yyyy}", dr.GetValueDate("LoanDate"));
                   ReportHeader.LoanStartDate = string.Format("{0:MM-yy}", dr.GetValueDate("LoanStartDate"));
                   ReportHeader.LoanEndDate = string.Format("{0:MM-yy}", dr.GetValueDate("LoanEndDate")); 
                   ReportHeader.LoanType = GetLoanStatement(reportFilter.LoanType);
                }
            }
        }

        public string GetLoanStatement(Enums.PaymentEnum.AccountID loanTypeID)
         {
             string desc = "PENYATA PINJAMAN: KOOPYGMB/";
             switch (loanTypeID)
             {
                 case PaymentEnum.AccountID.PinjamanBiasa:
                     desc += "BIASA/";
                     break;
                 case PaymentEnum.AccountID.PinjamanKhas:
                     desc += "KHAS/";
                     break;
                 case PaymentEnum.AccountID.PinjamanMedisihat:
                     desc += "MEDISIHAT/";
                     break;
                 case PaymentEnum.AccountID.PinjamanKecemasan:
                     desc += "KECEMASAN/";
                     break;
             }
             desc += DateTime.Today.Year.ToString();
             return desc;
         }

        public string GetQuery()
        {
             return null; 
        }
        private string GetQuery(int paymentMonth, int paymentYear)
        {
            SelectBuilder sb = new SelectBuilder();
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            sb.Fields.Add("NomborLarian");
            sb.Fields.Add("PaymentMonth");
            sb.Fields.Add("PaymentYear");
            sb.Fields.Add("Amount AS Base");
            sb.Fields.Add("Interest AS Interest");
            sb.Fields.Add("syscreated");
            sb.Table = "Payments p";
          //  sb.Table += string.Format(" INNER JOIN Payments p ON p. p.NomborLarian = p.NomborLarian and p.AccountID={0} and p.PaymentMonth={1} and p.PaymentYear={2} ", (int)reportFilter.LoanType, paymentMonth, paymentYear);
            sb.AddWhereField("memberID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
            sb.AddWhereField("PaymentMonth", paymentMonth, SPA.Enums.Data.DataType.Number);
            sb.AddWhereField("PaymentYear", paymentYear, SPA.Enums.Data.DataType.Number);
            sb.AddWhereField("AccountID", (int)reportFilter.LoanType, SPA.Enums.Data.DataType.Number);

            sb.GroupBy = "p.NomborLarian, p.PaymentMonth, p.PaymentYear, p.Amount, p.Interest, p.YearMonth, syscreated";
            sb.OrderBy = "p.PaymentYear, p.PaymentMonth";

            return sb.Sql;
        }

        public string GetQueryHeader()
        {
            var sb = new SelectBuilder();
            sb.Table = "Members m";
            sb.Fields.Add("m.Code as Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.StatusID");
            sb.Fields.Add("m.MembershipDate");
            sb.Fields.Add("m.PermanentAddress");
            sb.Fields.Add("m.PermanentPostcode");
            sb.Fields.Add("m.PermanentDistrict");
            sb.Fields.Add("m.PermanentStateID");
            sb.Fields.Add("m.OfficeRetiredDate");
            sb.Fields.Add("m.CategoryID");

            sb.Fields.Add("ml.Base");
            sb.Fields.Add("ml.Interest");
            sb.Fields.Add("ml.LoanDate");
            sb.Fields.Add("ml.LoanDuration");
            sb.Fields.Add("ml.LoanStartDate");
            sb.Fields.Add("ml.LoanEndDate");

            ReportFilter reportFilter = (ReportFilter)this.Filter;
            sb.AddWhereField("m.ID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
            sb.Table += string.Format(" LEFT OUTER JOIN MembersLoan ml ON ml.memberID = m.ID AND ml.TypeID = {0}", (int)reportFilter.LoanType);

            return sb.Sql;                 
        }
        public class ReportFilter : IReportFilter 
        {
            public int MemberID { get; set; }
            public int SelectedYear { get; set; }
            public double Base { get; set; }
            public double BaseBalance { get; set; }
            public double Interest { get; set; }
            public double InterestBalance { get; set; }
            public Enums.PaymentEnum.AccountID LoanType { get; set; }
        }

        public class ReportHeaderElement : IReportHeader 
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string NomborKP { get; set; }
            public string RetiredDate { get; set; }
            public string MembershipNo { get; set; }
            public string MemberStatus { get; set; }
            public string Category { get; set; }
            public double Base { get; set; }
            public double Interest { get; set; }
            public double MonthlyTotal { get; set; }
            public int LoanDuration { get; set; }
            public string LoanDate { get; set; }
            public string LoanStartDate { get; set; }
            public string LoanEndDate { get; set; }
            public string LoanType { get; set; }

            //public string SubTotalCaption { get; set; }
            //public string GrandTotalCaption { get; set; }
        }



        public void GenerateReportHeaderData()
        {
            throw new NotImplementedException();
        }
    }

    public class LoanLedgerByMemberDataField
    {
        public string NomborLarian { get; set; }
        public string BulanPotongan { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public double Base { get; set; }
        public double Interest { get; set; }
        public double BaseBalance { get; set; }
        public double InterestBalance { get; set; }
        public double MonthlyTotal { get; set; }
        public double MonthlyTotalBalance { get; set; }
        public string CreationDate { get; set; }
        public double YearlyBaseBalance { get; set; }
        public double YearlyInterestBalance { get; set; }
    }

}

