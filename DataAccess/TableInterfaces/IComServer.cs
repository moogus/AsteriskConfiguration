namespace DataAccess.TableInterfaces
{
  public interface IComServer : IDatabaseTable
  {
    string IpAddress { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    string MailServer { get; set; }
    string VoicemailDialNumber { get; set; }
    string AdminAccount { get; set; }
    string ExtensionIpRange { get; set; }
  }
}