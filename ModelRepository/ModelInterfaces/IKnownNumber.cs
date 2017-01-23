namespace ModelRepository.ModelInterfaces
{
  public interface IKnownNumber : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Description { get; set; }
    bool IsInternal { get; set; }
  }
}