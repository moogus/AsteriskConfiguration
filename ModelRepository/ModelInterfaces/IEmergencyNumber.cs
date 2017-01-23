namespace ModelRepository.ModelInterfaces
{
  public interface IEmergencyNumber : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Description { get; set; }
  }
}