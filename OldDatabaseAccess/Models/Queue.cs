using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class Queue : IQueue
    {
        private readonly AstExtension VoicemailExtension;
        private readonly AstQueue _under;
        private readonly ISessionWrapper _session;
        private readonly IRepository _repository;

        private readonly AstExtension _internalExtension;
        private AstExtension _ddiExtension;
        private readonly FuContactDetails _contactDetails;

        public Queue(AstQueue under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;

            _internalExtension =
                session.Query<AstExtension>().FirstOrDefault(
                    e => e.ExtensionNumber == _under.Name && e.Context == "Queues");
            if (_internalExtension != null && _internalExtension.Appdata.Contains(",") && _internalExtension.Appdata.Split(',').Count() == 3)
            {
                VoicemailDelay = int.Parse(_internalExtension.Appdata.Split(',')[2]);
            }

            _contactDetails = session.Query<FuContactDetails>().FirstOrDefault(d => d.FuExtensionNumber == _under.Name);

            _ddiExtension =
                session.Query<AstExtension>().FirstOrDefault(
                    e => e.Appdata == "simplequeue," + _under.Name && e.Context == "incoming");
            _ddiNumber = _ddiExtension == null
                             ? ""
                             : _ddiExtension.ExtensionNumber;

            parseJoinOptions();
            VoicemailExtension =
                     session.Query<AstExtension>().FirstOrDefault(
                         e =>
                         e.App == "Macro" && e.ExtensionNumber == under.Name && e.Context == "Queues" &&
                         e.Appdata.StartsWith("voicemail,"));
            if (VoicemailExtension != null)
            {
                VoiceMail = VoicemailExtension == null
                                ? null
                                : _repository.GetFromName<IVoiceMail>(VoicemailExtension.Appdata.Split(',')[1]);
            }
        }

        private void parseJoinOptions()
        {
            _joinOptions = new List<string>();
            foreach (var option in _under.JoinEmpty.Split(','))
            {
                _joinOptions.Add(option);
            }
        }

        public Queue(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;
            _under = new AstQueue();
            _internalExtension = new AstExtension();
            _contactDetails = new FuContactDetails();
            _internalExtension.Context = "Queues";
            _internalExtension.Priority = 1;
            _internalExtension.App = "Macro";
            _under.JoinEmpty = "paused";

            parseJoinOptions();
        }

        #region Implementation of IQueue

        public int Id
        {
            get { return _under.Id; }
            set { _under.Id = value; }
        }

        public string Number
        {
            get { return _under.Name; }
            set
            {
                _under.Name = value;
                _contactDetails.FuExtensionNumber = value;
                _internalExtension.ExtensionNumber = value;
                _internalExtension.Appdata = "simplequeue," + value;
                _internalExtension.DestinationNumber = "";
                _internalExtension.DestinationType = "";
            }
        }
        public string Notes
        {
            get { return _contactDetails.Notes; }
            set { _contactDetails.Notes = value; }
        }
        public string QueueName
        {
            get { return _contactDetails.LastName; }
            set { _contactDetails.LastName = value; }
        }
        public string Department
        {
            get { return _contactDetails.Department; }
            set { _contactDetails.Department = value; }
        }

        public string Strategy
        {
            get
            {
                switch (_under.Strategy)
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
                        _under.Strategy = "ringall";
                        break;
                    case "least recent":
                        _under.Strategy = "leastrecent";
                        break;
                    case "fewest calls":
                        _under.Strategy = "fewestcalls";
                        break;
                    case "random":
                        _under.Strategy = "random";
                        break;
                    case "round robin":
                        _under.Strategy = "rrmemory";
                        break;
                    case "linear":
                        _under.Strategy = "linear";
                        break;
                    case "weighted random":
                        _under.Strategy = "wrandom";
                        break;
                    default:
                        _under.Strategy = value;
                        break;
                }

            }
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
                _under.JoinEmpty = string.Join(",", _joinOptions);
            }
        }

        private string _ddiNumber;
        private List<string> _joinOptions;

        public string DDINumber
        {
            get { return _ddiNumber; }
            set { _ddiNumber = value; }
        }

        public IVoiceMail VoiceMail { get; set; }

        public int VoicemailDelay { get; set; }

        private List<IQueueMember> _queueMembers;
        public IEnumerable<IQueueMember> QueueMembers
        {
            get
            {
                return _queueMembers ??
                       (_queueMembers =
                        _session.Query<AstQueueMember>().Where(qm => qm.QueueName == Number).Select(
                            qm => _repository.GetFromId<IQueueMember>(qm.Id)).ToList());
            }
        }

        public void AddExtensionAsQueueMember(IExtension extension, string queueName)
        {
            ////do lazy load of list
            var temp = QueueMembers;
            IQueueMember queueMember = _repository.Add<IQueueMember>();
            queueMember.Extension = extension;
            queueMember.QueueName = queueName;
            _queueMembers.Add(queueMember);
            queueMember.Update();
        }

        public void AddQueueAsQueueMember(IQueue queueToAdd, string queueName)
        {
            ////do lazy load of list
            var temp = QueueMembers;
            IQueueMember queueMember = _repository.Add<IQueueMember>();
            queueMember.Queue = queueToAdd;
            queueMember.QueueName = queueName;
            _queueMembers.Add(queueMember);
            queueMember.Update();
        }

        #endregion

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
            if (VoiceMail != null)
            {
                _internalExtension.Appdata = "simplequeue," + Number + "," + VoicemailDelay;
            }
            else
            {
                _internalExtension.Appdata = "simplequeue," + Number;
            }

            _session.SaveOrUpdate(_internalExtension);
            _session.SaveOrUpdate(_contactDetails);
            if (_ddiExtension != null)
            {
                SaveOrDeleteDDIExtension(_ddiNumber);
            }
            else
            {
                CreateNewDDIExtension(_ddiNumber);
            }

            if (VoicemailExtension != null)
            {
                SaveOrDeleteVoicemailExtension(VoiceMail);
            }
            else
            {
                CreateNewVoicemailExtension(VoiceMail);
            }

        }


        private void SaveOrDeleteDDIExtension(string dDiNumber)
        {
            if (!string.IsNullOrEmpty(dDiNumber))
            {
                _ddiExtension.ExtensionNumber = dDiNumber;
                _session.SaveOrUpdate(_ddiExtension);
            }
            else
                _session.Delete(_ddiExtension);
        }

        private void CreateNewDDIExtension(string dDiNumber)
        {
            if (!string.IsNullOrEmpty(dDiNumber))
            {
                _ddiExtension = new AstExtension
                {
                    Context = "incoming",
                    Priority = 1,
                    App = "macro",
                    Appdata = "simplequeue," + Number,
                    ExtensionNumber = dDiNumber,
                    DestinationNumber = "",
                    DestinationType = ""

                };
                _session.SaveOrUpdate(_ddiExtension);
            }
        }
        public void ExtraDelete()
        {
            foreach (var queueMember in QueueMembers)
            {
                queueMember.Delete();
            }

            _session.Delete(_internalExtension);
            _session.Delete(_contactDetails);
            if (_ddiExtension != null)
                _session.Delete(_ddiExtension);

        }

        private void SaveOrDeleteVoicemailExtension(IVoiceMail voicemail)
        {
            if (voicemail != null)
            {
                VoicemailExtension.Appdata = "voicemail," + voicemail.Number;
                _session.SaveOrUpdate(VoicemailExtension);
            }
            else
                _session.Delete(VoicemailExtension);
        }

        private void CreateNewVoicemailExtension(IVoiceMail voicemail)
        {
            if (voicemail != null)
            {
                _ddiExtension = new AstExtension
                {
                    Context = "Queues",
                    Priority = 2,
                    App = "Macro",
                    Appdata = "voicemail," + voicemail.Number,
                    DestinationNumber = "",
                    DestinationType = "",
                    ExtensionNumber = Number
                };
                _session.SaveOrUpdate(_ddiExtension);
            }
        }

        #endregion

    }
}
