using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Data;
using System.Globalization;
using SPA.Cache;
using SPA.Core;
using SPA.Entity;

namespace SPA.Reporting
{
   public class OutstandingCalculator
   {
       private DateTime _currentDate;

       private List<PaidMonth> _paidMonths;
       private IOutstandingCalculatorDataManager _dataManager;
       public IOutstandingCalculatorDataManager DataManager
       { get
           {
               if (_dataManager ==null)
               {
                   _dataManager = new OutstandingCalculatorDataManager();
               }
               return _dataManager;
           }
           set
           {
               _dataManager = value;
           }
       }

       private int _accountId;
       protected  int AccountId
       {
           get
           {
               return _accountId;
           }
       }

       private int _memberId;
       protected  int MemberId
       {
           get
           {
               return _memberId;
           }
       }

       private DateTime _startMonth;
       public DateTime StartMonth
       {
           get
           {
               if (_startMonth == DateTime.MinValue)
               {
                   _startMonth = new DateTime(2015, 1, 1);
               }
               return _startMonth;
           }
           set
           {
               _startMonth = value;
           }
       }
       
       public OutstandingCalculator(DateTime currentDate)
       {
           _currentDate = currentDate;
       }

       public OutstandingCalculator(DateTime currentDate, int accountId)
       {
           _currentDate = currentDate;
           _accountId = accountId;
       }

       public List<OutstandingPayment> GetOutstanding(int memberId)
       {
           _memberId = memberId;
           List<OutstandingPayment> records = null;
           _paidMonths = DataManager.GetPaidMonth(StartMonth, _currentDate, memberId);
           int period = ((_currentDate.Year - StartMonth.Year) * 12) + _currentDate.Month - StartMonth.Month + 1;
           if (_paidMonths == null)
           {
               records = BuildNeverPaidOutstandingRecords(period); 
           }
           else
           {
               records =  BuildOutstandingRecords(period); 
           }
           return records;          
       }

       protected virtual List<OutstandingPayment> BuildOutstandingRecords(int period)
       {
           var records = new List<OutstandingPayment>();
           for (int i = 0; i < period; i++)
           {
               var newdate = StartMonth.AddMonths(i);
               var outstanding = new OutstandingPayment();
               if (i == 0 && !this.IsFiMasukPaid())
               {
                   outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
                   outstanding.FiMasuk = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.FiMasuk);
               }

               if (!IsPaidMonth(newdate, (int)Enums.PaymentEnum.AccountID.YuranBulanan))
               {
                   outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
                   outstanding.YuranBulanan = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.YuranBulanan);
               }

               if (!IsPaidMonth(newdate, (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa))
               {
                   outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
                   outstanding.KebajikanDermasiswa = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa);
               }

