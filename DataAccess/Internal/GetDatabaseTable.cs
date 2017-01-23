using StructureMap;

namespace DataAccess.Internal
{
  internal class GetDatabaseTable : IGetDatabaseTable
  {
    private readonly IContainer _container;

    public GetDatabaseTable(IContainer container)
    {
      _container = container;
    }

    public T GetDatabaseTableInstance<T>() where T : IDatabaseTable
    {
      return _container.GetInstance<T>();
    }
  }
}