using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  public class RingtonesController : Controller
  {
    private readonly IRepository _modelRepository;

    public RingtonesController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult RingToneData()
    {
      IOrderedEnumerable<IRingTone> ringtoneData = _modelRepository.GetList<IRingTone>().OrderBy(r => r.Name);
      return Json(new RingToneJsonViewModel(ringtoneData), JsonRequestBehavior.AllowGet);
    }
  }
}