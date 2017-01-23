using System.Linq;
using DataAccess.Internal.NHibernate.DataTables.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class HowDoesNHibernateWork
    {
        [TestMethod]
        public void UpdateObjectWithNoCommitDoesNotSave()
        {
            var session = getSession();
            using (var transaction = session.BeginTransaction())
            {
                //to use these tests you need to look up vales in the database to refernce

//                var trunk = session.Get<ComTrunk>(4);
//                Assert.AreEqual("test", trunk.ComTrunkName);
//                trunk.ComTrunkName = "test2";
            }
//            var session2 = getSession();
//            var trunk2 = session2.Get<ComTrunk>(4);
//            Assert.AreEqual("test", trunk2.ComTrunkName);
        }

        [TestMethod]
        public void UpdateObjectWithCommitSaves()
        {
            var session = getSession();
            using (var transaction = session.BeginTransaction())
            {
                //to use these tests you need to look up vales in the database to refernce

//                var trunk = session.Get<ComTrunk>(4);
//                Assert.AreEqual("test", trunk.ComTrunkName);
//                trunk.ComTrunkName = "test2";
//                transaction.Commit();
            }

            var session2 = getSession();
            using (var transaction = session2.BeginTransaction())
            {
//                var trunk = session2.Get<ComTrunk>(4);
//                Assert.AreEqual("test2", trunk.ComTrunkName);
//                trunk.ComTrunkName = "test";
//                transaction.Commit();
            }
        }

        [TestMethod]
        public void SaveNewObjectWithNoCommitDoesNotSave()
        {
            var session = getSession();
            using (var transaction = session.BeginTransaction())
            {
                var trunk = new ComTrunk();
                trunk.DefaultDestination = "";
                trunk.CLIPresentationType1 = "";
                trunk.CLIPresentationValue1 = "";
                trunk.CLIPresentationType2 = "";
                trunk.CLIPresentationValue2 = "";
                trunk.ComTrunkName = "IShouldNotBeSaved";
                session.Save(trunk);
            }

            var session2 = getSession();
            using (var transaction = session2.BeginTransaction())
            {
                Assert.IsFalse(session2.Query<ComTrunk>().Any(t => t.ComTrunkName == "IShouldNotBeSaved"));
            }
        }

        [TestMethod]
        public void SaveNewObjectWithCommitSaves()
        {
            var session = getSession();
            using (var transaction = session.BeginTransaction())
            {
                var trunk = new ComTrunk();
                trunk.DefaultDestination = "";
                trunk.CLIPresentationType1 = "";
                trunk.CLIPresentationValue1 = "";
                trunk.CLIPresentationType2 = "";
                trunk.CLIPresentationValue2 = "";
                trunk.ComTrunkName = "IShouldBeSaved";
                session.Save(trunk);
                transaction.Commit();
            }

            var session2 = getSession();
            using (var transaction = session2.BeginTransaction())
            {
                Assert.IsTrue(session2.Query<ComTrunk>().Any(t => t.ComTrunkName == "IShouldBeSaved"));
                session2.Delete(session2.Query<ComTrunk>().First(t => t.ComTrunkName == "IShouldBeSaved"));
                transaction.Commit();
            }
        }

        private static ISession getSession()
        {
            var cfg = new Configuration();

            cfg.Properties.Add(Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName);
            cfg.Properties.Add(Environment.ConnectionDriver, typeof(MySqlDataDriver).FullName);
            cfg.Properties.Add(Environment.Dialect, typeof(MySQLDialect).FullName);
            cfg.Properties.Add(Environment.ConnectionString,
                               "Server=10.10.20.183;Database=asterisk;User Id=asterisk;Password=password");
            cfg.Properties.Add(Environment.ShowSql, "false");
            cfg.AddAssembly("DataAccess");

            var sessionFactory = cfg.BuildSessionFactory();

            var session = sessionFactory.OpenSession();
            return session;
        }
    }
}
