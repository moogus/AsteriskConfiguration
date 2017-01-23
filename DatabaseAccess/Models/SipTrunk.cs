﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using DatabaseAccess.ModelUtilities.TrunkManager;

namespace DatabaseAccess.Models
{
  internal class SipTrunk : ISipTrunk
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComTrunk _under;
    private ComSipCredentials _underSipCredentials;
    private readonly ITrunkManager _trunkManager;
    private List<ComAccessCode> _underAccessCodes;

    internal SipTrunk(ComTrunk under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
      _trunkManager = new TrunkManager(_session, _repository);
      _underAccessCodes = _session.Query<ComAccessCode>().Where(a => a.TrunkId == _under.Id).ToList();
      GetOtherValues();
    }

    internal SipTrunk(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _trunkManager = new TrunkManager(_session, _repository);
      _under = new ComTrunk();
      _underAccessCodes = new List<ComAccessCode>();
      GetOtherValues();
    }

    private void GetOtherValues()
    {
      _underSipCredentials = _under.Id != 0
                               ? _session.Query<ComSipCredentials>().FirstOrDefault(d => d.TrunkId == _under.Id) ??
                                 new ComSipCredentials()
                               : new ComSipCredentials();

    }

    public int Id { get { return _under.Id; } }
    public string Name { get { return _under.Name; } set { _under.Name = value; } }

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

    public List<AccessCodeAndPriority> AccessCodes { get { return _trunkManager.GetAccessCodes(_underAccessCodes); } }

    public bool SetAccessCodes(string accessCodes)
    {
      _underAccessCodes = _trunkManager.SetAccessCodes(accessCodes, _under.Id);
      return (_underAccessCodes != null && _underAccessCodes.Any());
    }

    public string SipUserName { get { return _underSipCredentials.UserName; } set { _underSipCredentials.UserName = value; } }
    public string SipPassword { get { return _underSipCredentials.Password; } set { _underSipCredentials.Password = value; } }
    public string SipHost { get { return _underSipCredentials.Host; } set { _underSipCredentials.Host = value; } }
    public int SipAllowedChannles { get { return _underSipCredentials.AllowedChannels; } set { _underSipCredentials.AllowedChannels = value; } }

    public TrunkType TrunkType
    {
      get
      {
        if (string.IsNullOrEmpty(_under.Type))
        {
          return TrunkType.Sip;
        }
        return (TrunkType)Enum.Parse(typeof(TrunkType), _under.Type);
      }
      set { _under.Type = value.ToString(); }
    }


    public string DefaultDestination
    {
      get { return _under.DefaultDestination; }
      set
      {
        _trunkManager.CreateDefaults(value, _under.Id);
        _under.DefaultDestination = value;
      }
    }

    public string TrunkInPresentationValue1 { get { return _under.CLIPresentationValue1; } set { _under.CLIPresentationValue1 = value; } }
    public string TrunkInPresentationValue2 { get { return _under.CLIPresentationValue2; } set { _under.CLIPresentationValue2 = value; } }


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

    object IModel.Under { get { return _under; } }
    ISessionWrapper IModel.Session { get { return _session; } }

    public void ExtraUpdate()
    {
      _trunkManager.RemoveAccessCodes(_under.Id);
      _trunkManager.UpdateAccessCodes(AccessCodes, _under.Id);

      _underSipCredentials.TrunkId = _under.Id;
      _session.SaveOrUpdate(_underSipCredentials);
    }

    public void ExtraDelete()
    {
      _trunkManager.RemoveAccessCodes(_under.Id);
      _trunkManager.ResetDdiUsedValue(_under.Id);
      RemoveSipCredentials();
    }

    private void RemoveSipCredentials()
    {
      if (_underSipCredentials.Id != 0)
        _session.Delete(_underSipCredentials);
    }
  }
}
