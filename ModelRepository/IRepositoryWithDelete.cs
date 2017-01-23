namespace ModelRepository
{
  public interface IRepositoryWithDelete :IRepository
  {
    void Delete(object dataTable);
  }
}