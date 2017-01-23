using System;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class QueueMember : IQueueMember
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly AstQueueMember _under;

        public QueueMember(AstQueueMember under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;

            if (_under.Interface.StartsWith("SIP/"))
            {
                Type = QueueMemberType.Extension;
            }
            else if (_under.Interface.StartsWith("Local"))
            {
                Type = QueueMemberType.Queue;
            }
            else
            {
                Type = QueueMemberType.Unknown;
            }

            switch (Type)
            {
                case QueueMemberType.Extension:
                    string extNo = _under.Interface.Substring(4);
                    Extension = _repository.GetFromName<IExtension>(extNo);
                    break;
                case QueueMemberType.Queue:
                    string queueNo = _under.Interface.Substring(6);
                    int atPos = queueNo.IndexOf("@", StringComparison.Ordinal);
                    queueNo = queueNo.Substring(0, atPos);
                    Queue = _repository.GetFromName<IQueue>(queueNo);
                    break;
            }
        }

        public QueueMember(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;

            _under = new AstQueueMember();
        }

        #region Implementation of IModel

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

        #endregion

        #region Implementation of IQueueMember

        private IExtension _extension;
        private IQueue _queue;

        public int Id
        {
            get { return _under.Id; }
        }

        public string QueueName
        {
            set { _under.QueueName = value; }
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

        public QueueMemberType Type { get; private set; }

        public IExtension Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                _under.Interface = "SIP/" + Extension.Number;
                _under.MemberName = "SIP/" + Extension.Number;
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
                _queue = value;
                _under.Interface = "Local/" + value.Number + "@Queues";
                _under.MemberName = "Local/" + value.Number + "@Queues";
                _under.Penalty = 3;
                _under.Paused = 0;
                Type = QueueMemberType.Queue;
            }
        }

        #endregion
    }
}