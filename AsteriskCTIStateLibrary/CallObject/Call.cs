using System;
using System.ComponentModel;
using AsteriskCTIStateLibrary.Enums;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.CallObject
{
  public class Call : INotifyPropertyChanged
  {
    private readonly OurEvent _newAction;
    private CallStateEnum _callStateEnum;
    private DialStatusEnum _dialStatus;
    public ICallState CallState { get; set; }
    public ICallState Hangup { get; set; }
    public ICallState OnCall { get; set; }
    public ICallState Ringing { get; set; }
    public string CallersNumber { get; set; }
    public string DialedNumber { get; set; }
    public bool Incoming { get; set; }
    public string Id { get; set; }
    public DateTime TimeOfCall { get; private set; }

    public DialStatusEnum DialStatus
    {
      get { return _dialStatus; }
      set
      {
        _dialStatus = value;
        OnPropertyChanged("DialStatus");
      }
    }

    public CallStateEnum CallStateEnum
    {
      get { return _callStateEnum; }
      set
      {
        _callStateEnum = value;
        OnPropertyChanged("CallStateEnum");
      }
    }

    public Call(OurEvent newAction, bool incoming)
    {
      _newAction = newAction;
      Incoming = incoming;
      Id = _newAction.UniqueId;
      Hangup = new CallHangup(this);
      OnCall = new CallOnCall(this);
      Ringing = new CallRinging(this);
      CallState = Ringing;
      CallStateEnum = CallStateEnum.Ringing;
      DialStatus = DialStatusEnum.NoState;
      TimeOfCall = DateTime.Now;
      DialedNumber = string.Empty;
    }

    public void DetermineAction(OurEvent newAction)
    {
      CallState.DeterminAction(newAction);
    }

    public void ChangeState(ICallState newState)
    {
      CallState = newState;
    }

    public void SetDialStatus(string asteriskDialState)
    {
      if (Incoming)
      {
        switch (asteriskDialState)
        {
          case "ANSWER":
            DialStatus = DialStatusEnum.Incoming;
            break;

          case null:
            DialStatus = DialStatus;
            break;

          case "":
            DialStatus = DialStatus;
            break;

          default:
            DialStatus = DialStatusEnum.Missed;
            break;
        }
      }
      else
      {
        switch (asteriskDialState)
        {
          case "ANSWER":
            DialStatus = DialStatusEnum.Outgoing;
            break;

          case null:
            DialStatus = DialStatus;
            break;

          case "":
            DialStatus = DialStatus;
            break;

          case "INVALIDARGS":
            DialStatus = DialStatusEnum.Outgoing;
            break;

          case "CHANUNAVAIL":
            DialStatus = DialStatusEnum.Outgoing;
            break;

          default:
            DialStatus = DialStatusEnum.Outgoing;
            break;
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }


  }
}
