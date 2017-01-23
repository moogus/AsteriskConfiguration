using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class DDI : IDDI
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly FuDDI _under;

        public DDI(FuDDI under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;
        }

        public DDI(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;
            _under = new FuDDI();
        }

        #region Implementation of IModel

    
        #endregion

        #region Implementation of IDDI

        private string _used;

        public int Id
        {
            get { return _under.Id; }
        }

        public string DDINumber
        {
            get { return _under.DDI; }
            set { _under.DDI = value; }
        }

        public string Used
        {
            get
            {
                if (_used == null)
                {
                    _used = "";
                    IExtension extension =
                        _repository.GetList<IExtension>().FirstOrDefault(e => e.DDINumber == DDINumber);
                    if (extension != null)
                    {
                        _used = "Extension: " + extension.Number;
                    }
                    IQueue queue =
                        _repository.GetList<IQueue>().FirstOrDefault(e => e.DDINumber == DDINumber);
                    if (queue != null)
                    {
                        _used = "Queue: " + queue.Number;
                    }
                    IRoutingRule route =
                        _repository.GetList<IRoutingRule>().FirstOrDefault(e => e.Number == DDINumber);
                    if (route != null)
                    {
                        _used = "Route: " + route.Number;
                    }
                }
                return _used;
            }
        }

        #endregion

        #region IDDI Members

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