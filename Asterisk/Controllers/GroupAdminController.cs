using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class GroupAdminController : Controller
    {
        private readonly IRepository _modelRepository;

        public GroupAdminController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(_modelRepository.GetList<IDefault>().Where(d => d.Type == "Queue").OrderBy(i => i.Index));
        }

        [Authorize(Roles = "admin")]
        public string Add(string number, string notes, string qName, string strategy, string ringOnBusy, string voiceMail,
                          string mailDelay, string moh, string inDirectory)
        {
            if (number == "") return "Please specify a valid number.";
            if (_modelRepository.GetFromName<IQueue>(number) != null) return "The specified number is already in use by a queue.";
            if (_modelRepository.GetFromName<IExtension>(number) != null) return "The specified number is already in use by an extension.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var queue = _modelRepository.Add<IQueue>();

                queue.Number = number;
                queue.Notes = notes;
                queue.QueueName = qName;

           
                queue.Strategy =  GetStrategyEnumStrin(strategy);

                queue.VoiceMail = _modelRepository.GetFromName<IVoiceMail>(voiceMail);
                queue.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
                queue.MusicOnHold =
                  _modelRepository.GetFromName<IMusicOnHold>(moh.Trim().Equals("No Music On Hold") || string.IsNullOrEmpty(moh)
                                                          ? "Default"
                                                          : moh);
                queue.IncludeInDirectory = inDirectory.Equals("include");
                queue.RingOnBusy = ringOnBusy == "yes";

                return transaction.Commit() ? "Added queue." : "Failed to add queue.";
            }
        }

        [Authorize(Roles = "admin")]
        public string Update(int id, string number, string notes, string qName, string strategy, string ringOnBusy,
                             string voiceMail, string mailDelay, string moh, string inDirectory)
        {
            if (number == "") return "Please specify a valid number.";
            if (_modelRepository.GetFromName<IQueue>(number) == null) return "Failed to update queue.";
            if (_modelRepository.GetFromName<IExtension>(number) != null) return "The specified number is already in use by an extension.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var queue = _modelRepository.GetFromId<IQueue>(id);
                queue.Number = number;
                queue.Notes = notes;
                queue.QueueName = qName;

                queue.Strategy = GetStrategyEnumStrin(strategy);

                queue.RingOnBusy = ringOnBusy == "yes";
                queue.VoiceMail = _modelRepository.GetFromName<IVoiceMail>(voiceMail);
                queue.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
                queue.MusicOnHold =
                    _modelRepository.GetFromName<IMusicOnHold>(moh.Trim().Equals("No Music On Hold") ||
                                                               string.IsNullOrEmpty(moh)
                                                                   ? "Default"
                                                                   : moh);
                queue.IncludeInDirectory = inDirectory.Equals("include");

                return transaction.Commit() ? "Updated queue." : "Failed to update queue.";
            }
        }

        [Authorize(Roles = "admin")]
        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var queue = _modelRepository.GetFromId<IQueue>(id);

                RemoveQueueMembersForDeletedQueue(queue);

                queue.Delete();

                return transaction.Commit() ? "Deleted queue." : "Failed to delete queue.";
            }
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult QueueData()
        {
            var queues = _modelRepository.GetList<IQueue>().ToList();

            return Json(new QueuesJsonViewModel(queues), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        public string AddVoiceMail(string id)
        {
            var queue = _modelRepository.GetFromId<IQueue>(int.Parse(id));

            if (queue == null) return "Unable to add voicemail to queue. The queue is no longer valid.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var voiceMail = _modelRepository.Add<IVoiceMail>();

                voiceMail.Number = queue.Number;
                voiceMail.Password = "1234";
                voiceMail.NumberOfMessages = 100;
                voiceMail.HeldNumberOfMessages = 100;
                voiceMail.MessageLength = 60;
                voiceMail.DefaultEmail = "";
                voiceMail.EmailNotificationHasMp3 = false;

                queue.VoiceMail = voiceMail;
                queue.VoicemailDelay = 10;

                return transaction.Commit() ? "Voicemail has been added to the queue." : "Failed to add voicemail to the queue.";
            }
        }

        [Authorize(Roles = "admin")]
        private void RemoveQueueMembersForDeletedQueue(IQueue queue)
        {
            var queues = _modelRepository.GetList<IQueue>();
            {
                foreach (var nextQueue in queues)
                {
                    var queueMember = nextQueue.QueueMembers.Where(qm => qm.Queue != null && qm.Queue.Number == queue.Number);

                    foreach (var member in queueMember)
                    {
                        member.Delete();
                    }
                }
            }
        }

        private static QueueStrategy GetStrategyEnumStrin(string strategy)
        {
            if (strategy.ToLower().StartsWith("least"))
            {
                return QueueStrategy.Leastrecent;
            }
            if (strategy.ToLower().StartsWith("fewest"))
            {
                return QueueStrategy.Fewestcalls;
            }
            if (strategy.ToLower().StartsWith("random"))
            {
                return QueueStrategy.Random;
            }
            if (strategy.ToLower().StartsWith("round"))
            {
                return QueueStrategy.Rrmemory;
            }
            if (strategy.ToLower().StartsWith("linear"))
            {
                return QueueStrategy.Linear;
            }
            return strategy.ToLower().StartsWith("weight") ? QueueStrategy.Wrandom : QueueStrategy.Ringall;
        }
    }
}