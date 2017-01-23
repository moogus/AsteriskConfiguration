namespace DatabaseAccess.DatabaseTables
{
    public class AstQueueMember : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string MemberName { get; set; }
        public virtual string QueueName { get; set; }
        public virtual string Interface { get; set; }
        public virtual int Penalty { get; set; }
        public virtual int Paused { get; set; }
    }
}