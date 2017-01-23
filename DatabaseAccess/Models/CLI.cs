using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    internal class CLI : ICLI, IModel
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly ComCLI _under;

        internal CLI(ComCLI under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;
        }

        internal CLI(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;
            _under = new ComCLI();
        }

        public int Id { get { return _under.Id; } }
        public string CLINumber { get { return _under.CLINumber; } set { _under.CLINumber = value; } }
        public string CLIName { get { return _under.CLIName; } set { _under.CLIName = value; } }

        public ITrunk Trunk
        {
            get
            {
                return _under.TrunkId > 0 ? _repository.GetFromId<ITrunk>(_under.TrunkId) : _repository.Add<ITrunk>();
            }
            set
            {
                _under.TrunkId = value == null ? 0 : value.Id;
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