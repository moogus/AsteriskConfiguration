using System.ComponentModel;
using Asterisk.NET.Manager;
using CTIServer.Dial;

namespace CTIServer.ConnectionManger
{
  public interface IConnectionManager : INotifyPropertyChanged
  {
    bool Connected { get; }
    ManagerConnection ManagerConnection { get; }
    IDialer Dialer { get; }
    void Connect();
  }
}