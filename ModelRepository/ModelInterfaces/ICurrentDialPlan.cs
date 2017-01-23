namespace ModelRepository.ModelInterfaces
{
  public interface ICurrentDialPlan : IModel
  {
    IDialplan Dialplan { get; set; }
    bool AutomaticallyChange { get; set; }
    int Id { get; }
  }
}