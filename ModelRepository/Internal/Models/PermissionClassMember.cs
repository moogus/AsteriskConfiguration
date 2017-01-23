using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class PermissionClassMember : IPermissionClassMember
  {
    private readonly IFuPermisionClassMember _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public PermissionClassMember(IFuPermisionClassMember permisionClassMember, IRepositoryWithDelete modelRepository)
    {
      _under = permisionClassMember;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public IPermisionClass ParentPermissionClass
    {
      get { return _modelRepository.GetFromId<IPermisionClass>(_under.PermissionClassId); }
      set { _under.PermissionClassId = value.Id; }
    }

    public IPermissionPattern Pattern
    {
      get { return _modelRepository.GetFromId<IPermissionPattern>(_under.PermissionPatternId); }
      set { _under.PermissionPatternId = value.Id; }
    }

    public IDialplan Dialplan
    {
      get { return _modelRepository.GetFromId<IDialplan>(_under.DialplanId); }
      set { _under.DialplanId = value.Id; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}