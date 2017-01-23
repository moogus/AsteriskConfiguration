using System.Collections.Generic;
using DatabaseAccess;

namespace DialplanManager.Interfaces
{
  public interface IDialplanFilters
  {
    IDialplan FilterBasedOnDate(IEnumerable<IDialplanDate> input);
    IDialplan FilterBasedOnRange(IEnumerable<IDialplanRange> input);
  }
}