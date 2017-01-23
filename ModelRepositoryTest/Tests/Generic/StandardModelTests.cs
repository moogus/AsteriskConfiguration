using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataAccess;
using DataAccess.TableInterfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.ModelInterfaces;
using Moq;
using StructureMap;

namespace ModelRepositoryTest.Tests.Generic
{
    [TestClass]
    public class StandardModelTests
    {
        private readonly Random _rand = new Random();

        private void TestGetByIdFromModelHolder(StandardModelHolder modelHolder)
        {
            // create some sample data to add to properties of mock object
            var mockId = _rand.Next();
            var mockData = "1234";

            // create mock object
            // setup queryable collection for repo to return
            var mockResults = modelHolder.StandardModel.GetMockList();

            mockResults[0].Id = mockId;
            modelHolder.SetDbName(mockResults[0], mockData);

            // make data repository mock
            var dataRepository = new Mock<IDataRepository>();
            modelHolder.StandardModel.SetupRepoQuery(System.Linq.Queryable.AsQueryable(mockResults), dataRepository);

            //ACTUALLY MAKE OUR OBJECT TO TEST!!!
            var baseRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepository = new ModelRepositoryWithMapping(baseRepository, new ModelIOC().Container);

            //Act
            // get test object from mock objects
            IModel testObj = modelHolder.StandardModel.GetFromId(modelRepository, mockId);

            //Assert          
            //check sample data is the same in our new object
            modelHolder.GetModelName(testObj).Should().Be(mockData);
        }

        private void TestGetByNameFromModelHolder(StandardModelHolder modelHolder)
        {
            // create some sample data to add to properties of mock object
            var mockId = _rand.Next();
            var mockData = "testy test";

            // create mock object
            // setup queryable collection for repo to return
            var mockResults = modelHolder.StandardModel.GetMockList();

            mockResults[0].Id = mockId;
            modelHolder.SetDbName(mockResults[0], mockData);

            // make data repository mock
            var dataRepository = new Mock<IDataRepository>();
            modelHolder.StandardModel.SetupRepoQuery(System.Linq.Queryable.AsQueryable(mockResults), dataRepository);

            //ACTUALLY MAKE OUR OBJECT TO TEST!!!
            var baseRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepository = new ModelRepositoryWithMapping(baseRepository, new ModelIOC().Container);

            //Act
            // get test object from mock objects
            IModel testObj = modelHolder.StandardModel.GetFromName(modelRepository, mockData);

            //Assert          
            //check sample data is the same in our new object
            modelHolder.GetModelName(testObj).Should().Be(mockData);
        }

        public void TestAddFromModelHolder(StandardModelHolder modelHolder)
        {
            var mockId = _rand.Next();
            var mockData = "testy test";

            // create mock object
            var mockItem = modelHolder.StandardModel.GetMockItem();

            mockItem.Id = mockId;
            modelHolder.SetDbName(mockItem, mockData);

            // make data repository mock
            var dataRepository = new Mock<IDataRepository>();
            //modelHolder.StandardModel.SetupRepoQuery(System.Linq.Queryable.AsQueryable(mockResults), dataRepository);
            modelHolder.StandardModel.SetupRepoCreate(mockItem, dataRepository);

            //ACTUALLY MAKE OUR OBJECT TO TEST!!!
            var baseRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepository = new ModelRepositoryWithMapping(baseRepository, new ModelIOC().Container);

            //Act
            // try adding an item to the repository.
            IModel testObj = modelHolder.StandardModel.Add(modelRepository);

            //Assert          
            //check sample data is the same in our new object
            modelHolder.GetModelName(testObj).Should().Be(mockData);
        }

        #region GetById_Tests

