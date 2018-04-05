using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA
{
    public static class Helper
    {
        public static int CalculateAgeInYears(DateTime birthDate)
        {

            DateTime Now = DateTime.Now;
            int years = new DateTime(DateTime.Now.Subtract(birthDate).Ticks).Year - 1;
            DateTime PastYearDate = birthDate.AddYears(years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            return years;
        }

    }
}
