using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
  public interface IVoiceMessageFolder
  {
    string FolderName { get; set; }
    List<IVoiceMessage> FolderMessages { get; set; }
    int Order { get; set; }
  }
}
