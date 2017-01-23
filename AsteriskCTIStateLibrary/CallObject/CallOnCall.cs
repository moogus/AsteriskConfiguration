using AsteriskCTIStateLibrary.Enums;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.CallObject
{
  class CallOnCall : ICallState
  {
    private readonly Call _stateListener;


    public CallOnCall(Call stateListener)
    {
      _stateListener = stateListener;
    }

    public void DeterminAction(OurEvent dialEvent)
    {
     if (dialEvent.Type == SIPEvent.Dial && dialEvent.SubEvent == "End")
      {
        _stateListener.ChangeState(_stateListener.Hangup);
        _stateListener.SetDialStatus(dialEvent.DialStatus);
        _stateListener.CallStateEnum = CallStateEnum.HangUp;
      }

    }
  }

}
