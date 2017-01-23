using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using NHibernate;
using NHibernate.Linq;

namespace Repository
{
    public interface IExtension : IModel
    {
        string IpAddress { get; }
        string Model { get; }
        int Id { get; }
        string Status { get; }
        string Password { get; set; }
        string Notes { get; set; }
        string Number { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Department { get; set; }
        string Email { get; set; }
        string JobTitle { get; set; }
        string DDINumber { get; set; }
        IVoiceMail VoiceMail { get; set; }
        int VoicemailDelay { get; set; }
    }

    internal class Extension : IExtension
    {
        AstSipFriend _under;
        private readonly ISession _session;
        private readonly Repository _repository;
        FuContactDetails _contactDetails;
        AstExtension _internalAstExtension;
        AstExtension _ddiAstExtension;
        AstExtension VoicemailExtension;

        public Extension(AstSipFriend under, ISession session, Repository repository)
        {
            
            _under = under;
            _session = session;
            _repository = repository;

            _contactDetails = session.Query<FuContactDetails>().FirstOrDefault(d => d.FuExtensionNumber == under.Number);
            _internalAstExtension = session.Query<AstExtension>().FirstOrDefault(e => (e.Appdata == "SIP/" + under.Number || e.Appdata.StartsWith("SIP/" + under.Number + ",")) && e.Context == "LocalSets");

            if (_internalAstExtension != null && _internalAstExtension.Appdata.Contains(","))
            {
                VoicemailDelay = int.Parse(_internalAstExtension.Appdata.Split(',')[1]);
            }
            _ddiAstExtension = session.Query<AstExtension>().FirstOrDefault(e => e.Appdata == "SIP/" + under.Number && e.Context == "incoming");
            DDINumber = _ddiAstExtension == null ? "" : _ddiAstExtension.ExtensionNumber;


            VoicemailExtension = session.Query<AstExtension>().FirstOrDefault(e => e.App == "Macro" && e.ExtensionNumber == under.Number && e.Context == "LocalSets" && e.Appdata.StartsWith("voicemail,"));
            if (VoicemailExtension != null)
            {
                VoiceMail = VoicemailExtension == null
                           ? null
                           : _repository.GetFromName<IVoiceMail>(VoicemailExtension.Appdata.Split(',')[1]);
            }
        }

        public Extension(ISession session, Repository repository)
        {
            _session = session;
            _repository = repository;

            _under = new AstSipFriend
            {
                Host = "Dynamic",
                Type = "friend",
                Context = "LocalSets",
                SubscribeContext = "BLF_Group_1",
                CallGroup = 1,
                PickupGroup = 1,
                AllowSubscribe = "yes",
                NotifyRinging = "yes",
                NotifyHold = "yes",
                NotifyCid = "yes",
                CallLimit = 2
            };

            _contactDetails = new FuContactDetails();
            _internalAstExtension = new AstExtension
            {
                Context = "LocalSets",
                Priority = 1,
                App = "Dial",
                DestinationNumber = "",
                DestinationType = ""
            };
        }
        public int Id
        {
            get { return _under.Id; }
        }

        public string Number
        {
            get { return _under.Number; }
            set
            {
                _under.Number = value;
                _contactDetails.FuExtensionNumber = value;
                _internalAstExtension.ExtensionNumber = value;

                if (_ddiAstExtension != null)
                {
                    _ddiAstExtension.Appdata = "SIP/" + value;
                }
            }
        }

        public string Password
        {
            get { return _under.Password; }
            set { _under.Password = value; }
        }

        public string IpAddress
        {
            get { return _under.IpAddress; }
        }

        public string Model
        {
            get { return _under.Model; }
        }

        public string Notes
        {
            get { return _contactDetails.Notes; }
            set { _contactDetails.Notes = value; }
        }

        public string FirstName
        {
            get { return _contactDetails.FirstName; }
            set { _contactDetails.FirstName = value; }
        }

        public string LastName
        {
            get { return _contactDetails.LastName; }
            set { _contactDetails.LastName = value; }
        }

        public string Department
        {
            get { return _contactDetails.Department; }
            set { _contactDetails.Department = value; }
        }

        public string Email
        {
            get { return _contactDetails.Email; }
            set { _contactDetails.Email = value; }
        }

        public string JobTitle
        {
            get { return _contactDetails.JobTitle; }
            set { _contactDetails.JobTitle = value; }
        }

        public string Status
        {
            get
            {
                if (_under.StatusTime < 1)
                    return "never used";
                return ConvertFromUnixToDateTime(_under.StatusTime) > DateTime.UtcNow
                           ? "active"
                           : "not active"; 
            }
        }

        public string DDINumber { get; set; }

        public IVoiceMail VoiceMail { get; set; }

        public int VoicemailDelay { get; set; }

        private static DateTime ConvertFromUnixToDateTime(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public bool Update()
        {
            _internalAstExtension.Appdata = "SIP/" + Number;
            if (VoiceMail != null)
                _internalAstExtension.Appdata += "," + VoicemailDelay;

            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(_under);
                _session.SaveOrUpdate(_internalAstExtension);
                _session.SaveOrUpdate(_contactDetails);

                if (_ddiAstExtension != null)
                {
                    SaveOrDeleteDDIExtension(DDINumber);
                }
                else
                {
                    CreateNewDDIExtension(DDINumber);
                }

                if (VoicemailExtension != null)
                {
                    SaveOrDeleteVoicemailExtension(VoiceMail);
                }
                else
                {
                    CreateNewVoicemailExtension(VoiceMail);
                }

                transaction.Commit();
            }
            //TODO success?
            return true;
        }

        private void SaveOrDeleteDDIExtension(string dDiNumber)
        {
            if (!string.IsNullOrEmpty(dDiNumber))
            {
                _ddiAstExtension.ExtensionNumber = dDiNumber;
                _session.SaveOrUpdate(_ddiAstExtension);
            }
            else
                _session.Delete(_ddiAstExtension);
        }

        private void CreateNewDDIExtension(string dDiNumber)
        {
            if (!string.IsNullOrEmpty(dDiNumber))
            {
                _ddiAstExtension = new AstExtension
                                       {
                                           Context = "incoming",
                                           Priority = 1,
                                           App = "Dial",
                                           Appdata = "SIP/" + Number,
                                           ExtensionNumber = dDiNumber,
                                           DestinationNumber = "",
                                           DestinationType = ""
                                       };
                _session.SaveOrUpdate(_ddiAstExtension);
            }
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
                _ddiAstExtension = new AstExtension
                {
                    Context = "LocalSets",
                    Priority = 2,
                    App = "Macro",
                    Appdata = "voicemail," + voicemail.Number,
                    ExtensionNumber = Number
                };
                _session.SaveOrUpdate(_ddiAstExtension);
            }
        }
        public bool Delete()
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.Delete(_under);
                _session.Delete(_internalAstExtension);
                _session.Delete(_contactDetails);

                if (_ddiAstExtension != null)
                    _session.Delete(_ddiAstExtension);

                if (VoicemailExtension != null)
                    _session.Delete(VoicemailExtension);

                transaction.Commit();
            }
            //TODO success?
            return true;
        }
    }
}