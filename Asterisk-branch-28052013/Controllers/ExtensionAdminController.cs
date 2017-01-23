using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class ExtensionAdminController : Controller
  {
    private readonly IRepository _repository;
    private readonly IExtension _adminExtension;

    public ExtensionAdminController(IRepository repository)
    {
      _repository = repository;
      _adminExtension = _repository.GetList<IServer>().First().AdminExtension;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public ActionResult Extensions()
    {
      return View(_repository.GetList<IDefault>().Where(d => d.Type == "Extension").OrderBy(i => i.Index));
    }

    [Authorize(Roles = "admin")]
    public string Add(string number, string password, string notes, string ddi, string cli, string fName, string lName,
                      string dpart, string posititon, string email, string voiceMail, string mailDelay,
                      string permission, string credentialType, string inDirectory)
    {
      if (number != "" && _repository.GetFromName<IExtension>(number) == null &&
          _repository.GetFromName<IQueue>(number) == null)
      {
        var extension = _repository.Add<IExtension>();

        extension.Number = number;
        extension.Password = password;
        extension.Notes = notes;

        extension.DDI = !string.IsNullOrEmpty(ddi) ? _repository.GetFromName<IDDI>(ddi) : null;

        extension.FirstName = fName;
        extension.LastName = lName;
        extension.Department = dpart;
        extension.JobTitle = posititon;
        extension.Email = email;
        extension.PermisionClass = GetPermissionClass(permission);

        extension.CLI = !string.IsNullOrEmpty(cli) ? _repository.GetFromName<ICLI>(cli) : null;

        extension.VoiceMail = _repository.GetFromName<IVoiceMail>(voiceMail);
        extension.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
        extension.IncludeInDirectory = inDirectory.Equals("include");

        var userConfig = _repository.Add<IUserConfig>();
        userConfig.Number = number;
        userConfig.Password = number;
        userConfig.Role = credentialType;
        var up2 = userConfig.Update();

        var up1 = extension.Update();
        return up1 && up2 ? string.Format("added extension {0}", number) : "";
      }
      return "";
    }

    //TODO make these parameters into a viewmodel and pass it in ! Ask Matt how!!!
    [Authorize(Roles = "admin")]
    public string Update(string number, string password, string notes, string ddi, string cli, string fName,
                         string lName, string dpart, string posititon, string email, int id, string voiceMail,
                         string mailDelay, string permission, string credentialType, string inDirectory)
    {
      IExtension extension = _repository.GetFromId<IExtension>(id);

      extension.Number = number;
      extension.Password = string.IsNullOrEmpty(password) ? number : password;
      extension.Notes = notes;

      extension.DDI = !string.IsNullOrEmpty(ddi) ? _repository.GetFromName<IDDI>(ddi) : null;

      extension.FirstName = fName;
      extension.LastName = lName;
      extension.Department = dpart;
      extension.JobTitle = posititon;
      extension.Email = email;
      extension.PermisionClass = GetPermissionClass(permission);

      extension.CLI = !string.IsNullOrEmpty(cli) ? _repository.GetFromName<ICLI>(cli) : null;

      extension.VoiceMail = _repository.GetFromName<IVoiceMail>(voiceMail);
      extension.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
      extension.IncludeInDirectory = inDirectory.Equals("include");

      var userConfig = _repository.GetFromName<IUserConfig>(number);
      userConfig.Role = credentialType;
      var up2 = userConfig.Update();

      var up1 = extension.Update();
      return up1 && up2 ? string.Format("updated extension {0}", number) : "";
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var extension = _repository.GetFromId<IExtension>(id);

      if (extension.Number == User.Identity.Name)
        return "";

      var userConfig = _repository.GetFromName<IUserConfig>(extension.Number);
      RemoveQueueMembersForDeletedExtension(extension);

      return userConfig.Delete() && extension.Delete() ? "Deleted extension " + extension.Number : "";
    }


    [Authorize(Roles = "admin,user")]
    public JsonResult ExtensionData()
    {
      return Json(new ExtensionsJsonViewModel(ValidExtensions(), _repository.GetList<IUserConfig>().ToList()),
                  JsonRequestBehavior.AllowGet);
    }


    public JsonResult ExtensionEmailData()
    {
      return Json(new ExtensionsJsonViewModel(ValidExtensions().Where(e => !string.IsNullOrEmpty(e.Email)),
                                              _repository.GetList<IUserConfig>().ToList()), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult UploadExtensionsByCSV(HttpPostedFileBase file)
    {
      var importData = new ImportDataFromCSV(file, Server);
      Action<DataRow> s = (dataRow) =>
        {
          var extension = _repository.Add<IExtension>();
          extension.Number = dataRow[0].ToString();
          extension.Password = dataRow[1].ToString();

          extension.DDI = !string.IsNullOrEmpty(dataRow[2].ToString())
                            ? _repository.GetFromName<IDDI>(dataRow[2].ToString())
                            : null;

          extension.FirstName = dataRow[3].ToString();
          extension.LastName = dataRow[4].ToString();
          extension.Department = dataRow[5].ToString();
          extension.JobTitle = dataRow[6].ToString();
          extension.Email = dataRow[7].ToString();

          var userConfig = _repository.Add<IUserConfig>();
          userConfig.Number = dataRow[0].ToString();
          userConfig.Password = dataRow[0].ToString();
          userConfig.Role = "user";


          extension.Update();
          userConfig.Update();
        };

      return RedirectToAction(importData.SaveCSVData(s, 8) ? "Extensions" : "CSVError");
    }

    [Authorize(Roles = "admin")]
    public ActionResult CSVError()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public string AddVoiceMail(string id)
    {
      var extn = _repository.GetFromId<IExtension>(int.Parse(id));

      //create a new voicemail based on a default and info from this extension; then add it to this extension.
      var voiceMail = _repository.Add<IVoiceMail>();

      voiceMail.Number = extn.Number;
      voiceMail.Password = "1234";
      voiceMail.NumberOfMessages = 100;
      voiceMail.HeldNumberOfMessages = 100;
      voiceMail.MessageLength = 60;
      voiceMail.DefaultEmail = extn.Email;
      voiceMail.EmailNotificationHasMp3 = false;
      voiceMail.Update();

      extn.VoiceMail = voiceMail;
      extn.VoicemailDelay = 10;
      return extn.Update() ? string.Format("added voice-mail for {0}", extn.Number) : "";
    }

    //TODO remove this and put somewhere else - maybe the extension model maybe not
    [Authorize(Roles = "admin")]
    private void RemoveQueueMembersForDeletedExtension(IExtension extension)
    {
      foreach (var member in
        from queue in _repository.GetList<IQueue>()
        from qm in queue.QueueMembers
        where qm.Extension != null && qm.Extension.Id == extension.Id
        select qm)
      {
        member.Delete();
      }
    }

    [Authorize(Roles = "admin")]
    private IEnumerable<IExtension> ValidExtensions()
    {
      var federatedExtensions =
        _repository.GetList<IFederation>().Where(f => f.Extension != null).Select(f => f.Extension).ToList();
      var extensions = _repository.GetList<IExtension>().Where(e => e.Id != _adminExtension.Id);

      if (!federatedExtensions.Any())
      {
        return extensions;
      }

      return (from extension in extensions
              from e in federatedExtensions
              where extension.Id != e.Id
              select extension);
    }

    [Authorize(Roles = "admin")]
    private IPermisionClass GetPermissionClass(string permission)
    {
      return string.IsNullOrEmpty(permission)
               ? _repository.GetFromId<IPermisionClass>(1)
               : _repository.GetFromName<IPermisionClass>(permission);
    }
  }
}