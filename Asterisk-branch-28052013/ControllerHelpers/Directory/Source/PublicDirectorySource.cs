using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using DatabaseAccess;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class PublicDirectorySource : IDirectorySource
  {
    private readonly IRepository _repository;
    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    public PublicDirectorySource(IRepository repository)
    {
      _repository = repository;
      CreateDirectory();
    }

    private void CreateDirectory()
    {
      var allExtensions = _repository.GetList<IExtension>().Where(ext => ext.IncludeInDirectory);
      var allQueues = _repository.GetList<IQueue>().Where(que => que.IncludeInDirectory);

      DirectoryEntries = CreateDirectoriesFromExtensionsAndQueues(allExtensions, allQueues);
    }

    private IEnumerable<IDirectoryEntry> CreateDirectoriesFromExtensionsAndQueues(IEnumerable<IExtension> allExtensions,
                                                                                  IEnumerable<IQueue> allQueues)
    {
      return
        allExtensions.Select(ext => new ExtensionDirectoryEntry(ext))
                     .Cast<IDirectoryEntry>()
                     .ToList()
                     .Concat(allQueues.Select(queue => new QueueDirectoryEntry(queue)));
    }
  }
}