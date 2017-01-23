using System;
using System.Linq;
using System.Web.Security;
using ModelRepository;
using ModelRepository.ModelInterfaces;
using StructureMap;

namespace Asterisk.AccountManagement
{
  public class AsteriskRoleProvider : RoleProvider
  {
    public override string ApplicationName
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    //TODO: ThreadExceptionEventArgs repository does not HttpWorkerRequest when opening the UserConfig 

    //todo: fix all of NotImplementedException -- what are you going to do with them?  what needs implementing?

    public override bool IsUserInRole(string username, string roleName)
    {
      bool role = GetUserConfig(username).Role == roleName;

      return role;
    }

    private IUserConfig GetUserConfig(string username)
    {
      var repository = ObjectFactory.GetInstance<IRepository>();

      return repository.GetFromName<IUserConfig>(username);
    }

    public override string[] GetRolesForUser(string username)
    {
      //TODO : does this still blow up?  give it a while before removing this TD

      string x = GetUserConfig(username).Role;
      var rtn = new[] {x};
      return rtn;
    }

    public override void CreateRole(string roleName)
    {
      throw new NotImplementedException();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
      throw new NotImplementedException();
    }

    public override bool RoleExists(string roleName)
    {
      var repository = ObjectFactory.GetInstance<IRepository>();
      return repository.GetList<IUserConfig>().Any(r => r.Role == roleName);
    }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
      throw new NotImplementedException();
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
      throw new NotImplementedException();
    }

    public override string[] GetUsersInRole(string roleName)
    {
      throw new NotImplementedException();
    }

    public override string[] GetAllRoles()
    {
      throw new NotImplementedException();
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
      throw new NotImplementedException();
    }
  }
}