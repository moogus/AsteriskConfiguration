using System.Collections.Generic;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;

namespace Asterisk.ControllerHelpers.Directory.Source.Interfaces
{
  public interface IDirectorySource
  {
    IEnumerable<IDirectoryEntry> DirectoryEntries { get; }
  }
}