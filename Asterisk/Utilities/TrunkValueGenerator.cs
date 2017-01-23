using System.Collections.Generic;
using System.Linq;
using Asterisk.Utilities.Interfaces;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Utilities
{
  public class TrunkValueGenerator : ITrunkValueGenerator
  {
    private readonly IRepository _modelRepository;
    private ITrunk _trunk;

    public TrunkValueGenerator(IRepository modelRepository)
    {
        _modelRepository = modelRepository;
    }

    public void SetTrunk(ITrunk trunk)
    {
        _trunk = trunk;
    }

    public string GetCredentialString()
    {
      if (_trunk == null) return string.Empty;
      string rtn = string.Empty;
      switch (_trunk.TrunkType)
      {
        case TrunkType.Sip:
          var sipTrunk = _modelRepository.GetFromId<ISipTrunk>(_trunk.Id);
          rtn = string.Format("Sip,{0},{1},{2},{3},{4},{5},{6},{7}", sipTrunk.SipUserName, sipTrunk.SipPassword,
                              sipTrunk.SipHost, sipTrunk.SipAllowedChannles, sipTrunk.TrunkInPresentationType1,
                              sipTrunk.TrunkInPresentationValue1, sipTrunk.TrunkInPresentationType2,
                              sipTrunk.TrunkInPresentationValue2);
          break;
        case TrunkType.Bri:
          var briTrunk = _modelRepository.GetFromId<IBriTrunk>(_trunk.Id);
          rtn = GetChannelString(briTrunk);
          break;
        case TrunkType.Iax:
          var iaxTrunk = _modelRepository.GetFromId<IIaxTrunk>(_trunk.Id);
          rtn = string.Format("Iax,{0},None Required,{1},{2},{3},{4},{5},{6}", iaxTrunk.IaxName, iaxTrunk.HostAddress,
                              iaxTrunk.IaxAllowedChannles, iaxTrunk.TrunkInPresentationType1,
                              iaxTrunk.TrunkInPresentationValue1, iaxTrunk.TrunkInPresentationType2,
                              iaxTrunk.TrunkInPresentationValue2);
          break;
      }
      return rtn;
    }

    public string GetAccessCodeString()
    {
      if (_trunk == null) return string.Empty;

      return _trunk.AccessCodes.Aggregate(string.Empty, (current, c) => string.IsNullOrEmpty(current)
                                                                          ? string.Format("{0}:{1}", c.Code,
                                                                                          c.Priority)
                                                                          : string.Format("{0},{1}:{2}", current,
                                                                                          c.Code, c.Priority));
    }

    private string GetChannelString(IBriTrunk briTrunk)
    {
      string rtn = string.Empty;

      var channelList = new List<string>
        {
          "1",
          "2",
          "4",
          "5",
          "7",
          "8",
          "10",
          "11"
        };

      return channelList.Where(c => briTrunk.DahdiChannels.Contains(c)).Aggregate(
        rtn, (current, c) => current == string.Empty
                               ? string.Format("Bri,{0}", c)
                               : string.Format("{0},{1}", current, c)
        );
    }
  }
}