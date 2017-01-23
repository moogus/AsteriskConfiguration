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
  public class RouteController : Controller
  {
    private readonly IRepository _repository;

    public RouteController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string dialplan)
    {
      var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);
      return View(new RouteControllerViewModel(dP, _repository));
    }

    public string Add(string number, string d1, string d2, string d3, string d4, string d5, string dialplan)
    {
      return AddRoutesFromDestinationStrings(number, new List<string> {d1, d2, d3, d4, d5}, int.Parse(dialplan))
               ? "Added"
               : "";
    }

    public string Update(string number, string d1, string d2, string d3, string d4, string d5, string dialplan)
    {
      var remove = RemoveAllRulesForNumber(int.Parse(dialplan), number);
      var add = AddRoutesFromDestinationStrings(number, new List<string> {d1, d2, d3, d4, d5}, int.Parse(dialplan));
      return remove ? add ? "Updated" : "" : "";
    }

    public string Delete(int dialplan, string number)
    {
      return RemoveAllRulesForNumber(dialplan, number) ? "Deleted" : "";
    }

    public new JsonResult RouteData(int dialplan)
    {
      return Json(new RouteJsonViewModel(_repository.GetFromId<IDialplan>(dialplan), _repository),
                  JsonRequestBehavior.AllowGet);
    }

    private bool RemoveAllRulesForNumber(int dialplan, string number)
    {
      var rules =
        _repository.GetList<IRoutingRule>().Where(r => r.Number == number && r.Dialplan.Id == dialplan).ToList();
      return rules.Select(r => r.Delete()).All(b => b);
    }

    private bool AddRoutesFromDestinationStrings(string number, List<string> listOfStrings, int dialplan)
    {
      if (string.IsNullOrEmpty(number) || listOfStrings.All(s => !s.Contains(",")))
        return false;

      return
        listOfStrings.Where(s => s.Contains(","))
                     .Select((rule, index) => CreateRule(rule, index, number, dialplan))
                     .All(b => b);
    }

    private bool CreateRule(string desinationData, int position, string number, int dialplan)
    {
      var dest = GetDestination(desinationData);
      var rule = _repository.Add<IRoutingRule>();
      rule.Dialplan = _repository.GetFromId<IDialplan>(dialplan);

      rule.Number = number;
      rule.DestinationType = (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), dest[0].Trim());
      rule.DestinationNumber = dest[1].Trim();
      rule.Time = int.Parse(GetTime(desinationData));
      rule.Order = position;

      return rule.Update();
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