               if (!string.IsNullOrEmpty(outstanding.MonthYear))
               {
                   records.Add(outstanding);
               }
           }
           return records;
       }

       protected virtual List<OutstandingPayment> BuildNeverPaidOutstandingRecords(int period)
       {
           var records = new List<OutstandingPayment>();
           for (int i = 0; i < period; i++)
           {
               var newdate = StartMonth.AddMonths(i);
               var outstanding = new OutstandingPayment();
            
               outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
               outstanding.FiMasuk = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.FiMasuk);
               outstanding.YuranBulanan = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.YuranBulanan);
               outstanding.KebajikanDermasiswa = DataManager.GetBaseAmount((int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa);
               records.Add(outstanding);
           }
           return records;
       }


    protected bool IsPaidMonth(DateTime thisDate, int accountId)
    {  
        return _paidMonths.Exists(x => x.Month == thisDate.Month && x.Year == thisDate.Year && x.AccountId == accountId);
    }

    private bool IsFiMasukPaid()
    {
        return _paidMonths.Exists(x => x.AccountId == (int)Enums.PaymentEnum.AccountID.FiMasuk);
    }
   }

    public class OutstandingLoanCalculator : OutstandingCalculator
    {
        public OutstandingLoanCalculator(DateTime currentDate, int accountId) :  base(currentDate, accountId)
        {
           
        }

        protected override List<OutstandingPayment> BuildNeverPaidOutstandingRecords(int period)
        {
              var records = new List<OutstandingPayment>();
              for (int i = 0; i < period; i++)
              {
                  var newdate = StartMonth.AddMonths(i);
                  var outstanding = new OutstandingPayment();
                  outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
                  List<double> loanBaseInterest = DataManager.GetLoanBaseAmount(base.MemberId, base.AccountId);
                  if (loanBaseInterest != null && loanBaseInterest.Count > 1)
                  {
                      outstanding.Base = loanBaseInterest[0];
                      outstanding.Interest = loanBaseInterest[1];
                  }
                  records.Add(outstanding);
              }
              return records;
        }

        protected override List<OutstandingPayment> BuildOutstandingRecords(int period)
        {
            var records = new List<OutstandingPayment>();
            for (int i = 0; i < period; i++)
            {
                var newdate = StartMonth.AddMonths(i);
                var outstanding = new OutstandingPayment();

                if (!base.IsPaidMonth(newdate, base.AccountId))
                {
                    outstanding.MonthYear = new DateTime(newdate.Year, newdate.Month, 1).ToString("MMM - yyyy", CultureInfo.InvariantCulture);
                    List<double> loanBaseInterest = DataManager.GetLoanBaseAmount(base.MemberId, base.AccountId);
                    if (loanBaseInterest != null && loanBaseInterest.Count > 1)
                    {
                       outstanding.Base = loanBaseInterest[0];
                       outstanding.Interest = loanBaseInterest[1];
                    }
                }

                if (!string.IsNullOrEmpty(outstanding.MonthYear))
                {
                    records.Add(outstanding);
                }
            }
            return records;
        }
    }
    public class PaidMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int AccountId { get; set; }
    }

    public class OutstandingPayment
    {

        public string MonthYear { get; set; }
        public double FiMasuk { get; set; }
        public double YuranBulanan { get; set; }
        public double KebajikanDermasiswa { get; set; }
        public double Base { get; set; }
        public double Interest { get; set; }

    }

    public interface IOutstandingCalculatorDataManager
    {
        List<PaidMonth> GetPaidMonth(DateTime startMonth, DateTime endMonth, int memberId);
        double GetBaseAmount(int accountId);
        List<double> GetLoanBaseAmount(int memberId, int account);
    }

    public class OutstandingCalculatorDataManager: IOutstandingCalculatorDataManager
    {
        public List<PaidMonth> GetPaidMonth(DateTime startMonth, DateTime endMonth, int memberId)
        {
            //select * from Payments where MemberID > 3657  and YearMonth between 201501 and 201505

            var sb = new SelectBuilder();
            sb.Fields.Add("AccountID");
            sb.Fields.Add("PaymentMonth");
            sb.Fields.Add("PaymentYear");
            sb.Table = "Payments";
            sb.AddWhereField("MemberID", memberId, Enums.Data.DataType.Number);
            sb.AddWhereField("(YearMonth", Payment.GetYearMonth(startMonth.Month, startMonth.Year), Payment.GetYearMonth(endMonth.Month, endMonth.Year));

            var sb2 = new SelectBuilder();
            sb2.Fields.Add("AccountID");
            sb2.Fields.Add("PaymentMonth");
            sb2.Fields.Add("PaymentYear");
            sb2.Table = "Payments";
            sb2.AddWhereField("MemberID", memberId, Enums.Data.DataType.Number);
            sb2.AddWhereField("AccountId", (int)Enums.PaymentEnum.AccountID.FiMasuk, Enums.Data.DataType.Number);

            string sqlUnionized = sb.Sql + " UNION " + sb2.Sql;
            var dr = DbReader.Execute(sqlUnionized);
            List<PaidMonth> paidMonths = null;
            if (dr.HasRecord())
            {
               paidMonths = new List<PaidMonth>();
                while (dr.Read())
                {
                    var paidMonth = new PaidMonth();
                    paidMonth.Month = dr.GetValueInt("PaymentMonth");
                    paidMonth.Year = dr.GetValueInt("PaymentYear");
                    paidMonth.AccountId = dr.GetValueInt("AccountID");
                    paidMonths.Add(paidMonth);
                }
            }
            return paidMonths;
        }

        //public List<PaidMonth> GetPaidLoanMonth(DateTime startMonth, DateTime endMonth, int memberId, int accountId)
        //{
        //    //select * from Payments where MemberID > 3657  and YearMonth between 201501 and 201505

        //    var sb = new SelectBuilder();
        //    sb.Fields.Add("AccountID");
        //    sb.Fields.Add("PaymentMonth");
        //    sb.Fields.Add("PaymentYear");
        //    sb.Table = "Payments";
        //    sb.AddWhereField("MemberID", memberId, Enums.Data.DataType.Number);
        //    sb.AddWhereField("accountID", accountId, Enums.Data.DataType.Number);
        //    sb.AddWhereField("(YearMonth", Payment.GetYearMonth(startMonth.Month, startMonth.Year), Payment.GetYearMonth(endMonth.Month, endMonth.Year));

        //    var dr = DbReader.Execute(sb.Sql);
        //    List<PaidMonth> paidMonths = null;
        //    if (dr.HasRecord())
        //    {
        //        paidMonths = new List<PaidMonth>();
        //        while (dr.Read())
        //        {
        //            var paidMonth = new PaidMonth();
        //            paidMonth.Month = dr.GetValueInt("PaymentMonth");
        //            paidMonth.Year = dr.GetValueInt("PaymentYear");
        //            paidMonth.AccountId = dr.GetValueInt("AccountID");
        //            paidMonths.Add(paidMonth);
        //        }
        //    }
        //    return paidMonths;
        //}
     
        public double GetBaseAmount(int accountId)
        {
            return AccountCache.GetBaseAmount(accountId);
        }

        public List<double> GetLoanBaseAmount(int memberId, int accountId)
        {
            List<double> amounts = new List<double>();
            var sb = new SelectBuilder();
            sb.Fields.Add("Base");
            sb.Fields.Add("Interest");
            sb.Table = "MembersLoan";
            sb.AddWhereField("MemberID", memberId, Enums.Data.DataType.Number);
            sb.AddWhereField("TypeID", accountId, Enums.Data.DataType.Number);
   
            var dr = DbReader.Execute(sb.Sql);
            List<PaidMonth> paidMonths = null;
            if (dr.HasRecord())
            {
                paidMonths = new List<PaidMonth>();
                while (dr.Read())
                {

                    amounts.Add(dr.GetValueDouble("Base"));
                    amounts.Add(dr.GetValueDouble("Interest"));
                }
            }
            return amounts;
        }


    }

}
