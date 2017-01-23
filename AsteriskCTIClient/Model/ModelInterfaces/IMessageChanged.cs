using System;
using AsteriskCTIClient.Enums;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IMessageChanged
  {
    void NotifyChanged(MessageTypeEnum type, string name, string number, Action command);
  }
}