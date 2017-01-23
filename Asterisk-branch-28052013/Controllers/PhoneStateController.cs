using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asterisk.Controllers
{
  public class PhoneStateController : Controller
  {
    public class PhoneStateIndexVM
    {
      public string Request { get; set; }

      public PhoneStateIndexVM(string request)
      {
        Request = request;
      }
    }

    public ActionResult Index()
    {
      var request = Request.ServerVariables["REMOTE_ADDR"];
      return View(new PhoneStateIndexVM(request));
    }
  }
}