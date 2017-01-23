using System;
using System.Collections.Generic;
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
  public class TestGetForwardingFromExtension
  {
    private readonly MockDatabaseAccess _createDatabaseAccess;
    private readonly Mock<IRepository> _testRepository;
    private readonly RandomGenerator _randomGenerator;
    private readonly ForwardingDestination _destinationType;
    private readonly string _destinationNumber;
    private readonly string _extensionNumber;

    public TestGetForwardingFromExtension()
    {
      _createDatabaseAccess = new MockDatabaseAccess();
      _testRepository = _createDatabaseAccess.MockRepository;
      _randomGenerator = new RandomGenerator();
      _destinationType = _randomGenerator.RandomForwrdingDestination;
      _destinationNumber = _randomGenerator.RandomString;
      _extensionNumber = _randomGenerator.RandomString;
    }

    [Test]
    public void TestSetBaseForwardingNoneAvailable()
    {
      //Arrange
     var listOfDialPlans =   new List<string>()
        {
          "unconditionalOff",
          "onBusyOff",
          "noAnwserOff"
        };
     
      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 0);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingOneAvailable()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOff",
          "onBusyOff",
          "noAnwserOff"
        };

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 1);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingTwoAvailable()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOff",
          "onBusyOff",
          "noAnwserOff"
        };

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 2);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingAllOff()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOff",
          "onBusyOff",
          "noAnwserOff"
        };

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 3);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingUnconditonalOn()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOn",
          "onBusyOff",
          "noAnwserOff"
        };

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 3);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingOnBusyOn()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOff",
          "onBusyOn",
          "noAnwserOff"
        }; 

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 3);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetBaseForwardingNoAnwserOn()
    {
      //Arrange
      var listOfDialPlans = new List<string>()
        {
          "unconditionalOff",
          "onBusyOff",
          "noAnwserOn"
        };

      CreateForwardingRoutesAndDialPlans(listOfDialPlans, 3);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      getForwardingFromExtension.SetBaseForwarding(_extensionNumber);

      //Assert
      var model = getForwardingFromExtension.ForwardingRules;
      model.Count().Should().Be(3);
    }

    [Test]
    public void TestSetCurrentDialplan()
    {
      //  //Arrange
      var dialplanName = _randomGenerator.RandomString;
      var mockDialplan = _createDatabaseAccess.GetMockDialPlan(dialplanName);

      //set list with dialplan
      _createDatabaseAccess.SetUpRepositoryDialplan(mockDialplan.Object);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.GetCurrentForwardingDialplan(mockDialplan.Object.Name);

      //Assert
      model.Should().Be(dialplanName);
    }

    [Test]
    public void TestGetForwardingType()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      var dialplanName = _randomGenerator.RandomString;

      var mockRoutingRule = _createDatabaseAccess.GetMockForwardRoutingRule(extensionNumber, dialplanName, _destinationType.ToString(), _destinationNumber);

      //set list with routing rule
      _createDatabaseAccess.SetUpRepositoryForwardingRoutingRules(mockRoutingRule.Object);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.GetForwardingType(extensionNumber, dialplanName);

      //Assert
      model.Should().Be(_destinationType);
    }

    [Test]
    public void TestGetForwardingTypeIsError()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      var dialplanName = _randomGenerator.RandomString;

      //No Rule available
      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.GetForwardingType(extensionNumber, dialplanName);

      //Assert
      model.Should().Be(ForwardingDestination.Error);
    }

    [Test]
    public void TestGetForwardingNumber()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      var dialplanName = _randomGenerator.RandomString;

      var mockRoutingRule = _createDatabaseAccess.GetMockForwardRoutingRule(extensionNumber, dialplanName, _destinationType.ToString(), _destinationNumber);

      //set list with routing rule from extension
      _createDatabaseAccess.SetUpRepositoryForwardingRoutingRules(mockRoutingRule.Object);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.GetForwardingNumber(extensionNumber, dialplanName);

      //Assert
      model.Should().Be(_destinationNumber);
     
    }

    [Test]
    public void TestGetForwardingNumberIsNotNull()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      var dialplanName = _randomGenerator.RandomString;

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.GetForwardingNumber(extensionNumber, dialplanName);

      //Assert
      model.Should().NotBeNull();

    }

    [Test]
    public void IsForwardingIsEnabledThereAtAll()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      string dialplanName = _randomGenerator.RandomString;


      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.IsForwardingEnabled(extensionNumber, dialplanName);

      //Assert
      model.Should().Be(false);
    }

    [Test]
    public void IsForwardingIsEnabled()
    {
      //Arrange
      var extensionNumber = _randomGenerator.RandomString;
      var dialplanName = _randomGenerator.RandomString + "On";

      var mockRoutingRule = _createDatabaseAccess.GetMockForwardRoutingRule(extensionNumber, dialplanName, _destinationType.ToString(), _destinationNumber);

      //set list with routing rule from extension
      _createDatabaseAccess.SetUpRepositoryForwardingRoutingRules(mockRoutingRule.Object);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.IsForwardingEnabled(extensionNumber, dialplanName);

      //Assert
      model.Should().Be(true);
    }

    [Test]
    public void IsForwardingIsNotEnabled()
    {
      //Arrange
      var dialplanName = _randomGenerator.RandomString + "Off";

      var mockRoutingRule = _createDatabaseAccess.GetMockForwardRoutingRule(_extensionNumber, dialplanName, _destinationType.ToString(), _destinationNumber);

      //set list with routing rule from extension
      _createDatabaseAccess.SetUpRepositoryForwardingRoutingRules(mockRoutingRule.Object);

      IGetForwardingFromExtension getForwardingFromExtension = new GetForwardingFromExtension(_testRepository.Object);

      //Act
      var model = getForwardingFromExtension.IsForwardingEnabled(_extensionNumber, dialplanName);

      //Assert
      model.Should().Be(false);
    }
    
    private void CreateForwardingRoutesAndDialPlans(List<string> listOdDialPlans, int numRules)
    {
      var r1 = CreateMockRoutingRule(listOdDialPlans[0]);
      var r2 = CreateMockRoutingRule(listOdDialPlans[1]);
      var r3 = CreateMockRoutingRule(listOdDialPlans[2]);


      CreateMockForwardingDialPlans();

      var mockRouteList = new List<IRoutingRule>();


      switch (numRules)
      {
        case 0:
          break;

        case 1:
          mockRouteList = new List<IRoutingRule> { r1.Object };
          break;

        case 2:
          mockRouteList = new List<IRoutingRule> { r1.Object, r2.Object };
          break;

        case 3:
          mockRouteList = new List<IRoutingRule> { r1.Object, r2.Object, r3.Object };
          break;

      }

      //set list with dialplan
      _createDatabaseAccess.SetUpRepositoryForwardingRoutingRulesList(mockRouteList);
    }

    private void CreateMockForwardingDialPlans()
    {
      var dialPlanList = new List<IDialplan>
      {
      _createDatabaseAccess.GetMockDialPlan("unconditionalOn").Object,
      _createDatabaseAccess.GetMockDialPlan("unconditionalOff").Object,
      _createDatabaseAccess.GetMockDialPlan("onBusyOn").Object,
      _createDatabaseAccess.GetMockDialPlan("onBusyOff").Object,
      _createDatabaseAccess.GetMockDialPlan("noAnwserOn").Object,
      _createDatabaseAccess.GetMockDialPlan("noAnwserOff").Object
    };

      _createDatabaseAccess.SetUpRepositoryDialplanList(dialPlanList);
      _createDatabaseAccess.SetUpRepositoryDialplanListGetByName(dialPlanList);
      foreach (var dialplan in dialPlanList)
      {
        _createDatabaseAccess.SetUpRepositoryAddRule(_extensionNumber, dialplan);
      }
    }

    private Mock<IRoutingRule> CreateMockRoutingRule(string dialplan)
    {
      var rule = new Mock<IRoutingRule>();
      rule.Setup(r => r.Dialplan.Name).Returns(dialplan);
      rule.Setup(r => r.Number).Returns(_extensionNumber);
      return rule;
    }


  }
}
