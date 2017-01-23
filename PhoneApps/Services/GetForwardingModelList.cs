using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;
using PhoneApps.Models;
using PhoneApps.Models.Interfaces;
using PhoneApps.Services.Interfaces;

namespace PhoneApps.Services
{
  public class GetForwardingModelList : IGetForwardingModelList
  {
    private readonly IRepository _repository;

    public GetForwardingModelList(IRepository repository)
    {
      _repository = repository;
    }

    public IEnumerable<IForwardingModel> GetForwadingModelsFromType(ForwardingDestination forwardingRouteType)
    {
      switch (forwardingRouteType)
      {
        case ForwardingDestination.Extension:
          return _repository.GetList<IExtension>().Select(e => new ForwardingModel(e));

        case ForwardingDestination.Group:
          return _repository.GetList<IQueue>().Select(q => new ForwardingModel(q));
        
        case ForwardingDestination.Voicemail:
          return _repository.GetList<IVoiceMail>().Select(v => new ForwardingModel(v));

        case ForwardingDestination.External:
          return new List<IForwardingModel>{new ForwardingModel()};

        default:
          return _repository.GetList<IExtension>().Select(e => new ForwardingModel(e));
      }
    }
  }
}