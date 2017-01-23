using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class RingTone : IRingTone
  {
    private readonly IFuRingtone _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public RingTone(IFuRingtone fuRingtone, IRepositoryWithDelete modelRepository)
    {
      _under = fuRingtone;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Name
    {
      get { return _under.FuRingtoneName; }
      set { _under.FuRingtoneName = value; }
    }

    public string SipHeader
    {
      get { return _under.SipHeader; }
      set { _under.SipHeader = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}