using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using DatabaseAccess;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class AsteriskDirectorySource : IDirectorySource
  {
    private readonly IRepository _repository;

    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    public AsteriskDirectorySource(IRepository repository)
    {
      _repository = repository;

      CreateAllContacts();
    }

    private void CreateAllContacts()
    {
      DirectoryEntries = CreateIDirectoryList(_repository.GetList<IExtension>().
                                                          Where(ext => ext.IncludeInDirectory)
                                                         .Select(e => new ExtensionDirectoryEntry(e))).
        Concat(_repository.GetList<ITrunk>().Select(t => new TrunkDirectoryEntry(t)));
    }

    private IEnumerable<IDirectoryEntry> CreateIDirectoryList(IEnumerable<IDirectoryEntry> listOfDirectoryEntries)
    {
      return listOfDirectoryEntries.ToList();
    }
  }
}