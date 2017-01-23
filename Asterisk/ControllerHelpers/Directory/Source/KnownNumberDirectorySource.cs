using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class KnownNumberDirectorySource : IDirectorySource
  {
    private readonly IRepository _modelRepository;

    public KnownNumberDirectorySource(IRepository modelRepository)
    {
      _modelRepository = modelRepository;

      CreateDirectory();
    }

    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    private void CreateDirectory()
    {
      DirectoryEntries =
        _modelRepository.GetList<IKnownNumber>()
                   .Select(knownNumber => new KnownNumberDirectoryEntry(knownNumber))
                   .Cast<IDirectoryEntry>()
                   .ToList();
    }
  }
}