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
  public class RoutingAdminController : Controller
  {
    private readonly IRepository _repository;

    public RoutingAdminController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      var dialPlansForRouting = _repository.GetList<IDialplan>().Where(d => !d.Name.StartsWith("uncondition")
                                                                            && !d.Name.StartsWith("onBusy") &&
                                                                            !d.Name.StartsWith("noAnwser") &&
                                                                            !d.Name.Equals("ddiDefault") &&
                                                                            !d.Name.Equals("fallThrough") &&
                                                                            !d.Name.Equals("24hour7Days"));
      return View(dialPlansForRouting);
    }

    public string AddRange(string id, int priority, string days, string time, string plan)
    {
      if (id.Equals(""))
      {
        var range = _repository.Add<IDialplanRange>();

        range.DaysOfWeek = days;
        range.TimeRange = time;
        range.Dialplan = _repository.GetFromName<IDialplan>(plan);
        range.Priority = priority;

        return range.Update() ? "Added" : "Fail";
      }
      return string.Empty;
    }

    public string AddDate(string id, string date, string plan)
    {
      var rtn = string.Empty;

      if (id.Equals(""))
      {
        rtn = date.Contains("-") ? AddDateMultiRange(date, plan) : AddDateSingle(DateTime.Parse(date), plan);
      }
      return rtn;
    }

    public string UpdateRange(int id, int priority, string days, string time, string plan)
    {
      var range = _repository.GetFromId<IDialplanRange>(id);
      range.DaysOfWeek = days;
      range.TimeRange = time;
      range.Dialplan = _repository.GetFromName<IDialplan>(plan);
      range.Priority = priority;

      return range.Update() ? "Updated" : "Fail";
    }

    public string UpdateDate(int id, string date, string plan)
    {
      var planDate = _repository.GetFromId<IDialplanDate>(id);
      return date.Contains('-') ? UpdateDateRange(id, date, plan) : UpdateSingleDate(planDate, date, plan);
    }

    public string DeleteRange(int id)
    {
      var range = _repository.GetFromId<IDialplanRange>(id);
      return range.Delete() ? "Deleted" : "Fail";
    }

    public string DeleteDate(int id)
    {
      var planDate = _repository.GetFromId<IDialplanDate>(id);

      return planDate.Delete() ? "Deleted" : "Fail";
    }

    public JsonResult DialPlanRangeData()
    {
      var dialplanRanges = _repository.GetList<IDialplanRange>();

      return Json(new DialPlanRangeJsonViewModel(dialplanRanges), JsonRequestBehavior.AllowGet);
    }

    public JsonResult DialPlanDateData()
    {
      var dialplanDates = _repository.GetList<IDialplanDate>();
      return Json(new DialPlanDateJsonViewModel(dialplanDates), JsonRequestBehavior.AllowGet);
    }

    private static List<DateTime> GetDateRange(string date)
    {
      return date.Split('-').Select(DateTime.Parse).ToList();
    }

    public string AddDateSingle(DateTime date, string plan)
    {
      var planDate = _repository.Add<IDialplanDate>();
      planDate.StartDate = date;
      planDate.EndDate = date;
      planDate.Dialplan = _repository.GetFromName<IDialplan>(plan);

      return planDate.Update() ? "Added" : "Fail";
    }

    private string AddDateMultiRange(string date, string plan)
    {
      var datesToAdd = GetDateRange(date);

      var planDate = _repository.Add<IDialplanDate>();
      planDate.StartDate = datesToAdd[0];
      planDate.EndDate = datesToAdd[1];
      planDate.Dialplan = _repository.GetFromName<IDialplan>(plan);

      return planDate.Update() ? "Added" : "Fail";
    }


    public string UpdateSingleDate(IDialplanDate planDate, string date, string plan)
    {
      planDate.StartDate = DateTime.Parse(date);
      planDate.EndDate = DateTime.Parse(date);
      planDate.Dialplan = _repository.GetFromName<IDialplan>(plan);
      return planDate.Update() ? "Updated" : "Fail";
    }

    public string UpdateDateRange(int id, string date, string plan)
    {
      var datesToAdd = GetDateRange(date);

      var planDate = _repository.GetFromId<IDialplanDate>(id);
      planDate.StartDate = datesToAdd[0];
      planDate.EndDate = datesToAdd[1];
      planDate.Dialplan = _repository.GetFromName<IDialplan>(plan);

      return planDate.Update() ? "UPdated" : "Fail";
    }
  }
}