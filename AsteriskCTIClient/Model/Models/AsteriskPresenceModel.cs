using System.ComponentModel;
using AsteriskCTIClient.Enums;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Presence;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class AsteriskPresenceModel : IPresenceItem, INotifyPropertyChanged
  {
    private readonly IExtension _extension;
    private readonly IPresence _presence;
    private readonly IRepository _repository;
    private PresenceStateEnum _presenceStateEnum;

    public AsteriskPresenceModel(IPresence presence, IRepository repository)
    {
      _repository = repository;

      _presence = presence;
      _presence.PropertyChanged += (sender, args) =>
        {
          if (args.PropertyName == "State")
          {
            PhoneStateCheck();
          }
        };

      Extension = _presence.ExtensionNumber;
      _extension = _repository.GetFromName<IExtension>(Extension);
      PhoneStateCheck();
    }

    public string Extension { get; set; }

    public PresenceStateEnum PresenceStateEnum
    {
      get { return _presenceStateEnum; }
      set
      {
        _presenceStateEnum = value;
        OnPropertyChanged("PresenceStateEnum");
      }
    }

    private void PhoneStateCheck()
    {
      switch (_presence.State)
      {
        case PhoneState.OnCall:
          PresenceStateEnum = PresenceStateEnum.OnCall;
          break;
        case PhoneState.DND:
          PresenceStateEnum = PresenceStateEnum.UserSetUnavailable;
          break;
        default:
          SetPresenceForUserNotOnDnD(_extension, PresenceStateEnum.Available);
          break;
      }
    }

    private void SetPresenceForUserNotOnDnD(IExtension extension, PresenceStateEnum currentState)
    {
      //TODO : fix this NHibernate keeps blowing up
      extension.Refresh();
      PresenceStateEnum = extension.DND ? PresenceStateEnum.UserSetUnavailable : currentState;
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}