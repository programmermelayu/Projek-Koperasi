using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SPA.Core;
using SPA.Data;
using SPA.Enums;


namespace SPA.Entity
{
    public abstract class EntityBase
    {
        protected string TableName;
        public string SystemErrorMessage { get; set; }
        public abstract int GetID();
        public abstract int GetID(Enums.Data.KeyElements key, object value);

        public EntityBase()
        {

        }

        public EntityBase(string table)
        {
            TableName = table;
        }

        InsertBuilder _baseInsertBuilder;
        public InsertBuilder BaseInsertBuilder
        {
            get
            {
                if (_baseInsertBuilder == null)
                {
                    _baseInsertBuilder = new InsertBuilder();
                    _baseInsertBuilder.Table = TableName;
                }
                return _baseInsertBuilder;
            }
            set
            {
                _baseInsertBuilder = value;
            }
        }

        UpdateBuilder _baseUpdateBuilder;
        public UpdateBuilder BaseUpdateBuilder
        {
            get
            {
                if (_baseUpdateBuilder == null)
                {
                    _baseUpdateBuilder = new UpdateBuilder();
                    _baseUpdateBuilder.Table = TableName;
                }
                return _baseUpdateBuilder;
            }
            set
            {
                _baseUpdateBuilder = value;
            }
        }

        DeleteBuilder _baseDeleteBuilder;
        public DeleteBuilder BaseDeleteBuilder
        {
            get
            {
                if (_baseDeleteBuilder == null)
                {
                    _baseDeleteBuilder = new DeleteBuilder();
                    _baseDeleteBuilder.Table = TableName;
                }
                return _baseDeleteBuilder;
            }
            set
            {
                _baseDeleteBuilder = value;
            }
        }

        protected bool ExecuteInsert()
        {
            var inserter = new DbInserter();
            if (inserter.Execute(BaseInsertBuilder.Sql))
            {
                return true;
            }
            else
            {
                SystemErrorMessage = "Error:" + inserter.SystemErrorMessage  + " ** SQL:" + this.BaseInsertBuilder.Sql;
                LogHandler.WriteError(SystemErrorMessage);
                return false;
            }
        }


        protected bool ExecuteUpdate()
        {
            var updater = new DbUpdater();
            if (updater.Execute(BaseUpdateBuilder.Sql))
            {
                return true;
            }
            else
            {
                SystemErrorMessage = "Error:" + updater.ErrorMessage + " ** SQL:" + this.BaseInsertBuilder.Sql;
                LogHandler.WriteError(SystemErrorMessage);
                return false;
            }
        }

        protected bool ExecuteDelete()
        {
            var deleter = new DbDeleter();
            if (deleter.Execute(BaseDeleteBuilder.Sql))
            {
                return true;
            }
            else
            {
                SystemErrorMessage = "Error:" + deleter.ErrorMessage + " ** SQL:" + this.BaseDeleteBuilder.Sql;
                LogHandler.WriteError(SystemErrorMessage);
                return false;
            }
        }

        protected bool ExecuteQueries(List<string> sqlCollection)
        {
            try
            {
                var inserter = new DbInserter();
                if (inserter.Execute(sqlCollection))
                {
                    return true;
                }
                else
                {
                    LogHandler.WriteError(inserter.SystemErrorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteError(ex.Message);
                return false;
            }
        }

    }
}
