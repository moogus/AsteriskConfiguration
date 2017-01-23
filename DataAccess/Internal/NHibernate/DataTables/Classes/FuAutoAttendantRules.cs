using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuAutoAttendantRules :  IFuAutoAttendantRules
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return Entry; } }
    public virtual string AaName { get; set; }
    public virtual string Entry { get; set; }
    public virtual string Destination { get; set; }
    public virtual string DestinationType { get; set; }
  }
}