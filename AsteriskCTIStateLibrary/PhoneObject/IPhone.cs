using System.Collections.ObjectModel;
using AsteriskCTIStateLibrary.CallObject;
using AsteriskCTIStateLibrary.ManagerObject;

namespace AsteriskCTIStateLibrary.PhoneObject
{
  public interface IPhone
  {
    ObservableCollection<Call> Calls { get; }
    string MyNumber { get; set; }
    void RegisterWithManager(AsteriskManager manager);
  }
}
