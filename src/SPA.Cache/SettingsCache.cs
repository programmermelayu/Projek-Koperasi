using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Entity;

namespace SPA.Cache
{
    public class SettingsCache
    {

        public static List<Setting> States { get; set; }
        public static List<Setting> Religions { get; set; }
        public static List<Setting> Races { get; set; }
        public static List<Setting> Citizenships { get; set; }
        public static List<Setting> Wasis { get; set; }
        public static List<Setting> MaritalStatuses { get; set; }
        public static List<Setting> Categories { get; set; }

        public static void LoadSettings()
        {
            LoadStates();
            LoadReligions();
            LoadCitizenships();
            LoadWasis();
            LoadRaces();
            LoadMaritalStatuses();
            LoadCategories();
        }

        public static int GetID(Enums.SettingEnum.Setting settingName, string description)
        {
            Setting setting = null;
            switch (settingName)
            {
                case SPA.Enums.SettingEnum.Setting.Citizenship:
                    setting  = Citizenships.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.Race:
                    setting = Races.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.Religion:
                    setting = Religions.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.State:
                    setting = States.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.Wasi:
                    setting = Wasis.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.MaritalStatus:
                    setting = MaritalStatuses.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                case SPA.Enums.SettingEnum.Setting.Category:
                    setting = Categories.FirstOrDefault(x => x.Description.Trim() == description);
                    break;
                default:
                    break;
            }
            return (setting == null) ? -1 : setting.ID;
        }

        public static string GetDescription(Enums.SettingEnum.Setting settingName, int settingID)
        {
            Setting setting = null;
            switch (settingName)
            {
                case SPA.Enums.SettingEnum.Setting.Citizenship:
                    setting = Citizenships.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.Race:
                    setting = Races.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.Religion:
                    setting = Religions.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.State:
                    setting = States.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.Wasi:
                    setting = Wasis.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.MaritalStatus:
                    setting = MaritalStatuses.FirstOrDefault(x => x.ID == settingID);
                    break;
                case SPA.Enums.SettingEnum.Setting.Category:
                    setting = Categories.FirstOrDefault(x => x.ID == settingID);
                    break;
                default:
                    break;
            }
            return (setting == null) ? string.Empty : setting.Description;
        }

        private static void LoadStates()
        {
            States = new List<Setting>();
            LoadSetting(States, Enums.SettingEnum.Setting.State);
        }

        private static void LoadMaritalStatuses()
        {
            MaritalStatuses = new List<Setting>();
            LoadSetting(MaritalStatuses, Enums.SettingEnum.Setting.MaritalStatus);
        }

        private static void LoadRaces()
        {
            Races = new List<Setting>();
            LoadSetting(Races, Enums.SettingEnum.Setting.Race);
        }

        private static void LoadReligions()
        {
            Religions = new List<Setting>();
            LoadSetting(Religions, Enums.SettingEnum.Setting.Religion);
        }

        private static void LoadCitizenships()
        {
            Citizenships = new List<Setting>();
            LoadSetting(Citizenships, Enums.SettingEnum.Setting.Citizenship);
        }

        private static void LoadCategories()
        {
            Categories = new List<Setting>();
            LoadSetting(Categories, Enums.SettingEnum.Setting.Category);
        }

        private static void LoadWasis()
        {
            Wasis = new List<Setting>();
            LoadSetting(Wasis, Enums.SettingEnum.Setting.Wasi);
        }

        private static void LoadSetting(List<Setting> settings, Enums.SettingEnum.Setting settingName)
        {
            SettingReader reader = new SettingReader(settingName);
            if (reader.ReadMultiple())
            {
                foreach (var record in reader.MultipleRecords)
                {
                    settings.Add(record);
                }
            }
        }


    }
}
