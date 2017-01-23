using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class MusicOnHoldController : Controller
    {
        private readonly IRepository _modelRepository;

        public MusicOnHoldController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Add(string name, string application, string sort, string random)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var moh = _modelRepository.Add<IMusicOnHold>();

                moh.Name = name;
                moh.Application = application;
                moh.Sort = sort.Trim().Equals("yes");
                moh.Random = random.Trim().Equals("yes");
                moh.Format = "";

                return transaction.Commit() ? "Added music on hold." : "Failed to add music on hold.";
            }
        }

        public string Update(int id, string name, string application, string sort, string random)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var moh = _modelRepository.GetFromId<IMusicOnHold>(id);

                moh.Name = string.IsNullOrEmpty(name) ? "Default" : name;
                moh.Application = application;
                moh.Sort = sort.Equals("yes");
                moh.Format = "";
                moh.Random = random.Equals("yes");

                return transaction.Commit() ? "Updated music on hold." : "Failed to update music on hold.";
            }
        }

        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var moh = _modelRepository.GetFromId<IMusicOnHold>(id);
                moh.Delete();

                return transaction.Commit() ? "Deleted music on hold." : "Failed to delete music on hold.";
            }
        }

        public JsonResult MusicOnHoldData()
        {
            var musicOnHoldData = _modelRepository.GetList<IMusicOnHold>().Where(m => m.Id != 0);

            return Json(new MusuicOnHoldJsonViewModel(musicOnHoldData), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AvailableMoh()
        {
            var list = _modelRepository.GetList<IMusicOnHold>().Where(p => p.Id != 0).OrderBy(p => p.Id).Select(p => p.Name).ToList();
            list.Add("No Music On Hold");

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }

    public class MusuicOnHoldJsonViewModel
    {
        public List<string[]> aaData;
        public List<JsonMusicOnHold> aoData;

        public MusuicOnHoldJsonViewModel(IEnumerable<IMusicOnHold> musicOnHoldData)
        {
            aaData = new List<string[]>();
            aoData = new List<JsonMusicOnHold>();

            foreach (IMusicOnHold moh in musicOnHoldData)
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

                aoData.Add(new JsonMusicOnHold { Id = moh.Id, Name = moh.Name, Mode = moh.Mode.ToString() });
            }
        }

        public class JsonMusicOnHold
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Mode { get; set; }
        }
    }
}