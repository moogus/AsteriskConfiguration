using Asterisk.NET.Manager;
using Asterisk.NET.Manager.Action;

namespace CTIServer.Dial.Dialers
{
  public class AsteriskDialer : IDialer
  {
    private readonly ManagerConnection _managerConnection;

    public AsteriskDialer(ManagerConnection managerConnection)
    {
      _managerConnection = managerConnection;
    }

    public void AutoDial(string extension, string numberToDial)
    {
      var action = new OriginateAction
        {
          Channel = string.Format("local/{0}@AutoDial", extension),
          Context = "LocalSets",
          Exten = numberToDial,
          Priority = 1,
          Timeout = 10000,
          Async = true
        };
      _managerConnection.SendAction(action, 0);
    }
  }
}
