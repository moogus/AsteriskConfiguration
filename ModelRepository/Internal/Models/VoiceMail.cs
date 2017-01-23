using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.Internal.ModelHelpers;
using ModelRepository.ModelInterfaces;
using ModelUtilities;

namespace ModelRepository.Internal.Models
{
  internal class VoiceMail : IVoiceMail
  {
    private readonly IAstVoicemail _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private readonly IMessageFolderManager _messageFolderManager;

    private List<IVoiceMessage> _messages;

    public VoiceMail(IAstVoicemail astVoicemail, IRepositoryWithDelete modelRepository, IMessageFolderManager messageFolderManager)
    {
      _under = astVoicemail;
      _modelRepository = modelRepository;
      _messageFolderManager = messageFolderManager;
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

    public int NumberOfMessages
    {
      get { return _under.MaxMessages; }
      set { _under.MaxMessages = value; }
    }

    public int HeldNumberOfMessages
    {
      get { return _under.HeldMaxMessages; }
      set { _under.HeldMaxMessages = value; }
    }

    public IEnumerable<IVoiceMessageFolder> MessageFolders
    {
      get
      {
        var tempMessages = LazyVoiceMessages();
        return _messageFolderManager.GetAllMessageFolders().Select(f => new VoiceMessageFolder(tempMessages, f.FolderName, f.Order));
      }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
    private IEnumerable<IVoiceMessage> LazyVoiceMessages()
    {
      return _messages ?? _modelRepository.GetList<IVoiceMessage>().Where(m => m.MailBox != null && m.MailBox.Id == _under.Id).ToList();
    }
  }
}