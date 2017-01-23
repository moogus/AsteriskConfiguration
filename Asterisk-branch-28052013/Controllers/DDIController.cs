using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class DDIController : Controller
  {
    private readonly IRepository _repository;

    public DDIController(IRepository repository)
    {
      _repository = repository;
    }

    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return View(_repository.GetList<ITrunk>());
    }

    [Authorize(Roles = "admin")]
    public string Add(string ddi, string trunk)
    {
      if (ddi != "" && _repository.GetFromName<IDDI>(ddi) == null)
      {
        var d = _repository.Add<IDDI>();
        d.DDINumber = ddi;
        d.Trunk = !string.IsNullOrEmpty(trunk)
                    ? _repository.GetFromName<ITrunk>(trunk)
                    : _repository.Add<ITrunk>();
        d.Update();

        return string.Format("added DDI {0}", ddi);
      }
      return "";
    }

    [Authorize(Roles = "admin")]
    public string Update(int id, string ddi, string trunk)
    {
      var d = _repository.GetFromId<IDDI>(id);
      d.DDINumber = ddi;
      d.Trunk = !string.IsNullOrEmpty(trunk) ? _repository.GetFromName<ITrunk>(trunk) : _repository.Add<ITrunk>();
      d.Update();

      return "Done";
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var ddi = _repository.GetFromId<IDDI>(id);

      foreach (var r in _repository.GetList<IRoutingRule>().Where(r => r.Number == ddi.DDINumber))
      {
        r.Delete();
      }

      RemoveDDIFromExtensionAndQueues(ddi);

      return ddi.Delete() ? "Deleted ddi " + ddi.DDINumber : "Couldn't delete ddi";
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult AddRangeDDI(string ddiFrom, string ddiTo)
    {
      if (ddiFrom != "" && ddiTo != "")
      {
        var ddF = int.Parse(ddiFrom);
        var ddt = int.Parse(ddiTo);

        for (var i = ddF; i < ddt + 1; i++)
        {
          var ddiNumber = i.ToString(CultureInfo.InvariantCulture);
          if (_repository.GetFromName<IDDI>(ddiNumber) == null) continue;

          var d = _repository.Add<IDDI>();
          d.DDINumber = ddiNumber;
          d.Update();
        }
      }

      return RedirectToAction("Index");
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult DDIData()
    {
      var ddiData = _repository.GetList<IDDI>();
      return Json(new DDIJsonViewModel(ddiData), JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult AvailableDDIs()
    {
      return
        Json(
          _repository.GetList<IDDI>()
                     .Where(d => d.UsedOn == DDIUsedOn.NotUsed || d.UsedOn == DDIUsedOn.Default)
                     .Select(d => d.DDINumber), JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "admin")]
    private void RemoveDDIFromExtensionAndQueues(IDDI ddi)
    {
      foreach (var q in _repository.GetList<IQueue>().Where(q => q.DDINumber == ddi.DDINumber))
      {
        q.DDINumber = string.Empty;
        q.Update();
      }

      foreach (var e in _repository.GetList<IExtension>().Where(e => e.DDI.DDINumber == ddi.DDINumber))
      {
        e.DDI = null;
        e.Update();
      }
    }
  }
}