namespace DataAccess.TableInterfaces
{
  public interface IAstVoicemail : IDatabaseTable
  {
    string Context { get; set; }
    string Mailbox { get; set; }
    string Password { get; set; }
    string FullName { get; set; }
    string Email { get; set; }
    int MaxSecs { get; set; }
    int MinSecs { get; set; }
    int MaxMessages { get; set; }
    int HeldMaxMessages { get; set; }
    string Attach { get; set; }
  }
}