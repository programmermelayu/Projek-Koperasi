using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Reporting
{
    public class PaymentByDateRangeData : IDataGenerator  
    {

        public IReportFilter Filter { get; set; }
        public static List<PaymentByDateRangeDataField> Records;

        public string GetQuery()
        {
            throw new NotImplementedException();
        }

        public string GetQueryHeader()
        {
            throw new NotImplementedException();
        }

        public void GenerateReportDetailData(System.Data.SqlClient.SqlDataReader dr)
        {
            throw new NotImplementedException();
        }

        public void GenerateReportDetailData()
        {
            throw new NotImplementedException();
        }

        public void GenerateReportHeaderData(System.Data.SqlClient.SqlDataReader dr)
        {
            throw new NotImplementedException();
        }


        public void GenerateReportHeaderData()
        {
            throw new NotImplementedException();
        }
    }

    public class PaymentByDateRangeDataField
    {
        public int recordNum { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string MemberKP { get; set; }
        public DateTime  MembershipDate { get; set; }
        public double FiMasuk { get; set; }
        public double Saham { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double SimpananKhas { get; set; }
        public double Jumlah { get; set; }
    }

}
