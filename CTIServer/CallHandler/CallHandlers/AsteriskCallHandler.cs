using System.Collections.Generic;
using System.Collections.ObjectModel;
using Asterisk.NET.Manager;
using Asterisk.NET.Manager.Event;
using CTIServer.Call;
using CTIServer.Call.Calls;
using CTIServer.ConnectionManger;

namespace CTIServer.CallHandler.CallHandlers
{
  public class AsteriskCallHandler : ICallHandler
  {
    private readonly ManagerConnection _managerConnection;
    private readonly Dictionary<string, AsteriskCall> _allCalls;

    public ObservableCollection<ICall> Calls { get; private set; }

    public AsteriskCallHandler(IConnectionManager connectionManager)
    {
      _managerConnection = connectionManager.ManagerConnection;
      _allCalls = new Dictionary<string, AsteriskCall>();
      Calls = new ObservableCollection<ICall>();
      WireUpEvents();
    }

    private void WireUpEvents()
    {
      _managerConnection.FireAllEvents = true;

      _managerConnection.NewChannel += (s, e) =>
      {
        var call = new AsteriskCall(e);
        _allCalls.Add(e.UniqueId, call);
            Calls.Add(call);

      };
      _managerConnection.Hangup += (sender, e) =>
      {
        if (_allCalls.ContainsKey(e.UniqueId))
          _allCalls[e.UniqueId].Handle(e);

      };
      _managerConnection.NewState += (sender, e) =>
      {
        if (_allCalls.ContainsKey(e.UniqueId))
        {
          var call = _allCalls[e.UniqueId];
          call.Handle(e);
        }
      };
      _managerConnection.UnhandledEvent += (sender, e) =>
      {
        var te = (e as TransferEvent);
        if (te != null)
          if (_allCalls.ContainsKey(te.TargetUniqueId))
            _allCalls[te.TargetUniqueId].Handle(te);
      };
    }
  }
}
