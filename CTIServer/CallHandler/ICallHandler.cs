using System.Collections.ObjectModel;
using CTIServer.Call;

namespace CTIServer.CallHandler
{
  public interface ICallHandler
  {
    ObservableCollection<ICall> Calls { get; }
  }
}