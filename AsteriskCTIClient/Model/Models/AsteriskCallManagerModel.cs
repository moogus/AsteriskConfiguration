using System;
using AsteriskCTIClient.Enums;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Call;

namespace AsteriskCTIClient.Model.Models
{
  public class AsteriskCallManagerModel
  {
    private readonly IMessageChanged _messageChanged;
    private readonly Action _getCallerInformationAction;
    private readonly INumber _presentValidNumber;
    private readonly ICtiCall _ctiCall;
    private readonly bool _isExternal;

    public AsteriskCallManagerModel(ICtiCall ctiCall, IMessageChanged messageChanged, Action getCallerInformationAction, INumber presentValidNumber)
    {
      _isExternal = false;
      _ctiCall = ctiCall;
      _messageChanged = messageChanged;
      _getCallerInformationAction = getCallerInformationAction;
      _presentValidNumber = presentValidNumber;
      CallChanged();

      _ctiCall.PropertyChanged += (sender, args) =>
        {
          if (args.PropertyName == "State")
            CallChanged();
        };
    }

    private void CallChanged()
    {
      var messageType = MessageTypeEnum.InvalidDial;
      if (!_ctiCall.Incoming && !_ctiCall.IsvalidAsteriskExtension(_ctiCall.OtherEndNumber))
      {
        messageType = MessageTypeEnum.InvalidDial;
      }
      else
      {
        switch (_ctiCall.State)
        {
          case CallStateEnum.Ringing:
            messageType = MessageTypeEnum.IncommingRing;
            break;

          case CallStateEnum.RingingOtherEnd:
            messageType = _isExternal ? MessageTypeEnum.DialExternalRing : MessageTypeEnum.DialInternalRing;
            break;

          case CallStateEnum.OnCall:
            messageType = _ctiCall.Incoming
                            ? MessageTypeEnum.IncomingOnCall : _isExternal ? MessageTypeEnum.DialExternalOnCall
                                                                          : MessageTypeEnum.DialInternalOnCall;
            break;

          case CallStateEnum.Terminated:
            messageType = _ctiCall.Incoming
                            ? MessageTypeEnum.IncomingHangup : _isExternal ? MessageTypeEnum.DialExternalHangup
                                                                           : MessageTypeEnum.DialInternalHangup;
            break;

          //TODO: Consider all other states!!!!!
        }
      }
      _presentValidNumber.Number = _ctiCall.OtherEndNumber;

      SetNotifyIconClickFunction(messageType);
    }


    //TODO: if the call is internal should there be any functionality on the icon click?
    private void SetNotifyIconClickFunction(MessageTypeEnum messageType)
    {
      _messageChanged.NotifyChanged(messageType, _ctiCall.CallerName, _presentValidNumber.Number,
                                    _ctiCall.OtherEndNumber.Length > 6 ? _getCallerInformationAction : null);
    }
  }
}