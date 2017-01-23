using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.ModelInterfaces;
using ModelRepositoryTest.TestObjects;
using Moq;
using StructureMap;

namespace ModelRepositoryTest.Tests.Generic
{
  [TestClass]
  public class TestGenericModels
  {
    private readonly Mock<IAstSipReg> _mockAst;
    private readonly Mock<IComExtension> _mockExtension;
    private readonly IRepositoryWithDelete _stubModelRepository;
    private readonly TestDataObject _testClass;

    public TestGenericModels()
    {
      _mockExtension = new Mock<IComExtension>();
      _mockExtension.Setup(a => a.Id).Returns(19);
      _mockExtension.Setup(a => a.Number).Returns("1200");
      _mockExtension.Setup(a => a.Name).Returns("1200");
      _mockExtension.Setup(a => a.LastName).Returns("Hammond");


      _mockAst = new Mock<IAstSipReg>();
      _mockAst.Setup(a => a.Id).Returns(21);
        _mockAst.Setup(a => a.Number).Returns("1200");
        _mockAst.Setup(a => a.Name).Returns("1200");
        _mockAst.Setup(a => a.IpAddress).Returns("19.2.168.1.123");

      _testClass = new TestDataObject(typeof(IExtension), _mockExtension.Object.Id, _mockExtension.Object.Name);

      var extList = new List<IComExtension> { _mockExtension.Object };
      var astList = new List<IAstSipReg> { _mockAst.Object };

      var mockDatabaseAccess = new Mock<IDataRepository>();
      mockDatabaseAccess.Setup(d => d.CreateNewObject<IComExtension>()).Returns(_mockExtension.Object);
      mockDatabaseAccess.Setup(d => d.CreateNewObject<IAstSipReg>()).Returns(_mockAst.Object);
      mockDatabaseAccess.Setup(d => d.GetQueryable<IComExtension>()).Returns(extList.AsQueryable);
      mockDatabaseAccess.Setup(d => d.GetQueryable<IAstSipReg>()).Returns(astList.AsQueryable);


      var emptyRepo = new EmptyModelRepository(mockDatabaseAccess.Object);
      _stubModelRepository = new ModelRepository.Internal.ModelRepositoryWithMapping(emptyRepo, new Mock<IContainer>().Object);
    }

    [TestMethod]
    public void TestGetList()
    {
      //Arrange
      IEnumerable<IModel> listOfResults = ReflectTestMethodOnObjectList(_stubModelRepository, "GetList", _testClass.Type,
                                                                        null);

      //Assert none should be null
      listOfResults.All(r => r != null).Should().Be(true);
    }

    [TestMethod]
    public void TestAdd()
    {
      //Arrange
      Type type = typeof(IExtension);

      //Act
      IModel results = ReflectTestMethodOnObject(_stubModelRepository, "Add", type, null);

      //Assert none should be null
      results.Should().NotBeNull();
    }

    [TestMethod]
    public void TestGetFromId()
    {
      //Arrange
      Type type = typeof(IExtension);

      //Act
      var results = (IExtension)ReflectTestMethodOnObject(_stubModelRepository, "GetFromId", type, new object[] { 19 });

      //Assert none should be null
      results.LastName.Should().BeEquivalentTo(_mockExtension.Object.LastName);
      results.IpAddress.Should().BeEquivalentTo(_mockAst.Object.IpAddress);

    }

    [TestMethod]
    public void TestGetFromName()
    {
      //Arrange
      Type type = typeof(IExtension);

      //Act
      var results = (IExtension)ReflectTestMethodOnObject(_stubModelRepository, "GetFromName", type, new object[] { "1200" });

      //Assert none should be null
      results.LastName.Should().BeEquivalentTo(_mockExtension.Object.LastName);
      results.IpAddress.Should().BeEquivalentTo(_mockAst.Object.IpAddress);

    }

    private static IModel ReflectTestMethodOnObject(IRepository stubSession, string methodName, Type type,
                                                  object[] parameters)
    {
      MethodInfo methodtype = stubSession.GetType().GetMethod(methodName);
      MethodInfo method = methodtype.MakeGenericMethod(type);
      return (IModel)method.Invoke(stubSession, parameters);
    }

    private static IEnumerable<IModel> ReflectTestMethodOnObjectList(IRepository stubSession, string methodName,
                                                                     Type type, object[] parameters)
    {
      MethodInfo methodtype = stubSession.GetType().GetMethod(methodName);
      MethodInfo method = methodtype.MakeGenericMethod(type);
      return (IEnumerable<IModel>)method.Invoke(stubSession, parameters);
    }



  }

}
