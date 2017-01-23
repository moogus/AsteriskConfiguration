using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class AutoAttendantJsonViewModel
  {
    public class JsonAutoAttendant
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonAutoAttendant> aoData;

    public AutoAttendantJsonViewModel(IEnumerable<IAutoAttendant> autoData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonAutoAttendant>();
      foreach (var aa in autoData)
      {
        var line = new string[7];
        line[0] = aa.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = aa.Name;
        line[2] = string.Format("{0},{1}", aa.Name, aa.Id.ToString(CultureInfo.InvariantCulture));
        line[3] = aa.Timeout.ToString(CultureInfo.InvariantCulture);
        line[4] = aa.Id.ToString(CultureInfo.InvariantCulture);
        line[5] = "";
        line[6] = "";

        aoData.Add(new JsonAutoAttendant
          {
            Id = aa.Id,
            Name = aa.Name
          });

        aaData.Add(line);
      }
    }
  }
}