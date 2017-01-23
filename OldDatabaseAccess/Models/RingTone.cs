using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
   public class RingTone : IRingTone
    {
       private readonly ISessionWrapper _session;
        private readonly FuRingtone _under;

        public RingTone(FuRingtone under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
        }

        public RingTone(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _under = new FuRingtone();
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

        #region Implementation of IRingtone

        public int Id
        {
            get { return _under.Id; }
        }

        public string Name
        {
            get { return _under.Name; }
            set { _under.Name = value; }
        }

        public string SipHeader
        {
            get { return _under.SipHeader; }
            set { _under.SipHeader = value; }
        }

        #endregion
    }
}