using System;
namespace SPA.Data
{
    interface IQuerySave
    {
        void AddField(string column, object value);
        void AddField(string column, object value, Enums.Data.DataType type);
    }
}
