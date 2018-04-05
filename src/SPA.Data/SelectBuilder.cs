using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SPA.Core;

namespace SPA.Data
{
    public class SelectBuilder : QueryFilter, IQuerySelect
    {

        public override string Table { get; set; }
    
        public string On { get; set; }
        public string OrderBy { get; set; }
        public string GroupBy { get; set; }
        public bool Distinct { get; set; }
        public int Top { get; set; }
        public ArrayList Fields { get; set; }
        private List<WhereElement> whereFields { get; set; }

        public SelectBuilder() 
        {
            Fields = new ArrayList();
        }

        private string _sql;
        public override string Sql
        {
            get {
                    if (string.IsNullOrEmpty(_sql))
                    {
                        var queryString = new StringBuilder();
                        queryString.Append("SELECT ");
                        if (Distinct)
                        {
                            queryString.Append(" DISTINCT ");
                        }

                        if (Top > 0)
                        {
                            queryString.Append(" TOP ");
                            queryString.Append(Top);
                        }

                        queryString.Append(WriteFields());
                        queryString.Append(" FROM ");
                        queryString.Append(Table);

                       

                        queryString.Append(WriteWhereStatement());

                        if (!string.IsNullOrEmpty(GroupBy))
                        {
                            queryString.Append(" GROUP BY ");
                            queryString.Append(this.GroupBy);                            
                        }


                        if (!string.IsNullOrEmpty(OrderBy))
                        {
                            queryString.Append(" ORDER BY ");
                            queryString.Append(OrderBy);
                        }
                        _sql = queryString.ToString();
                   
                    }
                    return _sql;
                }
            set{
                _sql = value;
            }    
        }
        
        
        private string WriteFields()
        {
            var fields = new StringBuilder(); 
            foreach (var field in Fields)
            {
                fields.Append(field.ToString());
                fields.Append(",");
            }
            return fields.ToString().RemoveLastCharacter(','); ;
        }

    }

}

                