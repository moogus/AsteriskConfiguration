using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class AsteriskDirectorySource : IDirectorySource
  {
    private readonly IRepository _modelRepository;

    public AsteriskDirectorySource(IRepository modelRepository)
    {
      _modelRepository = modelRepository;

      CreateAllContacts();
    }

    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    private void CreateAllContacts()
    {
      DirectoryEntries = CreateIDirectoryList(_modelRepository.GetList<IExtension>().
                                                          Where(ext => ext.IncludeInDirectory)
                                                         .Select(e => new ExtensionDirectoryEntry(e))).
        Concat(_modelRepository.GetList<ITrunk>().Select(t => new TrunkDirectoryEntry(t)));
    }

    private IEnumerable<IDirectoryEntry> CreateIDirectoryList(IEnumerable<IDirectoryEntry> listOfDirectoryEntries)
    {
      return listOfDirectoryEntries.ToList();
    }
  }
}