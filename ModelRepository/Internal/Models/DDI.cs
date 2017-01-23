using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class DDI : IDDI
  {
    private readonly IFuDDI _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private string _used = string.Empty;

    public DDI(IFuDDI underFuddi, IRepositoryWithDelete modelRepository)
    {
      _under = underFuddi;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public string DDINumber
    {
      get { return _under.DDI; }
      set { _under.DDI = value; }
    }

    public ITrunk Trunk
    {
      get { return _modelRepository.GetFromId<ITrunk>(_under.TrunkId); }
      set { _under.TrunkId = value.Id; }
    }

    public DDIUsedOn UsedOn
    {
      get { return GetDDIUsedOn(); }

      set { _under.UsedOn = value.ToString(); }
    }

    public string Used
    {
      get { return _used; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }

    private DDIUsedOn GetDDIUsedOn()
    {
      var usedOnExtn = _modelRepository.GetList<IExtension>().Any(e => e.DDI!=null && e.DDI.Equals(this));
      if (usedOnExtn)
      {
        _used = string.Format("Extension: {0}", _modelRepository.GetList<IExtension>().First(e =>e.DDI!=null&& e.DDI.Equals(this)).Number);
        return DDIUsedOn.Extension;
      }

      var usedOnQueue = _modelRepository.GetList<IQueue>().Any(q => q.DDI!=null && q.DDI == this);
      if (usedOnQueue)
      {
        _used = string.Format("Queue: {0}", _modelRepository.GetList<IQueue>().First(q => q.DDI != null && q.DDI.Equals(this)).Number);
        return DDIUsedOn.Queue;
      }

      var usedOnRule = _modelRepository.GetList<IRoutingRule>().Any(r => r.Number != null && r.Number.Equals(DDINumber));
      if (usedOnRule)
      {
        _used = string.Format("Route: {0}",
                              _modelRepository.GetList<IRoutingRule>().First(r => r.Number != null &&  r.Number.Equals(DDINumber)).Number);
        return DDIUsedOn.Rule;
      }

      var usedOnDefault =
        _modelRepository.GetList<ITrunk>().Any(t => t.DDIs.Contains(this) && !string.IsNullOrEmpty(t.DefaultDestination));
      if (usedOnDefault)
      {
        _used = string.Format("Default: {0}",
                              _modelRepository.GetList<ITrunk>().First(t => t.DDIs.Contains(this)).DefaultDestination);
        return DDIUsedOn.Default;
      }

      _used = string.Empty;
      return DDIUsedOn.NotUsed;
    }

    public bool Equals(IDDI other)
    {
      if (other == null)
        return false;
      return Id == other.Id;
    }
  }
}