using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class UserConfig : IUserConfig
  {
    private readonly IFuUserConfig _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public UserConfig(IFuUserConfig userConfig, IRepositoryWithDelete modelRepository)
    {
      _under = userConfig;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Number
    {
      get { return _under.ExtensionNumber; }
      set { _under.ExtensionNumber = value; }
    }

    public string Password
    {
      get { return _under.Password; }
      set { _under.Password = value; }
    }

    public string Role
    {
      get { return _under.Role; }
      set { _under.Role = value; }
    }

    public IExtension UserExtension
    {
      get { return _modelRepository.GetFromName<IExtension>(_under.ExtensionNumber); }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}