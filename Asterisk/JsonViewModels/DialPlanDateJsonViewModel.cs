using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class DialPlanDateJsonViewModel
  {
    public List<string[]> aaData;

    public DialPlanDateJsonViewModel(IEnumerable<IDialplanDate> dialplanDates)
    {
      aaData = new List<string[]>();

      foreach (IDialplanDate dp in dialplanDates)
      {
        var line = new string[5];
        line[0] = dp.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = string.Format("{0}-{1}", dp.StartDate.ToString("dd/MM/yyyy"), dp.EndDate.ToString("dd/MM/yyyy"));
        line[2] = dp.Dialplan.Name;
        line[3] = "";
        line[4] = "";

        aaData.Add(line);
      }
    }
  }

  public class DialPlanDateRangeViewModel
  {
    public IDialplanDate StartDate { get; set; }
    public IDialplanDate EndDate { get; set; }
  }
}