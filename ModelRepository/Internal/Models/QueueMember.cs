using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class QueueMember : IQueueMember
  {
    private readonly IComQueueMember _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public QueueMember(IComQueueMember under, IRepositoryWithDelete modelRepository)
    {
      _under = under;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public IQueue ParentQueue
    {
      get { return _under.ParentQueueId != 0 ? _modelRepository.GetFromId<IQueue>(_under.ParentQueueId) : null; }
      set { _under.ParentQueueId = value == null ? 0 : value.Id; }
    }

    public int Penalty
    {
      get { return _under.Penalty; }
      set { _under.Penalty = value; }
    }

    public int Paused
    {
      get { return _under.Paused; }
      set { _under.Paused = value; }
    }

    public QueueMemberType Type
    {
      get { return GetQueueMemeberType(); }
      set { SetUnderType(value); }
    }

    public IExtension Extension
    {
      get { return _modelRepository.GetFromId<IExtension>(_under.ExtensionId); }
      set { SetValuesFromExtension(value); }
    }

    public IQueue Queue
    {
      get { return _modelRepository.GetFromId<IQueue>(_under.QueueId); }
      set { SetValuesFromQueue(value); }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }

    private void SetValuesFromExtension(IExtension value)
    {
      _under.ExtensionId = value.Id;
      _under.Penalty = 3;
      _under.Paused = 0;
      Type = QueueMemberType.Extension;
    }

    private void SetValuesFromQueue(IQueue value)
    {
      _under.QueueId = value.Id;
      _under.Penalty = 3;
      _under.Paused = 0;
      Type = QueueMemberType.Queue;
    }

    private void SetUnderType(QueueMemberType value)
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

    private QueueMemberType GetQueueMemeberType()
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
  }
}