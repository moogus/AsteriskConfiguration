using System.Net;
using System.Net.Mail;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class Server : IServer
  {
    private readonly IComServer _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public Server(IComServer comServer, IRepositoryWithDelete modelRepository)
    {
      _under = comServer;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string IpAddress
    {
      get { return _under.IpAddress; }
      set { _under.IpAddress = value; }
    }

    NetworkCredential IServer.Credentials
    {
      get { return new NetworkCredential(_under.UserName, _under.Password); }
      set { SetNetworkCredentails(value); }
    }

    private void SetNetworkCredentails(NetworkCredential value)
    {
      _under.UserName = value.UserName;
      _under.Password = value.Password;
    }

    SmtpClient IServer.MailServer
    {
      get { return new SmtpClient(_under.MailServer); }
      set { _under.MailServer = value.Host; }
    }

    public string VoicemailDialNumber
    {
      get { return _under.VoicemailDialNumber; }
      set { _under.VoicemailDialNumber = value; }
    }

    public IExtension AdminExtension
    {
      get
      {
        var ext = _modelRepository.GetFromName<IExtension>(_under.AdminAccount);

        return ext;
      }
      set { _under.AdminAccount = value.Number; }
    }

    public string ExtensionIpRange
    {
      get { return _under.ExtensionIpRange; }
      set { _under.ExtensionIpRange = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}