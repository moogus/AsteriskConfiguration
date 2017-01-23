using DatabaseAccess;
using PhoneApps.Models.Interfaces;

namespace PhoneApps.Models
{
  public class ForwardingModel : IForwardingModel
  {
    public string Number { get; private set; }
    public string Description { get; private set; }

    public ForwardingModel(IExtension extension)
    {
      Number = extension.Number;
      Description = string.Format("{0} {1}", extension.FirstName, extension.LastName);
    }

    public ForwardingModel(IQueue queue)
    {
      Number = queue.Number;
      Description = string.Format("{0}", queue.QueueName);
    }

    public ForwardingModel(IVoiceMail voiceMail)
    {
      Number = voiceMail.Number;
      Description = string.Format("{0}", voiceMail.DefaultEmail);
    }

    public ForwardingModel()
    {
      Number = string.Empty;
      Description = string.Empty;
    }
  }
}