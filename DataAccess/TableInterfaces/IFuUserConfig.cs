namespace DataAccess.TableInterfaces
{
  public interface IFuUserConfig : IDatabaseTable
  {
    string Password { get; set; }
    string ExtensionNumber { get; set; }
    string Role { get; set; }
  }
}