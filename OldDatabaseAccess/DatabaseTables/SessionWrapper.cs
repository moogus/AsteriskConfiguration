using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;

namespace DatabaseAccess.DatabaseTables
{
    public interface ISessionWrapper
    {
        IQueryable<T> Query<T>();
        ITransaction BeginTransaction();
        void SaveOrUpdate<T>(T what);
        void Delete<T>(T what);
    }

    internal class SessionWrapper : ISessionWrapper
    {
        private NHibernate.ISession _iSession;

        public SessionWrapper(NHibernate.ISession iSession)
        {
            this._iSession = iSession;
        }
        public IQueryable<T> Query<T>()
        {
            return _iSession.Query<T>();
        }
        public ITransaction BeginTransaction()
        {
            return _iSession.BeginTransaction();
        }
        public void SaveOrUpdate<T>(T what)
        {
            _iSession.SaveOrUpdate(what);
        }
        public void Delete<T>(T what)
        {
            _iSession.Delete(what);
        }
    }
}
