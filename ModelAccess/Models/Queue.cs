using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IQueue : IModel
    {
        int Id { get; }
        string Number { get; set; }
        string Notes { get; set; }
        string QueueName { get; set; }
        string Department { get; set; }
        string Strategy { get; set; }
        bool RingOnBusy { get; set; }
        string DDINumber { get; set; }

        IEnumerable<IQueueMember> QueueMembers { get; }

        void AddExtensionAsQueueMember(IExtension extension, string queueName);
        void AddQueueAsQueueMember(IQueue queueToAdd, string queueName);
    }


    public class Queue : IQueue
    {
        private readonly Repository<IQueueMember> _queueMemberRepository;
        private List<IQueueMember> _queueMembers;
        private readonly IUnderlyingQueue _underlyingQueue;

        private readonly List<string> _joinOptions;

        public Queue(IUnderlyingQueue astQueueLinked)
        {
            _queueMemberRepository = new Repository<IQueueMember>();
            _underlyingQueue = astQueueLinked;
            _joinOptions = new List<string>();

            if (String.IsNullOrEmpty(_underlyingQueue.Name))
            {
                SetDefaultInformation();
            }


            foreach (var option in _underlyingQueue.JoinEmpty.Split(','))
            {
                _joinOptions.Add(option);
            }

        }

        #region IQueue Members

        public int Id { get { return _underlyingQueue.Id; } }

        public string Number
        {
            get { return _underlyingQueue.Name; }
            set
            {
                _underlyingQueue.Name = value;
            }
        }

        public string Notes
        {
            get { return _underlyingQueue.Notes; }
            set { _underlyingQueue.Notes = value; }
        }

        public string QueueName
        {
            get { return _underlyingQueue.LastName; }
            set { _underlyingQueue.LastName = value; }
        }

        public string Department
        {
            get { return _underlyingQueue.Department; }
            set { _underlyingQueue.Department = value; }
        }

        public bool RingOnBusy
        {
            get { return !_joinOptions.Contains("inuse"); }
            set
            {
                if (value && _joinOptions.Contains("inuse"))
                    _joinOptions.Remove("inuse");
                if (!value && !_joinOptions.Contains("inuse"))
                    _joinOptions.Add("inuse");
                _underlyingQueue.JoinEmpty = string.Join(",", _joinOptions);
            }
        }

        public string DDINumber
        {
            get { return _underlyingQueue.DDINumber; }
            set { _underlyingQueue.DDINumber = value; }
        }

        public string Strategy
        {
            get
            {
                switch (_underlyingQueue.Strategy)
                {
                    case "ringall":
                        return "ring all";
                    case "leastrecent":
                        return "least recent";
                    case "fewestcalls":
                        return "fewest calls";
                    case "random":
                        return "random";
                    case "rrmemory":
                        return "round robin";
                    case "linear":
                        return "linear";
                    case "wrandom":
                        return "weighted random";
                }
                return "";
            }
            set
            {
                switch (value)
                {
                    case "ring all":
                        _underlyingQueue.Strategy = "ringall";
                        break;
                    case "least recent":
                        _underlyingQueue.Strategy = "leastrecent";
                        break;
                    case "fewest calls":
                        _underlyingQueue.Strategy = "fewestcalls";
                        break;
                    case "random":
                        _underlyingQueue.Strategy = "random";
                        break;
                    case "round robin":
                        _underlyingQueue.Strategy = "rrmemory";
                        break;
                    case "linear":
                        _underlyingQueue.Strategy = "linear";
                        break;
                    case "weighted random":
                        _underlyingQueue.Strategy = "wrandom";
                        break;
                    default:
                        _underlyingQueue.Strategy = value;
                        break;
                }

            }
        }
  
      public IEnumerable<IQueueMember> QueueMembers
      {
        get { return ListOfQueueMembers; }
      }

        protected IList<IQueueMember> ListOfQueueMembers
        {
            get {
                return _queueMembers ??
                       (_queueMembers = _underlyingQueue.QueueMembers.Select(q => _queueMemberRepository.GetFromUnderlying(q)).ToList());
            }
        }

        public bool Update()
        {
            return _underlyingQueue.Update();
        }

        public bool Delete()
        {
            foreach (var queueMemeber in QueueMembers)
            {
                queueMemeber.Delete();
            }
            _underlyingQueue.Delete();
            return true;
        }

        public void AddExtensionAsQueueMember(IExtension extension, string queueName)
        {
            IQueueMember queueMember = _queueMemberRepository.Add();
            queueMember.Extension = extension;
            queueMember.QueueName = queueName;
            ListOfQueueMembers.Add(queueMember);
            queueMember.Update();
        }

        public void AddQueueAsQueueMember(IQueue queueToAdd, string queueName)
        {
            IQueueMember queueMember = _queueMemberRepository.Add();
            queueMember.Queue = queueToAdd;
            queueMember.QueueName = queueName;
            ListOfQueueMembers.Add(queueMember);
            queueMember.Update();
        }

        #endregion

        private void SetDefaultInformation()
        {
            
            _underlyingQueue.JoinEmpty = "paused";
        }
    }

}