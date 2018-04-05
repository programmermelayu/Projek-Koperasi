using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Enums
{
    public class Data
    {
        public enum DataType { 
            Text, 
            Number, 
            Date, 
            Boolean, 
            Range
        };
        public enum AssignmentOperator
        { 
            IsEqual, 
            IsNotEqual, 
            IsLessThan, 
            IsGreaterThan, 
            IsLessOrEqual, 
            IsGreaterOrEqual,
            Between,
            IsNotNull,
            IsNull
        }

        public enum KeyElements
        {
            AccountCode,
            AccountID,
            UseColumnName,
            MemberCode, 
            MemberID,
            MemberKP, 
            PeriodBetweenNumber,
            PeriodBetweenDate
        }
    }

    public class PaymentEnum
        {
            public enum AccountType
            {
                Monthly=1,
                Yearly=2,
                Lifetime=3
            }

            public enum AccountID
            {
                YuranBulanan = 1,
                KebajikanDermasiswa=2, 
                Saham=3, 
                FiMasuk=4,
                SimpananKhas=5,
                Pinjaman=20,
                PinjamanBiasa=21,
                PinjamanKhas=22,
                PinjamanMedisihat=23,   
                PinjamanKecemasan=24
            }

        }
    
    public class FormEnum
    {
        public enum EntryMode
        {
            New = 1,
            Edit 
        }

    }

    public class MemberEnum
    {
        public enum Status
        {
            Active = 1,
            NotActive = 2
        }

    }

    public class SettingEnum
    {

        public enum Setting
        {
            Account,
            Citizenship,
            Race,
            Religion,
            State,
            Wasi,
            MaritalStatus,
            Category
        }


    }

    public class UserEnum
    {
        public enum UserType
        {
            Administrator = 1,
            RegularUser
        }

    }

}

