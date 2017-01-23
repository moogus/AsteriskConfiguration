using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Internal;
using DataAccess.TableInterfaces;
using Moq;
using NHibernate;
using StructureMap;

namespace DataAccessTest.TestObjects
{
  public class MockDataObjects
  {
    public List<TestDataObject> ListOfClassesToTest { get; private set; }
    public IDataRepository StubRepository { get; private set; }
    public List<Tuple<Type, Mock>> ListOfValidTypes { get; private set; }
    public List<Type> ListOfInValidTypes { get; private set; }
    public Mock<IGetDatabaseTable> MockIGetDataObject { get; private set; }
    public Mock<IDataConnector> MockDataConnector { get; private set; }

    public MockDataObjects()
    {
      ListOfClassesToTest = new List<TestDataObject>();
      ListOfInValidTypes = new List<Type>();
      MockDataConnector = new Mock<IDataConnector>();
      MockIGetDataObject = new Mock<IGetDatabaseTable>();

      var mockAst = new Mock<IAstSipReg>();
      mockAst.Setup(a => a.Id).Returns(2);
      mockAst.Setup(a => a.Name).Returns("1200");

      var mockAstVoiceMessage = new Mock<IAstVoiceMessage>();
      mockAstVoiceMessage.Setup(a => a.Id).Returns(3);
      mockAstVoiceMessage.Setup(a => a.Name).Returns("1200");

      var mockAstVoicemail = new Mock<IAstVoicemail>();
      mockAstVoicemail.Setup(a => a.Id).Returns(4);
      mockAstVoicemail.Setup(a => a.Name).Returns("1200");

      var mockComAccessCode = new Mock<IComAccessCode>();
      mockComAccessCode.Setup(a => a.Id).Returns(5);
      mockComAccessCode.Setup(a => a.Name).Returns("1200");

      var mockComCLI = new Mock<IComCLI>();
      mockComCLI.Setup(a => a.Id).Returns(6);
      mockComCLI.Setup(a => a.Name).Returns("1200");

      var mockComDahdiChannel = new Mock<IComDahdiChannel>();
      mockComDahdiChannel.Setup(a => a.Id).Returns(7);
      mockComDahdiChannel.Setup(a => a.Name).Returns("1200");

      var mockExtension = new Mock<IComExtension>();
      mockExtension.Setup(a => a.Id).Returns(8);
      mockExtension.Setup(a => a.Name).Returns("1200");

      var mockComMusicOnHold = new Mock<IComMusicOnHold>();
      mockComMusicOnHold.Setup(a => a.Id).Returns(9);
      mockComMusicOnHold.Setup(a => a.Name).Returns("1200");

      var mockComQueue = new Mock<IComQueue>();
      mockComQueue.Setup(a => a.Id).Returns(2);
      mockComQueue.Setup(a => a.Name).Returns("1200");

      var mockComQueueMember = new Mock<IComQueueMember>();
      mockComQueueMember.Setup(a => a.Id).Returns(3);
      mockComQueueMember.Setup(a => a.Name).Returns("1200");

      var mockIComRoutingRule = new Mock<IComRoutingRule>();
      mockIComRoutingRule.Setup(a => a.Id).Returns(4);
      mockIComRoutingRule.Setup(a => a.Name).Returns("1200");

      var mockIComServer = new Mock<IComServer>();
      mockIComServer.Setup(a => a.Id).Returns(5);
      mockIComServer.Setup(a => a.Name).Returns("1200");

      var mockIComSipCredentials = new Mock<IComSipCredentials>();
      mockIComSipCredentials.Setup(a => a.Id).Returns(6);
      mockIComSipCredentials.Setup(a => a.Name).Returns("1200");

      var mockIComTrunk = new Mock<IComTrunk>();
      mockIComTrunk.Setup(a => a.Id).Returns(7);
      mockIComTrunk.Setup(a => a.Name).Returns("1200");

      var mockIFuAutoAttendant = new Mock<IFuAutoAttendant>();
      mockIFuAutoAttendant.Setup(a => a.Id).Returns(8);
      mockIFuAutoAttendant.Setup(a => a.Name).Returns("1200");

      var mockIFuAutoAttendantRules = new Mock<IFuAutoAttendantRules>();
      mockIFuAutoAttendantRules.Setup(a => a.Id).Returns(9);
      mockIFuAutoAttendantRules.Setup(a => a.Name).Returns("1200");


      var mockIFuContactDetails = new Mock<IFuContactDetails>();
      mockIFuContactDetails.Setup(a => a.Id).Returns(2);
      mockIFuContactDetails.Setup(a => a.Name).Returns("1200");

      var mockIFuCurrentDialplan = new Mock<IFuCurrentDialplan>();
      mockIFuCurrentDialplan.Setup(a => a.Id).Returns(3);
      mockIFuCurrentDialplan.Setup(a => a.Name).Returns("1200");

      var mockIfuDDI = new Mock<IFuDDI>();
      mockIfuDDI.Setup(a => a.Id).Returns(4);
      mockIfuDDI.Setup(a => a.Name).Returns("1200");

      var mockIFuDefaults = new Mock<IFuDefaults>();
      mockIFuDefaults.Setup(a => a.Id).Returns(5);
      mockIFuDefaults.Setup(a => a.Name).Returns("1200");

      var mockIFuDialplan = new Mock<IFuDialplan>();
      mockIFuDialplan.Setup(a => a.Id).Returns(6);
      mockIFuDialplan.Setup(a => a.Name).Returns("1200");

      var mockIFuDialplanDate = new Mock<IFuDialplanDate>();
      mockIFuDialplanDate.Setup(a => a.Id).Returns(7);
      mockIFuDialplanDate.Setup(a => a.Name).Returns("1200");

      var mockIFuDialplanRange = new Mock<IFuDialplanRange>();
      mockIFuDialplanRange.Setup(a => a.Id).Returns(8);
      mockIFuDialplanRange.Setup(a => a.Name).Returns("1200");

      var mockIFuEmergencyNumber = new Mock<IFuEmergencyNumber>();
      mockIFuEmergencyNumber.Setup(a => a.Id).Returns(9);
      mockIFuEmergencyNumber.Setup(a => a.Name).Returns("1200");

      var mockIFuIaxCredentials = new Mock<IFuIaxCredentials>();
      mockIFuIaxCredentials.Setup(a => a.Id).Returns(3);
      mockIFuIaxCredentials.Setup(a => a.Name).Returns("1200");

      var mockIFuKnownNumber = new Mock<IFuKnownNumber>();
      mockIFuKnownNumber.Setup(a => a.Id).Returns(4);
      mockIFuKnownNumber.Setup(a => a.Name).Returns("1200");

      var mockIFuPermisionClassMember = new Mock<IFuPermisionClassMember>();
      mockIFuPermisionClassMember.Setup(a => a.Id).Returns(5);
      mockIFuPermisionClassMember.Setup(a => a.Name).Returns("1200");

      var mockIFuPermissionClass = new Mock<IFuPermissionClass>();
      mockIFuPermissionClass.Setup(a => a.Id).Returns(6);
      mockIFuPermissionClass.Setup(a => a.Name).Returns("1200");

      var mockIFuPermissionPattern = new Mock<IFuPermissionPattern>();
      mockIFuPermissionPattern.Setup(a => a.Id).Returns(7);
      mockIFuPermissionPattern.Setup(a => a.Name).Returns("1200");

      var mockIFuRingtone = new Mock<IFuRingtone>();
      mockIFuRingtone.Setup(a => a.Id).Returns(8);
      mockIFuRingtone.Setup(a => a.Name).Returns("1200");

      var mockIFuUserConfig = new Mock<IFuUserConfig>();
      mockIFuUserConfig.Setup(a => a.Id).Returns(9);
      mockIFuUserConfig.Setup(a => a.Name).Returns("1200");


      ListOfValidTypes = new List<Tuple<Type, Mock>>
        { 
          new Tuple<Type, Mock>(typeof(IAstSipReg),mockAst) , 
          new Tuple<Type, Mock>(typeof(IAstVoiceMessage),mockAstVoiceMessage) ,
          new Tuple<Type, Mock>( typeof(IAstVoicemail),mockAstVoicemail) , 
          new Tuple<Type, Mock>( typeof(IComAccessCode),mockComAccessCode),
          new Tuple<Type, Mock>( typeof(IComCLI),mockComCLI),
          new Tuple<Type, Mock>( typeof(IComDahdiChannel),mockComDahdiChannel),
          new Tuple<Type, Mock>(  typeof(IComExtension),mockExtension),
          new Tuple<Type, Mock>(  typeof(IComMusicOnHold),mockComMusicOnHold),
          new Tuple<Type, Mock>( typeof(IComQueue), mockComQueue),
          new Tuple<Type, Mock>(   typeof(IComQueueMember) ,mockComQueueMember),
          new Tuple<Type, Mock>(  typeof(IComRoutingRule),mockIComRoutingRule ),
          new Tuple<Type, Mock>( typeof(IComServer) ,mockIComServer) ,
          new Tuple<Type, Mock>( typeof(IComSipCredentials),mockIComSipCredentials ),
          new Tuple<Type, Mock>(   typeof(IComTrunk) ,mockIComTrunk ),
          new Tuple<Type, Mock>(  typeof(IFuAutoAttendant), mockIFuAutoAttendant),
          new Tuple<Type, Mock>(  typeof(IFuAutoAttendantRules),mockIFuAutoAttendantRules),
          new Tuple<Type, Mock>( typeof(IFuContactDetails),mockIFuContactDetails ),
          new Tuple<Type, Mock>(   typeof(IFuCurrentDialplan) ,mockIFuCurrentDialplan),
          new Tuple<Type, Mock>(  typeof(IFuDDI),mockIfuDDI ),
          new Tuple<Type, Mock>(  typeof(IFuDefaults) ,mockIFuDefaults),
          new Tuple<Type, Mock>( typeof(IFuDialplan),mockIFuDialplan ),
          new Tuple<Type, Mock>( typeof(IFuDialplanDate) ,mockIFuDialplanDate),
          new Tuple<Type, Mock>( typeof(IFuDialplanRange),mockIFuDialplanRange ),
          new Tuple<Type, Mock>(   typeof(IFuEmergencyNumber),mockIFuEmergencyNumber),
          new Tuple<Type, Mock>( typeof(IFuIaxCredentials), mockIFuIaxCredentials),
          new Tuple<Type, Mock>( typeof(IFuKnownNumber) ,mockIFuKnownNumber),
          new Tuple<Type, Mock>( typeof(IFuPermisionClassMember), mockIFuPermisionClassMember),
          new Tuple<Type, Mock>( typeof(IFuPermissionClass) ,mockIFuPermissionClass),
          new Tuple<Type, Mock>( typeof(IFuPermissionPattern),mockIFuPermissionPattern ),
          new Tuple<Type, Mock>(typeof(IFuRingtone),mockIFuRingtone),
          new Tuple<Type, Mock>(  typeof(IFuUserConfig),mockIFuUserConfig)
        };


      foreach (var t in ListOfValidTypes)
      {
        var dTable = (IDatabaseTable)t.Item2.Object;
        var type = t.Item1;
        ListOfClassesToTest.Add(new TestDataObject(type, dTable.Id, dTable.Name));
        ListOfInValidTypes.Add(typeof(IDatabaseTable));
      }

      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IAstSipReg>()).Returns(mockAst.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IAstVoiceMessage>()).Returns(mockAstVoiceMessage.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IAstVoicemail>()).Returns(mockAstVoicemail.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComAccessCode>()).Returns(mockComAccessCode.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComCLI>()).Returns(mockComCLI.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComDahdiChannel>()).Returns(mockComDahdiChannel.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComExtension>()).Returns(mockExtension.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComMusicOnHold>()).Returns(mockComMusicOnHold.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComQueue>()).Returns(mockComQueue.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComQueueMember>()).Returns(mockComQueueMember.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComRoutingRule>()).Returns(mockIComRoutingRule.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComServer>()).Returns(mockIComServer.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComSipCredentials>()).Returns(mockIComSipCredentials.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IComTrunk>()).Returns(mockIComTrunk.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuAutoAttendant>()).Returns(mockIFuAutoAttendant.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuAutoAttendantRules>()).Returns(mockIFuAutoAttendantRules.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuContactDetails>()).Returns(mockIFuContactDetails.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuCurrentDialplan>()).Returns(mockIFuCurrentDialplan.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuDDI>()).Returns(mockIfuDDI.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuDefaults>()).Returns(mockIFuDefaults.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuDialplan>()).Returns(mockIFuDialplan.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuDialplanDate>()).Returns(mockIFuDialplanDate.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuDialplanRange>()).Returns(mockIFuDialplanRange.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuEmergencyNumber>()).Returns(mockIFuEmergencyNumber.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuIaxCredentials>()).Returns(mockIFuIaxCredentials.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuKnownNumber>()).Returns(mockIFuKnownNumber.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuPermisionClassMember>()).Returns(mockIFuPermisionClassMember.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuPermissionClass>()).Returns(mockIFuPermissionClass.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuPermissionPattern>()).Returns(mockIFuPermissionPattern.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuRingtone>()).Returns(mockIFuRingtone.Object);
      MockIGetDataObject.Setup(g => g.GetDatabaseTableInstance<IFuUserConfig>()).Returns(mockIFuUserConfig.Object);

