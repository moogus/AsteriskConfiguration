using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Internal;

namespace ModelRepository.Internal
{
    internal class EmptyModelRepository : IEmptyModelRepository
    {
      /* THIS IS NOT THREADSAFE!!! if using threads, please make sure that _action is synchronised */

        private readonly IDataRepository _dataRepository;
        private readonly Dictionary<Type, Func<Func<IDatabaseTable, bool>, dynamic>> _getFromPredicateByType;
        private PredicateAction _action;

        public EmptyModelRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            _getFromPredicateByType = new Dictionary<Type, Func<Func<IDatabaseTable, bool>, dynamic>>();
        }

        public IRepositoryTransaction ModelTransaction()
        {

            return new ModelTransactionWrapper(_dataRepository.DatabaseTransaction());
        }

        public T Add<T>() where T : class, IModel
        {
            if (_getFromPredicateByType.ContainsKey(typeof(T)))
            {
                _action = PredicateAction.Add;
                var v = _getFromPredicateByType[typeof(T)](null) as IEnumerable<T>;
                if (v != null) return v.FirstOrDefault();
            }
            return null;
        }

        public T GetFromId<T>(int id) where T : class, IModel
        {
            if (_getFromPredicateByType.ContainsKey(typeof(T)))
            {
                _action = PredicateAction.GetFromPredicate;
                var v = _getFromPredicateByType[typeof(T)](o => o.Id == id) as IEnumerable<T>;
                if (v != null) return v.FirstOrDefault();
            }
            return null;
        }

        public T GetFromName<T>(string name) where T : class, IModel
        {
            if (_getFromPredicateByType.ContainsKey(typeof(T)))
            {
                _action = PredicateAction.GetFromPredicate;
                var v = _getFromPredicateByType[typeof(T)](o => o.Name == name) as IEnumerable<T>;
                if (v != null) return v.FirstOrDefault();
            }
            return null;
        }

        public void Delete(object dataTable)
        {
            _dataRepository.Delete(dataTable);
        }

        public bool IsIntransaction()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetList<T>() where T : class, IModel
        {
            if (_getFromPredicateByType.ContainsKey(typeof(T)))
            {
                _action = PredicateAction.GetList;
                var v = _getFromPredicateByType[typeof(T)](null) as IEnumerable<T>;
                return v;
            }
            return null;
        }

        public void Delete<T>(T model) where T : class, IDatabaseTable
        {
            _dataRepository.Delete(model);
        }

        public void AddMapping(Type type, Func<Func<IDatabaseTable, bool>, dynamic> mapping)
        {
            _getFromPredicateByType.Add(type, mapping);
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate) where T : IDatabaseTable
        {
            return DoGet(_action, predicate);
        }

        public IEnumerable<T> DoGet<T>(PredicateAction action, Func<T, bool> predicate) where T : IDatabaseTable
        {
            switch (action)
            {
                case PredicateAction.GetFromPredicate:
                    return _dataRepository.GetQueryable<T>().Where(predicate);
                case PredicateAction.GetList:
                    return _dataRepository.GetQueryable<T>();
                case PredicateAction.Add:
                    return new List<T> { _dataRepository.CreateNewObject<T>() };
            }
            return null;
        }

        #region -----private-----

        private class ModelTransactionWrapper : IRepositoryTransaction
        {
            private readonly IDataRepositoryTransaction _transaction;

            public ModelTransactionWrapper(IDataRepositoryTransaction transaction)
            {
                _transaction = transaction;
            }

            public void Dispose()
            {
                _transaction.Dispose();
            }

            public bool Commit()
            {
                return _transaction.Commit();
            }
        }

        #endregion
    }
}