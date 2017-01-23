using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class Queue : IQueue, IModel
  {
    private readonly ComQueue _under;
    private readonly ISessionWrapper _session;
    private readonly IRepository _repository;
    private List<IQueueMember> _queueMembers;

    internal Queue(ComQueue under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal Queue(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new ComQueue();
      _queueMembers = new List<IQueueMember>();
    }

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
      if (!string.IsNullOrEmpty(DDINumber))
      {
        //todo: remove dependancy...with NHibernate a another repository is need to do this (second session)
        var ddi = new Repository().GetFromName<IDDI>(Number);
        ddi.UsedOn = DDIUsedOn.NotUsed;
        ddi.Update();
      }
    }

    public int Id { get { return _under.Id; } }
    public string Number { get { return _under.Number; } set { _under.Number = value; } }
    public string Notes { get { return _under.Notes; } set { _under.Notes = value; } }
    public string QueueName { get { return _under.QueueName; } set { _under.QueueName = value; } }
    public string Department { get { return _under.Department; } set { _under.Department = value; } }
    public QueueStrategy Strategy { get { return (QueueStrategy)Enum.Parse(typeof(QueueStrategy), _under.Strategy); } set { _under.Strategy = value.ToString(); } }
    public bool RingOnBusy { get { return _under.RingOnBusy; } set { _under.RingOnBusy = value; } }

    public string DDINumber
    {
      get { return _under.DDINumber; }
      set
      {
        IDDI ddi;
        if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(_under.DDINumber))
        {
          ddi = _repository.GetFromName<IDDI>(_under.DDINumber);

          ddi.UsedOn = _repository.GetList<IRoutingRule>().FirstOrDefault(r => r.Dialplan.Id == 12 &&
                                    r.Number == ddi.DDINumber) != null ?
                                                DDIUsedOn.Default : DDIUsedOn.NotUsed;

          ddi.Update();
          _under.DDINumber = string.Empty;
        }
        else if (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(_under.DDINumber))
        {
          ddi = _repository.GetFromName<IDDI>(value);
          ddi.UsedOn = DDIUsedOn.Queue;
          ddi.Update();
          _under.DDINumber = value;
        }
      }
    }

    public string CLINumber { get { return _repository.GetFromId<ICLI>(_under.CLIId).CLINumber; } set { _under.CLIId = _repository.GetFromName<ICLI>(value).Id; } }

    public IVoiceMail VoiceMail
    {
      get { return GetVoiceMail(); }

      set { SetVoiceMail(value); }
    }

    private IVoiceMail GetVoiceMail()
    {
      return _repository.GetFromId<IVoiceMail>(_under.VoiceMailId);
    }

    private void SetVoiceMail(IVoiceMail value)
    {
      _under.VoiceMailId = value == null ? 0 : value.Id;
    }

    public int VoicemailDelay { get { return _under.VoicemailDelay; } set { _under.VoicemailDelay = value; } }
    public IMusicOnHold MusicOnHold
    {
      get
      {
        return _repository.GetFromId<IMusicOnHold>(_under.ComMusicOnHoldId);
      }
      set
      {
        _under.ComMusicOnHoldId = value == null ? 0 : value.Id;
      }
    }

    public IEnumerable<IQueueMember> QueueMembers { get { return _QueueMembers; } }

    public bool IncludeInDirectory { get { return _under.IncludeInDirectory; } set { _under.IncludeInDirectory = value; } }

    private List<IQueueMember> _QueueMembers
    {
      get
      {
        _queueMembers = _queueMembers ?? _repository.GetList<IQueueMember>().Where(a => a.ParentQueue.Id == _under.Id).ToList();
        return _queueMembers;
      }

    }

    public void AddExtensionAsQueueMember(IExtension extension, int queueId)
    {
      var member = _repository.Add<IQueueMember>();
      member.Extension = extension;
      member.ParentQueue = _repository.GetFromId<IQueue>(queueId);
      _QueueMembers.Add(member);
      member.Update();
    }

    public void AddQueueAsQueueMember(IQueue queueToAdd, int queueId)
    {
      var member = _repository.Add<IQueueMember>();
      member.Queue = queueToAdd;
      member.ParentQueue = _repository.GetFromId<IQueue>(queueId);
      _QueueMembers.Add(member);
      member.Update();
    }

  }
}
