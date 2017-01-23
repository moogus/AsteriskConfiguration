using System;
using DatabaseAccess.DatabaseTables;
using NHibernate;

namespace DatabaseAccess.Models
{
    public class DialplanDate : IDialplanDate
    {
        private readonly FuDialplanDate _under;
        private readonly ISessionWrapper _session;
        private readonly IRepository _repository;

        public DialplanDate(FuDialplanDate under, ISessionWrapper session, IRepository repository)
        {
            // TODO: Complete member initialization
            _under = under;
            _session = session;
            _repository = repository;
        }

        public DialplanDate(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;
            _under=new FuDialplanDate();
        }

        public int Id { get { return _under.Id; } }

        public DateTime Date
        {
            get { return _under.Date; }
            set { _under.Date = value; }
        }

        public bool IsRange
        {
            get { return _under.FuIsRange; }
            set { _under.FuIsRange = value; }
        }

        public int DateRangeId { get { return _under.FuRangeId; } set { _under.FuRangeId = value; } }

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
    }
}
