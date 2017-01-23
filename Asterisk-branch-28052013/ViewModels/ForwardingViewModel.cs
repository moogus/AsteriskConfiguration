using System.Collections.Generic;
using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class ForwardingViewModel
  {
    public IExtension ThisExtension { get; set; }
    public IEnumerable<IRoutingRule> PersonalRules { get; set; }
  }
}