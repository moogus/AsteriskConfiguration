namespace ModelRepository.ModelInterfaces
{
  public interface IDialplanRange : IModel
  {
    int Id { get; }
    string DaysOfWeek { get; set; }
    string TimeRange { get; set; }
    int Priority { get; set; }
    IDialplan Dialplan { get; set; }
  }
}