using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;
using NHibernate;
using NHibernate.Cfg;

namespace DatabaseAccess
{
  public class DatabaseTester
  {
    public DatabaseTester()
    {
      var cfg = new Configuration();
      cfg.Configure();
      cfg.AddAssembly(typeof(ComExtension).Assembly);
      ISessionFactory sessionfactory = cfg.BuildSessionFactory();
      ISession session = sessionfactory.OpenSession();
      //var sessionWrapper = new SessionWrapper(session);

      
        using (ITransaction transaction = session.BeginTransaction())
        {
          ComTrunk comTrunk = new ComTrunk();
          comTrunk.Name = "fred";
          comTrunk.Type = "fred";
          comTrunk.DefaultDestination = "fred";
          comTrunk.CLIPresentationType1 = "fred";
          comTrunk.CLIPresentationValue1 = "fred";
          session.SaveOrUpdate(comTrunk);

          //var server = new ComServer();
          //server.UserName = "fred";
          //session.SaveOrUpdate(server);

          transaction.Commit();
        }
      Console.ReadLine();
    }
  }
}
