namespace DataAccess.TableInterfaces
{
  public interface IFuContactDetails : IDatabaseTable
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    string Department { get; set; }
    string Email { get; set; }
    string JobTitle { get; set; }
    string FuExtensionNumber { get; set; }
    string Notes { get; set; }
  }
}