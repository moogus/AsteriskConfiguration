using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class DDIJsonViewModel
  {
    public class JsonDDI
    {
      public int Id { get; set; }
      public string DDINumber { get; set; }
      public string Used { get; set; }
      public string Trunk { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonDDI> aoData;

    public DDIJsonViewModel(IEnumerable<IDDI> ddiData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonDDI>();

      foreach (var ddi in ddiData)
      {
        var line = new string[6];
        line[0] = ddi.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = ddi.DDINumber;
        line[2] = ddi.Used;
        line[3] = ddi.Trunk == null ? "" : ddi.Trunk.Name;
        line[4] = "";
        line[5] = "";
        aaData.Add(line);

        aoData.Add(new JsonDDI
          {
            DDINumber = ddi.DDINumber,
            Id = ddi.Id,
            Trunk = ddi.Trunk == null ? "" : ddi.Trunk.Name,
            Used = ddi.Used
          });
      }
    }
  }
}