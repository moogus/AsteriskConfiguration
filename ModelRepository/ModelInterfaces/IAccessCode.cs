namespace ModelRepository.ModelInterfaces
{
  public interface IAccessCode : IModel
  {
    int Id { get; }
    string Code { get; set; }
    ITrunk ParentTrunk { get; set; }
    int Priority { get; set; }
  }
}