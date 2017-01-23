using System.Linq;
using NHibernate;

namespace DataAccess.Internal
{
  public interface IDataConnector
  {
    IQueryable<T> GetQuery<T>() where T : IDatabaseTable;
    IDataRepositoryTransaction GetTransaction();
    ISession Session { get; } 
  }
}