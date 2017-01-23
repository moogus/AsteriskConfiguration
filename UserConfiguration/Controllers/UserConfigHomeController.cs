using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserConfiguration.Controllers
{
  public class UserConfigHomeController : Controller
  {
    public ActionResult Index()
    {
      var message = (string)TempData["message"];
      ViewBag.Message = string.IsNullOrEmpty(message) ? "Welcome please select an option from the menu." : message;
      return View();
    }
  }
}
