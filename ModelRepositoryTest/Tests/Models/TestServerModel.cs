using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestServerModel
  {
    //requires the following packages  

    [TestMethod]
    public void TestServerHasAdminExtension()
    {
      var comServer = new Mock<IComServer>();
      var mockExtension = new Mock<IExtension>();


      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetFromName<IExtension>(mockExtension.Object.Number)).Returns(mockExtension.Object);

      var server = new Server(comServer.Object, repo.Object);
      var adminExt = server.AdminExtension;

      //Assert 
      repo.Verify(r => r.GetFromName<IExtension>(mockExtension.Object.Number), Times.Exactly(1));
    }
  }
}
