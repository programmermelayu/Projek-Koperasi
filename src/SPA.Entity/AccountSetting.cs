using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Data;
using SPA.Core;

namespace SPA.Entity
{
    public class AccountSetting : EntityBase, IEntity 
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public string AccountCode { get; set; }
        public double Amount { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime StartDate  { get; set; }
        public DateTime EndDate { get; set; }

        public AccountSetting(): base("AccountSettings")
        {
            
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
            if (ID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Insert();
            }
        }

        private bool Insert()
        {
            BaseInsertBuilder.AddField("AccountID", AccountID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("Amount", Amount, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("IsActive", GetActiveValue(), Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("StartDate", StartDate, Enums.Data.DataType.Date);
            BaseInsertBuilder.AddField("EndDate", EndDate, Enums.Data.DataType.Date);
            return ExecuteInsert();
        }

        private bool Update()
        {
            base.BaseUpdateBuilder.AddField("Amount", Amount, Enums.Data.DataType.Number);
            base.BaseUpdateBuilder.AddField("StartDate", StartDate, Enums.Data.DataType.Date);
            base.BaseUpdateBuilder.AddField("EndDate", EndDate, Enums.Data.DataType.Date);
            base.BaseUpdateBuilder.AddField("IsActive", GetActiveValue(), Enums.Data.DataType.Number);
            base.BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number, SPA.Enums.Data.AssignmentOperator.IsEqual);
            return base.ExecuteUpdate();
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
            throw new NotImplementedException();
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            throw new NotImplementedException();
        }

        public bool CheckExist()
        {
            SelectBuilder sb = new SelectBuilder();   
            sb.Sql = "SELECT TOP 1 ID FROM AccountSettings WHERE AccountID = " + this.AccountID;
            sb.Sql += " AND IsActive = 1";
            SqlDataReader dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                return true;
            }
            return false;
        }

        private int GetActiveValue()
        {
            int value = 1;
            if (EndDate < DateTime.Today)
            {
                value = 0;
            }
            else if (StartDate > DateTime.Today)
            {
                value = 0;                
            }

            return value;
        }


        #endregion
    }
}
