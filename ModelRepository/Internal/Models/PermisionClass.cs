using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class PermisionClass : IPermisionClass
  {
    private readonly IFuPermissionClass _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private List<IPermissionClassMember> _permissionClassMemeber;

    public PermisionClass(IFuPermissionClass permClass, IRepositoryWithDelete modelRepository)
    {
      _under = permClass;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Name
    {
      get { return _under.FuPermissionClassName; }
      set { _under.FuPermissionClassName = value; }
    }

    public string Description
    {
      get { return _under.Description; }
      set { _under.Description = value; }
    }

    //used to lazy-load the list
    private List<IPermissionClassMember> LazyPermissionClassMemebers
    {
      get
      {
        _permissionClassMemeber = _permissionClassMemeber ??
                                  _modelRepository.GetList<IPermissionClassMember>()
                                             .Where(a => a.ParentPermissionClass.Id == _under.Id)
                                             .ToList();
        return _permissionClassMemeber;
      }
    }

    public List<IPermissionClassMember> PermissionClassMemebers
    {
      get { return LazyPermissionClassMemebers; }
      set { _permissionClassMemeber = value; }
    }

    public void AddPermissionClassMemeber(IPermissionClassMember permissionClassMember)
    {
      LazyPermissionClassMemebers.Add(permissionClassMember);
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}