using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using ModelRepository;
using Moq;

namespace ModelRepositoryTest.Tests.Generic
{
    public class ComplexModelHolder
    {
        internal Func<dynamic, string> GetModelName;
        internal Action<dynamic, string> SetTableAProperty;
        internal Action<dynamic, string> SetTableBProperty;
        internal dynamic ComplexModel;

        internal ComplexModelHolder(Func<dynamic, string> getModelName, Action<dynamic, string> setTableAProperty, Action<dynamic, string> setTableBProperty, dynamic complexModel)
        {
            GetModelName = getModelName;
            SetTableAProperty = setTableAProperty;
            SetTableBProperty = setTableBProperty;
            ComplexModel = complexModel;
        }
    }

    public class ComplexModel<TModel, TDatabaseTableA, TDatabaseTableB>
        where TModel : class, IModel
        where TDatabaseTableA : class, IDatabaseTable
        where TDatabaseTableB : class, IDatabaseTable
    {
        internal IModel GetFromId(ModelRepository.Internal.ModelRepositoryWithMapping repo, int id)
        {
            return repo.GetFromId<TModel>(id);
        }

        internal List<TDatabaseTableA> GetMockListTableA()
        {
            var mockItemA = new Mock<TDatabaseTableA>();
            mockItemA.SetupAllProperties();

            return new List<TDatabaseTableA>() { mockItemA.Object };
        }

        internal List<TDatabaseTableB> GetMockListTableB()
        {
            var mockItemB = new Mock<TDatabaseTableB>();
            mockItemB.SetupAllProperties();

            return new List<TDatabaseTableB>() { mockItemB.Object };
        }

        internal void SetupRepoQuery(dynamic tableA, dynamic tableB, Mock<IDataRepository> repo)
        {
            repo.Setup(d => d.GetQueryable<TDatabaseTableA>()).Returns((IQueryable<TDatabaseTableA>)tableA);
            repo.Setup(d => d.GetQueryable<TDatabaseTableB>()).Returns((IQueryable<TDatabaseTableB>)tableB);
        }
    }
}
