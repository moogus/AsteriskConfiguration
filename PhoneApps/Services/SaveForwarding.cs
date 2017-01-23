using System;
using DatabaseAccess;
using PhoneApps.Services.Interfaces;
using System.Linq;

namespace PhoneApps.Services
{
  public class SaveForwarding : ISaveForwardingType
  {
    private string _forwardDestination;
    private bool _isEnabled;
    private string[] _listOfValues;
    private RoutingRuleDestination _ruleDestinationType;
    private Repository _repository;
    private string _myExtension;

    public void SaveForwardingType(string value)
    {
    //works on the basis we are only concerned with unconditional
       _listOfValues = value.Split(',');
       _isEnabled = _listOfValues[0] == "Enabled";
       _ruleDestinationType = (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), _listOfValues[1]);
       _forwardDestination = _listOfValues[2];
      _myExtension = _listOfValues[3];
       _repository = new Repository();
      SetForwardingRule();
   
    }

    private void SetForwardingRule()
    {
      var forwardingRule = _repository.GetList<IRoutingRule>().Where(r => r.Dialplan.Name.StartsWith("unconditional") && r.Number.Equals(_myExtension)).ToList();

      if (forwardingRule.Any())
      {
        UpdateRule(forwardingRule.First());
      }
      else
      {
        CreateRule();
      }
    }

    private void CreateRule()
    {
      var forwardingRule = _repository.Add<IRoutingRule>();
      forwardingRule.Number = _myExtension;
      forwardingRule.Order = 1;
      forwardingRule.Time = 0;
      forwardingRule.DestinationNumber = _forwardDestination;
      forwardingRule.DestinationType = _ruleDestinationType;
      forwardingRule.Dialplan = _repository.GetFromName<IDialplan>(_isEnabled ? "unconditionalOn" : "unconditionalOff");
      forwardingRule.Update();
    }

    private void UpdateRule(IRoutingRule forwardingRule)
    {
      forwardingRule.DestinationNumber = _forwardDestination;
      forwardingRule.DestinationType = _ruleDestinationType;
      forwardingRule.Dialplan = _repository.GetFromName<IDialplan>(_isEnabled ? "unconditionalOn" : "unconditionalOff");
      forwardingRule.Update();
    }
  }
}