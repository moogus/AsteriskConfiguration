using System.Collections.Generic;
using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class UserConfigHomeViewModel
  {
    public IEnumerable<IUserConfig> UserConfigs { get; set; }
  }
}