using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class Dialplan : IDialplan
    {
        private readonly FuDialplan _under;
        private readonly ISessionWrapper _session;

        public Dialplan(FuDialplan under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
        }

        public Dialplan(ISessionWrapper session, IRepository repository)
        {
            _session = session;

            _under = new FuDialplan();
        }

        public string Name { get { return _under.Name; } set { _under.Name = value; } }
        public int Id { get { return _under.Id; } }

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

        public bool Equals(IDialplan other)
        {
            if (other == null)
                return false;
            return Id == other.Id;
        }
    }
}
