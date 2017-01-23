using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuPermisionClassMember :  IFuPermisionClassMember
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return ""; } }
    public virtual int PermissionClassId { get; set; }
    public virtual int PermissionPatternId { get; set; }
    public virtual int DialplanId { get; set; }
  }
}
