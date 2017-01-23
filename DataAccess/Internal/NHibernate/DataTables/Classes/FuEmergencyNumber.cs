using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuEmergencyNumber :  IFuEmergencyNumber
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return Number; } }
    public virtual string Number { get; set; }
    public virtual string Description { get; set; }
  }
}