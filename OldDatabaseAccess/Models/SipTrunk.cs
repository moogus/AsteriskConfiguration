using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class SipTrunk : ISipTrunk
    {
        private readonly IRepository _repository;
        private readonly ISessionWrapper _session;
        private readonly AstSipFriend _under;

        public SipTrunk(AstSipFriend under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
            _repository = repository;
        }

        public SipTrunk(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _repository = repository;

            _under = new AstSipFriend {Type = "peer", Context = "Trunks", Insecure = "invite"};
            _accessCodes = new List<IAccessCode>();
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

        public void ExtraDelete(){
        }

        #endregion

        #region Implementation of ISipTrunk

        private List<IAccessCode> _accessCodes;

        public int Id
        {
            get { return _under.Id; }
        }

        public string Name
        {
            get { return _under.Number; }
            set { _under.Number = value; }
        }

        public string UserName
        {
            get { return _under.DefaultUser; }
            set { _under.DefaultUser = value; }
        }
        public string Password
        {
            get { return _under.Password; }
            set { _under.Password = value; }
        }
        public string Host
        {
            get { return _under.Host; }
            set { _under.Host = value; }
        }

        public IAccessCode AddAccessCode(string code)
        {
            var accessCode = _repository.Add<IAccessCode>();
            accessCode.Code = code;
            accessCode.Trunk = this;
            accessCode.Update();
            UpdateAccessCodes();
            return accessCode;
        }

        public IEnumerable<IAccessCode> AccessCodes
        {
            get
            {
                if (_accessCodes == null)
                {
                    UpdateAccessCodes();
                }
                return _accessCodes;
            }
        }

        private void UpdateAccessCodes()
        {
            IEnumerable<IAccessCode> allAccessCodes = _repository.GetList<IAccessCode>();
            _accessCodes = new List<IAccessCode>(allAccessCodes.Where(a => a.Trunk != null && a.Trunk.Id == Id));
        }

        #endregion
    }
}