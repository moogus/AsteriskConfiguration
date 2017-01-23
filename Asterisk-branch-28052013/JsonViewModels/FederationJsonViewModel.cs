using System.Collections.Generic;
using System.Globalization;
using Asterisk.Utilities.Interfaces;
using DatabaseAccess;

namespace Asterisk.JsonViewModels
{
  public class FederationJsonViewModel
  {
    public List<string[]> aaData;

    private readonly ITrunkValueGenerator _trunkValueGenerator;

    public FederationJsonViewModel(IEnumerable<IFederation> federationData, ITrunkValueGenerator trunkValueGenerator)
    {
      _trunkValueGenerator = trunkValueGenerator;
      aaData = new List<string[]>();

      foreach (var f in federationData)
      {
        _trunkValueGenerator.SetTrunk(f.Trunk);

        var line = new string[8];
        line[0] = f.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = f.Name;
        line[2] = f.Description;

        switch (f.Type)
        {
          case FederationType.FourCom:
            line[3] = "4Com";
            break;

          case FederationType.Samsung:
            line[3] = "Samsung";
            break;
        }

        line[4] = GetAccesCode(_trunkValueGenerator.GetAccessCodeString());
        line[5] = _trunkValueGenerator.GetCredentialString().Substring(4);
        line[6] = "";
        line[7] = "";

        aaData.Add(line);
      }
    }

    private static string GetAccesCode(string code)
    {
      return code.Split(':')[0];
    }
  }
}