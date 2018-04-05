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
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            while (dr.Read())  
            {
                PaidMemberDataField record = new PaidMemberDataField();
                record.MemberName = dr.GetValueString("Name");
                record.MembershipNo = dr.GetValueString("Code");
                record.MemberKP = dr.GetValueString("NewIC");
                record.MembershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateMalayFormatted(true);
                record.FiMasuk = dr.GetValueDouble("FiMasuk");
                record.Saham = dr.GetValueDouble("Saham");
                record.YuranBulanan = dr.GetValueDouble("YuranBulanan");
                record.TabungDerma = dr.GetValueDouble("TabungDerma");
                record.SimpananKhas = dr.GetValueDouble("SimpananKhas");
                record.Jumlah = record.FiMasuk + record.Saham + record.YuranBulanan + record.TabungDerma;
                Records.Add(record);
            }
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
            sb.Fields.Add("m.Code");
            sb.Fields.Add("m.Name");
            sb.Fields.Add("m.NewIC");
            sb.Fields.Add("m.MembershipDate");
            sb.Fields.Add("SUM(p1.Amount) AS YuranBulanan");
            sb.Fields.Add("SUM(p2.Amount) AS TabungDerma");
            sb.Fields.Add("SUM(p3.Amount) AS Saham");
            sb.Fields.Add("SUM(p4.Amount) AS FiMasuk");
            sb.Fields.Add("SUM(p5.Amount) AS SimpananKhas");

            ReportFilter reportFilter = (ReportFilter)this.Filter;
            sb.Table += string.Format(" LEFT JOIN Payments p1 ON p.NomborLarian = p1.NomborLarian and p1.AccountID={0} and p1.YearMonth between {1} and {2} and p1.MemberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.YuranBulanan, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p2 ON p.NomborLarian = p2.NomborLarian and p2.AccountID={0} and p2.YearMonth between {1} and {2} and p2.MemberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.KebajikanDermasiswa, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p3 ON p.NomborLarian = p3.NomborLarian and p3.AccountID={0} and p3.YearMonth between {1} and {2} and p3.MemberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.Saham, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p4 ON p.NomborLarian = p4.NomborLarian and p4.AccountID={0} and p4.YearMonth between {1} and {2} and p4.MemberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.FiMasuk, reportFilter.PeriodFrom, reportFilter.PeriodTo);
            sb.Table += string.Format(" LEFT JOIN Payments p5 ON p.NomborLarian = p5.NomborLarian and p5.AccountID={0} and p5.YearMonth between {1} and {2} and p5.MemberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.SimpananKhas, reportFilter.PeriodFrom, reportFilter.PeriodTo);


            sb.GroupBy = "m.Code, m.Name, m.NewIC, m.MembershipDate, p1.Amount, p2.Amount, p3.Amount, p4.Amount, p5.Amount ";
            sb.OrderBy = "m.Code";

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



        public void GenerateReportDetailData()
        {
            throw new NotImplementedException();
        }
    }

    public class PaidMemberDataField
    {
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string MembershipNo { get; set; }
        public double FiMasuk { get; set; }
        public double Saham { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double SimpananKhas { get; set; }
        public double Jumlah { get; set; }
    }
}

