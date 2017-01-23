using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface ISipTrunk : IModel
    {
        int Id { get; }
        string Name { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        IAccessCode AddAccessCode(string code);
        IEnumerable<IAccessCode> AccessCodes { get; set; }
    }

    public interface IAccessCode : IModel
    {
        string Code { get; set; }
    }

    public class AccessCode : IAccessCode
    {
        private readonly IUnderlyingAccessCode _accessCode;

        public AccessCode(IUnderlyingAccessCode accessCode)
        {
            _accessCode = accessCode;
        }

        public string Code
        {
            get { return _accessCode.Code; }
            set { _accessCode.Code = value; }
        }

        public bool Update()
        {
            return _accessCode.Update();
        }

        public bool Delete()
        {
            return _accessCode.Delete();
        }
    }

    public class SipTrunk : ISipTrunk
    {
        private readonly IUnderlyingSipTrunk _underlyingSipTrunk;

        public IAccessCode AddAccessCode(string code)
        {
            var accessCode=_underlyingSipTrunk.AddAccessCode(code);
            UpdateAccessCodes();
            return new AccessCode(accessCode);
        }

      private IEnumerable<IAccessCode> _accessCodes;
      public IEnumerable<IAccessCode> AccessCodes
      {
        get
        {
          UpdateAccessCodes();
          return _accessCodes;
        }
        set { _accessCodes = value; }
      }

      public SipTrunk(IUnderlyingSipTrunk underlyingSipTrunk)
        {
            _underlyingSipTrunk = underlyingSipTrunk;
            UpdateAccessCodes();
            if (_underlyingSipTrunk.Id ==0)
            {
                SetDefaultValues();
            }
        }

        private void UpdateAccessCodes()
        {
            _accessCodes = _underlyingSipTrunk.AccessCodes.Select(a => new AccessCode(a));
        }

        private void SetDefaultValues()
        {
            _underlyingSipTrunk.Type = "peer";
            _underlyingSipTrunk.Context = "Trunks";
            _underlyingSipTrunk.Insecure = "invite";
        }

        public bool Update()
        {
            return _underlyingSipTrunk.Update();
        }

        public bool Delete()
        {
            return _underlyingSipTrunk.Delete();
        }

        public int Id
        {
            get { return _underlyingSipTrunk.Id; }
        }

        public string Name
        {
            get { return _underlyingSipTrunk.Name; }
            set { _underlyingSipTrunk.Name = value; }
        }

        public string UserName
        {
            get { return _underlyingSipTrunk.Username; }
            set { _underlyingSipTrunk.Username = value; }
        }

        public string Password
        {
            get { return _underlyingSipTrunk.Password; }
            set { _underlyingSipTrunk.Password = value; }
        }

        public string Host
        {
            get { return _underlyingSipTrunk.Host; }
            set { _underlyingSipTrunk.Host = value; }
        }
    }

}