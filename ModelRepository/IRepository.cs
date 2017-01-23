using System.Collections.Generic;
using ModelRepository.Internal;

namespace ModelRepository
{
  public interface IRepository
  {
    IRepositoryTransaction ModelTransaction();
    T Add<T>() where T : class, IModel;
    IEnumerable<T> GetList<T>() where T : class, IModel;
    T GetFromId<T>(int id) where T : class, IModel;
    T GetFromName<T>(string name) where T : class, IModel;
  }
}