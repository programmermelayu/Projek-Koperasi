using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Data;
using System.Data.SqlClient;
using SPA.Core;

namespace SPA.Entity
{
    public class AccountSettingReader : IEntityReader<AccountSetting>
    {
     
        #region IEntityReader<AccountSetting> Members

        public string ErrorMessage { get; set; }
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

        public AccountSetting SingleRecord
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

        private List<AccountSetting> _multipleRecords;
        public List<AccountSetting> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<AccountSetting>();
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
            throw new NotImplementedException();
        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple()
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(null));
            return CreateMultipleRecordsDataReader(dr);
        }

        public bool ReadMultiple(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            return CreateMultipleRecordsDataReader(dr);           
        }

        #endregion
        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("s.ID, a.Code as AccountCode, a.ID as AccountID, Amount, IsActive, StartDate, EndDate");
            sb.Table = "AccountSettings s";
            sb.Table += " INNER JOIN Accounts a ON s.AccountID = a.ID";
            if (filter != null)
            {
                sb.AddWhereField(filter.ColumnName, filter.Value.ToString(), SPA.Enums.Data.DataType.Boolean);
            }

            return sb.Sql;
        }

        private bool CreateMultipleRecordsDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    var accountSetting = new AccountSetting();
                    accountSetting.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    accountSetting.AccountID = dr.GetInt32(dr.GetOrdinal("AccountID"));
                    accountSetting.AccountCode = dr.GetString(dr.GetOrdinal("AccountCode"));
                    accountSetting.Amount = dr.GetDouble(dr.GetOrdinal("Amount"));
                    accountSetting.IsActive = dr.GetBoolean(dr.GetOrdinal("IsActive"));
                    accountSetting.StartDate = dr.GetDateTime(dr.GetOrdinal("StartDate"));
                    accountSetting.EndDate = dr.GetDateTime(dr.GetOrdinal("EndDate"));
                    MultipleRecords.Add(accountSetting);
                } dr.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
