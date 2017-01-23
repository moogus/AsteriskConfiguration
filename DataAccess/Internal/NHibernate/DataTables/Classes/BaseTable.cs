namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class BaseTable
  {
    private readonly IDataConnector _databaseConnector;

    public BaseTable(IDataConnector databaseConnector)
    {
      _databaseConnector = databaseConnector;
    }

    public BaseTable()
    {
      // default constructor for NHibernate setup DO NOT USE!!!
    }

    public virtual bool Update()
    {
      return _databaseConnector.Save(this);
    }

    public virtual bool Delete()
    {
      return _databaseConnector.Delete(this);
    }
  }
}
