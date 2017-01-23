using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class PermissionPatternController : Controller
    {
        private readonly IRepository _modelRepository;

        public PermissionPatternController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string dialplan)
        {
            var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);

            return View(new PermissionControllerViewModel(dP, _modelRepository));
        }

        public JsonResult PatternData()
        {
            var patternData = _modelRepository.GetList<IPermissionPattern>();

            return Json(new PatternJsonViewModel(patternData), JsonRequestBehavior.AllowGet);
        }

        public string Add(string name, string pattern)
        {
            if (name == "") return "The name was invalid.";
            if (_modelRepository.GetFromName<IPermissionPattern>(name) != null) return "The name is already is use.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var p = _modelRepository.Add<IPermissionPattern>();

                p.Name = name;
                p.Pattern = pattern;
                
                return transaction.Commit() ? "The pattern has been added." : "Failed to add the new pattern.";
            }            
        }

        public string Update(int id, string name, string pattern)
        {
            var p = _modelRepository.GetFromId<IPermissionPattern>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                p.Name = name;
                p.Pattern = pattern;

                return transaction.Commit() ? "The pattern has been updated." : "Failed to update the pattern.";
            }
        }

        public string Delete(int id)
        {
            var p = _modelRepository.GetFromId<IPermissionPattern>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                p.Delete();

                return transaction.Commit() ? "The pattern has been deleted." : "Failed to delete the pattern.";
            }
        }
    }
}