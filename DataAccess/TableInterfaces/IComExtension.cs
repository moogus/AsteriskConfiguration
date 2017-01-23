namespace DataAccess.TableInterfaces
{
  public interface IComExtension : IDatabaseTable
  {
    string Password { get; set; }
    string Notes { get; set; }
    string Number { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Department { get; set; }
    string Email { get; set; }
    string JobTitle { get; set; }
    string DDINumber { get; set; }
    int VoiceMailId { get; set; }
    int VoiceMailDelay { get; set; }
    int CLIId { get; set; }
    bool DND { get; set; }
    int PermissionClassId { get; set; }
    bool IncludeInDirectory { get; set; }
  }
}
