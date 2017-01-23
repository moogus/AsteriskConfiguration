namespace DatabaseAccess.DatabaseTables
{
  internal class FuIaxCredentials : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Host { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int AllowedChannels{get; set; }
  }
}