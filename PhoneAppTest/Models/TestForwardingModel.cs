using NUnit.Framework;
using FluentAssertions;
using PhoneAppTests.TestHelpers.Mocks;

namespace PhoneAppTests.Models
{
  [TestFixture]
  public class TestForwardingModel
  {
    [Test]
    public void TestForwardingModelPropertiesPropertiesAreValid()
    {
      //Arrange
      var forwardingModelBase =new MockForwardingModelBase();

      //Assert
      forwardingModelBase.TestForwardingModel.Object.Number.Should().NotBeNull();
      forwardingModelBase.TestForwardingModel.Object.Description.Should().NotBeNull();
    }
  }
}
