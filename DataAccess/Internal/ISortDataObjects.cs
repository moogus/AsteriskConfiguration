using System.Collections.Generic;

namespace DataAccess.Internal
{
  public  interface ISortDataObjects
  {
    bool SortObjectsForTransactions(IEnumerable<IDatabaseTable> databaseTables);
  }
}