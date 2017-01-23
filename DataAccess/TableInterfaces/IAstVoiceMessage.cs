using System;

namespace DataAccess.TableInterfaces
{
  public interface IAstVoiceMessage : IDatabaseTable
  {
    int MsgNum { get; set; }
    string Directrory { get; set; }
    string CallerId { get; set; }
    string OrigTime { get; set; }
    string Duration { get; set; }
    string MailBox { get; set; }
    byte[] Recording { get; set; }
    DateTime TimeStamp { get; set; }
  }
}