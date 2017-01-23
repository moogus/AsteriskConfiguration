using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asterisk.ViewModels;
using DatabaseAccess;
using Asterisk.AccountManagement;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserConfigHomeController : Controller
  {
    private readonly IRepository _repository;

    public UserConfigHomeController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string extn)
    {
      if (string.IsNullOrEmpty(extn) && string.IsNullOrEmpty(User.Identity.Name))
      {
        return RedirectToAction("LogOn", "Account");
      }
      return View(_repository.GetList<IUserConfig>().OrderBy(u => u.Number));
    }
  }
}