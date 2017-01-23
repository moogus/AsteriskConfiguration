using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class AutoAttendantRulesController : Controller
  {
    private readonly IRepository _repository;

    public AutoAttendantRulesController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(int id)
    {
      var autoAttendant = _repository.GetFromId<IAutoAttendant>(id);
      return View(autoAttendant);
    }

    public string Add(string name, string entry, string dest)
    {
      if (name != "" || dest.Contains(","))
      {
        var desinationData = GetDestination(dest);
        var autoAttendantRule = _repository.Add<IAutoAttendantRules>();
        autoAttendantRule.AaName = name;
        autoAttendantRule.Entry = entry == "invalid" ? "i" : entry == "timeout" ? "t" : entry;

        //todo this will go bang if not route is selected on the page
        autoAttendantRule.DestinationNumber = desinationData[1].Trim();
        autoAttendantRule.DestinationType =
          (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), desinationData[0].Trim());

        return autoAttendantRule.Update() ? "Added" : "";
      }
      return "";
    }

    public string Update(string id, string name, string entry, string dest)
    {
      if (dest.Contains(","))
      {
        var desinationData = GetDestination(dest);
        var autoAttendantRule = _repository.GetFromId<IAutoAttendantRules>(int.Parse(id));
        autoAttendantRule.AaName = name;
        autoAttendantRule.Entry = entry == "invalid" ? "i" : entry == "timeout" ? "t" : entry;

        //todo this will go bang if not route is selected on the page
        autoAttendantRule.DestinationNumber = desinationData[1].Trim();
        autoAttendantRule.DestinationType =
          (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), desinationData[0].Trim());

        return autoAttendantRule.Update() ? "Updated" : "";
      }
      return "";
    }

    public string Delete(int id)
    {
      var autoAttendantRule = _repository.GetFromId<IAutoAttendantRules>(id);
      return autoAttendantRule.Delete() ? "Deleted" : "";
    }

    public JsonResult AutoAttendantRulesData(string autoName)
    {
      var aRData = _repository.GetList<IAutoAttendantRules>().Where(aa => aa.AaName == autoName);
      return Json(new AutoAttendantRulesJsonViewModel(aRData), JsonRequestBehavior.AllowGet);
    }

    private static List<string> GetDestination(string destination)
    {
      return destination.Split('f')[0].Split(',').ToList();
    }

    public JsonResult AttendantRuleData(string atten)
    {
      var autoAttendant = _repository.GetFromName<IAutoAttendant>(atten);

      var entries = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "i", "t"};

      return
        Json(new {aoData = entries.Where(e => autoAttendant.Rules.All(r => r.Entry != e)).Select(e => new {Key = e})},
             JsonRequestBehavior.AllowGet);
    }
  }
}