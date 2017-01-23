using System.Collections.Generic;
using DatabaseAccess;
using PhoneApps.Models.Interfaces;

namespace PhoneApps.ViewModels.Interfaces
{
  public interface IForwardingRouteVM
  {
    string CurrentExtension { get; }
    bool IsEnabled { get; }
    IEnumerable<IForwardingModel> RoutesInformation { get; }
    ForwardingDestination ForwardingType { get; }
    string ActionMessage { get; set; }
  }
}