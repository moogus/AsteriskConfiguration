using DataAccess;
using DataAccess.TableInterfaces;
using ModelRepository.Internal.ModelHelpers;
using ModelRepository.Internal.Models;
using ModelRepository.Internal;
using ModelRepository.ModelInterfaces;
using ModelUtilities;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace ModelRepository
{
    public class ModelIOC : Registry
    {
        private IContainer _internalContainer;
        private readonly IContainer _dataContainer;
        private readonly IContainer _utilitycontainer;
        internal IContainer Container { get; private set; }

        public ModelIOC()
        {
            _dataContainer = new Container(new DataIOC());
            _utilitycontainer = new Container(new UtilityIOC());
            CreateContainer();
            For<IRepository>().Use(_internalContainer.GetInstance<IRepository>);


        }

        private void CreateContainer()
        {
            _internalContainer = new Container(x =>
              {
                  //external resources
                  x.For<IDataRepository>().Use(_dataContainer.GetInstance<IDataRepository>);

                  //internal
                  x.For<IRepository>().Use<Internal.ModelRepositoryWithMapping>();
                  x.For<IRepositoryWithDelete>().Use<Internal.ModelRepositoryWithMapping>().Ctor<IContainer>();
                  x.For<IEmptyModelRepository>().Use<EmptyModelRepository>();

                  //Model Helpers
                  x.For<IVoiceMessageFolder>().Use<VoiceMessageFolder>();

                  //ModelHelpers from Utilities
                  x.For<IAsteriskAudioStream>().Use(_utilitycontainer.GetInstance<IAsteriskAudioStream>);
                  x.For<IMessageFolderManager>().Use(_utilitycontainer.GetInstance<IMessageFolderManager>);

                  //Models
                  x.For<IAccessCode>().Use<AccessCode>();
                  x.For<IAutoAttendant>().Use<AutoAttendant>();
                  x.For<IAutoAttendantRules>().Use<AutoAttendantRules>();
                  x.For<IBriTrunk>().Use<BriTrunk>();
                  x.For<ICLI>().Use<CLI>();
                  x.For<ICurrentDialPlan>().Use<CurrentDialPlan>();
                  x.For<IDahdiChannel>().Use<DahdiChannel>();
                  x.For<IDDI>().Use<DDI>();
                  x.For<IDefault>().Use<Default>();
                  x.For<IDialplan>().Use<Dialplan>();
                  x.For<IDialplanDate>().Use<DialplanDate>();
                  x.For<IDialplanRange>().Use<DialplanRange>();
                  x.For<IEmergencyNumber>().Use<EmergencyNumber>();
                  x.For<IExtension>().Use<Extension>();
                  x.For<IFourComFederatedLink>().Use<FourComFederatedLink>();
                  x.For<ISamsungFederatedLink>().Use<SamsungFederatedLink>();
                  x.For<IIaxTrunk>().Use<IaxTrunk>();
                  x.For<IKnownNumber>().Use<KnownNumber>();
                  x.For<IMusicOnHold>().Use<MusicOnHold>();
                  x.For<IPermisionClass>().Use<PermisionClass>();
                  x.For<IPermissionClassMember>().Use<PermissionClassMember>();
                  x.For<IPermissionPattern>().Use<PermissionPattern>();
                  x.For<IQueue>().Use<Queue>();
                  x.For<IQueueMember>().Use<QueueMember>();
                  x.For<IRingTone>().Use<RingTone>();
                  x.For<IRoutingRule>().Use<RoutingRule>();
                  x.For<IServer>().Use<Server>();
                  x.For<ISipTrunk>().Use<SipTrunk>();
                  x.For<ITrunk>().Use<Trunk>();
                  x.For<IUserConfig>().Use<UserConfig>();
                  x.For<IVoiceMail>().Use<VoiceMail>();
                  x.For<IVoiceMessage>().Use<VoiceMessage>();
              });

            Container = new Container(x => x.For<IRepository>().Use(_internalContainer.GetInstance<IRepository>));
        }
    }
}