using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class MessageViewModel
  {
    public string Extension { get; set; }
    public IVoiceMail VoiceMail { get; set; }
    public string LastFolder { get; set; }
  }
}