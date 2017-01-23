using System.Collections.Generic;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class IaxTrunk : IIaxTrunk
  {
    private readonly IFuIaxCredentials _underIaxCred;
    private readonly IRepositoryWithDelete _modelRepository;
    private readonly ITrunk _baseTrunk;

    public IaxTrunk(ITrunk trunkId, IFuIaxCredentials iaxCred, IRepositoryWithDelete modelRepository)
    {
      _underIaxCred = iaxCred;
      _modelRepository = modelRepository;
      _baseTrunk = trunkId;
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

    public string IaxName
    {
      get { return _underIaxCred.FuIaxCredentialName; }
      set { _underIaxCred.FuIaxCredentialName = value; }
    }

    public string HostAddress
    {
      get { return _underIaxCred.Host; }
      set { _underIaxCred.Host = value; }
    }

    public int IaxAllowedChannles
    {
      get { return _underIaxCred.AllowedChannels; }
      set { _underIaxCred.AllowedChannels = value; }
    }

    public void Delete()
    {
      _baseTrunk.Delete();
      _modelRepository.Delete(_underIaxCred);
    }
  }
}