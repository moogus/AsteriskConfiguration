using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class Voicemail : IVoiceMail, IModel
  {
    private readonly AstVoicemail _under;
    private readonly ISessionWrapper _session;
    private readonly IRepository _repository;

    internal Voicemail(AstVoicemail under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal Voicemail(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new AstVoicemail { Context = "default" };
    }

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Number
    {
      get { return _under.Mailbox; }
      set { _under.Mailbox = value; }
    }

    public string Password
    {
      get { return _under.Password; }
      set { _under.Password = value; }
    }

    public int MessageLength
    {
      get { return _under.MaxSecs; }
      set { _under.MaxSecs = value; }
    }

    public string DefaultEmail
    {
      get { return _under.Email; }
      set { _under.Email = value; }
    }

    public bool EmailNotificationHasMp3
    {
      get { return _under.Attach == "yes"; }
      set { _under.Attach = value ? "yes" : "no"; }
    }

    public int NumberOfMessages { get { return _under.MaxMessages; } set { _under.MaxMessages = value; } }

    public int HeldNumberOfMessages { get { return _under.HeldMaxMessages; } set { _under.HeldMaxMessages = value; } }

    public IEnumerable<VoiceMessageFolder> MessageFolders { get { return GetAllFolders(); } }

    private IEnumerable<VoiceMessageFolder> GetAllFolders()
    {
      var messages = _repository.GetList<IVoiceMessage>().Where(m => m.MailBox != null && m.MailBox.Id == _under.Id && !m.Folder.Equals("unavail"));
      var allFolders = new List<VoiceMessageFolder>();

      CreateDefaultFolders(allFolders);

      foreach (var message in messages.OrderBy(f => f.Folder))
      {
        if (allFolders.Any(f => f.FolderName == message.Folder))
        {
          var messageCopy = message;
          foreach (var vm in allFolders.Where(vm => vm.FolderName == messageCopy.Folder))
          {
            vm.FolderMessages.Add(message);
          }
        }
        else
        {
          allFolders.Add(new VoiceMessageFolder(message) { Order = message.Folder == "INBOX" ? 1 : message.Folder == "Old" ? 2 : message.Folder == "Deleted" ? 3 : allFolders.Count + 1 });
        }
      }
      return allFolders;
    }

    private void CreateDefaultFolders(List<VoiceMessageFolder> allFolders)
    {
      if (allFolders.All(f => f.FolderName != "INBOX"))
      {
        allFolders.Add(new VoiceMessageFolder { FolderName = "INBOX", Order = 1 });
      }

      if (allFolders.All(f => f.FolderName != "Old"))
      {
        allFolders.Add(new VoiceMessageFolder { FolderName = "Old", Order = 2 });
      }

      if (allFolders.All(f => f.FolderName != "Deleted"))
      {
        allFolders.Add(new VoiceMessageFolder { FolderName = "Deleted", Order = 3 });
      }
      else
      {
        allFolders.First(f => f.FolderName == "Deleted").Order = 3;
      }
    }
  }

  public class VoiceMessageFolder
  {
    public string FolderName { get; set; }
    public List<IVoiceMessage> FolderMessages { get; set; }
    public int Order { get; set; }

    public VoiceMessageFolder(IVoiceMessage message)
    {
      FolderName = message.Folder;
      FolderMessages = new List<IVoiceMessage> { message };
    }

    public VoiceMessageFolder()
    {
      FolderMessages = new List<IVoiceMessage>();
    }
  }

}
