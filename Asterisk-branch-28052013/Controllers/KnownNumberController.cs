using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class KnownNumberController : Controller
  {
    private readonly IRepository _repository;

    public KnownNumberController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult CSVError()
    {
      return View();
    }

    public string Add(string number, string description, string isInternal)
    {
      var knownNumber = _repository.Add<IKnownNumber>();
      knownNumber.Number = number;
      knownNumber.Description = description;
      knownNumber.IsInternal = !isInternal.EndsWith("no");

      return knownNumber.Update() ? "Added " + knownNumber.Number : "something went wrong......";
    }

    public string Update(int id, string number, string description, string isInternal)
    {
      var knownNumber = _repository.GetFromId<IKnownNumber>(id);
      knownNumber.Number = number;
      knownNumber.Description = description;
      knownNumber.IsInternal = !isInternal.EndsWith("no");

      return knownNumber.Update() ? "Updated " + knownNumber.Number : "something went wrong......";
    }

    public string Delete(int id)
    {
      var number = _repository.GetFromId<IKnownNumber>(id);

      return number.Delete() ? "Deleted number " + number.Number : "Couldn't delete number.";
    }

    public JsonResult KnownNumberData()
    {
      var ddiData = _repository.GetList<IKnownNumber>();
      return Json(new KnownNumberJsonViewModel(ddiData), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public ActionResult UploadExtensionsByCSV(HttpPostedFileBase file)
    {
      var importData = new ImportDataFromCSV(file, Server);
      Action<DataRow> s = (dataRow) =>
        {
          var knownNumber = _repository.Add<IKnownNumber>();
          knownNumber.Number = dataRow[0].ToString();
          knownNumber.Description = dataRow[1].ToString();
          knownNumber.IsInternal = !dataRow[2].ToString().EndsWith("no");
          knownNumber.Update();
        };

      return RedirectToAction(importData.SaveCSVData(s, 3) ? "Index" : "CSVError");
    }
  }
}