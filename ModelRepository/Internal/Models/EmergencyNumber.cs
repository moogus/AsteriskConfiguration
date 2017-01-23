using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class EmergencyNumber : IEmergencyNumber
  {
    private readonly IFuEmergencyNumber _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public EmergencyNumber(IFuEmergencyNumber fuEmerg, IRepositoryWithDelete modelRepository)
    {
      _under = fuEmerg;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public string Number
    {
      get { return _under.Number; }
      set { _under.Number = value; }
    }

    public string Description
    {
      get { return _under.Description; }
      set { _under.Description = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}