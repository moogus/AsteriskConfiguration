using System.Linq;
using DataAccess;
using DataAccess.TableInterfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.Internal.Models;
using StructureMap;

namespace ModelRepositoryTest.Tests.Generic
{
    [TestClass]
    public class TestGetExtension
    {
        [TestMethod]
        public void TestGetByName()
        {
            var modelContainer = new Container(new ModelIOC());
            var dataContainer = new Container(new DataIOC());

            var dataRepository = dataContainer.GetInstance<IDataRepository>();

            var emptyModelRepository = new EmptyModelRepository(dataRepository);

            var modelRepository = new ModelRepositoryWithMapping(emptyModelRepository, modelContainer);

            var results = from ext in emptyModelRepository.Get<IComExtension>(null)
                    join ast in emptyModelRepository.Get<IAstSipReg>(a => true)
                        on ext.Name equals ast.Name
                          select new Extension(ext, ast, modelRepository);

            //var s = from comTrunk in emptyModelRepository.Get<IComTrunk>(null)
            //        from sipCred in emptyModelRepository.Get<IComSipCredentials>(x => true)
            //        where comTrunk.Id == sipCred.TrunkId || sipCred.TrunkId == 0
            //        select new SipTrunk(new Trunk(comTrunk, modelRepository), sipCred, modelRepository);

            results.Count().Should().BeGreaterThan(0);
        }
    }
}
