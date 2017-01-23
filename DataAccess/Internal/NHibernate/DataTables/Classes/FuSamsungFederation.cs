using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class FuSamsungFederation : IFuSamsungFederation
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuFederationName; } }
    public virtual string FuFederationName { get; set; }
    public virtual string Description { get; set; }
    public virtual int ComTrunkId { get; set; }
    public virtual int ComExtensionId { get; set; }
    public virtual int ComRoutingRuleId { get; set; }
  }
}