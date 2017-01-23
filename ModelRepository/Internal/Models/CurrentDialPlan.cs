using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class CurrentDialPlan : ICurrentDialPlan
  {
    private readonly IFuCurrentDialplan _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private IDialplan _dialplan;

    public CurrentDialPlan(IFuCurrentDialplan fuCurrentDialplan, IRepositoryWithDelete modelRepository)
    {
      _under = fuCurrentDialplan;
      _modelRepository = modelRepository;
      _dialplan = _modelRepository.GetFromId<IDialplan>(_under.CurrentDialplan);
    }

    //this model does not have a get by name

    public int Id
    {
      get { return _under.Id; }
    }

    public IDialplan Dialplan
    {
      get { return _dialplan; }
      set
      {
        _dialplan = value;
        _under.CurrentDialplan = value.Id;
      }
    }

    public bool AutomaticallyChange
    {
      get { return _under.AutomaticallyChange; }
      set { _under.AutomaticallyChange = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}