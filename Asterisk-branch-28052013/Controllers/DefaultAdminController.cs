using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class DefaultAdminController : Controller
  {
    private readonly IRepository _repository;

    public DefaultAdminController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public string Add(string defaultValue)
    {
      return "";
    }

    public string Update(int id, string defaultValue)
    {
      var thisDefault = _repository.GetFromId<IDefault>(id);
      thisDefault.DefaultValue = defaultValue;

      return thisDefault.Update() ? string.Format("updated {0}, {1}", thisDefault.Type, thisDefault.ColumnTitle) : "";
    }

    public string Delete(int id)
    {
      return "";
    }

    public JsonResult DefaultData()
    {
      var defaults = _repository.GetList<IDefault>().Where(d => !string.IsNullOrEmpty(d.DefaultValue));

      return Json(new DefaultJsonViewModel(defaults), JsonRequestBehavior.AllowGet);
    }
  }
}