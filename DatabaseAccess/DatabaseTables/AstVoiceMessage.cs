using System;

namespace DatabaseAccess.DatabaseTables
{
    internal class AstVoiceMessage : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual int MsgNum { get; set; }
    public virtual string Directrory { get; set; }
    public virtual string CallerId { get; set; }
    public virtual string OrigTime { get; set; }
    public virtual string Duration { get; set; }
    public virtual string MailBox { get; set; }
    public virtual byte[] Recording { get; set; }
    public virtual DateTime TimeStamp { get; set; }
  }
}
