using System.Linq;
using System.Web.Mvc;
using AMIWrapper;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize]
    public class UserVoiceMailController : Controller
    {
        private readonly IRepository _modelRepository;

        public UserVoiceMailController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string extn, bool isAdminEdit)
        {
            var extension = _modelRepository.GetFromName<IExtension>(extn);

            if (extension.VoiceMail != null)
            {
                return View(new UserVoiceMailViewModel(extension));
            }

            return View("NoVoiceMail");
        }

        [HttpPost]
        public ActionResult Index(UserVoiceMailViewModel vm)
        {
            var extension = _modelRepository.GetFromId<IExtension>(vm.Id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                extension.VoicemailDelay = vm.MailDelay;

                extension.VoiceMail.DefaultEmail = vm.DefaultEmail;
                extension.VoiceMail.EmailNotificationHasMp3 = vm.IncludeMp3;
                extension.VoiceMail.Password = vm.VoicePassword;

                TempData["message"] = transaction.Commit()
                                          ? "Your voicemail has been updated."
                                          : "Failed to update voicemail.";                
            }

            return RedirectToAction("Index", "UserConfigHome", new { extn = extension.Number });
        }

        public string CallForAudio(string extension)
        {
            var makeCall = new Connector(_modelRepository.GetList<IServer>().First().IpAddress);

            makeCall.Connect();

            var rtn = makeCall.CallAsterisk(extension, "soundName", extension, "MakeVoiceMailGreeting")
                           ? "calling" + extension
                           : "";

            makeCall.Disconnect();

            return rtn;
        }
    }
}