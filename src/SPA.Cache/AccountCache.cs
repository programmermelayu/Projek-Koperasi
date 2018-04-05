using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Entity;

namespace SPA.Cache
{
    public static class AccountCache
    {
        public static List<Account> Accounts { get; set; }
        public static List<AccountSetting> AccountSettings { get; set; }

        public static void LoadAccounts()
        {
            if (Accounts == null)
            {
                Accounts = new List<Account>();
                AccountReader reader = new AccountReader();
                if (reader.ReadMultiple())
                {
                    foreach (var record in reader.MultipleRecords)
                    {
                        Accounts.Add(record);
                    }                    
                }
            }
        }

        public static void LoadAccountSettings()
        {
            AccountSettings = new List<AccountSetting>();
            AccountSettingReader reader = new AccountSettingReader();
            if (reader.ReadMultiple(new FilterElement() { Key = Enums.Data.KeyElements.UseColumnName, ColumnName = "IsActive", Value = 1 }))
            {
                foreach (var accountSetting in reader.MultipleRecords)
                {
                    AccountSettings.Add(accountSetting);
                }
            }
        }

        public static int GetAccountID(string code)
        {
            //map obsolete code to the new code for Pinjaman Khas
            if (code == "4601") code = "0301";
     
            var account = Accounts.FirstOrDefault(x => x.Code.Trim() == code);
            return (account != null) ? account.ID : -1;
        }

        public static string GetAccountCode(int accountId)
        {
            var account = Accounts.FirstOrDefault(x => x.ID == accountId);
            return (account != null) ? account.Code  : "-";
        }

        public static string GetAccountDescription(int accountId)
        {
            var account = Accounts.FirstOrDefault(x => x.ID == accountId);
            return (account != null) ? account.Description : string.Empty;
        }

        public static double GetBaseAmount(int accountId)
        {
            var accountSetting = AccountSettings.FirstOrDefault(x => x.AccountID == accountId);
            return (accountSetting != null) ? accountSetting.Amount : 0;
        }
    }
}
