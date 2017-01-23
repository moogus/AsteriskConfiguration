using System;
using System.Linq;

namespace DataAccess.Internal
{
    internal class DatabaseRepository : IDataRepository
    {
        private readonly IDataConnector _dataConnector;
        private readonly IGetDatabaseTable _getDatabaseTable;

        public DatabaseRepository(IDataConnector dataConnector, IGetDatabaseTable getDatabaseTable)
        {
            _dataConnector = dataConnector;
            _getDatabaseTable = getDatabaseTable;
        }

        public IDataRepositoryTransaction DatabaseTransaction()
        {
            return _dataConnector.GetTransaction();
        }

        public T CreateNewObject<T>() where T : IDatabaseTable
        {
            if (ValidateType<T>())
            {
                var objectToReturn = GetObjectByFunction(_getDatabaseTable.GetDatabaseTableInstance<T>());
                _dataConnector.Session.Save(objectToReturn);
                return objectToReturn;
            }
            return default(T);
        }

        public IQueryable<T> GetQueryable<T>() where T : IDatabaseTable
        {
            return ValidateType<T>() ? GetListByFunction(_dataConnector.GetQuery<T>()) : null;
        }

        public void Delete(object databaseTable)
        {
            _dataConnector.Session.Delete(databaseTable);
        }

        #region private methods

        private static bool ValidateType<T>() where T : IDatabaseTable
        {
            return typeof(T) != typeof(IDatabaseTable) && typeof(IDatabaseTable).IsAssignableFrom(typeof(T));
        }

        private static T GetObjectByFunction<T>(T getObject)
        {
            try
            {
                return getObject;
            }
            catch
            {
                return default(T);
            }
        }

        private static IQueryable<T> GetListByFunction<T>(IQueryable<T> listOfObjects)
        {
            try
            {
                return listOfObjects;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }


}