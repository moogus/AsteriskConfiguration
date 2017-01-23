using Moq;
using PhoneApps.Controllers;
using PhoneApps.Models.Interfaces;
using PhoneApps.Services.Interfaces;

namespace PhoneAppTests.TestHelpers.Mocks
{
  internal class MockForwardingControllerBase
  {
    public Mock<IGetExtensionFromIp> LookupMockIGetExtensionFromIp { get; set; }
    public Mock<IGetForwardingFromExtension> LookupMockIGetForwardingFromExtension { get; set; }
    public Mock<IGetForwardingModelList> LookupMockIGetForwardingModels { get; set; }

    public Mock<IForwardingControllerModel> MockForwardingControllerModel { get; set; }

    public ForwardingController TestForwadingController { get; private set; }

    internal MockForwardingControllerBase()
    {
      LookupMockIGetExtensionFromIp = new Mock<IGetExtensionFromIp>();
      LookupMockIGetForwardingFromExtension = new Mock<IGetForwardingFromExtension>();
      LookupMockIGetForwardingModels = new Mock<IGetForwardingModelList>();

      MockForwardingControllerModel = new Mock<IForwardingControllerModel>();
      MockForwardingControllerModel.Setup(m => m.GetExtensionFromIp).Returns(LookupMockIGetExtensionFromIp.Object);
      MockForwardingControllerModel.Setup(m => m.GetForwardingFromExtension).Returns(LookupMockIGetForwardingFromExtension.Object);
      MockForwardingControllerModel.Setup(m => m.GetForwardingModelList).Returns(LookupMockIGetForwardingModels.Object);
    
      TestForwadingController = new ForwardingController(MockForwardingControllerModel.Object);
    }
  }
}