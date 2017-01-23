namespace DatabaseAccess.DatabaseTables
{
    public class AstQueue : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string MusicOnHold { get; set; }
        public virtual string Strategy { get; set; }
        public virtual string JoinEmpty { get; set; }
        public virtual string LeaveWhenEmpty { get; set; }
        public virtual bool RingInUse { get; set; }
    }
}