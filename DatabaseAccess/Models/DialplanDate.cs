using System;
using DatabaseAccess.DatabaseTables;
using NHibernate;

namespace DatabaseAccess.Models
{
    internal class DialplanDate : IDialplanDate, IModel
  {
    private readonly FuDialplanDate _under;
    private readonly ISessionWrapper _session;
    private readonly IRepository _repository;

    internal DialplanDate(FuDialplanDate under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal DialplanDate(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuDialplanDate();
    }

    public int Id { get { return _under.Id; } }
    public DateTime StartDate
    {
      get { return _under.StartDate; }
      set { _under.StartDate = value; }
    }
    public DateTime EndDate
    {
      get { return _under.EndDate; }
      set { _under.EndDate = value; }
    }

    public IDialplan Dialplan
    {
      get
      {
        var dialplan = _repository.GetFromId<IDialplan>(_under.FuDialplanId);
        return dialplan;
      }
      set
      {
        var dialplan = value;
        _under.FuDialplanId = dialplan == null ? 0 : dialplan.Id;
      }
    }

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
  }
}
