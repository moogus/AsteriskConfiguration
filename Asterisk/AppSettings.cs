using System.Linq;
using System.Net.Mail;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk
{
  public interface IAppSettings
  {
    SmtpClient MailServer { get; }
  }

  public class AppSettings : IAppSettings
  {
    private readonly IRepository _modelRepository;

    private SmtpClient _MailSever;

    public AppSettings(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
    }

    public SmtpClient MailServer
    {
      get { return _MailSever ?? (_MailSever = _modelRepository.GetList<IServer>().First().MailServer); }
    }
  }
}