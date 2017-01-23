using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize]
    public class VoiceMailController : Controller
    {
        private readonly IRepository _modelRepository;

        public VoiceMailController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult VoiceMailConfig()
        {
            return View(_modelRepository.GetList<IDefault>().Where(d => d.Type == "VoiceMail").OrderBy(i => i.Index));
        }

        public string Add(string number, string password, string canRecord, string maxMessages, string messageLength,
                          string defaultEmail, string hasMp3)
        {
            if (number == "") return "The number was invalid.";
            if (_modelRepository.GetFromName<IVoiceMail>(number) != null) return "The number is already in use.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var voiceMail = _modelRepository.Add<IVoiceMail>();

                voiceMail.Number = number;
                voiceMail.Password = password;
                voiceMail.HeldNumberOfMessages = ConvertStringToInt(maxMessages);
                voiceMail.NumberOfMessages = canRecord == "yes" ? voiceMail.HeldNumberOfMessages : 0;
                voiceMail.MessageLength = ConvertStringToInt(messageLength);
                voiceMail.DefaultEmail = defaultEmail;
                voiceMail.EmailNotificationHasMp3 = hasMp3 == "yes";

                return transaction.Commit() ? "Voicemail added." : "Failed to add voicemail.";
            }            
        }

        public string Update(int id, string number, string password, string canRecord, string maxMessages,
                             string messageLength, string defaultEmail, string hasMp3)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var voiceMail = _modelRepository.GetFromId<IVoiceMail>(id);

                voiceMail.Number = number;
                voiceMail.Password = password;
                voiceMail.HeldNumberOfMessages = ConvertStringToInt(maxMessages);
                voiceMail.NumberOfMessages = canRecord == "yes" ? voiceMail.HeldNumberOfMessages : 0;
                voiceMail.MessageLength = ConvertStringToInt(messageLength);
                voiceMail.DefaultEmail = defaultEmail;
                voiceMail.EmailNotificationHasMp3 = hasMp3 == "yes";

                return transaction.Commit() ? "Voicemail updated." : "Failed to update voicemail.";
            }
        }

        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var voiceMail = _modelRepository.GetFromId<IVoiceMail>(id);

                RemoveVoiceMailFromExtensionsAndQueues(id);
                RemoveVoiceMessagesForDeletedMialBox(id);

                voiceMail.Delete();

                return transaction.Commit() ? "Deleted voicemail." : "Failed to delete voicemail.";
            }
        }

        private void RemoveVoiceMessagesForDeletedMialBox(int voiceMailId)
        {
            var messages = _modelRepository.GetList<IVoiceMessage>().Where(m => m.MailBox.Id == voiceMailId);

            foreach (var message in messages)
            {
                message.Delete();                
            }            
        }

        public JsonResult VoiceMailData()
        {
            return Json(new VoiceMailJsonViewModel(_modelRepository.GetList<IVoiceMail>()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AvailableVoiceMails()
        {
            return Json(_modelRepository.GetList<IVoiceMail>().OrderBy(v => v.Number).Select(FormatVoiceMailDetails), JsonRequestBehavior.AllowGet);
        }

        private void RemoveVoiceMailFromExtensionsAndQueues(int id)
        {
            var extensions = _modelRepository.GetList<IExtension>().Where(ex => ex.VoiceMail != null && ex.VoiceMail.Id == id);

            foreach (var extension in extensions)
            {
                extension.VoiceMail = null;
            }

            var queues = _modelRepository.GetList<IQueue>().Where(qu => qu.VoiceMail != null && qu.VoiceMail.Id == id);

            foreach (var queue in queues)
            {
                queue.VoiceMail = null;
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