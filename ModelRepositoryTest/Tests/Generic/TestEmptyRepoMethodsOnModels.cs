using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.ModelInterfaces;
using Moq;
using StructureMap;

namespace ModelRepositoryTest.Tests.Generic
{
  [TestClass]
  public class TestEmptyRepoMethodsOnModels
  {
    private ModelRepository.Internal.ModelRepositoryWithMapping _stubModelRepositoryWithMapping;
    private readonly Mock<IEmptyModelRepository> _emptyRepo;
    private const int NumberOfTestedModels = 31;

    public TestEmptyRepoMethodsOnModels()
    {
      _emptyRepo = new Mock<IEmptyModelRepository>();
    }

    [TestMethod]
    public void TestGetFromIdModels()
    {
      //Arrange
      _stubModelRepositoryWithMapping = new ModelRepository.Internal.ModelRepositoryWithMapping(_emptyRepo.Object, new Mock<IContainer>().Object);

      var anyInt = new Random().Next();

      //Act
      var listOfModels = new List<IModel>
        {
          _stubModelRepositoryWithMapping.GetFromId<IAccessCode>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IAutoAttendant>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IAutoAttendantRules>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IBriTrunk>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<ICLI>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<ICurrentDialPlan>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDahdiChannel>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDDI>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDefault>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDialplan>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDialplanRange>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IDialplanDate>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IEmergencyNumber>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IExtension>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IFederation>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IIaxTrunk>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IKnownNumber>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IMusicOnHold>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IPermissionPattern>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IPermisionClass>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IPermissionClassMember>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IQueue>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IQueueMember>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IRingTone>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IRoutingRule>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IServer>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<ITrunk>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<ISipTrunk>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IUserConfig>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IVoiceMail>(anyInt),
          _stubModelRepositoryWithMapping.GetFromId<IVoiceMessage>(anyInt)
        };

      //Assert
      _emptyRepo.Verify(x => x.GetFromId<IExtension>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDDI>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<ICLI>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IAutoAttendant>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IAutoAttendantRules>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<ICurrentDialPlan>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDefault>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDialplan>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDialplanRange>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDialplanDate>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IEmergencyNumber>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IFederation>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IKnownNumber>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IPermissionPattern>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IPermisionClass>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IPermissionClassMember>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IQueue>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IQueueMember>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IRingTone>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IRoutingRule>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IServer>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IAccessCode>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IDahdiChannel>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<ITrunk>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<ISipTrunk>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IIaxTrunk>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IBriTrunk>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IUserConfig>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IVoiceMail>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IVoiceMessage>(anyInt), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromId<IMusicOnHold>(anyInt), Times.Exactly(1));

      listOfModels.Count.Should().Be(NumberOfTestedModels);
    }


    [TestMethod]
    public void TestGetFromNameModels()
    {
      //Arrange
      _stubModelRepositoryWithMapping = new ModelRepository.Internal.ModelRepositoryWithMapping(_emptyRepo.Object, new Mock<IContainer>().Object);

      var anyString = new Random().Next().ToString(CultureInfo.InvariantCulture);

      //Act

      var listOfModels = new List<IModel>
        {
          _stubModelRepositoryWithMapping.GetFromName<IExtension>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDDI>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<ICLI>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IAutoAttendant>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IAutoAttendantRules>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<ICurrentDialPlan>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDefault>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDialplan>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDialplanRange>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDialplanDate>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IEmergencyNumber>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IFederation>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IKnownNumber>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IPermissionPattern>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IPermisionClass>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IPermissionClassMember>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IQueue>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IQueueMember>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IRingTone>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IRoutingRule>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IServer>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IAccessCode>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IDahdiChannel>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<ITrunk>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<ISipTrunk>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IIaxTrunk>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IBriTrunk>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IUserConfig>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IVoiceMail>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IVoiceMessage>(anyString),
          _stubModelRepositoryWithMapping.GetFromName<IMusicOnHold>(anyString)
        };

      //Assert
      _emptyRepo.Verify(x => x.GetFromName<IExtension>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDDI>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<ICLI>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IAutoAttendant>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IAutoAttendantRules>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<ICurrentDialPlan>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDefault>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDialplan>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDialplanRange>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDialplanDate>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IEmergencyNumber>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IFederation>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IKnownNumber>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IPermissionPattern>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IPermisionClass>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IPermissionClassMember>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IQueue>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IQueueMember>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IRingTone>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IRoutingRule>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IServer>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IAccessCode>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IDahdiChannel>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<ITrunk>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<ISipTrunk>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IIaxTrunk>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IBriTrunk>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IUserConfig>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IVoiceMail>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IVoiceMessage>(anyString), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetFromName<IMusicOnHold>(anyString), Times.Exactly(1));

      listOfModels.Count.Should().Be(NumberOfTestedModels);
    }

