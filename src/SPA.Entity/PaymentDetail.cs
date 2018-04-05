using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class PaymentDetail
    {
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public int AccountID { get; set; }
        public double Amount { get; set; }
        public double Interest { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public string  Syscreated { get; set; }
    }
}
