using System.Collections.Generic;
using System.Linq;
using Asterisk.Utilities.Interfaces;
using DatabaseAccess;

namespace Asterisk.Utilities
{
  public class TrunkValueGenerator : ITrunkValueGenerator
  {
    private ITrunk _trunk;
    private readonly IRepository _repository;

    public TrunkValueGenerator(IRepository repository)
    {
      _repository = repository;
    }

    public void SetTrunk(ITrunk trunk)
    {
      _trunk = trunk;
    }

    public string GetCredentialString()
    {
      if (_trunk == null) return string.Empty;
      var rtn = string.Empty;
      switch (_trunk.TrunkType)
      {
        case TrunkType.Sip:
          var sipTrunk = _repository.GetFromId<ISipTrunk>(_trunk.Id);
          rtn = string.Format("Sip,{0},{1},{2},{3},{4},{5},{6},{7}", sipTrunk.SipUserName, sipTrunk.SipPassword,
                              sipTrunk.SipHost, sipTrunk.SipAllowedChannles, sipTrunk.TrunkInPresentationType1,
                              sipTrunk.TrunkInPresentationValue1, sipTrunk.TrunkInPresentationType2,
                              sipTrunk.TrunkInPresentationValue2);
          break;
        case TrunkType.Bri:
          var briTrunk = _repository.GetFromId<IBriTrunk>(_trunk.Id);
          rtn = GetChannelString(briTrunk);
          break;
        case TrunkType.Iax:
          var iaxTrunk = _repository.GetFromId<IIaxTrunk>(_trunk.Id);
          rtn = string.Format("Iax,{0},None Required,{1},{2},{3},{4},{5},{6}", iaxTrunk.IaxName, iaxTrunk.HostAddress,
                              iaxTrunk.IaxAllowedChannles, iaxTrunk.TrunkInPresentationType1,
                              iaxTrunk.TrunkInPresentationValue1, iaxTrunk.TrunkInPresentationType2,
                              iaxTrunk.TrunkInPresentationValue2);
          break;
      }
      return rtn;
    }

    private string GetChannelString(IBriTrunk briTrunk)
    {
      var rtn = string.Empty;

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

    public string GetAccessCodeString()
    {
      if (_trunk == null) return string.Empty;

      return _trunk.AccessCodes.Aggregate(string.Empty, (current, c) => string.IsNullOrEmpty(current)
                                                                          ? string.Format("{0}:{1}", c.AccessCode,
                                                                                          c.Priority)
                                                                          : string.Format("{0},{1}:{2}", current,
                                                                                          c.AccessCode, c.Priority));
    }
  }
}