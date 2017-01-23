using DatabaseAccess.DatabaseTables;
using NHibernate;

namespace Repository
{
    public interface IVoiceMail : IModel
    {
        int Id { get; }
        string Number { get; set; }
        string Password { get; set; }
        int NumberOfMessages { get; set; }
        int MessageLength { get; set; }
        int HeldNumberOfMessages { get; set; }
        string DefaultEmail { get; set; }
        bool EmailNotificationHasMp3 { get; set; }
    }

    public class Voicemail : IVoiceMail
    {
        private readonly AstVoicemail _under;
        private readonly ISession _session;

        public Voicemail(AstVoicemail under, ISession session,Repository repository)
        {
            _under = under;
            _session = session;
        }

        public Voicemail(ISession session, Repository repository)
        {
            _session = session;
            _under = new AstVoicemail();
            _under.Context = "default";
        }

        public bool Update()
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(_under);

                transaction.Commit();
            }
            //TODO success?
            return true;
        }

        public bool Delete()
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.Delete(_under);

                transaction.Commit();
            }
            //TODO success?
            return true;
        }

        public int Id
        {
            get { return _under.Id; }
        }

        public string Number
        {
            get { return _under.Mailbox; }
            set { _under.Mailbox = value; }
        }

        public string Password
        {
            get { return _under.Password; }
            set { _under.Password = value; }
        }

        public int MessageLength
        {
            get { return _under.MaxSecs; }
            set { _under.MaxSecs = value; }
        }

        public string DefaultEmail
        {
            get { return _under.Email; }
            set { _under.Email = value; }
        }

        public bool EmailNotificationHasMp3
        {
            get { return _under.Attach == "yes"; }
            set { _under.Attach = value ? "yes" : "no"; }
        }

        public int NumberOfMessages { get { return _under.MaxMessages; } set { _under.MaxMessages = value; } }

        public int HeldNumberOfMessages { get { return _under.HeldMaxMessages; } set { _under.HeldMaxMessages = value; } }
   
    }
}
