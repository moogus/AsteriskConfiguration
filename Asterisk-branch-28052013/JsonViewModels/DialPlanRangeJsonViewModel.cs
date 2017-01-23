using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class DialPlanRangeJsonViewModel
  {
    public List<string[]> aaData;

    public DialPlanRangeJsonViewModel(IEnumerable<IDialplanRange> dialplanRanges)
    {
      aaData = new List<string[]>();
      foreach (var dpr in dialplanRanges)
      {
        var line = new string[7];
        line[0] = dpr.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = dpr.Priority.ToString(CultureInfo.InvariantCulture);
        line[2] = dpr.DaysOfWeek;
        line[3] = dpr.TimeRange;
        line[4] = dpr.Dialplan.Name;
        line[5] = "";
        line[6] = "";

        aaData.Add(line);
      }
    }
  }
}