using System.Collections.Generic;
using System.Linq;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.ModelHelpers
{
  internal class VoiceMessageFolder : IVoiceMessageFolder
  {
    public string FolderName { get; set; }
    public List<IVoiceMessage> FolderMessages { get; set; }
    public int Order { get; set; }

    public VoiceMessageFolder(IEnumerable<IVoiceMessage> voiceMessages, string folderName, int order)
    {
      FolderName = folderName;
      FolderMessages = voiceMessages.Where(vm => vm.Folder.FolderName == folderName).ToList();
      Order = order;
    }
  }
}
