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
    public class OutstandingPaymentData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<OutstandingPaymentDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            Records = new List<OutstandingPaymentDataField>();
            var reportFilter = (ReportFilter)this.Filter;
            var calculator = new OutstandingCalculator(DateTime.Today); 
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
                    membershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateFormatted();
                }

                if (reportFilter.StartMonth < DateTime.Parse(membershipDate))
                {
                    calculator.StartMonth = DateTime.Parse(membershipDate);
                }

                List<OutstandingPayment> outstandingRecords = calculator.GetOutstanding(memberID);
                foreach (var outstandingRecord in outstandingRecords)
                {
                    var record = new OutstandingPaymentDataField();
                    record.Index = count;
                    record.ReportTitle = GetReportTitle();
                    record.MemberName = memberName;
                    record.MembershipNo = memberCode;
                    record.MembershipDate = membershipDate.ToDateMalayFormatted(true).ToUpper();
                    record.MemberKP = newIC;
                    record.Month = outstandingRecord.MonthYear.ToUpper();
                    record.FiMasuk = outstandingRecord.FiMasuk;
                    record.TabungDerma = outstandingRecord.KebajikanDermasiswa;
                    record.YuranBulanan = outstandingRecord.YuranBulanan;
                    record.Jumlah = record.FiMasuk + record.TabungDerma + record.YuranBulanan;
                    Records.Add(record);
                    
                }
            }
            int total = Records.Count;
        }

        private string GetReportTitle()
        {
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            return "LAPORAN TUNGGAKAN PEMBAYARAN BERMULA " + reportFilter.StartMonth.ToString().ToMonthFormatted().ToDateMalayFormatted(true).ToUpper();
        }
 
        public void GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

         public void GenerateReportHeaderData()
         {
             ReportFilter reportFilter = (ReportFilter)this.Filter;
             ReportHeader = new ReportHeaderElement();
             ReportHeader.ReportTitle = "LAPORAN TUNGGAKAN BAYARAN";
         }

        public string GetQuery()
        {
            var reportFilter = (ReportFilter)this.Filter;
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("m.ID");
            sb.Fields.Add("m.Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.MembershipDate");
            sb.Table += "Members m";
            sb.AddWhereField("m.MembershipDate", Enums.Data.AssignmentOperator.IsNotNull);
            if (reportFilter.MemberID != -1) sb.AddWhereField("ID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);   
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

    public class OutstandingPaymentDataField
    {
        public int Index { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string MembershipNo { get; set; }
        public string Month { get; set; }
        public double FiMasuk { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double Jumlah { get; set; }
        public string ReportTitle { get; set; }
    }



}

