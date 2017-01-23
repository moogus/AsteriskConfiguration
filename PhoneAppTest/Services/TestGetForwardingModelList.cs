using System.Linq;
using DatabaseAccess;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using PhoneAppTests.TestHelpers;
using PhoneAppTests.TestHelpers.Mocks;
using PhoneApps.Services;
using PhoneApps.Services.Interfaces;

namespace PhoneAppTests.Services
{
  [TestFixture]
  public class TestServiceInterfaces
  {
    private readonly MockDatabaseAccess _createDatabaseAccess;
    private readonly Mock<IRepository> _testRepository;
    private readonly RandomGenerator _randomGenerator;

    public TestServiceInterfaces()
    {
      _randomGenerator = new RandomGenerator();
      _createDatabaseAccess = new MockDatabaseAccess();
      _testRepository = _createDatabaseAccess.MockRepository;
    }

    [Test]
    public void TestGetForwardingModelsFromTypeIsExtension()
    {
      //create variables
      const ForwardingDestination type = ForwardingDestination.Extension;
      var extensionNumber = _randomGenerator.RandomString;
      var mockExtension = _createDatabaseAccess.GetMockExtension(extensionNumber);

      //set list with extension
      _createDatabaseAccess.SetUpRepositoryExtension(mockExtension.Object);

      IGetForwardingModelList getForwardingModelList = new GetForwardingModelList(_testRepository.Object);

      //Act
      var models = getForwardingModelList.GetForwadingModelsFromType(type);

      //Assert
      models.Select(m => m.Number).First().Should().Be(extensionNumber);
    }

    [Test]
    public void TestGetForwardingModelsFromTypeIsQueue()
    {
      //create variables 
      const ForwardingDestination type = ForwardingDestination.Group;
      var queueNumber = _randomGenerator.RandomString;

      var mockQueue = _createDatabaseAccess.GetMockQueue(queueNumber);

      //set list with queue
      _createDatabaseAccess.SetUpRepositoryQueue(mockQueue.Object);

      IGetForwardingModelList getForwardingModelList = new GetForwardingModelList(_testRepository.Object);

      //Act
      var models = getForwardingModelList.GetForwadingModelsFromType(type);

      //Assert
      models.Select(m => m.Number).First().Should().Be(queueNumber);
    }

    [Test]
    public void TestGetForwardingModelsFromTypeIsVoicemail()
    {
      //create variables 
      const ForwardingDestination type = ForwardingDestination.Voicemail;
      var voiceMailNumber = _randomGenerator.RandomString;
      var mockVoiceMail = _createDatabaseAccess.GetMockVoiceMail(voiceMailNumber);

      //set list with VoiceMail
      _createDatabaseAccess.SetUpRepositoryVoiceMail(mockVoiceMail.Object);

      IGetForwardingModelList getForwardingModelList = new GetForwardingModelList(_testRepository.Object);

      //Act
      var models = getForwardingModelList.GetForwadingModelsFromType(type);

      //Assert
      models.Select(m => m.Number).First().Should().Be(voiceMailNumber);
    }

    [Test]
    public void TestGetForwardingModelsFromTypeIsExternal()
    {
      //Arrange
      const ForwardingDestination type = ForwardingDestination.External;
      var mockRepository = new Mock<IRepository>();
      IGetForwardingModelList getForwardingModelList = new GetForwardingModelList(mockRepository.Object);

      //Act
      var models = getForwardingModelList.GetForwadingModelsFromType(type);

      //Assert
      models.Select(m => m.Number).First().Should().Be(string.Empty);
    }
  }
}
