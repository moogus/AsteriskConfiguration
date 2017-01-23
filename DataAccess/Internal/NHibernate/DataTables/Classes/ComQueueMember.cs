using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComQueueMember :  IComQueueMember
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return ""; } }
    public virtual int ParentQueueId { get; set; }
    public virtual int Penalty { get; set; }
    public virtual int Paused { get; set; }
    public virtual int Type { get; set; }
    public virtual int ExtensionId { get; set; }
    public virtual int QueueId { get; set; }
  }
}