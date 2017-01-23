using System.Net.Mime;
using System.Web.Mvc;
using Asterisk.Utilities;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class VoiceMessagesController : Controller
  {
    private readonly IRepository _repository;
    private readonly IAppSettings _settings;

    public VoiceMessagesController(IRepository repository, IAppSettings settings)
    {
      _repository = repository;
      _settings = settings;
    }

    public ActionResult Index(string extn, bool isAdminEdit, string lastFolder)
    {
      var mailBox = new MessageViewModel
        {
          Extension = extn,
          VoiceMail = _repository.GetFromName<IVoiceMail>(extn),
          LastFolder = lastFolder
        };
      return View(mailBox);
    }

    public ActionResult Play(int id)
    {
      var cd = new ContentDisposition
        {
          Inline = true,
          FileName = "message.wav"
        };
      Response.AppendHeader("Content-Disposition", cd.ToString());

      return File(_repository.GetFromId<IVoiceMessage>(id).Audiostream.Stream, "audio/wav");
    }

    public ActionResult Download(int id)
    {
      var cd = new ContentDisposition
        {
          Inline = false,
          FileName = "message.wav"
        };
      Response.AppendHeader("Content-Disposition", cd.ToString());

      return File(_repository.GetFromId<IVoiceMessage>(id).Audiostream.Stream, "audio/wav");
    }

    public string Forward(int id, string emailTo, string messageBody)
    {
      return
        _repository.GetFromId<IVoiceMessage>(id)
                   .Audiostream.ForwardMail(_repository.GetFromName<IExtension>(User.Identity.Name),
                                            emailTo, messageBody, _settings.MailServer)
          ? "message sent"
          : "something went wrong";
    }

    public string MoveFolder(int id, string folder)
    {
      var message = _repository.GetFromId<IVoiceMessage>(id);
      message.Folder = folder;
      return message.Update() ? "moved" : "not moved";
    }
  }
}