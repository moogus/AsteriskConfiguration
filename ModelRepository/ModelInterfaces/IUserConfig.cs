namespace ModelRepository.ModelInterfaces
{
  public interface IUserConfig : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Password { get; set; }
    string Role { get; set; }
    IExtension UserExtension { get; }
  }
}