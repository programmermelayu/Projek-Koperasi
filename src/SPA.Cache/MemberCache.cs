using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Entity;

namespace SPA.Cache
{
    public static class MemberCache
    {
        public static List<Member> Members { get; set; }
        public static void LoadMembers()
        {

                Members = new List<Member>();
                MemberReader reader = new MemberReader();
                if (reader.ReadMultiple())
                {
                    foreach (var record in reader.MultipleRecords)
                    {
                        Members.Add(record);
                    }
                }
            
        }

        public static int GetMemberID(string key)
        {
            Member member = GetMember(key);
            return (member != null) ? member.ID : -1;
        }

        public static Member GetMember(string key)
        {
            var member = Members.Where(x => x.Code.Trim() == key).FirstOrDefault();// Members.FirstOrDefault(x => x.Code.Trim() == key);
            if (member == null)
            {
                member = Members.Where(x => x.NewIC.Trim() == key).FirstOrDefault();
            }
            return member;
        }

    }
}
