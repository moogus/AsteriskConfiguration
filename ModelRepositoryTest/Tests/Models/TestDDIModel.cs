using System;
using System.Collections.Generic;
using System.Globalization;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using FluentAssertions;
using Moq;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestDDIModel
  {
    private readonly Mock<IDDI> _mockDdi;

    public TestDDIModel()
    {
      _mockDdi = new Mock<IDDI>();
    }

    [TestMethod]
    public void TestDDINotUsed()
    {
      //Arrange
      var repo = new Mock<IRepository>();
      _mockDdi.Setup(d => d.UsedOn).Returns(DDIUsedOn.NotUsed);
      _mockDdi.Setup(d => d.Used).Returns(string.Empty);

      //Act
      repo.Setup(r => r.Add<IDDI>()).Returns(_mockDdi.Object);

      var ddi = repo.Object.Add<IDDI>();

      //Assert 
      ddi.UsedOn.Should().Be(DDIUsedOn.NotUsed);
      ddi.Used.Should().Be(string.Empty);
    }

    [TestMethod]
    public void TestDDIOnExtension()
    {
      //Arrange
      //create mock modelRepository
      var randomNumber = new Random().Next();
      var repo = new Mock<IRepositoryWithDelete>();
      //make repo return a mock extension on getList<IExtn>
      var extn = new Mock<IExtension>();
      var extnList = new List<IExtension> {extn.Object};
      repo.Setup(r => r.GetList<IExtension>()).Returns(extnList);

      //make DDI
      var ddiToTest = new DDI(new Mock<IFuDDI>().Object, repo.Object);

      //Act
      //make mock extn return that DDI on its DDI getter
      extn.SetupProperty(e => e.DDI, ddiToTest);
      extn.SetupProperty(e => e.Number, randomNumber.ToString(CultureInfo.InvariantCulture));

      //Assert
      //check that ddi.UsedOn is extn
      ddiToTest.UsedOn.Should().Be(DDIUsedOn.Extension);
      ddiToTest.Used.Should().Be(string.Format("Extension: {0}", extn.Object.Number));
    }

    [TestMethod]
    public void TestDDIOnQueue()
    {
      //Arrange
      //create mock modelRepository
      var randomNumber = new Random().Next();
      var repo = new Mock<IRepositoryWithDelete>();
      //make repo return a mock extension on getList<IExtn>
      var queue = new Mock<IQueue>();
      var queueList = new List<IQueue> { queue.Object };
      repo.Setup(r => r.GetList<IQueue>()).Returns(queueList);

      //make DDI
      var ddiToTest = new DDI(new Mock<IFuDDI>().Object, repo.Object);

      //Act
      //make mock queue return that DDI on its DDI getter
      queue.SetupProperty(e => e.DDI, ddiToTest);
      queue.SetupProperty(e => e.Number, randomNumber.ToString(CultureInfo.InvariantCulture));

      //Assert
      //check that ddi.UsedOn is queue
      ddiToTest.UsedOn.Should().Be(DDIUsedOn.Queue);
      ddiToTest.Used.Should().Be(string.Format("Queue: {0}", queue.Object.Number));
    }

    [TestMethod]
    public void TestDDIOnRule()
    {
      //Arrange
      //create mock modelRepository
      var randomNumber = new Random().Next();
      var randomDialplan = new Random().Next(0, 11);
      var repo = new Mock<IRepositoryWithDelete>();
      //make repo return a mock extension on getList<IExtn>
      var rule = new Mock<IRoutingRule>();
      var ruleList = new List<IRoutingRule> { rule.Object };
      repo.Setup(r => r.GetList<IRoutingRule>()).Returns(ruleList);

      var mockFuDDI = new Mock<IFuDDI>();
      mockFuDDI.Setup(f => f.DDI).Returns(randomNumber.ToString(CultureInfo.InvariantCulture));

      //make DDI
      var ddiToTest = new DDI(mockFuDDI.Object, repo.Object);

      //Act
      //make mock rule return that DDI Number on its DDI getter
      rule.SetupProperty(e => e.Number, ddiToTest.DDINumber);
      rule.Setup(e => e.Dialplan.Id).Returns(randomDialplan);

      //Assert
      //check that ddi.UsedOn is rule
      ddiToTest.UsedOn.Should().Be(DDIUsedOn.Rule);
      ddiToTest.Used.Should().Be(string.Format("Route: {0}", rule.Object.Number));
    }

    [TestMethod]
    public void TestDDIOnTrunkDefault()
    {
      //Arrange
      //create mock modelRepository
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      //make repo return a mock extension on getList<IExtn>
      var trunk = new Mock<ITrunk>();
      var trunkList = new List<ITrunk> { trunk.Object };
      repo.Setup(r => r.GetList<ITrunk>()).Returns(trunkList);

      //make DDI
      var ddiToTest = new DDI(new Mock<IFuDDI>().Object, repo.Object);

      //Act
      //make mock trunk return that DDI on its DDI getter
      trunk.Setup(t => t.DDIs).Returns(new List<IDDI> { ddiToTest });
      trunk.SetupProperty(t => t.DefaultDestination, randomNumber.ToString(CultureInfo.InvariantCulture));
      //Assert
      //check that ddi.UsedOn is default
      ddiToTest.UsedOn.Should().Be(DDIUsedOn.Default);
      ddiToTest.Used.Should().Be(string.Format("Default: {0}", trunk.Object.DefaultDestination));
    }

  }
}
