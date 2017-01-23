using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class CLI : ICLI
  {
    private readonly IComCLI _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public CLI(IComCLI comCLI, IRepositoryWithDelete modelRepository)
    {
      _under = comCLI;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public string CLINumber
    {
      get { return _under.CLINumber; }
      set { _under.CLINumber = value; }
    }

    public string CLIName
    {
      get { return _under.CLIName; }
      set { _under.CLIName = value; }
    }

    public ITrunk Trunk
    {
      get { return _modelRepository.GetFromId<ITrunk>(_under.TrunkId); }
      set { _under.TrunkId = value.Id; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}