    [TestMethod]
    public void TestGetListModels()
    {
      //Arrange
      _stubModelRepositoryWithMapping = new ModelRepository.Internal.ModelRepositoryWithMapping(_emptyRepo.Object, new Mock<IContainer>().Object);

      //Act

      var listOfModels = new List<IEnumerable<IModel>>
        {
          _stubModelRepositoryWithMapping.GetList<IExtension>(),
          _stubModelRepositoryWithMapping.GetList<IDDI>(),
          _stubModelRepositoryWithMapping.GetList<ICLI>(),
          _stubModelRepositoryWithMapping.GetList<IAutoAttendant>(),
          _stubModelRepositoryWithMapping.GetList<IAutoAttendantRules>(),
          _stubModelRepositoryWithMapping.GetList<ICurrentDialPlan>(),
          _stubModelRepositoryWithMapping.GetList<IDefault>(),
          _stubModelRepositoryWithMapping.GetList<IDialplan>(),
          _stubModelRepositoryWithMapping.GetList<IDialplanRange>(),
          _stubModelRepositoryWithMapping.GetList<IDialplanDate>(),
          _stubModelRepositoryWithMapping.GetList<IEmergencyNumber>(),
          _stubModelRepositoryWithMapping.GetList<IFederation>(),
          _stubModelRepositoryWithMapping.GetList<IKnownNumber>(),
          _stubModelRepositoryWithMapping.GetList<IPermissionPattern>(),
          _stubModelRepositoryWithMapping.GetList<IPermisionClass>(),
          _stubModelRepositoryWithMapping.GetList<IPermissionClassMember>(),
          _stubModelRepositoryWithMapping.GetList<IQueue>(),
          _stubModelRepositoryWithMapping.GetList<IQueueMember>(),
          _stubModelRepositoryWithMapping.GetList<IRingTone>(),
          _stubModelRepositoryWithMapping.GetList<IRoutingRule>(),
          _stubModelRepositoryWithMapping.GetList<IServer>(),
          _stubModelRepositoryWithMapping.GetList<IAccessCode>(),
          _stubModelRepositoryWithMapping.GetList<IDahdiChannel>(),
          _stubModelRepositoryWithMapping.GetList<ITrunk>(),
          _stubModelRepositoryWithMapping.GetList<ISipTrunk>(),
          _stubModelRepositoryWithMapping.GetList<IIaxTrunk>(),
          _stubModelRepositoryWithMapping.GetList<IBriTrunk>(),
          _stubModelRepositoryWithMapping.GetList<IUserConfig>(),
          _stubModelRepositoryWithMapping.GetList<IVoiceMail>(),
          _stubModelRepositoryWithMapping.GetList<IVoiceMessage>(),
          _stubModelRepositoryWithMapping.GetList<IMusicOnHold>()
        };

      //Assert
      _emptyRepo.Verify(x => x.GetList<IExtension>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDDI>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<ICLI>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IAutoAttendant>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IAutoAttendantRules>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<ICurrentDialPlan>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDefault>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDialplan>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDialplanRange>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDialplanDate>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IEmergencyNumber>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IFederation>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IKnownNumber>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IPermissionPattern>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IPermisionClass>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IPermissionClassMember>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IQueue>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IQueueMember>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IRingTone>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IRoutingRule>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IServer>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IAccessCode>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IDahdiChannel>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<ITrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<ISipTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IBriTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IIaxTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IUserConfig>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IVoiceMail>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IVoiceMessage>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.GetList<IMusicOnHold>(), Times.Exactly(1));

      listOfModels.Count.Should().Be(NumberOfTestedModels);
    }

