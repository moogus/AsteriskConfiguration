using System.Collections.Generic;
using System.Linq;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class BriTrunk : IBriTrunk
  {
    private readonly IRepositoryWithDelete _modelRepository;
    private readonly ITrunk _baseTrunk;
    private List<IDahdiChannel> _dahdiChannels;

    public BriTrunk(ITrunk baseTrunk, IRepositoryWithDelete modelRepository)
    {
      _modelRepository = modelRepository;
      _baseTrunk = baseTrunk;
    }

    public int Id
    {
      get { return _baseTrunk.Id; }
    }

    public string Name
    {
      get { return _baseTrunk.Name; }
      set { _baseTrunk.Name = value; }
    }

    public string TrunkInPresentationValue1
    {
      get { return _baseTrunk.TrunkInPresentationValue1; }
      set { _baseTrunk.TrunkInPresentationValue1 = value; }
    }

    public string TrunkInPresentationValue2
    {
      get { return _baseTrunk.TrunkInPresentationValue2; }
      set { _baseTrunk.TrunkInPresentationValue2 = value; }
    }

    public TrunkType TrunkType
    {
      get { return _baseTrunk.TrunkType; }
      set { _baseTrunk.TrunkType = value; }
    }

    public string DefaultDestination
    {
      get { return _baseTrunk.DefaultDestination; }
      set { _baseTrunk.DefaultDestination = value; }
    }

    public TrunkInPresentationType TrunkInPresentationType1
    {
      get { return _baseTrunk.TrunkInPresentationType1; }
      set { _baseTrunk.TrunkInPresentationType1 = value; }
    }

    public TrunkInPresentationType TrunkInPresentationType2
    {
      get { return _baseTrunk.TrunkInPresentationType2; }
      set { _baseTrunk.TrunkInPresentationType2 = value; }
    }

    public IEnumerable<IDDI> DDIs
    {
      get { return _baseTrunk.DDIs; }
    }

    public List<IAccessCode> AccessCodes
    {
      get { return _baseTrunk.AccessCodes; }
    }

    public bool TrunkHasIAccessCode(string code, int priority)
    {
      return _baseTrunk.TrunkHasIAccessCode(code, priority);
    }

    public void AddAccessCodes(IAccessCode accessCode)
    {
      _baseTrunk.AddAccessCodes(accessCode);
    }
   
    public IEnumerable<string> DahdiChannels
    {
      get { return LazyDahdiChannels.Select(d => d.ChannelName); }
      set { SetDahdiChannels(value); }
    }

      public void SetDahdiChannels(string channels)
      {
          List<string> csl = channels.Split(',').ToList();
          csl.Remove("Bri");

          if (LazyDahdiChannels.Count == 0)
          {
              _dahdiChannels = new List<IDahdiChannel>();
          }

          foreach (var ch in csl)
          {
              _dahdiChannels.Add(CreateDahdiChannel(ch));
          }
      }

      private void SetDahdiChannels(IEnumerable<string> value)
    {
      if (!DeleteAllDahdiChannels()) return;

      foreach (var ch in value)
      {
        _dahdiChannels.Add(CreateDahdiChannel(ch));
      }
    }

    public void Delete()
    {
      _baseTrunk.Delete();
    }

    private List<IDahdiChannel> LazyDahdiChannels
    {
      get
      {
        _dahdiChannels = _dahdiChannels ??
                         _modelRepository.GetList<IDahdiChannel>().Where(a => a.ParentTrunk.Id == _baseTrunk.Id).ToList();
        return _dahdiChannels;
      }
    }

    private IDahdiChannel CreateDahdiChannel(string channelName)
    {
      var dChannel = _modelRepository.Add<IDahdiChannel>();
      dChannel.ParentTrunk = _baseTrunk;
      dChannel.ChannelName = channelName;
      return dChannel;
    }

    private bool DeleteAllDahdiChannels()
    {
      if (LazyDahdiChannels == null || LazyDahdiChannels.Count == 0) return false ;
      foreach (var dch in _dahdiChannels)
       dch.Delete();

      return true;
    }
  }
}