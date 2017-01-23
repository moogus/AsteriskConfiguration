using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using StructureMap;

namespace ModelRepositoryTest.Tests.Generic
{
    [TestClass]
    public class TestGetSipTrunk
    {
        //requires the following packages    
      

        [TestMethod]
        public void TestFunction()
        {
            var modelContainer = new Container(new ModelIOC());
            var dataContainer = new Container(new DataIOC());
            var dataRepository = dataContainer.GetInstance<IDataRepository>();

            var emptyModelRepository = new EmptyModelRepository(dataRepository);

            var modelRepository = new ModelRepository.Internal.ModelRepositoryWithMapping(emptyModelRepository,modelContainer);



                        var s = from comTrunk in emptyModelRepository.Get<IComTrunk>(null)
                                from sipCred in emptyModelRepository.Get<IComSipCredentials>(x => true)
                                where comTrunk.Id == sipCred.TrunkId || sipCred.TrunkId == 0
                                select new SipTrunk(new Trunk(comTrunk, modelRepository), sipCred, modelRepository);

            s.Count().Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void TestFunction2()
        {
            var modelContainer = new Container(new ModelIOC());
            var dataContainer = new Container(new DataIOC());
            var dataRepository = dataContainer.GetInstance<IDataRepository>();

            var emptyModelRepository = new EmptyModelRepository(dataRepository);

            var modelRepository = new ModelRepository.Internal.ModelRepositoryWithMapping(emptyModelRepository, modelContainer);
           var s = emptyModelRepository.Add<IFourComFederatedLink>();

        


            s.Should().NotBeNull();
        }
    }
}
