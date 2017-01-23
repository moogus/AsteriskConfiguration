namespace DataAccess.TableInterfaces
{
  public interface IComSipCredentials : IDatabaseTable
  {
    string UserName { get; set; }
    string Password { get; set; }
    string Host { get; set; }
    int TrunkId { get; set; }
    int AllowedChannels { get; set; }
  }
}