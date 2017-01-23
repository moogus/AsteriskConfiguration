namespace DataAccess.TableInterfaces
{
  public interface IFuEmergencyNumber : IDatabaseTable
  {
    string Number { get; set; }
    string Description { get; set; }
  }
}