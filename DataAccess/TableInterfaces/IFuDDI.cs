namespace DataAccess.TableInterfaces
{
  public interface IFuDDI : IDatabaseTable
  {
    string DDI { get; set; }
    int TrunkId { get; set; }
    string UsedOn { get; set; }
  }
}