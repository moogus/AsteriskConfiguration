using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuAutoAttendant :  IFuAutoAttendant
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuAutoAttendantName; } }
    public virtual string FuAutoAttendantName { get; set; }
    public virtual string Announcement { get; set; }
    public virtual int Timeout { get; set; }
  }
}