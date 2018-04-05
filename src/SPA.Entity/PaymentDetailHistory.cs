using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class PaymentDetailHistory
    {
        public string  AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public int LastPaymentMonth { get; set; }
        public int LastPaymentYear{ get; set; }
        public double Outstanding { get; set; }
    }
}
