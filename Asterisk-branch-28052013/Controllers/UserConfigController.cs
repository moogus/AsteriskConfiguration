using System.Linq;
using System.Web.Mvc;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserConfigController : Controller
  {
    private readonly IRepository _repository;

    public UserConfigController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Logon()
    {
      return View();
    }

    [HttpPost]
    public bool TestLogon(string userName, string password)
    {
      return _repository.GetList<IUserConfig>().Any(u => u.Number == userName && u.Password == password);
    }
  }
}