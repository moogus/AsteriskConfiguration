using System.Collections.Generic;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewModels
{
  public class EndUserCurrentDialplanViewModel
  {
    public string Message { get; set; }
    public ICurrentDialPlan CurrentDialPlan { get; set; }
    public IEnumerable<IDialplan> Dialplans { get; set; }
  }
}