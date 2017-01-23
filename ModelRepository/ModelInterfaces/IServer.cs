using System.Net;
using System.Net.Mail;

namespace ModelRepository.ModelInterfaces
{
  public interface IServer : IModel
  {
    int Id { get; }
    string IpAddress { get; set; }
    NetworkCredential Credentials { get; set; }
    SmtpClient MailServer { get; set; }
    string VoicemailDialNumber { get; set; }
    IExtension AdminExtension { get; set; }
    string ExtensionIpRange { get; set; }
  }
}