using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class CLIJsonViewModel
  {
    public class JsonCLI
    {
      public int Id { get; set; }
      public string CLIName { get; set; }
      public string CLINumber { get; set; }
      public string Trunk { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonCLI> aoData;

    public CLIJsonViewModel(IEnumerable<ICLI> cliData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonCLI>();
      foreach (var cli in cliData)
      {
        var line = new string[6];
        line[0] = cli.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = cli.CLIName;
        line[2] = cli.CLINumber;
        line[3] = cli.Trunk.Name;
        line[4] = "";
        line[5] = "";

        aoData.Add(new JsonCLI
          {
            Id = cli.Id,
            CLIName = cli.CLIName,
            CLINumber = cli.CLINumber,
            Trunk = cli.Trunk.Name
          });

        aaData.Add(line);
      }
    }
  }
}