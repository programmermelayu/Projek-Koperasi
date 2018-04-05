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
    public class MemberWasiReader : IEntityReader<MemberWasi>
    {
        public string ErrorMessage { get; set; }
        public string SystemErrorMessage{get;set;}

        public MemberWasi SingleRecord
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

        private List<MemberWasi> _multipleRecords; 
        public List<MemberWasi> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<MemberWasi>();
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
            throw new NotImplementedException();
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

        private string GetQuery(FilterElement filter)
        {
            SelectBuilder sb = new SelectBuilder();
            CreateQueryHeader(sb);
            sb.AddWhereField("MemberID", filter.Value, SPA.Enums.Data.DataType.Number);
            return sb.Sql;
        }

        private bool CreateMultipleRecordsDataReader(SqlDataReader dr)
        {
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    MemberWasi wasi = new MemberWasi();
                    wasi.ID = dr.GetValueInt("ID");
                    wasi.Name = dr.GetValueString("Name");
                    wasi.Phone = dr.GetValueString("Phone");
                    wasi.MyKad = dr.GetValueString("MyKad");
                    wasi.Birthdate = dr.GetValueDate("Birthdate").ToString();
                    wasi.WasiID = dr.GetValueInt("WasiID");
             
                    MultipleRecords.Add(wasi);
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
            sb.Fields.Add("Name");
            sb.Fields.Add("MyKad");
            sb.Fields.Add("Phone");
            sb.Fields.Add("Birthdate");
            sb.Fields.Add("WasiID");

            sb.Table = "MembersWasi";
        }

    }
}
