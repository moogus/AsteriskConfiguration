using System.Collections.ObjectModel;
using CTIServer.Call;
using CTIServer.Presence;

namespace CTIServer.Phone
{
  public interface IPhone : IPresence
  {
    bool HasMissedCall { get; }
    ObservableCollection<ICall> Calls { get; }
    void AddCall(ICall call);
    void RemoveCall(ICall call);
  }
}
