using System.Web.Mvc;

namespace Asterisk.Controllers
{
  public class PhoneStateController : Controller
  {
    public ActionResult Index()
    {
      string request = Request.ServerVariables["REMOTE_ADDR"];
      return View(new PhoneStateIndexVm(request));
    }

    public class PhoneStateIndexVm
    {
      public PhoneStateIndexVm(string request)
      {
        Request = request;
      }

      public string Request { get; set; }
    }
  }
}