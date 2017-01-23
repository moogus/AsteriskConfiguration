namespace ModelRepository.ModelInterfaces
{
  public interface IRoutingRule : IModel
  {
    int Id { get; }
    IDialplan Dialplan { get; set; }
    string Number { get; set; }
    int Time { get; set; }
    int Order { get; set; }
    RoutingRuleDestination DestinationType { get; set; }
    string DestinationNumber { get; set; }
  }
}