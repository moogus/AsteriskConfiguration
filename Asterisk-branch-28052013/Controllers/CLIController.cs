using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class CLIController : Controller
  {
    private readonly IRepository _repository;

    public CLIController(IRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return RedirectToAction("CLIItems");
    }

    [Authorize(Roles = "admin")]
    public ActionResult CSVError()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public ActionResult CLIItems()
    {
      var trunkList = _repository.GetList<ITrunk>().Select(t => t.Name);
      return View(trunkList);
    }

    [Authorize(Roles = "admin")]
    public string Add(string cliName, string cliNumber, string trunk)
    {
      if (cliName != "" && cliNumber != "")
      {
        var cli = _repository.Add<ICLI>();
        cli.CLIName = cliName;
        cli.CLINumber = cliNumber;
        cli.Trunk = _repository.GetFromName<ITrunk>(trunk);

        return cli.Update() ? "Added :" + cliNumber : "";
      }
      return "";
    }

    [Authorize(Roles = "admin")]
    public string Update(int id, string cliName, string cliNumber, string trunk)
    {
      var cli = _repository.GetFromId<ICLI>(id);

      cli.CLIName = cliName;
      cli.CLINumber = cliNumber;
      cli.Trunk = _repository.GetFromName<ITrunk>(trunk);

      return cli.Update() ? "Updated " + cliNumber : "";
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var cli = _repository.GetFromId<ICLI>(id);
      RemoveCLIFromExtensionsAndQueues(cli);
      return cli.Delete() ? "Deleted" : "";
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult CLINumberData()
    {
      return Json(_repository.GetList<ICLI>().OrderBy(c => c.Trunk).Select(c => c.CLINumber),
                  JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult CLIData()
    {
      if (_repository.GetList<ICLI>().Any())
      {
        var cliData = _repository.GetList<ICLI>().OrderBy(c => c.Trunk.Id);
        return Json(new CLIJsonViewModel(cliData), JsonRequestBehavior.AllowGet);
      }

      return Json(new CLIJsonViewModel(_repository.GetList<ICLI>()), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult UploadCliByCSV(HttpPostedFileBase file)
    {
      var importData = new ImportDataFromCSV(file, Server);
      Action<DataRow> s = (dataRow) =>
        {
          var cli = _repository.Add<ICLI>();
          cli.CLIName = dataRow[0].ToString();
          cli.CLINumber = dataRow[1].ToString();

          cli.Update();
        };
      importData.SaveCSVData(s, 2);
      return RedirectToAction("CLIItems");
    }

    [Authorize(Roles = "admin")]
    private void RemoveCLIFromExtensionsAndQueues(ICLI cli)
    {
      var allextension =
        _repository.GetList<IExtension>().Where(e => e.CLI.CLINumber == cli.CLINumber);
      foreach (var e in allextension)
      {
        e.CLI = null;
        e.Update();
      }

      var allQueues = _repository.GetList<IQueue>().Where(q => q.CLINumber == cli.CLINumber);
      foreach (var q in allQueues)
      {
        q.CLINumber = "";
        q.Update();
      }
    }
  }
}