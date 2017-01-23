using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Manger.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;

namespace Asterisk.ControllerHelpers.Directory.Manger
{
  public class DirectoryManager : IDirectoryManager
  {
    private readonly List<IDirectorySource> _directorySources;

    public DirectoryManager(List<IDirectorySource> directorySources)
    {
      _directorySources = directorySources;
    }

    public IEnumerable<IDirectoryEntry> GetDirectoryEntries(bool onlyNative)
    {
      var rtn = new List<IDirectoryEntry>();

      if (onlyNative)
      {
        foreach (IDirectorySource source in _directorySources)
        {
          rtn.AddRange(
            source.DirectoryEntries.Where(
              d =>
              d.Entrytype != DirectoryEntrytype.KnownNumberExternal &&
              d.Entrytype != DirectoryEntrytype.KnownNumberInternal));
        }
      }
      else
      {
        foreach (IDirectorySource source in _directorySources)
        {
          rtn.AddRange(source.DirectoryEntries.Where(d => d.Entrytype != DirectoryEntrytype.KnownNumberExternal));
        }
      }
      IOrderedEnumerable<IDirectoryEntry> x = rtn.OrderBy(e => e.Name);
      return x;
    }
  }
}