        [TestMethod]
        public void TestGetAutoAttendantById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.FuAutoAttendantName = n,
                new StandardModel<IAutoAttendant, IFuAutoAttendant>()
                ));
        }

        [TestMethod]
        public void TestGetAutoAttendantRulesById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.AaName,
                (d, n) => d.AaName = n,
                new StandardModel<IAutoAttendantRules, IFuAutoAttendantRules>()
                ));
        }

        [TestMethod]
        public void TestGetCliById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.CLIName,
                (d, n) => d.CLIName = n,
                new StandardModel<ICLI, IComCLI>()
                ));
        }

        [TestMethod]
        public void TestGetCurrentDialPlanById()
        {
            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next();

            var mocktbl = new Mock<IFuCurrentDialplan>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.CurrentDialplan).Returns(id2);

            var mockTbl2 = new Mock<IFuDialplan>();
            mockTbl2.Setup(e => e.Id).Returns(id2);

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IFuCurrentDialplan>())
                          .Returns(new List<IFuCurrentDialplan> { mocktbl.Object }.AsQueryable);
            dataRepository.Setup(d => d.GetQueryable<IFuDialplan>())
                          .Returns(new List<IFuDialplan> { mockTbl2.Object }.AsQueryable);


            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<ICurrentDialPlan>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.Dialplan.Id.Should().Be(id2);

        }

        [TestMethod]
        public void TestGetCurrentDialPlanDateById()
        {
            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next();

            var mocktbl = new Mock<IFuDialplanDate>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.FuDialplanId).Returns(id2);

            var mockDialPlan = new Mock<IFuDialplan>();
            mockDialPlan.Setup(e => e.Id).Returns(id2);

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IFuDialplanDate>())
                          .Returns(new List<IFuDialplanDate> { mocktbl.Object }.AsQueryable);
            dataRepository.Setup(d => d.GetQueryable<IFuDialplan>())
                          .Returns(new List<IFuDialplan> { mockDialPlan.Object }.AsQueryable);


            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<IDialplanDate>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.Dialplan.Id.Should().Be(id2);  
        }

        [TestMethod]
        public void TestGetDdiById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.DDINumber,
                (d, n) => d.DDI = n,
                new StandardModel<IDDI, IFuDDI>()
                ));
        }

        [TestMethod]
        public void TestGetDefaultById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Type,
                (d, n) => d.Type = n,
                new StandardModel<IDefault, IFuDefaults>()
                ));
        }

        [TestMethod]
        public void TestGetDialplanById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.FuDialPlanName = n,
                new StandardModel<IDialplan, IFuDialplan>()
                ));
        }

        [TestMethod]
        public void TestGetDialPlanRangeById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.DaysOfWeek,
                (d, n) => d.DaysOfWeek = n,
                new StandardModel<IDialplanRange, IFuDialplanRange>()
                ));
        }

        [TestMethod]
        public void TestGetEmergencyNumberById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Number,
                (d, n) => d.Number = n,
                new StandardModel<IEmergencyNumber, IFuEmergencyNumber>()
                ));
        }

        [TestMethod]
        public void TestGetFourcomFederatedLinkById()
        {
            //TODO: This is failing because it contains nested records.
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Description,
                (d, n) => d.Description = n,
                new StandardModel<IFourComFederatedLink, IFu4ComFederation>()
                ));
        }

        [TestMethod]
        public void TestGetKnownNumberById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Number,
                (d, n) => d.Number = n,
                new StandardModel<IKnownNumber, IFuKnownNumber>()
                ));
        }

        [TestMethod]
        public void TestGetMusicOnHoldById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.ComMusicOnHoldName = n,
                new StandardModel<IMusicOnHold, IComMusicOnHold>()
                ));
        }

        [TestMethod]
        public void TestGetPermissionPatternById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.FuPatternName = n,
                new StandardModel<IPermissionPattern, IFuPermissionPattern>()
                ));
        }

        [TestMethod]
        public void TestGetPermissionClassById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.FuPermissionClassName = n,
                new StandardModel<IPermisionClass, IFuPermissionClass>()
                ));
        }

        [TestMethod]
        public void TestGetPermissionClassMemberById()
        {

            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next();

            var mocktbl = new Mock<IFuPermisionClassMember>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.DialplanId).Returns(id2);

            var mockDialPlan = new Mock<IFuDialplan>();
            mockDialPlan.Setup(e => e.Id).Returns(id2);

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IFuPermisionClassMember>())
                          .Returns(new List<IFuPermisionClassMember> { mocktbl.Object }.AsQueryable);
            dataRepository.Setup(d => d.GetQueryable<IFuDialplan>())
                          .Returns(new List<IFuDialplan> { mockDialPlan.Object }.AsQueryable);


            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<IPermissionClassMember>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.Dialplan.Id.Should().Be(id2); 
         
        }

        [TestMethod]
        public void TestGetQueueMemberById()
        {
            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next();

            var mocktbl = new Mock<IComQueueMember>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.ParentQueueId).Returns(id2);

            var mockTbl2 = new Mock<IComQueue>();
            mockTbl2.Setup(e => e.Id).Returns(id2);

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IComQueueMember>())
                          .Returns(new List<IComQueueMember> { mocktbl.Object }.AsQueryable);
            dataRepository.Setup(d => d.GetQueryable<IComQueue>())
                          .Returns(new List<IComQueue> { mockTbl2.Object }.AsQueryable);


            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<IQueueMember>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.ParentQueue.Id.Should().Be(id2); 

         
        }

        [TestMethod]
        public void TestGetQueueById()
        {            
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Number,
                (d, n) => d.Number = n,
                new StandardModel<IQueue, IComQueue>()
                ));
        }

        [TestMethod]
        public void TestGetRingToneById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.FuRingtoneName = n,
                new StandardModel<IRingTone, IFuRingtone>()
                ));
        }

        [TestMethod]
        public void TestGetRoutingRuleById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Number,
                (d, n) => d.Number = n,
                new StandardModel<IRoutingRule, IComRoutingRule>()
                ));
        }

   

        [TestMethod]
        public void TestGetServerById()
        {            
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.IpAddress,
                (d, n) => d.IpAddress = n,
                new StandardModel<IServer, IComServer>()
                ));
        }

        [TestMethod]
        public void TestGetTrunkById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.ComTrunkName = n,
                new StandardModel<ITrunk, IComTrunk>()
                ));
        }

        [TestMethod]
        public void TestGetAccessCodeById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Code,
                (d, n) => d.Code = n,
                new StandardModel<IAccessCode, IComAccessCode>()
                ));
        }


        [TestMethod]
        public void TestGetBriTrunkById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Name,
                (d, n) => d.ComTrunkName = n,
                new StandardModel<IBriTrunk, IComTrunk>()
                ));
        }

        [TestMethod]
        public void TestGetDahdiChannelById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.ChannelName,
                (d, n) => d.ChannelName = n,
                new StandardModel<IDahdiChannel, IComDahdiChannel>()
                ));
        }

        [TestMethod]
        public void TestGetUserConfigById()
        {
            TestGetByIdFromModelHolder(new StandardModelHolder(
                m => m.Number,
                (d, n) => d.ExtensionNumber = n,
                new StandardModel<IUserConfig, IFuUserConfig>()
                ));
        }

        [TestMethod]
        public void TestGetVoicemailById()
        {

            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next().ToString(CultureInfo.InvariantCulture);

            var mocktbl = new Mock<IAstVoicemail>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.Mailbox).Returns(id2);

            

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IAstVoicemail>())
                          .Returns(new List<IAstVoicemail> { mocktbl.Object }.AsQueryable);
            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<IVoiceMail>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.Number.Should().Be(id2); 
        }

        [TestMethod]
        public void TestGetVoiceMessageById()
        {
            var rand = new Random();
            var id1 = rand.Next();
            var id2 = rand.Next().ToString(CultureInfo.InvariantCulture);

            var mocktbl = new Mock<IAstVoiceMessage>();
            mocktbl.Setup(e => e.Id).Returns(id1);
            mocktbl.Setup(e => e.Duration).Returns(id2);

            var dataRepository = new Mock<IDataRepository>();
            dataRepository.Setup(d => d.GetQueryable<IAstVoiceMessage>())
                          .Returns(new List<IAstVoiceMessage> { mocktbl.Object }.AsQueryable);

            //action
            var emptyModelRepository = new EmptyModelRepository(dataRepository.Object);
            var modelRepoWithMapping = new ModelRepositoryWithMapping(emptyModelRepository,
                                                                      new Mock<IContainer>().Object);

            var s = modelRepoWithMapping.GetFromId<IVoiceMessage>(id1);
            ////assert
            s.Id.Should().Be(id1);
            s.Duration.Should().Be(int.Parse(id2));

        }

        #endregion GetById_Tests

        #region GetByName_Tests

        #endregion

        #region Add_Tests

        #endregion
    }
}
