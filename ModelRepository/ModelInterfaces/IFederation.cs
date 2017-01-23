namespace ModelRepository.ModelInterfaces
{
  public interface IFederation : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    ITrunk Trunk { get; set; }
    bool SetFederationValues(string name, string accessCode, string password);
  }
}