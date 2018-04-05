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
    public class PaidMemberMonthYearData : IDataGenerator
    {
        public IReportFilter Filter { get; set; }
        public static List<PaidMemberMonthYearDataField> Records { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }

        public void GenerateReportDetailData(SqlDataReader dr)
        {
            int count = 0;
            ReportFilter reportFilter = (ReportFilter)this.Filter;
            var unsortedRecords = new List<PaidMemberMonthYearDataField>();
            while (dr.Read())
            {
                PaidMemberMonthYearDataField record = new PaidMemberMonthYearDataField();
                count += 1;
                record.Index = count;
                record.MemberID = dr.GetValueInt("ID");
                record.MemberName = dr.GetValueString("Name").ToUpper();
                record.MembershipNo = dr.GetValueString("Code");
                if (record.MembershipNo == string.Empty) continue;

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
                record.Race = Cache.SettingsCache.GetDescription(SettingEnum.Setting.Race, dr.GetValueInt("RaceID")).ToUpper();
                record.PositionTitle = dr.GetValueString("OfficePositionTitle").ToUpper();
                record.Sex = (dr.GetValueInt("SexID") == 1 ? "LELAKI" : "PEREMPUAN");

                record.FiMasuk = GetTotal(record.MemberID, (int)Enums.PaymentEnum.AccountID.FiMasuk, reportFilter.SelectedMonth, reportFilter.IsYearly);
                record.Saham = GetTotal(record.MemberID, (int)Enums.PaymentEnum.AccountID.Saham, reportFilter.SelectedMonth, reportFilter.IsYearly);
                record.YuranBulanan = GetTotal(record.MemberID, (int)Enums.PaymentEnum.AccountID.YuranBulanan, reportFilter.SelectedMonth, reportFilter.IsYearly);
                record.TabungDerma = GetTotal(record.MemberID, (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa, reportFilter.SelectedMonth, reportFilter.IsYearly);
                record.SimpananKhas = GetTotal(record.MemberID, (int)Enums.PaymentEnum.AccountID.SimpananKhas, reportFilter.SelectedMonth, reportFilter.IsYearly);
                record.Jumlah = record.FiMasuk + record.Saham + record.YuranBulanan + record.TabungDerma + record.SimpananKhas;

                unsortedRecords.Add(record);
            }
            Records = unsortedRecords;
            //Records = unsortedRecords.OrderBy(o => o.MembershipNo).ToList();
            //int recordCount = 0;
            //foreach (var record in Records)
            //{
            //    recordCount += 1;
            //    record.Index += recordCount;
            //}
        }

        private double GetTotal(int memberID, int accountID, DateTime selectedMonth, bool isYearly)
        {
            var sb = new SelectBuilder();
            sb.Fields.Add("SUM(Amount) AS Amount");
            sb.Table = "Payments";
            sb.AddWhereField("MemberID", memberID, Enums.Data.DataType.Number);
            sb.AddWhereField("AccountID", accountID, Enums.Data.DataType.Number);
            sb.AddWhereField("PaymentYear", selectedMonth.Year, Enums.Data.DataType.Number);
            if (isYearly == false)
            {
                sb.AddWhereField("PaymentMonth", selectedMonth.Month, Enums.Data.DataType.Number);
            }
            
            sb.GroupBy = "Amount";

            var dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    var amount = dr.GetValueDouble("Amount");
                    if (amount > 0)
                    {
                        return amount;
                    }
                }
            }
            return 0.0;
        }

        public void GenerateReportHeaderData(SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

         public void GenerateReportHeaderData()
         {
             ReportFilter reportFilter = (ReportFilter)this.Filter;
             ReportHeader = new ReportHeaderElement();
             if (reportFilter.IsYearly)
             {
                 ReportHeader.ReportTitle = "LAPORAN BAGI TAHUN BERAKHIR " + "31 DISEMBER " + reportFilter.SelectedMonth.Year;
             }else
             {
                 ReportHeader.ReportTitle = "LAPORAN BAGI BULAN " + reportFilter.SelectedMonth.ToString().ToMonthFormatted().ToDateMalayFormatted(false).ToUpper();

             }
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
            sb.Fields.Add("m.SexID");
            sb.Fields.Add("m.RaceID");
            sb.Fields.Add("m.OfficePositionTitle");
            sb.Table += "Members m";
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
            public bool IsYearly { get; set; }
            public DateTime SelectedMonth { get; set; }
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

    public class PaidMemberMonthYearDataField
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
        public string Sex { get; set; }
        public string Race { get; set; }
        public string PositionTitle { get; set; }
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

