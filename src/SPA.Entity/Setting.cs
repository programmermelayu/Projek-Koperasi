using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Enums;
using SPA.Data;
using System.Data.SqlClient;
using SPA.Core;

namespace SPA.Entity
{
    public class Setting: EntityBase, IEntity 
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Sql { get; set; }
        
        private SettingEnum.Setting settingName;
        public Setting(SettingEnum.Setting name)
        {
            settingName = name;
            base.TableName = "_" + GetTableName();
        }

        public Setting()
        {

        }

        public bool Create()
        {
            if (ID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Insert();
            }
        }

        private bool Insert()
        {
            BaseInsertBuilder.AddField("ID", GetNextID());
            BaseInsertBuilder.AddField("Description", Description);
            return base.ExecuteInsert();
        }
       
        private bool Update()
        {
            base.BaseUpdateBuilder.AddField("Description", Description);
            base.BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number, SPA.Enums.Data.AssignmentOperator.IsEqual);
            return base.ExecuteUpdate();
        }

        public bool Delete()
        {
            base.BaseDeleteBuilder.AddWhereField("ID", ID);
            return base.ExecuteDelete();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

     
        private string GetTableName()
        {
            string tableName = string.Empty;
            switch (settingName)
            {
                case SettingEnum.Setting.Account:
                    break;
                case SettingEnum.Setting.Citizenship:
                    tableName = "Citizenship";
                    break;
                case SettingEnum.Setting.Race:
                    tableName = "Race";
                    break;
                case SettingEnum.Setting.Religion:
                    tableName = "Religion";
                    break;
                case SettingEnum.Setting.State:
                    tableName = "State";
                    break;
                case SettingEnum.Setting.Wasi:
                    tableName = "Wasi";
                    break;
                case SettingEnum.Setting.MaritalStatus:
                    tableName = "MaritalStatus";
                    break;
                case SettingEnum.Setting.Category:
                    tableName = "Category";
                    break;
                default:
                    break;
            }
            return tableName;
        }

        private string GetUsedInTable()
        {
            string tableName = string.Empty;
            switch (settingName)
            {
                case SettingEnum.Setting.State:
                case SettingEnum.Setting.Religion:
                case SettingEnum.Setting.Race:
                case SettingEnum.Setting.MaritalStatus:
                case SettingEnum.Setting.Citizenship:
                case SettingEnum.Setting.Category:
                    tableName = "Members";
                    break;
                case SettingEnum.Setting.Wasi:
                    tableName = "MembersWasi";
                    break;
                default:
                    break;
            }
            return tableName;
        }

        public override int GetID()
        {
            var reader = new SettingReader(settingName);
            if (reader.ReadSingle(new FilterElement() { Key = Enums.Data.KeyElements.UseColumnName, ColumnName ="Description",  Value = this.Description }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            throw new NotImplementedException();
        }

        private int GetNextID()
        {
            int nextId = 1;
            SelectBuilder sb = new SelectBuilder();
            sb.Fields.Add("MAX(ID) as MaxID");
            sb.Table = "_" + GetTableName();
            SqlDataReader dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    nextId = dr.GetValue(dr.GetOrdinal("MaxID")).ToString().ToInt() + 1;
                }
            }
            return nextId;
        }

        public bool IsBeingUsed()
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Top = 1;
            sb.Fields.Add("ID");
            sb.Table = GetUsedInTable();
            sb.AddWhereField(GetTableName() + "ID", this.ID, Enums.Data.DataType.Number);
            SqlDataReader dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                return true;
            }
            return false;
        }

        public bool CheckExist()
        {
            SelectBuilder sb = new SelectBuilder();
            sb.Top = 1;
            sb.Fields.Add("ID");
            sb.Table = "_" + GetTableName();
            sb.AddWhereField("Description", this.Description);
            SqlDataReader dr = DbReader.Execute(sb.Sql);
            if (dr.HasRecord())
            {
                return true;
            }
            return false;
        }

    }
}
