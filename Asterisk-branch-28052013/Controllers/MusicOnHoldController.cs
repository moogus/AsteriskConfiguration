using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class MusicOnHoldController : Controller
  {
    private readonly IRepository _repository;

    public MusicOnHoldController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public string Add(string name, string application, string sort, string random)
    {
      var moh = _repository.Add<IMusicOnHold>();
      moh.Name = name;
      moh.Application = application;
      moh.Sort = sort.Trim().Equals("yes");
      moh.Random = random.Trim().Equals("yes");
      moh.Format = "";
      return moh.Update() ? "Added " + moh.Name : "something went wrong......";
    }

    public string Update(int id, string name, string application, string sort, string random)
    {
      var moh = _repository.GetFromId<IMusicOnHold>(id);
      moh.Name = string.IsNullOrEmpty(name) ? "Default" : name;
      moh.Application = application;
      moh.Sort = sort.Equals("yes");
      moh.Format = "";
      moh.Random = random.Equals("yes");

      return moh.Update() ? "Updated " + moh.Name : "something went wrong......";
    }

    public string Delete(int id)
    {
      var moh = _repository.GetFromId<IMusicOnHold>(id);

      return moh.Delete() ? "Deleted music on hold " + moh.Name : "Couldn't delete.";
    }

    public JsonResult MusicOnHoldData()
    {
      var musicOnHoldData = _repository.GetList<IMusicOnHold>().Where(m => m.Id != 0);
      return Json(new MusuicOnHoldJsonViewModel(musicOnHoldData), JsonRequestBehavior.AllowGet);
    }

    public JsonResult AvailableMoh()
    {
      var list =
        _repository.GetList<IMusicOnHold>().Where(p => p.Id != 0).OrderBy(p => p.Id).Select(p => p.Name).ToList();
      list.Add("No Music On Hold");
      return Json(list, JsonRequestBehavior.AllowGet);
    }
  }

  public class MusuicOnHoldJsonViewModel
  {
    public class JsonMusicOnHold
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Mode { get; set; }
    }

    public List<string[]> aaData;
    public List<JsonMusicOnHold> aoData;

    public MusuicOnHoldJsonViewModel(IEnumerable<IMusicOnHold> musicOnHoldData)
    {
      aaData = new List<string[]>();
      aoData = new List<JsonMusicOnHold>();

      foreach (var moh in musicOnHoldData)
      {
        var line = new string[7];
        line[0] = moh.Id.ToString(CultureInfo.InvariantCulture);
        line[1] = moh.Name;
        line[2] = string.IsNullOrEmpty(moh.Application) ? "play-wav" : moh.Application;
        line[3] = moh.Sort ? "yes" : "no";
        line[4] = moh.Random ? "yes" : "no";
        line[5] = "";
        line[6] = "";
        aaData.Add(line);

        aoData.Add(new JsonMusicOnHold {Id = moh.Id, Name = moh.Name, Mode = moh.Mode.ToString()});
      }
    }
  }
}