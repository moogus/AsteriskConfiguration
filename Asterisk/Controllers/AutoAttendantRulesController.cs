using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class AutoAttendantRulesController : Controller
    {
        private readonly IRepository _modelRepository;

        public AutoAttendantRulesController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(int id)
        {
            var autoAttendant = _modelRepository.GetFromId<IAutoAttendant>(id);

            return View(autoAttendant);
        }

        public string Add(string name, string entry, string dest)
        {
            if (name == "") return "Please specify a valid name.";
            if (entry == "") return "Please specify a valid ??.";
            if (!dest.Contains(",")) return "Please select a valid destination.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var desinationData = GetDestination(dest);
                var autoAttendantRule = _modelRepository.Add<IAutoAttendantRules>();

                autoAttendantRule.AaName = name;
                autoAttendantRule.Entry = entry == "invalid" ? "i" : entry == "timeout" ? "t" : entry;
                autoAttendantRule.DestinationNumber = desinationData[1].Trim();
                autoAttendantRule.DestinationType = (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), desinationData[0].Trim());

                return transaction.Commit() ? "Added rule." : "Failed to add rule.";
            }            
        }

        public string Update(string id, string name, string entry, string dest)
        {
            if (name == "") return "Please specify a valid name.";
            if (entry == "") return "Please specify a valid ??.";
            if (!dest.Contains(",")) return "Please select a valid destination.";

            var transaction = _modelRepository.ModelTransaction();

            using(transaction)
            {
                var desinationData = GetDestination(dest);
                var autoAttendantRule = _modelRepository.GetFromId<IAutoAttendantRules>(int.Parse(id));

                autoAttendantRule.AaName = name;
                autoAttendantRule.Entry = entry == "invalid" ? "i" : entry == "timeout" ? "t" : entry;
                
                autoAttendantRule.DestinationNumber = desinationData[1].Trim();
                autoAttendantRule.DestinationType =
                (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), desinationData[0].Trim());

                return transaction.Commit() ? "Updated rule." : "Failed to update rule.";
            }            
        }

        public string Delete(int id)
        {
            var autoAttendantRule = _modelRepository.GetFromId<IAutoAttendantRules>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                autoAttendantRule.Delete();

                return transaction.Commit() ? "Deleted rule." : "Failed to delete rule.";
            }            
        }

        public JsonResult AutoAttendantRulesData(string autoName)
        {
            var aRData = _modelRepository.GetList<IAutoAttendantRules>().Where(aa => aa.AaName == autoName);

            return Json(new AutoAttendantRulesJsonViewModel(aRData), JsonRequestBehavior.AllowGet);
        }

        private static List<string> GetDestination(string destination)
        {
            return destination.Split('f')[0].Split(',').ToList();
        }

        public JsonResult AttendantRuleData(string atten)
        {
            var autoAttendant = _modelRepository.GetFromName<IAutoAttendant>(atten);

            var entries = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "i", "t" };

            return Json(new { aoData = entries.Where(e => autoAttendant.Rules.All(r => r.Entry != e)).Select(e => new { Key = e }) }, JsonRequestBehavior.AllowGet);
        }
    }
}