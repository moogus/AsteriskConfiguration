using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class KnownNumber : IKnownNumber
  {
    private readonly IFuKnownNumber _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public KnownNumber(IFuKnownNumber knownNumber, IRepositoryWithDelete modelRepository)
    {
      _under = knownNumber;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

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

    public bool IsInternal
    {
      get { return _under.IsInternal; }
      set { _under.IsInternal = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}