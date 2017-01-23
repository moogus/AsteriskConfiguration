using System.Linq;
using System.Web.Mvc;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserConfigController : Controller
  {
    private readonly IRepository _modelRepository;

    public UserConfigController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
    }

    public ActionResult Logon()
    {
      return View();
    }

    [HttpPost]
    public bool TestLogon(string userName, string password)
    {
      return _modelRepository.GetList<IUserConfig>().Any(u => u.Number == userName && u.Password == password);
    }
  }
}