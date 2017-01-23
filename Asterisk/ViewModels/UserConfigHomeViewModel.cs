using System.Collections.Generic;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewModels
{
  public class UserConfigHomeViewModel
  {
    public IEnumerable<IUserConfig> UserConfigs { get; set; }
  }
}