using System.Net.Mail;
using ModelRepository.ModelInterfaces;
using ModelUtilities;

namespace Asterisk.Utilities
{
  public static class IIsForwardableMixIn
  {
    public static bool ForwardMail(this IIsForwardable forwardable, IExtension from, string emailTo, string message,
                                   SmtpClient client)
    {
      var mailMessage = new MailMessage(from.Email == "" ? "mail@asterisk.com" : from.Email, emailTo,
                                        string.Format("Forwarded message from {0} {1}", from.FirstName, from.LastName),
                                        string.Format("This message is forwarded from {0} {1} \r\n\r\n{2}",
                                                      from.FirstName, from.LastName, message));

      using (mailMessage)
      {
        mailMessage.Attachments.Add(new Attachment(forwardable.ForwardableStream, "message.wav"));


        using (client)
        {
          try
          {
            client.Send(mailMessage);
            return true;
          }
          catch (SmtpException)
          {
            return false;
          }
        }
      }
    }
  }
}