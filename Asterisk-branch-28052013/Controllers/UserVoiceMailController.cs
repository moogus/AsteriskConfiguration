using System.Linq;
using System.Web.Mvc;
using AMIWrapper;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserVoiceMailController : Controller
  {
    private readonly IRepository _repository;

    public UserVoiceMailController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string extn, bool isAdminEdit)
    {
      var extension = _repository.GetFromName<IExtension>(extn);
      if (extension.VoiceMail != null)
      {
        return View(new UserVoiceMailViewModel(extension));
      }
      return View("NoVoiceMail");
    }

    [HttpPost]
    public ActionResult Index(UserVoiceMailViewModel vm)
    {
      var extension = _repository.GetFromId<IExtension>(vm.Id);
      var voiceMail = extension.VoiceMail;

      extension.VoicemailDelay = vm.MailDelay;
      var update1 = extension.Update();

      voiceMail.DefaultEmail = vm.DefaultEmail;
      voiceMail.EmailNotificationHasMp3 = vm.IncludeMp3;
      voiceMail.Password = vm.VoicePassword;
      var update2 = voiceMail.Update();

      TempData["message"] = update1 && update2 ? "Your voice-mail has been updated." : "Something went wrong.";

      return RedirectToAction("Index", "UserConfigHome", new {extn = extension.Number});
    }

    public string CallForAudio(string extension)
    {
      var makeCall = new Connector(_repository.GetList<IServer>().First().IpAddress);
      makeCall.Connect();
      var rtn = makeCall.CallAsterisk(extension, "soundName", extension, "MakeVoiceMailGreeting")
                  ? "calling" + extension
                  : "";
      makeCall.Disconnect();

      return rtn;
    }
  }
}