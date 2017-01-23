using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using DatabaseAccess;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class KnownNumberDirectorySource : IDirectorySource
  {
    private readonly IRepository _repository;
    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    public KnownNumberDirectorySource(IRepository repository)
    {
      _repository = repository;

      CreateDirectory();
    }

    private void CreateDirectory()
    {
      DirectoryEntries =
        _repository.GetList<IKnownNumber>()
                   .Select(knownNumber => new KnownNumberDirectoryEntry(knownNumber))
                   .Cast<IDirectoryEntry>()
                   .ToList();
    }
  }
}