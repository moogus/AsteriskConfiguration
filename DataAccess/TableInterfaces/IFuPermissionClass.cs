namespace DataAccess.TableInterfaces
{
  public interface IFuPermissionClass : IDatabaseTable
  {
    string Description { get; set; }
    string FuPermissionClassName { get; set; }
  }
}