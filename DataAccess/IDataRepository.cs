using System.Linq;
using DataAccess.Internal;

namespace DataAccess
{
  public interface IDataRepository
  {
    IDataRepositoryTransaction DatabaseTransaction();
    T CreateNewObject<T>() where T : IDatabaseTable;
    IQueryable<T> GetQueryable<T>() where T : IDatabaseTable;
    void Delete(object databaseTable) ;
  }
}
