using System.Collections.Generic;
using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class QueueMemberViewModel
  {
    public IQueue Queue;
    public IEnumerable<IExtension> ListOfExtensions;
    public IEnumerable<IQueue> ListOfQueues;
    public string Status { get; set; }
  }
}