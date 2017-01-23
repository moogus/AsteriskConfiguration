using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class DDI : IDDI, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly FuDDI _under;

    internal DDI(FuDDI under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal DDI(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new FuDDI { UsedOn = "NotUsed" };
    }

    #region Implementation of IModel


    #endregion

    #region Implementation of IDDI

    private string _used = string.Empty;
    public DDIUsedOn UsedOn
    {
      get
      {
        if (string.IsNullOrEmpty(_under.UsedOn))
        {
          return DDIUsedOn.NotUsed;
        }

        return (DDIUsedOn)Enum.Parse(typeof(DDIUsedOn), _under.UsedOn);

      }
      set { _under.UsedOn = value.ToString(); }
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string DDINumber
    {
      get { return _under.DDI; }
      set { _under.DDI = value; }
    }

    public ITrunk Trunk
    {
      get
      {
        return _under.TrunkId > 0 ? _repository.GetFromId<ITrunk>(_under.TrunkId) : _repository.Add<ITrunk>();
      }
      set
      {
        if (_used == null && !string.IsNullOrEmpty(value.DefaultDestination))
        {
          _used = "Default: " + value.DefaultDestination;
        }
        _under.TrunkId = value.Id;
      }
    }

    public string Used
    {
      get
      {
        UpdateDDIUsedOn();
        return _used;
      }
    }

    #endregion

    #region IDDI Members

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

    private void UpdateDDIUsedOn()
    {
      switch (UsedOn)
      {
        case DDIUsedOn.NotUsed:
          _used = string.Empty;
          GetUseOfDDI();
          break;

        case DDIUsedOn.Extension:
          IExtension extension =
            _repository.GetList<IExtension>().FirstOrDefault(e => e.DDI != null && e.DDI.DDINumber == DDINumber);

          if (extension != null)
          {
            _used = "Extension: " + extension.Number;
          }
          break;

        case DDIUsedOn.Queue:
          IQueue queue =
           _repository.GetList<IQueue>().FirstOrDefault(e => e.DDINumber == DDINumber);

          if (queue != null)
          {
            _used = "Queue: " + queue.Number;
          }
          break;

        case DDIUsedOn.Rule:
          IRoutingRule route =
          _repository.GetList<IRoutingRule>().FirstOrDefault(e => e.Number == DDINumber);

          if (route != null)
          {
            _used = "Route: " + route.Number;
          }
          break;

        case DDIUsedOn.Default:
          _used = "Default: " + Trunk.DefaultDestination;
          break;
      }

    }

    private void GetUseOfDDI()
    {
      IExtension extension =
        _repository.GetList<IExtension>().FirstOrDefault(e => e.DDI != null && e.DDI.DDINumber == DDINumber);
      if (extension != null)
      {
        _used = "Extension: " + extension.Number;
        UsedOn = DDIUsedOn.Extension;
      }
      IQueue queue =
        _repository.GetList<IQueue>().FirstOrDefault(e => e.DDINumber == DDINumber);
      if (queue != null)
      {
        _used = "Queue: " + queue.Number;
        UsedOn = DDIUsedOn.Queue;
      }
      IRoutingRule route =
        _repository.GetList<IRoutingRule>().FirstOrDefault(e => e.Number == DDINumber && e.Dialplan.Id != 12);
      if (route != null)
      {
        _used = "Route: " + route.Number;
        UsedOn = DDIUsedOn.Rule;
      }

      if (_used.EndsWith(DDINumber) && !string.IsNullOrEmpty(Trunk.DefaultDestination) ||
          (string.IsNullOrEmpty(_used) && !string.IsNullOrEmpty(Trunk.DefaultDestination)))
      {
        _used = "Default: " + Trunk.DefaultDestination;
        UsedOn = DDIUsedOn.Default;
      }

      this.Update();
    }
  }
}