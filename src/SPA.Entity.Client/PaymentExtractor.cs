using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SPA.Core;
using SPA.Cache;
using SPA.Entity;

namespace SPA.Entity.Client
{
    public class PaymentExtractor
    {
        private const int 
        Start_NoKPBaru = 2,
        End_NoKPBaru = 14,

        Start_NoKPLama = 90,
        End_NoKPLama = 19,

        Start_MemberName = 55,
        End_MemberName = 90,

        Start_AccountCode = 37,
        End_AccountCode = 41,

        Start_Amount = 41,
        End_Amount = 48,

        Start_Interest = 48,
        End_Interest = 55,

        Start_PaymentMonth = 35,
        End_PaymentMonth = 37,

        Start_PaymentYear = 31,
        End_PaymentYear = 35,

        Start_KodMajikan = 55,
        End_KodMajikan = 65,

        Start_KodKoperasi = 14,
        End_KodKoperasi = 19,

        Start_NoBaucer = 78,
        End_NoBaucer = 85,

        Start_NoLarian = 85,
        End_NoLarian = 90;

        public string FilePath { get; set; }

        private List<Payment> _payments;
        public List<Payment> Payments {
            get
            {
                if (_payments == null)
                {
                    _payments = new List<Payment>();
                }
                return _payments;
            }
            set
            {
                _payments = value;
            }
        }

        public string SystemErrorMessage { get; set; }
        public string SuccessMessage { 
            get
            {
                return "Maklumat selesai diimpot!";
            }
        }

        public string ErrorMessage { get; set; }

        public PaymentExtractor(string filePath)
        {
            FilePath = filePath;
        }

        public PaymentExtractor()
        {

        }

        public string DefaultDirectory 
        { 
            get
            {
                return DirectoryHandler.GetDefaultDirectoryImport();                
            }
        }

        //TODO: to add all element of payments
        public bool Extract()
        {
            try
            {
                Payment payment = null;
                PaymentDetail paymentDetail;
                foreach (var line in File.ReadAllLines(FilePath))
                {
                    string lineText = line;
                    if (line.Substring(0, 1) == "0")
                    {
                        if (payment != null)
                        {
                            Payments.Add(payment);
                        }
                        payment = new Payment();
                        payment.NoBaucer  = this.GetElement(lineText, Start_NoBaucer, End_NoBaucer).Trim();
                        payment.KodMajikan = this.GetElement(lineText, Start_KodMajikan, End_KodMajikan).Trim();
                        payment.KodKoperasi = this.GetElement(lineText, Start_KodKoperasi, End_KodKoperasi).Trim();
                        payment.NoKPBaru = this.GetElement(lineText, Start_NoKPBaru, End_NoKPBaru).Trim();
                        payment.NoLarian  = this.GetElement(lineText, Start_NoLarian, End_NoLarian).Trim();
                    }
                    else
                    {
                        paymentDetail = new PaymentDetail();
                        payment.MemberName = this.GetElement(lineText, Start_MemberName, End_MemberName).Trim();
                        paymentDetail.AccountCode = this.GetElement(lineText, Start_AccountCode , End_AccountCode).Trim();
                        paymentDetail.AccountID = AccountCache.GetAccountID(paymentDetail.AccountCode);
                        paymentDetail.Amount = Get2DecimalAmount(double.Parse(this.GetElement(lineText, Start_Amount, End_Amount, Enums.Data.DataType.Number)));
                        paymentDetail.Interest = Get2DecimalAmount(double.Parse(this.GetElement(lineText, Start_Interest, End_Interest, Enums.Data.DataType.Number)));
                        paymentDetail.PaymentMonth = int.Parse(this.GetElement(lineText, Start_PaymentMonth,End_PaymentMonth, Enums.Data.DataType.Number));
                        paymentDetail.PaymentYear = int.Parse(this.GetElement(lineText, Start_PaymentYear, End_PaymentYear, Enums.Data.DataType.Number));                        
                        payment.PaymentDetails.Add(paymentDetail);
                    }
                }
                Payments.Add(payment);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Data dari " + this.FilePath + " gagal dibaca sepenuhnya!";
                SystemErrorMessage = ex.Message + " " + ex.StackTrace;
                LogHandler.WriteError("Importing failed for " + this.FilePath + ". " + this.SystemErrorMessage);
                return false;
            }

            return true;            
        }

        private string GetElement(string textLine, int startIndex, int endIndex, SPA.Enums.Data.DataType type)
        {
            string element = "";
            switch (type)
            {
                case SPA.Enums.Data.DataType.Number:
                    int elementVal;
                    if (textLine.Length > endIndex && textLine.Length > startIndex)
                    {
                        element = textLine.Substring(startIndex, endIndex - startIndex);
                        if (int.TryParse(element, out elementVal) == false)
                        {
                            element = "0";
                        }
                    }
                    else
                    {
                        element = "0";
                    }                  
                    break;
                default:
                    if (textLine.Length > endIndex && textLine.Length > startIndex)
                    {
                        element = textLine.Substring(startIndex, endIndex - startIndex);
                    }
                    break;
            } 
            return element;
        }

        private string GetElement(string textLine, int startIndex, int endIndex)
        {
            string element = "";
            if (textLine.Length > endIndex && textLine.Length > startIndex)
            {
                element = textLine.Substring(startIndex, endIndex - startIndex);
            }    
            return element;
        }

        private double Get2DecimalAmount(double amount)
        {
            return amount / 100;
        }
    }
}
