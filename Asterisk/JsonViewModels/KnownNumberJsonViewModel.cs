using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class KnownNumberJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonKnownNumber> aoData;

    public KnownNumberJsonViewModel(IEnumerable<IKnownNumber> knownNumberData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonKnownNumber>();

      foreach (IKnownNumber kn in knownNumberData)
      {
        var line = new string[6];
        line[0] = kn.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = kn.Number;
        line[2] = kn.Description;
        line[3] = kn.IsInternal ? "yes" : "no";
        line[4] = "";
        line[5] = "";
        aaData.Add(line);

        aoData.Add(new JsonKnownNumber
          {
            Id = kn.Id,
            Number = kn.Number,
            Description = kn.Description,
            IsInternal = kn.IsInternal
          });
      }
    }

    public class JsonKnownNumber
    {
      public int Id { get; set; }
      public string Number { get; set; }
      public string Description { get; set; }
      public bool IsInternal { get; set; }
    }
  }
}