using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    internal class CurrentDialPlan : ICurrentDialPlan, IModel
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly FuCurrentDialplan _under;
        private IDialplan _dialplan;

        internal CurrentDialPlan(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;


            _under = _session.Query<FuCurrentDialplan>().First();
            _dialplan = _repository.GetFromId<IDialplan>(_under.CurrentDialplan);
        }

        #region ICurrentDialPlan Members

        public IDialplan Dialplan
        {
            get { return _dialplan; }
            set
            {
                _dialplan = value;
                _under.CurrentDialplan = value.Id;
            }
        }

        public bool AutomaticallyChange
        {
            get { return _under.AutomaticallyChange; }
            set { _under.AutomaticallyChange = value; }
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