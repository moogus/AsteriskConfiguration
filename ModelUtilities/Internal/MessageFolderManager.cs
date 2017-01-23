using System.Collections.Generic;

namespace ModelUtilities.Internal
{
  internal class MessageFolderManager : IMessageFolderManager
  {
    public IEnumerable<IFolderInformation> GetAllMessageFolders()
    {
      return new List<IFolderInformation>
        {
          new FolderInformation {FolderName = "INBOX", Order= 1},
          new FolderInformation {FolderName = "Old", Order= 2},
         new FolderInformation {FolderName = "Deleted", Order= 3}
        };
    }
  }
}