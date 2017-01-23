using System;
using System.Windows.Input;
using AsteriskCTIClient.Enums;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class MessageVM
  {
    public MessageVM(MessageTypeEnum messageType, string name, string number, Action actionAgainstMessage)
    {
      MessageType = messageType;
      Name = name;
      Number = number;
      //TODO: if the call is internal should there be any functionality on the icon click?
      ActionAgainstMessage = actionAgainstMessage;
    }

    public MessageTypeEnum MessageType { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public Action ActionAgainstMessage { get; set; }
  }
}