using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Data
{
    public class PairElement
    {
        public string Column { get; set; }
        public object Value { get; set; }

        public object ValueMin { get; set; }
        public object ValueMax { get; set; }
        public Enums.Data.DataType ValueType { get; set; }
    }
}
