using Moq;
using NUnit.Framework;
using FluentAssertions;
using PhoneAppTests.TestHelpers;
using PhoneApps.Models;
using PhoneApps.Models.Interfaces;
using PhoneApps.Services.Interfaces;

namespace PhoneAppTests.Models
{
  [TestFixture]
  public class TestForwardingControllerModel
  {
    private readonly RandomGenerator _randomGenerator;

    public TestForwardingControllerModel()
    {
      _randomGenerator = new RandomGenerator(); ;
    }

    [Test]
    public void DoTest()
    {
      //Arrange
      var mockGetExtensionFromIp = new Mock<IGetExtensionFromIp>();
      var mockGetForwatrdingFromExtensions = new Mock<IGetForwardingFromExtension>();
      var mockGetForwardingModelList = new Mock<IGetForwardingModelList>();
      var mockSaveForwarding = new Mock<ISaveForwardingType>();
      //Act
      IForwardingControllerModel forwardingControllerModel = new ForwardingControllerModel(mockGetExtensionFromIp.Object,
                                                                                           mockGetForwatrdingFromExtensions.Object,
                                                                                           mockGetForwardingModelList.Object, mockSaveForwarding.Object);

      //Assert
      forwardingControllerModel.GetExtensionFromIp.Should().NotBeNull();
      forwardingControllerModel.GetForwardingFromExtension.Should().NotBeNull();
      forwardingControllerModel.GetForwardingModelList.Should().NotBeNull();
    }
  }
}
