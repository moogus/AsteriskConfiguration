using AsteriskCTIStateLibrary.Enums;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.CallObject
{
  public class CallHangup : ICallState
  {
    private readonly Call _stateListener;

    public CallHangup(Call stateListener)
    {
      _stateListener = stateListener;
    }

    public void DeterminAction(OurEvent dialEvent)
    {
      if ((dialEvent.Type == SIPEvent.NewChannel ||dialEvent.Type==SIPEvent.NewCallerId))
      {
        _stateListener.ChangeState(_stateListener.Ringing);
        _stateListener.CallStateEnum = CallStateEnum.Ringing;
      }
      
    }
  }
}

