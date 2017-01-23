using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class VoiceMailJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonVoiceMail> aoData;

    public VoiceMailJsonViewModel(IEnumerable<IVoiceMail> extensions)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonVoiceMail>();

      foreach (IVoiceMail voiceMail in extensions)
      {
        var line = new string[10];
        line[0] = voiceMail.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = voiceMail.Number;
        line[2] = voiceMail.Password;
        line[3] = voiceMail.NumberOfMessages > 0 ? "yes" : "no";
        line[4] = voiceMail.HeldNumberOfMessages.ToString(CultureInfo.InvariantCulture);
        line[5] = voiceMail.MessageLength.ToString(CultureInfo.InvariantCulture);
        line[6] = voiceMail.DefaultEmail;
        line[7] = voiceMail.EmailNotificationHasMp3 ? "yes" : "no";
        line[8] = "";
        line[9] = "";

        aoData.Add(new JsonVoiceMail
          {
            Id = voiceMail.Id,
            Number = voiceMail.Number
          });
        aaData.Add(line);
      }
    }

    public class JsonVoiceMail
    {
      public int Id { get; set; }
      public string Number { get; set; }
    }
  }
}