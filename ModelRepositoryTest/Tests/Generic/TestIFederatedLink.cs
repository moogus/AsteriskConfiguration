using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using ModelRepository;
using ModelRepository.Internal.ModelHelpers;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Generic
{
    [TestClass]
    public class TestIFederatedLink
    {
        private readonly Mock<ITrunk> _trunk;
        private readonly Mock<IExtension> _ext;
        private readonly Mock<IDialplan> _dialPlan;
        private readonly Mock<IRoutingRule> _rule;
        private readonly string _name;
        private readonly string _password;
        private readonly string _accessCode;
        private readonly int _randTId;
        private readonly int _randTId2;
        private readonly int _randEId;
        private readonly int _randRId;
        private List<ITrunk> _listOfTrunk;
        private List<IExtension> _listOfExtn;
        private List<IRoutingRule> _listOfRule;
        private readonly Mock<IRepositoryWithDelete> _repo;
        private readonly Mock<IFuSamsungFederation> _mockFed;
        private Mock<IFederation> _mockBaseFed;

        public TestIFederatedLink()
        {
            var rand = new Random();
            _repo = new Mock<IRepositoryWithDelete>();
            _randTId = rand.Next();
            _randTId2 = rand.Next();
            _randEId = rand.Next();
            _randRId = rand.Next();
            _name = rand.Next().ToString(CultureInfo.InvariantCulture);
            _password = rand.Next().ToString(CultureInfo.InvariantCulture);
            _accessCode = string.Format("{0}:{1}", rand.Next().ToString(CultureInfo.InvariantCulture), rand.Next().ToString(CultureInfo.InvariantCulture));

            _trunk = new Mock<ITrunk>();
            _trunk.Setup(t => t.Id).Returns(_randTId);

            _repo.Setup(r => r.GetFromId<ITrunk>(_randTId)).Returns(_trunk.Object);
            _listOfTrunk = new List<ITrunk> { _trunk.Object };

            _ext = new Mock<IExtension>();
            _ext.Setup(e => e.Id).Returns(_randEId);

            _repo.Setup(r => r.GetFromId<IExtension>(_randEId)).Returns(_ext.Object);
            _listOfExtn = new List<IExtension> { _ext.Object };

            _dialPlan = new Mock<IDialplan>();
            _dialPlan.SetupProperty(d => d.Name, "fallThrough");
            _repo.Setup(r => r.GetFromName<IDialplan>("fallThrough")).Returns(_dialPlan.Object);

            _rule = new Mock<IRoutingRule>();
            _rule.Setup(e => e.Id).Returns(_randRId);

            _repo.Setup(r => r.GetFromId<IRoutingRule>(_randRId)).Returns(_rule.Object);
            _listOfRule = new List<IRoutingRule> { _rule.Object };

            _mockFed = new Mock<IFuSamsungFederation>();
            _mockFed.SetupProperty(f => f.ComTrunkId, _randTId);

         
        }

        [TestMethod]
        public void TestSamsungLinkSetsValuesNoNewTrunk()
        {
            //Arrange
      
            _mockFed.Setup(f => f.ComTrunkId).Returns(_randTId);
            _mockFed.Setup(f => f.ComExtensionId).Returns(_randEId);
            _mockFed.Setup(f => f.ComRoutingRuleId).Returns(_randRId);

            _repo.Setup(r => r.Add<IRoutingRule>()).Returns(new Mock<IRoutingRule>().Object);
            _repo.Setup(r => r.Add<IExtension>()).Returns(new Mock<IExtension>().Object);

            var samsungFedLink = new SamsungFederatedLink(_mockFed.Object, _repo.Object);

            //Act
            var result = samsungFedLink.SetFederationValues(_name, _accessCode, _password);

            //Assert
            result.Should().BeTrue();
            _repo.Verify(r => r.GetFromName<IDialplan>("fallThrough"), Times.Exactly(1));
        }

        [TestMethod]
        public void TestSamsungLinkSetsValuesNewTrunk()
        {
            //Arrange
           
            _mockFed.Setup(f => f.ComTrunkId).Returns(_randTId);
            _mockFed.Setup(f => f.ComExtensionId).Returns(_randEId);
            _mockFed.Setup(f => f.ComRoutingRuleId).Returns(_randRId);

            _repo.Setup(r => r.Add<IRoutingRule>()).Returns(new Mock<IRoutingRule>().Object);
            _repo.Setup(r => r.Add<IExtension>()).Returns(new Mock<IExtension>().Object);

            var samsungFedLink = new SamsungFederatedLink(_mockFed.Object,  _repo.Object);

            var newTrunk = new Mock<ITrunk>();
            newTrunk.Setup(t => t.Id).Returns(_randTId2);

            //Act
            var result = samsungFedLink.SetFederationValues(_name, _accessCode, _password);
            //Assert
           
            result.Should().BeTrue();
            _repo.Verify(r => r.GetFromName<IDialplan>("fallThrough"), Times.Exactly(1));
      
        }

        [TestMethod]
        public void TestFourComLinkSetsValuesNoNewTrunk()
        {
            //Arrange
            var fourComFedLink = new FourComFederatedLink(new Mock<IFu4ComFederation>().Object,_repo.Object);

            //Act
            var result = fourComFedLink.SetFederationValues(_name, _accessCode, _password);

            //Assert
            result.Should().BeTrue();
         
        }

        [TestMethod]
        public void TestFourComLinkSetsValuesNewTrunk()
        {
            //Arrange
            var mockBaseFed = new Mock<IFu4ComFederation>();
            mockBaseFed.Setup(m => m.Id).Returns(_randTId2);

            var fourComFedLink = new FourComFederatedLink(mockBaseFed.Object, _repo.Object); 

            var newTrunk = new Mock<ITrunk>();
            newTrunk.Setup(t => t.Id).Returns(_randTId2);

            //Act
            var result = fourComFedLink.SetFederationValues(_name, _accessCode, _password);

            //Assert
            result.Should().BeTrue();

        }
    }
}
