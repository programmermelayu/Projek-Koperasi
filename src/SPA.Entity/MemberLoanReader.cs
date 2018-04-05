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
    public class MemberLoanReader : IEntityReader<MemberLoan>
    {
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

        public MemberLoan _singleRecord { get; set; }
        public MemberLoan SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new MemberLoan();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        public List<MemberLoan> MultipleRecords
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

        public bool ReadSingle(FilterElement filter)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filter));
            return CreateSingleRecordDataReader(dr);   
        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            SqlDataReader dr = DbReader.Execute(GetQuery(filterCollection));
            return CreateSingleRecordDataReader(dr);
        }

        public bool ReadMultiple()
        {
            throw new NotImplementedException();
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
            CreateQueryHeader(sb);
            if (filter != null)
            {
                sb.AddWhereField("MemberID", new Member().GetID(filter.Key, filter.Value), SPA.Enums.Data.DataType.Number);
            }
            return sb.Sql;
        }

        private string GetQuery(List<FilterElement> filterCollection)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            if (filterCollection != null)
            {
                foreach (var filter in filterCollection)
                {
                    switch (filter.Key)
                    {
                        case Enums.Data.KeyElements.MemberID:
                            sb.AddWhereField("MemberID", filter.Value, SPA.Enums.Data.DataType.Number);
                            break;
                        case Enums.Data.KeyElements.UseColumnName:
                            sb.AddWhereField(filter.ColumnName, filter.Value);
                            break;
                    }

                }
            }

            return sb.Sql;
        }

        private bool CreateSingleRecordDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    SingleRecord.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    SingleRecord.MemberID = dr.GetValueInt("MemberID");
                    SingleRecord.Base = dr.GetValueDouble("Base");
                    SingleRecord.Interest = dr.GetValueDouble("Interest");
                    SingleRecord.BaseBalance = dr.GetValueDouble("BaseBalance");
                    SingleRecord.InterestBalance = dr.GetValueDouble("InterestBalance");
                    SingleRecord.LoanDuration = dr.GetValueInt("LoanDuration");
                    SingleRecord.LoanDate = dr.GetValueDate("LoanDate").ToString();
                    SingleRecord.LoanStartDate = dr.GetValueDate("LoanStartDate").ToString();
                    SingleRecord.LoanEndDate = dr.GetValueDate("LoanEndDate").ToString();
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }
        private void CreateQueryHeader(SelectBuilder sb)
        {
            sb.Fields.Add("ID");
            sb.Fields.Add("MemberID");
            sb.Fields.Add("Base");
            sb.Fields.Add("Interest");
            sb.Fields.Add("BaseBalance");
            sb.Fields.Add("InterestBalance");
            sb.Fields.Add("LoanDuration");
            sb.Fields.Add("LoanDate");
            sb.Fields.Add("LoanStartDate");
            sb.Fields.Add("LoanEndDate");
            sb.Table = "MembersLoan";

        }
    }
}
