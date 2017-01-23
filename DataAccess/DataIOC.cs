using DataAccess.Internal;
using DataAccess.Internal.NHibernate;
using DataAccess.Internal.NHibernate.DataTables.Classes;
using DataAccess.TableInterfaces;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace DataAccess
{
  public class DataIOC:Registry
  {
    public DataIOC()
    {
      CreateContainer();
      For<IDataRepository>().Use(_internalContainer.GetInstance<IDataRepository>);
    }

    const string LiveConnection = "Server=10.10.1.50;Database=asterisk;User Id=asterisk;Password=password";
    const string DevConnection = "Server=10.10.20.183;Database=asterisk;User Id=asterisk;Password=password";

    private IContainer _internalContainer;
    internal IContainer Container { get; private set; }
    private void CreateContainer()
    {

      _internalContainer = new Container(x =>
      {
        //internal repo
        x.For<IDataConnector>().Use<NhibernateDataConnector>().Ctor<string>("connection").Is(DevConnection);
        x.For<IDataRepository>().Use<DatabaseRepository>();
        x.For<IGetDatabaseTable>().Use<GetDatabaseTable>();

        //table mappings
        x.For<IAstSipReg>().Use<AstSipReg>();
        x.For<IAstVoicemail>().Use<AstVoicemail>();
        x.For<IAstVoiceMessage>().Use<AstVoiceMessage>();
        x.For<IComAccessCode>().Use<ComAccessCode>();
        x.For<IComCLI>().Use<ComCLI>();
        x.For<IComDahdiChannel>().Use<ComDahdiChannel>();
        x.For<IComExtension>().Use<ComExtension>();
        x.For<IComMusicOnHold>().Use<ComMusicOnHold>();
        x.For<IComQueue>().Use<ComQueue>();
        x.For<IComQueueMember>().Use<ComQueueMember>();
        x.For<IComRoutingRule>().Use<ComRoutingRule>();
        x.For<IComServer>().Use<ComServer>();
        x.For<IComSipCredentials>().Use<ComSipCredentials>();
        x.For<IComTrunk>().Use<ComTrunk>();
        x.For<IFuAutoAttendant>().Use<FuAutoAttendant>();
        x.For<IFuAutoAttendantRules>().Use<FuAutoAttendantRules>();
        x.For<IFuContactDetails>().Use<FuContactDetails>();
        x.For<IFuCurrentDialplan>().Use<FuCurrentDialplan>();
        x.For<IFuDDI>().Use<FuDDI>();
        x.For<IFuDefaults>().Use<FuDefaults>();
        x.For<IFuDialplan>().Use<FuDialplan>();
        x.For<IFuDialplanDate>().Use<FuDialplanDate>();
        x.For<IFuDialplanRange>().Use<FuDialplanRange>();
        x.For<IFuEmergencyNumber>().Use<FuEmergencyNumber>();
        x.For<IFu4ComFederation>().Use<Fu4ComFederation>();
        x.For<IFuSamsungFederation>().Use<FuSamsungFederation>();
        x.For<IFuIaxCredentials>().Use<FuIaxCredentials>();
        x.For<IFuKnownNumber>().Use<FuKnownNumber>();
        x.For<IFuPermisionClassMember>().Use<FuPermisionClassMember>();
        x.For<IFuPermissionClass>().Use<FuPermissionClass>();
        x.For<IFuPermissionPattern>().Use<FuPermissionPattern>();
        x.For<IFuRingtone>().Use<FuRingtone>();
        x.For<IFuUserConfig>().Use<FuUserConfig>();
      });

      Container = new Container(x =>
      {
        x.For<IDataRepository>().Use(_internalContainer.GetInstance<IDataRepository>);
      });

    }
  }

}
