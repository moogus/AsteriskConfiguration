using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class DefaultJsonViewModel
  {
    public List<string[]> aaData;

    public DefaultJsonViewModel(IEnumerable<IDefault> defaults)
    {
      aaData = new List<string[]>();
      foreach (var d in defaults)
      {
        var line = new string[6];
        line[0] = d.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = d.Type;
        line[2] = d.ColumnTitle;
        line[3] = d.DefaultValue;
        line[4] = "";
        line[5] = "";

        aaData.Add(line);
      }
    }
  }
}