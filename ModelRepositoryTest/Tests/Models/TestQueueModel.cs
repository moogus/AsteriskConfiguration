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
  public class TestQueueModel
  {

    [TestMethod]
    public void TestBlankQueueHasNoDDI()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();

      var queue = new Queue(comQueue.Object,  repo.Object);
   

      //Assert 
      repo.Verify(r => r.GetFromName<IDDI>(""), Times.Never());
    }

    [TestMethod]
    public void TestBlankQueueHasNoCLI()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();

      var queue = new Queue(comQueue.Object, repo.Object);
    

      //Assert 
      repo.Verify(r => r.GetFromName<ICLI>(""), Times.Never());
    }

    [TestMethod]
    public void TestQueueWithCLISetsCliid()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();
      var mockCli = new Mock<ICLI>();
    
      repo.Setup(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber)).Returns(mockCli.Object);

      var queue = new Queue(comQueue.Object, repo.Object);

      queue.CLI = mockCli.Object;

      //Assert 
      repo.Verify(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber), Times.Exactly(1));
    }

    [TestMethod]
    public void TestQueueWithNoVoicemail()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();

      var queue = new Queue(comQueue.Object, repo.Object);
  

      //Assert
      repo.Verify(r => r.GetFromId<IVoiceMail>(0), Times.Never());
    }

    [TestMethod]
    public void TestQueueWithVoicemail()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();

      var mockVoice = new Mock<IVoiceMail>();
      mockVoice.Setup(v => v.Id).Returns(1);
      comQueue.Setup(c => c.VoiceMailId).Returns(mockVoice.Object.Id);

      repo.Setup(r => r.GetFromId<IVoiceMail>(comQueue.Object.VoiceMailId)).Returns(mockVoice.Object);

      var queue = new Queue(comQueue.Object, repo.Object);
      var voice = queue.VoiceMail;

      //Assert 
      repo.Verify(r => r.GetFromId<IVoiceMail>(mockVoice.Object.Id), Times.Exactly(1));
    }

    [TestMethod]
    public void TestQueueWithMusicOnHold()
    {
      var comQueue = new Mock<IComQueue>();
      var repo = new Mock<IRepositoryWithDelete>();

      var mockMusic = new Mock<IMusicOnHold>();
      mockMusic.Setup(v => v.Id).Returns(1);
      comQueue.Setup(c => c.ComMusicOnHoldId).Returns(mockMusic.Object.Id);

      repo.Setup(r => r.GetFromId<IMusicOnHold>(comQueue.Object.ComMusicOnHoldId)).Returns(mockMusic.Object);

      var queue = new Queue(comQueue.Object, repo.Object);
      var music = queue.MusicOnHold;

      //Assert 
      repo.Verify(r => r.GetFromId<IMusicOnHold>(mockMusic.Object.Id), Times.Exactly(1));
    }

    [TestMethod]
    public void TestQueueGetsList()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var mockQueueMem = new Mock<IQueueMember>();
      mockQueueMem.Setup(c => c.ParentQueue.Id).Returns(randomNumber);

      var mockComQueue = new Mock<IComQueue>();
      mockComQueue.Setup(c => c.Id).Returns(randomNumber);

      var listQm = new List<IQueueMember> { mockQueueMem.Object };
      repo.Setup(r => r.GetList<IQueueMember>()).Returns(listQm);

      //Act
      var queue = new Queue(mockComQueue.Object, repo.Object);
      var listOfMemebers = queue.QueueMembers;
      //Assert

      repo.Verify(r => r.GetList<IQueueMember>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestQueueAddExtensionToQueueMembers()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var mockComQueue = new Mock<IComQueue>();
      mockComQueue.Setup(c => c.Id).Returns(randomNumber);


      var mockNewMemeber =new Mock<IQueueMember>();
      var mockExtension = new Mock<IExtension>();
      mockNewMemeber.Setup(m => m.Extension).Returns(mockExtension.Object);
      repo.Setup(r => r.Add<IQueueMember>()).Returns(mockNewMemeber.Object);
      //Act
      var queue = new Queue(mockComQueue.Object, repo.Object);
      queue.AddExtensionAsQueueMember(mockExtension.Object, randomNumber);

      //Assert
      repo.Verify(r=>r.Add<IQueueMember>(), Times.Exactly(1));
      queue.QueueMembers.Should().Contain(mockNewMemeber.Object);
    }

    [TestMethod]
    public void TestQueueAddQueueToQueueMembers()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var mockComQueue = new Mock<IComQueue>();
      mockComQueue.Setup(c => c.Id).Returns(randomNumber);


      var mockNewMemeber = new Mock<IQueueMember>();
      var mockQueue = new Mock<IQueue>();
      mockNewMemeber.Setup(m => m.Queue).Returns(mockQueue.Object);
      repo.Setup(r => r.Add<IQueueMember>()).Returns(mockNewMemeber.Object);
      //Act
      var queue = new Queue(mockComQueue.Object, repo.Object);
      queue.AddQueueAsQueueMember(mockQueue.Object, randomNumber);

      //Assert
      repo.Verify(r => r.Add<IQueueMember>(), Times.Exactly(1));
      queue.QueueMembers.Should().Contain(mockNewMemeber.Object);
    }
  }
}
