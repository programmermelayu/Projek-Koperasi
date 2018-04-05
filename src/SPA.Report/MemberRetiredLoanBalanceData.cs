using SPA.Cache;
using SPA.Core;
using SPA.Data;
using SPA.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SPA.Reporting
{
    public class MemberRetiredLoanBalanceData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<MemberRetiredLoanBalanceDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }
        public void GenerateReportDetailData(SqlDataReader dr)
        {
                   
            ReportFilter reportFilter = (ReportFilter)Filter;
            Records = new List<MemberRetiredLoanBalanceDataField>();
            int count = 0;
            while (dr.Read())
            {
                count++;
                MemberRetiredLoanBalanceDataField record = new MemberRetiredLoanBalanceDataField();
                record.Index = count;
                record.MemberId = dr.GetValueInt("ID");
                record.MembershipNo = dr.GetValueString("Code");
                record.Name = dr.GetValueString("Name");
                record.MemberIC = dr.GetValueString("NewIC");
                record.RetiredDate = string.Format("{0:MMM-yyyy}", dr.GetValueDate("OfficeRetiredDate")).ToUpper();

                double baseTotal = dr.GetValueDouble("Base");
                double interestTotal = dr.GetValueDouble("Interest");
                List<double> paidLoan = GetTotalPaidLoan(record.MemberId, (int)reportFilter.LoanType);
                record.BaseTotal = baseTotal;
                record.InterestTotal = interestTotal;
                record.Total = baseTotal + interestTotal;
                record.BaseBalance = baseTotal - (paidLoan[0]);
                record.InterestBalance = interestTotal - (paidLoan[1]);
                record.TotalBalance = record.BaseBalance + record.InterestBalance;

                Records.Add(record);
            }

        }

        private List<double> GetTotalPaidLoan(int memberId, int loanTypeId)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("SUM(Amount) AS TotalBase, SUM(Interest) AS TotalInterest");
            sb.Table = "Payments";
            sb.AddWhereField("AccountID", loanTypeId, Enums.Data.DataType.Number);
            sb.AddWhereField("memberID", memberId, SPA.Enums.Data.DataType.Number);
            List<double> totalPaid = new List<double>();
            SqlDataReader dr = DbReader.Execute(sb.Sql);
            {
                if (dr.HasRecord())
                {
                    while (dr.Read())
                    {
                        totalPaid.Add(dr.GetValueDouble("TotalBase"));
                        totalPaid.Add(dr.GetValueDouble("TotalInterest"));
                    }
                }
                else
                {
                    totalPaid.Add(0.0);
                    totalPaid.Add(0.0);
                }
            }
            return totalPaid;
        }
     
        public void GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
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
            var sb = new SelectBuilder();
            sb.Table = "Members m";
            sb.Fields.Add("m.ID");
            sb.Fields.Add("m.Code as Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.OfficeRetiredDate");

            sb.Fields.Add("ml.BaseBalance AS Base");
            sb.Fields.Add("ml.InterestBalance AS Interest");
           
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            sb.Table += string.Format(" INNER JOIN MembersLoan ml ON ml.memberID = m.ID AND ml.TypeID = {0}", (int)reportFilter.LoanType);
            sb.Table += " WHERE m.OfficeRetiredDate IS NOT NULL";
            sb.Table += " AND m.OfficeRetiredDate >= '"  + reportFilter.RetiredAfter.ToString().ToDateDbFormatted() + "'"; 
            sb.OrderBy = "m.OfficeRetiredDate ASC";

            return sb.Sql;
        }
        public class ReportFilter : IReportFilter
        {
            public Enums.PaymentEnum.AccountID LoanType { get; set; }
            public DateTime  RetiredAfter { get; set; }
        }

        public void GenerateReportHeaderData()
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            ReportHeader = new ReportHeaderElement();
            var strLoan = string.Empty;
            switch (reportFilter.LoanType)
            {
              
                case PaymentEnum.AccountID.PinjamanBiasa:
                    strLoan = "BIASA";
                    break;
                case PaymentEnum.AccountID.PinjamanKecemasan:
                    strLoan = "KECEMASAN";
                    break;
                case PaymentEnum.AccountID.PinjamanKhas:
                    strLoan = "KHAS";
                    break;
                case PaymentEnum.AccountID.PinjamanMedisihat:
                    strLoan = "MEDISIHAT";
                    break;
                default:
                    strLoan = "???";
                    break;
            }

            ReportHeader.ReportTitle = string.Format("LAPORAN BAKI PINJAMAN {0} BERDASARKAN TARIKH BERSARA", strLoan);
        }

        public class ReportHeaderElement : IReportHeader
        {
            public string ReportTitle { get; set; }
        }

        public string GetQueryHeader()
        {
            return string.Empty;
        }

        void IDataGenerator.GenerateReportDetailData()
        {
            throw new NotImplementedException();
        }

        void IDataGenerator.GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

    }

    public class MemberRetiredLoanBalanceDataField
    {
        public int MemberId { get; set; }
        public int Index { get; set; }
        public string MembershipNo { get; set; }
        public string Name { get; set; }
        public string MemberIC { get; set; }
        public string RetiredDate { get; set; }
        public double BaseTotal { get; set; }
        public double InterestTotal { get; set; }
        public double Total { get; set; }
        public double BaseBalance { get; set; }
        public double InterestBalance { get; set; }
        public double TotalBalance { get; set; }
    }

}

