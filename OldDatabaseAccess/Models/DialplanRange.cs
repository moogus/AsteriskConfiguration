using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class DialplanRange : IDialplanRange
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly FuDialplanRange _under;

        public DialplanRange(FuDialplanRange under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;
        }

        public DialplanRange(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;
            _under = new FuDialplanRange();
        }

        #region IDialplanRange Members

        public int Id
        {
            get { return _under.Id; }
        }

        public string DaysOfWeek
        {
            get { return _under.DaysOfWeek; }
            set { _under.DaysOfWeek = value; }
        }

        public string TimeRange
        {
            get { return _under.TimeRange; }
            set { _under.TimeRange = value; }
        }

        public int Priority
        {
            get { return _under.Priority; }
            set { _under.Priority = value; }
        }

        public string Dialplan
        {
            get
            {
                var dialplan = _repository.GetFromId<IDialplan>(_under.FuDialplanId);
                return dialplan == null ? "" : dialplan.Name;
            }
            set
            {
                var dialplan = _repository.GetFromName<IDialplan>(value);
                _under.FuDialplanId = dialplan == null ? 0 : dialplan.Id;
            }
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

        #endregion
    }
}