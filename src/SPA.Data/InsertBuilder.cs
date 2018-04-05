using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SPA.Enums;
using SPA.Core;
using SPA.Cache;

namespace SPA.Data
{
    public class InsertBuilder : QueryFilter, IQuerySave 
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
                    queryString.Append("INSERT INTO ");
                    queryString.Append(Table);
                    queryString.Append(WriteColumns());
                    queryString.Append("VALUES");
                    queryString.Append(WriteValues());

                    _sql = queryString.ToString(); 
                }
                return _sql;
            }
            set
            {
                _sql = value;           
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

        private string WriteColumns()
        {
            var columns = new StringBuilder();
            foreach (var field in fields)
            {
                columns.Append(field.Column);
                columns.Append(",");
                
            }

            return GetStatementClaused(columns.ToString());
            
        }

        private string WriteValues()
        {
            var values = new StringBuilder();
            foreach (var field in fields)
            {
                switch (field.ValueType)
                {
                    case Enums.Data.DataType.Date:
                        if (field.Value == null) field.Value = string.Empty;
                        values.Append("'" + field.Value.ToString().ToDateDbFormatted() + "'");                                              
                        break;
                    case Enums.Data.DataType.Number:
                    case Enums.Data.DataType.Boolean:
                        values.Append(field.Value);
                        break;
                    case Enums.Data.DataType.Text:
                        values.Append("'" + GetSecuredString(field.Value) + "'");
                        break;
                    default:
                        break;
                }
                values.Append(",");
            }
            return GetStatementClaused(values.ToString());
        }

        private string GetStatementClaused(string clauseText)
        {
            return "(" + clauseText.RemoveLastCharacter(',') +")";;
        }

        private void AppendDefaultFields()
        {
            fields.Add(new FieldElement("syscreated", DateTime.Now.ToString().ToLongDateDbFormatted(), Enums.Data.DataType.Date));
            fields.Add(new FieldElement("sysmodified", DateTime.Now.ToString().ToLongDateDbFormatted(), Enums.Data.DataType.Date));
            fields.Add(new FieldElement("syscreator", UserCache.CurrentUserID, Enums.Data.DataType.Number));
            fields.Add(new FieldElement("sysmodifier",UserCache.CurrentUserID, Enums.Data.DataType.Number));
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

    }
}
