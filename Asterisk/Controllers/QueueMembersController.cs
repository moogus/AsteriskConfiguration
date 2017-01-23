using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class QueueMembersController : Controller
    {
        private readonly IRepository _modelRepository;

        public QueueMembersController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(int id)
        {
            var queue = _modelRepository.GetFromId<IQueue>(id);

            IOrderedEnumerable<IExtension> extensions = _modelRepository.GetList<IExtension>()
                                                                   .Where(
                                                                     e =>
                                                                     queue.QueueMembers.Count(
                                                                       qm => qm.Type == QueueMemberType.Extension
                                                                             && qm.Extension.Id == e.Id) == 0)
                                                                   .OrderBy(e => e.Number);

            IOrderedEnumerable<IQueue> queues = _modelRepository.GetList<IQueue>()
                                                           .Where(
                                                             q =>
                                                             queue.QueueMembers.Count(qm => qm.Type == QueueMemberType.Queue
                                                                                            && qm.Queue.Number == q.Number) ==
                                                             0)
                                                           .OrderBy(q => q.Number);

            var state = string.IsNullOrEmpty((string)TempData["state"]) ? string.Empty : (string)TempData["state"];

            var qVm = new QueueMemberViewModel
              {
                  Queue = queue,
                  ListOfExtensions = extensions,
                  ListOfQueues = queues,
                  Status = state
              };

            return View(qVm);
        }

        public ActionResult AddExtensionsGroup(List<int> selectedExtensionId, string queueNumber)
        {
            var queue = _modelRepository.GetFromName<IQueue>(queueNumber);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                foreach (var id in selectedExtensionId)
                {
                    var extension = _modelRepository.GetFromId<IExtension>(id);

                    queue.AddExtensionAsQueueMember(extension, queue.Id);
                }

                TempData["state"] = transaction.Commit() ? "Added extension group." : "Failed to add extension group.";

                return RedirectToAction("Index", new {id = queue.Id});
            }
        }


        public ActionResult AddQueuesGroup(List<string> selectedQueueId, string queueNumber)
        {
            var queue = _modelRepository.GetFromName<IQueue>(queueNumber);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                foreach (var number in selectedQueueId)
                {
                    var queueToAdd = _modelRepository.GetFromName<IQueue>(number);
                    queue.AddQueueAsQueueMember(queueToAdd, queue.Id);
                }

                TempData["state"] = transaction.Commit() ? "Added queue group." : "Failed to add queue group.";

                return RedirectToAction("Index", new {id = queue.Id});
            }
        }

        public string UpdateQueueMember(string queueNumber, int queueMemeberId, int penalty, string paused)
        {
            var queueMemeber = _modelRepository.GetFromName<IQueue>(queueNumber).QueueMembers.First(qm => qm.Id == queueMemeberId);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                queueMemeber.Penalty = penalty;
                queueMemeber.Paused = paused == "yes" ? 1 : 0;

                return transaction.Commit() ? "Updated queue member." : "Failed to update queue member.";
            }
        }

        public ActionResult RemoveFromGroup(List<int> queueMemeberId, string queueNumber)
        {
            var queue = _modelRepository.GetFromName<IQueue>(queueNumber);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                foreach (var i in queueMemeberId)
                {
                    var queueMember = queue.QueueMembers.FirstOrDefault(q => q.Id == i);

                    if (queueMember != null) queueMember.Delete();
                }

                TempData["state"] = transaction.Commit() ? "Removed from group." : "Failed to remove from group.";

                return RedirectToAction("Index", new {id = queue.Id});
            }
        }
    }
}