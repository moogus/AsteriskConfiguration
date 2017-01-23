using System;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IRoutingRule : IModel
    {
        int Id { get; }
        IDialplan Dialplan { get; set; }
        string Number { get; set; }
        int Time { get; set; }
        int Order { get; set; }
        RoutingRuleDestination DestinationType { get; set; }
        string DestinationNumber { get; set; }
    }

    public enum RoutingRuleDestination
    {
        Error,
        Extension,
        Group,
        Voicemail,
        External
    }

    public class RoutingRule : IRoutingRule
    {
        private readonly IUnderlyingRoutingRule _underlyingRoutingRule;

        public RoutingRule(IUnderlyingRoutingRule underlyingRoutingRule)
        {

            _underlyingRoutingRule = underlyingRoutingRule;
            if (_underlyingRoutingRule != null)
            {
                if (!string.IsNullOrEmpty(_underlyingRoutingRule.Context))
                {
                    var callplanId = int.Parse(_underlyingRoutingRule.Context.Substring(8));
                    if (callplanId > 0)
                    {
                        var context = new Repository<IDialplan>();
                        _dialplan = context.GetFromId(callplanId);
                    }
                }
                switch (DestinationType)
                {
                    case RoutingRuleDestination.Extension:
                        var timeSplit = _underlyingRoutingRule.AppData.Split(',');
                        _time = timeSplit.Length > 1 ? int.Parse(timeSplit[1]) : 0;
                        break;
                    case RoutingRuleDestination.External:
                        var timeSplit3 = _underlyingRoutingRule.AppData.Split(',');
                        _time = timeSplit3.Length > 1 ? int.Parse(timeSplit3[1]) : 0;
                        break;
                    case RoutingRuleDestination.Group:
                        var timeSplit2 = _underlyingRoutingRule.AppData.Split(',');
                        _time = timeSplit2.Length > 2 ? int.Parse(timeSplit2[3]) : 0;
                        break;
                    case RoutingRuleDestination.Voicemail:
                        _time = 0;
                        break;
                }
            }
        }

        #region IRoutingRule Members

        public bool Update()
        {
            switch (DestinationType)
            {
                case RoutingRuleDestination.Extension:
                    _underlyingRoutingRule.App = "Dial";
                    _underlyingRoutingRule.AppData = "SIP/" + DestinationNumber;
                    if (_time > 0)
                        _underlyingRoutingRule.AppData += "," + Time;
                    break;
                case RoutingRuleDestination.Group:
                    _underlyingRoutingRule.App = "Queue";
                    _underlyingRoutingRule.AppData = DestinationNumber + "rt";
                    if (_time > 0)
                        _underlyingRoutingRule.AppData += ",,," + Time;
                    break;
                case RoutingRuleDestination.Voicemail:
                    _underlyingRoutingRule.App = "Voicemail";
                    _underlyingRoutingRule.AppData = DestinationNumber + "u";
                    break;
                case RoutingRuleDestination.External:
                    _underlyingRoutingRule.App = "Dial";
                    _underlyingRoutingRule.AppData = "local/" + DestinationNumber + "Outgoing";
                    if (_time > 0)
                        _underlyingRoutingRule.AppData += "," + Time;
                    break;
            }

            return _underlyingRoutingRule.Update();
        }

        public bool Delete()
        {
            return _underlyingRoutingRule.Delete();
        }

        public int Id
        {
            get { return _underlyingRoutingRule.Id; }
        }

        private IDialplan _dialplan;

        public IDialplan Dialplan
        {
            get { return _dialplan; }
            set
            {
                _dialplan = value;
                _underlyingRoutingRule.Context = "CallPlan" + _dialplan.Id;
            }
        }

        public string Number
        {
            get { return _underlyingRoutingRule.ExtensionNumber; }
            set { _underlyingRoutingRule.ExtensionNumber = value; }
        }

        private int _time;

        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public int Order
        {
            get { return _underlyingRoutingRule.Order; }
            set { _underlyingRoutingRule.Order = value; }
        }

        public RoutingRuleDestination DestinationType
        {
            get
            {
                RoutingRuleDestination enumout;
                return Enum.TryParse(_underlyingRoutingRule.DestinationType, out enumout) ? enumout : RoutingRuleDestination.Error;
            }
            set { _underlyingRoutingRule.DestinationType = value.ToString(); }
        }

        public string DestinationNumber { get { return _underlyingRoutingRule.DestinationNumber; } set { _underlyingRoutingRule.DestinationNumber = value; } }

        #endregion
    }

}
