using System;
using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class AstVoiceMessage :  IAstVoiceMessage
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return MailBox; } }
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
