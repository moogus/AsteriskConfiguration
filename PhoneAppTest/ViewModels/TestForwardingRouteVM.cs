using System.Collections.Generic;
using System.Collections.Specialized;
using FluentAssertions;
using NUnit.Framework;
using PhoneAppTests.TestHelpers;
using PhoneAppTests.TestHelpers.Mocks;
using PhoneApps.Controllers;
using PhoneApps.Models.Interfaces;
using PhoneApps.ViewModels.Interfaces;

namespace PhoneAppTests.ViewModels
{
  [TestFixture]
  public class TestForwardingRouteVM
  {
    private readonly MockForwardingControllerBase _forwardingControllerBase;
    private readonly ForwardingController _controller;
    private readonly RandomGenerator _randomGenerator;
    private readonly string _randomIp;

    public TestForwardingRouteVM()
    {
      _randomGenerator = new RandomGenerator();

#if DEBUG
      _randomIp =  "10.10.20.2";
#else
        _randomIp = _randomGenerator.RandomString;
#endif
   
      _forwardingControllerBase = new MockForwardingControllerBase();
       _controller = _forwardingControllerBase.TestForwadingController;
    }

    [Test]
    public void TestForwardingRouteVMCurrentExtensionIsMyExtension()
    {
      //Arrange
      //create random extn number
      string randomExt = _randomGenerator.RandomString;

      _forwardingControllerBase.LookupMockIGetExtensionFromIp.Setup(e => e.GetExtension(_randomIp)).Returns(randomExt);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client extension address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingRouteVM model = GetActionResultForController.GetGetForwardingRoutesViewModel(_controller);

      //Assert
      model.CurrentExtension.Should().Be(randomExt);
    }

    [Test]
    public void TestForwardingVmIsEnabled()
    {
      //Arrange
      var randomDialPlan = _randomGenerator.RandomString;

      //get extension
      var extension = _forwardingControllerBase.LookupMockIGetExtensionFromIp.Object.GetExtension(_randomIp);
      //get dialplan
      var forwardingDp = _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetCurrentForwardingDialplan(randomDialPlan);

      _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Setup(f => f.IsForwardingEnabled(extension,forwardingDp)).Returns(true);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client extension address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingRouteVM model = GetActionResultForController.GetGetForwardingRoutesViewModel(_controller);

      //Assert
      model.IsEnabled.Should().Be(true);
    }

    [Test]
    public void TestForwardingVmIsNotEnabled()
    {
      //Arrange
     // var randomIp = _randomGenerator.RandomString;
      var randomDialPlan = _randomGenerator.RandomString;

      //get extension
      var extension = _forwardingControllerBase.LookupMockIGetExtensionFromIp.Object.GetExtension(_randomIp);
      //get dialplan
      var forwardingDp = _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetCurrentForwardingDialplan(randomDialPlan);

      _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Setup(f => f.IsForwardingEnabled(extension,forwardingDp)).Returns(false);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client extension address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingRouteVM model = GetActionResultForController.GetGetForwardingRoutesViewModel(_controller);

      //Assert
      model.IsEnabled.Should().Be(false);
    }

    [Test]
    public void TestForwardingVmRoutesInformation()
    {
      //Arrange

      //Create mock variables
      var randomDialPlan = _randomGenerator.RandomString;

      //get extension
      var extension = _forwardingControllerBase.LookupMockIGetExtensionFromIp.Object.GetExtension(_randomIp);
      //get dialplan
      var forwardingDp =_forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetCurrentForwardingDialplan(randomDialPlan);

      //create mock forwarding model
      var mockForwardingModel = new MockForwardingModelBase().TestForwardingModel;

      //create mock forwarding list
      var listOfMockForwardingModel = new List<IForwardingModel> { mockForwardingModel.Object };
     
      _forwardingControllerBase.LookupMockIGetForwardingModels.Setup(r => r.GetForwadingModelsFromType(
                                                                                      _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetForwardingType(extension, forwardingDp))
                                                                                      ).Returns(listOfMockForwardingModel);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client extension address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingRouteVM model = GetActionResultForController.GetGetForwardingRoutesViewModel(_controller);

      //Assert
      model.RoutesInformation.Should().Contain(mockForwardingModel.Object);
    }

 }
}
