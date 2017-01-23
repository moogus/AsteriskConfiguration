using System.ComponentModel;
using Asterisk.NET.Manager;
using CTIServer.Annotations;
using CTIServer.Dial;
using CTIServer.Dial.Dialers;

namespace CTIServer.ConnectionManger.ConnectionManagers
{
  public class AsteriskConnectionManager :  IConnectionManager
  {
    public  ManagerConnection ManagerConnection { get; private set; }
    private readonly string _password;
    private readonly string _serverAddress;
    private readonly int _serverPort;
    private readonly string _username;
    private bool _connected;
    public bool Connected
    {
      get { return _connected; }
      private set
      {
        _connected = value;
        OnPropertyChanged("Connected");
      }
    }

    public IDialer Dialer { get; private set; }

    public AsteriskConnectionManager(string serverAddress, int serverPort, string username, string password)
    {
      _serverAddress = serverAddress;
      _serverPort = serverPort;
      _username = username;
      _password = password;

      ManagerConnection = new  ManagerConnection(_serverAddress, _serverPort, _username, _password);
      ManagerConnection.ConnectionState += (s, e) => HandleConnectionStateChanged(ManagerConnection);

      Dialer = new AsteriskDialer(ManagerConnection);
    }

    public void Connect()
    {
      ManagerConnection.Login();
    }

    private  void HandleConnectionStateChanged(ManagerConnection managerConnection)
    {
      Connected = managerConnection.IsConnected();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

    }
  }
}
