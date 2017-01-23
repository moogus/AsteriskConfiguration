using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class PermissionClassJsonViewModel
  {
    public List<string[]> aaData;
    public List<JsonPermissionClass> aoData;


    public PermissionClassJsonViewModel(IEnumerable<IPermisionClass> pcData, List<IExtension> extensions,
                                        int currentDialPlan)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonPermissionClass>();

      foreach (IPermisionClass pc in pcData.Where(p => p.Id != 1))
      {
        var line = new string[7];
        line[0] = pc.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = pc.Name;
        line[2] = pc.Description;


        string pString = pc.PermissionClassMemebers.Where(p => p.Dialplan.Id == currentDialPlan).Aggregate
          (
            string.Empty, (current, mem) => string.IsNullOrEmpty(current)
                                              ? mem.Pattern.Name
                                              : string.Format("{0},{1}", current, mem.Pattern.Name
                                                  ));
        line[3] = pString;

        string allExt = extensions.Where(e => e.PermisionClass.Id == pc.Id).Aggregate(
          string.Empty, (current, ext) => string.IsNullOrEmpty(current)
                                            ? ext.Number
                                            : string.Format("{0},{1}", current, ext.Number));
        line[4] = allExt;
        line[5] = "";
        line[6] = "";

        aaData.Add(line);
        aoData.Add(new JsonPermissionClass {Id = pc.Id, Name = pc.Name, Description = pc.Description});
      }
    }

    public class JsonPermissionClass
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
    }
  }
}