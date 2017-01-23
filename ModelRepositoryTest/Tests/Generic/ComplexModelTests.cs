using System;
using System.Linq;
using DataAccess;
using DataAccess.TableInterfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.ModelInterfaces;
using Moq;

namespace ModelRepositoryTest.Tests.Generic
{
    [TestClass]
    public class ComplexModelTests
    {
        private readonly Random _rand = new Random();

        private void TestGetByIdFromModelHolder(ComplexModelHolder modelHolder)
        {
            // create some sample data to add to properties of mock object
            var mockId = 1000; //_rand.Next();
            var mockData = "123456";

            // create mock object
            // setup queryable collection for repo to return
            var mockResultsA = modelHolder.ComplexModel.GetMockListTableA();
            var mockResultsB = modelHolder.ComplexModel.GetMockListTableB();

            mockResultsA[0].Id = mockId;
            modelHolder.SetTableAProperty(mockResultsA[0], mockData);

            mockResultsB[0].Id = mockId;
            modelHolder.SetTableBProperty(mockResultsB[0], mockData);

            // make data repository mock
            var dataRepository = new Mock<IDataRepository>();
            modelHolder.ComplexModel.SetupRepoQuery(
                Queryable.AsQueryable(mockResultsA),
                Queryable.AsQueryable(mockResultsB),
                dataRepository);

            //ACTUALLY MAKE OUR OBJECT TO TEST!!!
            var baseRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepository = new ModelRepositoryWithMapping(baseRepository, new ModelIOC().Container);

            //Act
            // get test object from mock objects
            IModel testObj = modelHolder.ComplexModel.GetFromId(modelRepository, mockId);

            //Assert          
            //check sample data is the same in our new object
            modelHolder.GetModelName(testObj).Should().Be(mockData);
        }

        [TestMethod]
        public void TestGetExtensionById()
        {
            TestGetByIdFromModelHolder(new ComplexModelHolder(
                m => m.Number,
                (d, n) => d.Number = n,
                (d, n) => d.Number = n,
                new ComplexModel<IExtension, IComExtension, IAstSipReg>()
                ));
        }

        [TestMethod]
        public void TestGetIaxTrunkById()
        {
            TestGetByIdFromModelHolder(new ComplexModelHolder(
                m => m.IaxName,
                (d, n) => d.FuIaxCredentialName = n,
                (d, n) => d.ComTrunkName = n,
                new ComplexModel<IIaxTrunk, IFuIaxCredentials, IComTrunk>()
                ));
        }

        [TestMethod]
        public void TestGetFourcomFederatedLinkById()
        {
            TestGetByIdFromModelHolder(new ComplexModelHolder(
                m => m.Name,
                (d, n) => d.FuFederationName = n,
                (d, n) => d.ComTrunkName = n,
                new ComplexModel<IFourComFederatedLink, IFu4ComFederation, IComTrunk>()
                ));
        }

        [TestMethod]
        public void TestGetSamsungFederatedLinkById()
        {
            TestGetByIdFromModelHolder(new ComplexModelHolder(
                m => m.Name,
                (d, n) => d.FuFederationName = n,
                (d, n) => d.ComTrunkName = n,
                new ComplexModel<ISamsungFederatedLink, IFuSamsungFederation, IComTrunk>()
                ));
        }

        [TestMethod]
        public void TestGetSipTrunkById()
        {
            TestGetByIdFromModelHolder(new ComplexModelHolder(
                m => m.SipUserName,
                (d, n) => d.UserName = n,
                (d, n) => d.ComTrunkName = n,
                new ComplexModel<ISipTrunk, IComSipCredentials, IComTrunk>()
                ));
        }

        //[TestMethod]    
        //public void TestGetExtensionByIdbyHand()
        //{
        //    var fieldPredicate = 19;
        //    var linkingField = "1200";

        //    var mockParentObject = new Mock<IComExtension>();
        //    mockParentObject.Setup(e => e.Id).Returns(fieldPredicate);
        //    mockParentObject.Setup(e => e.Number).Returns(linkingField);

        //    var mockChildObject = new Mock<IAstSipReg>();
        //    mockChildObject.Setup(a => a.Number).Returns(linkingField);


        //    var dataContainer = new Container(new DataIOC());
        //    var dataRepository = new Mock<IDataRepository>();//dataContainer.GetInstance<IDataRepository>();
        //    dataRepository.Setup(d => d.GetQueryable<IComExtension>()).Returns(new List<IComExtension>{mockParentObject.Object}.AsQueryable);
        //    dataRepository.Setup(d => d.GetQueryable<IAstSipReg>()).Returns(new List<IAstSipReg>{mockChildObject.Object}.AsQueryable);

        //    //var comExtensionResults = dataRepository.Object.GetQueryable<IComExtension>();
        //    //var astSipRegResults = dataRepository.Object.GetQueryable<IAstSipReg>();

        //    //action
        //    var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
        //    var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
        //                                                              new Mock<IContainer>().Object);

        //    var s = modelRepoWithMapping.GetFromId<IExtension>(19);
        //    //var results = modelRepoWithMapping.GetList<IExtension>();

        //    ////assert
        //    s.Id.Should().Be(fieldPredicate);
        //    s.Number.Should().Be(linkingField);
        //}
    }
}
