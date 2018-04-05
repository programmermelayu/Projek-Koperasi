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
    public class PaidMemberByStateData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<PaidMemberByStateDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            int count = 0;
            var unsortedRecords = new List<PaidMemberByStateDataField>();
            while (dr.Read())
            {
                PaidMemberByStateDataField record = new PaidMemberByStateDataField();
                count += 1;
                //record.Index = count;
                record.MemberID = dr.GetValueInt("ID");
                record.MemberName = dr.GetValueString("Name").ToUpper();
                record.MembershipNo = dr.GetValueString("Code");
                record.MemberKP = dr.GetValueString("NewIC");
                record.MembershipDate = string.Empty;
                if (dr.GetValueDate("MembershipDate").Year != 2999)
                {
                    record.MembershipDate = dr.GetValueDate("MembershipDate").ToString().ToDateFormatted().ToDateMalayFormatted(true).ToUpper();
                }
                record.Address = dr.GetValueString("PermanentAddress").ToUpper();
                record.Postcode = dr.GetValueString("PermanentPostcode");
                record.District = dr.GetValueString("PermanentDistrict").ToUpper();
                record.State = Cache.SettingsCache.GetDescription(SettingEnum.Setting.State, dr.GetValueInt("PermanentStateID")).ToUpper();
                record.Category  = Cache.SettingsCache.GetDescription(SettingEnum.Setting.Category, dr.GetValueInt("CategoryID"));

                var calculator = new PaymentCalculator();
                var total = calculator.GetTotalPayment(record.MemberID);

                record.FiMasuk = total.ExpectedFiMasuk;
                record.Saham = total.ExpectedSaham;
                record.YuranBulanan = total.ExpectedYuranBulanan;
                record.TabungDerma = total.ExpectedTabungDerma;
                record.SimpananKhas = total.ExpectedSimpananKhas;
                record.Jumlah = record.FiMasuk + record.Saham + record.YuranBulanan + record.TabungDerma + record.SimpananKhas;

                string lastPaidMonth = GetLastPaidMonth(record.MemberID);
                if (!string.IsNullOrEmpty(lastPaidMonth))
                {
                    record.Note = "BAYARAN SEHINGGA " + lastPaidMonth;
                }

                unsortedRecords.Add(record);
            }

           Records = unsortedRecords.OrderByDescending(o => o.Jumlah).ToList();
           int recordCount = 0;
           foreach (var record in Records)
           {
               recordCount += 1;
               record.Index += recordCount;
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
             ReportHeader.ReportTitle = "LAPORAN ANGGOTA BAGI NEGERI " + SPA.Cache.SettingsCache.GetDescription(SettingEnum.Setting.State, reportFilter.StateID).ToUpper();
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
            sb.Fields.Add("m.PermanentAddress");
            sb.Fields.Add("m.PermanentPostcode");
            sb.Fields.Add("m.PermanentDistrict");
            sb.Fields.Add("m.PermanentStateID");
            sb.Fields.Add("m.CategoryID");
            sb.Fields.Add("ISNULL(SUM(p1.Amount),0) AS YuranBulanan");
            sb.Fields.Add("ISNULL(SUM(p2.Amount),0)  AS TabungDerma");
            sb.Fields.Add("ISNULL(SUM(p3.Amount),0)  AS Saham");
            sb.Fields.Add("ISNULL(SUM(p4.Amount),0)  AS FiMasuk");
            sb.Fields.Add("ISNULL(SUM(p5.Amount),0)  AS SimpananKhas");
            sb.Table += "Members m";
            sb.Table += string.Format(" LEFT JOIN Payments p1 ON p1.AccountID={0} and p1.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.YuranBulanan);
            sb.Table += string.Format(" LEFT JOIN Payments p2 ON p2.AccountID={0} and p2.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.KebajikanDermasiswa);
            sb.Table += string.Format(" LEFT JOIN Payments p3 ON p3.AccountID={0} and p3.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.Saham);
            sb.Table += string.Format(" LEFT JOIN Payments p4 ON p4.AccountID={0} and p4.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.FiMasuk);
            sb.Table += string.Format(" LEFT JOIN Payments p5 ON p5.AccountID={0} and p5.memberID = m.id ", (int)SPA.Enums.PaymentEnum.AccountID.SimpananKhas);

            sb.AddWhereField("PermanentStateID", reportFilter.StateID, Enums.Data.DataType.Number);
            if (reportFilter.StatusID > 0)
            {
                if (reportFilter.StatusID == 1)
                    sb.AddWhereField("m.StatusID", 1, Enums.Data.DataType.Number);
                else
                    sb.AddWhereField("m.StatusID", 1, Enums.Data.DataType.Number, Enums.Data.AssignmentOperator.IsNotEqual);
            }

            sb.GroupBy = "m.ID, m.PermanentStateID, m.PermanentPostcode,m.PermanentDistrict,m.PermanentAddress, m.CategoryID, m.Code, m.Name, m.NewIC, m.MembershipDate, p1.Amount, p2.Amount, p3.Amount, p4.Amount, p5.Amount ";
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
            public int SelectedYear { get; set; }
            public int PeriodFrom { get; set; }
            public int PeriodTo { get; set; }
            public DateTime StartMonth { get; set; }
            public DateTime EndMonth { get; set; }
            public int StateID { get; set; }
            public int StatusID { get; set; }
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

        public string GetLastPaidMonth(int memberID)
        {
            var sb = new SelectBuilder();
            sb.Fields.Add("MAX(YearMonth) AS YearMonth");
            sb.Table = "Payments";
            sb.AddWhereField("MemberID", memberID, Enums.Data.DataType.Number);
            sb.AddWhereField("AccountID", 1, Enums.Data.DataType.Number);

            var dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                while(dr.Read())
                {
                    var yearMonth = dr.GetValueString("YearMonth");
                    if (yearMonth != string.Empty || yearMonth.Length >= 4)
                    {
                        return yearMonth.Remove(0, 4).Trim() + "/" + yearMonth.Remove(4, 2).Trim();
                    }
                }
            }
            return string.Empty;
        }
    
    }

    public class PaidMemberByStateDataField
    {
        public int Index { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public string MembershipDate { get; set; }
        public string MembershipNo { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public double FiMasuk { get; set; }
        public double Saham { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double SimpananKhas { get; set; }
        public double Jumlah { get; set; }
    }



}

