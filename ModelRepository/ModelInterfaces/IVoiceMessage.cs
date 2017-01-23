using System;
using System.IO;
using System.Net.Mail;

namespace ModelRepository.ModelInterfaces
{
    public interface IVoiceMessage : IModel
    {
        int Id { get; }
        IVoiceMessageFolder Folder { get; set; }
        string CallerId { get; }
        string CallerNumber { get; }
        DateTime CalledAt { get; }
        int Duration { get; }
        IVoiceMail MailBox { get; }
        TimeSpan TimeSinceEdited { get; }
        Stream Audiostream { get; }
        bool ForwardMail(IExtension from, string emailTo, string message, SmtpClient client);
    }
}