using System.Collections.Generic;
using DatabaseAccess;

namespace PhoneApps.Services.Interfaces
{
  public interface IGetForwardingFromExtension
  {
    IEnumerable<IRoutingRule> ForwardingRules { get; }
    void SetBaseForwarding(string extension);
    string GetCurrentForwardingDialplan(string dialPlanName);
    ForwardingDestination GetForwardingType(string extension, string dialPlan);
    string GetForwardingNumber(string extension, string dialPlan);
    bool IsForwardingEnabled(string extension, string dialPlan);
  }
}