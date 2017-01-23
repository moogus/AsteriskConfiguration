using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  public class ExtensionAdminController : Controller
  {
   private readonly IRepository _modelRepository;

    public ExtensionAdminController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
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
      return View(_modelRepository.GetList<IDefault>().Where(d => d.Type == "Extension").OrderBy(i => i.Index));
    }

    [Authorize(Roles = "admin")]
    public string Add(string number, string password, string notes, string ddi, string cli, string fName, string lName,
                      string dpart, string posititon, string email, string voiceMail, string mailDelay,
                      string permission, string credentialType, string inDirectory)
    {
      if (number != "" && _modelRepository.GetFromName<IExtension>(number) == null &&
          _modelRepository.GetFromName<IQueue>(number) == null)
      {
        var transaction = _modelRepository.ModelTransaction();

        using (transaction)
        {
          var extension = _modelRepository.Add<IExtension>();

          extension.Number = number;
          extension.Password = password;
          extension.Notes = notes;

          extension.DDI = !string.IsNullOrEmpty(ddi) ? _modelRepository.GetFromName<IDDI>(ddi) : null;

          extension.FirstName = fName;
          extension.LastName = lName;
          extension.Department = dpart;
          extension.JobTitle = posititon;
          extension.Email = email;
          extension.PermisionClass = GetPermissionClass(permission);

          extension.CLI = !string.IsNullOrEmpty(cli) ? _modelRepository.GetFromName<ICLI>(cli) : null;

          extension.VoiceMail = _modelRepository.GetFromName<IVoiceMail>(voiceMail);
          extension.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
          extension.IncludeInDirectory = inDirectory.Equals("include");

          var userConfig = _modelRepository.Add<IUserConfig>();
          userConfig.Number = number;
          userConfig.Password = number;
          userConfig.Role = string.IsNullOrEmpty(credentialType) ? "user" : credentialType;

          return transaction.Commit() ? string.Format("added extension {0}", number) : "";
        }
      }
      return "";
    }

    //TODO make these parameters into a viewmodel and pass it in ! Ask Matt how!!!
    [Authorize(Roles = "admin")]
    public string Update(string number, string password, string notes, string ddi, string cli, string fName,
                         string lName, string dpart, string posititon, string email, int id, string voiceMail,
                         string mailDelay, string permission, string credentialType, string inDirectory)
    {
      var transaction = _modelRepository.ModelTransaction();

      using (transaction)
      {
        var extension = _modelRepository.GetFromId<IExtension>(id);        

        extension.Number = number;
        extension.Password = string.IsNullOrEmpty(password) ? number : password;
        extension.Notes = notes;

        extension.DDI = !string.IsNullOrEmpty(ddi) ? _modelRepository.GetFromName<IDDI>(ddi) : null;

        extension.FirstName = fName;
        extension.LastName = lName;
        extension.Department = dpart;
        extension.JobTitle = posititon;
        extension.Email = email;
        extension.PermisionClass = GetPermissionClass(permission);

        extension.CLI = !string.IsNullOrEmpty(cli) ? _modelRepository.GetFromName<ICLI>(cli) : null;

        extension.VoiceMail = _modelRepository.GetFromName<IVoiceMail>(voiceMail);
        extension.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
        extension.IncludeInDirectory = inDirectory.Equals("include");

        var userConfig = _modelRepository.GetFromName<IUserConfig>(number);
        userConfig.Role = credentialType;
        
        return transaction.Commit() ? string.Format("updated extension {0}", number) : "";
      }
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var transaction = _modelRepository.ModelTransaction();

      using (transaction)
      {
        var extension = _modelRepository.GetFromId<IExtension>(id);

        if (extension.Number == User.Identity.Name)
          return "";

        var userConfig = _modelRepository.GetFromName<IUserConfig>(extension.Number);

        RemoveQueueMembersForDeletedExtension(extension);

        extension.Delete();
        userConfig.Delete();

        return transaction.Commit() ? "Deleted extension" + extension.Number : "";
      }
    }


    [Authorize(Roles = "admin,user")]
    public JsonResult ExtensionData()
    {
        IEnumerable<IExtension> validExtensions = ValidExtensions().ToList();
        List<IUserConfig> userCredentials = _modelRepository.GetList<IUserConfig>().ToList();
        var a = new ExtensionsJsonViewModel(validExtensions, userCredentials);
        return Json(a, JsonRequestBehavior.AllowGet);
    }


    public JsonResult ExtensionEmailData()
    {
      return Json(new ExtensionsJsonViewModel(ValidExtensions().Where(e => !string.IsNullOrEmpty(e.Email)),
                                              _modelRepository.GetList<IUserConfig>().ToList()), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult UploadExtensionsByCSV(HttpPostedFileBase file)
    {
      var importData = new ImportDataFromCSV(file, Server);

      var transaction = _modelRepository.ModelTransaction();

      using (transaction)
      {
        Action<DataRow> saveDataRow = (dataRow) =>
          {
            var extension = _modelRepository.Add<IExtension>();
            extension.Number = dataRow[0].ToString();
            extension.Password = dataRow[1].ToString();

            extension.DDI = !string.IsNullOrEmpty(dataRow[2].ToString())
                              ? _modelRepository.GetFromName<IDDI>(dataRow[2].ToString())
                              : null;

            extension.FirstName = dataRow[3].ToString();
            extension.LastName = dataRow[4].ToString();
            extension.Department = dataRow[5].ToString();
            extension.JobTitle = dataRow[6].ToString();
            extension.Email = dataRow[7].ToString();

            var userConfig = _modelRepository.Add<IUserConfig>();
            userConfig.Number = dataRow[0].ToString();
            userConfig.Password = dataRow[0].ToString();
            userConfig.Role = "user";
          };

        var result = RedirectToAction(importData.SaveCSVData(saveDataRow, 8) ? "Extensions" : "CSVError");

        transaction.Commit();

        return result;
      }
    }

    [Authorize(Roles = "admin")]
    public ActionResult CSVError()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public string AddVoiceMail(string id)
    {
      var transaction = _modelRepository.ModelTransaction();

      using (transaction)
      {
        var extn = _modelRepository.GetFromId<IExtension>(int.Parse(id));

        //create a new voicemail based on a default and info from this extension; then add it to this extension.
        var voiceMail = _modelRepository.Add<IVoiceMail>();

        voiceMail.Number = extn.Number;
        voiceMail.Password = "1234";
        voiceMail.NumberOfMessages = 100;
        voiceMail.HeldNumberOfMessages = 100;
        voiceMail.MessageLength = 60;
        voiceMail.DefaultEmail = extn.Email;
        voiceMail.EmailNotificationHasMp3 = false;        

        extn.VoiceMail = voiceMail;
        extn.VoicemailDelay = 10;

        return transaction.Commit() ? string.Format("Added voicemail for {0}", extn.Number) : "";
      }
    }

    //TODO remove this and put somewhere else - maybe the extension model maybe not
    [Authorize(Roles = "admin")]
    private void RemoveQueueMembersForDeletedExtension(IExtension extension)
    {
      foreach (IQueueMember member in
        from queue in _modelRepository.GetList<IQueue>()
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
      var federatedlinks =_modelRepository.GetList<ISamsungFederatedLink>().Where(f => f.Extension != null).ToList();
      var extensionsfromfederations =  federatedlinks.Select(f => f.Extension).ToList();

        var adminExtension = _modelRepository.GetList<IServer>().First().AdminExtension;

      var extensionsExcludingAdmin = _modelRepository.GetList<IExtension>().Where(e => e.Id != adminExtension.Id).ToList();

      if (!extensionsfromfederations.Any())
      {
        return extensionsExcludingAdmin;
      }

      var t= (from extension in extensionsExcludingAdmin
              from e in extensionsfromfederations
              where extension.Id != e.Id
              select extension);

      return new List<IExtension>();
    }

    [Authorize(Roles = "admin")]
    private IPermisionClass GetPermissionClass(string permission)
    {
      return string.IsNullOrEmpty(permission)
               ? _modelRepository.GetFromId<IPermisionClass>(1)
               : _modelRepository.GetFromName<IPermisionClass>(permission);
    }
  }
}