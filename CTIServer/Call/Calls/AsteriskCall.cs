using System;
using System.ComponentModel;
using Asterisk.NET.Manager.Event;
using CTIServer.Annotations;
using CTIServer.Phone;

namespace CTIServer.Call.Calls
{
  public class AsteriskCall : ICall
  {
    public string Id { get; set; }
    public string Channel { get; private set; }
    public bool Incoming { get; set; }

    private bool _isMissed;
    public bool IsMissed
    {
      get { return _isMissed; }
      private set
      {
        _isMissed = value;
        OnPropertyChanged("IsMissed");
      }
    }

    private string _otherEndNumber;
    public string OtherEndNumber
    {
      get { return _otherEndNumber; }
      private set
      {
        _otherEndNumber = value;
        OnPropertyChanged("OtherEndNumber");
      }
    }

    private CallStateEnum _state;
    public CallStateEnum State
    {
      get { return _state; }
      private set
      {
        _state = value;
        OnPropertyChanged("State");
      }
    }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public AsteriskCall(NewChannelEvent e)
    {
      Id = e.UniqueId;
      OtherEndNumber = string.Empty;
      StartTime = DateTime.Now;
      State = CallStateEnum.Initializing;

      //added to ensure the autodial calls are caught...this is still not right and will need looking at
      Channel = e.Channel.StartsWith("L")? e.Channel.Substring(6).Split('@')[0]: e.Channel.Substring(4).Split('-')[0];
      IsMissed = false;

      if (e.Attributes.ContainsKey("exten"))
      {
        OtherEndNumber = e.Attributes["exten"];
        Incoming = false;
      }
      else
      {
        Incoming = true;
      }
    }

    public void Handle(HangupEvent e)
    {
      if (State == CallStateEnum.Ringing && Incoming)
      {
        IsMissed = true;
      }
      EndTime = DateTime.Now;
      State = CallStateEnum.Terminated;
    }

    public void Handle(NewStateEvent e)
    {
      switch (e.ChannelStateDesc)
      {
        case "Up":
          State = CallStateEnum.OnCall;
          break;
        case "Ringing":
          State = CallStateEnum.Ringing;
          if (e.Attributes != null && e.Attributes.ContainsKey("connectedlinenum"))
          {
            OtherEndNumber = e.Attributes["connectedlinenum"];
          }
          break;
        case "Ring":
          State = CallStateEnum.RingingOtherEnd;
          break;
      }
    }

    public void Handle(TransferEvent e)
    {
      if (e.TransferExten != "")
        OtherEndNumber = e.TransferExten;
    }

    public void Pickup(IPhone phone)
    {
      //TODO:
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
