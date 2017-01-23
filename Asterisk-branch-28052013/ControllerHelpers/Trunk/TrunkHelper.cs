using System;
using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Trunk.Interfaces;
using DatabaseAccess;

namespace Asterisk.ControllerHelpers.Trunk
{
  public class TrunkHelper : ITrunkHelper
  {
    private readonly IRepository _repository;

    public TrunkHelper(IRepository repository)
    {
      _repository = repository;
    }

    public TrunkType GetTrunkType(string info)
    {
      var type = info.Substring(0, 3);
      return (type.Equals("Sip") || type.Equals("Bri") || type.Equals("Iax"))
               ? (TrunkType) Enum.Parse(typeof (TrunkType), type)
               : TrunkType.Sip;
    }

    public IEnumerable<ITrunk> GetValidTrunks()
    {
      var rtnTrunk = new List<ITrunk>();
      var federatedTrunks = _repository.GetList<IFederation>().Where(f => f.Trunk != null).Select(f => f.Trunk).ToList();

      var sip = _repository.GetList<ISipTrunk>().Where(s => s.TrunkType == TrunkType.Sip).ToList();
      var bri = _repository.GetList<IBriTrunk>().Where(b => b.TrunkType == TrunkType.Bri).ToList();
      var iax = _repository.GetList<IIaxTrunk>().Where(i => i.TrunkType == TrunkType.Iax).ToList();

      rtnTrunk.AddRange(sip);
      rtnTrunk.AddRange(bri);
      rtnTrunk.AddRange(iax);

      var sumList = sip.Select(s => s.Id).Union(bri.Select(b => b.Id).Union(iax.Select(i => i.Id)));

      if (!federatedTrunks.Any())
      {
        return rtnTrunk;
      }

      var fedIds = federatedTrunks.Select(t => t.Id);
      var trunksIds = GetTrunkData(sumList, fedIds).ToList();

      return (from trunk in rtnTrunk from ids in trunksIds where trunk.Id == ids select trunk).ToList();
    }

    private static IEnumerable<int> GetTrunkData(IEnumerable<int> trunkData, IEnumerable<int> federatedTrunks)
    {
      return trunkData.Except(federatedTrunks);
    }

    public ITrunk SetSipTrunkValues(string info)
    {
      var trunk = _repository.Add<ISipTrunk>();
      var credentials = info.Split(',').ToList();
      trunk.SipUserName = credentials[1].Trim();
      trunk.SipPassword = credentials[2].Trim();
      trunk.SipHost = credentials[3].Trim();
      trunk.SipAllowedChannles = int.Parse(credentials[4].Trim());
      trunk.TrunkInPresentationType1 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[5].Trim());
      trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
      trunk.TrunkInPresentationType2 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[7].Trim());
      trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
      return trunk;
    }

    public ITrunk GetSipTrunkValues(string info, int id)
    {
      var trunk = _repository.GetFromId<ISipTrunk>(id);
      var credentials = info.Split(',').ToList();
      trunk.SipUserName = credentials[1].Trim();
      trunk.SipPassword = credentials[2].Trim();
      trunk.SipHost = credentials[3].Trim();
      trunk.SipAllowedChannles = int.Parse(credentials[4].Trim());
      trunk.TrunkInPresentationType1 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[5].Trim());
      trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
      trunk.TrunkInPresentationType2 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[7].Trim());
      trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
      return trunk;
    }

    public ITrunk SetBriTrunkValues(string info)
    {
      var trunk = _repository.Add<IBriTrunk>();
      var csl = info.Split(',').ToList();
      csl.Remove("Bri");
      trunk.DahdiChannels = csl.Select(s => s.Trim()).ToList();
      return trunk;
    }

    public ITrunk GetBriTrunkValues(string info, int id)
    {
      var trunk = _repository.GetFromId<IBriTrunk>(id);
      var csl = info.Split(',').ToList();
      csl.Remove("Bri");
      trunk.DahdiChannels = csl.Select(s => s.Trim()).ToList();
      return trunk;
    }

    public ITrunk SetIaxTrunkValues(string info)
    {
      var trunk = _repository.Add<IIaxTrunk>();
      var credentials = info.Split(',').ToList();
      trunk.IaxName = credentials[1];
      trunk.HostAddress = credentials[2];
      trunk.IaxAllowedChannles = int.Parse(credentials[4].Trim());
      trunk.TrunkInPresentationType1 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[5].Trim());
      trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
      trunk.TrunkInPresentationType2 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[7].Trim());
      trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
      return trunk;
    }

    public ITrunk GetIaxTrunkValues(string info, int id)
    {
      var trunk = _repository.GetFromId<IIaxTrunk>(id);
      var credentials = info.Split(',').ToList();
      trunk.IaxName = credentials[1];
      trunk.HostAddress = credentials[3];
      trunk.IaxAllowedChannles = int.Parse(credentials[4].Trim());
      trunk.TrunkInPresentationType1 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[5].Trim());
      trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
      trunk.TrunkInPresentationType2 =
        (TrunkInPresentationType) Enum.Parse(typeof (TrunkInPresentationType), credentials[7].Trim());
      trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
      return trunk;
    }
  }
}