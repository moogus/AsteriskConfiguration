using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class ExtensionsJsonViewModel
  {
    public class JsonExtension
    {
      public string Number { get; set; }
      public string FullName { get; set; }
      public string Department { get; set; }
      public string JobTitle { get; set; }
      public string Email { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonExtension> aoData;

    public ExtensionsJsonViewModel(IEnumerable<IExtension> extensions, List<IUserConfig> userCredentials)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonExtension>();
      foreach (var exten in extensions)
      {
        var line = new string[20];
        line[0] = exten.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = exten.Number;
        line[2] = exten.Password;
        line[3] = exten.DDI != null ? exten.DDI.DDINumber : "";
        line[4] = exten.CLI.CLINumber;
        line[5] = exten.FirstName;
        line[6] = exten.LastName;
        line[7] = exten.Department;
        line[8] = exten.JobTitle;
        line[9] = exten.Email;
        line[10] = exten.PermisionClass.Name;
        line[11] = exten.VoiceMail == null ? "" : exten.VoiceMail.Number;
        line[12] = exten.VoicemailDelay.ToString(CultureInfo.InvariantCulture);

        line[13] = exten.IpAddress;
        line[14] = exten.Model;
        line[15] = exten.Status;
        line[16] = userCredentials.First(u => u.Number == exten.Number).Role;
        line[17] = exten.IncludeInDirectory ? "include" : "exclude";
        line[18] = "";
        line[19] = "";

        aoData.Add(new JsonExtension
          {
            Number = exten.Number,
            Department = exten.Department,
            FullName = string.Format("{0} {1}", exten.FirstName, exten.LastName),
            JobTitle = exten.JobTitle,
            Email = exten.Email
          });

        aaData.Add(line);
      }
    }
  }
}