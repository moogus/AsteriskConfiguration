using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using NHibernate;

namespace DatabaseAccess.Models
{

  public class RoutingRule : IRoutingRule
  {
    private readonly AstExtension _under;
    private readonly ISessionWrapper _session;
    private readonly Repository _repository;

    public RoutingRule(AstExtension under, ISessionWrapper session, Repository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;

      if (!string.IsNullOrEmpty(_under.Context))
      {
        var callplanId = _under.Context.Equals("LocalSets") ? 8 : int.Parse(_under.Context.Substring(8));
        if (callplanId > 0)
        {
          var context = new Repository();
          _dialplan = context.GetFromId<IDialplan>(callplanId);
        }
      }
      switch (DestinationType)
      {
        case RoutingRuleDestination.Extension:
          var timeSplit = _under.Appdata.Split(',');
          _time = timeSplit.Length > 1 ? int.Parse(timeSplit[1]) : 0;
          break;
        case RoutingRuleDestination.External:
          var timeSplit3 = _under.Appdata.Split(',');
          _time = timeSplit3.Length > 1 ? int.Parse(timeSplit3[1]) : 0;
          break;
        case RoutingRuleDestination.Route:
          var timeSplit4 = _under.Appdata.Split(',');
          _time = timeSplit4.Length > 1 ? int.Parse(timeSplit4[1]) : 0;
          break;
        case RoutingRuleDestination.Group:
          var timeSplit2 = _under.Appdata.Split(',');
          _time = timeSplit2.Length > 2 ? int.Parse(timeSplit2[2]) : 0;
          break;
        case RoutingRuleDestination.Voicemail:
          _time = 0;
          break;
        case RoutingRuleDestination.Ringtone:
          _time = 0;
          break;
        case RoutingRuleDestination.AutoAttendant:
          _time = 0;
          break;
        case RoutingRuleDestination.Playback:
          _time = 0;
          break;
      }
    }
    public RoutingRule(ISessionWrapper session, Repository repository)
    {
      _under = new AstExtension { App = "", Appdata = "", DestinationNumber = "", DestinationType = "" };

      _session = session;
      _repository = repository;
    }

    public RoutingRule()
    {
      // TODO: Complete member initialization
    }

    #region Implementation of IModel
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
      switch (DestinationType)
      {
        case RoutingRuleDestination.Extension:
          _under.App = "Dial";
          _under.Appdata = "SIP/" + DestinationNumber;
          if (_time > 0)
            _under.Appdata += "," + Time;
          break;
        case RoutingRuleDestination.Group:
          _under.App = "Macro";
          _under.Appdata = "simplequeuenohang," + DestinationNumber;
          if (_time > 0)
            _under.Appdata += "," + Time;
          break;
        case RoutingRuleDestination.Voicemail:
          _under.App = "Voicemail";
          _under.Appdata = DestinationNumber;
          break;
        case RoutingRuleDestination.External:
          _under.App = "Dial";
          _under.Appdata = "local/" + DestinationNumber + "@Outgoing";
          if (_time > 0)
            _under.Appdata += "," + Time;
          break;
        case RoutingRuleDestination.Ringtone:
          _under.App = "SIPAddHeader";
          var ringtone = _repository.GetFromName<IRingTone>(DestinationNumber);
          _under.Appdata = ringtone.SipHeader;
          break;
        case RoutingRuleDestination.Route:
          _under.App = "Dial";
          _under.Appdata = "local/" + DestinationNumber + "@CallRouting";
          if (_time > 0)
            _under.Appdata += "," + Time;
          break;
        case RoutingRuleDestination.AutoAttendant:
          _under.App = "Macro";
          _under.Appdata = "AA," + DestinationNumber;
          break;
        case RoutingRuleDestination.Playback:
          _under.App = "Playback";
          _under.Appdata = DestinationNumber;
          break;
      }

      // set the priority on this rule to really, really high so we dont get abny conflicts
      // don't worry, it will be set back to what it should be in a couple of lines time!
      _under.Priority = 10003;
      _session.SaveOrUpdate(_under);

      // update priority on all routes for this ExtensionNumber and callplan
      var routes = _session.Query<AstExtension>().Where(c => c.Context == _under.Context && c.ExtensionNumber == _under.ExtensionNumber).OrderBy(c => c.Order);
      var counter = 1;
      foreach (var route in routes)
      {
        route.Priority = counter++;
      }
      foreach (var route in routes)
      {
        _session.SaveOrUpdate(route);
      }
    }

    public void ExtraDelete()
    {
    }

    #endregion

    #region Implementation of IRoutingRule

    public int Id { get { return _under.Id; } }

    private IDialplan _dialplan;

    public IDialplan Dialplan
    {
      get { return _dialplan; }
      set
      {
        _dialplan = value;

        if (_dialplan.Name.Equals("onBusyOn"))
        {
          _under.Context = "LocalSets";
          _under.ForwardingType = 1;
          _under.Priority = 2;
        }
        else
        {
          _under.Context = "CallPlan" + _dialplan.Id;
        }
        
      }
    }

    public string Number
    {
      get { return _under.ExtensionNumber; }
      set { _under.ExtensionNumber = value; }
    }
    private int _time;
    public int Time
    {
      get { return _time; }
      set { _time = value; }
    }

    public int Order
    {
      get { return _under.Order; }
      set { _under.Order = value; }
    }
    public RoutingRuleDestination DestinationType
    {
      get
      {
        RoutingRuleDestination enumout;
        return Enum.TryParse(_under.DestinationType, out enumout) ? enumout : RoutingRuleDestination.Error;
      }
      set { _under.DestinationType = value.ToString(); }
    }

    public string DestinationNumber
    {
      get { return _under.DestinationNumber; }
      set { _under.DestinationNumber = value; }
    }


    #endregion
  }
}
