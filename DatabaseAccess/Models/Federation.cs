using System;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class Federation : IFederation, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuFederation _under;
    private readonly IExtension _extension;
    private readonly IRoutingRule _routingRule;
    private ITrunk _trunk;

    internal Federation(FuFederation under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
      _trunk = _repository.GetFromId<ITrunk>(_under.ComTrunkId);
      _extension = _repository.GetFromId<IExtension>(_under.ComExtensionId);
      _routingRule = _repository.GetFromId<IRoutingRule>(_under.ComRoutingRuleId);
    }

    internal Federation(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuFederation();
      _extension = _repository.Add<IExtension>();
      _routingRule = _repository.Add<IRoutingRule>();
    }

    public int Id { get { return _under.Id; } }
    public string Name { get { return _under.Name; } set { _under.Name = value; } }

    public FederationType Type
    {
      get
      {
        return (FederationType)Enum.Parse(typeof(FederationType), _under.Type);
      }
      set
      {
        _under.Type = value.ToString();
      }
    }

    public string Description { get { return _under.Description; } set { _under.Description = value; } }
    public IExtension Extension { get { return _extension; } }
    public ITrunk Trunk { get { return _trunk; } }
    public IRoutingRule RoutingRule { get { return _routingRule; } }

    public bool SetFederationValues(string name, string accessCode, string password,ITrunk trunk)
    {
      if (string.IsNullOrEmpty(name) ||string.IsNullOrEmpty(accessCode) || string.IsNullOrEmpty(password)) return false;
     
      _under.ComTrunkId = trunk.Id;
      _trunk = trunk;
      switch (Type)
      {
        case FederationType.Samsung:
          var rtn = SetExtensionValues(name, password) && SetRoutingRuleValues(accessCode.Split(':')[0]);
          _under.ComExtensionId = _extension.Id;
          _under.ComRoutingRuleId = _routingRule.Id;
          return rtn;
        case FederationType.FourCom:
          return true;
        default:
          return false;
      }     
    }

    #region IDDI Members

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
    }

    #endregion

    private bool SetExtensionValues(string name, string password)
    {
      _extension.Number = name;
      _extension.Password = password;
      return _extension.Update();
    }

    //todo sort this out currently blanket sets a rule on the samsung
    private bool SetRoutingRuleValues(string accessCode)
    {
      _routingRule.DestinationNumber = accessCode;
      _routingRule.DestinationType = RoutingRuleDestination.AddCode;
      _routingRule.Order = 0;
      _routingRule.Time = 0;
      _routingRule.Number = "Any";
      _routingRule.Dialplan = _repository.GetFromName<IDialplan>("fallThrough");
      return _routingRule.Update();
    }
  }
}