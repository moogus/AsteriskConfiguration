namespace DataAccess.Internal
{
  public interface IGetDatabaseTable
  {
    T GetDatabaseTableInstance<T>() where T :  IDatabaseTable;
  }
}