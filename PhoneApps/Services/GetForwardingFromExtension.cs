using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;
using PhoneApps.Services.Interfaces;

namespace PhoneApps.Services
{
  public class GetForwardingFromExtension : IGetForwardingFromExtension
  {
    private readonly IRepository _repository;
    public IEnumerable<IRoutingRule> ForwardingRules { get; private set; }
    public GetForwardingFromExtension(IRepository repository)
    {
      _repository = repository;
    }

    public void SetBaseForwarding(string extension)
    {
      var ru = _repository.GetList<IRoutingRule>().Where(r => r.Number.Equals(extension));
      var rules = ru.Where(r => r.Dialplan.Name.StartsWith("unconditional") || 
                                r.Dialplan.Name.StartsWith("onBusy") || 
                                r.Dialplan.Name.StartsWith("noAnwser")).ToList();

      var rtn = new List<IRoutingRule>();
      if (rules.Count() < 3)
      {
        if (!rules.Exists(r => r.Dialplan.Name.StartsWith("unconditional")))
        {
          rtn.Add(CreatePersonalRule(extension, "unconditionalOff"));
        }
        if (!rules.Exists(r => r.Dialplan.Name.StartsWith("onBusy")))
        {
          rtn.Add(CreatePersonalRule(extension, "onBusyOff"));
        }
        if (!rules.Exists(r => r.Dialplan.Name.StartsWith("noAnwser")))
        {
          rtn.Add(CreatePersonalRule(extension, "noAnwserOff"));
        }
        rtn.AddRange(rules);
        ForwardingRules = rtn;
      }
      else
      {
        ForwardingRules = rules;
      }
    }

    private IRoutingRule CreatePersonalRule(string extn, string dialPlan)
    {
      var rule = _repository.Add<IRoutingRule>();
      rule.Dialplan = _repository.GetFromName<IDialplan>(dialPlan);
      rule.Number = extn;
      return rule;
    }

    public string GetCurrentForwardingDialplan(string dialPlanName)
    {
      return _repository.GetList<IDialplan>().First(d => d.Name.StartsWith(dialPlanName)).Name;
    }

    public ForwardingDestination GetForwardingType(string extension, string dialPlan)
    {
      var allDialplansForextension = _repository.GetList<IRoutingRule>().Where(r => r.Dialplan.Name.StartsWith(dialPlan)).Where(r=>r.Number.Equals(extension)).ToList();
      
      if (!allDialplansForextension.Any())
      {
        return ForwardingDestination.Error;
      }

     return (ForwardingDestination)Enum.Parse(typeof(ForwardingDestination),allDialplansForextension.First().DestinationType.ToString());
     
    }

    public string GetForwardingNumber(string extension, string dialPlan)
    {
      var firstOrDefault = _repository.GetList<IRoutingRule>().FirstOrDefault(r => r.Number.Equals(extension) && r.Dialplan.Name.StartsWith(dialPlan));

      return firstOrDefault != null ? firstOrDefault.DestinationNumber : "";
    }

    public bool IsForwardingEnabled(string extension, string dialPlan)
    {
      var isRule =  _repository.GetList<IRoutingRule>().Where(r => r.Number.Equals(extension) &&
                                                            r.Dialplan.Name.StartsWith(dialPlan)).ToList();

      return isRule.Any() && isRule.First().Dialplan.Name.EndsWith("On");
    }
  }
}