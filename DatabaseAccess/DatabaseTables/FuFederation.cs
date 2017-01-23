namespace DatabaseAccess.DatabaseTables
{
  internal class FuFederation : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Type { get; set; }
    public virtual string Description { get; set; }
    public virtual int ComTrunkId { get; set; }
    public virtual int ComExtensionId { get; set; }
    public virtual int ComRoutingRuleId { get; set; }
  }
}