using DatabaseAccess;
using DialplanManager.DialplanClasses;
using DialplanManager.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using StructureMap;

namespace PeriodCheck
{
  public class Program
  {
    
    private static void Main()
    {
      var cfg = new Configuration();
      cfg.Configure();
      //cfg.AddAssembly(typeof(ComExtension).Assembly);
      ISessionFactory sessionfactory = cfg.BuildSessionFactory();
      ISession session = sessionfactory.OpenSession();


      var repository = new Repository();
      var currentDialplanManager = new CurrentDialplanManager();
      //var container = ConfigureDepancies();
      //var currentDialplanManager = container.GetInstance<ICurrentDialplanManager>();
      currentDialplanManager.SetCurrentDialplan();
    }
 
  }
}

