using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using DatabaseAccess.ModelUtilities.TrunkManager;

namespace DatabaseAccess.Models
{
  internal class Trunk : ITrunk
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComTrunk _under;
    private readonly ITrunkManager _trunkManager;
    private List<ComAccessCode> _underAccessCodes;

    internal Trunk(ComTrunk under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
      _underAccessCodes = _session.Query<ComAccessCode>().Where(a => a.TrunkId == _under.Id).ToList();
      _trunkManager = new TrunkManager(_session, _repository);
    }

    internal Trunk(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _underAccessCodes = new List<ComAccessCode>();
      _trunkManager = new TrunkManager(_session, _repository);
      _under = new ComTrunk();
    }

    public int Id { get { return _under.Id; } }
    public string Name { get { return _under.Name; } set { _under.Name = value; } }
    public string TrunkInPresentationValue1 { get { return _under.CLIPresentationValue1; } set { _under.CLIPresentationValue1 = value; } }
    public string TrunkInPresentationValue2 { get { return _under.CLIPresentationValue2; } set { _under.CLIPresentationValue2 = value; } }
    public TrunkType TrunkType { get { return (TrunkType)Enum.Parse(typeof(TrunkType), _under.Type); } set { _under.Type = value.ToString(); } }

    private List<IDDI> _ddis;
    public IEnumerable<IDDI> DDIs
    {
      get
      {
        var list = _repository.GetList<IDDI>().Where(d => d.Trunk.Id == _under.Id).ToList();
        if (!list.Any())
        {
          return new List<IDDI>();
        }
        _ddis = list.Where(a => a.Trunk.Id == _under.Id).ToList();
        return _ddis;
      }
    }

    public string DefaultDestination
    {
      get { return _under.DefaultDestination; }
      set
      {
        _trunkManager.CreateDefaults(value, Id);
        _under.DefaultDestination = value;
      }
    }

    public TrunkInPresentationType TrunkInPresentationType1
    {
      get
      {
        if (string.IsNullOrEmpty(_under.CLIPresentationType1))
        {
          return TrunkInPresentationType.None;
        }
        return (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), _under.CLIPresentationType1);
      }
      set
      {
        _under.CLIPresentationType1 = value.ToString();
      }
    }
    public TrunkInPresentationType TrunkInPresentationType2
    {
      get
      {
        if (string.IsNullOrEmpty(_under.CLIPresentationType2))
        {
          return TrunkInPresentationType.None;
        }
        return (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), _under.CLIPresentationType2);
      }
      set
      {
        _under.CLIPresentationType2 = value.ToString();
      }
    }

    public List<AccessCodeAndPriority> AccessCodes
    {
      get { return _trunkManager.GetAccessCodes(_underAccessCodes); }
    }

    public bool SetAccessCodes(string accessCodes)
    {
      _underAccessCodes = _trunkManager.SetAccessCodes(accessCodes, _under.Id);
      return (_underAccessCodes != null && _underAccessCodes.Any());
    }

    object IModel.Under { get { return _under; } }
    ISessionWrapper IModel.Session { get { return _session; } }

    public void ExtraUpdate()
    {
      _trunkManager.RemoveAccessCodes(_under.Id);
      _trunkManager.UpdateAccessCodes(AccessCodes, _under.Id);
    }

    public void ExtraDelete()
    {
      _trunkManager.RemoveAccessCodes(_under.Id);
      _trunkManager.ResetDdiUsedValue(_under.Id);
    }
  }
}
