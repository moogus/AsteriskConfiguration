using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ModelAccess.Models;

namespace UserConfiguration.Controllers
{
  public class UserVoiceMailController : Controller
  {
    private readonly Repository<IExtension> _extensionContext;
    public UserVoiceMailController()
    {
      _extensionContext = new Repository<IExtension>();
    }

    public ActionResult Index()
    {
      var uVMvm = new UserVoiceMailViewModel { Extension = _extensionContext.GetFromName(User.Identity.Name), MailDelay = SetMailDelayValues() };
      return View(uVMvm);
    }

    private static Dictionary<int, string> SetMailDelayValues()
    {
      return new Dictionary<int, string> {
                                          {0, "Voice-Mail off"},
                                          {5,"5 second delay"},
                                          {10, "10 second delay"},
                                          {15, "15 second delay"},
                                          {20, "20 second delay"},
                                          {25, "25 second delay"},
                                          {30, "30 second delay"}
      };
    }

    [HttpPost]
    public ActionResult Index(int id, int mailDelay, string defaultEmail, List<int> includeMp3)
    {
      var extension = _extensionContext.GetFromId(id);
      var voiceMail = extension.VoiceMail;

      extension.VoicemailDelay = mailDelay;
      var update1 = extension.Update();

      voiceMail.DefaultEmail = defaultEmail;
      voiceMail.EmailNotificationHasMp3 = (includeMp3 != null);
      var update2 = voiceMail.Update();

      TempData["message"]= update1 && update2 ? "Your voice-mail has been updated." : "Something went wrong.";

      return RedirectToAction("Index", "UserConfigHome");
    }
 
  }
  
  public class UserVoiceMailViewModel
  {
    public IExtension Extension { get; set; }
    public Dictionary<int, string> MailDelay { get; set; }
    public string Status { get; set; }
  }
}
