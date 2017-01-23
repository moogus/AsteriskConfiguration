namespace ModelRepository.ModelInterfaces
{
  public interface IDahdiChannel : IModel
  {
    int Id { get; }
    ITrunk ParentTrunk { get; set; }
    string ChannelName { get; set; }
  }
}