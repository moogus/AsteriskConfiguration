using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class Dialplan : IDialplan
  {
    private readonly IFuDialplan _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public Dialplan(IFuDialplan fuDialP, IRepositoryWithDelete modelRepository)
    {
      _under = fuDialP;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public string Name
    {
      get { return _under.FuDialPlanName; }
      set { _under.FuDialPlanName = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }

    public bool Equals(IDialplan other)
    {
      if (other == null)
        return false;
      return Id == other.Id;
    }
  }
}