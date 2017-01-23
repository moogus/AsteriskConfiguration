using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public class Repository<T>
    {
        // cache the underlying repositiories so that we only get one instance of each type (we know that this static is not shared across instances of different types so we disable the warning
        // ReSharper disable StaticFieldInGenericType
        private static dynamic _underlyingRepository;
        // ReSharper restore StaticFieldInGenericType

        public Repository()
        {
            if (_underlyingRepository == null)
            {
                if (typeof(T) == typeof(IExtension))
                {
                    _underlyingRepository = new Repository<IExtension, IUnderlyingExtension>(a => new Extension(a));
                }
                if (typeof(T) == typeof(IQueue))
                {
                    _underlyingRepository = new Repository<IQueue, IUnderlyingQueue>(a => new Queue(a));
                }
                if (typeof(T) == typeof(IQueueMember))
                {
                    _underlyingRepository = new Repository<IQueueMember, IUnderlyingQueueMember>(a => new QueueMember(a));
                }
                if (typeof(T) == typeof(IDDI))
                {
                    _underlyingRepository = new Repository<IDDI, IUnderlyingDDI>(a => new DDI(a));
                }
                if (typeof(T) == typeof(IDialplan))
                {
                    _underlyingRepository = new Repository<IDialplan, IUnderlyingDialplan>(a => new Dialplan(a));
                }
                if (typeof(T) == typeof(IDialplanDate))
                {
                    _underlyingRepository =
                        new Repository<IDialplanDate, IUnderlyingDialplanDate>(a => new DialplanDate(a));
                }
                if (typeof(T) == typeof(IDialplanRange))
                {
                    _underlyingRepository =
                        new Repository<IDialplanRange, IUnderlyingDialplanRange>(a => new DialplanRange(a));
                }
                if (typeof(T) == typeof(IRoutingRule))
                {
                    _underlyingRepository = new Repository<IRoutingRule, IUnderlyingRoutingRule>(a => new RoutingRule(a));
                }
                if (typeof(T) == typeof(ISipTrunk))
                {
                    _underlyingRepository = new Repository<ISipTrunk, IUnderlyingSipTrunk>(a => new SipTrunk(a));
                }
                if (typeof(T) == typeof(IVoiceMail))
                {
                    _underlyingRepository = new Repository<IVoiceMail, IUnderlyingVoicemail>(a => new VoiceMail(a));
                }
                if (typeof(T) == typeof(IUserConfig))
                {
                  _underlyingRepository = new Repository<IUserConfig, IUnderlyingUserConfig>(a => new UserConfig(a));
                }
                if (typeof(T) == typeof(IDefault))
                {
                  _underlyingRepository = new Repository<IDefault, IUnderlyingDefaults>(a => new Default(a));
                }
            }
        }

        public IEnumerable<T> GetList()
        {
            return _underlyingRepository.GetList();
        }

        public T GetFromId(int id)
        {
            return _underlyingRepository.GetFromId(id);
        }

        public T Add()
        {
            return _underlyingRepository.Add();
        }

        public T GetFromName(string name)
        {
            return _underlyingRepository.GetFromName(name);
        }

        public T GetFromUnderlying(dynamic underlying)
        {
            return _underlyingRepository.GetFromUnderlying(underlying);
        }

    }

    // Some methods are never accessed directly as the instance of this will alwayd be held in a dynamic variable.
    // ReSharper disable UnusedMember.Global

    public class Repository<T, TUnderlying> where TUnderlying : class
    {
        private readonly Func<TUnderlying, T> _factory;
        private readonly DatabaseAccessRepository<TUnderlying> _underlyingRepository;

        public Repository(Func<TUnderlying, T> factory)
        {
            _factory = factory;
            _underlyingRepository = new DatabaseAccessRepository<TUnderlying>();
        }

        public IEnumerable<T> GetList()
        {
            var h = _underlyingRepository.GetAll();
            return h.Select(_factory);
        }

        public T GetFromId(int id)
        {
            return _factory(_underlyingRepository.GetFromId(id));
        }

        public T Add()
        {
            return _factory(_underlyingRepository.Add());
        }

        public T GetFromName(string name)
        {
            var underlying = _underlyingRepository.GetFromName(name);
            return underlying == null ? default(T) : _factory(underlying);
        }

        public T GetFromUnderlying(TUnderlying underlying)
        {
            return _factory(underlying);
        }
    }
    // ReSharper restore UnusedMember.Global

}
