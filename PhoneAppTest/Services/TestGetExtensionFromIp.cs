using System.Collections.Generic;
using DatabaseAccess;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using PhoneAppTests.TestHelpers;
using PhoneApps.Services;
using PhoneApps.Services.Interfaces;

namespace PhoneAppTests.Services
{
  [TestFixture]
  public class TestGetExtensionFromIp
  {
    private readonly string _ip;
    private readonly string _extension;
    private readonly Mock<IExtension> _mockExtension;
    private readonly Mock<IRepository> _mockRepository;

    public TestGetExtensionFromIp()
    {
      var randomGenerator = new RandomGenerator();
      _ip = randomGenerator.RandomString;
      _extension = randomGenerator.RandomString;
      _mockExtension = new Mock<IExtension>();
      _mockExtension.Setup(e => e.Number).Returns(_extension);
      _mockExtension.Setup(e => e.IpAddress).Returns(_ip);
      _mockRepository = new Mock<IRepository>();
      _mockRepository.Setup(g => g.GetList<IExtension>()).Returns(new List<IExtension> { _mockExtension.Object });
    }

    [Test]
    public void TestGetExtensionIsExtension()
    {
      //Arrange --done above in constructor
      //Act
      IGetExtensionFromIp getExtensionFromIp = new GetExtensionFromIp(_mockRepository.Object);

      var models = getExtensionFromIp.GetExtension(_ip);

      //Assert
      models.Should().Be(_extension);
    }
  }

}
