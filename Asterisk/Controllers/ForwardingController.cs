using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize]
    public class ForwardingController : Controller
    {
        private readonly IRepository _modelRepository;

        public ForwardingController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string extn, bool isAdminEdit)
        {
            var fVm = new ForwardingViewModel
              {
                  ThisExtension = _modelRepository.GetFromName<IExtension>(extn),
                  PersonalRules = SetBaseRules(extn)
              };

            return View(fVm);
        }

        public string Update(string extension, string enabled, string destination, string dialplan)
        {
            if (destination.Equals("Not Set") || string.IsNullOrEmpty(destination)) return "";

            var message = dialplan.StartsWith("uncon")
                               ? " unconditional"
                               : dialplan.StartsWith("onB") ? " when busy" : " no anwser";
            
            return UpdateForwarding(extension, enabled, destination, dialplan)
                ? string.Format("...updated{0} set {1}", message, enabled.ToLower())
                : "";            
        }

        private IEnumerable<IRoutingRule> SetBaseRules(string extn)
        {
            var ru = _modelRepository.GetList<IRoutingRule>().Where(r => r.Number == extn);

            var rules = ru.Where(
                r =>
                r.Dialplan.Name.StartsWith("unconditional") || r.Dialplan.Name.StartsWith("onBusy") ||
                r.Dialplan.Name.StartsWith("noAnwser")).ToList();

            var rtn = new List<IRoutingRule>();

            if (rules.Count() < 3)
            {
                if (!rules.Exists(r => r.Dialplan.Name.StartsWith("unconditional")))
                {
                    rtn.Add(CreatePersonalRule(extn, "unconditionalOff"));
                }

                if (!rules.Exists(r => r.Dialplan.Name.StartsWith("onBusy")))
                {
                    rtn.Add(CreatePersonalRule(extn, "onBusyOff"));
                }

                if (!rules.Exists(r => r.Dialplan.Name.StartsWith("noAnwser")))
                {
                    rtn.Add(CreatePersonalRule(extn, "noAnwserOff"));
                }

                rtn.AddRange(rules);

                return rtn;
            }

            return rules;
        }

        private IRoutingRule CreatePersonalRule(string extn, string dialPlan)
        {
            var rule = _modelRepository.Add<IRoutingRule>();

            rule.Dialplan = _modelRepository.GetFromName<IDialplan>(dialPlan);
            rule.Number = extn;

            return rule;
        }

        private bool UpdateForwarding(string extension, string enabled, string destination, string dialplan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var destinationData = GetDestinationData(destination);

                if (destinationData.Count != 2)
                {
                    return false;
                }

                var rule = GetRule(extension, dialplan);

                rule.Order = 1;
                dialplan = string.Format(enabled.Equals("Enabled") ? "{0}On" : "{0}Off", dialplan);
                rule.Dialplan = _modelRepository.GetFromName<IDialplan>(dialplan);
                rule.Number = extension;
                rule.DestinationType = (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), destinationData[0]);
                rule.DestinationNumber = destinationData[1].Trim();
                rule.Time = 0;

                return transaction.Commit();
            }
        }

        private IRoutingRule GetRule(string extension, string dialplan)
        {
            var rules = _modelRepository.GetList<IRoutingRule>()
                .Where(r => r.Number == extension && r.Dialplan.Name.StartsWith(dialplan))
                .ToList();

            return rules.Count < 1 ? _modelRepository.Add<IRoutingRule>() : rules.First();
        }

        private static List<string> GetDestinationData(string destination)
        {
            return destination.Split(',').ToList();
        }
    }
}