using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuIaxCredentials :  IFuIaxCredentials
  {
      public FuIaxCredentials()
      {
          FuIaxCredentialName = string.Empty;
      }
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuIaxCredentialName; } }
    public virtual string FuIaxCredentialName { get; set; }
    public virtual string Host { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int AllowedChannels{get; set; }
  }
}