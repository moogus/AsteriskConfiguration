using System.Net;
using System.Net.Mail;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class Server : IServer, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComServer _under;
    internal Server(ComServer under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal Server(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new ComServer();
    }

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
    }

    public int Id { get { return _under.Id; } }
    public string IpAddress { get { return _under.IpAddress; } set { _under.IpAddress = value; } }
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
    public string VoicemailDialNumber { get { return _under.VoicemailDialNumber; } set { _under.VoicemailDialNumber = value; } }
    public IExtension AdminExtension { get { return _repository.GetFromName<IExtension>( _under.AdminAccount); } set { _under.AdminAccount = value.Number; } }
    public string ExtensionIpRange { get { return _under.ExtensionIpRange; } set { _under.ExtensionIpRange = value; } }

  }
}