    [TestMethod]
    public void TestAddModels()
    {
      //Arrange
      _stubModelRepositoryWithMapping = new ModelRepository.Internal.ModelRepositoryWithMapping(_emptyRepo.Object, new Mock<IContainer>().Object);

      //Act

      var listOfModels = new List<IModel>
        {
          _stubModelRepositoryWithMapping.Add<IExtension>(),
          _stubModelRepositoryWithMapping.Add<IDDI>(),
          _stubModelRepositoryWithMapping.Add<ICLI>(),
          _stubModelRepositoryWithMapping.Add<IAutoAttendant>(),
          _stubModelRepositoryWithMapping.Add<IAutoAttendantRules>(),
          _stubModelRepositoryWithMapping.Add<ICurrentDialPlan>(),
          _stubModelRepositoryWithMapping.Add<IDefault>(),
          _stubModelRepositoryWithMapping.Add<IDialplan>(),
          _stubModelRepositoryWithMapping.Add<IDialplanRange>(),
          _stubModelRepositoryWithMapping.Add<IDialplanDate>(),
          _stubModelRepositoryWithMapping.Add<IEmergencyNumber>(),
          _stubModelRepositoryWithMapping.Add<IFederation>(),
          _stubModelRepositoryWithMapping.Add<IKnownNumber>(),
          _stubModelRepositoryWithMapping.Add<IPermissionPattern>(),
          _stubModelRepositoryWithMapping.Add<IPermisionClass>(),
          _stubModelRepositoryWithMapping.Add<IPermissionClassMember>(),
          _stubModelRepositoryWithMapping.Add<IQueue>(),
          _stubModelRepositoryWithMapping.Add<IQueueMember>(),
          _stubModelRepositoryWithMapping.Add<IRingTone>(),
          _stubModelRepositoryWithMapping.Add<IRoutingRule>(),
          _stubModelRepositoryWithMapping.Add<IServer>(),
          _stubModelRepositoryWithMapping.Add<IAccessCode>(),
          _stubModelRepositoryWithMapping.Add<IDahdiChannel>(),
          _stubModelRepositoryWithMapping.Add<ITrunk>(),
          _stubModelRepositoryWithMapping.Add<ISipTrunk>(),
          _stubModelRepositoryWithMapping.Add<IIaxTrunk>(),
          _stubModelRepositoryWithMapping.Add<IBriTrunk>(),
          _stubModelRepositoryWithMapping.Add<IUserConfig>(),
          _stubModelRepositoryWithMapping.Add<IVoiceMail>(),
          _stubModelRepositoryWithMapping.Add<IVoiceMessage>(),
          _stubModelRepositoryWithMapping.Add<IMusicOnHold>()
        };



      //Assert
      _emptyRepo.Verify(x => x.Add<IExtension>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDDI>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<ICLI>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IAutoAttendant>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IAutoAttendantRules>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<ICurrentDialPlan>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDefault>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDialplan>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDialplanRange>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDialplanDate>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IEmergencyNumber>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IFederation>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IKnownNumber>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IPermissionPattern>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IPermisionClass>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IPermissionClassMember>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IQueue>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IQueueMember>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IRingTone>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IRoutingRule>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IServer>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IAccessCode>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IDahdiChannel>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<ITrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<ISipTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IIaxTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IBriTrunk>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IUserConfig>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IVoiceMail>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IVoiceMessage>(), Times.Exactly(1));
      _emptyRepo.Verify(x => x.Add<IMusicOnHold>(), Times.Exactly(1));

      listOfModels.Count.Should().Be(NumberOfTestedModels);
    }

  }
}
