using DatabaseAccess;

namespace ModelAccess.Models
{
    public enum QueueMemberType
    {
        Unknown,
        Extension,
        Queue
    }
    public interface IQueueMember : IModel
    {
        int Id { get; }
        string QueueName { set; }
        int Penalty { get; set; }
        int Paused { get; set; }
        QueueMemberType Type { get; }

        IExtension Extension { get; set; }
        IQueue Queue { get; set; }
    }

    public class QueueMember : IQueueMember
    {
        private readonly IUnderlyingQueueMember _astQueueMemberLinked;
        private readonly Repository<IExtension> _extensionRepository;
        private readonly Repository<IQueue> _queueRepository;
        private IExtension _extension;

        public QueueMember(IUnderlyingQueueMember astQueueMemberLinked)
        {
            _extensionRepository = new Repository<IExtension>();
            _queueRepository = new Repository<IQueue>();
            _astQueueMemberLinked = astQueueMemberLinked;

            switch (_astQueueMemberLinked.Type)
            {
                case UnderlyingQueueMemberType.Extension:
                    Type = QueueMemberType.Extension;
                    break;
                case UnderlyingQueueMemberType.Queue:
                    Type = QueueMemberType.Queue;
                    break;
                default:
                    Type = QueueMemberType.Unknown;
                    break;
            }

            switch (Type)
            {
                case QueueMemberType.Extension:
                    try
                    {
                        _extension = _extensionRepository.GetFromUnderlying(_astQueueMemberLinked.AstSipFriend);
                    }
                    catch (System.NullReferenceException)
                    {
                        Type=QueueMemberType.Unknown;
                    }
                    break;
                case QueueMemberType.Queue:
                    _queue = _queueRepository.GetFromUnderlying(_astQueueMemberLinked.AstQueueLinked);
                    break;
            }

        }

        #region IQueueMember Members

        public int Id
        {
            get { return _astQueueMemberLinked.Id; }
        }

        public string QueueName
        {
            set { _astQueueMemberLinked.QueueName = value; }
        }

        public int Penalty
        {
            get { return _astQueueMemberLinked.Penalty; }
            set { _astQueueMemberLinked.Penalty = value; }
        }

        public int Paused
        {
            get { return _astQueueMemberLinked.Paused; }
            set { _astQueueMemberLinked.Paused = value; }
        }

        public QueueMemberType Type { get; private set; }

        public IExtension Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                _astQueueMemberLinked.Interface = "SIP/" + Extension.Number;
                _astQueueMemberLinked.MemberName = "SIP/" + Extension.Number;
                _astQueueMemberLinked.Penalty = 3;
                _astQueueMemberLinked.Paused = 0;
            }
        }

        private IQueue _queue;
        public IQueue Queue
        {
            get { return _queue; }
            set
            {
                _queue = value;
                _astQueueMemberLinked.Interface = "Local/" + value.Number + "@Queues";
                _astQueueMemberLinked.MemberName = "Local/" + value.Number + "@Queues";
                _astQueueMemberLinked.Penalty = 3;
                _astQueueMemberLinked.Paused = 0;
            }
        }

        public bool Update()
        {
            return _astQueueMemberLinked.Update();
        }

        public bool Delete()
        {
            return _astQueueMemberLinked.Delete();
        }

        #endregion
    }
}