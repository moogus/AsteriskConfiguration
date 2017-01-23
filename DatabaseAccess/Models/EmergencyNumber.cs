using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class EmergencyNumber : IEmergencyNumber, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuEmergencyNumber _under;

    internal EmergencyNumber(FuEmergencyNumber under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal EmergencyNumber(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuEmergencyNumber();
    }

    public int Id { get { return _under.Id; } }
    public string Number { get { return _under.Number; } set { _under.Number = value; } }
    public string Description { get { return _under.Description; } set { _under.Description = value; } }

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
