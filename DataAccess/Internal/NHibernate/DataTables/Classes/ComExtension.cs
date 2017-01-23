using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComExtension :IComExtension
  {
    public virtual int Id { get; set; }
    public virtual string Name{get { return Number ?? ""; }}
    public virtual string Password { get; set; }
    public virtual string Notes { get; set; }
    public virtual string Number { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Department { get; set; }
    public virtual string Email { get; set; }
    public virtual string JobTitle { get; set; }
    public virtual string DDINumber { get; set; }
    public virtual int VoiceMailId { get; set; }
    public virtual int VoiceMailDelay { get; set; }
    public virtual int CLIId { get; set; }
    public virtual bool DND { get; set; }
    public virtual int PermissionClassId { get; set; }
    public virtual bool IncludeInDirectory { get; set; }
  }
}
