using System;
using System.ComponentModel;
using System.Windows.Threading;
using Asterisk.NET.Manager;
using AsteriskCTIStateLibrary.PhoneObject;

namespace AsteriskCTIStateLibrary.ManagerObject
{
  public interface IAsteriskManager : INotifyPropertyChanged
  {
    void Connect();
    IPhone RegisterPhone(string extension);
    void Disconnect();
    bool Connected { get; set; }
    string Extension { get; set; }
  }

  public interface IRegisterphone
  {
    IPhone RegisterPhone(string extension);
  }

  public class AsteriskManager : IAsteriskManager, IRegisterphone
  {
    private ManagerConnection _manager;
    private readonly string _serverAddress;
    private bool _connected;
    public bool Connected
    {
      get { return _connected; }
      set
      {
        _connected = value;
        OnPropertyChanged("Connected");
      }
    }
    public string Extension { get; set; }

    public AsteriskManager(string serverAddress)
    {
      _serverAddress = serverAddress;
    }

    public delegate void OurEventHandler(object sender, OurEvent e);
    public event OurEventHandler ManagerEvent;

    private bool _shouldWeReconnect;
    private DispatcherTimer _connectTimer;

    public void Connect()
    {
      if (_connectTimer == null)
      {
        _connectTimer = new DispatcherTimer();
        _connectTimer.Interval = TimeSpan.FromSeconds(10);
        _connectTimer.Tick += (s, e) => ConnectThreadDoWork();
      }
      _shouldWeReconnect = true;
      _connectTimer.Start();
    }

    private void ConnectThreadDoWork()
    {
      _manager = new ManagerConnection(_serverAddress, 5038, "admin", "31994");

      try
      {
        _manager.Login();
      }
      catch (ManagerException ex)
      {
        Connected = false;
        return;
      }
      CreateEvents();
      Connected = true;
      _connectTimer.Stop();
    }

    public void Disconnect()
    {
      _shouldWeReconnect = false;
      //stop connection thread if started
      if (_connectTimer.IsEnabled)
      {
        _connectTimer.Stop();
      }

      // if we are connected do this:
      if (Connected)
      {
        try
        {
          _manager.Logoff();
          Connected = false;
        }
        catch (ManagerException)
        {
        }
      }
      Connected = false;
    }

    public IPhone RegisterPhone(string extension)
    {
      var currentPhone = new Phone(extension);
      Extension = extension;
      currentPhone.RegisterWithManager(this);
      return currentPhone;
    }

    private void CreateEvents()
    {
      _manager.FireAllEvents = true;
      _manager.NewChannel += (sender, e) =>
        {
          if (ManagerEvent != null) ManagerEvent(this, new OurEvent(e));
        };
      _manager.Dial += (sender, e) =>
        {
          if (ManagerEvent != null) ManagerEvent(this, new OurEvent(e));
        };
      _manager.NewState += (sender, e) =>
        {
          if (ManagerEvent != null) ManagerEvent(this, new OurEvent(e));
        };
      _manager.NewCallerId += (sender, e) =>
        {
          if (ManagerEvent != null) ManagerEvent(this, new OurEvent(e));
        };
      _manager.ConnectionState += (s, e) =>
        {
          if (e.Reconnect && _shouldWeReconnect) Connect();
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
