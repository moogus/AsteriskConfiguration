using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuPermissionClass :  IFuPermissionClass
  {
    public virtual int Id { get; set; }
    public virtual string Name{get { return FuPermissionClassName; }}
    public virtual string FuPermissionClassName { get; set; }
    public virtual string Description { get; set; }
  }
}
