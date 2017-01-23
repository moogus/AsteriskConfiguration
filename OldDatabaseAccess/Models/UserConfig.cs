
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{

    public class UserConfig : IUserConfig
    {
        private readonly FuUserConfig _under;
        private readonly ISessionWrapper _session;
        private readonly IRepository _repository;

        public UserConfig(FuUserConfig under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;

            UserExtension = _repository.GetFromName<IExtension>(_under.ExtensionNumber);
        }

        public UserConfig(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;

            _under = new FuUserConfig();
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

        #region Implementation of IUserConfig

        public int Id { get { return _under.Id; } set { _under.Id = value; } }
        public string Number { get { return _under.ExtensionNumber; } set
        {
            _under.ExtensionNumber = value;
            UserExtension = _repository.GetFromName<IExtension>(_under.ExtensionNumber);
        } }
        public string Password { get { return _under.Password; } set { _under.Password = value; } }
        public string Role { get { return _under.Role; } set { _under.Role = value; } }
        public IExtension UserExtension { get; private set; }

        #endregion
    }
}
