using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Data;
using SPA.Core;
using System.Data.SqlClient;

namespace SPA.Entity
{
    public class UserReader : IEntityReader<User>
    {
        public string ErrorMessage { get; set; }
        public string SystemErrorMessage { get; set; }

        private User _singleRecord;
        public User SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new User();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        private List<User> _multipleRecords;
        public List<User> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<User>();
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
            return CreateSingleRecordsDataReader(dr);   
        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filterCollection));
            return CreateSingleRecordsDataReader(dr);   
        }

        public bool ReadMultiple()
        {
            SqlDataReader dr = DbReader.Execute(GetQuery());
            return CreateMultipleRecordsDataReader(dr);   
        }

        public bool ReadMultiple(List<FilterElement> filterCollection)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filterCollection));
            return CreateMultipleRecordsDataReader(dr);   
        }

        public bool ReadMultiple(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            return CreateMultipleRecordsDataReader(dr);   
        }

        private bool CreateMultipleRecordsDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    User user = new User();
                    user.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    user.Name = dr.GetString(dr.GetOrdinal("Name"));
                    user.Username = dr.GetString(dr.GetOrdinal("Username"));
                    user.Password = dr.GetString(dr.GetOrdinal("Password"));
                    user.UserTypeID = dr.GetInt32(dr.GetOrdinal("UserTypeID"));
                    MultipleRecords.Add(user);
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool CreateSingleRecordsDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    SingleRecord.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    SingleRecord.Name = dr.GetString(dr.GetOrdinal("Name"));
                    SingleRecord.Username = dr.GetString(dr.GetOrdinal("Username"));
                    SingleRecord.Password = dr.GetString(dr.GetOrdinal("Password"));
                    SingleRecord.UserTypeID = dr.GetInt32(dr.GetOrdinal("UserTypeID"));
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            if (filter != null)
            {
                if (filter.Key == Enums.Data.KeyElements.UseColumnName)
                {
                    sb.AddWhereField(filter.ColumnName, filter.Value);
                }                
            }

            return sb.Sql; 
        }

        private string GetQuery(List<FilterElement> filterCollection)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            foreach (var filter in filterCollection)
            {
                switch (filter.Key)
                {
                    case Enums.Data.KeyElements.UseColumnName:
                        sb.AddWhereField(filter.ColumnName, filter.Value);
                        break;
                }

            }

            return sb.Sql;
        }

        private string GetQuery()
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            return sb.Sql;
        }

        private void CreateQueryHeader(SelectBuilder sb)
        {
            sb.Fields.Add("ID");
            sb.Fields.Add("Name");
            sb.Fields.Add("Username");
            sb.Fields.Add("Password");
            sb.Fields.Add("UserTypeID");
            sb.Table = "Users";
        }
    }
}
