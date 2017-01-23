using System.Web.Mvc;
using FluentAssertions;
using NUnit.Framework;
using PhoneAppTests.TestHelpers;
using PhoneAppTests.TestHelpers.Mocks;
using PhoneApps.Controllers;
using PhoneApps.ViewModels.Interfaces;

namespace PhoneAppTests.Controllers
{
  [TestFixture]
  public class TestForwardingController
  {
    private readonly MockForwardingControllerBase _forwardingControllerBase;
    private readonly ForwardingController _controller;


    public TestForwardingController()
    {
      _forwardingControllerBase = new MockForwardingControllerBase();
      _controller = _forwardingControllerBase.TestForwadingController;
    
    }
    [Test]
    public void TestControllerReturnsIndexView()
    {
      //Arrange
      MvcHelpers.AddServerVarsTo(_controller);

      //Act
      var view = _controller.Index() as ViewResult;

      //Assert
      view.ViewName.Should().Be("Index");
    }

    [Test]
    public void TestIndexVMType()
    {
      //Arange
      MvcHelpers.AddServerVarsTo(_controller);

      //Act
      IForwardingVM model = GetActionResultForController.GetIndexViewModel(_controller);

      //Assert
      model.Should().NotBeNull();
    }

    [Test]
    public void TestControllerReturnsGetForwardingRoutes()
    {
      //Arrange
      MvcHelpers.AddServerVarsTo(_controller);

      //Act
      var view = _controller.GetForwardingRoutes(new RandomGenerator().RandomString) as ViewResult;

      //Assert
      view.ViewName.Should().Be("GetForwardingRoutes");
    }

    [Test]
    public void TestGetForwardingRoutesVMType()
    {
      //Arrange
      MvcHelpers.AddServerVarsTo(_controller);

      //Act
      
      IForwardingRouteVM model = GetActionResultForController.GetGetForwardingRoutesViewModel(_controller);

      //Assert
      model.Should().NotBeNull();
    }

    [Test]
    public void TestCreateForwardingModel()
    {
      //Arrange


      //Act


      //Assert

    }

  }
}