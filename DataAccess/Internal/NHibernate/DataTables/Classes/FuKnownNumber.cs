using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuKnownNumber :  IFuKnownNumber
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return Number; } }
    public virtual string Number { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsInternal { get; set; }
  }
}