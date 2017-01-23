using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Metadata;
using StructureMap;
using Environment = NHibernate.Cfg.Environment;


namespace DataAccess.Internal.NHibernate
{
  internal class NhibernateDataConnector : IDataConnector
  {
    private readonly IContainer _container;
    private readonly string _connection;

    public NhibernateDataConnector(IContainer container, string connection)
    {
      _container = container;
      _connection = connection;
      Session = CreateSession();
    }

    public ISession Session { get; private set; }

    public IQueryable<T> GetQuery<T>() where T : IDatabaseTable
    {
      return Session.Query<T>();
    }

    public IDataRepositoryTransaction GetTransaction()
    {
      return new DataTransactionWrapper(Session.BeginTransaction());
    }

    #region -----private-----

    private ISession CreateSession()
    {
      return BuildSessionFactory().OpenSession(new StructureMapInterceptor(_container));
    }

    private ISessionFactory BuildSessionFactory()
    {
      var cfg = new Configuration();
      cfg.Properties.Add(Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName);
      cfg.Properties.Add(Environment.ConnectionDriver, typeof(MySqlDataDriver).FullName);
      cfg.Properties.Add(Environment.Dialect, typeof(MySQLDialect).FullName);
      cfg.Properties.Add(Environment.ConnectionString, _connection);
      cfg.Properties.Add(Environment.ShowSql, "false");
      cfg.AddAssembly("DataAccess");

      return cfg.BuildSessionFactory();
    }

    //private class used implement the IDataRepository and access stuff in this class without alot of dicking around
    private class DataTransactionWrapper : IDataRepositoryTransaction
    {
      private readonly ITransaction _transaction;
      public DataTransactionWrapper(ITransaction transaction)
      {
        _transaction = transaction;
      }

      public void Dispose()
      {
        _transaction.Dispose();
      }

      public bool Commit()
      {
        _transaction.Commit();
        return _transaction.WasCommitted;
      }
    }

    //private class to allow structuremap to work with nHibernate
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

    #endregion

  }
}
