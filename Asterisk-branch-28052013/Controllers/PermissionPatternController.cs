using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class PermissionPatternController : Controller
  {
    private readonly IRepository _repository;

    public PermissionPatternController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string dialplan)
    {
      var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);
      return View(new PermissionControllerViewModel(dP, _repository));
    }

    public JsonResult PatternData()
    {
      var patternData = _repository.GetList<IPattern>();
      return Json(new PatternJsonViewModel(patternData), JsonRequestBehavior.AllowGet);
    }


    public string Add(string name, string pattern)
    {
      if (name != "" && _repository.GetFromName<IPattern>(name) == null)
      {
        var p = _repository.Add<IPattern>();
        p.Name = name;
        p.Pattern = pattern;
        p.Update();

        return string.Format("added Pattern {0}", name);
      }
      return "";
    }

    public string Update(int id, string name, string pattern)
    {
      var p = _repository.GetFromId<IPattern>(id);
      p.Name = name;
      p.Pattern = pattern;
      p.Update();

      return "Done";
    }

    public string Delete(int id)
    {
      var pattern = _repository.GetFromId<IPattern>(id);
      return pattern.Delete() ? "Deleted pattern" + pattern.Name : "Couldn't delete pattern.";
    }
  }
}