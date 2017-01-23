using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;

namespace Repository
{
    public class Repository
    {
        private readonly Dictionary<Type, dynamic> _repositories;
        private readonly ISession _session;

        public Repository()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof (AstSipFriend).Assembly);
            ISessionFactory sessionfactory = cfg.BuildSessionFactory();
            _session = sessionfactory.OpenSession();

            _repositories = new Dictionary<Type, dynamic>
                                {
                                    {
                                        typeof (IExtension),
                                        new Repository<IExtension, AstSipFriend>(_session, u => u.Context == "LocalSets",
                                                                                 u => new Extension(u, _session, this),
                                                                                 () => new Extension(_session, this),
                                                                                 (u, n) => u.Number == n)
                                        },
                                    {
                                        typeof (IVoiceMail),
                                        new Repository<IVoiceMail, AstVoicemail>(_session, u => true,
                                                                                 u => new Voicemail(u, _session, this),
                                                                                 () => new Voicemail(_session, this),
                                                                                 (u, n) => u.Mailbox == n)
                                        }
                                };
        }

        public IEnumerable<T> GetList<T>() where T : IModel
        {
            return _repositories.ContainsKey(typeof (T)) ? _repositories[typeof (T)].GetList() : null;
        }

        public T Add<T>() where T : class, IModel
        {
            return _repositories.ContainsKey(typeof (T)) ? _repositories[typeof (T)].Add() : null;
        }

        public T GetFromId<T>(int id) where T : class
        {
            return _repositories.ContainsKey(typeof (T)) ? _repositories[typeof (T)].GetFromId(id) : null;
        }

        public T GetFromName<T>(string name) where T : class
        {
            return _repositories.ContainsKey(typeof (T)) ? _repositories[typeof (T)].GetFromName(name) : null;
        }
    }

    public class Repository<T, TDatabase> where TDatabase : IDatabaseTable where T : IModel
    {
        private readonly Func<TDatabase, T> _factory;
        private readonly Func<T> _newFactory;
        private readonly Func<TDatabase, bool> _predicate;
        private readonly ISession _session;
        private readonly Func<TDatabase, string, bool> _testName;

        public Repository(ISession session, Func<TDatabase, bool> predicate, Func<TDatabase, T> factory,
                          Func<T> newFactory, Func<TDatabase, string, bool> testName)
        {
            _session = session;
            _predicate = predicate;
            _factory = factory;
            _newFactory = newFactory;
            _testName = testName;
        }

        public IEnumerable<T> GetList()
        {
            IEnumerable<TDatabase> unders = _session.Query<TDatabase>().Where(_predicate);
            dynamic overs = unders.Select(u => _factory(u));
            return overs;
        }

        public T Add()
        {
            return _newFactory();
        }

        public T GetFromId(int id)
        {
            return GetFromPredicate(i => i.Id == id);
        }

        public T GetFromName(string name)
        {
            return GetFromPredicate(a => _testName(a, name));
        }

        private T GetFromPredicate(Func<TDatabase, bool> predicate)
        {
            return _factory(_session.Query<TDatabase>().FirstOrDefault(predicate));
        }
    }

    public interface IModel
    {
        bool Update();
        bool Delete();
    }
}