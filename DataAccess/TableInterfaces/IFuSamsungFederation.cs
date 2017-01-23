namespace DataAccess.TableInterfaces
{
    public interface IFuSamsungFederation : IDatabaseTable
  {
    string Description { get; set; }
    int ComTrunkId { get; set; }
    int ComExtensionId { get; set; }
    int ComRoutingRuleId { get; set; }
    string FuFederationName { get; set; }
  }
}