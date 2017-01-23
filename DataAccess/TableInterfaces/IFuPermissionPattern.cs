namespace DataAccess.TableInterfaces
{
  public interface IFuPermissionPattern : IDatabaseTable
  {
    string Pattern { get; set; }
    string FuPatternName { get; set; }
  }
}