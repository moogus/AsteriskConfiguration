using System;
using System.Collections.Specialized;
using System.Linq;
using DatabaseAccess;
using FluentAssertions;
using NUnit.Framework;
using PhoneAppTests.TestHelpers;
using PhoneAppTests.TestHelpers.Mocks;
using PhoneApps.Controllers;
using PhoneApps.ViewModels.Interfaces;

namespace PhoneAppTests.ViewModels
{
  [TestFixture]
  public class TestForwardingVM
  {
    private readonly MockForwardingControllerBase _forwardingControllerBase;
    private readonly ForwardingController _controller;
    private readonly RandomGenerator _randomGenerator;
    private readonly string _randomIp;

    public TestForwardingVM()
    {
      _randomGenerator = new RandomGenerator();

#if DEBUG
      _randomIp = "10.10.20.2";
#else
        _randomIp = _randomGenerator.RandomString;
#endif

      _forwardingControllerBase = new MockForwardingControllerBase();
      _controller = _forwardingControllerBase.TestForwadingController;
    }

    [Test]
    public void TestForwardingVmCurrentRuleType()
    {
      //Arrange
      //get variables
      var randomType = _randomGenerator.RandomForwrdingDestination;
      var randomDialPlan = _randomGenerator.RandomString;

      //get extension
      var extension = _forwardingControllerBase.LookupMockIGetExtensionFromIp.Object.GetExtension(_randomIp);
      //get dialplan
      var forwardingDp = _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetCurrentForwardingDialplan(randomDialPlan);

      _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Setup(f => f.GetForwardingType(extension, forwardingDp)).Returns(randomType);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client ip address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingVM model = GetActionResultForController.GetIndexViewModel(_controller);

      //Assert
      model.CurrentRuleType.Should().Be(randomType);

    }

    [Test]
    public void TestForwardingVmCurrentRuleDestination()
    {
      //Arrange
      string randomDestinationNumber = _randomGenerator.RandomString;
      var randomDialPlan = _randomGenerator.RandomString;

      //get extension
      var extension = _forwardingControllerBase.LookupMockIGetExtensionFromIp.Object.GetExtension(_randomIp);
      //get dialplan
      var forwardingDp = _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Object.GetCurrentForwardingDialplan(randomDialPlan);

      _forwardingControllerBase.LookupMockIGetForwardingFromExtension.Setup(f => f.GetForwardingNumber(extension, forwardingDp)).Returns(randomDestinationNumber);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client ip address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingVM model = GetActionResultForController.GetIndexViewModel(_controller);

      //Assert
      model.CurrentRuleDestination.Should().Be(randomDestinationNumber);
    }

    [Test]
    public void TestForwardingVmRouteTypes()
    {
      //Arrange
      MvcHelpers.AddServerVarsTo(_controller);

      //Act
      IForwardingVM model = GetActionResultForController.GetIndexViewModel(_controller);

      //Assert
      string[] vals = Enum.GetNames(typeof(ForwardingDestination));
      vals.Count(v => model.RouteTypes.Contains(v)).Should().Be(4);
    }

    [Test]
    public void TestForwardingVmCurrentExtensionIsMyNumber()
    {
      //Arrange
      //create random extn number
      string randomExt = _randomGenerator.RandomString;

      //create controller
      _forwardingControllerBase.LookupMockIGetExtensionFromIp.Setup(e => e.GetExtension(_randomIp)).Returns(randomExt);

      NameValueCollection serverVariables = MvcHelpers.AddServerVarsTo(_controller);

      // set client ip address
      serverVariables.Add("REMOTE_ADDR", _randomIp);

      //Act
      IForwardingVM model = GetActionResultForController.GetIndexViewModel(_controller);

      //Assert
      model.CurrentExtension.Should().Be(randomExt);
    }
  }
}