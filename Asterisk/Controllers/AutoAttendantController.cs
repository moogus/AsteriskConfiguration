using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AMIWrapper;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class AutoAttendantController : Controller
    {
        private readonly IRepository _modelRepository;

        public AutoAttendantController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public string Add(string name, int timeout)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                if (name != "" && _modelRepository.GetFromName<IAutoAttendant>(name) == null)
                {
                    var autoAttendant = _modelRepository.Add<IAutoAttendant>();
                    autoAttendant.Name = name;
                    autoAttendant.Timeout = timeout;
                    autoAttendant.Announcement = autoAttendant.Name;

                    return transaction.Commit() ? string.Format("Added {0}", autoAttendant.Name) : string.Format("Failed to add {0}", autoAttendant.Name);
                }

                return "Invalid name specified.";
            }            
        }

        [Authorize(Roles = "admin")]
        public string Update(int id, string name, int timeout)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var autoAttendant = _modelRepository.GetFromId<IAutoAttendant>(id);
                autoAttendant.Name = name;
                autoAttendant.Timeout = timeout;
                autoAttendant.Announcement = autoAttendant.Name;
                                
                return transaction.Commit() ? string.Format("Updated {0}", autoAttendant.Name) : string.Format("Failed to update {0}", autoAttendant.Name);
            }
        }

        [Authorize(Roles = "admin")]
        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var autoAttendant = _modelRepository.GetFromId<IAutoAttendant>(id);
                autoAttendant.Delete();

                return transaction.Commit() ? string.Format("Deleted {0}", autoAttendant.Name) : string.Format("Failed to delete {0}", autoAttendant.Name);
            }
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult AutoAttendantData()
        {
            IEnumerable<IAutoAttendant> aAData = _modelRepository.GetList<IAutoAttendant>();
            return Json(new AutoAttendantJsonViewModel(aAData), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        public string CallForAudio(string extension, string id)
        {
            var makeCall = new Connector(_modelRepository.GetList<IServer>().First().IpAddress);
            makeCall.Connect();

            var rtn = makeCall.CallAsterisk(extension, "soundName", _modelRepository.GetFromId<IAutoAttendant>(int.Parse(id)).Name, "CreateAudioForAA")
                           ? "Calling" + extension
                           : "Asterisk call failed";

            makeCall.Disconnect();

            return rtn;
        }
    }
}