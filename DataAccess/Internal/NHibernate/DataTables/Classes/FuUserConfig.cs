using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuUserConfig :  IFuUserConfig
  {
    public FuUserConfig()
    {
      Role = "user";
    }

    public virtual int Id { get; set; }
    public virtual string Name { get { return ExtensionNumber?? ""; } }
    public virtual string Password { get; set; }
    public virtual string ExtensionNumber { get; set; }
    public virtual string Role { get; set; }
  }
}