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
using StructureMap;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestExtensionModel
  {
    [TestMethod]
    public void TestBlankExtensionHasNoDDI()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);

      //Assert 
      repo.Verify(r => r.GetFromName<IDDI>(""), Times.Never());
    }

    [TestMethod]
    public void TestBlankExtensionHasNoCLI()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);


      //Assert 
      repo.Verify(r => r.GetFromName<ICLI>(""), Times.Never());
    }

    [TestMethod]
    public void TestExtensionWithCLISetsCliid()
    {
      var comExtension = new Mock<IComExtension>();
      var mockCli = new Mock<ICLI>();


      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();
      repo.Setup(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber)).Returns(mockCli.Object);

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      newExtension.CLI = mockCli.Object;

      //Assert 
      repo.Verify(r => r.GetFromName<ICLI>(mockCli.Object.CLINumber), Times.Exactly(1));
    }

    [TestMethod]
    public void TestExtensionWithNoVoicemail()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);


      //Assert
      repo.Verify(r => r.GetFromId<IVoiceMail>(0), Times.Never());
    }

    [TestMethod]
    public void TestExtensionWithVoicemail()
    {
      var comExtension = new Mock<IComExtension>();

      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var mockVoice = new Mock<IVoiceMail>();
      mockVoice.Setup(v => v.Id).Returns(1);
      comExtension.Setup(c => c.VoiceMailId).Returns(mockVoice.Object.Id);

      repo.Setup(r => r.GetFromId<IVoiceMail>(comExtension.Object.VoiceMailId)).Returns(mockVoice.Object);

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      var voice = newExtension.VoiceMail;

      //Assert 
      repo.Verify(r => r.GetFromId<IVoiceMail>(mockVoice.Object.Id), Times.Exactly(1));
    }

    [TestMethod]
    public void TestExtensionWithNoPermissionClass()
    {
      var comExtension = new Mock<IComExtension>();
      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);


      //Assert 
      repo.Verify(r => r.GetFromId<IPermisionClass>(0), Times.Never());
    }

    [TestMethod]
    public void TestExtensionWithPermissionClass()
    {
      var comExtension = new Mock<IComExtension>();

      var astSipReg = new Mock<IAstSipReg>();
      var repo = new Mock<IRepositoryWithDelete>();

      var mockPermission = new Mock<IPermisionClass>();
      mockPermission.Setup(v => v.Id).Returns(1);
      comExtension.Setup(c => c.PermissionClassId).Returns(mockPermission.Object.Id);

      repo.Setup(r => r.GetFromId<IPermisionClass>(comExtension.Object.VoiceMailId)).Returns(mockPermission.Object);

      var newExtension = new Extension(comExtension.Object, astSipReg.Object, repo.Object);
      var permClass = newExtension.PermisionClass;

      //Assert that there is no DDI associated with this ext
      repo.Verify(r => r.GetFromId<IPermisionClass>(mockPermission.Object.Id), Times.Exactly(1));
    }

  }
}