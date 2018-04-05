using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Enums;

namespace SPA.Data
{
    public class WhereElement : PairElement 
    {
        public Enums.Data.AssignmentOperator AssignmentOperator { get; set; }
  
        public WhereElement(string column, object value)
        {
            Column = column;
            Value = value;
            ValueType = Enums.Data.DataType.Text;
            AssignmentOperator = Enums.Data.AssignmentOperator.IsEqual;
        }

        public WhereElement(string column, object value, Enums.Data.DataType type)
        {
            Column = column;
            Value = value;
            ValueType = type;
            AssignmentOperator = Enums.Data.AssignmentOperator.IsEqual;
        }


        public WhereElement(string column, object value, Enums.Data.AssignmentOperator assignmentOperator)
        {
            Column = column;
            Value = value;
            ValueType = Enums.Data.DataType.Text;
            AssignmentOperator = assignmentOperator;
        }

        public WhereElement(string column, object value, Enums.Data.DataType type, Enums.Data.AssignmentOperator assignmentOperator)
        {
            Column = column;
            Value = value;
            ValueType = type;
            AssignmentOperator = assignmentOperator;
        }

        public WhereElement(string column, object valueMin, object valueMax)
        {
            Column = column;
            ValueMin = valueMin;
            ValueMax = valueMax;
            ValueType = SPA.Enums.Data.DataType.Range;
            AssignmentOperator = SPA.Enums.Data.AssignmentOperator.Between;
        }

        public WhereElement(string column,  Enums.Data.AssignmentOperator assignmentOperator)
        {
            Column = column;
            AssignmentOperator = assignmentOperator;
        }


    }


}
