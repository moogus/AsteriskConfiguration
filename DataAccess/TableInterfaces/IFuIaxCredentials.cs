namespace DataAccess.TableInterfaces
{
  public interface IFuIaxCredentials : IDatabaseTable
  {
    string Host { get; set; }
    string FuIaxCredentialName { get; set; }
    int TrunkId { get; set; }
    int AllowedChannels { get; set; }
  }
}