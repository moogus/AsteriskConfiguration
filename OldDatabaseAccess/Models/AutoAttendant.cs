using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  class AutoAttendant : IAutoAttendant
  {

    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuAutoAttendant _under;
    private readonly List<IAutoAttendantRules> _rules;

    public AutoAttendant(FuAutoAttendant under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;

      var rule = _repository.GetList<IAutoAttendantRules>().Where(r => r.AaName == _under.Name).ToList();
      _rules = rule.Count == 0 ? new List<IAutoAttendantRules>() : rule;
    }

    public AutoAttendant(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuAutoAttendant();

      var rule = _repository.GetList<IAutoAttendantRules>().Where(r => r.AaName == _under.Name).ToList();
      _rules = rule.Count == 0 ? new List<IAutoAttendantRules>() : rule;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Name
    {
      get { return _under.Name; }
      set { _under.Name = value; }
    }

    public string Announcement
    {
      get { return _under.Announcement; }
      set { _under.Announcement = value; }
    }

    public int Timeout
    {
      get { return _under.Timeout; }
      set { _under.Timeout = value; }
    }

    public IEnumerable<IAutoAttendantRules> Rules
    {
      get { return _rules; }
    }

    #region IAutoAttendant Members

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
