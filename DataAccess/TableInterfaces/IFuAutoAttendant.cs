namespace DataAccess.TableInterfaces
{
  public interface IFuAutoAttendant : IDatabaseTable
  {
    string FuAutoAttendantName { get; set; }
    string Announcement { get; set; }
    int Timeout { get; set; }
  }
}