using System.Linq;
using System.Web.Mvc;
using AMIWrapper;
using Asterisk.JsonViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class AutoAttendantController : Controller
  {
    private readonly IRepository _repository;

    public AutoAttendantController(IRepository repository)
    {
      _repository = repository;
    }

    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public string Add(string name, int timeout)
    {
      if (name != "" && _repository.GetFromName<IAutoAttendant>(name) == null)
      {
        var autoAttendant = _repository.Add<IAutoAttendant>();
        autoAttendant.Name = name;
        autoAttendant.Timeout = timeout;
        autoAttendant.Announcement = autoAttendant.Name;
        return autoAttendant.Update() ? "Added" + autoAttendant.Name : "";
      }
      return "";
    }

    [Authorize(Roles = "admin")]
    public string Update(int id, string name, int timeout)
    {
      var autoAttendant = _repository.GetFromId<IAutoAttendant>(id);
      autoAttendant.Name = name;
      autoAttendant.Timeout = timeout;
      autoAttendant.Announcement = autoAttendant.Name;
      return autoAttendant.Update() ? "Updated " + autoAttendant.Name : "";
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var autoAttendant = _repository.GetFromId<IAutoAttendant>(id);
      return autoAttendant.Delete() ? "Deleted" : "";
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult AutoAttendantData()
    {
      var aAData = _repository.GetList<IAutoAttendant>();
      return Json(new AutoAttendantJsonViewModel(aAData), JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "admin")]
    public string CallForAudio(string extension, string id)
    {
      var makeCall = new Connector(_repository.GetList<IServer>().First().IpAddress);
      makeCall.Connect();
      var rtn = makeCall.CallAsterisk(extension, "soundName", _repository.GetFromId<IAutoAttendant>(int.Parse(id)).Name,
                                      "CreateAudioForAA")
                  ? "calling" + extension
                  : "";
      makeCall.Disconnect();

      return rtn;
    }
  }
}