using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
    internal class FourComFederatedLink : IFourComFederatedLink
    {
        private readonly IFu4ComFederation _under;
        private readonly IRepositoryWithDelete _modelRepository;

        public FourComFederatedLink(IFu4ComFederation baseFederation,IRepositoryWithDelete modelRepository)
        {
            _under = baseFederation;
            _modelRepository = modelRepository;
        }

        public int Id { get { return _under.Id; } }

        public string Name
        {
            get { return _under.FuFederationName; }
            set { _under.FuFederationName = value; }
        }

        public string Description
        {
            get { return _under.Description; }
            set { _under.Description = value; }
        }

        public ITrunk Trunk
        {
            get
            {
                return _under.ComTrunkId == 0
                    ? null
                    : _modelRepository.GetFromId<ITrunk>(_under.ComTrunkId);
            }
            set { _under.ComTrunkId = value.Id; }
        }

        public bool SetFederationValues(string name, string accessCode, string password)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(accessCode) && string.IsNullOrEmpty(password))
            {
                return false;
            }
            return true;
        }

        public void Delete()
        {
            _modelRepository.Delete(_under);
        }
    }
}