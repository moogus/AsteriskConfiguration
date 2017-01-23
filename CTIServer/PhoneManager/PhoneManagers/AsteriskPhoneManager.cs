using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using CTIServer.Annotations;
using CTIServer.Call.Calls;
using CTIServer.CallHandler;
using CTIServer.ConnectionManger;
using CTIServer.Phone;
using CTIServer.Presence;

namespace CTIServer.PhoneManager.PhoneManagers
{
  public class AsteriskPhoneManager : IPhoneManager
  {
    private readonly IConnectionManager _connectorManager;
    private readonly ICallHandler _callhandler;
    private Dictionary<string, IPhone> _phoneLookup;

    private bool _connected;
    private Dictionary<string, string> _ddiLookup;

    public bool Connected
    {
      get { return _connected; }
      set
      {
        _connected = value;
        OnPropertyChanged("Connected");
      }
    }

    public AsteriskPhoneManager(IEnumerable<IPhone> allContacts, IConnectionManager connectionManager, ICallHandler callHandler )
    {
      _connectorManager = connectionManager;

      _connectorManager.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == "Connected")
        {
          Connected = _connectorManager.Connected;
        }
      };

      _connectorManager.Connect();
      CreatePhoneDictionary(allContacts);
      _callhandler = callHandler;
    
      _callhandler.Calls.CollectionChanged += (sender, args) => ManagePhoneCalls(args);
    }

    public IPhone GetPhone(string extensionNumber)
    {
      return _phoneLookup.ContainsKey(extensionNumber) ? _phoneLookup[extensionNumber] : null;
    }

    public IPresence GetPresence(string extensionNumber)
    {
      return _phoneLookup.ContainsKey(extensionNumber) ? _phoneLookup[extensionNumber] : null;
    }

    private void CreatePhoneDictionary(IEnumerable<IPhone> phones)
    {
      _phoneLookup = new Dictionary<string, IPhone>();
      _ddiLookup = new Dictionary<string, string>();

      foreach (var phone in phones)
      {
        _phoneLookup.Add(phone.ExtensionNumber, phone);
        if (! string.IsNullOrEmpty(phone.DDI))
        {
          _ddiLookup.Add(phone.DDI, phone.ExtensionNumber);
        }
      }
    }

    private void ManagePhoneCalls(NotifyCollectionChangedEventArgs args)
    {
      if (args.Action == NotifyCollectionChangedAction.Add)
        foreach (AsteriskCall call in args.NewItems)
        {
          if (_phoneLookup.ContainsKey(call.Channel))
          {
           _phoneLookup[call.Channel].AddCall(call);
          }
          if (_ddiLookup.ContainsKey(call.Channel))
          {
            _phoneLookup[_ddiLookup[call.Channel]].AddCall(call);
          }
        }

      if (args.Action == NotifyCollectionChangedAction.Remove)
        foreach (AsteriskCall call in args.OldItems)
        {
          if (_phoneLookup.ContainsKey(call.Channel))
          {
             _phoneLookup[call.Channel].RemoveCall(call);
          }
          if (_ddiLookup.ContainsKey(call.Channel))
          {
            _phoneLookup[_ddiLookup[call.Channel]].RemoveCall(call);
          }
        }
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
