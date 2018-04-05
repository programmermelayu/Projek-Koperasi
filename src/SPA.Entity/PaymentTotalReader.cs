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
    public class PaymentTotalReader : IEntityReader<PaymentTotal>
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

        private PaymentTotal _singleRecord;
        public PaymentTotal SingleRecord
        {
            get
            {
                if (_singleRecord == null)
                {
                    _singleRecord = new PaymentTotal();
                }
                return _singleRecord;
            }
            set
            {
                _singleRecord = value;
            }
        }

        public List<PaymentTotal> MultipleRecords
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
            throw new NotImplementedException();
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

        private bool CreateSingleRecordDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    SingleRecord.ID = dr.GetValue(dr.GetOrdinal("ID")).ToString().ToInt();
                    SingleRecord.MemberID = dr.GetValueInt("MemberID");
                    SingleRecord.FiMasuk = dr.GetValueDouble("FiMasuk");
                    SingleRecord.YuranBulanan = dr.GetValueDouble("YuranBulanan"); 
                    SingleRecord.TabungDerma = dr.GetValueDouble("TabungKebajikan");
                    SingleRecord.Saham = dr.GetValueDouble("Saham");
                    SingleRecord.SimpananKhas = dr.GetValueDouble("SimpananKhas"); 
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
            sb.Fields.Add("FiMasuk");
            sb.Fields.Add("YuranBulanan");
            sb.Fields.Add("TabungKebajikan");
            sb.Fields.Add("Saham");
            sb.Fields.Add("SimpananKhas");
            sb.Table = "PaymentsTotal";
            sb.OrderBy = "Syscreated DESC";

        }
    }
}
