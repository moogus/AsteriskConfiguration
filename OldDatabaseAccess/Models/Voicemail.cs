using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
   internal class Voicemail : IVoiceMail
    {
        private readonly AstVoicemail _under;
        private readonly ISessionWrapper _session;

        public Voicemail(AstVoicemail under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
        }

        public Voicemail(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _under = new AstVoicemail();
            _under.Context = "default";
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
