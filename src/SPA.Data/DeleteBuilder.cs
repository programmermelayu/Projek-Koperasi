using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SPA.Data
{
    public class DeleteBuilder : QueryFilter 
    {
        //public string Table { get; set; }
        //private List<WhereElement> whereFields { get; set; }
       
        public override string Sql
        {
            get
            {
                var queryString = new StringBuilder();
                queryString.Append("DELETE FROM ");
                queryString.Append(Table);
                queryString.Append(WriteWhereStatement());
                return queryString.ToString();
            }
        }

        //public void AddWhereField(string column, object value)
        //{
        //    if (whereFields == null)
        //    {
        //        whereFields = new List<WhereElement>();
        //    }
        //    whereFields.Add(new WhereElement(column, value, Enums.Data.DataType.Text));
           
        //}

        //public void AddWhereField(string column, object value, Enums.Data.DataType type)
        //{
        //    if (whereFields == null)
        //    {
        //        whereFields = new List<WhereElement>();
        //    }
        //    whereFields.Add(new WhereElement(column, value, type));

        //}

        //private string WriteWhereStatement()
        //{
        //    var columns = new StringBuilder();
        //    if (whereFields.Count > 0)
        //    {
        //        columns.Append(" WHERE ");                
        //    }
        //    foreach (var field in whereFields)
        //    {
        //        columns.Append(field.Column);
        //        columns.Append(" = ");
        //        if (field.ValueType == Enums.Data.DataType.Number)
        //        {
        //            columns.Append(field.Value);
        //        }
        //        else
        //        {
        //            columns.Append("'" + field.Value + "'");
        //        }
        //        columns.Append(" AND ");
        //    }


        //    string setStatement = columns.ToString();
        //    setStatement = setStatement.Remove(setStatement.LastIndexOf("AND"));
        //    return setStatement;

        //}
     
        //private string GetStatementClaused(string clauseText)
        //{
        //    string statementClaused = clauseText.ToString();
        //    statementClaused = statementClaused.Remove(statementClaused.LastIndexOf(','));
        //    statementClaused = "(" + statementClaused + ")";
        //    return statementClaused;

        //}

    }
}
