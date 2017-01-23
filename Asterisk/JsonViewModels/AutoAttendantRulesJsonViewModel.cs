using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class AutoAttendantRulesJsonViewModel
  {
    public List<string[]> aaData;

    public AutoAttendantRulesJsonViewModel(IEnumerable<IAutoAttendantRules> autoData)
    {
      aaData = new List<string[]>();
      foreach (IAutoAttendantRules aR in autoData)
      {
        var line = new string[5];
        line[0] = aR.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = aR.Entry == "i" ? "invalid" : aR.Entry == "t" ? "timeout" : aR.Entry;
        line[2] = string.Format("{0}, {1}", aR.DestinationType, aR.DestinationNumber);
        line[3] = "";
        line[4] = "";
        aaData.Add(line);
      }
    }
  }
}