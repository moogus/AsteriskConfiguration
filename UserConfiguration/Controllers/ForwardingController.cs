using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelAccess.Models;

namespace UserConfiguration.Controllers
{
  public class ForwardingController : Controller
  {
    private readonly Repository<IDDI> _ddiContext;
    private readonly Repository<IExtension> _extensionContext;
    private readonly Repository<IQueue> _queueContext;
    private readonly Repository<IVoiceMail> _voiceMailContext;
    private readonly Repository<IRoutingRule> _routingRuleContext;
    private readonly Repository<IDialplan> _dialplanContext;

    public ForwardingController()
    {
      _ddiContext = new Repository<IDDI>();
      _extensionContext = new Repository<IExtension>();
      _queueContext = new Repository<IQueue>();
      _voiceMailContext = new Repository<IVoiceMail>();
      _routingRuleContext = new Repository<IRoutingRule>();
      _dialplanContext = new Repository<IDialplan>();
    }

    public ActionResult Index()
    {
      var extn = _extensionContext.GetFromName(User.Identity.Name);
      var ddi = _ddiContext.GetList();
      var e = _extensionContext.GetList();
      var q = _queueContext.GetList();
      var vm = _voiceMailContext.GetList();

      var fVm = new ForwardingViewModel
      {
        ThisExtension = extn,
        DDIList = ddi,
        ExtensionList = e,
        QueueList = q,
        VoiceMailList = vm
      };
      return View(fVm);
    }
    public JsonResult RingPlanData()
    {
      var x = _routingRuleContext.GetList();
      var ringPlanData = x.Where(r => r.Number == User.Identity.Name && r.Dialplan.Name == "personalOff" || r.Dialplan.Name == "personalOn");
     
      return Json(new RingPlanJsonViewModel(ringPlanData), JsonRequestBehavior.AllowGet);
    }

    public string Add(string id, string enabled, string goesTo)
    {
      var destinationData = GetDestinationData(goesTo);
      if (id.Equals("") && destinationData.Count == 2)
      {
        var rule = _routingRuleContext.Add();
        rule.Dialplan = enabled == "yes" ? _dialplanContext.GetFromName("personalOn") : _dialplanContext.GetFromName("personalOff");
        rule.Number = User.Identity.Name;

        rule.DestinationType = (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), destinationData[0]);
        rule.DestinationNumber = destinationData[1];

        rule.Time = 0;
        rule.Order = 1;

        return rule.Update() ? enabled == "yes" ? TurnOffOtherPersonalRoutes(rule, "Added") : "Added" : "";
      }
      return "";
    }

    public string Update(int id,string enabled, string goesTo)
    {
      var destinationData = GetDestinationData(goesTo);
      if (!id.Equals("") && destinationData.Count == 2)
      {
        var rule = _routingRuleContext.GetFromId(id);
        rule.Dialplan = enabled == "yes" ? _dialplanContext.GetFromName("personalOn") : _dialplanContext.GetFromName("personalOff");
        rule.Number = User.Identity.Name;

        rule.DestinationType = (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), destinationData[0]);
        rule.DestinationNumber = destinationData[1];

        rule.Time = 0;
        rule.Order = 1;

        return rule.Update() ? enabled == "yes" ? TurnOffOtherPersonalRoutes(rule, "Updated") : "Updated" : "";
      }
      return "";
    }
    
    public string Delete(int id)
    {
      var rule = _routingRuleContext.GetFromId(id);
      return rule.Delete() ? "Deleted" : "";
    }

    private static List<string> GetDestinationData(string destination)
    {
      return destination.Split(',').ToList();
    }

    private string TurnOffOtherPersonalRoutes(IRoutingRule rule, string action)
    {
      var currentRuleId = rule.Id;
      var rtn = true;
      var usersPersonalRoutes = _routingRuleContext.GetList().Where(rr => rr.Number.Equals(User.Identity.Name)
                                                                          && (rr.Dialplan.Name.Equals("personalOn")));

      foreach (var r in usersPersonalRoutes)
      {
        r.Dialplan = r.Id == currentRuleId ? _dialplanContext.GetFromName("personalOn") :
                                              _dialplanContext.GetFromName("personalOff");
        rtn = r.Update();
      }
      return rtn ? action : "";
    }
  }
  public class ForwardingViewModel
  {
    public IExtension ThisExtension { get; set; }
    public IEnumerable<IDDI> DDIList { get; set; }
    public IEnumerable<IExtension> ExtensionList { get; set; }
    public IEnumerable<IQueue> QueueList { get; set; }
    public IEnumerable<IVoiceMail> VoiceMailList { get; set; }
  }

  public class RingPlanJsonViewModel
  {
    public List<string[]> aaData;

    public RingPlanJsonViewModel(IEnumerable<IRoutingRule> ringPlanData)
    {
      aaData = new List<string[]>();

      foreach (var plan in ringPlanData)
      {
        var line = new string[5];
        line[0] = plan.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = plan.Dialplan.Name == "personalOn" ? "yes" : "no";
        line[2] = string.Format("{0},{1}", plan.DestinationType.ToString(), plan.DestinationNumber);
        line[3] = "";
        line[4] = "";
        aaData.Add(line);
      }
  
    }
  }
}
