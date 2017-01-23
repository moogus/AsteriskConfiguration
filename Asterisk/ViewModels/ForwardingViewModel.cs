using System.Collections.Generic;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewModels
{
  public class ForwardingViewModel
  {
    public IExtension ThisExtension { get; set; }
    public IEnumerable<IRoutingRule> PersonalRules { get; set; }
  }
}