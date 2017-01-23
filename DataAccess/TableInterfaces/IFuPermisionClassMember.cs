namespace DataAccess.TableInterfaces
{
  public interface IFuPermisionClassMember : IDatabaseTable
  {
    int PermissionClassId { get; set; }
    int PermissionPatternId { get; set; }
    int DialplanId { get; set; }
  }
}