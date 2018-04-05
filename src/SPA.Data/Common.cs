using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Enums;

namespace SPA.Data
{
    public class Common
    {
        public static string GetAssignmentOperatorSymbol(Enums.Data.AssignmentOperator operatorDescription)
        {
            var operatorSymbol = "=";
            switch (operatorDescription)
            {
                case Enums.Data.AssignmentOperator.IsEqual:
                    operatorSymbol = "=";
                    break;
                case Enums.Data.AssignmentOperator.IsNotEqual:
                    operatorSymbol = "<>";
                    break;
                case Enums.Data.AssignmentOperator.IsLessThan:
                    operatorSymbol = "<";
                    break;
                case Enums.Data.AssignmentOperator.IsGreaterThan:
                    operatorSymbol = ">";
                    break;
                case Enums.Data.AssignmentOperator.IsLessOrEqual:
                    operatorSymbol = "<=";
                    break;
                case Enums.Data.AssignmentOperator.IsGreaterOrEqual:
                    operatorSymbol = ">=";
                    break;
                case Enums.Data.AssignmentOperator.Between:
                    operatorSymbol = " BETWEEN ";
                    break;
                case Enums.Data.AssignmentOperator.IsNull:
                    operatorSymbol = " IS NULL ";
                    break;
                case Enums.Data.AssignmentOperator.IsNotNull:
                    operatorSymbol = " IS NOT NULL ";
                    break;
                default:
                    break;
            }
            return operatorSymbol;


        }

        public static string GetMemberStatusDescription(MemberEnum.Status status)
        {
            var description = string.Empty;
            switch (status)
            {
                case  MemberEnum.Status.Active:
                    description = "Aktif";
                    break;
                case MemberEnum.Status.NotActive:
                    description = "Tidak Aktif";
                    break;
                default:
                    description = "NA";
                    break;
            }
            return description;
        }
    }

}
