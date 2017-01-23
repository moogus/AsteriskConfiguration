using System;
using System.Collections.Generic;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestPermissionClassModel
  {

    [TestMethod]
    public void TestPermissionClassGetsList()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
      var mockPcM = new Mock<IPermissionClassMember>();
      mockPcM.Setup(p => p.ParentPermissionClass.Id).Returns( randomNumber);

      var mockFuPermClass = new Mock<IFuPermissionClass>();
      mockFuPermClass.Setup(c => c.Id).Returns( randomNumber);

      var listPcm = new List<IPermissionClassMember> {mockPcM.Object};

      
      repo.Setup(r => r.GetList<IPermissionClassMember>()).Returns(listPcm);

      //Act
      var permClass = new PermisionClass(mockFuPermClass.Object, repo.Object);
      var memebers = permClass.PermissionClassMemebers;
      //Assert

      repo.Verify(r=>r.GetList<IPermissionClassMember>(), Times.Exactly(1));
    }
  }
}
