
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{

    public class AccessCode : IAccessCode
    {
        private readonly AstExtension _under;
        private readonly ISessionWrapper _session;
        private readonly IRepository _repository;

        public AccessCode(AstExtension under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;

            var trunkname = _under.Appdata.Split('@')[1];
            _trunk = _repository.GetFromName<ISipTrunk>(trunkname);
        }

        public AccessCode(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;

            _under = new AstExtension
                         {
                             Context = "Outgoing",
                             Priority = 1,
                             App = "Macro",
                             DestinationNumber = "",
                             DestinationType = "",
                             ExtensionNumber = ""
                         };
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

        public void ExtraUpdate(){
        }

        public void ExtraDelete()
        {
        }

        #endregion

        #region Implementation of IAccessCode

        public int Id
        {
            get { return _under.Id; }
        }

        public string Code
        {
            get { return StripAccessCode(_under.ExtensionNumber); }
            set { _under.ExtensionNumber = "_" + value + "X."; }
        }

        private ISipTrunk _trunk;
        public ISipTrunk Trunk
        {
            get { return _trunk; }
            set
            {
                _under.Appdata = "trunkdial,SIP/${EXTEN:1}@" + value.Name;
                _trunk = value;
            }
        }

        #endregion

        private string StripAccessCode(string input)
        {
            return input.Substring(1, input.Length - 3);

        }
    }
}
