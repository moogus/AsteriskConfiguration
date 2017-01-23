namespace ModelRepository.ModelInterfaces
{
  public interface IPermissionPattern : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Pattern { get; set; }
  }
}