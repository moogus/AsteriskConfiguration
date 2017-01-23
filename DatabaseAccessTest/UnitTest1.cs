using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;
using DatabaseAccess.DatabaseTables;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DatabaseAccessTest
{
    [TestClass]
    public class NewRepositoryTest
    {
        [TestMethod]
        public void TestGetFromIdUnknownType()
        {
            var sessionWrapper = new MockSessionWrapper();

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);

            repository.GetFromId<IModel>(1).Should().Be(null);
        }

        [TestMethod]
        public void TestGetFromIdKnownType()
        {
            var sessionWrapper = new MockSessionWrapper()
                .WithFakeDb()
                .AndAnEntryFor(1, "fred");

            Func<IFakeDatabaseTable, FakeModel> factory = t => new FakeModel { Name = t.Name };

            Func<IFakeDatabaseTable, string, bool> name = (t, n) => t.Name == n;

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);


            repository.AddTransformation(factory, name, null);

            repository.GetFromId<FakeModel>(1).Name.Should().Be("fred");
        }

        [TestMethod]
        public void TestGetFromNameUnKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper().WithFakeDb();

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);

            repository.GetFromName<IModel>("fred").Should().Be(null);
        }

        [TestMethod]
        public void TestGetFromNameKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper()
                .WithFakeDb()
                .AndAnEntryFor(1, "fred");

            var mockModel = new Mock<IModel>();
            Func<IDatabaseTable, IModel> factory =
                t => t == sessionWrapper.Db.First() ? mockModel.Object : null;
            Func<IFakeDatabaseTable, string, bool> name = (t, n) => t.Name == n;

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);
            repository.AddTransformation(factory, name, null);

            repository.GetFromName<IModel>("fred").Should().Be(mockModel.Object);
        }

        [TestMethod]
        public void TestListUnKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper()
                .WithFakeDb();

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);

            repository.GetList<IModel>().Should().BeNull();
        }

        [TestMethod]
        public void TestListKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper()
                .WithFakeDb()
                .AndAnEntryFor(1, "fred1")
                .AndAnEntryFor(2, "fred2")
                .AndAnEntryFor(3, "fred3");

            Func<IFakeDatabaseTable, FakeModel> factory = t =>
                {
                    var a = new FakeModel();
                    IFakeDatabaseTable b = t;
                    a.Name = b.Name;
                    return a;
                };
            Func<IFakeDatabaseTable, string, bool> name = (t, n) => t.Name == n;

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);
            repository.AddTransformation(factory, name, null);

            repository.GetList<FakeModel>().Count().Should().Be(3);

            repository.GetList<FakeModel>().First().Name.Should().Be("fred1");
        }

        [TestMethod]
        public void TestAddUnKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper().WithFakeDb();

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);

            repository.Add<FakeModel>().Should().BeNull();
        }

        [TestMethod]
        public void TestAddKnownType()
        {
            MockSessionWrapper sessionWrapper = new MockSessionWrapper().WithFakeDb();


            Func<IDatabaseTable, FakeModel> factory = t => null;
            Func<FakeModel> newFactory = () =>
                {
                    var m = new FakeModel { Name = "fred" };
                    return m;
                };
            Func<IFakeDatabaseTable, string, bool> name = (t, n) => t.Name == n;

            IConfigurableRepository repository = new GenericRepository(sessionWrapper.Object);
            repository.AddTransformation(factory, name, newFactory);

            repository.Add<FakeModel>().Name.Should().Be("fred");
        }

        public class FakeModel : IModel
        {
            public string Name { get; set; }
            public object Under { get; set; }
            public ISessionWrapper Session { get; set; }

            public void ExtraUpdate()
            {
                throw new NotImplementedException();
            }

            public void ExtraDelete()
            {
                throw new NotImplementedException();
            }
        }

        internal interface IFakeDatabaseTable : IDatabaseTable
        {
            string Name { get; }
        }

        public class MockSessionWrapper
        {
            private readonly Mock<ISessionWrapper> _mockSessionWrapper;
            private List<IFakeDatabaseTable> _db;

            public MockSessionWrapper()
            {
                _mockSessionWrapper = new Mock<ISessionWrapper>();
            }

            public ISessionWrapper Object
            {
                get { return _mockSessionWrapper.Object; }
            }

            internal List<IFakeDatabaseTable> Db
            {
                get { return _db; }
            }

            public MockSessionWrapper WithFakeDb()
            {
                _db = new List<IFakeDatabaseTable>();
                _mockSessionWrapper.Setup(s => s.Query<IFakeDatabaseTable>()).Returns(_db.AsQueryable());
                return this;
            }

            public MockSessionWrapper AndAnEntryFor(int id, string name)
            {
                var mockDbTable = new Thingy { Id = id, Name = name };
                _db.Add(mockDbTable);
                return this;
            }

            private class Thingy : IFakeDatabaseTable
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}