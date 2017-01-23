using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DatabaseAccess;
using Moq;
using NUnit.Framework;

namespace PhoneAppTests.TestHelpers.Mocks
{
  public class MockDatabaseAccess
  {
    public Mock<IRepository> MockRepository { get; private set; }

    public MockDatabaseAccess()
    {
      MockRepository = new Mock<IRepository>();
    }

    public void SetUpRepositoryExtension(IExtension value)
    {
      MockRepository.Setup(g => g.GetList<IExtension>()).Returns(new List<IExtension> { value });
    }

    public void SetUpRepositoryQueue(IQueue value)
    {
      MockRepository.Setup(g => g.GetList<IQueue>()).Returns(new List<IQueue> { value });
    }

    public void SetUpRepositoryVoiceMail(IVoiceMail value)
    {
      MockRepository.Setup(g => g.GetList<IVoiceMail>()).Returns(new List<IVoiceMail> { value });
    }

    public void SetUpRepositoryDialplan(IDialplan value)
    {
      MockRepository.Setup(g => g.GetList<IDialplan>()).Returns(new List<IDialplan> { value });
    }

    public void SetUpRepositoryDialplanList(List<IDialplan> value)
    {
      MockRepository.Setup(g => g.GetList<IDialplan>()).Returns(value);
    }

    public void SetUpRepositoryDialplanListGetByName(List<IDialplan> value)
    {
      foreach (var d in value)
      {
        var dialplan = d;
        MockRepository.Setup(g => g.GetFromName<IDialplan>(dialplan.Name)).Returns(d);
      }
    }

    public void SetUpRepositoryForwardingRoutingRules(IRoutingRule value)
    {
      MockRepository.Setup(g => g.GetList<IRoutingRule>()).Returns(new List<IRoutingRule> { value });
    }

    public void SetUpRepositoryForwardingRoutingRulesList(List<IRoutingRule> value)
    {
      MockRepository.Setup(g => g.GetList<IRoutingRule>()).Returns(value);
    }

    public void SetUpRepositoryAddRule(string extension, IDialplan dialplan)
    {
      MockRepository.Setup(a => a.Add<IRoutingRule>().Dialplan).Returns(dialplan);
    }

    public Mock<IExtension> GetMockExtension(string number)
    {
      var mockExtension = new Mock<IExtension>();
      mockExtension.Setup(e => e.Number).Returns(number);
      return mockExtension;
    }

    public Mock<IQueue> GetMockQueue(string number)
    {
      var mockQueue = new Mock<IQueue>();
      mockQueue.Setup(e => e.Number).Returns(number);
      return mockQueue;
    }

    public Mock<IVoiceMail> GetMockVoiceMail(string number)
    {
      var mockVoiceMail = new Mock<IVoiceMail>();
      mockVoiceMail.Setup(e => e.Number).Returns(number);
      return mockVoiceMail;
    }

    public Mock<IRoutingRule> GetMockForwardRoutingRule(string number, string dialplanName, string destinationType, string destinationNumber)
    {
      var mockRoutingRule = new Mock<IRoutingRule>();
      mockRoutingRule.Setup(r => r.Dialplan.Name).Returns(dialplanName);
      mockRoutingRule.Setup(r => r.Number).Returns(number);
      mockRoutingRule.Setup(r => r.DestinationType)
                     .Returns((RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), destinationType));
      mockRoutingRule.Setup(r => r.DestinationNumber).Returns(destinationNumber);
      return mockRoutingRule;
    }

    public Mock<IDialplan> GetMockDialPlan(string dialplanName)
    {
      var mockDialplan = new Mock<IDialplan>();
      mockDialplan.Setup(d => d.Name).Returns(dialplanName);
      return mockDialplan;
    }
  }
}
