using System;
using System.Collections.Generic;
using DataAccess;

namespace ModelRepository.Internal
{
  public interface IEmptyModelRepository : IRepositoryWithDelete
  {
    void AddMapping(Type type, Func<Func<IDatabaseTable, bool>, object> mapping);
    IEnumerable<T> Get<T>(Func<T, bool> predicate) where T : IDatabaseTable;
  }
}