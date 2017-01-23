namespace DataAccess.TableInterfaces
{
  public interface IFuDialplanRange : IDatabaseTable
  {
    string DaysOfWeek { get; set; }
    string TimeRange { get; set; }
    int Priority { get; set; }
    int FuDialplanId { get; set; }
  }
}