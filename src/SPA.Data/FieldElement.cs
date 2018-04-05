using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Data
{
    public class FieldElement : PairElement 
    {
       
        public FieldElement(string column, object value, Enums.Data.DataType type)
        {
            Column = column;
            Value = value;
            ValueType = type;
        }

        public FieldElement(string column, object value)
        {
            Column = column;
            Value = value;
            ValueType = Enums.Data.DataType.Text;
        }
    }
  
}
