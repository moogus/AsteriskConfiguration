﻿using System.Linq;
using System.Web.Mvc;
using Asterisk.ViewModels;
using DatabaseAccess;
using DatabaseAccess.Models;


namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class EndUserController : Controller
  {
    private readonly IRepository _repository;

    public EndUserController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string message)
    {
      var vm = new EndUserCurrentDialplanViewModel
        {
          Message =
            message ??
            "The Dialplan is automatically set by the phone system at different times of day. This can be overridden by changing it below.",
          CurrentDialPlan = _repository.Add<ICurrentDialPlan>(),
          Dialplans =
            _repository.GetList<IDialplan>()
                       .Where(
                         d =>
                         !d.Name.StartsWith("uncondition") && !d.Name.StartsWith("onBusy") &&
                         !d.Name.StartsWith("noAnwser") && !d.Name.Equals("ddiDefault") &&
                         !d.Name.Equals("NotRecognised"))
        };

      return View(vm);
    }

    public ActionResult Update(int currentDialPlan, bool enableCalendar = false)
    {
      var cdp = _repository.Add<ICurrentDialPlan>();
      var dp = _repository.GetFromId<IDialplan>(currentDialPlan);
      cdp.Dialplan = dp;
      cdp.AutomaticallyChange = enableCalendar;

      return RedirectToAction("Index",
                              cdp.Update()
                                ? new {message = string.Format("The dialplan has been set to {0}.", dp.Name)}
                                : new
                                  {
                                    message =
                                    string.Format(
                                      "Something went wrong the dialplan is still {0}.  Please contact your system administrator.",
                                      cdp.Dialplan.Name)
                                  });
    }
  }
}