using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Core
{
    public static class StringHandler
    {
        public static double ToDouble(this String str)
        {
            double output;
            if (!double.TryParse(str, out output))
            {
                output = 0.0;
            }
            return output;            
        }

        public static int ToInt(this String str)
        {
            str = str.Trim();
            int output;
            if (!int.TryParse(str, out output))
            {
                output = 0;
            }
            return output;
        }

        public static string ToDateFormatted(this String str)
        {
            str = str.Trim();
            DateTime output;
            if (!DateTime.TryParse(str, out output))
            {
               return string.Empty;
            }
           return String.Format("{0:dd-MMM-yyyy}", output);
        }

        public static string ToDateDbFormatted(this String str)
        {
            str = str.Trim();
            DateTime output;
            if (!DateTime.TryParse(str, out output))
            {
                return String.Format("{0:yyyy/MM/dd}", DateTime.Today);
            }
            return String.Format("{0:yyyy/MM/dd}", output);
        }

        public static string ToLongDateDbFormatted(this String str)
        {
            str = str.Trim();
            DateTime output;
            if (!DateTime.TryParse(str, out output))
            {
                return str;
            }
            return String.Format("{0:yyyy/MM/dd HH:mm:ss}", output);
        }

        public static string ToMonthFormatted(this String str)
        {
            str = str.Trim();
            DateTime output;
            if (!DateTime.TryParse(str, out output))
            {
                return str;
            }
            return String.Format("{0:MMM-yyyy}", output);
        }

        public static string ShowTo2Decimal(this string text)
        {
            double dAmount;
            if (double.TryParse(text, out  dAmount))
            {
                if (text.IndexOf(".") < 0)
                {
                    text += ".00";
                }
            }
            return text;
        }

        public static string GetCode(this string codeDescription)
        {
            string[] words = codeDescription.Split('-');
            return words[0].Trim();
        }

        public static string GetDescription(this string codeDescription)
        {
            string[] words = codeDescription.Split('-');
            return words[1].Trim();
        }

        public static string ToDateMalayFormatted(this String str, bool isAbbrev)
        {
            if (str.IndexOf("Jan") > -1)
            {
                str = (isAbbrev) ? str.Replace("Jan", "Jan") : str.Replace("Jan", "Januari");
            }

            if (str.IndexOf("Feb") > -1)
            {
                str = (isAbbrev) ? str.Replace("Feb", "Feb") : str.Replace("Feb", "Februari");
            }

            if (str.IndexOf("Mar") > -1)
            {
                str = (isAbbrev) ? str.Replace("Mar", "Mac") : str.Replace("Mar", "Mac");
            }

            if (str.IndexOf("May") > -1)
            {
                str = (isAbbrev) ? str.Replace("May", "Mei") : str.Replace("May", "Mei");
            }

            if (str.IndexOf("Jul") > -1)
            {
                str = (isAbbrev) ? str.Replace("Jul", "Jul") : str.Replace("Jul", "Julai");
            }

            if (str.IndexOf("Aug") > -1)
            {
                str = (isAbbrev) ? str.Replace("Aug", "Ogos") : str.Replace("Aug", "Ogos");
            }

            if (str.IndexOf("Oct") > -1)
            {
                str = (isAbbrev) ? str.Replace("Oct", "Okt") : str.Replace("Oct", "Oktober");
            }

            if (str.IndexOf("Dec") > -1)
            {
                str = (isAbbrev) ? str.Replace("Dec", "Dis") : str.Replace("Dec", "Disember");
            }

           
            return str;
        }

        public static string RemoveLastCharacter(this string statement, char charx)
        {
            return statement.Remove(statement.LastIndexOf(charx));
        }

        public static string RemoveLastPhrase(this string statement, string phrase)
        {
            return statement.Remove(statement.LastIndexOf(phrase));
        }

    }
}
