using System;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestRoutingRuleModel
  {
    [TestMethod]
    public void TestRuleHasDialplan()
    {
      var rand = new Random().Next();

      var comRule = new Mock<IComRoutingRule>();
      comRule.Setup(r => r.DialplanId).Returns(rand);
     
      var dialplan = new Mock<IDialplan>();
      dialplan.Setup(d => d.Id).Returns(rand);

      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetFromId<IDialplan>(rand)).Returns(dialplan.Object);

      var rule = new RoutingRule(comRule.Object,  repo.Object);
      var usedDialplan = rule.Dialplan;

      //Assert 
      repo.Verify(r => r.GetFromId<IDialplan>(rand), Times.Exactly(1));
    }

  }
}
