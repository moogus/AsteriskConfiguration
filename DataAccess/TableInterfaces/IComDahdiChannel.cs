namespace DataAccess.TableInterfaces
{
  public interface IComDahdiChannel : IDatabaseTable
  {
    int TrunkId { get; set; }
    string ChannelName { get; set; }
  }
}