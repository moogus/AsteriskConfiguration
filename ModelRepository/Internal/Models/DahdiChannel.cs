using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class DahdiChannel : IDahdiChannel
  {
    private readonly IComDahdiChannel _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public DahdiChannel(IComDahdiChannel comDahdiChannel, IRepositoryWithDelete modelRepository)
    {
      _under = comDahdiChannel;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public ITrunk ParentTrunk
    {
      get { return _modelRepository.GetFromId<ITrunk>(_under.TrunkId); }
      set { _under.TrunkId = value.Id; }
    }

    public string ChannelName
    {
      get { return _under.ChannelName; }
      set { _under.ChannelName = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}