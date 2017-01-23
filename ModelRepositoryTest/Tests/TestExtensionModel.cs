using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataAccess;
using DataAccess.TableInterfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using ModelRepositoryTest.TestObjects;
using Moq;

namespace ModelRepositoryTest.Tests
{
  [TestClass]
  public class TestExtensionModel
  {
    private readonly Mock<IAstSipReg> _mockAst;
    private readonly Mock<IComExtension> _mockExtension;
    private readonly IRepository _stubRepository;
    private readonly TestDataObject _testClass;

    public TestExtensionModel()
    {
      _mockExtension = new Mock<IComExtension>();
      _mockExtension.Setup(a => a.Id).Returns(19);
      _mockExtension.Setup(a => a.Name).Returns("1200");
      _mockExtension.Setup(a => a.LastName).Returns("Hammond");
   

      _mockAst = new Mock<IAstSipReg>();
      _mockAst.Setup(a => a.Id).Returns(21);
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
     

      var emptyRepo = new BaseRepository(mockDatabaseAccess.Object);
      _stubRepository = new Repository(emptyRepo);
    }


    [TestMethod]
    public void TestGetList()
    {
      //Arrange
      IEnumerable<IModel> listOfResults = ReflectTestMethodOnObjectList(_stubRepository, "GetList", _testClass.Type,
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
      IModel results = ReflectTestMethodOnObject(_stubRepository, "Add", type, null);

      //Assert none should be null
      results.Should().NotBeNull();
    }

    [TestMethod]
    public void TestGetFromId()
    {
      //Arrange
      Type type = typeof(IExtension);

      //Act
      var results = (IExtension)ReflectTestMethodOnObject(_stubRepository, "GetFromId", type, new object[] { 19 });

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
      var results = (IExtension)ReflectTestMethodOnObject(_stubRepository, "GetFromName", type, new object[] { "1200" });

      //Assert none should be null
      results.LastName.Should().BeEquivalentTo(_mockExtension.Object.LastName);
      results.IpAddress.Should().BeEquivalentTo(_mockAst.Object.IpAddress);
     
    }


    [TestMethod]
    public void TestBlankExtensionHasNoDDI()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepository>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object,repo.Object);
      newExtension.Update();

      //Assert that there is no DDI associated with this ext
      repo.Verify(r => r.GetFromName<IDDI>(""), Times.Never());
    }

    [TestMethod]
    public void TestBlankExtensionHasNoCLI()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepository>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      newExtension.Update();

      //Assert that there is no DDI associated with this ext
      repo.Verify(r => r.GetFromName<ICLI>(""), Times.Never());
    }

    [TestMethod]
    public void TestExtensionWithNoVoicemail()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepository>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      newExtension.Update();

      //Assert that there is no DDI associated with this ext
      repo.Verify(r => r.GetFromId<IVoiceMail>(0), Times.Never());
    }

    [TestMethod]
    public void TestExtensionWithVoicemail()
    {
      var comExtension = new Mock<IComExtension>();
     
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepository>();
      
      var mockVoice = new Mock<IVoiceMail>();
      mockVoice.Setup(v => v.Id).Returns(1);
      comExtension.Setup(c => c.VoiceMailId).Returns(mockVoice.Object.Id);

      repo.Setup(r => r.GetFromId<IVoiceMail>(comExtension.Object.VoiceMailId)).Returns(mockVoice.Object);

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      var voice = newExtension.VoiceMail;

      //Assert that there is no DDI associated with this ext
      repo.Verify(r => r.GetFromId<IVoiceMail>(mockVoice.Object.Id), Times.Exactly(1));
    }

    [TestMethod]
    public void TestExtensionWithCLISetsCliid()
    {
      var comExtension = new Mock<IComExtension>();
      var mockCli = new Mock<ICLI>();
    

      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepository>();
      repo.Setup(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber)).Returns(mockCli.Object);

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      newExtension.CLI = mockCli.Object;

      //Assert CLI is found once
      repo.Verify(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber), Times.Exactly(1));
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