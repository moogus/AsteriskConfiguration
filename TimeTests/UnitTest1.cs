using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;
using DatabaseAccess.DatabaseTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace TimeTests
{
    [TestClass]
    public class UnitTest1 : AbstractUsesSession
    {
        [TestMethod]
        public void TimeModelFromRepo()
        {
            var repo = new Repository<IExtension>();

            var t = DateTime.Now;
            var a = repo.GetList().ToList();

            Debug.WriteLine(DateTime.Now - t);
            Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 2));
        }

        //[TestMethod]
        //public void TimeModel()
        //{
        //    var repo = new DatabaseAccessRepository<IUnderlyingQueueMember>();

        //    var t = DateTime.Now;

        //    var a = repo.GetAll().ToList();
        //   var b = a.Select(f => new QueueMember(f)).ToList();

        //    Debug.WriteLine(DateTime.Now - t);
        //    Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 1));
        //}

        //[TestMethod]
        //public void TimeModelFromUnderlyingModel()
        //{
        //    var t = DateTime.Now;
        //    var repo = new DatabaseAccessRepository<IUnderlyingExtension>();
        //    List<IUnderlyingExtension> a = new List<IUnderlyingExtension>();
        //    for (var i = 1; i < 10; i++)
        //    {
        //        a = repo.GetAll().ToList();

        //    }

        //    for (var i = 1; i < 10; i++)
        //    {
        //        var b = a.Select(c => new Extension(c)).ToList();
        //    }

        //    Debug.WriteLine(DateTime.Now - t);
        //    Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 3));
        //}

        //[TestMethod]
        //public void TimeUnderlyingModel()
        //{
        //    var repo = new DatabaseAccessRepository<IUnderlyingExtension>();

        //    var t = DateTime.Now;
        //    for (var i = 1; i < 10; i++)
        //    {
        //        var a = repo.GetAll().ToList();
        //    }

        //    Debug.WriteLine(DateTime.Now - t);
        //    Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 3));
        //}


        //[TestMethod]
        //public void TimeGetFromDB()
        //{
        //    var t = DateTime.Now;

        //    using (ISession session = SessionFactory().OpenSession())
        //    {

        //        IList<FuDefaults> underlyingEntities =
        //            session.CreateCriteria(typeof(FuDefaults)).List<FuDefaults>().Where(
        //                a => true).ToList();

        //    }
        //    Debug.WriteLine(DateTime.Now - t);
        //    Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 2));
        //}

        //[TestMethod]
        //public void TimeCreateUnderlyingModel()
        //{
        //    IList<FuDefaults> underlyingEntities;
        //    using (ISession session = SessionFactory().OpenSession())
        //    {

        //        underlyingEntities = session.CreateCriteria(typeof(FuDefaults)).List<FuDefaults>().Where(
        //            a => true).ToList();
        //    }
        //    var t = DateTime.Now;

            
        //    var b = underlyingEntities.Select(a => new FuDefaultsLinked(a)).ToList();

        //    Debug.WriteLine(DateTime.Now - t);
        //    Assert.IsTrue(DateTime.Now - t < new TimeSpan(0, 0, 3));
        //}
    }

    
}
