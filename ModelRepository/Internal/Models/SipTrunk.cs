using System.Collections.Generic;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class SipTrunk : ISipTrunk
  {
    private readonly ITrunk _baseTrunk;
    private readonly IComSipCredentials _underSipCredentials;
    private readonly IRepositoryWithDelete _modelRepository;

    public SipTrunk(ITrunk baseTrunk, IComSipCredentials sipCredentials, IRepositoryWithDelete modelRepository)
    {
      _underSipCredentials = sipCredentials;
      _modelRepository = modelRepository;
      _baseTrunk = baseTrunk;

      if (_underSipCredentials.TrunkId == 0)
      {
          _underSipCredentials.TrunkId = _baseTrunk.Id;
      }
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

    public string SipUserName
    {
      get { return _underSipCredentials.UserName; }
      set { _underSipCredentials.UserName = value; }
    }

    public string SipPassword
    {
      get { return _underSipCredentials.Password; }
      set { _underSipCredentials.Password = value; }
    }

    public string SipHost
    {
      get { return _underSipCredentials.Host; }
      set { _underSipCredentials.Host = value; }
    }

    public int SipAllowedChannles
    {
      get { return _underSipCredentials.AllowedChannels; }
      set { _underSipCredentials.AllowedChannels = value; }
    }

    public void Delete()
    {
      _baseTrunk.Delete();
      _modelRepository.Delete(_underSipCredentials);
    }
  }
}