using System.Collections.Generic;
using System.Globalization;
using DatabaseAccess.DatabaseTables;
using DatabaseAccess.Models;
using NHibernate;
using NHibernate.Cfg;

namespace DatabaseAccess
{
  public class Repository : IRepository
  {
    private readonly IConfigurableRepository _repository;

    public Repository()
    {
      var cfg = new Configuration();
      cfg.Configure();
      cfg.AddAssembly(typeof(ComExtension).Assembly);
      ISessionFactory sessionfactory = cfg.BuildSessionFactory();
      ISession session = sessionfactory.OpenSession();
      var sessionWrapper = new SessionWrapper(session);

      _repository = new GenericRepository(sessionWrapper);

      _repository.AddTransformation<IExtension, ComExtension>(
          u => new Extension(u, sessionWrapper, this),
          (u, n) => u.Number == n,
          () => new Extension(sessionWrapper, this));

      _repository.AddTransformation<IVoiceMail, AstVoicemail>(
          u => new Voicemail(u, sessionWrapper, this),
          (u, n) => u.Mailbox == n,
          () => new Voicemail(sessionWrapper, this));

      _repository.AddTransformation<IDDI, FuDDI>(
          u => new DDI(u, sessionWrapper, this),
          (u, n) => u.DDI == n,
          () => new DDI(sessionWrapper, this));

      _repository.AddTransformation<IQueue, ComQueue>(
          u => new Queue(u, sessionWrapper, this),
          (u, n) => u.Number == n,
          () => new Queue(sessionWrapper, this));

      _repository.AddTransformation<IQueueMember, ComQueueMember>(
          u => new QueueMember(u, sessionWrapper, this),
          (u, n) => u.Id.ToString(CultureInfo.InvariantCulture) == n,
          () => new QueueMember(sessionWrapper, this));

      _repository.AddTransformation<IUserConfig, FuUserConfig>(
          u => new UserConfig(u, sessionWrapper, this),
          (u, n) => u.ExtensionNumber == n,
          () => new UserConfig(sessionWrapper, this));

      _repository.AddTransformation<IDefault, FuDefaults>(
          u => new Default(u, sessionWrapper, this),
          (u, n) => u.ColumnTitle == n,
          () => new Default(sessionWrapper, this));

      _repository.AddTransformation<IDialplan, FuDialplan>(
          u => new Dialplan(u, sessionWrapper, this),
          (u, n) => u.Name == n,
          () => new Dialplan(sessionWrapper, this));

      _repository.AddTransformation<IDialplanDate, FuDialplanDate>(
          u => new DialplanDate(u, sessionWrapper, this),
          (u, n) => u.StartDate.ToShortDateString() == n,
          () => new DialplanDate(sessionWrapper, this));

      _repository.AddTransformation<IDialplanRange, FuDialplanRange>(
          u => new DialplanRange(u, sessionWrapper, this),
          (u, n) => u.DaysOfWeek == n,
          () => new DialplanRange(sessionWrapper, this));

      _repository.AddTransformation<IRoutingRule, ComRoutingRule>(
          u => new RoutingRule(u, sessionWrapper, this),
          (u, n) => u.Number == n,
          () => new RoutingRule(sessionWrapper, this));

      _repository.AddTransformation<IRingTone, FuRingtone>(
          u => new RingTone(u, sessionWrapper, this),
          (u, n) => u.Name == n,
          () => new RingTone(sessionWrapper, this));

      _repository.AddTransformation<ICurrentDialPlan, FuCurrentDialplan>(
          u => new CurrentDialPlan(sessionWrapper, this),
          (u, n) => u.CurrentDialplan.ToString(CultureInfo.InvariantCulture) == n,
          () => new CurrentDialPlan(sessionWrapper, this));

      _repository.AddTransformation<IAutoAttendant, FuAutoAttendant>(
          u => new AutoAttendant(u, sessionWrapper, this),
          (u, n) => u.Name == n,
          () => new AutoAttendant(sessionWrapper, this));

      _repository.AddTransformation<IAutoAttendantRules, FuAutoAttendantRules>(
          u => new AutoAttendantRules(u, sessionWrapper, this),
          (u, n) => u.Entry == n,
          () => new AutoAttendantRules(sessionWrapper, this));

      _repository.AddTransformation<ICLI, ComCLI>(
          u => new CLI(u, sessionWrapper, this),
          (u, n) => u.CLINumber == n,
          () => new CLI(sessionWrapper, this));

      _repository.AddTransformation<IServer, ComServer>(
          u => new Server(u, sessionWrapper, this),
          (u, n) => u.IpAddress == n,
          () => new Server(sessionWrapper, this));

      _repository.AddTransformation<IVoiceMessage, AstVoiceMessage>(
          u => new VoiceMessage(u, sessionWrapper, this),
          (u, n) => u.MailBox == n,
          () => new VoiceMessage(sessionWrapper, this));

      _repository.AddTransformation<IKnownNumber, FuKnownNumber>(
            u => new KnownNumber(u, sessionWrapper, this),
            (u, n) => u.Number == n,
            () => new KnownNumber(sessionWrapper, this));

      _repository.AddTransformation<IPattern, FuPermissionPattern>(
          u => new PermissionPattern(u, sessionWrapper, this),
          (u, n) => u.Name == n,
          () => new PermissionPattern(sessionWrapper, this));

      _repository.AddTransformation<IPermisionClass, FuPermissionClass>(
         u => new PermisionClass(u, sessionWrapper, this),
         (u, n) => u.Name == n,
         () => new PermisionClass(sessionWrapper, this));

      _repository.AddTransformation<IPermissionClassMember, FuPermisionClassMember>(
          u => new PermissionClassMember(u, sessionWrapper, this),
          (u, n) => "" == n,
          () => new PermissionClassMember(sessionWrapper, this));

      _repository.AddTransformation<IEmergencyNumber, FuEmergencyNumber>(
        u => new EmergencyNumber(u, sessionWrapper, this),
        (u, n) => u.Number == n,
        () => new EmergencyNumber(sessionWrapper, this));

      _repository.AddTransformation<IMusicOnHold, ComMusicOnHold>(
        u => new MusicOnHold(u, sessionWrapper, this),
        (u, n) => u.Name == n,
        () => new MusicOnHold(sessionWrapper, this));

      _repository.AddTransformation<IFederation, FuFederation>(
        u => new Federation(u, sessionWrapper, this),
        (u, n) => u.Name == n,
        () => new Federation(sessionWrapper, this));

      _repository.AddTransformation<ITrunk, ComTrunk>(
      u => new Trunk(u, sessionWrapper, this),
      (u, n) => u.Name == n,
      () => new Trunk(sessionWrapper, this));

      _repository.AddTransformation<ISipTrunk, ComTrunk>(
        u => new SipTrunk(u, sessionWrapper, this),
        (u, n) => u.Name == n,
        () => new SipTrunk(sessionWrapper, this));

      _repository.AddTransformation<IBriTrunk, ComTrunk>(
         u => new BriTrunk(u, sessionWrapper, this), 
         (u, n) => u.Name == n,
         () => new BriTrunk(sessionWrapper, this));

      _repository.AddTransformation<IIaxTrunk, ComTrunk>(
         u => new IaxTrunk(u, sessionWrapper, this), 
         (u, n) => u.Name == n,
         () => new IaxTrunk(sessionWrapper, this));
    }

    public IEnumerable<T> GetList<T>() where T : class, IModel
    {
      return _repository.GetList<T>();
    }

    public T Add<T>() where T : class, IModel
    {
      return _repository.Add<T>();
    }

    public T GetFromId<T>(int id) where T : class, IModel
    {
      return _repository.GetFromId<T>(id);
    }

    public T GetFromName<T>(string name) where T : class, IModel
    {
      return _repository.GetFromName<T>(name);
    }
  }
}