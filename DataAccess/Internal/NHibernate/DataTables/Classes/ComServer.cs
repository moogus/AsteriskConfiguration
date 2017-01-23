using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComServer :  IComServer
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return IpAddress; } }
    public virtual string IpAddress { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public virtual string MailServer { get; set; }
    public virtual string VoicemailDialNumber { get; set; }
    public virtual string AdminAccount { get; set; }
    public virtual string ExtensionIpRange { get; set; }
  }
}