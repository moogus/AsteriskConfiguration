using System.Collections.Generic;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewModels
{
  public class QueueMemberViewModel
  {
    public IEnumerable<IExtension> ListOfExtensions;
    public IEnumerable<IQueue> ListOfQueues;
    public IQueue Queue;
    public string Status { get; set; }
  }
}