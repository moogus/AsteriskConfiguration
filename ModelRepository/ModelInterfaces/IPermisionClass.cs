using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
  public interface IPermisionClass : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    List<IPermissionClassMember> PermissionClassMemebers { get; set; }
    void AddPermissionClassMemeber(IPermissionClassMember permissionClassMember);
  }
}