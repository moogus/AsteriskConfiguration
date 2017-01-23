using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComAccessCode : IComAccessCode
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return string.Empty; } }
    public virtual string Code { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int Priority { get; set; }
  }
}