namespace DataAccess.TableInterfaces
{
  public interface IComAccessCode :IDatabaseTable
  {
    string Code { get; set; }
    int TrunkId { get; set; }
    int Priority { get; set; }
  }
}