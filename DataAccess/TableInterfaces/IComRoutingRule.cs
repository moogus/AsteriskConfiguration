namespace DataAccess.TableInterfaces
{
  public interface IComRoutingRule : IDatabaseTable
  {
    int DialplanId { get; set; }
    string Number { get; set; }
    int Time { get; set; }
    int Order { get; set; }
    string DestinationType { get; set; }
    string DestinationNumber { get; set; }
  }
}