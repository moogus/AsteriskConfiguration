using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class DialplanRange : IDialplanRange, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuDialplanRange _under;

    internal DialplanRange(FuDialplanRange under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal DialplanRange(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuDialplanRange();
    }

    #region IDialplanRange Members

    public int Id
    {
      get { return _under.Id; }
    }

    public string DaysOfWeek
    {
      get { return _under.DaysOfWeek; }
      set { _under.DaysOfWeek = value; }
    }

    public string TimeRange
    {
      get { return _under.TimeRange; }
      set { _under.TimeRange = value; }
    }

    public int Priority
    {
      get { return _under.Priority; }
      set { _under.Priority = value; }
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

    #endregion
  }
}