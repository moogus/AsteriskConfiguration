using System.Collections.Generic;
using System.Globalization;
using Asterisk.Utilities.Interfaces;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
  public class TrunkJsonViewModel
  {
    private readonly ITrunkValueGenerator _trunkValueGenerator;
    public List<string[]> aaData;

    public TrunkJsonViewModel(IEnumerable<ITrunk> trunkData, ITrunkValueGenerator trunkValueGenerator)
    {
      _trunkValueGenerator = trunkValueGenerator;
      aaData = new List<string[]>();

      foreach (ITrunk trunk in trunkData)
      {
        
        _trunkValueGenerator.SetTrunk(trunk);
        var line = new string[7];
        line[0] = trunk.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = trunk.Name;
        line[2] = _trunkValueGenerator.GetAccessCodeString();
        line[3] = _trunkValueGenerator.GetCredentialString();
        line[4] = trunk.DefaultDestination;
        line[5] = "";
        line[6] = "";
        aaData.Add(line);
      }
    }
  }
}