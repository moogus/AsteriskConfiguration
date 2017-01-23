using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class PermissionPattern : IPermissionPattern
  {
    private readonly IFuPermissionPattern _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public PermissionPattern(IFuPermissionPattern pattern, IRepositoryWithDelete modelRepository)
    {
      _under = pattern;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Name
    {
      get { return _under.FuPatternName; }
      set { _under.FuPatternName = value; }
    }

    public string Pattern
    {
      get { return _under.Pattern; }
      set { _under.Pattern = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}