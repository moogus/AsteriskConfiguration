using System;
using System.Collections.Generic;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess
{
    public interface IRepository
    {
        IEnumerable<T> GetList<T>() where T : class, IModel;
        T Add<T>() where T : class, IModel;
        T GetFromId<T>(int id) where T : class, IModel;
        T GetFromName<T>(string name) where T : class, IModel;
    }

    internal interface IConfigurableRepository : IRepository
    {
        void AddTransformation<TModel, TDb>(Func<TDb, TModel> factory, Func<TDb, string, bool> name,
                                            Func<TModel> newFactory)
            where TDb : IDatabaseTable
            where TModel : IModel;
    }
}