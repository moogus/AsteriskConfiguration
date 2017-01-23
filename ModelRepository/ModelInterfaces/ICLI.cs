namespace ModelRepository.ModelInterfaces
{
  public interface ICLI : IModel
  {
    int Id { get; }
    string CLINumber { get; set; }
    string CLIName { get; set; }
    ITrunk Trunk { get; set; }
  }
}