using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class DialplanRange : IDialplanRange
  {
    private readonly IFuDialplanRange _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public DialplanRange(IFuDialplanRange fuDpRange, IRepositoryWithDelete modelRepository)
    {
      _under = fuDpRange;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
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
        var dialplan = _modelRepository.GetFromId<IDialplan>(_under.FuDialplanId);
        return dialplan;
      }
      set
      {
        var dialplan = value;
        _under.FuDialplanId = dialplan == null ? 0 : dialplan.Id;
      }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}