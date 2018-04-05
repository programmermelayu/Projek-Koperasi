using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class User : EntityBase, IEntity 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserTypeID { get; set; }

        public User() : base("Users")
        {

        }

        public bool Delete()
        {
            base.BaseDeleteBuilder.AddWhereField("ID", ID);
            return base.ExecuteDelete();
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
            BaseInsertBuilder.AddField("Name", Name);
            BaseInsertBuilder.AddField("Username", Username);
            BaseInsertBuilder.AddField("Password", Password);
            BaseInsertBuilder.AddField("UserTypeID", UserTypeID);
            return base.ExecuteInsert();
        }

        private bool Update()
        {
            if (!string.IsNullOrEmpty(Name)) BaseUpdateBuilder.AddField("Name", Name);
            if (!string.IsNullOrEmpty(Password)) BaseUpdateBuilder.AddField("Password", Password);
            if(UserTypeID > 0) BaseUpdateBuilder.AddField("UserTypeID", UserTypeID);
     
            BaseUpdateBuilder.AddWhereField("ID", ID);
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

        public bool CheckExist()
        {
            UserReader reader = new UserReader();
            FilterElement filterUsername = new FilterElement() { Key = Enums.Data.KeyElements.UseColumnName, ColumnName = "Username", Value = Username };
            if (reader.ReadSingle(filterUsername)) return true;
            return false;
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
