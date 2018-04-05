using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class Account : EntityBase, IEntity 
    {

        #region IEntity Members

        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Account() : base("Accounts")
        {

        }

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

        public override int GetID()
        {
            var reader = new AccountReader();
            if (reader.ReadSingle(new FilterElement() { Key = Enums.Data.KeyElements.AccountCode, Value = this.Code }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        public int GetID(string code)
        {
            var reader = new AccountReader();
            if (reader.ReadSingle(new FilterElement() { Key = Enums.Data.KeyElements.AccountCode, Value = code }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            var reader = new AccountReader();
            if (reader.ReadSingle(new FilterElement() { Key = key, Value = value }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}
