using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
    internal class Trunk : ITrunk
    {
        private readonly IComTrunk _under;
        private readonly IRepositoryWithDelete _modelRepository;
        private List<IDDI> _ddis;
        private List<IAccessCode> _accessCodes;

        public Trunk(IComTrunk comTrunk, IRepositoryWithDelete modelRepository)
        {
            _under = comTrunk;
            _modelRepository = modelRepository;
        }

        public int Id
        {
            get { return _under.Id; }
        }

        public string Name
        {
            get { return _under.ComTrunkName; }
            set { _under.ComTrunkName = value; }
        }

        public string TrunkInPresentationValue1
        {
            get { return _under.CLIPresentationValue1; }
            set { _under.CLIPresentationValue1 = value; }
        }

        public string TrunkInPresentationValue2
        {
            get { return _under.CLIPresentationValue2; }
            set { _under.CLIPresentationValue2 = value; }
        }

        public TrunkType TrunkType
        {
            get { return (TrunkType)Enum.Parse(typeof(TrunkType), _under.Type); }
            set { _under.Type = value.ToString(); }
        }

        public string DefaultDestination
        {
            get { return _under.DefaultDestination; }
            set { _under.DefaultDestination = value; }
        }

        public TrunkInPresentationType TrunkInPresentationType1
        {
            get
            {
                if (string.IsNullOrEmpty(_under.CLIPresentationType1))
                {
                    return TrunkInPresentationType.None;
                }
                return (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), _under.CLIPresentationType1);
            }
            set { _under.CLIPresentationType1 = value.ToString(); }
        }

        public TrunkInPresentationType TrunkInPresentationType2
        {
            get
            {
                if (string.IsNullOrEmpty(_under.CLIPresentationType2))
                {
                    return TrunkInPresentationType.None;
                }
                return (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), _under.CLIPresentationType2);
            }
            set { _under.CLIPresentationType2 = value.ToString(); }
        }

        public IEnumerable<IDDI> DDIs
        {
            get { return LazyDDIs; }
        }

        public List<IAccessCode> AccessCodes
        {
            get { return LazyAccessCodes; }
        }

        public bool TrunkHasIAccessCode(string code, int priority)
        {
            return _modelRepository.GetList<IAccessCode>()
                                   .Any(a => a.ParentTrunk.Id == Id && a.Code == code && a.Priority == priority);
        }

        public void AddAccessCodes(IAccessCode accessCode)
        {
            if (LazyAccessCodes.Contains(accessCode))
            {
                throw new PrivateDuplicateAccessCodeException("The access code is not distinct", this, accessCode);
            }

            _accessCodes.Add(accessCode);
        }

        public void Delete()
        {
            _modelRepository.Delete(_under);
        }

        //used to lazy-load the list
        private IEnumerable<IDDI> LazyDDIs
        {
            get
            {
                _ddis = _ddis ?? _modelRepository.GetList<IDDI>().Where(a => a.Trunk.Id == _under.Id).ToList();
                return _ddis;
            }
        }

        //used to lazy-load the list
        private List<IAccessCode> LazyAccessCodes
        {
            get
            {
                _accessCodes = _accessCodes ??
                               _modelRepository.GetList<IAccessCode>().Where(a => a.ParentTrunk.Id == _under.Id).ToList();
                return _accessCodes;
            }
        }

        private class PrivateDuplicateAccessCodeException : DuplicateAccessCodeException
        {
            public PrivateDuplicateAccessCodeException(string message, ITrunk trunk, IAccessCode accessCode)
                : base(message)
            {
                Trunk = trunk;
                AccessCode = accessCode;
            }
        }
    }

}