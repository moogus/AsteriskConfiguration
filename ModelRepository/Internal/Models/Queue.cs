using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class Queue : IQueue
  {
    private readonly IComQueue _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private List<IQueueMember> _queueMembers;

    public Queue(IComQueue comQueue, IRepositoryWithDelete modelRepository)
    {
      _under = comQueue;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Number
    {
      get { return _under.Number; }
      set { _under.Number = value; }
    }

    public string Notes
    {
      get { return _under.Notes; }
      set { _under.Notes = value; }
    }

    public string QueueName
    {
      get { return _under.QueueName; }
      set { _under.QueueName = value; }
    }

    public string Department
    {
      get { return _under.Department; }
      set { _under.Department = value; }
    }

    public QueueStrategy Strategy
    {
      get { return (QueueStrategy) Enum.Parse(typeof (QueueStrategy), _under.Strategy); }
      set { _under.Strategy = value.ToString(); }
    }

    public bool RingOnBusy
    {
      get { return _under.RingOnBusy; }
      set { _under.RingOnBusy = value; }
    }

    public IEnumerable<IQueueMember> QueueMembers
    {
      get { return LazyQueueMembers; }
    }

    public bool IncludeInDirectory
    {
      get { return _under.IncludeInDirectory; }
      set { _under.IncludeInDirectory = value; }
    }

    public int VoicemailDelay
    {
      get { return _under.VoicemailDelay; }
      set { _under.VoicemailDelay = value; }
    }

    public IVoiceMail VoiceMail
    {
      get { return _modelRepository.GetFromId<IVoiceMail>(_under.VoiceMailId); }
      set { _under.VoiceMailId = value == null ? 0 : value.Id; }
    }

    public IDDI DDI
    {
      get { return !string.IsNullOrEmpty(_under.DDINumber) ? _modelRepository.GetFromName<IDDI>(_under.DDINumber) : null; }
      set
      {
        if (value == null)
        {
          _under.DDINumber = string.Empty;
          return;
        }
        _under.DDINumber = value.DDINumber;
      }
    }

    public ICLI CLI
    {
      get { return _under.CLIId != 0 ? _modelRepository.GetFromId<ICLI>(_under.CLIId) : null; }
      set { _under.CLIId = value == null ? 0 : _modelRepository.GetFromName<ICLI>(value.CLINumber).Id; }
    }

    public IMusicOnHold MusicOnHold
    {
      get { return _modelRepository.GetFromId<IMusicOnHold>(_under.ComMusicOnHoldId); }
      set { _under.ComMusicOnHoldId = value == null ? 0 : value.Id; }
    }

    public void AddExtensionAsQueueMember(IExtension extension, int queueId)
    {
      var member = _modelRepository.Add<IQueueMember>();
      member.Extension = extension;
      member.ParentQueue = _modelRepository.GetFromId<IQueue>(queueId);
      LazyQueueMembers.Add(member);
    }

    public void AddQueueAsQueueMember(IQueue queueToAdd, int queueId)
    {
      var member = _modelRepository.Add<IQueueMember>();
      member.Queue = queueToAdd;
      member.ParentQueue = _modelRepository.GetFromId<IQueue>(queueId);
      LazyQueueMembers.Add(member);
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }

    //used to lazy-load the list
    private List<IQueueMember> LazyQueueMembers
    {
      get
      {
        _queueMembers = _queueMembers ??
                        _modelRepository.GetList<IQueueMember>().Where(a => a.ParentQueue.Id == _under.Id).ToList();
        return _queueMembers;
      }
    }
  }
}