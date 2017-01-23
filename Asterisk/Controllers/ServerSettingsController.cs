using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class ServerSettingsController : Controller
    {
        private readonly IRepository _modelRepository;

        public ServerSettingsController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ServerSettingsData()
        {
            var serverData = _modelRepository.GetList<IServer>();

            return Json(new ServerJsonViewModel(serverData), JsonRequestBehavior.AllowGet);
        }

        public string Update(int id, string ipaddress, string userName, string password, string mailserver, string voiceMail,
                             string admin, string extenIpRange)
        {
            if (!IsValidIpRange(extenIpRange)) return @"The IP address range is invalid please ensure it is formatted '192.168.0.1\255.255.255.0'"; 

            var server = _modelRepository.GetFromId<IServer>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                server.IpAddress = ipaddress;
                server.Credentials = new NetworkCredential(userName, password);
                server.MailServer = new SmtpClient(mailserver);
                server.VoicemailDialNumber = voiceMail;
                server.AdminExtension = _modelRepository.GetFromName<IExtension>(admin) ?? server.AdminExtension;
                server.ExtensionIpRange = extenIpRange;

                return transaction.Commit() ? "Updated settings." : "Failed to update settings.";
            }
        }

        private static bool IsValidIpRange(string extenIpRange)
        {
            return (extenIpRange.Contains(@"\") && extenIpRange.Split('.').Count() == 7);
        }
    }
}