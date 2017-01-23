using System.Collections.Generic;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Trunk.Interfaces
{
  public interface ITrunkHelper
  {
    TrunkType GetTrunkType(string info);
    IEnumerable<ITrunk> GetValidTrunks();
    ITrunk SetSipTrunkValues(string info);
    ITrunk GetSipTrunkValues(string info, int id);
    ITrunk SetBriTrunkValues(string info);
    ITrunk GetBriTrunkValues(string info, int id);
    ITrunk SetIaxTrunkValues(string info);
    ITrunk GetIaxTrunkValues(string info, int id);
  }
}