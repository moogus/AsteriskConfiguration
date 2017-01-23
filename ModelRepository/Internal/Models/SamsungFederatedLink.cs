using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
    internal class SamsungFederatedLink : ISamsungFederatedLink
    {
        private readonly IFuSamsungFederation _under;
        private readonly IRepositoryWithDelete _modelRepository;

        public SamsungFederatedLink(IFuSamsungFederation baseFederation, IRepositoryWithDelete modelRepository)
        {
            _under = baseFederation;
            _modelRepository = modelRepository;

            Extension = _under.ComExtensionId == 0
                            ? null
                            : _modelRepository.GetFromId<IExtension>(_under.ComExtensionId);

            RoutingRule = _under.ComRoutingRuleId == 0
                              ? null
                              : _modelRepository.GetFromId<IRoutingRule>(_under.ComRoutingRuleId);
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

        public IExtension Extension
        {
            get
            {
                return _under.ComExtensionId == 0
                    ? null
                    : _modelRepository.GetFromId<IExtension>(_under.ComExtensionId);
            }
            set { _under.ComExtensionId = value != null ? value.Id : 0; }
        }

        public IRoutingRule RoutingRule
        {
            get
            {
                return _under.ComRoutingRuleId == 0
                    ? null
                    : _modelRepository.GetFromId<IRoutingRule>(_under.ComRoutingRuleId);
            }
            set { _under.ComRoutingRuleId = value != null ? value.Id : 0; }
        }

        public bool SetFederationValues(string name, string accessCode, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(accessCode) || string.IsNullOrEmpty(password)) return false;
            var routingRule = _modelRepository.Add<IRoutingRule>();
            routingRule.DestinationNumber = accessCode.Split(':')[0];
            routingRule.DestinationType = RoutingRuleDestination.AddCode;
            routingRule.Order = 0;
            routingRule.Time = 0;
            routingRule.Number = "Any";
            routingRule.Dialplan = _modelRepository.GetFromName<IDialplan>("fallThrough");

            var extension = _modelRepository.Add<IExtension>();
            extension.Number = name;
            extension.Password = password;

            Extension = extension;
            RoutingRule = routingRule;

            return true;
        }

        public void Delete()
        {
            Extension.Delete();
            RoutingRule.Delete();
            _modelRepository.Delete(_under);
        }
    }
}