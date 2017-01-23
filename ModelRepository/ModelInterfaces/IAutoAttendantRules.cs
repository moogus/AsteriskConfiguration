namespace ModelRepository.ModelInterfaces
{
  public interface IAutoAttendantRules : IModel
  {
    int Id { get; }
    string AaName { get; set; }
    string Entry { get; set; }
    string DestinationNumber { get; set; }
    RoutingRuleDestination DestinationType { get; set; }
  }
}