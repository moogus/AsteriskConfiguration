using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using ModelRepository;
using Moq;

namespace ModelRepositoryTest.Tests.Generic
{
    public class StandardModelHolder
    {
        internal Func<dynamic, string> GetModelName;
        internal Action<dynamic, string> SetDbName;
        internal dynamic StandardModel;

        internal StandardModelHolder(Func<dynamic, string> getModelName, Action<dynamic, string> setDbName, dynamic standardModel)
        {
            GetModelName = getModelName;
            SetDbName = setDbName;
            StandardModel = standardModel;
        }
    }

    public class StandardModel<TModel, TDb>
        where TModel : class, IModel
        where TDb : class, IDatabaseTable
    {
        internal IModel Add(ModelRepository.Internal.ModelRepositoryWithMapping repo)
        {
            return repo.Add<TModel>();
        }
        
        internal IModel GetFromId(ModelRepository.Internal.ModelRepositoryWithMapping repo, int id)
        {
            return repo.GetFromId<TModel>(id);
        }

        internal IModel GetFromName(ModelRepository.Internal.ModelRepositoryWithMapping repo, string name)
        {
            return repo.GetFromName<TModel>(name);
        }

        internal List<TDb> GetMockList()
        {
            var mockItem = new Mock<TDb>();
            mockItem.SetupAllProperties();

            return new List<TDb>() { mockItem.Object };
        }

        internal TDb GetMockItem()
        {
            var mockItem = new Mock<TDb>();
            mockItem.As<IDatabaseTable>();            

            mockItem.SetupAllProperties();

            return mockItem.Object;
        }

        internal void SetupRepoQuery(dynamic queryable, Mock<IDataRepository> repo)
        {
            repo.Setup(d => d.GetQueryable<TDb>()).Returns((IQueryable<TDb>)queryable);
        }
        
        internal void SetupRepoCreate(dynamic creatable, Mock<IDataRepository> repo)
        {
            repo.Setup(d => d.CreateNewObject<TDb>()).Returns((TDb)creatable);
        }
    }

    
}
