using System.ComponentModel;
using CTIServer.Phone;

namespace CTIServer.Presence
{
  public interface IPresence : INotifyPropertyChanged
  {
    string DDI { get; }
    string ExtensionNumber { get; }
    string Name { get; }
    string Department { get; }
    PhoneState State { get; set; }
  }
}