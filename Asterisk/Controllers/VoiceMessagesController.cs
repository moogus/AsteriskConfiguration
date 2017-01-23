using System.Net.Mime;
using System.Web.Mvc;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class VoiceMessagesController : Controller
    {
        private readonly IRepository _modelRepository;
        private readonly IAppSettings _settings;

        public VoiceMessagesController(IRepository modelRepository, IAppSettings settings)
        {
            _modelRepository = modelRepository;
            _settings = settings;
        }

        public ActionResult Index(string extn, bool isAdminEdit, string lastFolder)
        {
            var mailBox = new MessageViewModel
              {
                  Extension = extn,
                  VoiceMail = _modelRepository.GetFromName<IVoiceMail>(extn),
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

            return File(_modelRepository.GetFromId<IVoiceMessage>(id).Audiostream, "audio/wav");
        }

        public ActionResult Download(int id)
        {
            var cd = new ContentDisposition
              {
                  Inline = false,
                  FileName = "message.wav"
              };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(_modelRepository.GetFromId<IVoiceMessage>(id).Audiostream, "audio/wav");
        }

        public string Forward(int id, string emailTo, string messageBody)
        {
            return
              _modelRepository.GetFromId<IVoiceMessage>(id).ForwardMail(_modelRepository.GetFromName<IExtension>(User.Identity.Name),
                                                  emailTo, messageBody, _settings.MailServer)
                ? "message sent"
                : "something went wrong";
        }

        public string MoveFolder(int id, string folder)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var message = _modelRepository.GetFromId<IVoiceMessage>(id);
                message.Folder.FolderName = folder;
                return transaction.Commit() ? "moved" : "not moved";
            }
        }
    }
}