using System.Collections.Generic;
using DatabaseAccess;

namespace PhoneApps.ViewModels.Interfaces
{
  public interface IForwardingVM
  {
    ForwardingDestination CurrentRuleType { get; }
     string CurrentRuleDestination { get; }
    IEnumerable<string> RouteTypes { get; }
    string CurrentExtension { get; }
  }


}