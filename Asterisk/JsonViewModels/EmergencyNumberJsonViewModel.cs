using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class EmergencyNumberJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonEmergencyNumber> aoData;

    public EmergencyNumberJsonViewModel(IEnumerable<IEmergencyNumber> emergencyData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonEmergencyNumber>();

      foreach (IEmergencyNumber aa in emergencyData)
      {
        var line = new string[5];
        line[0] = aa.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = aa.Description;
        line[2] = aa.Number;
        line[3] = "";
        line[4] = "";

        aaData.Add(line);

        aoData.Add(new JsonEmergencyNumber
          {
            Id = aa.Id,
            Number = aa.Number,
            Description = aa.Description
          });
      }
    }

    public class JsonEmergencyNumber
    {
      public int Id { get; set; }
      public string Number { get; set; }
      public string Description { get; set; }
    }
  }
}