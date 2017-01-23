using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class PermisionClass : IPermisionClass, IModel
  {
    private readonly FuPermissionClass _under;
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private List<IPermissionClassMember> _permissionClassMemeber;


    internal PermisionClass(FuPermissionClass under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal PermisionClass(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuPermissionClass();
      _permissionClassMemeber = new List<IPermissionClassMember>();
    }

    public int Id { get { return _under.Id; } }
    public string Name { get { return _under.Name; } set { _under.Name = value; } }
    public string Description { get { return _under.Description; } set { _under.Description = value; } }
    public List<IPermissionClassMember> PermissionClassMemebers
    {
      get { return _PermissionClassMemebers; }
      //todo make this like adding extensions to queumemebers
      set { _permissionClassMemeber = value; }
    }


    private List<IPermissionClassMember> _PermissionClassMemebers
    {
      get
      {
        _permissionClassMemeber = _permissionClassMemeber ?? _repository.GetList<IPermissionClassMember>().Where(a => a.ParentPermissionClass.Id == _under.Id).ToList();
        return _repository.GetList<IPermissionClassMember>().Where(a => a.ParentPermissionClass.Id == _under.Id).ToList(); ;
      }

    }

    #region IModel Members

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
    }

    public void ExtraDelete()
    {
    }

    #endregion

    
    public void AddPermissionClassMemeber(IPermissionClassMember permissionClassMember)
    {
      _PermissionClassMemebers.Add(permissionClassMember);
    }
  
  }
}
