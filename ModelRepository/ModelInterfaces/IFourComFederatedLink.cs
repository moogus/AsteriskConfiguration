namespace ModelRepository.ModelInterfaces
{
    public interface IFourComFederatedLink : IFederation
    {
        ITrunk Trunk { get; set; }
    }
}