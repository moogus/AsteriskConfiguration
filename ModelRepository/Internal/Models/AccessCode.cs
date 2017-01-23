using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal  class AccessCode : IAccessCode
  {
    private readonly IComAccessCode _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public AccessCode(IComAccessCode comAccessCode, IRepositoryWithDelete modelRepository)
    {
      _under = comAccessCode;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Code
    {
      get { return _under.Code; }
      set { _under.Code = value; }
    }

    public ITrunk ParentTrunk
    {
      get { return _modelRepository.GetFromId<ITrunk>(_under.TrunkId); }
      set { _under.TrunkId = value.Id; }
    }

    public int Priority
    {
      get { return _under.Priority; }
      set { _under.Priority = value; }
    }

    public void Delete()
    {
       _modelRepository.Delete(_under);
    }
  }
}