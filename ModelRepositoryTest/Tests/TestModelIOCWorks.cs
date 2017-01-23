using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace ModelRepositoryTest.Tests
{
  [TestClass]
  public class TestModelIOCWorks
  {

    [TestMethod]
    public void TestGetIExtension()
    {
      //Arrange
     var ioc = new ModelIOC(new DataAccess.DataIOC().Container ).Container;
      var repo = ioc.GetInstance<IRepository>();

      //Act
      var e = repo.GetList<IExtension>();

      e.Count().Should().Be(1);

      //Assert
    }
  }
}
