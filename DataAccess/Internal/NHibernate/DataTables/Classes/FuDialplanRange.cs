using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuDialplanRange : IFuDialplanRange
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return DaysOfWeek; } }
    public virtual string DaysOfWeek { get; set; }
    public virtual string TimeRange { get; set; }
    public virtual int Priority { get; set; }
    public virtual int FuDialplanId { get; set; }
  }
}