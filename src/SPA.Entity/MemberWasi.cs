using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class MemberWasi : EntityBase, IEntity
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string MyKad { get; set; }
        public int WasiID { get; set; }
        public string Phone { get; set; }
        public string Birthdate { get; set; }

        public MemberWasi(): base("MembersWasi")
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
            BaseUpdateBuilder.AddField("Name", Name);
            BaseUpdateBuilder.AddField("MyKad", MyKad);
            BaseUpdateBuilder.AddField("Phone", Phone);
            BaseUpdateBuilder.AddField("WasiID", WasiID);
            BaseUpdateBuilder.AddField("Birthdate", Birthdate, Enums.Data.DataType.Date);
            BaseUpdateBuilder.AddWhereField("ID", ID);
            return base.ExecuteUpdate();
        }

        private bool Insert()
        {
            BaseInsertBuilder.AddField("MemberID", MemberID);
            BaseInsertBuilder.AddField("Name", Name);
            BaseInsertBuilder.AddField("MyKad", MyKad);
            BaseInsertBuilder.AddField("Phone", Phone);
            BaseInsertBuilder.AddField("WasiID", WasiID);
            BaseInsertBuilder.AddField("Birthdate", Birthdate, Enums.Data.DataType.Date);
            return base.ExecuteInsert();
        }

        public string Sql{get;set;}
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
