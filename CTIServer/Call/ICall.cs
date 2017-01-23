using System;
using System.ComponentModel;
using CTIServer.Phone;

namespace CTIServer.Call
{
  public interface ICall : INotifyPropertyChanged
  {
    string Id { get; }
    string OtherEndNumber { get; }
    string Channel { get; }
    bool Incoming { get; }
    bool IsMissed { get; }
    DateTime StartTime { get; }
    DateTime EndTime { get; }
    CallStateEnum State { get; }
    void Pickup(IPhone phone);
  }
}