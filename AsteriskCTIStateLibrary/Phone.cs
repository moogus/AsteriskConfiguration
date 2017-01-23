using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Asterisk.NET.Manager;

namespace AsteriskCTIStateLibrary
{
  public interface IPhone
  {
    void RegisterWithManager(AsteriskManager manager);
    void UnregisterWithManger();
    ObservableCollection<Call> Calls { get; }
    string MyNumber { get; set; }
  }

  public class Phone : IPhone
  {
    public string MyNumber { get; set; }

    private readonly Dictionary<string, Call> _callsToThisPhone;
    public ObservableCollection<Call> Calls { get; private set; }
    private AsteriskManager _manager;

    public Phone(string mynumber)
    {
      MyNumber = mynumber;
      _callsToThisPhone = new Dictionary<string, Call>();
      Calls = new ObservableCollection<Call>();
    }

    public void DetermineAction(OurEvent newAction)
    {
      //todo move to localized variables
      Call thiscall = null;
      if (_callsToThisPhone.ContainsKey(newAction.UniqueId))
      {
        thiscall = _callsToThisPhone[newAction.UniqueId];
        thiscall.DetermineAction(newAction);
        if (thiscall.CallState.GetType() == typeof(CallHangup))
        {
          _callsToThisPhone.Remove(newAction.UniqueId);
          Calls.Remove(thiscall);
        }
        return;
      }

      if (!newAction.Destination.Equals(MyNumber) && !newAction.CallerNumber.Equals(MyNumber)) return;

      if ((newAction.Type == SIPEvent.NewChannel || newAction.Type == SIPEvent.NewCallerId) && !string.IsNullOrEmpty(newAction.Destination) 
            && !string.IsNullOrEmpty(newAction.CallerNumber) && newAction.CallerNumber!="start")
      {
        bool incoming = MyNumber == newAction.Destination;
        thiscall = new Call(newAction.UniqueId, newAction.CallerNumber, newAction.Destination, incoming);
        thiscall.DetermineAction(newAction);
        _callsToThisPhone.Add(newAction.UniqueId, thiscall);
        Calls.Add(thiscall);
      }
    }

    public void RegisterWithManager(AsteriskManager manager)
    {
      _manager = manager;
      _manager.ManagerEvent += DoAction;
    }

    private void DoAction(object sender, OurEvent e)
    {
      DetermineAction(e);
    }

    public void UnregisterWithManger()
    {
      _manager.ManagerEvent += DoAction;
    }

  }

}
