namespace DataAccess.TableInterfaces
{
  public interface IComCLI: IDatabaseTable
  {
    string CLIName { get; set; }
    string CLINumber { get; set; }
    int TrunkId { get; set; }
  }
}