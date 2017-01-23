using System.Collections.Generic;
using System.Collections.ObjectModel;
using AsteriskCTIStateLibrary.CallObject;
using AsteriskCTIStateLibrary.Enums;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.PhoneObject
{
  public class Phone : IPhone
  {
    private readonly Dictionary<string, Call> _callsToThisPhone;
    public string MyNumber { get; set; }
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
      var num = newAction.ChannelNumber;
      var incoming = num != MyNumber;

      if (newAction.ChannelNumber == MyNumber || incoming)
      {
        //TODO: the problem here is that you are monitoring all incoming calls
        //you need to flag the fact that an incoming and on a later event ensure it was definately for your phone. pump all calls into the dictionary then get the phone to subscribe to it.
        //inversion of control pattern (dependancy injection) and observer.
        lock (_callsToThisPhone)
        {
          Call call = null;
          if (!_callsToThisPhone.ContainsKey(newAction.UniqueId))
          {

            call = incoming ? new Call(newAction, true) { DialedNumber = num }
                                   : new Call(newAction, false) { CallersNumber = MyNumber };

            if (newAction.Type == SIPEvent.NewChannel)
            {
              call.DialedNumber = call.Incoming ? num : newAction.NumberDialed;
              call.CallersNumber = call.Incoming ? newAction.CallerIdNum : MyNumber;
            }
            _callsToThisPhone.Add(newAction.UniqueId, call);
            Calls.Add(call);
          }
          else
          {
            call = _callsToThisPhone[newAction.UniqueId];
          }

          call.DetermineAction(newAction);

          if (call.CallStateEnum == CallStateEnum.HangUp)
          {
            _callsToThisPhone.Remove(newAction.UniqueId);
            Calls.Remove(call);
          }
        }
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

  }
}
