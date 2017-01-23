using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize]
  public class VoiceMailController : Controller
  {
    private readonly Repository _repository;

    public VoiceMailController(Repository repository)
    {
      _repository = repository;
    }

    public ActionResult VoiceMailConfig()
    {
      return View(_repository.GetList<IDefault>().Where(d => d.Type == "VoiceMail").OrderBy(i => i.Index));
    }

    public string Add(string number, string password, string canRecord, string maxMessages, string messageLength,
                      string defaultEmail, string hasMp3)
    {
      if (number != "" && _repository.GetFromName<IVoiceMail>(number) == null)
      {
        var voiceMail = _repository.Add<IVoiceMail>();
        voiceMail.Number = number;
        voiceMail.Password = password;
        voiceMail.HeldNumberOfMessages = ConvertStringToInt(maxMessages);
        voiceMail.NumberOfMessages = canRecord == "yes" ? voiceMail.HeldNumberOfMessages : 0;
        voiceMail.MessageLength = ConvertStringToInt(messageLength);
        voiceMail.DefaultEmail = defaultEmail;
        voiceMail.EmailNotificationHasMp3 = hasMp3 == "yes";


        return voiceMail.Update() ? "Added" + voiceMail.Number : "";
      }
      return "";
    }

    public string Update(int id, string number, string password, string canRecord, string maxMessages,
                         string messageLength, string defaultEmail, string hasMp3)
    {
      var voiceMail = _repository.GetFromId<IVoiceMail>(id);
      voiceMail.Number = number;
      voiceMail.Password = password;
      voiceMail.HeldNumberOfMessages = ConvertStringToInt(maxMessages);
      voiceMail.NumberOfMessages = canRecord == "yes" ? voiceMail.HeldNumberOfMessages : 0;
      voiceMail.MessageLength = ConvertStringToInt(messageLength);
      voiceMail.DefaultEmail = defaultEmail;
      voiceMail.EmailNotificationHasMp3 = hasMp3 == "yes";

      return voiceMail.Update() ? string.Format("updated voice-mail {0}", number) : "";
    }

    public string Delete(int id)
    {
      var voiceMail = _repository.GetFromId<IVoiceMail>(id);

      RemoveVoiceMailFromExtensionsAndQueues(id);
      var deleteMessages = RemoveVoiceMessagesForDeletedMialBox(id);

      return voiceMail.Delete() && deleteMessages ? "Deleted voice-mail " + voiceMail.Number : "";
    }

    private bool RemoveVoiceMessagesForDeletedMialBox(int voiceMailId)
    {
      var rtn = false;
      var messages = _repository.GetList<IVoiceMessage>().Where(m => m.MailBox.Id == voiceMailId);
      foreach (var m in messages)
      {
        rtn = m.Delete();
      }
      return rtn;
    }

    public JsonResult VoiceMailData()
    {
      return Json(new VoiceMailJsonViewModel(_repository.GetList<IVoiceMail>()), JsonRequestBehavior.AllowGet);
    }

    public JsonResult AvailableVoiceMails()
    {
      return Json(_repository.GetList<IVoiceMail>().OrderBy(v => v.Number).Select(FormatVoiceMailDetails),
                  JsonRequestBehavior.AllowGet);
    }

    private void RemoveVoiceMailFromExtensionsAndQueues(int id)
    {
      var e = _repository.GetList<IExtension>().Where(ex => ex.VoiceMail != null && ex.VoiceMail.Id == id);
      var q = _repository.GetList<IQueue>().Where(qu => qu.VoiceMail != null && qu.VoiceMail.Id == id);
      foreach (var extension in e)
      {
        extension.VoiceMail = null;
        extension.Update();
      }
      foreach (var queue in q)
      {
        queue.VoiceMail = null;
        queue.Update();
      }
    }

    private string FormatVoiceMailDetails(IVoiceMail voiceMail)
    {
      return string.Format("{0}", voiceMail.Number);
    }

    private int ConvertStringToInt(string intString)
    {
      return string.IsNullOrEmpty(intString) ? 0 : int.Parse(intString);
    }
  }
}