using AsteriskCTIStateLibrary.Enums;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.CallObject
{
  public class CallRinging : ICallState
  {
    private readonly Call _stateListener;

    public CallRinging(Call stateListener)
    {
      _stateListener = stateListener;
    }

    public void DeterminAction(OurEvent dialEvent)
    {
      // check if this is an end event
      // if so change state to hangup
      if (dialEvent.Type == SIPEvent.Dial && dialEvent.SubEvent == "End")
      {
        _stateListener.ChangeState(_stateListener.Hangup);
        _stateListener.SetDialStatus(dialEvent.DialStatus);
        _stateListener.CallStateEnum = CallStateEnum.HangUp;
        return;
      }
      if (dialEvent.Type == SIPEvent.NewState && dialEvent.ChannelStateDesc == "Up")
      {
        _stateListener.ChangeState(_stateListener.OnCall);
        _stateListener.CallersNumber = dialEvent.CallerIdNum;
        _stateListener.CallStateEnum = CallStateEnum.OnCall;
        return;
      }

    }
  }
}
