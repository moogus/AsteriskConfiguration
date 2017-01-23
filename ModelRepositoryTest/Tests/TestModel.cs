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
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using ModelRepositoryTest.TestObjects;
using Moq;

namespace ModelRepositoryTest.Tests
{
  [TestClass]
  public class TestModel
  {
    private readonly Repository _stubRepository;
    private readonly Mock<IComExtension> _mockComExt;
    private BaseRepository emptyRepo;
    public TestModel()
    {
      _mockComExt = new Mock<IComExtension>();
      _mockComExt.SetupAllProperties();

      var mockDatabaseAccess = new Mock<IDataRepository>();

       emptyRepo = new BaseRepository(mockDatabaseAccess.Object);
      _stubRepository = new Repository(emptyRepo);
    }

    

    [TestMethod]
    public void TestRepoGetFromId()
    {
      //Arrange
     var ext= _stubRepository.GetFromId<IExtension>(1);

      //Act


      //Assert
      emptyRepo.Verify()
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
