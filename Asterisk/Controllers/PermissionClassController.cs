using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class PermissionClassController : Controller
    {
        private readonly IRepository _modelRepository;

        public PermissionClassController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string dialplan)
        {
            var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);

            return View(new PermissionControllerViewModel(dP, _modelRepository));
        }

        public JsonResult PermissionClassData(string dialplan)
        {
            return
              Json(
                new PermissionClassJsonViewModel(_modelRepository.GetList<IPermisionClass>(),
                                                 _modelRepository.GetList<IExtension>().ToList(), int.Parse(dialplan)),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult AvailablePermissions()
        {
            return Json(_modelRepository.GetList<IPermisionClass>().OrderBy(p => p.Id).Select(p => p.Name), JsonRequestBehavior.AllowGet);
        }

        public string Add(string name, string description, string patterns, string dialplan, string extens)
        {
            if (name == "") return "Please enter a valid name.";
            if (_modelRepository.GetFromName<IPermisionClass>(name) != null) return "The name is already in use.";

            var allExten = GetAllExtensions(extens);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var permissionClass = _modelRepository.Add<IPermisionClass>();

                permissionClass.Name = name;
                permissionClass.Description = description;

                SetClassMembers(GetPatterns(patterns), permissionClass, _modelRepository.GetFromId<IDialplan>(int.Parse(dialplan)));
                UpdateAllExtensions(allExten, permissionClass);                
                
                return transaction.Commit() ? "Added permission class." : "Failed to add permission class.";
            }
        }

        public string Update(int id, string name, string description, string patterns, string dialplan, string extens)
        {
            var allExten = GetAllExtensions(extens);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var permissionClass = _modelRepository.GetFromId<IPermisionClass>(id);

                permissionClass.Name = name;
                permissionClass.Description = description;

                SetClassMembers(GetPatterns(patterns), permissionClass, _modelRepository.GetFromId<IDialplan>(int.Parse(dialplan)));
                UpdateAllExtensions(allExten, permissionClass);                

                return transaction.Commit() ? "Updated permissions class." : "Failed to update permissions class.";
            }
        }

        public string Delete(int id)
        {
            var pattern = _modelRepository.GetFromId<IPermisionClass>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                //delete all class memebers dialplan set to 0 to indicate all
                DeletePermissionClassMembers(pattern, 0);

                //set all the extensions back to default that where in the permission class
                ResetExtensionsInPermissionClass(_modelRepository.GetList<IExtension>().Where(e => e.PermisionClass.Id == pattern.Id));

                pattern.Delete();                

                return transaction.Commit() ? "Deleted permissions class." : "Failed to delete the permissions class.";
            }
        }

        private void ResetExtensionsInPermissionClass(IEnumerable<IExtension> extensions)
        {
            UpdatePermissionClassOnExtension(extensions, _modelRepository.GetFromName<IPermisionClass>("Default"));
        }

        private void UpdateAllExtensions(IEnumerable<IExtension> allExten, IPermisionClass permisionClass)
        {
            UpdatePermissionClassOnExtension(
                    _modelRepository.GetList<IExtension>().Where(e => e.PermisionClass.Id == permisionClass.Id),
                    _modelRepository.GetFromName<IPermisionClass>("Default"));

            UpdatePermissionClassOnExtension(allExten, permisionClass);
        }

        private static void UpdatePermissionClassOnExtension(IEnumerable<IExtension> allExten, IPermisionClass permisionClass)
        {
            foreach (var extension in allExten.Where(extension => extension != null))
            {
                extension.PermisionClass = permisionClass;
            }
        }

        private IEnumerable<IExtension> GetAllExtensions(string extens)
        {
            return extens.Split(',').ToList().Select(s => _modelRepository.GetFromName<IExtension>(s)).ToList();
        }

        private List<IPermissionPattern> GetPatterns(string patterns)
        {
            if (string.IsNullOrEmpty(patterns))
            {
                return new List<IPermissionPattern>();
            }

            var vals = patterns.Split(',');

            return vals.Select(val => _modelRepository.GetFromName<IPermissionPattern>(val)).ToList();
        }

        private void SetClassMembers(List<IPermissionPattern> patterns, IPermisionClass parentClass, IDialplan dialplan)
        {
            if (patterns.Any())
            {
                DeletePermissionClassMembers(parentClass, dialplan.Id);

                foreach (var pattern in patterns)
                {
                    var tempPerClassMem = _modelRepository.Add<IPermissionClassMember>();
                    tempPerClassMem.ParentPermissionClass = parentClass;

                    var defaultDialplan = dialplan;
                    tempPerClassMem.Dialplan = defaultDialplan;
                    tempPerClassMem.Pattern = pattern;
                }
            }
        }

        private static void DeletePermissionClassMembers(IPermisionClass parentClass, int dialplanId)
        {
            foreach (var mem in parentClass.PermissionClassMemebers)
            {
                if (dialplanId != 0)
                {
                    if (mem.Dialplan.Id == dialplanId)
                    {
                        mem.Delete();
                    }
                }
                else
                {
                    mem.Delete();
                }
            }
        }
    }
}