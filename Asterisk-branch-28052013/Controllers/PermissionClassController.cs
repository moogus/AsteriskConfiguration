using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class PermissionClassController : Controller
  {
    private readonly IRepository _repository;

    public PermissionClassController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string dialplan)
    {
      var dP = string.IsNullOrEmpty(dialplan) ? 1 : int.Parse(dialplan);
      return View(new PermissionControllerViewModel(dP, _repository));
    }

    public JsonResult PermissionClassData(string dialplan)
    {
      return
        Json(
          new PermissionClassJsonViewModel(_repository.GetList<IPermisionClass>(),
                                           _repository.GetList<IExtension>().ToList(), int.Parse(dialplan)),
          JsonRequestBehavior.AllowGet);
    }

    public JsonResult AvailablePermissions()
    {
      return Json(_repository.GetList<IPermisionClass>().OrderBy(p => p.Id).Select(p => p.Name),
                  JsonRequestBehavior.AllowGet);
    }

    public string Add(string name, string description, string patterns, string dialplan, string extens)
    {
      var allExten = GetAllExtensions(extens);

      if (name != "" && _repository.GetFromName<IPermisionClass>(name) == null)
      {
        var permissionClass = _repository.Add<IPermisionClass>();
        permissionClass.Name = name;
        permissionClass.Description = description;
        var valid = permissionClass.Update();

        var validMemeber = SetClassMembers(GetPatterns(patterns), permissionClass,
                                           _repository.GetFromId<IDialplan>(int.Parse(dialplan)));

        var validExtensions = UpdateAllExtensions(allExten, permissionClass);

        if (valid && validMemeber && validExtensions)
        {
          return string.Format("{0} Permission Class {1}", "added", name);
        }
      }
      return string.Format("{0} Permission Class {1}", "failed to add", name);
    }

    public string Update(int id, string name, string description, string patterns, string dialplan, string extens)
    {
      var allExten = GetAllExtensions(extens);

      var permissionClass = _repository.GetFromId<IPermisionClass>(id);
      permissionClass.Name = name;
      permissionClass.Description = description;
      var valid = permissionClass.Update();

      var validMemeber = SetClassMembers(GetPatterns(patterns), permissionClass,
                                         _repository.GetFromId<IDialplan>(int.Parse(dialplan)));

      var validExtensions = UpdateAllExtensions(allExten, permissionClass);

      if (valid && validMemeber && validExtensions)
      {
        return string.Format("{0} Permission Class {1}", "updated", name);
      }

      return string.Format("{0} Permission Class {1}", "failed to add", name);
    }

    public string Delete(int id)
    {
      var pattern = _repository.GetFromId<IPermisionClass>(id);

      //delete all class memebers dialplan set to 0 to indicate all
      DeletePermissionClassMembers(pattern, 0);

      //set all the extensions back to default that where in the permission class
      var validExtensions =
        ResetExtensionsInPermissionClass(_repository.GetList<IExtension>().Where(e => e.PermisionClass.Id == pattern.Id));

      return pattern.Delete() && validExtensions ? "Deleted class" + pattern.Name : "Couldn't delete class.";
    }

    private bool ResetExtensionsInPermissionClass(IEnumerable<IExtension> extensions)
    {
      return UpdatePermissionClassOnExtension(extensions, _repository.GetFromName<IPermisionClass>("Default"));
    }

    private bool UpdateAllExtensions(IEnumerable<IExtension> allExten, IPermisionClass permisionClass)
    {
      var extensionsAreOutOfClass =
        UpdatePermissionClassOnExtension(
          _repository.GetList<IExtension>().Where(e => e.PermisionClass.Id == permisionClass.Id),
          _repository.GetFromName<IPermisionClass>("Default"));

      var extensionsAreInNewClass = UpdatePermissionClassOnExtension(allExten, permisionClass);

      return extensionsAreOutOfClass && extensionsAreInNewClass;
    }

    private static bool UpdatePermissionClassOnExtension(IEnumerable<IExtension> allExten,
                                                         IPermisionClass permisionClass)
    {
      var rtn = false;
      foreach (var extension in allExten.Where(extension => extension != null))
      {
        extension.PermisionClass = permisionClass;
        rtn = extension.Update();
      }
      return rtn;
    }

    private IEnumerable<IExtension> GetAllExtensions(string extens)
    {
      return extens.Split(',').ToList().Select(s => _repository.GetFromName<IExtension>(s)).ToList();
    }

    private List<IPattern> GetPatterns(string patterns)
    {
      if (string.IsNullOrEmpty(patterns))
      {
        return new List<IPattern>();
      }

      var vals = patterns.Split(',');
      return vals.Select(val => _repository.GetFromName<IPattern>(val)).ToList();
    }

    private bool SetClassMembers(List<IPattern> patterns, IPermisionClass parentClass, IDialplan dialplan)
    {
      var rtn = true;
      if (patterns.Any())
      {
        DeletePermissionClassMembers(parentClass, dialplan.Id);

        foreach (var pattern in patterns)
        {
          var tempPerClassMem = _repository.Add<IPermissionClassMember>();
          tempPerClassMem.ParentPermissionClass = parentClass;
          var defaultDialplan = dialplan;
          tempPerClassMem.Dialplan = defaultDialplan;
          tempPerClassMem.Pattern = pattern;

          if (rtn)
          {
            rtn = tempPerClassMem.Update();
          }
          else
          {
            tempPerClassMem.Update();
          }
        }
      }
      return rtn;
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