namespace DatabaseAccess.DatabaseTables
{
  internal class ComAccessCode : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Code { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int Priority { get; set; }
  }
}