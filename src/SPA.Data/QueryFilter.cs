using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Core;
using SPA.Enums;

namespace SPA.Data
{
    public class QueryFilter : Query 
    {
        private List<WhereElement> whereFields { get; set; }

        public void AddWhereField(string column, object value)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, value, Enums.Data.DataType.Text));
        }

        public void AddWhereField(string column, object value, Enums.Data.DataType type)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, value, type));
        }

        public void AddWhereField(string column, object value, Enums.Data.AssignmentOperator assignmentOperator)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, value, assignmentOperator));
        }

        public void AddWhereField(string column, object value, Enums.Data.DataType type, Enums.Data.AssignmentOperator assignmentOperator)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, value, type, assignmentOperator));
        }

        public void AddWhereField(string column, object valueMin, object valueMax)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, valueMin, valueMax));
        }

        public void AddWhereField(string column, Enums.Data.AssignmentOperator assignmentOperator)
        {
            if (whereFields == null)
            {
                whereFields = new List<WhereElement>();
            }
            whereFields.Add(new WhereElement(column, assignmentOperator));
        }

        protected string WriteWhereStatement()
        {
            var columns = new StringBuilder();
            if (whereFields == null || whereFields.Count < 1)
            {
                return string.Empty;
            }
            columns.Append(" WHERE ");
            foreach (var field in whereFields)
            {
                columns.Append(GetWhereField(field));

                columns.Append(Common.GetAssignmentOperatorSymbol(field.AssignmentOperator));
                if (field.AssignmentOperator == Enums.Data.AssignmentOperator.IsNull || 
                    field.AssignmentOperator == Enums.Data.AssignmentOperator.IsNotNull)
                {
                    columns.Append(" AND ");
                    continue;
                }

                switch (field.ValueType)
                {
                    case SPA.Enums.Data.DataType.Boolean:
                    case SPA.Enums.Data.DataType.Number:
                        columns.Append(field.Value);
                        break;
                    case SPA.Enums.Data.DataType.Date:
                    case SPA.Enums.Data.DataType.Text:
                        columns.Append("'" + field.Value + "'");
                        break;
                    case SPA.Enums.Data.DataType.Range:
                        columns.Append(field.ValueMin);
                        columns.Append(" AND ");
                        columns.Append(field.ValueMax);
                        columns.Append(")");
                        break;
                    default:
                        columns.Append("'" + field.Value + "'");
                        break;
                }
                columns.Append(" AND ");
            }

            return columns.ToString().RemoveLastPhrase("AND"); 
          
        }

        private string GetWhereField(WhereElement field)
        {
            if (field.AssignmentOperator == Enums.Data.AssignmentOperator.Between)
            {
                if (field.Column.StartsWith("("))
                {
                    return field.Column;
                }
                else
                {
                    return "(" + field.Column;
                }
            }
            else
            {
                return field.Column;
            }
        }

    }
}
