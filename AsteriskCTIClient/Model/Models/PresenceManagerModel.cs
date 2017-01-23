using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.PhoneManager;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class PresenceManagerModel : IPresenceManagerModel
  {
    private readonly IPhoneManager _manager;
    private readonly IRepository _repository;

    public PresenceManagerModel(IPhoneManager manager, IRepository repository)
    {
      _manager = manager;
      _repository = repository;
    }

    public IPresenceItem GetAsteriskPresenceItem(string extension)
    {
      return new AsteriskPresenceModel(_manager.GetPresence(extension), _repository);
    }
  }
}