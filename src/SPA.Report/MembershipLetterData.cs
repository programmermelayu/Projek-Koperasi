using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Entity;
using SPA.Data;
using SPA.Core;
using SPA.Cache;


namespace SPA.Reporting
{
    public class MembershipLetterData : IDataGenerator 
    {

        public IReportFilter Filter { get; set; }
        public static ReportHeaderElement ReportHeader { get; set; }
        public string GetQuery()
        {
            return string.Empty;            
        }

        public string GetQueryHeader()
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("ID");
            sb.Fields.Add("Code");
            sb.Fields.Add("NewIC");
            sb.Fields.Add("Name");
            sb.Fields.Add("CurrentAddress");
            sb.Fields.Add("CurrentPostcode");
            sb.Fields.Add("CurrentDistrict");
            sb.Fields.Add("CurrentStateID");
            sb.Fields.Add("StatusID");
            sb.Table = "Members";

            ReportFilter reportFilter = (ReportFilter)this.Filter;

            if (reportFilter != null)
            {
                if (reportFilter.MemberID > 0)
                {
                    sb.AddWhereField("ID", reportFilter.MemberID, SPA.Enums.Data.DataType.Number);
                }
            }

            return sb.Sql;
        }
        
        public void GenerateReportDetailData(System.Data.SqlClient.SqlDataReader dr)
        {
            return;
        }

        public void GenerateReportDetailData()
        {
            return;
        }

        public void GenerateReportHeaderData(System.Data.SqlClient.SqlDataReader dr)
        {
            ReportHeader = new ReportHeaderElement();
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    ReportHeader.Name = dr.GetString(dr.GetOrdinal("Name")).ToUpper();
                    ReportHeader.MemberCode = dr.GetString(dr.GetOrdinal("Code"));
                    ReportHeader.CurrentAddress = dr.GetString(dr.GetOrdinal("CurrentAddress")).ToUpper();
                    ReportHeader.CurrentPostcode = dr.GetString(dr.GetOrdinal("CurrentPostcode"));
                    ReportHeader.CurrentDistrict = dr.GetString(dr.GetOrdinal("CurrentDistrict")).ToUpper();
                    ReportHeader.CurrentState = SettingsCache.GetDescription(Enums.SettingEnum.Setting.State, dr.GetValueInt("CurrentStateID"));
                    ReportHeader.CurrentDate = DateTime.Now.ToString().ToDateFormatted().ToDateMalayFormatted(true);
                    ReportHeader.ReferenceNo = "M -" + ReportHeader.MemberCode;
                }
            }
        }

        public class ReportFilter : IReportFilter
        {
            public int MemberID { get; set; }
        }

        public class ReportHeaderElement : IReportHeader
        {
            public string Name { get; set; }
            public string MemberCode { get; set; }
            public string ReferenceNo { get; set; }
            public string CurrentDate { get; set; }
            public string CurrentAddress { get; set; }
            public string CurrentPostcode { get; set; }
            public string CurrentDistrict { get; set; }
            public string CurrentState { get; set; }
        }



        public void GenerateReportHeaderData()
        {
            throw new NotImplementedException();
        }
    }
}
