using Moq;
using PhoneApps.Models.Interfaces;

namespace PhoneAppTests.TestHelpers.Mocks
{
  internal class MockForwardingModelBase
  {
    private readonly RandomGenerator _randomGenerator;
    public Mock<IForwardingModel> TestForwardingModel { get; private set; }

    public MockForwardingModelBase()
    {
      _randomGenerator = new RandomGenerator();
      var randomNumberString = _randomGenerator.RandomString;
      var randomDescriptionString = _randomGenerator.RandomString;
      TestForwardingModel = new Mock<IForwardingModel>();
      TestForwardingModel.Setup(f => f.Number).Returns(randomNumberString);
      TestForwardingModel.Setup(f => f.Description).Returns(randomDescriptionString);
    }
  }
}
