namespace DataAccess.TableInterfaces
{
  public interface IFuAutoAttendantRules : IDatabaseTable
  {
    string AaName { get; set; }
    string Entry { get; set; }
    string Destination { get; set; }
    string DestinationType { get; set; }
  }
}