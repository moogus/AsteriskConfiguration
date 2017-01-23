using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    internal class AutoAttendantRules : IAutoAttendantRules, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuAutoAttendantRules _under;

    internal AutoAttendantRules(FuAutoAttendantRules under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal AutoAttendantRules(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuAutoAttendantRules();
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string AaName
    {
      get { return _under.AaName; }
      set { _under.AaName = value; }
    }

    public string Entry
    {
      get { return _under.Entry; }
      set { _under.Entry = value; }
    }

    public string DestinationNumber
    {
      get { return _under.Destination; }
      set { _under.Destination = value; }
    }

    public RoutingRuleDestination DestinationType
    {
      get { return (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination) ,_under.DestinationType); }
      set {  _under.DestinationType = value.ToString(); }
    }

    #region IAutoAttendant Members

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

  }
}
