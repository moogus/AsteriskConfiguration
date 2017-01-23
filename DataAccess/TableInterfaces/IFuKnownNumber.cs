namespace DataAccess.TableInterfaces
{
  public interface IFuKnownNumber : IDatabaseTable
  {
    string Number { get; set; }
    string Description { get; set; }
    bool IsInternal { get; set; }
  }
}