using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public class FilterElement
    {
            public SPA.Enums.Data.KeyElements Key { get; set; }
            public string ColumnName { get; set; }
            public object Value { get; set; }
            public object Value2 { get; set; }
    }
}
