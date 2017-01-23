using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using AsteriskCTIClient.Enums;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.Views.CtiBalloon;
using CTIServer.PhoneManager;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class NotifyIconVM : INotifyPropertyChanged, IMessageChanged
  {
    private readonly IPhoneManager _manager;
    private readonly INumber _presentValidNumber;
    private bool _connected;
    private MessageVM _message;

    public CtiBalloonPopupVM CtiBalloonVm { get; private set; }

    public bool IsDialing { get; set; }
    public ICommand OpenSettingsWindowCommand { get; set; }
    public ICommand OpenPhoneWindowCommand { get; set; }
    public ICommand OpenHistoryWindowCommand { get; set; }
    public ICommand MessageActionCommand { get; set; }

    public bool Connected
    {
      get { return _connected; }
      private set
      {
        _connected = value;
        OnPropertyChanged("Connected");
      }
    }

    public string CopyPasteString { get; set; }

    public MessageVM Message
    {
      get { return _message; }
      set
      {
        SetBalloonProperties(value);
        _message = value;
        OnPropertyChanged("Message");
      }
    }

    public NotifyIconVM(IPhoneManager manager, ICommand showSettingsWindowCommand, ICommand showPhoneWindowCommand, ICommand showHistoryWindowCommand,
      INumber presentValidNumber , CtiBalloonPopupVM ctiBalloonVm)
    {
      _manager = manager;
      _presentValidNumber = presentValidNumber;

      _manager.PropertyChanged += (sender, args) =>
        {
          if (args.PropertyName == "Connected")
          {
            Connected = _manager.Connected;
          }
        };

      OpenSettingsWindowCommand = showSettingsWindowCommand;
      OpenPhoneWindowCommand = showPhoneWindowCommand;
      OpenHistoryWindowCommand = showHistoryWindowCommand;
      CtiBalloonVm = ctiBalloonVm;
    }

    void IMessageChanged.NotifyChanged(MessageTypeEnum messageType, string name, string number, Action command)
    {
      _presentValidNumber.Number = number;

      Message = new MessageVM(messageType, name, _presentValidNumber.OriginalNumber, command);
    }

    public void OpenCrmForCompanyNumber()
    {
      Process.Start("http://frontend/db/main.fpl?execute=phonesearch&number=" + _presentValidNumber.Number);
    }

    public void ShowStartUpConnectedState()
    {
      Connected = _manager.Connected;
    }

    private void SetBalloonProperties(MessageVM message)
    {
      switch (message.MessageType)
      {
        case MessageTypeEnum.DialExternalRing:
        case MessageTypeEnum.DialInternalRing:
          CtiBalloonVm.BalloonTitle = "Dial";
          CtiBalloonVm.BalloonText = SetMessage("Dialing ", message.Name, message.Number, "on");
          break;

        case MessageTypeEnum.DialExternalOnCall:
        case MessageTypeEnum.DialInternalOnCall:
          CtiBalloonVm.BalloonTitle = "Dial";
          CtiBalloonVm.BalloonText = SetMessage("Connected to", message.Name, message.Number, "on");
          break;

        case MessageTypeEnum.DialExternalHangup:
        case MessageTypeEnum.DialInternalHangup:
          CtiBalloonVm.BalloonTitle = "Dial";
          CtiBalloonVm.BalloonText = SetMessage("Connection ended", message.Name, message.Number, "on");
          break;

        case MessageTypeEnum.IncommingRing:
          CtiBalloonVm.BalloonTitle = "Incoming Call";
          CtiBalloonVm.BalloonText = SetMessage("You have an incomming call from", message.Name, message.Number, "on");
          break;

        case MessageTypeEnum.IncomingOnCall:
          CtiBalloonVm.BalloonTitle = "Incoming Call";
          CtiBalloonVm.BalloonText = SetMessage("Connected to", message.Name, message.Number, "on");
          break;

        case MessageTypeEnum.IncomingHangup:
          CtiBalloonVm.BalloonTitle = "Incoming Call";
          CtiBalloonVm.BalloonText = SetMessage("Connection ended", message.Name, message.Number, "on");
          break;


        case MessageTypeEnum.NumberToDial:
          CtiBalloonVm.BalloonTitle = "Do you want to dial?";
          CtiBalloonVm.BalloonText = SetMessage("You have just copied a number ", message.Number, "click this message to dial.", "");
          break;

      }

      CtiBalloonVm.DoAction = message.ActionAgainstMessage;
    }

    private string SetMessage(string baseMessage, string name, string number, string condition)
    {
      return string.Format("{0} {1} {3} {2}", baseMessage, name, number, condition);
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

  }
}