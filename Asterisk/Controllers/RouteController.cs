using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class RouteController : Controller
    {
        private readonly IRepository _modelRepository;

        public RouteController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string dialplan)
        {
            var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);

            return View(new RouteControllerViewModel(dP, _modelRepository));
        }

        public string Add(string number, string d1, string d2, string d3, string d4, string d5, string dialplan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                AddRoutesFromDestinationStrings(number, new List<string> {d1, d2, d3, d4, d5}, int.Parse(dialplan));

                return transaction.Commit() ? "Added route." : "Failed to add route.";

            }
        }

        public string Update(string number, string d1, string d2, string d3, string d4, string d5, string dialplan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                RemoveAllRulesForNumber(int.Parse(dialplan), number);

                AddRoutesFromDestinationStrings(number, new List<string> {d1, d2, d3, d4, d5}, int.Parse(dialplan));

                return transaction.Commit() ? "Updated route." : "Failed to update route.";
            }
        }

        public string Delete(int dialplan, string number)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                RemoveAllRulesForNumber(dialplan, number);

                return transaction.Commit() ? "Deleted route." : "Failed to delete route.";
            }
        }

        public new JsonResult RouteData(int dialplan)
        {
            return Json(new RouteJsonViewModel(_modelRepository.GetFromId<IDialplan>(dialplan), _modelRepository), JsonRequestBehavior.AllowGet);
        }

        private void RemoveAllRulesForNumber(int dialplan, string number)
        {
            var rules = _modelRepository.GetList<IRoutingRule>().Where(r => r.Number == number && r.Dialplan.Id == dialplan).ToList();

            foreach (var routingRule in rules)
            {
                routingRule.Delete();
            }
        }

        private void AddRoutesFromDestinationStrings(string number, List<string> listOfStrings, int dialplan)
        {
            if (string.IsNullOrEmpty(number) || listOfStrings.All(s => !s.Contains(","))) return;

            var idx = 0;

            foreach (var str in listOfStrings.Where(s=>s.Contains(",")))
            {
                CreateRule(str, idx++, number, dialplan);
            }
         
        }

        private void CreateRule(string desinationData, int position, string number, int dialplan)
        {
            var dest = GetDestination(desinationData);

            var rule = _modelRepository.Add<IRoutingRule>();
            rule.Dialplan = _modelRepository.GetFromId<IDialplan>(dialplan);
            rule.Number = number;
            rule.DestinationType = (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), dest[0].Trim());
            rule.DestinationNumber = dest[1].Trim();
            rule.Time = int.Parse(GetTime(desinationData));
            rule.Order = position;
        }

        private static List<string> GetDestination(string destination)
        {
            return destination.Split('f')[0].Split(',').ToList();
        }

        private static string GetTime(string time)
        {
            return string.IsNullOrEmpty(time.Split('f')[1].Substring(3)) ? "0" : time.Split('f')[1].Substring(3);
        }
    }
}