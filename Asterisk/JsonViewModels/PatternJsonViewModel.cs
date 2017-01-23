using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class PatternJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonPattern> aoData;

    public PatternJsonViewModel(IEnumerable<IPermissionPattern> patternData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonPattern>();

      foreach (IPermissionPattern pattern in patternData)
      {
        var line = new string[5];
        line[0] = pattern.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = pattern.Name;
        line[2] = pattern.Pattern;
        line[3] = "";
        line[4] = "";
        aaData.Add(line);
        aoData.Add(new JsonPattern {Id = pattern.Id, Pattern = pattern.Pattern, Name = pattern.Name});
      }
    }

    public class JsonPattern
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Pattern { get; set; }
    }
  }
}