namespace ModelRepository.ModelInterfaces
{
  public interface IExtension : IModel
  {
    string IpAddress { get; }
    string Model { get; }
    int Id { get; }
    string Status { get; }
    string Password { get; set; }
    string Notes { get; set; }
    string Number { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Department { get; set; }
    string Email { get; set; }
    string JobTitle { get; set; }
    IDDI DDI { get; set; }
    ICLI CLI { get; set; }
    bool DND { get; set; }
    IVoiceMail VoiceMail { get; set; }
    int VoicemailDelay { get; set; }
    IPermisionClass PermisionClass { get; set; }
    bool IncludeInDirectory { get; set; }
  }
}