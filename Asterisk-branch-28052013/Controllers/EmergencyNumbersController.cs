using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class EmergencyNumbersController : Controller
  {
    private readonly IRepository _repository;

    public EmergencyNumbersController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public string Add(string number, string description, string isInternal)
    {
      var emergencyNumber = _repository.Add<IEmergencyNumber>();
      emergencyNumber.Number = number;
      emergencyNumber.Description = description;

      return emergencyNumber.Update() ? "Added " + emergencyNumber.Number : "something went wrong......";
    }

    public string Update(int id, string description, string number, string isInternal)
    {
      var emergencyNumber = _repository.GetFromId<IEmergencyNumber>(id);
      emergencyNumber.Number = number;
      emergencyNumber.Description = description;

      return emergencyNumber.Update() ? "Updated " + emergencyNumber.Number : "something went wrong......";
    }

    public string Delete(int id)
    {
      var number = _repository.GetFromId<IEmergencyNumber>(id);
      return number.Delete() ? "Deleted number " + number.Number : "Couldn't delete number.";
    }

    public JsonResult EmergencyNumberData()
    {
      var emergencyData = _repository.GetList<IEmergencyNumber>();
      return Json(new EmergencyNumberJsonViewModel(emergencyData), JsonRequestBehavior.AllowGet);
    }
  }

  public class EmergencyNumberJsonViewModel
  {
    public class JsonEmergencyNumber
    {
      public int Id { get; set; }
      public string Number { get; set; }
      public string Description { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonEmergencyNumber> aoData;

    public EmergencyNumberJsonViewModel(IEnumerable<IEmergencyNumber> emergencyData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonEmergencyNumber>();

      foreach (var aa in emergencyData)
      {
        var line = new string[5];
        line[0] = aa.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = aa.Description;
        line[2] = aa.Number;
        line[3] = "";
        line[4] = "";

        aaData.Add(line);

        aoData.Add(new JsonEmergencyNumber
          {
            Id = aa.Id,
            Number = aa.Number,
            Description = aa.Description
          });
      }
    }
  }
}