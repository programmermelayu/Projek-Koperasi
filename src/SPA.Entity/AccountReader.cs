using SPA.Core;
using SPA.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class AccountReader : IEntityReader<Account>
    {
        #region IEntityReader<Account> Members

        public string ErrorMessage
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

        public string SystemErrorMessage { get; set; }

        private Account _singleRecord;
        public Account SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new Account();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        private List<Account> _multipleRecords;
        public List<Account> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<Account>();
                }
                return _multipleRecords;
            }
            set
            {
                _multipleRecords = value;
            }
        }

        public bool ReadSingle(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    SingleRecord.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    SingleRecord.Code = dr.GetString(dr.GetOrdinal("Code"));
                    SingleRecord.Description = dr.GetValue(dr.GetOrdinal("Description")).ToString();
                }

                return true;
            }
            else
            {
                return false;
            }
          

        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple()
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(null));
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    Account account = new Account();
                    account.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    account.Code = dr.GetString(dr.GetOrdinal("Code"));
                    account.Description = dr.GetValue(dr.GetOrdinal("Description")).ToString();
                    MultipleRecords.Add(account);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadMultiple(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple(FilterElement filter)
        {
            throw new NotImplementedException();
        }

        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("ID");
            sb.Fields.Add("Code");
            sb.Fields.Add("Description");
            sb.Table = "Accounts";
            if (filter != null)
            {
                if (filter.Key == Enums.Data.KeyElements.AccountCode)
                {
                    sb.AddWhereField("Code", filter.Value.ToString());
                }
            }

            return sb.Sql;
        }

        #endregion
    }
}
