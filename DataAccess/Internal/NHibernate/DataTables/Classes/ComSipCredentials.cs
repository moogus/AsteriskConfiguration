using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComSipCredentials :  IComSipCredentials
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return UserName; } }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public virtual string Host { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int AllowedChannels { get; set; }
  }
}
