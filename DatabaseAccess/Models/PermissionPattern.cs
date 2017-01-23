using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class PermissionPattern : IPattern, IModel
  {
    private readonly ISessionWrapper _session;
    private IRepository _repository;
    private readonly FuPermissionPattern _under;
    

    internal PermissionPattern(FuPermissionPattern under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal PermissionPattern(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuPermissionPattern();
    }

    #region Implementation of IPattern

    public int Id { get { return _under.Id; } }
    public string Name { get { return _under.Name; } set { _under.Name = value; } }
    public string Pattern { get { return _under.Pattern; } set { _under.Pattern = value; } }

    #endregion

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


  }
}
