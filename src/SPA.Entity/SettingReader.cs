using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Data;
using SPA.Core;
using SPA.Enums;

namespace SPA.Entity
{
    public class SettingReader : IEntityReader<Setting>
    {
        private SettingEnum.Setting settingName;
        public SettingReader(SettingEnum.Setting name)
        {
            settingName = name;
        }

        public string ErrorMessage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SystemErrorMessage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Setting SingleRecord
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        private List<Setting> _multipleRecords;
        public List<Setting> MultipleRecords
        {
            get
            {
                if (_multipleRecords == null)
                {
                    _multipleRecords = new List<Setting>();
                }
                return _multipleRecords;
            }

            set
            {
                _multipleRecords = value;
            }
        }


        public bool ReadSingle(FilterElement filter)
        {
            throw new NotImplementedException();
        }

        public bool ReadSingle(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple()
        {
            SqlDataReader dr = DbReader.Execute(GetQuery());
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    Setting setting = new Setting(settingName);
                    setting.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    setting.Description = dr.GetValue(dr.GetOrdinal("Description")).ToString();
                    MultipleRecords.Add(setting);
                }
                dr.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ReadMultiple(List<FilterElement> filterCollection)
        {
            throw new NotImplementedException();
        }

        public bool ReadMultiple(FilterElement filter)
        {
            throw new NotImplementedException();
        }

        private string GetQuery()
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Table = "_" + GetTableName(sb);
            sb.Fields.Add("ID");
            sb.Fields.Add("Description");
            return sb.Sql;
        }

        private string GetTableName(SelectBuilder sb)
        {
            string tableName = string.Empty;
            switch (settingName)
            {
                case SettingEnum.Setting.Citizenship:
                    tableName = "Citizenship";
                    sb.OrderBy = "ID ASC";
                    break;
                case SettingEnum.Setting.Race:
                    tableName = "Race";
                    sb.OrderBy = "ID ASC";
                    break;
                case SettingEnum.Setting.Religion:
                    tableName = "Religion";
                    sb.OrderBy = "ID ASC";
                    break;
                case SettingEnum.Setting.State:
                    tableName = "State";
                    sb.OrderBy = "Description ASC";
                    break;
                case SettingEnum.Setting.Wasi:
                    tableName = "Wasi";
                    sb.OrderBy = "Description ASC";
                    break;
                case SettingEnum.Setting.MaritalStatus:
                    tableName = "MaritalStatus";
                    sb.OrderBy = "ID ASC";
                    break;
                case SettingEnum.Setting.Category:
                    tableName = "Category";
                    sb.OrderBy = "ID ASC";
                    break;
                default:
                    break;
            }
            return tableName;
        }

    }
}
