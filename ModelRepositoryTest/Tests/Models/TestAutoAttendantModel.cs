using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using ModelRepository;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestAutoAttendantModel
  {

    [TestMethod]
    public void TestAutoAttendantHasRule()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();

      var mockRule = new Mock<IAutoAttendantRules>();
      mockRule.SetupProperty(r => r.AaName, randomNumber.ToString(CultureInfo.InvariantCulture));
      
      var listOfRules = new List<IAutoAttendantRules> {mockRule.Object};

      repo.Setup(r => r.GetList<IAutoAttendantRules>()).Returns(listOfRules);

      var fuAa =new Mock<IFuAutoAttendant>();
      fuAa.Setup(f => f.Name).Returns( randomNumber.ToString(CultureInfo.InvariantCulture));

      //Act
      var autoAttenToTest = new AutoAttendant(fuAa.Object, repo.Object);

      //Assert
      autoAttenToTest.Rules.Count().Should().Be(1);
    }

  }
}
