using System.Collections.Generic;

namespace DataAccess.Internal.NHibernate
{
  class SortNHibernateDataObjects : ISortDataObjects
  {
    private readonly IDataConnector _dataConnector;

    internal SortNHibernateDataObjects(IDataConnector dataConnector)
    {
      _dataConnector = dataConnector;
    }

    public bool SortObjectsForTransactions(IEnumerable<IDatabaseTable> databaseTables)
    {
      //foreach (var table in databaseTables)
      //{
      //  switch (table.TableSate)
      //  {
      //    case TableSate.Update:
      //      _dataConnector.Save(table);
      //      break;

      //    case TableSate.Delete:
      //      _dataConnector.Delete(table);
      //      break;
      //  }
      //}

      //return _dataConnector.CommitTables();
      return true;
    }
  }
}