using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class Fu4ComFederation : IFu4ComFederation
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuFederationName; } }
    public virtual string FuFederationName { get; set; }
    public virtual string Description { get; set; }
    public virtual int ComTrunkId { get; set; }
  }
}