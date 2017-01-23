using System.Linq;
using System.Web.Mvc;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserConfigHomeController : Controller
  {
    private readonly IRepository _modelRepository;

    public UserConfigHomeController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
    }

    public ActionResult Index(string extn)
    {
      if (string.IsNullOrEmpty(extn) && string.IsNullOrEmpty(User.Identity.Name))
      {
        return RedirectToAction("LogOn", "Account");
      }
      return View(_modelRepository.GetList<IUserConfig>().OrderBy(u => u.Number));
    }
  }
}