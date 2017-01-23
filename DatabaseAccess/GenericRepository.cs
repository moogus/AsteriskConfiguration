using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess
{
  internal class GenericRepository : IConfigurableRepository
  {
    private readonly Dictionary<Type, Func<int, IModel>> _getFromId;
    private readonly Dictionary<Type, Func<string, IModel>> _getFromName;
    private readonly Dictionary<Type, Func<IEnumerable<IModel>>> _getList;
    private readonly Dictionary<Type, Func<IModel>> _getNew;
    private readonly ISessionWrapper _sessionWrapper;

    internal GenericRepository(ISessionWrapper sessionWrapper)
    {
      _sessionWrapper = sessionWrapper;
      _getFromId = new Dictionary<Type, Func<int, IModel>>();
      _getFromName = new Dictionary<Type, Func<string, IModel>>();
      _getList = new Dictionary<Type, Func<IEnumerable<IModel>>>();
      _getNew = new Dictionary<Type, Func<IModel>>();
    }

    public IEnumerable<T> GetList<T>() where T : class, IModel
    {
      if (_getList.ContainsKey(typeof(T)))
      {
        lock (_sessionWrapper)
        {
          return _getList[typeof(T)]().Select(x => x as T);
        }
      }
      return null;
    }

    public T Add<T>() where T : class, IModel
    {
      if (_getNew.ContainsKey(typeof(T)))

        lock (_sessionWrapper)
        {
          return _getNew[typeof(T)]() as T;
        }

      return null;
    }

    public T GetFromId<T>(int id) where T : class, IModel
    {
      if (_getFromId.ContainsKey(typeof(T)))
      {
        lock (_sessionWrapper)
        {
          return _getFromId[typeof(T)](id) as T;
        }
      }
      return null;
    }

    public T GetFromName<T>(string name) where T : class, IModel
    {
      if (_getFromName.ContainsKey(typeof(T)))
      {
        lock (_sessionWrapper)
        {
          return _getFromName[typeof(T)](name) as T;
        }
      }
      return null;
    }

    public void AddTransformation<TModel, TDb>(Func<TDb, TModel> factory,
                                               Func<TDb, string, bool> name,
                                               Func<TModel> newFactory)
      where TDb : IDatabaseTable
      where TModel : IModel
    {
      _getList.Add(typeof(TModel), () =>
                                        {
                                          var a = _sessionWrapper.Query<TDb>().ToList();
                                          var b = a.Select(x => (IModel)factory(x));
                                          return b;
                                        });
      _getFromId.Add(typeof(TModel),
                     id => _sessionWrapper.Query<TDb>()
                                          .Where(x => x.Id == id)
                                          .Select(factory)
                                          .FirstOrDefault());
      _getFromName.Add(typeof(TModel),
                       n => _sessionWrapper.Query<TDb>()
                                           .ToList()
                                           .Where(x => name(x, n))
                                           .Select(factory)
                                           .FirstOrDefault());
      _getNew.Add(typeof(TModel),
                  () => newFactory());
    }
  }
}