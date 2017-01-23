using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class AstVoicemail :  IAstVoicemail
  {
    public AstVoicemail()
    {
      Context = "";
      Mailbox = "";
      Password = "";
      Attach = "yes";     
    }

    public virtual int Id { get; set; }
    public virtual string Name { get { return Mailbox; } }
    public virtual string Context { get; set; }
    public virtual string Mailbox { get; set; }
    public virtual string Password { get; set; }
    public virtual string FullName { get; set; }
    public virtual string Email { get; set; }
    public virtual int MaxSecs { get; set; }
    public virtual int MinSecs { get; set; }
    public virtual int MaxMessages { get; set; }
    public virtual int HeldMaxMessages { get; set; }
    public virtual string Attach { get; set; }
  }
}
