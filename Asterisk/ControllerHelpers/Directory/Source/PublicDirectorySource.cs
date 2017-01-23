using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Directory.Entry;
using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Directory.Source
{
  public class PublicDirectorySource : IDirectorySource
  {
    private readonly IRepository _modelRepository;

    public PublicDirectorySource(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
      CreateDirectory();
    }

    public IEnumerable<IDirectoryEntry> DirectoryEntries { get; private set; }

    private void CreateDirectory()
    {
      var allExtensions = _modelRepository.GetList<IExtension>().Where(ext => ext.IncludeInDirectory);
      var allQueues = _modelRepository.GetList<IQueue>().Where(que => que.IncludeInDirectory);

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