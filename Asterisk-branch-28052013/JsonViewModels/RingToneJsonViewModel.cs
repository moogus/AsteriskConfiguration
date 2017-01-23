using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class RingToneJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonRingtone> aoData;

    public RingToneJsonViewModel(IEnumerable<IRingTone> ringtoneData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonRingtone>();
      foreach (var ringTone in ringtoneData)
      {
        var line = new string[2];

        line[0] = ringTone.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = ringTone.Name;

        aoData.Add(new JsonRingtone
          {
            Id = ringTone.Id,
            Name = ringTone.Name
          });

        aaData.Add(line);
      }
    }

    public class JsonRingtone
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
  }
}