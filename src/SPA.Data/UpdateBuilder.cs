using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SPA.Core;
using SPA.Cache;

namespace SPA.Data
{
    public class UpdateBuilder : QueryFilter, IQuerySave 
    {
        public override string Table { get; set; }
        private List<FieldElement> fields { get; set; }

        private string _sql;
        public override string Sql
        {
            get
            {
                if (string.IsNullOrEmpty(_sql))
                {
                    AppendDefaultFields();
                    var queryString = new StringBuilder();
                    queryString.Append("UPDATE ");
                    queryString.Append(Table);
                    queryString.Append(" SET ");
                    queryString.Append(WriteSetStatement());
                    queryString.Append(WriteWhereStatement());
                    _sql = queryString.ToString();
                }
                return _sql;
            }
        }

        public void AddField(string column, object value)
        {
            if (fields == null)
            {
                fields = new List<FieldElement>();
            }
            fields.Add(new FieldElement(column, value));
        }

        public void AddField(string column, object value, Enums.Data.DataType type)
        {
            if (fields == null)
            {
                fields = new List<FieldElement>();
            }
            fields.Add(new FieldElement(column, value, type));
        }

        private string WriteSetStatement()
        {
            var columns = new StringBuilder();
            foreach (var field in fields)
            {
                columns.Append(field.Column);
                columns.Append(" = ");
                switch (field.ValueType)
                {
                    case Enums.Data.DataType.Date:
                        if (field.Value == null)
                        {
                            columns.Append("null");
                        }
                        else
                        {
                            columns.Append("'" + field.Value.ToString().ToDateDbFormatted() + "'");
                        }
                        break;
                    case Enums.Data.DataType.Number:
                    case Enums.Data.DataType.Boolean:
                        columns.Append(field.Value);
                        break;
                    case Enums.Data.DataType.Text:
                        columns.Append("'" + GetSecuredString(field.Value) + "'");
                        break;
                    default:
                        break;
                }


                columns.Append(",");
                
            }

            return columns.ToString().RemoveLastCharacter(','); ;
            
        }

        private string GetSecuredString(object value)
        {
            if (value != null)
            {
                string securedText = value.ToString().Trim();
                securedText = securedText.Replace("'", "''");
                return securedText;
            }
            else
            {
                return string.Empty;
            }
        }

        private void AppendDefaultFields()
        {
            fields.Add(new FieldElement("sysmodified", DateTime.Now.ToString().ToLongDateDbFormatted(), Enums.Data.DataType.Date));
            fields.Add(new FieldElement("sysmodifier", UserCache.CurrentUserID, Enums.Data.DataType.Number));
        }
      
       
    }
}
