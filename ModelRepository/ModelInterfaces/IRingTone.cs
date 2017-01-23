namespace ModelRepository.ModelInterfaces
{
  public interface IRingTone : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string SipHeader { get; set; }
  }
}