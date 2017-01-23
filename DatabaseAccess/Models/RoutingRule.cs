using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using NHibernate;

namespace DatabaseAccess.Models
{

    internal class RoutingRule : IRoutingRule, IModel
  {
    private readonly ComRoutingRule _under;
    private readonly ISessionWrapper _session;
    private readonly Repository _repository;

    internal RoutingRule(ComRoutingRule under, ISessionWrapper session, Repository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }
    internal RoutingRule(ISessionWrapper session, Repository repository)
    {
      _session = session;
      _repository = repository;
      _under = new ComRoutingRule();
    }

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public int Id { get { return _under.Id; } }
    public string Number
    {
      get { return _under.Number; }
      set
      {
        if (IsDDI(value))
        {
          IDDI ddi;
          switch (IsDdiOnARoute(value))
          {
            case DDIUsedOn.Rule:
              if (string.IsNullOrEmpty(value))
              {
                ddi = _repository.GetFromName<IDDI>(_under.Number);
                ddi.UsedOn = string.IsNullOrEmpty(ddi.Trunk.DefaultDestination) ? DDIUsedOn.NotUsed : DDIUsedOn.Default;
                ddi.Update();
              }
              break;
            case DDIUsedOn.NotUsed:
              if (!string.IsNullOrEmpty(value))
              {
                ddi = _repository.GetFromName<IDDI>(value);
                ddi.UsedOn = DDIUsedOn.Rule;
                ddi.Update();
              }
              break;
            case DDIUsedOn.Default:
              if (!string.IsNullOrEmpty(value))
              {
                ddi = _repository.GetFromName<IDDI>(value);
                ddi.UsedOn = DDIUsedOn.Rule;
                ddi.Update();
              }
              break;
          }
        }

        _under.Number = value;
      }
    }

    private bool IsDDI(string value)
    {
      return _repository.GetFromName<IDDI>(value) != null;
    }

    private DDIUsedOn IsDdiOnARoute(string value)
    {
      return _repository.GetFromName<IDDI>(value).UsedOn;
    }

    public int Time { get { return _under.Time; } set { _under.Time = value; } }
    public int Order { get { return _under.Order; } set { _under.Order = value; } }
    public string DestinationNumber { get { return _under.DestinationNumber; } set { _under.DestinationNumber = value; } }
  
    public RoutingRuleDestination DestinationType
    {
      get
      {
        if(string.IsNullOrEmpty( _under.DestinationType))
        {
          return RoutingRuleDestination.Error;
        }
        return (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), _under.DestinationType);
      }
      set { _under.DestinationType = value.ToString(); }
    }

    public IDialplan Dialplan
    {
      get { return GetDialPlan(); }

      set { SetDialPlan(value); }
    }

    private IDialplan GetDialPlan()
    {
      return _repository.GetFromId<IDialplan>(_under.DialplanId);
    }

    private void SetDialPlan(IDialplan value)
    {
      _under.DialplanId = value == null ? 0 : value.Id;
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
      if (!IsDDI(Number) || IsDdiOnARoute(Number) != DDIUsedOn.Rule) return;
      //todo: remove dependancy...with NHibernate a another repository is need to do this (second session)
      var ddi = new Repository().GetFromName<IDDI>(Number);
      ddi.UsedOn = string.IsNullOrEmpty(ddi.Trunk.DefaultDestination) ? DDIUsedOn.NotUsed : DDIUsedOn.Default;
      ddi.Update();
    }

  }
}
