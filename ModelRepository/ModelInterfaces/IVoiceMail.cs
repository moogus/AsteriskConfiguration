using System.Collections.Generic;
using ModelUtilities;

namespace ModelRepository.ModelInterfaces
{
  public interface IVoiceMail : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Password { get; set; }
    int NumberOfMessages { get; set; }
    int MessageLength { get; set; }
    int HeldNumberOfMessages { get; set; }
    string DefaultEmail { get; set; }
    bool EmailNotificationHasMp3 { get; set; }
    IEnumerable<IVoiceMessageFolder> MessageFolders { get; }
  }
}