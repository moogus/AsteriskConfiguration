using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using ModelUtilities;
using StructureMap;

namespace ModelRepository.Internal
{
    internal class ModelRepositoryWithMapping : IRepositoryWithDelete
    {
        private readonly IEmptyModelRepository _emptyModelRepository;
        private readonly IContainer _modelContainer;

        //TODO: Remove container from from this 
        public ModelRepositoryWithMapping(IEmptyModelRepository emptyModelRepository, IContainer modelContainer)
        {
           _emptyModelRepository = emptyModelRepository;
            _modelContainer = modelContainer;

            //Add models below:

            _emptyModelRepository.AddMapping(
              typeof(IAutoAttendant),
              (func) =>
              from
                fuAa in _emptyModelRepository.Get<IFuAutoAttendant>(func)
              select new AutoAttendant(fuAa, this));

            _emptyModelRepository.AddMapping(
              typeof(IAutoAttendantRules),
              (func) =>
              from
                fuAaRule in _emptyModelRepository.Get<IFuAutoAttendantRules>(func)
              select new AutoAttendantRules(fuAaRule, this));

            _emptyModelRepository.AddMapping(
              typeof(ICLI),
              (func) =>
              from
                cli in _emptyModelRepository.Get<IComCLI>(func)
              select new CLI(cli, this));

            _emptyModelRepository.AddMapping(
              typeof(ICurrentDialPlan),
              (func) =>
              from
                fuCurrentd in _emptyModelRepository.Get<IFuCurrentDialplan>(func)
              select new CurrentDialPlan(fuCurrentd, this));

            _emptyModelRepository.AddMapping(
              typeof(IDDI),
              (func) =>
              from
                ddi in _emptyModelRepository.Get<IFuDDI>(func)
              select new DDI(ddi, this));

            _emptyModelRepository.AddMapping(
              typeof(IDefault),
              (func) =>
              from
                fudefault in _emptyModelRepository.Get<IFuDefaults>(func)
              select new Default(fudefault, this));

            _emptyModelRepository.AddMapping(
              typeof(IDialplan),
              (func) =>
              from
                fuDialP in _emptyModelRepository.Get<IFuDialplan>(func)
              select new Dialplan(fuDialP, this));

            _emptyModelRepository.AddMapping(
              typeof(IDialplanDate),
              (func) =>
              from
                fuDpdate in _emptyModelRepository.Get<IFuDialplanDate>(func)
              select new DialplanDate(fuDpdate, this));

            emptyModelRepository.AddMapping(
              typeof(IDialplanRange),
              (func) =>
              from
                fuDpRange in _emptyModelRepository.Get<IFuDialplanRange>(func)
              select new DialplanRange(fuDpRange, this));

            emptyModelRepository.AddMapping(
              typeof(IEmergencyNumber),
              (func) =>
              from
                fuEmerg in _emptyModelRepository.Get<IFuEmergencyNumber>(func)
              select new EmergencyNumber(fuEmerg, this));

            _emptyModelRepository.AddMapping(

              typeof(IExtension),
              (func) =>
              from
                ext in _emptyModelRepository.Get<IComExtension>(func)
              join
                ast in _emptyModelRepository.Get<IAstSipReg>(x => true)
                on ext.Number equals ast.Number
              select new Extension(ext, ast, this));

            _emptyModelRepository.AddMapping(
                typeof(IFourComFederatedLink),
                (func) =>
                from
                    fed in _emptyModelRepository.Get<IFu4ComFederation>(func)
                select new FourComFederatedLink(fed, this));

            _emptyModelRepository.AddMapping(
              typeof(IKnownNumber),
              (func) =>
              from
                known in _emptyModelRepository.Get<IFuKnownNumber>(func)
              select new KnownNumber(known, this));

            _emptyModelRepository.AddMapping(
              typeof(IMusicOnHold),
              (func) =>
              from
                music in _emptyModelRepository.Get<IComMusicOnHold>(func)
              select new MusicOnHold(music, this));

            _emptyModelRepository.AddMapping(
              typeof(IPermissionPattern),
              (func) =>
              from
                patt in _emptyModelRepository.Get<IFuPermissionPattern>(func)
              select new PermissionPattern(patt, this));

            _emptyModelRepository.AddMapping(
              typeof(IPermisionClass),
              (func) =>
              from
                permClass in _emptyModelRepository.Get<IFuPermissionClass>(func)
              select new PermisionClass(permClass, this));

            _emptyModelRepository.AddMapping(
              typeof(IPermissionClassMember),
              (func) =>
              from
                permClassMem in _emptyModelRepository.Get<IFuPermisionClassMember>(func)
              select new PermissionClassMember(permClassMem, this));

            _emptyModelRepository.AddMapping(
              typeof(IQueueMember),
              (func) =>
              from
                queueMem in _emptyModelRepository.Get<IComQueueMember>(func)
              select new QueueMember(queueMem, this));

            _emptyModelRepository.AddMapping(
              typeof(IQueue),
              (func) =>
              from
                queue in _emptyModelRepository.Get<IComQueue>(func)
              select new Queue(queue, this));

            _emptyModelRepository.AddMapping(
              typeof(IRingTone),
              (func) =>
              from
                ring in _emptyModelRepository.Get<IFuRingtone>(func)
              select new RingTone(ring, this));

            _emptyModelRepository.AddMapping(
              typeof(IRoutingRule),
              (func) =>
              from
                rule in _emptyModelRepository.Get<IComRoutingRule>(func)
              select new RoutingRule(rule, this));

            _emptyModelRepository.AddMapping(
           typeof(ISamsungFederatedLink),
           (func) =>
           from
             fed in _emptyModelRepository.Get<IFuSamsungFederation>(func)
           select new SamsungFederatedLink(fed, this));

            _emptyModelRepository.AddMapping(
              typeof(IServer),
              (func) =>
              from
                serv in _emptyModelRepository.Get<IComServer>(func)
              select new Server(serv, this));

            _emptyModelRepository.AddMapping(
              typeof(ITrunk),
              (func) =>
              from
                comTrunk in _emptyModelRepository.Get<IComTrunk>(func)
              select new Trunk(comTrunk, this));

            _emptyModelRepository.AddMapping(
              typeof(IAccessCode),
              (func) =>
              from
                comCode in _emptyModelRepository.Get<IComAccessCode>(func)
              select new AccessCode(comCode, this));


            //TODO: inorder to test this the IDatabaseTable needs to be included in the mock not sure how
            _emptyModelRepository.AddMapping(
              typeof(IIaxTrunk),
              (func) =>
                  from
                 comTrunk in _emptyModelRepository.Get<IComTrunk>(func)
                  from iaxCred in _emptyModelRepository.Get<IFuIaxCredentials>(x => true)
                  where comTrunk.Id == iaxCred.TrunkId || iaxCred.TrunkId == 0
                  select new IaxTrunk(new Trunk(comTrunk, this), iaxCred, this));

            _emptyModelRepository.AddMapping(
              typeof(ISipTrunk),
              (func) =>
              from
                  comTrunk in _emptyModelRepository.Get<IComTrunk>(func)
              from sipCred in _emptyModelRepository.Get<IComSipCredentials>(x => true)
              where comTrunk.Id == sipCred.TrunkId || sipCred.TrunkId == 0
              select new SipTrunk(new Trunk(comTrunk, this), sipCred, this));

            _emptyModelRepository.AddMapping(
              typeof(IBriTrunk),
              (func) =>
              from
                comTrunk in _emptyModelRepository.Get<IComTrunk>(func)
              select new BriTrunk(new Trunk(comTrunk, this), this));

            _emptyModelRepository.AddMapping(
              typeof(IDahdiChannel),
              (func) =>
              from
                dChannel in _emptyModelRepository.Get<IComDahdiChannel>(func)
              select new DahdiChannel(dChannel, this));

            _emptyModelRepository.AddMapping(
              typeof(IUserConfig),
              (func) =>
              from
                userC in _emptyModelRepository.Get<IFuUserConfig>(func)
              select new UserConfig(userC, this));

            _emptyModelRepository.AddMapping(
              typeof(IVoiceMail),
              (func) =>
              from
                voicemail in _emptyModelRepository.Get<IAstVoicemail>(func)
              select new VoiceMail(voicemail, this, _modelContainer.GetInstance<IMessageFolderManager>()));

            _emptyModelRepository.AddMapping(
              typeof(IVoiceMessage),
              (func) =>
              from
                voicemessage in _emptyModelRepository.Get<IAstVoiceMessage>(func)
              select new VoiceMessage(voicemessage, this, _modelContainer.GetInstance<IAsteriskAudioStream>()));
        }

        public IRepositoryTransaction ModelTransaction()
        {
            return _emptyModelRepository.ModelTransaction();
        }

        public T Add<T>() where T : class, IModel
        {
            return _emptyModelRepository.Add<T>();
        }

        public IEnumerable<T> GetList<T>() where T : class, IModel
        {
            return _emptyModelRepository.GetList<T>();
        }

        public T GetFromId<T>(int id) where T : class, IModel
        {
            return _emptyModelRepository.GetFromId<T>(id);
        }

        public T GetFromName<T>(string name) where T : class, IModel
        {
            return _emptyModelRepository.GetFromName<T>(name);
        }

        public void Delete(object dataTable)
        {
            _emptyModelRepository.Delete(dataTable);
        }
    }
}