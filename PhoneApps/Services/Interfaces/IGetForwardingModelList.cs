using System.Collections.Generic;
using DatabaseAccess;
using PhoneApps.Models.Interfaces;

namespace PhoneApps.Services.Interfaces
{
  public interface IGetForwardingModelList
  {
    IEnumerable<IForwardingModel> GetForwadingModelsFromType(ForwardingDestination forwardingRouteType);
  }
}