using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class MemberLoan : EntityBase, IEntity 
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public double Base { get; set; }
        public double Interest { get; set; }
        public double BaseBalance { get; set; }
        public double InterestBalance { get; set; }
        public string LoanDate { get; set; }
        public string LoanStartDate { get; set; }
        public string LoanEndDate { get; set; }
        public int  LoanDuration { get; set; }
        public Enums.PaymentEnum.AccountID Type { get; set; }
        public string Sql { get; set; }
        public MemberLoan(): base("MembersLoan")
        {

        }
        public bool Delete()
        {
            BaseDeleteBuilder.AddWhereField("MemberID", MemberID);
            return base.ExecuteDelete();
        }

        public bool Create()
        {
            if (ID > 0)
            {
                return Update();
            }
            else
            {
                return Insert();
            }
        }

        private bool Update()
        {
            BaseUpdateBuilder.AddField("Base", Base, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("Interest", Interest, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("BaseBalance", BaseBalance, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("InterestBalance", InterestBalance, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("LoanDuration", LoanDuration, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("LoanDate", LoanDate, Enums.Data.DataType.Date);
            BaseUpdateBuilder.AddField("LoanStartDate", LoanStartDate, Enums.Data.DataType.Date);
            BaseUpdateBuilder.AddField("LoanEndDate", LoanEndDate, Enums.Data.DataType.Date);
            BaseUpdateBuilder.AddField("TypeID", (int)Type, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddWhereField("ID", ID);
            return base.ExecuteUpdate();
        }

        private bool Insert()
        {
            BaseInsertBuilder.AddField("MemberID", MemberID);
            BaseInsertBuilder.AddField("Base", Base, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("Interest", Interest, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("BaseBalance", BaseBalance, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("InterestBalance", InterestBalance, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("LoanDuration", LoanDuration, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("LoanDate", LoanDate, Enums.Data.DataType.Date);
            BaseInsertBuilder.AddField("LoanStartDate", LoanStartDate, Enums.Data.DataType.Date);
            BaseInsertBuilder.AddField("LoanEndDate", LoanEndDate, Enums.Data.DataType.Date);
            BaseInsertBuilder.AddField("TypeID", (int)Type , Enums.Data.DataType.Number);
            return base.ExecuteInsert();
        }

        public override int GetID()
        {
            throw new NotImplementedException();
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            throw new NotImplementedException();
        }


    }
}
