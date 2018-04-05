using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class PaymentTotal : EntityBase, IEntity 
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public double FiMasuk { get; set; }
        public double YuranBulanan { get; set; }
        public double TabungDerma { get; set; }
        public double Saham { get; set; }
        public double SimpananKhas { get; set; }
        public PaymentTotal()
            : base("PaymentsTotal")
        {

        }
        public bool Delete()
        {
            throw new NotImplementedException();
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

        public bool Insert()
        {
            BaseInsertBuilder.AddField("MemberID", MemberID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("FiMasuk", FiMasuk,  Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("YuranBulanan", YuranBulanan, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("TabungKebajikan", TabungDerma , Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("Saham", Saham,  Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("SimpananKhas", SimpananKhas, Enums.Data.DataType.Number);
            return base.ExecuteInsert();
        }

        public bool Update()
        {     
            BaseUpdateBuilder.AddField("MemberID", MemberID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("FiMasuk", FiMasuk, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("YuranBulanan", YuranBulanan,  Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("TabungKebajikan", TabungDerma,  Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("Saham", Saham,  Enums.Data.DataType.Number);
			BaseUpdateBuilder.AddField("SimpananKhas", SimpananKhas, Enums.Data.DataType.Number);
			BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number);
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

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            throw new NotImplementedException();
        }

        public override int GetID()
        {
            throw new NotImplementedException();
        }
    }
}
