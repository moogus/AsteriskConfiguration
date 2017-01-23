using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuContactDetails :  IFuContactDetails
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuExtensionNumber; } }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Department { get; set; }
    public virtual string Email { get; set; }
    public virtual string JobTitle { get; set; }
    public virtual string FuExtensionNumber { get; set; }
    public virtual string Notes { get; set; }
  }
}