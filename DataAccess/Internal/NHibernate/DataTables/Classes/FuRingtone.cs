using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuRingtone :  IFuRingtone
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuRingtoneName; } }
    public virtual string FuRingtoneName { get; set; }
    public virtual string SipHeader { get; set; }
  }
}