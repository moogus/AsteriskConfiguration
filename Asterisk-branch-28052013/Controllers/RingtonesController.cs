using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class RingtonesController : Controller
  {
    private readonly IRepository _repository;

    public RingtonesController(IRepository repository)
    {
      _repository = repository;
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult RingToneData()
    {
      var ringtoneData = _repository.GetList<IRingTone>().OrderBy(r => r.Name);
      return Json(new RingToneJsonViewModel(ringtoneData), JsonRequestBehavior.AllowGet);
    }
  }
}