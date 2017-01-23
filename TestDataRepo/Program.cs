using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Internal;
using DataAccess.Internal.NHibernate;
using DataAccess.TableInterfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Metadata;
using StructureMap;
using Environment = NHibernate.Cfg.Environment;


namespace TestDataRepo
{
  class Program
  {
    static void Main(string[] args)
    {
      var container = new DataIOC().Container;

      var cfg = new Configuration();

      //var ndbc = new DBConnection();
      var dbcon = container.GetInstance<IDBConnection>();
      var ic = container.GetInstance<IContainer>();
      var sfc = container.GetInstance<ISessionFactoryCreator>();
      
      var dbc = container.GetInstance<IDataConnector>();
      var dbq = container.GetInstance<IDataQuery>();

      cfg.Properties.Add(Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName);
      cfg.Properties.Add(Environment.ConnectionDriver, typeof(MySqlDataDriver).FullName);
      cfg.Properties.Add(Environment.Dialect, typeof(MySQLDialect).FullName);
      cfg.Properties.Add(Environment.ConnectionString, "Server=10.10.20.183;Database=asterisk;User Id=asterisk;Password=password");
      cfg.Properties.Add(Environment.ShowSql, "false");
      cfg.AddAssembly("DataAccess");

      ISessionFactory SessionFactory = cfg.BuildSessionFactory();

      SessionFactory.OpenSession(new StructureMapInterceptor(container));

      var x = container.GetInstance<IComExtension>();

      Console.ReadKey();

    }

    private class StructureMapInterceptor : EmptyInterceptor
    {
      private readonly IContainer _container;
      private ISession _session;

      public StructureMapInterceptor(IContainer container)
      {
        _container = container;
      }

      public override void SetSession(ISession session)
      {
        base.SetSession(session);
        _session = session;
      }

      public override object Instantiate(string clazz, EntityMode entityMode, object id)
      {
        if (entityMode == EntityMode.Poco)
        {
          ISessionFactory sessionFactory = _session.SessionFactory;
          IClassMetadata metadata = sessionFactory.GetAllClassMetadata()[clazz];
          Type type = metadata.GetMappedClass(entityMode);

          object instance = _container.GetInstance(type);
          metadata.SetIdentifier(instance, id, entityMode);

          return instance;
        }
        return base.Instantiate(clazz, entityMode, id);
      }
    }

  }
}