      MockDataConnector.Setup(d => d.Session).Returns(new Mock<ISession>().Object);

      MockDataConnector.Setup(d => d.GetQuery<IAstSipReg>()).Returns(new List<IAstSipReg> { mockAst.Object }.AsQueryable);

      MockDataConnector.Setup(d => d.GetQuery<IAstVoiceMessage>()).Returns(new List<IAstVoiceMessage> { mockAstVoiceMessage.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IAstVoicemail>()).Returns(new List<IAstVoicemail> { mockAstVoicemail.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComAccessCode>()).Returns(new List<IComAccessCode> { mockComAccessCode.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComCLI>()).Returns(new List<IComCLI> { mockComCLI.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComDahdiChannel>()).Returns(new List<IComDahdiChannel> { mockComDahdiChannel.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComExtension>()).Returns(new List<IComExtension> { mockExtension.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComMusicOnHold>()).Returns(new List<IComMusicOnHold> { mockComMusicOnHold.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComQueue>()).Returns(new List<IComQueue> { mockComQueue.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComQueueMember>()).Returns(new List<IComQueueMember> { mockComQueueMember.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComRoutingRule>()).Returns(new List<IComRoutingRule> { mockIComRoutingRule.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComServer>()).Returns(new List<IComServer> { mockIComServer.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComSipCredentials>()).Returns(new List<IComSipCredentials> { mockIComSipCredentials.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IComTrunk>()).Returns(new List<IComTrunk> { mockIComTrunk.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuAutoAttendant>()).Returns(new List<IFuAutoAttendant> { mockIFuAutoAttendant.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuAutoAttendantRules>()).Returns(new List<IFuAutoAttendantRules> { mockIFuAutoAttendantRules.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuContactDetails>()).Returns(new List<IFuContactDetails> { mockIFuContactDetails.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuCurrentDialplan>()).Returns(new List<IFuCurrentDialplan> { mockIFuCurrentDialplan.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuDDI>()).Returns(new List<IFuDDI> { mockIfuDDI.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuDefaults>()).Returns(new List<IFuDefaults> { mockIFuDefaults.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuDialplan>()).Returns(new List<IFuDialplan> { mockIFuDialplan.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuDialplanDate>()).Returns(new List<IFuDialplanDate> { mockIFuDialplanDate.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuDialplanRange>()).Returns(new List<IFuDialplanRange> { mockIFuDialplanRange.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuEmergencyNumber>()).Returns(new List<IFuEmergencyNumber> { mockIFuEmergencyNumber.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuIaxCredentials>()).Returns(new List<IFuIaxCredentials> { mockIFuIaxCredentials.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuKnownNumber>()).Returns(new List<IFuKnownNumber> { mockIFuKnownNumber.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuPermisionClassMember>()).Returns(new List<IFuPermisionClassMember> { mockIFuPermisionClassMember.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuPermissionClass>()).Returns(new List<IFuPermissionClass> { mockIFuPermissionClass.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuPermissionPattern>()).Returns(new List<IFuPermissionPattern> { mockIFuPermissionPattern.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuRingtone>()).Returns(new List<IFuRingtone> { mockIFuRingtone.Object }.AsQueryable);
      MockDataConnector.Setup(d => d.GetQuery<IFuUserConfig>()).Returns(new List<IFuUserConfig> { mockIFuUserConfig.Object }.AsQueryable);

      StubRepository = new DatabaseRepository(MockDataConnector.Object, MockIGetDataObject.Object);
    }
  }
}