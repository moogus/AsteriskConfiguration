using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class KnownNumber : IKnownNumber, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuKnownNumber _under;

    internal KnownNumber(FuKnownNumber under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal KnownNumber(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuKnownNumber();
    }

    #region Implementation of IKnownNumber

    public int Id { get { return _under.Id; } }
    public string Number { get { return _under.Number; } set { _under.Number = value; } }
    public string Description { get { return _under.Description; } set { _under.Description = value; } }
    public bool IsInternal { get { return _under.IsInternal; } set { _under.IsInternal = value; } }

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