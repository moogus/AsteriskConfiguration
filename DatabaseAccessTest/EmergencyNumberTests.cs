using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess.DatabaseTables;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;

namespace DatabaseAccessTest
{
  [TestClass]
  public class EmergencyNumberTests
  {
    [TestMethod]
    public void what()
    {
      var cfg = new Configuration();
      cfg.Configure();
      cfg.AddAssembly(typeof(ComExtension).Assembly);
      ISessionFactory sessionfactory = cfg.BuildSessionFactory();
      ISession session = sessionfactory.OpenSession();
      var a = session.Get<FuEmergencyNumber>(1);
      a.Number.Should().Be("07726000101");
    }
      
  }
}
