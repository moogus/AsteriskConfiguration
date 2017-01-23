using System;
using System.Collections.Generic;

namespace ModelUtilities
{
  public interface IMessageFolderManager
  {
    IEnumerable<IFolderInformation> GetAllMessageFolders();
  }
}