using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;
using ModelUtilities;


namespace ModelRepository.Internal.Models
{
    internal class VoiceMessage : IVoiceMessage
    {
        private readonly IAstVoiceMessage _under;
        private readonly IRepositoryWithDelete _modelRepository;
        private readonly IAsteriskAudioStream _asteriskAudioStream;


        public VoiceMessage(IAstVoiceMessage astVoiceMessage, IRepositoryWithDelete modelRepository, IAsteriskAudioStream asteriskAudioStream)
        {
            _under = astVoiceMessage;
            _modelRepository = modelRepository;
            _asteriskAudioStream = asteriskAudioStream;
        }

        public int Id
        {
            get { return _under.Id; }
        }

        public IVoiceMessageFolder Folder { get; set; }

        public string CallerId
        {
            get { return GetCallerId(); }
        }

        public string CallerNumber
        {
            get { return GetCallerNumber(); }
        }

        public DateTime CalledAt
        {
            get { return ConvertFromUnixToDateTime(_under.OrigTime); }
        }

        public int Duration
        {
            get { return int.Parse(_under.Duration); }
        }

        public IVoiceMail MailBox
        {
            get { return _modelRepository.GetFromName<IVoiceMail>(_under.MailBox); }
        }

        public TimeSpan TimeSinceEdited
        {
            get { return new TimeSpan(DateTime.Now.Ticks - _under.TimeStamp.Ticks); }
        }

        public Stream Audiostream
        {
            get { return _asteriskAudioStream.Stream(_under.Recording); }
        }

        public bool ForwardMail(IExtension @from, string emailTo, string message, SmtpClient client)
        {
            var mailMessage = new MailMessage(from.Email == "" ? "mail@asterisk.com" : from.Email, emailTo,
                                        string.Format("Forwarded message from {0} {1}", from.FirstName, from.LastName),
                                        string.Format("This message is forwarded from {0} {1} \r\n\r\n{2}",
                                                      from.FirstName, from.LastName, message));

            using (mailMessage)
            {
                mailMessage.Attachments.Add(new Attachment(Audiostream, "message.wav"));

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

        public void Delete()
        {
            _modelRepository.Delete(_under);
        }

        private static DateTime ConvertFromUnixToDateTime(string timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return !string.IsNullOrEmpty(timestamp) ? origin.AddSeconds(double.Parse(timestamp)) : origin;
        }

        private string GetCallerId()
        {
            return !string.IsNullOrEmpty(_under.CallerId) || _under.CallerId.Contains('<') ? GetSipCallerName() : CallerNumber;
        }

        private string GetCallerNumber()
        {
            return (_under.CallerId.Contains('<'))
                     ? _under.CallerId.Split('<')[1].Split('>')[0]
                     : _under.CallerId.Length > 5 ? string.Format("0{0}", _under.CallerId) : _under.CallerId;
        }

        private string GetSipCallerName()
        {
            var rtn = "No caller ID";

            foreach (var e in _modelRepository.GetList<IExtension>().Where(e => e.Number.Equals(CallerNumber)))
            {
                rtn = string.Format("{0} {1}", e.FirstName, e.LastName);
            }

            foreach (var k in _modelRepository.GetList<IKnownNumber>().Where(k => k.Number.Equals(CallerNumber)))
            {
                rtn = string.Format("{0}", k.Description);
            }

            return rtn;
        }
    }
}