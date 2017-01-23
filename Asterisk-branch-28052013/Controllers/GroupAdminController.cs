using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using DatabaseAccess;
using DatabaseAccess.ModelUtilities.QueueStrategy;

namespace Asterisk.Controllers
{
  public class GroupAdminController : Controller
  {
    private readonly IRepository _repository;

    public GroupAdminController(IRepository repository)
    {
      _repository = repository;
    }

    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return View(_repository.GetList<IDefault>().Where(d => d.Type == "Queue").OrderBy(i => i.Index));
    }

    [Authorize(Roles = "admin")]
    public string Add(string number, string notes, string qName, string strategy, string ringOnBusy, string voiceMail,
                      string mailDelay, string moh, string inDirectory)
    {
      if (number != "" && _repository.GetFromName<IQueue>(number) == null &&
          _repository.GetFromName<IExtension>(number) == null)
      {
        var queue = _repository.Add<IQueue>();

        queue.Number = number;

        queue.Notes = notes;
        queue.QueueName = qName;
        queue.Strategy = strategy.ToQueueStrategy();
        queue.VoiceMail = _repository.GetFromName<IVoiceMail>(voiceMail);
        queue.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
        queue.MusicOnHold =
          _repository.GetFromName<IMusicOnHold>(moh.Trim().Equals("No Music On Hold") || string.IsNullOrEmpty(moh)
                                                  ? "Default"
                                                  : moh);
        queue.IncludeInDirectory = inDirectory.Equals("include");
        queue.RingOnBusy = ringOnBusy == "yes";

        queue.Update();

        return "Added " + queue.Number;
      }

      return "";
    }

    [Authorize(Roles = "admin")]
    public string Update(int id, string number, string notes, string qName, string strategy, string ringOnBusy,
                         string voiceMail, string mailDelay, string moh, string inDirectory)
    {
      var queue = _repository.GetFromId<IQueue>(id);
      queue.Number = number;
      queue.Notes = notes;

      queue.QueueName = qName;
      queue.Strategy = strategy.ToQueueStrategy();
      queue.RingOnBusy = ringOnBusy == "yes";
      queue.VoiceMail = _repository.GetFromName<IVoiceMail>(voiceMail);
      queue.VoicemailDelay = !string.IsNullOrEmpty(mailDelay) ? int.Parse(mailDelay) : 0;
      queue.MusicOnHold =
        _repository.GetFromName<IMusicOnHold>(moh.Trim().Equals("No Music On Hold") || string.IsNullOrEmpty(moh)
                                                ? "Default"
                                                : moh);
      queue.IncludeInDirectory = inDirectory.Equals("include");
      queue.Update();

      return "Done";
    }

    [Authorize(Roles = "admin")]
    public string Delete(int id)
    {
      var queue = _repository.GetFromId<IQueue>(id);

      RemoveQueueMembersForDeletedQueue(queue);

      return queue.Delete() ? "Deleted queue " + queue.Number : "Couldn't delete queue";
    }

    [Authorize(Roles = "admin,user")]
    public JsonResult QueueData()
    {
      var queues = _repository.GetList<IQueue>();
      return Json(new QueuesJsonViewModel(queues), JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "admin")]
    public string AddVoiceMail(string id)
    {
      var queue = _repository.GetFromId<IQueue>(int.Parse(id));

      //create a new voicemail based on a default and info from this queue; then add it to this queue.
      var voiceMail = _repository.Add<IVoiceMail>();

      voiceMail.Number = queue.Number;
      voiceMail.Password = "1234";
      voiceMail.NumberOfMessages = 100;
      voiceMail.HeldNumberOfMessages = 100;
      voiceMail.MessageLength = 60;
      voiceMail.DefaultEmail = "";
      voiceMail.EmailNotificationHasMp3 = false;
      voiceMail.Update();

      queue.VoiceMail = voiceMail;
      queue.VoicemailDelay = 10;
      queue.Update();

      return id;
    }

    [Authorize(Roles = "admin")]
    private void RemoveQueueMembersForDeletedQueue(IQueue queue)
    {
      foreach (var q in _repository.GetList<IQueue>())
      {
        var queueMember = q.QueueMembers.Where(qm => qm.Queue != null && qm.Queue.Number == queue.Number);
        foreach (var member in queueMember)
        {
          member.Delete();
        }
      }
    }
  }
}