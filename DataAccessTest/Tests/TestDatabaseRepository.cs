using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataAccess;
using DataAccess.Internal;
using DataAccess.Internal.NHibernate;
using DataAccess.TableInterfaces;
using DataAccessTest.TestObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace DataAccessTest.Tests
{
  [TestClass]
  public class TestDatabaseRepository
  {
    private readonly MockDataObjects _mockObjects;

    public TestDatabaseRepository()
    {
      _mockObjects = new MockDataObjects();
    }


    [TestMethod]
    public void TestCanGetStuffFromDB()
    {
      //Arrange
      var c = new Container(new  DataIOC());
      var repo = c.GetInstance<IDataRepository>();
      var x = repo.GetQueryable<IComExtension>().FirstOrDefault();

      //Act
      x.Should().NotBeNull();

      //Assert
    }

    [TestMethod]
    public void TestGetQueryable()
    {
      //Arrange
      //Act
      IEnumerable<IEnumerable<IDatabaseTable>> listOfResults =
       _mockObjects.ListOfClassesToTest.Select(classToTest => ReflectTestMethodOnObjectList(
          _mockObjects.StubRepository,
          "GetQueryable",
          classToTest.Type,
          null
                                                     ));

      //Assert none should be null
      listOfResults.All(r => r != null).Should().Be(true);
    }

    [TestMethod]
    public void TestGetQueryableInvalidObject()
    {
      //Arrange
      //Act
      IEnumerable<IEnumerable<IDatabaseTable>> listOfResults =
        _mockObjects.ListOfClassesToTest.Select(classToTest => ReflectTestMethodOnObjectList(
          _mockObjects.StubRepository,
          "GetQueryable",
          typeof(IDatabaseTable),
          null
                                                     ));

      //Assert all should be null
        listOfResults.All(r => r == null).Should().Be(true);
    }

    [TestMethod]
    public void TestCreateNewObject()
    {
      //Arrange
      //Act
      IEnumerable<IDatabaseTable> results =
        _mockObjects.ListOfValidTypes.Select(x => ReflectTestMethodOnObject(_mockObjects.StubRepository, "CreateNewObject", x.Item1, null));

      //Assert none should be null
      results.All(r => r != null).Should().Be(true);
    }

    [TestMethod]
    public void TestCreateNewObjectInvalidObject()
    {
      //Arrange
      //Act
      IEnumerable<IDatabaseTable> results =
        _mockObjects.ListOfInValidTypes.Select(x => ReflectTestMethodOnObject(_mockObjects.StubRepository, "CreateNewObject", x, null));

      //Assert all should be null
      results.All(r => r == null).Should().Be(true);
    }


    private static IDatabaseTable ReflectTestMethodOnObject(IDataRepository stubSession, string methodName,
                                                            Type type, object[] parameters)
    {
      MethodInfo methodtype = stubSession.GetType().GetMethod(methodName);
      MethodInfo method = methodtype.MakeGenericMethod(type);
      return (IDatabaseTable)method.Invoke(stubSession, parameters);
    }

    private static IEnumerable<IDatabaseTable> ReflectTestMethodOnObjectList(IDataRepository stubSession,
                                                                             string methodName, Type type,
                                                                             object[] parameters)
    {
      MethodInfo methodtype = stubSession.GetType().GetMethod(methodName);
      MethodInfo method = methodtype.MakeGenericMethod(type);
      return (IEnumerable<IDatabaseTable>)method.Invoke(stubSession, parameters);
    }
  }
}
