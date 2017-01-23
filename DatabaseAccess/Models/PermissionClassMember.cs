using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class PermissionClassMember : IPermissionClassMember, IModel
  {
    private readonly FuPermisionClassMember _under;
    private readonly ISessionWrapper _session;
    private readonly IRepository _repository;

    internal PermissionClassMember(FuPermisionClassMember under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal PermissionClassMember(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuPermisionClassMember();
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

    public int Id { get { return _under.Id; } }
    public IPermisionClass ParentPermissionClass
    {
      get
      {
        return _repository.GetFromId<IPermisionClass>(_under.PermissionClassId);
      }
      set
      {
        _under.PermissionClassId = value.Id;
      }
    }
    public IPattern Pattern
    {
      get
      {
        return _repository.GetFromId<IPattern>(_under.PermissionPatternId);
      }
      set
      {
        _under.PermissionPatternId = value.Id;
      }
    }

    public IDialplan Dialplan
    {
      get
      {
        return _repository.GetFromId<IDialplan>(_under.DialplanId);
      }
      set
      {
        _under.DialplanId = value.Id;
      }
    }
  }
}
