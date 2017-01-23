using System;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    internal class QueueMember : IQueueMember, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComQueueMember _under;
    private readonly IExtension _extension;
    private readonly IQueue _queue;

    internal QueueMember(ComQueueMember under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;

      switch (Type)
      {
        case QueueMemberType.Extension:
          _extension = _repository.GetFromId<IExtension>(_under.ExtensionId);
          break;
        case QueueMemberType.Queue:
          _queue = _repository.GetFromId<IQueue>(_under.QueueId);
          break;
      }
    }


    internal QueueMember(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;

      _under = new ComQueueMember();
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
    }

    public int Id { get { return _under.Id; } }

    public IQueue ParentQueue
    {
      get { return _repository.GetFromId<IQueue>(_under.ParentQueueId); }
      set { _under.ParentQueueId = value.Id; }
    }
  
    public int Penalty { get { return _under.Penalty; } set { _under.Penalty = value; } }
    public int Paused { get { return _under.Paused; } set { _under.Paused = value; } }

    public QueueMemberType Type
    {
      get
      {
        var rtn = QueueMemberType.Unknown;
        switch (_under.Type)
        {
          case 0:
            rtn = QueueMemberType.Unknown;
            break;
          case 1:
            rtn = QueueMemberType.Extension;
            break;
          case 2:
            rtn = QueueMemberType.Queue;
            break;

        }
        return rtn;
      }
      set
      {
        switch (value)
        {
          case QueueMemberType.Unknown:
            _under.Type = 0;
            break;
          case QueueMemberType.Extension:
            _under.Type = 1;
            break;
          case QueueMemberType.Queue:
            _under.Type = 2;
            break;
        }
      }
    }
    public IExtension Extension
    {
      get { return _extension; }
      set
      {
        _under.ExtensionId = value.Id;
        _under.Penalty = 3;
        _under.Paused = 0;
        Type = QueueMemberType.Extension;
      }
    }
    public IQueue Queue
    {
      get { return _queue; }
      set
      {
        _under.QueueId = value.Id;
        _under.Penalty = 3;
        _under.Paused = 0;
        Type = QueueMemberType.Queue;
      }
    }
  }
}
