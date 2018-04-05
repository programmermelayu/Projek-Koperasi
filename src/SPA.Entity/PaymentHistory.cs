using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class PaymentHistory  : IEntity 
    {
        private const string TableName = "Payments";
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberNoKP { get; set; }
        public double TotalOutstanding { get; set; }

        private List<PaymentDetailHistory> _paymentsDetailHistory;
        public List<PaymentDetailHistory> PaymentsDetailHistory
        {

            get
            {
                if (_paymentsDetailHistory == null)
                {
                    _paymentsDetailHistory = new List<PaymentDetailHistory>();
                }
                return _paymentsDetailHistory;
            }
            set
            {
                _paymentsDetailHistory = value;
            }
        }

        #region IEntity Members

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public string Sql
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SystemErrorMessage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
