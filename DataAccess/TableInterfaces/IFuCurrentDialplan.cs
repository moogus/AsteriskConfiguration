namespace DataAccess.TableInterfaces
{
  public interface IFuCurrentDialplan : IDatabaseTable
  {
    int CurrentDialplan { get; set; }
    bool AutomaticallyChange { get; set; }
  }
}