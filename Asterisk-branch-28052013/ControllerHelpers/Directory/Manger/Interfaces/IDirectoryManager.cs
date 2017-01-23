using System.Collections.Generic;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;

namespace Asterisk.ControllerHelpers.Directory.Manger.Interfaces
{
  public interface IDirectoryManager
  {
    IEnumerable<IDirectoryEntry> GetDirectoryEntries(bool onlyNative);
  }
}