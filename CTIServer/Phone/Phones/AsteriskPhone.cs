using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CTIServer.Annotations;
using CTIServer.Call;
using CTIServer.Call.Calls;
using CTIServer.Presence;

namespace CTIServer.Phone.Phones
{
  public class AsteriskPhone : IPhone
  {
    public ObservableCollection<ICall> Calls { get; private set; }

    public AsteriskPhone(string extension,string ddi, string name, string department)
    {
      ExtensionNumber = extension;
      DDI = ddi;
      Name = name;
      Department = department;
      Calls = new ObservableCollection<ICall>();
      Calls.CollectionChanged += (o, e) => CallsCollectionChangedHandler(e);
    }

    public string ExtensionNumber { get; private set; }
    public string DDI { get; private set; }
    public string Name { get; private set; }
    public string Department { get; private set; }

    private PhoneState _state;
    public PhoneState State
    {
      get { return _state; }
      set
      {
        _state = value;
        OnPropertyChanged("State");
      }
    }

    private bool _hasMissedCall;
    private ICall _currentCall;

    public bool HasMissedCall
    {
      get { return _hasMissedCall; }
      private set
      {
        _hasMissedCall = value;
        OnPropertyChanged("HasMissedCall");
      }
    }

    private void CallsCollectionChangedHandler(NotifyCollectionChangedEventArgs args)
    {
      if (args.Action == NotifyCollectionChangedAction.Add)
        foreach (AsteriskCall call in args.NewItems)
        {
          var thisCall = call;
          thisCall.PropertyChanged += (s, e) =>
            {
              if (e.PropertyName == "State")
                SetState(thisCall);

              if (e.PropertyName == "IsMissed")
                SetMissedCallStatus(thisCall);
            };
          SetState(thisCall);
          SetMissedCallStatus(thisCall);
        }
      if (args.Action == NotifyCollectionChangedAction.Remove)
        foreach (AsteriskCall call in args.OldItems)
        {
          var thisCall = call;
          thisCall.PropertyChanged += (s, e) =>
          {
            if (e.PropertyName == "State")
              SetState(thisCall);

            if (e.PropertyName == "IsMissed")
              SetMissedCallStatus(thisCall);
          };
        }
    }

    private void SetMissedCallStatus(AsteriskCall thisCall)
    {
      if (thisCall.IsMissed)
      {
        HasMissedCall = true;
      }
    }

    //TODO this will need work when DND is included
    private void SetState(ICall call)
    {
      switch (call.State)
      {
        case CallStateEnum.Terminated:
       State = PhoneState.Available;
          break;

        case CallStateEnum.RingingOtherEnd:
            State = PhoneState.OnCall;
          break;

        case CallStateEnum.Ringing:
            State = PhoneState.OnCall;
          break;

        case CallStateEnum.OnCall:
          State = PhoneState.OnCall;
          break;

        default:
          State = PhoneState.OnCall;
          break;
      }

    }

    public void AddCall(ICall call)
    {
      //todo: cannot change an observable collection from within a observable collection 
      Calls.Add(call);
    }

    public void RemoveCall(ICall call)
    {
      Calls.Remove(call);
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