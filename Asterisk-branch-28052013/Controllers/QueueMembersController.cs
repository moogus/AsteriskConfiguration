using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class QueueMembersController : Controller
  {
    private readonly IRepository _repository;

    public QueueMembersController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(int id)
    {
      var queue = _repository.GetFromId<IQueue>(id);

      var extensions = _repository.GetList<IExtension>()
                                  .Where(e => queue.QueueMembers.Count(qm => qm.Type == QueueMemberType.Extension
                                                                             && qm.Extension.Id == e.Id) == 0)
                                  .OrderBy(e => e.Number);

      var queues = _repository.GetList<IQueue>()
                              .Where(q => queue.QueueMembers.Count(qm => qm.Type == QueueMemberType.Queue
                                                                         && qm.Queue.Number == q.Number) == 0)
                              .OrderBy(q => q.Number);

      var state = string.IsNullOrEmpty((string) TempData["state"]) ? string.Empty : (string) TempData["state"];

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
      var queue = _repository.GetFromName<IQueue>(queueNumber);

      foreach (var id in selectedExtensionId)
      {
        var extension = _repository.GetFromId<IExtension>(id);
        queue.AddExtensionAsQueueMember(extension, queue.Id);
      }

      TempData["state"] = "added";

      return RedirectToAction("Index", new {id = queue.Id});
    }


    public ActionResult AddQueuesGroup(List<string> selectedQueueId, string queueNumber)
    {
      var queue = _repository.GetFromName<IQueue>(queueNumber);

      foreach (var number in selectedQueueId)
      {
        var queueToAdd = _repository.GetFromName<IQueue>(number);
        queue.AddQueueAsQueueMember(queueToAdd, queue.Id);
      }

      TempData["state"] = "added";

      return RedirectToAction("Index", new {id = queue.Id});
    }

    public string UpdateQueueMember(string queueNumber, int queueMemeberId, int penalty, string paused)
    {
      var queueMemeber = _repository.GetFromName<IQueue>(queueNumber).QueueMembers.First(qm => qm.Id == queueMemeberId);

      queueMemeber.Penalty = penalty;
      queueMemeber.Paused = paused == "yes" ? 1 : 0;

      queueMemeber.Update();

      return "Status updated";
    }

    public ActionResult RemoveFromGroup(List<int> queueMemeberId, string queueNumber)
    {
      var queue = _repository.GetFromName<IQueue>(queueNumber);

      foreach (var i in queueMemeberId)
      {
        var queueMember = queue.QueueMembers.FirstOrDefault(q => q.Id == i);
        if (queueMember != null)
          queueMember.Delete();
      }

      TempData["state"] = "removed";

      return RedirectToAction("Index", new {id = queue.Id});
    }
  }
}