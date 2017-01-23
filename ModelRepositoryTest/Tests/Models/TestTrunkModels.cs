using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
  public class TestTrunkModels
  {
  
    [TestMethod]
    public void TestTrunkHasListOfDDIs()
    {
      //Arrange
      var randId = new Random().Next();
      var mockDdi = new Mock<IDDI>();
      mockDdi.Setup(d => d.Trunk.Id).Returns(randId);

      var comTrunk = new Mock<IComTrunk>();
      comTrunk.Setup(c => c.Id).Returns(randId);
      var listofDDIs = new List<IDDI> {mockDdi.Object};

      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetList<IDDI>()).Returns(listofDDIs);

      //Act
      var trunk = new Trunk(comTrunk.Object, repo.Object);
      var allDDIs = trunk.DDIs;
      
      //Assert
      allDDIs.Should().Equal(listofDDIs);

    }

    [TestMethod]
    public void TestTrunkHasListOfAccessCodes()
    {
      //Arrange
      var randId = new Random().Next();
      var mockDdi = new Mock<IAccessCode>();
      mockDdi.Setup(d => d.ParentTrunk.Id).Returns(randId);

      var comTrunk = new Mock<IComTrunk>();
      comTrunk.Setup(c => c.Id).Returns(randId);
      var listofAccess = new List<IAccessCode> { mockDdi.Object };

      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetList<IAccessCode>()).Returns(listofAccess);

      //Act
      var trunk = new Trunk(comTrunk.Object, repo.Object);
      var allAcess = trunk.AccessCodes;

      //Assert
      allAcess.Should().Equal(listofAccess);

    }

    [TestMethod]
    public void TestBriTrunkHasDahdiChannels()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var mockTrunk = new Mock<ITrunk>();
      mockTrunk.Setup(c => c.Id).Returns(randomNumber);


      var mockDChannel = new Mock<IDahdiChannel>();
      mockDChannel.Setup(m => m.ParentTrunk).Returns(mockTrunk.Object);

      var listDChannels = new List<IDahdiChannel> {mockDChannel.Object};

      repo.Setup(r => r.GetList<IDahdiChannel>()).Returns(listDChannels);
      repo.Setup(r => r.GetFromId<ITrunk>(randomNumber)).Returns(mockTrunk.Object);
      //Act
      var briTrunk = new BriTrunk(mockTrunk.Object, repo.Object);
      var allChannels = briTrunk.DahdiChannels;

      //Assert
      repo.Verify(r => r.GetList<IDahdiChannel>(), Times.Exactly(1));
      
    }

    [TestMethod]
    public void TestBriTrunkAddsDahdiChannels()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var randomAmount = new Random().Next(1, 8);
      var mockTrunk = new Mock<ITrunk>();
      mockTrunk.Setup(c => c.Id).Returns(randomNumber);

      var mockDChannel = new Mock<IDahdiChannel>();
      
      mockDChannel.Setup(m => m.ParentTrunk.Id).Returns(randomNumber);

      var listDChannels = new List<IDahdiChannel> ();

      for (int i = 0; i < randomAmount; i++)
      {
        listDChannels.Add(mockDChannel.Object);
      }

      repo.Setup(r => r.Add<IDahdiChannel>()).Returns(mockDChannel.Object);
      repo.Setup(r => r.GetFromId<ITrunk>(randomNumber)).Returns(mockTrunk.Object);
      repo.Setup(r => r.GetList<IDahdiChannel>()).Returns(listDChannels);
      //Act
      var briTrunk = new BriTrunk(mockTrunk.Object, repo.Object);
      briTrunk.DahdiChannels = listDChannels.Select(d => d.ChannelName);

      //Assert
      mockDChannel.Verify(d => d.Delete(), Times.Exactly(randomAmount));
      repo.Verify(r => r.Add<IDahdiChannel>(), Times.Exactly(randomAmount));
    }
  }
}
