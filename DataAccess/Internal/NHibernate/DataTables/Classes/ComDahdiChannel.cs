using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComDahdiChannel :  IComDahdiChannel
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return ChannelName; } }
    public virtual int TrunkId { get; set; }
    public virtual string ChannelName { get; set; }
  }
}
