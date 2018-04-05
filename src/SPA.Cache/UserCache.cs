using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Enums;

namespace SPA.Cache
{
    public class UserCache
    {
        public static int CurrentUserID { get; set; }
        public static string CurrentName { get; set; }
        public static string CurrentUserName { get; set; }
        public static string CurrentUserPassword { get; set; }
        public static UserEnum.UserType CurrentUserType { get; set; }
    }
}
