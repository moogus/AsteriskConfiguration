using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComQueue :  IComQueue
  {
      public ComQueue()
      {
          Strategy = "Ringall";
      }
    public virtual int Id { get; set; }
    public virtual string Name { get { return Number; } }
    public virtual string Number { get; set; }
    public virtual string Notes { get; set; }
    public virtual string QueueName { get; set; }
    public virtual string Department { get; set; }
    public virtual string Strategy { get; set; }
    public virtual bool RingOnBusy { get; set; }
    public virtual string DDINumber { get; set; }
    public virtual int VoiceMailId { get; set; }
    public virtual int VoicemailDelay { get; set; }
    public virtual int CLIId { get; set; }
    public virtual int ComMusicOnHoldId { get; set; }
    public virtual bool IncludeInDirectory { get; set; }
  }
}