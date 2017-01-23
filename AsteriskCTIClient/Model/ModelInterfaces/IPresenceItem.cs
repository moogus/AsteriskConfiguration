using AsteriskCTIClient.Enums;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IPresenceItem
  {
    string Extension { get; set; }
    PresenceStateEnum PresenceStateEnum { get; set; }
    //TODO: are these needed?
    //void ConnectToServer();
    //void DisconnectFromServer();
  }
}