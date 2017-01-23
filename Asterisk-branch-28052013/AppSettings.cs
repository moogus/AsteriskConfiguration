using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using DatabaseAccess;

namespace Asterisk
{
  public interface IAppSettings
  {
    SmtpClient MailServer { get; }
  }

  public class AppSettings : IAppSettings
  {
    private readonly IRepository _repository;

    private SmtpClient _MailSever;

    public SmtpClient MailServer
    {
      get { return _MailSever ?? (_MailSever = _repository.GetList<IServer>().First().MailServer); }
    }

    public AppSettings(IRepository repository)
    {
      _repository = repository;
    }
  }
}