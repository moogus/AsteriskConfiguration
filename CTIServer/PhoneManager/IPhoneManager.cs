using System.ComponentModel;
using CTIServer.Phone;
using CTIServer.Presence;

namespace CTIServer.PhoneManager
{
  public interface IPhoneManager : INotifyPropertyChanged
  {
    IPhone GetPhone(string extensionNumber);
    IPresence GetPresence(string extensionNumber);
    bool Connected { get; set; }
  }
}