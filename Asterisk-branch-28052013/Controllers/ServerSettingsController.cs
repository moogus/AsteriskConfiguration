using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class ServerSettingsController : Controller
  {
    private readonly IRepository _repository;

    public ServerSettingsController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public JsonResult ServerSettingsData()
    {
      var serverData = _repository.GetList<IServer>();
      return Json(new ServerJsonViewModel(serverData), JsonRequestBehavior.AllowGet);
    }

    public string Update(int id, string ipaddress, string userName, string password, string mailserver, string voiceMail,
                         string admin, string extenIpRange)
    {
      var server = _repository.GetFromId<IServer>(id);

      server.IpAddress = ipaddress;
      server.Credentials = new NetworkCredential(userName, password);
      server.MailServer = new SmtpClient(mailserver);
      server.VoicemailDialNumber = voiceMail;
      server.AdminExtension = _repository.GetFromName<IExtension>(admin) ?? server.AdminExtension;

      if (IsValidIpRange(extenIpRange))
      {
        server.ExtensionIpRange = extenIpRange;
        return server.Update() ? "server details updated" : "something went wrong";
      }
      return "something went wrong";
    }

    private static bool IsValidIpRange(string extenIpRange)
    {
      return (extenIpRange.Contains(@"\") && extenIpRange.Split('.').Count() == 8);
    }
  }
}