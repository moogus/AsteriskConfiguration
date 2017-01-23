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
  public class TestQueueMemberModel
  {
    //requires the following packages    

    [TestMethod]
    public void TestGetParentQueueNeverCalledOn0()
    {
      var comQm = new Mock<IComQueueMember>();

      var repo = new Mock<IRepositoryWithDelete>();

      var queuMem = new QueueMember(comQm.Object, repo.Object);
    

      //Assert 
      repo.Verify(r => r.GetFromId<IQueue>(0), Times.Never());
    }

    [TestMethod]
    public void TestQueueMemberType()
    {
      var mockComQm = new Mock<IComQueueMember>();

      mockComQm.SetupProperty(q => q.Type, 0);
      var queueMemeber = new QueueMember(mockComQm.Object, new Mock<IRepositoryWithDelete>().Object);
      queueMemeber.Type.Should().Be(QueueMemberType.Unknown);

      mockComQm.SetupProperty(q => q.Type, 1);
      queueMemeber = new QueueMember(mockComQm.Object, new Mock<IRepositoryWithDelete>().Object);
      queueMemeber.Type.Should().Be(QueueMemberType.Extension);

      mockComQm.SetupProperty(q => q.Type, 2);
      queueMemeber = new QueueMember(mockComQm.Object, new Mock<IRepositoryWithDelete>().Object);
      queueMemeber.Type.Should().Be(QueueMemberType.Queue);

    }

    [TestMethod]
    public void TestQueueMemberHasExtension()
    {
      var rand = new Random();
      var randId = rand.Next();
      var mockExtension = new Mock<IExtension>();
      mockExtension.Setup(e => e.Id).Returns( randId);

      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetFromId<IExtension>(randId)).Returns(mockExtension.Object);

      var mockComQ = new Mock<IComQueueMember>();
      mockComQ.Setup(q => q.ExtensionId).Returns( randId);

      var queueMember = new QueueMember(mockComQ.Object, repo.Object);
      queueMember.Extension.Should().Be(mockExtension.Object);
      queueMember.Queue.Should().BeNull();
    }

    [TestMethod]
    public void TestQueueMemberHasQueue()
    {
      var rand = new Random();
      var randId = rand.Next();
      var mockQueue = new Mock<IQueue>();
      mockQueue.Setup(e => e.Id).Returns(randId);

      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetFromId<IQueue>(randId)).Returns(mockQueue.Object);

      var mockComQ = new Mock<IComQueueMember>();
      mockComQ.Setup(q => q.QueueId).Returns(randId);

      var queueMember = new QueueMember(mockComQ.Object, repo.Object);
      queueMember.Queue.Should().Be(mockQueue.Object);
      queueMember.Extension.Should().BeNull();
    }

  }
}
