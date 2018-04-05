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
    public class OutstandingLoanData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<OutstandingLoanDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            Records = new List<OutstandingLoanDataField>();
            var reportFilter = (ReportFilter)this.Filter;
            var calculator = new OutstandingLoanCalculator(DateTime.Today, reportFilter.AccountId);
            calculator.StartMonth = reportFilter.StartMonth;
            int count = 0;
            while (dr.Read())
            {
                count += 1;
                var memberID = dr.GetValueInt("ID");
                var memberName = dr.GetValueString("Name").ToUpper();
                var memberCode = dr.GetValueString("Code");
                var newIC = dr.GetValueString("NewIC");
                var membershipDate = string.Empty;
                if (dr.GetValueDate("MembershipDate").Year != 2999)
                {
                     membershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateFormatted().ToDateMalayFormatted(true).ToUpper();
                }

                var loanStartDate = dr.GetValueDate("LoanStartDate").ToString().ToDateFormatted();

                if (reportFilter.StartMonth < DateTime.Parse(loanStartDate))
                {
                    calculator.StartMonth = DateTime.Parse(loanStartDate);
                }
                else
                {
                    calculator.StartMonth = reportFilter.StartMonth;
                }
        
                List<OutstandingPayment> outstandingRecords = calculator.GetOutstanding(memberID);
                foreach (var outstandingRecord in outstandingRecords)
                {
                    var record = new OutstandingLoanDataField();
                    record.ReportTitle = GetReportTitle();
                    record.Index = count;
                    record.MemberName = memberName;
                    record.MembershipNo = memberCode;
                    record.MembershipDate = membershipDate;
                    record.LoanStartDate = loanStartDate.ToDateMalayFormatted(true).ToUpper(); 
                    record.MemberKP = newIC;
                    record.Month = outstandingRecord.MonthYear.ToUpper();
                    record.Base = outstandingRecord.Base;
                    record.Interest = outstandingRecord.Interest;
                    record.Jumlah = record.Base + record.Interest;
                    Records.Add(record);

                }
            }

            //int total = Records.Count;

        }
        
        private string GetReportTitle()
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            return "LAPORAN TUNGGAKAN " + Cache.AccountCache.GetAccountDescription(reportFilter.AccountId).ToUpper() + " BERMULA " + reportFilter.StartMonth.ToString().ToMonthFormatted().ToDateMalayFormatted(true).ToUpper();
        }
 
        public void GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

         public void GenerateReportHeaderData()
         {
             ReportFilter reportFilter = (ReportFilter)this.Filter;
             ReportHeader = new ReportHeaderElement();
             ReportHeader.ReportTitle = "LAPORAN TUNGGAKAN " + Cache.AccountCache.GetAccountDescription(reportFilter.AccountId) + " BERMULA " + reportFilter.StartMonth.ToString().ToDateMalayFormatted(true).ToUpper();
         }

        public string GetQuery()
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("m.ID");
            sb.Fields.Add("m.Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.MembershipDate");
            sb.Fields.Add("ml.LoanStartDate");
            sb.Table += "Members m";
            sb.Table += " INNER JOIN MembersLoan ml ON ml.MemberId = m.ID AND ml.TypeID = " + reportFilter.AccountId;
            //sb.AddWhereField("m.MembershipDate", Enums.Data.AssignmentOperator.IsNotNull);
            if (reportFilter.MemberID != -1) sb.AddWhereField("m.ID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
            sb.OrderBy = "m.Code";

            return sb.Sql;   
        }

        public string GetQueryHeader()
        {
            return string.Empty;      
        }
        public class ReportFilter : IReportFilter 
        {
            public int MemberID { get; set; }
            public DateTime StartMonth { get; set; }
            public int AccountId { get; set; }
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

    }

    public class OutstandingLoanDataField
    {
        public int Index { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string LoanStartDate { get; set; }
        public string MembershipNo { get; set; }
        public string Month { get; set; }
        public double Base { get; set; }
        public double Interest { get; set; }
        public double Jumlah { get; set; }
        public string ReportTitle { get; set; }
    }



}

