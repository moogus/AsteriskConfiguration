using DatabaseAccess;


namespace Asterisk.ViewModels
{
  public class UserVoiceMailViewModel
  {
    public UserVoiceMailViewModel()
    {
    }

    public UserVoiceMailViewModel(IExtension ext)
    {
      if (ext == null) return;
      var vm = ext.VoiceMail;
      Id = ext.Id;
      MailDelay = ext.VoicemailDelay;
      ExtNumber = ext.Number;
      DefaultEmail = vm.DefaultEmail;
      VoicePassword = vm.Password;
      IncludeMp3 = vm.EmailNotificationHasMp3;
    }

    public int Id { get; set; }
    public int MailDelay { get; set; }
    public string ExtNumber { get; set; }
    public string DefaultEmail { get; set; }
    public string VoicePassword { get; set; }
    public bool IncludeMp3 { get; set; }
  }
}