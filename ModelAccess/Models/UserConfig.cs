using DatabaseAccess;

namespace ModelAccess.Models
{
  public interface IUserConfig : IModel
  {
    int Id { get; set; }
    string Number { get; set; }
    string Password { get; set; }
    string Role { get; set; }
    IExtension UserExtension { get; }
  }

  public class UserConfig : IUserConfig
  {
    public IUnderlyingUserConfig Under { get; private set; }

    public UserConfig(IUnderlyingUserConfig under)
    {
      Under = under;
      var extRepo = new Repository<IExtension>();
      UserExtension = extRepo.GetFromName(Under.ExtensionNumber);
    }

    public bool Update()
    {
      return Under.Update();
    }

    public bool Delete()
    {
      return Under.Delete();
    }

    public int Id { get { return Under.Id; } set { Under.Id = value; } }

    public string Number { get { return Under.ExtensionNumber; } set { Under.ExtensionNumber = value; } }

    public string Password { get { return Under.Password; } set { Under.Password = value; } }

    public string Role { get { return Under.Role; } set { Under.Role = value; } }

    public IExtension UserExtension
    {
      get; private set; }
  }
 
}