namespace ModelRepository.ModelInterfaces
{
    public interface ISamsungFederatedLink : IFederation
    {
        IExtension Extension { get; set; }
        IRoutingRule RoutingRule { get; set; }
    }
}