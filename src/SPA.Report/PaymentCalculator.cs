using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Cache;
using SPA.Data;
using System.Data;
using SPA.Core;

namespace SPA.Reporting
{
    public class PaymentCalculator
    {
     
        private DateTime _latestPaymentDate;
        
        public DateTime LatestPaymentDate
        {
            get { return _latestPaymentDate; }
            set { _latestPaymentDate = value; }
        }

        public DateTime MembershipDate { get; set; }

        public ExpectedPayment GetExpectedPayment()
        {
            var expected = new ExpectedPayment();
            int period = GetPeriod();

            expected.ExpectedYuranBulanan  = AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.YuranBulanan) * period;
            expected.ExpectedTabungDerma = AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa) * period;
            expected.ExpectedFiMasuk = AccountCache.GetBaseAmount((int)Enums.PaymentEnum.AccountID.FiMasuk);
           
            return expected;
        }

        private int GetPeriod()
        {
            int period = ((LatestPaymentDate.Year - MembershipDate.Year) * 12) + LatestPaymentDate.Month - MembershipDate.Month + 1;
            return (period < 0) ? 0 : period;
        }

        public ExpectedPayment GetTotalPayment(int memberID)
        {
            var expected = new ExpectedPayment();
            int period = GetPeriod();

            expected.ExpectedYuranBulanan = GetTotal(memberID, (int)Enums.PaymentEnum.AccountID.YuranBulanan);
            expected.ExpectedTabungDerma = GetTotal(memberID, (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa);
            expected.ExpectedFiMasuk = GetTotal(memberID, (int)Enums.PaymentEnum.AccountID.FiMasuk);
            expected.ExpectedSimpananKhas = GetTotal(memberID, (int)Enums.PaymentEnum.AccountID.SimpananKhas);
            expected.ExpectedSaham = GetTotal(memberID, (int)Enums.PaymentEnum.AccountID.Saham);

            return expected;
        }

        private double GetTotal(int memberID, int accountID)
        {
            var sb = new SelectBuilder();
            sb.Fields.Add("SUM(Amount) AS Amount");
            sb.Table = "Payments";
            sb.AddWhereField("MemberID", memberID, Enums.Data.DataType.Number);
            sb.AddWhereField("AccountID", accountID, Enums.Data.DataType.Number);
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


    }

  

}
